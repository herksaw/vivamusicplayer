using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace MusicPlayer
{
    public class FileManagement
    {
        private List<string> stringList = new List<string>();
        private string[] stringArray = new string[0];
        private static bool? result = null;

        public bool? OpenMediaDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Multiselect = true;

            fileDialog.DefaultExt = "Audio files (*.mp3)|*.mp3";

            result = fileDialog.ShowDialog();
            
            return result;
        }        
    }
}
