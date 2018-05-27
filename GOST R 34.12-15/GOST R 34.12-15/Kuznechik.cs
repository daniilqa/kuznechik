using System.Numerics;
using static GOST_R_34._12_15.Data;

namespace GOST_R_34._12_15
{
    class Kuznechik
    {
        //генерация ключей для зашифрования
        public void generateEncryptionRoundKeys(byte[] masterKey, ref Vector<byte>[] roundKeys)
        {
            Vector<byte> left = new Vector<byte>(), right = new Vector<byte>();

            roundKeys[0] = new Vector<byte>(masterKey, 0);
            roundKeys[1] = new Vector<byte>(masterKey, 16);
            for (int i = 2; i < roundKeys.Length; i++)
            {
                roundKeys[i] = new Vector<byte>();
            }

            for (int i = 1; i <= 4; i++)
            {
                left = roundKeys[i * 2 - 2];
                right = roundKeys[i * 2 - 1];
                for (int j = 1; j <= 8; j++)
                {
                    functionF(8 * (i - 1) + j, ref left, ref right);
                }
                roundKeys[i * 2] = left;
                roundKeys[i * 2 + 1] = right;
            }
            byte[] temp = new byte[16];
            for(int i = 0; i < roundKeys.Length; i++)
            {
                roundKeys[i].CopyTo(temp, 0);
            }
        }

        //генерация ключей для расшифрования
        public void generateDencryptionRoundKeys(byte[] masterKey, ref Vector<byte>[] roundKeys)
        {
            Vector<byte> temp1 = new Vector<byte>(),
                temp2 = new Vector<byte>();
            generateEncryptionRoundKeys(masterKey, ref roundKeys);
            for(int i = 1; i <= 8; i++)
            {
                temp1 = roundKeys[i];
                functionS(ref temp1, pi);
                functionLS(temp1, ref temp2, precomputedInversedLSTable);
                roundKeys[i] = temp2;
            }
        }

        //итерация сети Фейстеля
        private void functionF(int constantIndex, ref Vector<byte> left, ref Vector<byte> right)
        {
            Vector<byte> temp1 = new Vector<byte>(roundConstants, 16 * (constantIndex - 1)), 
                temp2 = Vector.Xor(left, temp1);
            functionLS(temp2, ref temp1, precomputedLSTable);
            right = Vector.Xor(right, temp1);
            swapBlocks(ref left, ref right);
        }

        //LS-преобразование
        private void functionLS(Vector<byte> input, ref Vector<byte> output, byte[] LStable)
        {
            output = new Vector<byte>();
            Vector<byte> temp;
            for(int i = 0; i < 16; i++)
            {
                temp = new Vector<byte>(LStable, i * 16 * 256 + input[i] * 16);                
                output = Vector.Xor(temp, output);
            }
        }

        //S-преобразование
        private void functionS(ref Vector<byte> input, byte[] pi)
        {
            byte[] inputBytes = new byte[16];
            input.CopyTo(inputBytes, 0);
            for(int i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] = pi[inputBytes[i]];
            }
            input = new Vector<byte>(inputBytes);
        }

        //замена блоков местами
        private void swapBlocks(ref Vector<byte> left, ref Vector<byte> right)
        {
            Vector<byte> temp = right;
            right = left;
            left = temp;
        }

        //зашифрование
        public void encrypt(ref Vector<byte> data, Vector<byte>[] roundKeys)
        {
            Vector<byte> temp = new Vector<byte>();
            for (int i = 0; i < 9; i++)
            {
                temp = Vector.Xor(data, roundKeys[i]);
                functionLS(temp, ref data, precomputedLSTable);
            }
            data = Vector.Xor(data, roundKeys[9]);
        }

        //расшифрование
        public void decrypt(ref Vector<byte> data, Vector<byte>[] roundKeys)
        {
            Vector<byte> temp1 = new Vector<byte>(),
                temp2 = new Vector<byte>();
            temp1 = Vector.Xor(data, roundKeys[9]);
            functionS(ref temp1, pi);
            functionLS(temp1, ref temp2, precomputedInversedLSTable);
            
            for(int i = 8; i > 0; i--)
            {
                functionLS(temp2, ref temp1, precomputedInversedLSTable);
                temp2 = Vector.Xor(temp1, roundKeys[i]);
            }
            functionS(ref temp2, inversedPi);
            data = Vector.Xor(temp2, roundKeys[0]);

        }

    }
}
