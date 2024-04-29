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
using Microsoft.Win32;
using Console;
using System.IO;
//using System.CodeDom;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filenameCoding = "";
        private string filenameDecoding = "";
        private string pathDecoding = "";
        private Coder coder;
        private Console.Decoder decoder;
        private bool IsCoding = true;

        public MainWindow()
        {
            coder = new Coder();
            decoder = new Console.Decoder();
            InitializeComponent();
        }

        private void StartCoding(object sender, RoutedEventArgs e)
        {

            //StatusLabelCoding.Content = "Происходит процесс кодирования...";
            //progressBar.Value = 50;
            //new Thread(UpdateProgressBar).Start();
            if (filenameCoding == "")
            {
                MessageBox.Show("Файл не выбран!");
            }
            else
            {
                long begin = DateTime.Now.Ticks;
                coder.Coding(filenameCoding);
                long end = DateTime.Now.Ticks;
                TimeSpan totalTimeSpan = new TimeSpan(end - begin);
                timeCoding.Text = totalTimeSpan.ToString();
                IsCoding = false;
                compressionRatio.Text = Convert.ToString(String.Format("{0:0.##}", ((new FileInfo(filenameCoding).Length - new FileInfo(filenameCoding.Replace(filenameCoding.Substring(filenameCoding.LastIndexOf('.')), ".vld")).Length) / (double)new FileInfo(filenameCoding).Length) * 100)) + "%";
            }
            
            //UpdateProgressBar();
        }

        //private void UpdateProgressBar()
        //{
        //    while (IsCoding)
        //    {
        //        progressBar.Value = coder.ProgressValue;
        //    }
        //    IsCoding = true;
        //}

        private void StartDecoding(object sender, RoutedEventArgs e)
        {
            if (filenameDecoding == "")
            {
                MessageBox.Show("Файл не выбран!");
            }
            else
            {
                if (pathDecoding == "")
                {
                    MessageBox.Show("Путь не выбран!");
                }
                else
                {
                    long begin = DateTime.Now.Ticks;
                    decoder.Decoding(filenameDecoding, pathDecoding + "\\" + nameOutputFile + ".txt");
                    long end = DateTime.Now.Ticks;
                    TimeSpan totalTimeSpan = new TimeSpan(end - begin);
                    timeDecoding.Text = totalTimeSpan.ToString();
                }
            }
        }

        private void ChooseFileClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Текстовый файл (*.txt)|*.txt";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                filenameCoding = dlg.FileName;
                ChosenFile.Text = filenameCoding;
            }
        }

        private void FinishMessage()
        {
            StatusLabelCoding.Content = "Файл закодирован";
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ChooseFileClick2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "ArhiVLAD (*.vld)|*.vld";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                filenameDecoding = dlg.FileName;
                ChosenFile2.Text = filenameDecoding;
                nameOutputFile.Text = dlg.SafeFileName.Replace(dlg.SafeFileName.Substring(dlg.SafeFileName.LastIndexOf('.')), "_DECODE");
            }


        }

        private void ChoosePath(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            dialog.Multiselect = false;
            dialog.Title = "Выберете папку...";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                pathDecoding = dialog.FolderName;
                ChosenPath2.Text = pathDecoding;
            }
        }
    }
}