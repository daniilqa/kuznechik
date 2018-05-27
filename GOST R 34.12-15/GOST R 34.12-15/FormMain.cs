using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Numerics;
using static GOST_R_34._12_15.Data;

namespace GOST_R_34._12_15
{
    public partial class FormMain : Form
    {
        private Stopwatch sw = new Stopwatch();
        private byte[] masterKey = new byte[32];
        private byte[] magicString = { 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10, 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef};
        private byte[] key = new byte[32];
        Vector<byte>[] roundKeys = new Vector<byte>[10];
        private Kuznechik k = new Kuznechik();
        private Kuznechik_standart ks = new Kuznechik_standart();
        private byte[] textBytes;
        private bool loadKey = false, loadFile = false;
        private double speed;
        private double size;

        public FormMain()
        {
            InitializeComponent();
            buttonEncrypt.Enabled = false;
            buttonDecrypt.Enabled = false;
            getPrecomputedLSTable();
            getPrecomputedInversedLSTable();
        }

        //расшифрование файла
        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            k.generateDencryptionRoundKeys(masterKey, ref roundKeys);
            Vector<byte> tmp;
            byte[] block = new byte[16];
            sw.Restart();
            for (int i = 0; i < textBytes.Length / 16; i++)
            {
                tmp = new Vector<byte>(textBytes, i * 16);
                k.decrypt(ref tmp, roundKeys);
                tmp.CopyTo(block, 0);
                Buffer.BlockCopy(block, 0, textBytes, i * 16, 16);
            }
            sw.Stop();
            writeInFile();
            richTextBoxOutput.Text += "Файл " + openFileDialog1.FileName + " успешно расшифрован\r\n";
            fileContents("Содержимое файла: ", textBytes, 10);
            size = textBytes.Length / 1024.0 / 1024.0;
            speed = size / (sw.ElapsedMilliseconds / 1000.0);
            richTextBoxOutput.Text += "Скорость шифрования: " + size.ToString("0.###") + " мб / " +
                (sw.ElapsedMilliseconds / 1000.0) + " сек = " + speed.ToString("0.###") + " мб/с\r\n\r\n";
        }

        //запись в файл
        private void writeInFile()
        {
            string text = Encoding.GetEncoding(1251).GetString(textBytes);
            File.WriteAllText(openFileDialog1.FileName, text, Encoding.GetEncoding(1251));
        }

        //открытие диалогового окна с тестом
        private void выполнитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTest ft = new FormTest();
            ft.Owner = this;
            ft.ShowDialog();
        }

        //генерация ключа
        private void buttonGenerateKey_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            byte[] randomKey = new byte[32];
            for(int i = 0; i < randomKey.Length; i++)
            {
                randomKey[i] = (byte)(r.Next(byte.MinValue, byte.MaxValue) ^ magicString[i]);
                randomKey[i] ^= magicString[i];
            }
            if (writeInFile(randomKey))
            {
                richTextBoxOutput.Text += "Ключ успешно сгенерирован в файл " + saveFileDialog1.FileName + "\r\n";
                fileContents("Зашифрованный мастер-ключ: ", randomKey, randomKey.Length);
                richTextBoxOutput.Text += "\r\n";
            }
        }

        //загрузка ключа из файла
        private void buttonLoadKey_Click(object sender, EventArgs e)
        {
            byte[] encryptedKey = new byte[32];
            if (readFromFile(ref encryptedKey, ref openFileDialog2, ref loadKey))
            {
                try
                {
                    for (int i = 0; i < masterKey.Length; i++)
                    {
                        masterKey[i] = (byte)(encryptedKey[i] ^ magicString[i]);
                    }
                }
                catch (Exception ex)
                {
                    loadKey = false;
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    loadKey = true;
                    richTextBoxOutput.Text += "Ключ из файла " + openFileDialog2.FileName + " успешно загружен\r\n";
                    fileContents("Расшифрованный мастер-ключ: ", masterKey, masterKey.Length);
                    richTextBoxOutput.Text += "\r\n";
                }
                checkFiles();
            }
        }

        //чтение содержимого файла
        private bool readFromFile(ref byte[] value, ref OpenFileDialog opf, ref bool check)
        {
            Stream mystr = null;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                if ((mystr = opf.OpenFile()) != null)
                {
                    StreamReader myread = new StreamReader(mystr, Encoding.GetEncoding(1251));
                    try
                    {
                        value = Encoding.GetEncoding(1251).GetBytes(myread.ReadToEnd());
                    }
                    catch (Exception ex)
                    {
                        check = false;
                        MessageBox.Show(ex.Message);
                        return false;
                    }
                    finally
                    {
                        check = true;
                        myread.Close();
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //запись в файл
        private bool writeInFile(byte[] value)
        {
            Stream mystr = null;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((mystr = saveFileDialog1.OpenFile()) != null)
                {
                    StreamWriter mywriter = new StreamWriter(mystr, Encoding.GetEncoding(1251));
                    try
                    {
                        mywriter.Write(Encoding.GetEncoding(1251).GetString(value));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return false;
                    }
                    finally
                    {
                        mywriter.Close();
                    }
                }
                return true;
            }
            return false;
        }

        //очистить лог
        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxOutput.Text = "";
        }

        //зашифровать файл
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            k.generateEncryptionRoundKeys(masterKey, ref roundKeys);

            if (textBytes.Length % 16 != 0)
            {
                textBytes.CopyTo(textBytes = new byte[textBytes.Length + 16 - (textBytes.Length % 16)], 0);
            }

            byte[] block = new byte[16];
            Vector<byte> tmp;

            sw.Restart();
            for (int i = 0; i < textBytes.Length / 16; i++)
            {
                tmp = new Vector<byte>(textBytes, i * 16);
                k.encrypt(ref tmp, roundKeys);
                tmp.CopyTo(block, 0);
                Buffer.BlockCopy(block, 0, textBytes, i * 16, 16);
            }
            sw.Stop();

            writeInFile();
            richTextBoxOutput.Text += "Файл " + openFileDialog1.FileName + " успешно зашифрован\r\n";
            fileContents("Содержимое файла: ", textBytes, 10);
            size = textBytes.Length / 1024.0 / 1024.0;
            speed = size / (sw.ElapsedMilliseconds / 1000.0);
            richTextBoxOutput.Text += "Скорость шифрования: " + size.ToString("0.###") + " мб / " + 
                (sw.ElapsedMilliseconds / 1000.0) + " сек = " + speed.ToString("0.###") + " мб/с\r\n\r\n";
        }

        //открыть файл
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (readFromFile(ref textBytes, ref openFileDialog1, ref loadFile))
            {
                richTextBoxOutput.Text += "Файл " + openFileDialog1.FileName + " успешно открыт\r\n";
                fileContents("Содержимое файла: ", textBytes, 10);
                richTextBoxOutput.Text += "\r\n";
                checkFiles();
            }
        }

        //проверка для разблокировки кнопок
        private void checkFiles()
        {
            if(loadFile & loadKey)
            {
                buttonEncrypt.Enabled = true;
                buttonDecrypt.Enabled = true;
            }
            else
            {
                buttonEncrypt.Enabled = false;
                buttonDecrypt.Enabled = false;
            }
        }

        //отображение содержимого файла
        private void fileContents(string text, byte[] b, int count)
        {
            richTextBoxOutput.Text += text;
            if (count > b.Length)
                count = b.Length;
            for (int i = 0; i < count; i++)
            {
                richTextBoxOutput.Text += b[i].ToString("X") + ((i == count - 1) ? "\r\n" : " ");
            }
        }

    }
}
