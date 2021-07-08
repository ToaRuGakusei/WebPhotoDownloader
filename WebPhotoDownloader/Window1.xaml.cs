using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WebPhotoDownloader
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            tempFolder();

            saveFolder();
        }

        private void saveFolder()
        {
            //ファイルの保存先を表示
            if (File.Exists("./saveFolder.txt"))
            {
                StreamReader SR = new StreamReader("./saveFolder.txt", Encoding.GetEncoding("Shift_JIS"));
                string temp = SR.ReadToEnd();
                string replace = temp.Replace("\n", "");
                string tempfile = replace.Replace("\r", "");
                save.Text = tempfile;
                SR.Close();
            }
            else
            {
                FileStream fs = File.Create("./saveFolder.txt");
                fs.Close();
            }
        }

        private void tempFolder()
        {
            //一時ファイルの保存先を表示
            if (File.Exists("./tempFolder.txt"))
            {
                StreamReader SR = new StreamReader("./tempFolder.txt", Encoding.GetEncoding("Shift_JIS"));
                string temp = SR.ReadToEnd();
                string replace = temp.Replace("\n", "");
                string tempfile = replace.Replace("\r", "");
                box.Text = tempfile;
                SR.Close();
            }
            else
            {
                FileStream fs = File.Create("./tempFolder.txt");
                fs.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tf = "tempFolder";
            selectFolder(tf);
            tempFolder();

        }

        private void selectFolder(string a)
        {
            var browser = new System.Windows.Forms.FolderBrowserDialog();

            browser.Description = "フォルダを選択してください。";

            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                if (File.Exists("./tempFolder.txt"))
                {

                    Encoding enc = Encoding.GetEncoding("Shift_JIS");



                    //ファイルを開く
                    StreamWriter writer = new StreamWriter($@"./{a}.txt", false, enc);
                    

                    //書き込み
                    writer.WriteLine(browser.SelectedPath);
                    writer.Close();
                }
                else
                {
                    //ファイルの作成
                    FileStream fs = File.Create($"./{a}");
                    fs.Close();
                }




            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string sf = "saveFolder";
            selectFolder(sf);
            saveFolder();
        }
    }
}
