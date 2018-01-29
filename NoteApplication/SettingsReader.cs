using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace NoteApplication {
    class SettingsReader {

        public String filename = "Settings.txt";
        public Windows.UI.Color applicationMainColour { get; set; }
        public Windows.UI.Color applicationSecondaryColour { get; set; }
        public Windows.UI.Color fontColour { get; set; }
        public string fontFamily { get; set; }
        public int fontSize { get; set; }
        private String file = ApplicationData.Current.LocalFolder.Path; // getting the path of the storage location

        public void readSettingsFile() {
            // variables
            int count = 0;
            string temp;
            string[] str = new string[5];

            // read in each line of the file
            // appending on the filename to the storage location
            using (var stringReader = File.OpenText(this.file + @"\" + filename)) {
                while ((temp = stringReader.ReadLine()) != null) {
                    // store each line in a string array
                    str[count] = temp;
                    // increment the counter e.g string array position
                    count++;
                }// while
            }// reader

            extractSettings(str);

        }// readSettingsFile

        public void extractSettings(string[] str) {
            var color = GetSolidColorBrush(str[0]).Color;
            this.applicationMainColour = color;
            color = GetSolidColorBrush(str[1]).Color;
            this.applicationSecondaryColour = color;
            color = GetSolidColorBrush(str[2]).Color;
            this.fontColour = color;
            this.fontFamily = str[3];
            this.fontSize = Int32.Parse(str[4]);
        }// extractSettings

        public SolidColorBrush GetSolidColorBrush(string hex) {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }// GetSolidColorBrush

    }// SettingsReader
}// NoteApplication
