using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NoteApplication {
    class SettingsWriter {

        private String filename = "Settings.txt";
        private Windows.UI.Color applicationMainColour;
        private Windows.UI.Color applicationSecondaryColour;
        private Windows.UI.Color fontColour;
        private string fontFamily;
        private int fontSize;

        public SettingsWriter(Windows.UI.Color applicationMainColour, Windows.UI.Color applicationSecondaryColour, Windows.UI.Color fontColour, string fontFamily, int fontSize) {
            this.applicationMainColour = applicationMainColour;
            this.applicationSecondaryColour = applicationSecondaryColour;
            this.fontColour = fontColour;
            this.fontFamily = fontFamily;
            this.fontSize = fontSize;
        }// Parameterised Constructor

        // the 'async' keyword enables the 'await' keyword
        // the async method pauses until the awaitable is complete (so it waits)
        // but the actual thread is not blocked (so it’s asynchronous).
        public async Task writeToFileAsync() {
            // setting the file we will write to
            // it is created if it does not exist
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            // Creating a string from the note object
            // this will then be written to a file
            String str = this.applicationMainColour.ToString() + "\n" + this.applicationSecondaryColour.ToString() + "\n" + this.fontColour.ToString() + "\n" + this.fontFamily + "\n" + this.fontSize;

            // open the stream
            using (Stream stream = await file.OpenStreamForWriteAsync()) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    // write the string to the file
                    await writer.WriteAsync(str);
                    // flush the stream buffer
                    await writer.FlushAsync();
                }// StreamWriter
            }// Stream
        }// writeToFileAsync

    }// SettingsWriter
}// NoteApplication
