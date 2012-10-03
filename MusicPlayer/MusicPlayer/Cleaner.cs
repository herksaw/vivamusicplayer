using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicPlayer
{
    public static class Cleaner
    {
        public static void CloseWaveOut()
        {
            if (AudioPlayer.playerDevice != null)
            {
                AudioPlayer.playerDevice.Stop();                
            }

            if (AudioPlayer.mp3Reader != null)
            {
                AudioPlayer.mp3Reader.Dispose();
                AudioPlayer.mp3Reader = null;
            }

            if (AudioPlayer.waveReader != null)
            {
                AudioPlayer.waveReader.Dispose();
                AudioPlayer.waveReader = null;
            }
            
            if (AudioPlayer.playerDevice != null)
            {
                AudioPlayer.playerDevice.Dispose();
                AudioPlayer.playerDevice = null;
            }
        }
    }
}
