using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace NoteApplication
{
    class NoteReader
    {
        private String filename = "Notes.txt";
        private String searchTag = "";
        private Note[] notes;

        public NoteReader() {

        }// default constructor

        public NoteReader(String searchTag) {
            this.searchTag = searchTag;
        }// parameterised constructor

        public Note[] readFile() {
            int count = 0;
            int lineNum = lineCount();
            string[] str = new string[lineNum];

            using (var stringReader = File.OpenText(filename)) {
                while (stringReader.ReadLine() != null) {
                    str[count] = stringReader.ReadLine();

                    count++;
                }// while
            }// reader

            extractNotes(str);

            return null;
        }// readFileAsync

        public void extractNotes(string[] str) {
            int lineNum = lineCount();
            int numOfNotes = 0;
            Note note;
            notes = new Note[lineNum / 4];

            for (int i = 0; i < lineNum; i++) {
                note = new Note(str[i], str[i+1], str[i+2], str[i+3]);
                notes[numOfNotes] = note;
                numOfNotes++;


            }

        }// extractNotes

        public int lineCount() {
            var lineCount = 0;
            using (var reader = File.OpenText(filename)) {
                while (reader.ReadLine() != null) {
                    lineCount++;
                }// while
            }// reader
            return lineCount;
        }// lineCount

        public Note[] GetNotes() {
            return this.notes;
        }// GetNotes

    }// class
}// namespace
