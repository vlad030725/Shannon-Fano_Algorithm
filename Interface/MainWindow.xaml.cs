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
                TimeSpan totalTimeSpan = TimeSpan.Zero;
                double CompressionRatio = 0;
                coder.Coding(filenameCoding, ref totalTimeSpan, ref CompressionRatio);
                timeCoding.Text = totalTimeSpan.ToString();
                compressionRatio.Text = Convert.ToString(String.Format("{0:0.##}", CompressionRatio)) + "%";
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
                    TimeSpan totalTimeSpan = TimeSpan.Zero;
                    decoder.Decoding(filenameDecoding, pathDecoding + "\\" + nameOutputFile.Text + ".txt", ref totalTimeSpan);
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
                filenameCoding = dlg.FileName;
                ChosenFile.Text = filenameCoding;
            }
        }

        private void FinishMessage()
        {
            StatusLabelCoding.Content = "Файл закодирован";
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