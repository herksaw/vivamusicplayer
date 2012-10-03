using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using Microsoft.Win32;
using NAudio;
using NAudio.Wave;
using Xceed.Wpf.Toolkit;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public bool? result = null;        

        public static string[] fileNames = null;     
        public string fileNameOnly = "";      

        public string oldTitleName = "";
        public static bool checkCodeOnceStatus = true;

        public int controller = 0;

        public DispatcherTimer musicTimer = new DispatcherTimer();
        public DispatcherTimer sliderTimer = new DispatcherTimer();

        public static double changedSliderValue = 0;        
                
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Title = "Select one or more files to open";

            fileDialog.Multiselect = true;

            fileDialog.Filter = "Audio files (*.mp3;*.wav;)|*.mp3;*.wav";

            result = fileDialog.ShowDialog();           
            
            if (result == true)
            {
                Array.Resize(ref fileNames, fileDialog.FileNames.Length);
                
                for (int i = 0; i < fileDialog.FileNames.Length; i++)
                {
                    fileNames[i] = fileDialog.FileNames[i];
                }
                controller = 0;
            }            
                      
            if (fileNames == null)
            {
                InitializeTimer();
            }
            else if (fileNames[controller].EndsWith(".mp3") || fileNames[controller].EndsWith(".wav"))
            {
                RegularAudioTask();                
            }
            
        }

        void sliderTimer_Tick(object sender, EventArgs e)
        {
            if (AudioPlayer.playerDevice != null)
            {
                if (fileNames[controller].EndsWith(".mp3"))
                {
                    slider.Maximum = AudioPlayer.mp3Reader.TotalTime.TotalSeconds;
                    textBlockTotal.Text = (TimeSpan.FromSeconds(AudioPlayer.mp3Reader.TotalTime.TotalSeconds)).ToString();
                    slider.Value = AudioPlayer.mp3Reader.CurrentTime.TotalSeconds;
                    textBlockCurrent.Text = (TimeSpan.FromSeconds(AudioPlayer.mp3Reader.CurrentTime.TotalSeconds)).ToString();
                }
                else if (fileNames[controller].EndsWith(".wav"))
                {
                    slider.Maximum = AudioPlayer.waveReader.TotalTime.TotalSeconds;
                    textBlockTotal.Text = (TimeSpan.FromSeconds(AudioPlayer.waveReader.TotalTime.TotalSeconds)).ToString();
                    slider.Value = AudioPlayer.waveReader.CurrentTime.TotalSeconds;
                    textBlockCurrent.Text = (TimeSpan.FromSeconds(AudioPlayer.waveReader.CurrentTime.TotalSeconds)).ToString();
                }
            }
        }            

        void timer_Tick(object sender, EventArgs e)
        {
            CheckIsMusicEnd();
        }

        public void RegularAudioTask()
        {
            CheckCodeTask();

            if (controller < fileNames.Length && controller >= 0)
            {                
                Cleaner.CloseWaveOut();
                AudioPlayer.CreateInputStream(fileNames, controller);

                fileNameOnly = " - " + System.IO.Path.GetFileNameWithoutExtension(fileNames[controller]);
                Title += fileNameOnly;               
                
                AudioPlayer.AudioPlay(fileNames[controller]);                
            }
            else
            {
                Cleaner.CloseWaveOut();
                controller = 0;                
                RegularAudioTask();
            }
        }               

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cleaner.CloseWaveOut();            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Cleaner.CloseWaveOut();
            this.Close();
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.playerDevice != null)
            {
                if (AudioPlayer.playerDevice.PlaybackState == PlaybackState.Paused)
                {
                    AudioPlayer.playerDevice.Play();
                }
                else if (AudioPlayer.playerDevice.PlaybackState == PlaybackState.Stopped)
                {
                    AudioPlayer.playerDevice.Play();
                }
            }
            else AudioPlayer.playerDevice = null;
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.playerDevice != null)
            {
                if (AudioPlayer.playerDevice.PlaybackState == PlaybackState.Playing)
                {
                    AudioPlayer.playerDevice.Pause();
                }
                else if (AudioPlayer.playerDevice.PlaybackState == PlaybackState.Paused)
                {
                    AudioPlayer.playerDevice.Play();
                }
            }
            else AudioPlayer.playerDevice = null;
        }        

        public void CheckCodeTask()
        {
            if (checkCodeOnceStatus == true)
            {
                InitializeTimer();                
                oldTitleName = Title.ToString();
                checkCodeOnceStatus = false;
            }

            if (Title.Length > oldTitleName.Length)
            {
                Title = Title.Remove(oldTitleName.Length);
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            NextAudioTask();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();            
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.playerDevice != null)
            {
                PreviousAudioTask();
            }
        }

        public void CheckIsMusicEnd()
        {
            if (AudioPlayer.playerDevice != null)
            {
                if (fileNames[controller].EndsWith(".mp3"))
                {
                    if (AudioPlayer.mp3Reader.CurrentTime >= AudioPlayer.mp3Reader.TotalTime)
                    {
                        NextAudioTask();
                    }
                }
                else if (fileNames[controller].EndsWith(".wav"))
                {
                    if (AudioPlayer.waveReader.CurrentTime >= AudioPlayer.waveReader.TotalTime)
                    {
                        NextAudioTask();
                    }
                }
            }
        }

        public void NextAudioTask()
        {
            if (fileNames.Length > 1)
            {
                Cleaner.CloseWaveOut();
                controller++;
                RegularAudioTask();
            }
            else
            {
                RegularAudioTask();
            }
        }

        public void PreviousAudioTask()
        {
            if (fileNames.Length > 1)
            {
                Cleaner.CloseWaveOut();
                controller--;
                RegularAudioTask();
            }
            else
            {
                RegularAudioTask();
            }
        }

        public void InitializeTimer()
        {            
            musicTimer.Tick += new EventHandler(timer_Tick);
            musicTimer.Interval = new TimeSpan(0, 0, 1);
            musicTimer.Start();

            sliderTimer.Tick += new EventHandler(sliderTimer_Tick);
            sliderTimer.Interval = new TimeSpan(0, 0, 1);
            sliderTimer.Start();
        }

        private void slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AudioPlayer.playerDevice != null)
            {
                if (fileNames[controller].EndsWith(".mp3"))
                {
                changedSliderValue = ((Slider)sender).Value;
                AudioPlayer.mp3Reader.CurrentTime = TimeSpan.FromSeconds(changedSliderValue);
                }
                else if (fileNames[controller].EndsWith(".wav"))
                {
                changedSliderValue = ((Slider)sender).Value;
                AudioPlayer.waveReader.CurrentTime = TimeSpan.FromSeconds(changedSliderValue);
                }
            }
        }         
    }
}
