using System;
using static GOST_R_34._12_15.Data;

namespace GOST_R_34._12_15
{
    class Kuznechik_standart
    {
        private readonly byte[] factors = {
            0x94, 0x20, 0x85, 0x10, 0xC2, 0xC0, 0x01, 0xFB,
            0x01, 0xC0, 0xC2, 0x10, 0x85, 0x20, 0x94, 0x01
        };
        private byte[][] roundKeys;
        private byte[][] mulTable = initMulTable();

        //зашифрование
        public byte[] encrypt(byte[] data)
        {
            for (int i = 0; i < 9; i++)
            {
                functionX(ref data, roundKeys[i]);
                functionS(ref data, pi);
                functionL(ref data);

            }
            functionX(ref data, roundKeys[9]);
            return data;

        }

        //расшифрование
        public byte[] decrypt(byte[] data)
        {
            functionX(ref data, roundKeys[9]);
            for(int i = 8; i >= 0; i--)
            {
                functionLInv(ref data);
                functionS(ref data, inversedPi);
                functionX(ref data, roundKeys[i]);
            }
            return data;
        }

        //генерация раундовых ключей
        public void setKeys(byte[] key)
        {
            roundKeys = new byte[10][];
            for (int i = 0; i < 10; i++)
            {
                roundKeys[i] = new byte[16];
            }

            byte[] leftPart = new byte[16];
            byte[] rightPart = new byte[16];
            byte[] constVector = new byte[16];

            for (int i = 0; i < 16; i++)
            {
                roundKeys[0][i] = leftPart[i] = key[i];
                roundKeys[1][i] = rightPart[i] = key[i + 16];
            }

            for (int k = 1; k < 5; k++)
            {

                for (int j = 1; j <= 8; j++)
                {
                    getConstVector(ref constVector, 8 * (k - 1) + j);
                    functionF(ref constVector, ref leftPart, ref rightPart);
                }

                leftPart.CopyTo(roundKeys[2 * k], 0);
                rightPart.CopyTo(roundKeys[2 * k + 1], 0);

            }

        }

        //вычисление таблицы умножения
        private static byte[][] initMulTable()
        {
            byte[][] mulTable = new byte[256][];
            for (int i = 0; i < 256; i++)
            {
                mulTable[i] = new byte[256];
                for (int j = 0; j < 256; j++)
                {
                    mulTable[i][j] = multiplication((byte)i, (byte)j);
                }
            }
            return mulTable;
        }

        //умножение в поле Галуа над неприводимым многочленом
        private static byte multiplication(byte i, byte j)
        {
            byte p = 0;
            byte counter;
            byte hi_bit_set;
            for (counter = 0; counter < 8 && i != 0 && j != 0; counter++)
            {
                if ((j & 1) != 0)
                    p ^= i;
                hi_bit_set = (byte)(i & 0x80);
                i <<= 1;
                if (hi_bit_set != 0)
                    i ^= 0xc3; /* x^8 + x^7 + x^6 + x + 1 */
                j >>= 1;
            }
            return p;
        }

        //S-преобразование
        private void functionS(ref byte[] data, byte[] pi)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = pi[data[i]];
            }
        }

        //X-преобразование
        private void functionX(ref byte[] result, byte[] data)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] ^= data[i];
            }
        }

        //L-преобразование
        private void functionL(ref byte[] data)
        {
            for (int i = 0; i < 16; i++)
            {
                byte x = data[15];
                for (int j = 14; j >= 0; j--)
                {
                    x ^= mulTable[data[j]][factors[j]];
                }
                Buffer.BlockCopy(data, 0, data, 1, 15);
                data[0] = x;
            }
        }

        //обратное L-преобразование
        private void functionLInv(ref byte[] data)
        {
            for(int i = 0; i < 16; i++)
            {
                byte x = data[0];
                for(int j = 0; j < 15; j++)
                {
                    x ^= mulTable[data[j]][factors[j]];
                }
                Buffer.BlockCopy(data, 1, data, 0, 15);
                data[15] = x;
            }
        }

        //итерация сети Фейстеля
        private void functionF(ref byte[] constVector, ref byte[] leftPart, ref byte[] rightPart)
        {
            byte[] temp = new byte[16];
            temp.CopyTo(leftPart, 0);
            functionX(ref temp, constVector);
            functionS(ref temp, pi);
            functionL(ref temp);
            functionX(ref temp, rightPart);

            leftPart.CopyTo(rightPart, 0);
            temp.CopyTo(leftPart, 0);

        }

        //генерация константных векторов
        private void getConstVector(ref byte[] constVector, int i)
        {
            Array.Clear(constVector, 0, 16);
            constVector[15] = (byte)i;
            functionL(ref constVector);
        }
    }
}
