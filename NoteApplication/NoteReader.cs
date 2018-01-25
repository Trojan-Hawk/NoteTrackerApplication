using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace NoteApplication
{
    class NoteReader
    {
        private String filename = "Notes.txt";
        private String searchTag = "";
        private Note[] notes;
        private int numOfTaggedNotes = 0;
        private String file = ApplicationData.Current.LocalFolder.Path; // getting the path of the storage location

        public NoteReader() {

        }// default constructor

        public NoteReader(String searchTag) {
            this.searchTag = searchTag;
        }// parameterised constructor

        public int getNumOfTaggedNotes() {
            return this.numOfTaggedNotes;
        }// getNumOfTaggedNotes

        public Note[] readFile() {
            // variables
            int count = 0;
            int lineNum = lineCount();
            string temp;
            string[] str = new string[lineNum];

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

            if(searchTag.Length > 0) {
                extractTaggedNotes(str);
            }// if
            else {
                extractNotes(str);
            }// else

            return this.notes;
        }// readFile

        // Takes in a string array and populates the notes object array 
        public void extractNotes(string[] str) {

            // variables
            int lineNum = lineCount();
            int numOfNotes = lineNum / 3;                     //***** when adding in the date to the file this needs to be changed
            int stringPosition = 0;
            Note note;
            this.notes = new Note[numOfNotes];

            // have string array of lines
            // take every 3 lines as a note 
            // then add note to notes array
            for (int i = 0; i < numOfNotes; i++) {
                // note variables
                string title = "", tag1 = "", tag2 = "", tag3 = "", tag4 = "", contents = "";
                string[] tags = new string[4];

                // extracting the title for the note
                title = str[stringPosition];
                // increment the string position after the title has been stored
                stringPosition++;

                // as all tags are on the same line,
                // the string needs to be split on the break character used e.g'#'
                tags = str[stringPosition].Split('#');
                // increment the string position after the tags have been stored
                stringPosition++;
                // then we store the tags if they have a value
                if (tags[0].Length > 0)
                    tag1 = tags[0];
                if (tags[1].Length > 0)
                    tag2 = tags[1];
                if (tags[2].Length > 0)
                    tag3 = tags[2];
                if (tags[3].Length > 0)
                    tag4 = tags[3];

                // tag1 = tags[4];


                // next we have to extract the contents of the note
                contents = str[stringPosition];
                // increment the string position after the contents have been stored
                stringPosition++;

                // creating the note object
                note = new Note(title, tag1, tag2, tag3, tag4, contents);
                // storing the note object on the notes array
                this.notes[i] = note;
            }// for

        }// extractNotes

        // Takes in a string array and populates the notes object array 
        public void extractTaggedNotes(string[] str) {

            // variables
            int lineNum = lineCount();
            int numOfNotes = lineNum / 3;                     //***** when adding in the date to the file this needs to be changed
            int stringPosition = 0;
            Note note;
            this.notes = new Note[numOfNotes];

            // have string array of lines
            // take every 3 lines as a note 
            // then add note to notes array
            for (int i = 0; i < numOfNotes; i++) {
                // note variables
                string title = "", tag1 = "", tag2 = "", tag3 = "", tag4 = "", contents = "";
                string[] tags = new string[4];

                // extracting the title for the note
                title = str[stringPosition];
                // increment the string position after the title has been stored
                stringPosition++;

                // as all tags are on the same line,
                // the string needs to be split on the break character used e.g'#'
                tags = str[stringPosition].Split('#');
                // increment the string position after the tags have been stored
                stringPosition++;
                // then we store the tags if they have a value
                if (tags[0].Length > 0)
                    tag1 = tags[0];
                if (tags[1].Length > 0)
                    tag2 = tags[1];
                if (tags[2].Length > 0)
                    tag3 = tags[2];
                if (tags[3].Length > 0)
                    tag4 = tags[3];

                // next we have to extract the contents of the note
                contents = str[stringPosition];
                // increment the string position after the contents have been stored
                stringPosition++;

                // if the tags match the search tag
                if(tag1.Equals(this.searchTag) || tag2.Equals(this.searchTag) || 
                    tag3.Equals(this.searchTag) || tag4.Equals(this.searchTag)) {
                    // creating the note object
                    note = new Note(title, tag1, tag2, tag3, tag4, contents);
                    // storing the note object on the notes array
                    this.notes[this.numOfTaggedNotes] = note;
                    // increment the counter for the tagged notes
                    this.numOfTaggedNotes++;
                }// if
            }// for
        }// extractTaggedNotes

        // returns the number of lines in the notes.txt file
        public int lineCount() {
            var lineCount = 0;
            using (var reader = File.OpenText(this.file + @"\" + filename)) {
                while (reader.ReadLine() != null) {
                    lineCount++;
                }// while
            }// reader
            return lineCount;
        }// lineCount

        // returns the number of notes in the notes.txt file
        public int noteCount() {
            int lineNum = lineCount();
            return lineNum / 3;                     //***** when adding in the date to the file this needs to be changed
        }// noteCount

        public Note[] GetNotes() {
            return this.notes;
        }// GetNotes

    }// class
}// namespace
