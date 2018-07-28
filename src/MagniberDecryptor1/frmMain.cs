using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace MagniberDecryptor
{
    public partial class frmMain : Form
    {
        public const string DefaultIV = "EP866p5M93wDS513";
        public const string DefaultKey = "S25943n9Gt099y4K";
        public const int ChunkSize = 128;

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DecryptFile(textBox1.Text);
            MessageBox.Show("Decrypt Accomplished!");
        }

        private void DecryptFile(string filename)
        {
            string outfile = filename.Replace(".ovvmxybv", "");
            List<byte[]> decrypt_list = new List<byte[]>();

            //AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            Aes aes = new AesManaged()
            {
                Mode = CipherMode.CBC,
                BlockSize = 128,
                Padding = PaddingMode.Zeros
            };
            //AesCryptoServiceProvider.Create(
            var decoder = aes.CreateDecryptor(Encoding.ASCII.GetBytes(DefaultKey), Encoding.ASCII.GetBytes(DefaultIV));

            using (FileStream file = new FileStream(filename, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(file))
            {
                bool flag = true;
                while (flag)
                {
                    byte[] src = reader.ReadBytes(ChunkSize);
                    if (src.Length > 0)
                    {
                        byte[] result = DecryptBlob(src, decoder);
                        decrypt_list.Add(result);
                    }
                    else
                        flag = false;
                }
            }

            using (FileStream file = new FileStream(outfile, FileMode.CreateNew))
            using (BinaryWriter writer = new BinaryWriter(file))
            {
                foreach (var buffer in decrypt_list)
                {
                    writer.Write(buffer);
                }
            }
        }

        private byte[] DecryptBlob(byte[] src, ICryptoTransform decryptor)
        {
            byte[] result = decryptor.TransformFinalBlock(src, 0, src.Length);
            return result;
        }

        //private byte[] DecryptBlob(byte[] src, ICryptoTransform decryptor)
        //{
        //    byte[] result = new byte[src.Length];
        //    int loop = ChunkSize / decryptor.InputBlockSize;

        //    int src_offset = 0;
        //    int dest_offset = 0;
        //    int size = decryptor.InputBlockSize;

        //    for (int i = 0; i < loop; i++)
        //    {
        //        dest_offset = src_offset = decryptor.InputBlockSize * i;

        //        decryptor.TransformBlock(src, src_offset, size, result, dest_offset);
        //    }

        //    return result;
        //}

        private void InitUI()
        {
            textBox1.Text = @"F:\4-鴻志\P1020430.JPG.ovvmxybv";
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            InitUI();
        }
    }
}
