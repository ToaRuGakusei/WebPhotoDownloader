﻿using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


// フォルダー選択ダイアログの名前空間を

namespace WebPhotoDownloader
{

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();

            this.SizeToContent = SizeToContent.WidthAndHeight;

            //初期URL
            //addressBar.Text = "https://google.com";

            cancel.IsEnabled = false;
            



        }
        private async Task InitializeAsync()
        {
            await wv.EnsureCoreWebView2Async(null); // CoreWebView2初期化待ち
            wv.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }



        private int n = 0;
        private int i = 0;
        public string txt;
        public string[] setLength;
        public Encoding enc = Encoding.GetEncoding("Shift_JIS");
        public bool taskCancel = false;
        System.Net.WebClient wc = new System.Net.WebClient();
        System.Net.WebClient wc2 = new System.Net.WebClient();














        private void Button_Click(object sender, RoutedEventArgs e)
        {





            // https://　または、http:// が付いていたときの処理
            if (addressBar.Text.Contains("https://"))
            {
                wv.CoreWebView2.Navigate(addressBar.Text);




            }
            else
            {
                addressBar.Text = "https://" + addressBar.Text;
                wv.CoreWebView2.Navigate(addressBar.Text);
            }

            //Encoding enc = Encoding.GetEncoding("Shift_JIS");

            //ファイルを開く
            //StreamWriter writer = new StreamWriter(@"./accessURL.txt", true, enc);

            //書き込み
            //writer.WriteLine($"{addressBar.Text}");
            //writer.Close();




        }


        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {



        }

        private void addressBar_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void addressBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void addressBar_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void addressBar_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            wv.GoBack();
            addressBar.Text = wv.CoreWebView2.Source;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            wv.GoForward();
            addressBar.Text = wv.CoreWebView2.Source;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            wv.CoreWebView2.Stop();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            wv.Reload();
            addressBar.Text = wv.CoreWebView2.Source;
        }



        private void start_Click(object sender, RoutedEventArgs e)
        {

        }



        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //プログレスバーの値を設定
            prog.Maximum = 100;
            prog.Minimum = 0;

            //キャンセルをできるように変更
            cancel.IsEnabled = true;


            //ListViewをリセット
            ls1.Items.Clear();

            run.IsEnabled = false;
            System.Net.WebClient wc = new System.Net.WebClient();
            //URLからストリームを取得
            System.IO.Stream st = wc.OpenRead(wv.CoreWebView2.Source);

            //エンコード
            Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
            System.IO.StreamReader sr = new System.IO.StreamReader(st, enc);

            string html = sr.ReadToEnd();

            //タイトルの取得
            string title = Regex.Match(html, @"<title>(.*)</title>", RegexOptions.IgnoreCase).Groups[1].Value;
            MessageBox.Show(title);

            if (title.TrimEnd() == "")
            {
                MessageBox.Show("タイトルの取得に失敗しました。");
                //return false;
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//img");

            var browser = new System.Windows.Forms.FolderBrowserDialog();


            browser.Description = "保存先を選択をしてください";


            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                save.Content = browser.SelectedPath;

                n = 0;
                foreach (HtmlAgilityPack.HtmlNode node in nodes)
                {
                    string url = node.GetAttributeValue("src", "");
                    Match match = Regex.Match(url, @"(.*\.(jpg|jpeg|png))", RegexOptions.IgnoreCase);

                    if (String.IsNullOrWhiteSpace(Convert.ToString(match)) == false)
                    {
                        // nullでも、空文字列でも、空白でもない
                        ls1.Items.Add(match.Value);

                        n++;
                    }
                    else
                    {
                        // nullもしくは空文字列もしくは空白である
                    }

                }

                i = 1;

                foreach (string lvi in ls1.Items)
                {


                    if (String.IsNullOrWhiteSpace(Convert.ToString(lvi)) == false)
                    {
                        prog.Equals(0);
                        System.Net.WebClient wc2 = new System.Net.WebClient();



                        if (zip.IsChecked == true && renban.IsChecked != true && select.IsChecked != true)
                        {


                            StreamReader SR = new StreamReader("./tempFolder.txt", Encoding.GetEncoding("Shift_JIS"));
                            string temp = SR.ReadToEnd();
                            string replace = temp.Replace("\n", "");
                            string tempfile = replace.Replace("\r", "");
                            SR.Close();

                            string ex = System.IO.Path.GetExtension(lvi);
                            string photoName = System.IO.Path.GetFileName(lvi);
                            string afterTitle = title.Replace("|", " ");
                            string fullPath = tempfile + "\\" + "\\" + photoName;
                            wc2.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(downloadClient_DownloadFileCompleted);
                            wc2.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(wc2_DownloadProgressChanged);
                            wc2.DownloadFileAsync(new Uri(lvi), fullPath);
                            file.Content = photoName + "をダウンロードしています。";

                            i++;

                        }
                        else if (zip.IsChecked == true && renban.IsChecked == true)
                        {

                            StreamReader SR = new StreamReader("./tempFolder.txt", Encoding.GetEncoding("Shift_JIS"));
                            string temp = SR.ReadToEnd();
                            string replace = temp.Replace("\n", "");
                            string tempfile = replace.Replace("\r", "");
                            SR.Close();



                            string ex = System.IO.Path.GetExtension(lvi);
                            string photoName = System.IO.Path.GetFileName(lvi);
                            string afterTitle = title.Replace("|", " ");
                            string fullPath = tempfile + "\\" + i + ex;
                            wc2.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(downloadClient_DownloadFileCompleted);
                            wc2.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(wc2_DownloadProgressChanged);
                            wc2.DownloadFileAsync(new Uri(lvi), fullPath);

                            file.Content = photoName + "をダウンロードしています。";
                            i++;
                        }





                        if (zip.IsChecked == false && renban.IsChecked != true && select.IsChecked != true)
                        {

                            string ex = System.IO.Path.GetExtension(lvi);
                            string photoName = System.IO.Path.GetFileName(lvi);
                            string fullPath = browser.SelectedPath + "\\" + photoName;
                            wc2.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(downloadClient_DownloadFileCompleted);
                            wc2.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(wc2_DownloadProgressChanged);
                            wc2.DownloadFileAsync(new Uri(lvi), fullPath);

                            file.Content = photoName + "をダウンロードしています。";


                        }
                        else if (zip.IsChecked == false && renban.IsChecked == true)
                        {

                            string ex = System.IO.Path.GetExtension(lvi);
                            string photoName = System.IO.Path.GetFileName(lvi);
                            string fullPath = browser.SelectedPath + "\\" + i + ex;
                            wc2.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(downloadClient_DownloadFileCompleted);
                            wc2.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(wc2_DownloadProgressChanged);
                            wc2.DownloadFileAsync(new Uri(lvi), fullPath);

                            file.Content = photoName + "をダウンロードしています。";

                        }
                    }

                }

            }
            else if(browser.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                cancel.IsEnabled = false;
                run.IsEnabled = true;
                MessageBox.Show("キャンセルしました。");
            }




            i = 0;
        }





        private void wc2_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {

            prog.Value = e.ProgressPercentage;
            pro.Content = e.ProgressPercentage + "%";

        }


        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            if (!e.Uri.Contains("_blank"))
            {
                e.Handled = true; // NewWindowのキャンセル

                wv.CoreWebView2.Navigate(e.Uri); // 既存WebView2で遷移


            }

        }




        private void wv_NavigationStarting_1(object sender, CoreWebView2NavigationStartingEventArgs e)
        {

        }

        private void wv_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            addressBar.Text = wv.CoreWebView2.Source;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

            if (wc2 != null || wc != null)
            {
                wc2.CancelAsync();
                wc.CancelAsync();
                cancel.IsEnabled = false;
                run.IsEnabled = true;
                prog.Value = 0;
                pro.Content = "0" + "%";
                MessageBox.Show("キャンセルされました。");
                wc2.Dispose();
            }




        }

        private void downloadClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            i++;

            if (ls1.Items.Count == i + 1)
            {
                if (zip.IsChecked == true)
                {


                    //URLからストリームを取得
                    System.IO.Stream st = wc.OpenRead(wv.CoreWebView2.Source);

                    //エンコード
                    Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
                    System.IO.StreamReader sr = new System.IO.StreamReader(st, enc);

                    string html = sr.ReadToEnd();

                    //タイトルの取得
                    string title = Regex.Match(html, @"<title>(.*)</title>", RegexOptions.IgnoreCase).Groups[1].Value;
                    string pattern = "[\\~#%&*{}/:<>?|\"-]";
                    string replacement = " ";
                    Regex regEx = new Regex(pattern);
                    string sanitized = Regex.Replace(regEx.Replace(title, replacement), @"\s+", " ");


                    //zip作成
                    StreamReader SR = new StreamReader("./tempFolder.txt", Encoding.GetEncoding("Shift_JIS"));
                    string temp = SR.ReadToEnd();
                    string replace = temp.Replace("\n", "");
                    string tempfile = replace.Replace("\r", "");



                    System.IO.Compression.ZipFile.CreateFromDirectory(tempfile + "\\", save.Content + "\\" + sanitized + ".zip");
                    MessageBox.Show("ダウンロードが終わりました", "終了", MessageBoxButton.OK, MessageBoxImage.Information);


                    //選択をリセット
                    pro.Content = "0" + "%";
                    cancel.IsEnabled = false;
                    zip.IsChecked = false;
                    renban.IsChecked = false;
                    run.IsEnabled = true;
                    i = 0;
                    prog.Value = 0;

                    //一時ファイルを削除
                    string[] filePaths = Directory.GetFiles(tempfile);
                    foreach (string f in filePaths)
                    {
                        File.SetAttributes(f, FileAttributes.Normal);
                        File.Delete(f);
                    }


                }
                else
                {
                    MessageBox.Show("ダウンロードが終わりました", "終了", MessageBoxButton.OK, MessageBoxImage.Information);
                    //MessageBox.Show("終了しました。");
                    pro.Content = "0" + "%";
                    prog.Value = 0;
                    cancel.IsEnabled = false;
                    zip.IsChecked = false;
                    run.IsEnabled = true;
                }



            }
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var win = new Window1();
            win.Show();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            zip.IsChecked = false;
            renban.IsChecked = false;
        }
    }
}
