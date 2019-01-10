using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rsa_crypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RSACryptoServiceProvider RsaKey = new RSACryptoServiceProvider();
                string publickey = RsaKey.ToXmlString(false);
                string privatekey = RsaKey.ToXmlString(true);
                File.WriteAllText("private.xml", privatekey, Encoding.UTF8);
                File.WriteAllText("public.xml", publickey, Encoding.UTF8);
                MessageBox.Show("Ok!");
                Process.Start("explorer.exe", System.IO.Directory.GetCurrentDirectory());
            }
            catch (Exception ex)
            {
            
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                byte[] DecryptedData;

                string privatexml = "";
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Private Key path";
                op.ShowDialog();
                privatexml = File.ReadAllText(op.FileName, Encoding.UTF8);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                try
                {
                    if (privatexml.Length == 0)
                    {

                        MessageBox.Show("Private key error");
                        return;
                    }
                    else
                    {
                        rsa.FromXmlString(privatexml);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error \n" + ex.Message.ToString());
                }

                try
                {
                    OpenFileDialog op2 = new OpenFileDialog();
                    op2.Filter = "Abs (*.abs)|*.abs";
                    op2.ShowDialog();
                    DecryptedData = rsa.Decrypt(File.ReadAllBytes(op2.FileName), false);
                    string text = Encoding.UTF8.GetString(DecryptedData);
                    key.Text = text;
                    MessageBox.Show("Ok!");

                }
                catch (CryptographicException ex)
                {

                    MessageBox.Show("Error... \n" + ex.Message.ToString());
                    return;
                }

            }
            catch
            {

            }

           
        }

        private void Button_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                byte[] EncryptedData;

                string publicxml = "";
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Public Key path";
                op.ShowDialog();
                publicxml = File.ReadAllText(op.FileName, Encoding.UTF8);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                try
                {
                    if (publicxml.Length == 0)
                    {

                        MessageBox.Show("Public key error");
                        return;
                    }
                    else
                    {
                        rsa.FromXmlString(publicxml);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error \n" + ex.Message.ToString());
                }

                try
                {
                    //OpenFileDialog op2 = new OpenFileDialog();
                    //  op2.ShowDialog();
                    // EncryptedData = rsa.Encrypt(File.ReadAllBytes(op2.FileName), false);
                    EncryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(key.Text), false);
                    SaveFileDialog sd = new SaveFileDialog();
                    sd.Filter = "Abs (*.abs)|*.abs";
                    sd.ShowDialog();
                    string file = sd.FileName;
                    File.WriteAllBytes(file, EncryptedData);
                    MessageBox.Show("Ok!");
                }
                catch (CryptographicException ex)
                {

                    MessageBox.Show("Error... \n" + ex.Message.ToString());
                }
            }
            catch
            {

            }
           
        }
    }
}
