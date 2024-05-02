using Console;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filenameCoding;
        private string filenameDecoding;
        private string pathDecoding;
        private Coder coder;
        private Decoder decoder;
        private bool IsCoding = false;

        public MainWindow()
        {
            filenameCoding = "";
            filenameDecoding = "";
            pathDecoding = "";
            coder = new Coder();
            decoder = new Decoder();
            InitializeComponent();
        }

        private void StartCoding(object sender, RoutedEventArgs e)
        {
            if (filenameCoding == "")
            {
                MessageBox.Show("Файл не выбран!");
            }
            else
            {
                IsCoding = true;
                long begin = DateTime.Now.Ticks;
                coder.Coding(filenameCoding);
                long end = DateTime.Now.Ticks;
                TimeSpan totalTimeSpan = new TimeSpan(end - begin);
                timeCoding.Text = totalTimeSpan.ToString();
                IsCoding = false;
                compressionRatio.Text = Convert.ToString(String.Format("{0:0.##}", ((new FileInfo(filenameCoding).Length - new FileInfo(filenameCoding.Replace(filenameCoding.Substring(filenameCoding.LastIndexOf('.')), ".vld")).Length) / (double)new FileInfo(filenameCoding).Length) * 100)) + "%";
            }
        }

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
                    decoder.Decoding(filenameDecoding, pathDecoding + "\\" + nameOutputFile.Text + ".txt");
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