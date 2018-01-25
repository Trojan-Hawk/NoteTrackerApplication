using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApplication {
    class Note {
        public String Title { get; set; }
        public String Tag1 { get; set; }
        public String Tag2 { get; set; }
        public String Tag3 { get; set; }
        public String Tag4 { get; set; }
        public String Contents { get; set; }

        public Note(String title, String tag1, String tag2, String tag3, String tag4, String contents) {
            this.Title = title;
            this.Tag1 = tag1;
            this.Tag2 = tag2;
            this.Tag3 = tag3;
            this.Tag4 = tag4;
            this.Contents = contents;
        }// Parameterised Constructor
    }// Note(class)
}// namespace
