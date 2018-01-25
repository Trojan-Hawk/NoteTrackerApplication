using System;
using System.Collections.Generic;
using Windows.Storage;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace NoteApplication
{
    class NoteWriter
    {
        private String filename = "Notes.txt";
        public Note note;

        public NoteWriter(Note note) {
            this.note = note;
        }// Parameterised Constructor

        // the 'async' keyword enables the 'await' keyword
        // the async method pauses until the awaitable is complete (so it waits)
        // but the actual thread is not blocked (so it’s asynchronous).
        public async Task appendToFileAsync() {
            // setting the file we will write to
            // it is created if it does not exist
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);

            // Creating a string from the note object
            // this will then be written to a file
            String str = note.Title + "\n" + note.Tag1 + "#" + note.Tag2 + "#" + note.Tag3
                         + "#" + note.Tag4  + "#" + "\n" + note.Contents + "\n";

            // open the stream
            using (Stream stream = await file.OpenStreamForWriteAsync()) {
                // place the origin of the output stream at the end of the file
                stream.Seek(0, SeekOrigin.End);

                using (StreamWriter writer = new StreamWriter(stream)) {
                    // write the string to the file
                    await writer.WriteAsync(str);
                    // flush the stream buffer
                    await writer.FlushAsync();
                }// StreamWriter
            }// Stream
        }// appendToFile

    }// class
}// namespace
