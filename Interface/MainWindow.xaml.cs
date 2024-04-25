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
//using System.CodeDom;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filename;
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
            if (filename == null)
            {
                MessageBox.Show("Файл не выбран!");
            }
            else
            {
                coder.Coding(filename);
                IsCoding = false;
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

        private void ChooseFileClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                ChosenFile.Content = filename;
            }
        }

        private void FinishMessage()
        {
            StatusLabelCoding.Content = "Файл закодирован";
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}