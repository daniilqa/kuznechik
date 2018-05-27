using System;
using System.Text;
using System.Windows.Forms;
using System.Numerics;
using System.Diagnostics;

namespace GOST_R_34._12_15
{
    public partial class FormTest : Form
    {
        private uint initialValue;
        private uint iterationStep;
        private uint numberIterations;
        private byte[] masterKey =  { 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10, 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef};
        public string text;
        private StringBuilder sb;
        private Kuznechik k = new Kuznechik();
        private Kuznechik_standart ks = new Kuznechik_standart();
        private Vector<byte>[] roundKeys = new Vector<byte>[10];
        private Vector<byte> tmp;
        private Stopwatch sw = new Stopwatch();
        private byte[] data;
        private double speed;
        private string temp;
        private byte[] block = new byte[16];
        private FormMain fm;

        public FormTest()
        {
            InitializeComponent();
        }

        //проведение анализа скорости шифрования
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (uint.TryParse(textBoxInitialValue.Text, out initialValue) &&
                uint.TryParse(textBoxIterationStep.Text, out iterationStep) &&
                uint.TryParse(textBoxNumberIterations.Text, out numberIterations) &&
                initialValue <= 100 && iterationStep <= 100 && numberIterations <= 10)
            {
                sb = new StringBuilder();
                ks.setKeys(masterKey);
                data = new byte[(initialValue + iterationStep * numberIterations) * 1024 * 1024];
                encryptStandart();
                encryptModified();
                decryptStandart();
                decryptModified();
                fm = this.Owner as FormMain;
                fm.richTextBoxOutput.Text += sb.ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка, некорректные данные");
            }
        }

        //отмена
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //зашифрование стандартным методом
        private void encryptStandart()
        {
            sb.Append("Засшифрование стандартным методом:\r\n");
            for (uint i = initialValue, count = 0; count < numberIterations; i += iterationStep, count++)
            {
                sw.Restart();
                for (int j = 0; j < (i * 1024 * 1024) / 16; j++)
                {
                    Buffer.BlockCopy(data, j * 16, block, 0, 16);
                    block = ks.encrypt(block);
                }
                sw.Stop();
                speed = i / (sw.ElapsedMilliseconds / 1000.0);
                temp = speed.ToString("0.###");
                sb.Append((count + 1) + ". " + i + " мб / " + (sw.ElapsedMilliseconds / 1000.0) + " сек = " + temp + " мб/с\r\n");
            }
            sb.Append("\r\n");

        }

        //расшифрование стандартным методом
        private void decryptStandart()
        {
            sb.Append("Расшифрование стандартным методом:\r\n");
            for (uint i = initialValue, count = 0; count < numberIterations; i += iterationStep, count++)
            {
                sw.Restart();
                for (int j = 0; j < (i * 1024 * 1024) / 16; j++)
                {
                    Buffer.BlockCopy(data, j * 16, block, 0, 16);
                    block = ks.decrypt(block);
                }
                sw.Stop();
                speed = i / (sw.ElapsedMilliseconds / 1000.0);
                temp = speed.ToString("0.###");
                sb.Append((count + 1) + ". " + i + " мб / " + (sw.ElapsedMilliseconds / 1000.0) + " сек = " + temp + " мб/с\r\n");
            }
            sb.Append("\r\n");
        }

        //зашифрование модифицированным методом
        private void encryptModified()
        {
            sb.Append("Зашифрование модифицированным методом:\r\n");
            k.generateEncryptionRoundKeys(masterKey, ref roundKeys);
            for (uint i = initialValue, count = 0; count < numberIterations; i += iterationStep, count++)
            {
                sw.Restart();
                for (int j = 0; j < (i * 1024 * 1024) / 16; j++)
                {
                    tmp = new Vector<byte>(data, j * 16);
                    k.encrypt(ref tmp, roundKeys);
                }
                sw.Stop();
                speed = i / (sw.ElapsedMilliseconds / 1000.0);
                temp = speed.ToString("0.###");
                sb.Append((count + 1) + ". " + i + " мб / " + (sw.ElapsedMilliseconds / 1000.0) + " сек = " + temp + " мб/с\r\n");
            }
            sb.Append("\r\n");
        }

        //расшифрование модифицированным методом
        private void decryptModified()
        {
            sb.Append("Расшифрование модифицированным методом:\r\n");
            k.generateDencryptionRoundKeys(masterKey, ref roundKeys);
            for (uint i = initialValue, count = 0; count < numberIterations; i += iterationStep, count++)
            {
                sw.Restart();
                for (int j = 0; j < (i * 1024 * 1024) / 16; j++)
                {
                    tmp = new Vector<byte>(data, j * 16);
                    k.decrypt(ref tmp, roundKeys);
                }
                sw.Stop();
                speed = i / (sw.ElapsedMilliseconds / 1000.0);
                temp = speed.ToString("0.###");
                sb.Append((count + 1) + ". " + i + " мб / " + (sw.ElapsedMilliseconds / 1000.0) + " сек = " + temp + " мб/с\r\n");
            }
            sb.Append("\r\n");
        }

    }
}
