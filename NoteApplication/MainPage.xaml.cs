﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NoteApplication {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        private Note[] notes;

        public MainPage() {
            this.InitializeComponent();

            showMenu();
            hideAddNote();
        }// MainPage

        // MAIN MENU
        // Add Note main menu option
        private void menubtnAddNote_Click(object sender, RoutedEventArgs e) {
            hideMenu();
            showAddNote();
        }// menubtnAddNote_Click
        private void menubtnViewNotes_Click(object sender, RoutedEventArgs e) {
            hideMenu();
            showViewNotes();
        }// menubtnViewNotes_Click
        private void menubtnViewTaggedNotes_Click(object sender, RoutedEventArgs e) {
            hideMenu();

        }// menubtnViewTaggedNotes_Click
        private void menubtnSettings_Click(object sender, RoutedEventArgs e) {
            hideMenu();

        }// menubtnSettings_Click
        private void menubtnExitApp_Click(object sender, RoutedEventArgs e) {
            // exit the app
            Application.Current.Exit();
        }// menubtnExitApp_Click

        // ADD NOTE
        // Add Note exit to menu
        private void addnotebtnOpenMenu_Click(object sender, RoutedEventArgs e) {
            hideAddNote();
            showMenu();
        }// addnotebtnOpenMenu_Click
        // Add note button clicked
        private void addnotebtnaddNote_Click(object sender, RoutedEventArgs e) {
            if (addnotetxbxNote.Text.Length > 0 && addnotetxbxTitle.Text.Length > 0){
                appendNote();
                hideAddNote();
                showMenu();
            }// if
            else {
                // title and/or contents blank
                // set the error message to visable
                addnotetxblkError.Visibility = Visibility.Visible;
                // print the error message
                addnotetxblkError.Text = "ERROR: Note Title and/or Contents cannot be blank!";
            }// else
        }// addnotebtnaddNote_Click
        // Append note to file asynchronously
        public async void appendNote()
        {
            // creating the note object
            Note note = new Note(addnotetxbxTitle.Text, addnotetxbxTag1.Text,
                addnotetxbxTag2.Text, addnotetxbxTag3.Text, addnotetxbxTag4.Text, addnotetxbxNote.Text);
            // sending the note object to the note writer constructor
            NoteWriter nw = new NoteWriter(note);
            // call the asynchronous method to append the note to the file
            await nw.appendToFileAsync();
        }// appendNote

        // VIEW NOTES
        private void viewnotesbtnOpenMenu_Click(object sender, RoutedEventArgs e) {
            hideViewNotes();
            showMenu();

            // read in all the notes
            NoteReader nr = new NoteReader();
            // amount of notes in file
            int numOfNotes = nr.noteCount();
            // allocating memory for the note array
            notes = new Note[numOfNotes]; // might be redundant
            // read in the notes from the file 
            nr.readFile();
            // populating the notes array
            notes = nr.GetNotes();

            // for loop that prints each note to the screen


        }// viewnotesbtnOpenMenu_Click

        // VIEW TAGGED NOTES
        


        // SETTINGS
        


        private void showMenu() {
            // changing the header text
            header.Text = "Note Tracker";

            // setting all the menu items to visible
            menubtnAddNote.Visibility = Visibility.Visible;
            menubtnViewNotes.Visibility = Visibility.Visible;
            menubtnViewTaggedNotes.Visibility = Visibility.Visible;
            menubtnSettings.Visibility = Visibility.Visible;
            menubtnExitApp.Visibility = Visibility.Visible;
        }// showMenu
        private void hideMenu() {
            // setting all the menu items to collapsed
            // which does not show the element or reserve space for it
            menubtnAddNote.Visibility = Visibility.Collapsed;
            menubtnViewNotes.Visibility = Visibility.Collapsed;
            menubtnViewTaggedNotes.Visibility = Visibility.Collapsed;
            menubtnSettings.Visibility = Visibility.Collapsed;
            menubtnExitApp.Visibility = Visibility.Collapsed;
        }// hideMenu
        private void showAddNote() {
            // changing the header text
            header.Text = "Add Note";

            // setting all the add note items to visible
            addnotetxbxTitle.Visibility = Visibility.Visible;
            addnotetxbxTag1.Visibility = Visibility.Visible;
            addnotetxbxTag2.Visibility = Visibility.Visible;
            addnotetxbxTag3.Visibility = Visibility.Visible;
            addnotetxbxTag4.Visibility = Visibility.Visible;
            addnotetxbxNote.Visibility = Visibility.Visible;
            addnotebtnaddNote.Visibility = Visibility.Visible;
            addnotebtnOpenMenu.Visibility = Visibility.Visible;
        }// showAddNote
        private void hideAddNote() {
            resetAddNoteFields();

            // setting all the add Note items to collapsed 
            // which does not show the element or reserve space for it
            addnotetxblkError.Visibility = Visibility.Collapsed;
            addnotetxbxTitle.Visibility = Visibility.Collapsed;
            addnotetxbxTag1.Visibility = Visibility.Collapsed;
            addnotetxbxTag2.Visibility = Visibility.Collapsed;
            addnotetxbxTag3.Visibility = Visibility.Collapsed;
            addnotetxbxTag4.Visibility = Visibility.Collapsed;
            addnotetxbxNote.Visibility = Visibility.Collapsed;
            addnotebtnaddNote.Visibility = Visibility.Collapsed;
            addnotebtnOpenMenu.Visibility = Visibility.Collapsed;
        }// hideAddNote
        private void showViewNotes() {
            // changing the header text
            header.Text = "View Notes";

            // setting all the add note items to visible
            viewnotestxblkError.Visibility = Visibility.Visible;
            viewnotesbtnOpenMenu.Visibility = Visibility.Visible;

            // 
        }// hideViewNotes
        private void hideViewNotes() {
            // setting all the add Note items to collapsed 
            // which does not show the element or reserve space for it
            viewnotestxblkError.Visibility = Visibility.Collapsed;
            viewnotesbtnOpenMenu.Visibility = Visibility.Collapsed;
        }// hideViewNotes



        private void resetAddNoteFields() {
            // set all the input fields to blank
            addnotetxbxTitle.Text = "";
            addnotetxbxTag1.Text = "";
            addnotetxbxTag2.Text = "";
            addnotetxbxTag3.Text = "";
            addnotetxbxTag4.Text = "";
            addnotetxbxNote.Text = "";
        }// resetAddNoteFields

    }// MainPage
}// NoteApplication
