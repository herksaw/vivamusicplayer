using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {           
            ReadFileTask(@"\README.txt", smallTextBox);
            ReadFileTask(@"\features.txt", largeTextBox);
            ReadFileTask(@"\LICENSE.txt", licenseBox);
            ReadFileTask(@"\thanks.txt", thanksBox);
        }

        public void ReadFileTask(string filePath, TextBox target)
        {
            FileStream fileStream = null;
            StreamReader streamReader = null;
            string aboutString = null;

            fileStream = new FileStream(System.IO.Directory.GetCurrentDirectory().ToString() + filePath,
                FileMode.Open, FileAccess.Read);

            streamReader = new StreamReader(fileStream);

            aboutString = streamReader.ReadToEnd();

            fileStream.Close();
            streamReader.Close();
            fileStream.Dispose();
            streamReader.Dispose();

            target.Text = aboutString;
        }        
    }
}
