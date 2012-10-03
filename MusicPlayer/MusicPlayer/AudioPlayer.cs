using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using NAudio;
using NAudio.Wave;

namespace MusicPlayer
{
    public static class AudioPlayer
    {
        public static IWavePlayer playerDevice;       
        public static WaveStream mp3Reader;
        public static WaveStream waveReader;        

        public static WaveStream CreateInputStream(string[] fileNames, int i)
        {
            playerDevice = new WaveOut();
            
            if (fileNames[i].EndsWith(".mp3"))
            {
                mp3Reader = new Mp3FileReader(fileNames[i]);                
            }
            else if (fileNames[i].EndsWith(".wav"))
            {
                waveReader = new WaveFileReader(fileNames[i]);                
            }
            else
            {
                MessageBox.Show("Invalid file extension or format.", "File extension error", MessageBoxButton.OK); 
            }

            return mp3Reader;
        }

        public static void AudioPlay(string localFileName)
        {
            if (localFileName.EndsWith(".mp3"))
            {
                playerDevice.Init(mp3Reader);
                playerDevice.Play();
            }
            else if (localFileName.EndsWith(".wav"))
            {
                playerDevice.Init(waveReader);
                playerDevice.Play();
            }
        }                                  
    }
}
