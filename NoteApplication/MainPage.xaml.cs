using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace NoteApplication {
    public sealed partial class MainPage : Page {
        // private variables
        private Note[] notes;
        private int amountOfNotes;
        private Windows.UI.Color applicationMainColour;
        private Windows.UI.Color applicationSecondaryColour;
        private Windows.UI.Color fontColour = Windows.UI.Colors.Black;
        private Windows.UI.Xaml.Media.FontFamily fontFamily;
        private int fontSize;

        public MainPage() {
            this.InitializeComponent();

            // load the application settings
            loadSettings();
            // applying the colour scheme
            applyColourScheme();
            // applying the font scheme
            applyFontScheme();

            // setting the application start-up visibility values
            showMenu();
            hideAddNote();
            hideViewNotes();
            hideViewTaggedNotes();
            hideSettings();
        }// MainPage

        // MAIN MENU
        // Add Note main menu options click events
        private void menubtnAddNote_Click(object sender, RoutedEventArgs e) {
            hideMenu();
            showAddNote();
        }// menubtnAddNote_Click

        // view notes click event
        private void menubtnViewNotes_Click(object sender, RoutedEventArgs e) {
            hideMenu();
            showViewNotes();
            getNotes();
        }// menubtnViewNotes_Click

        // view tagged notes click event
        private void menubtnViewTaggedNotes_Click(object sender, RoutedEventArgs e) {
            hideMenu();
            showViewTaggedNotes();
        }// menubtnViewTaggedNotes_Click

        // settings click event
        private void menubtnSettings_Click(object sender, RoutedEventArgs e) {
            hideMenu();
            showSettings();
        }// menubtnSettings_Click

        // exit application click event
        private void menubtnExitApp_Click(object sender, RoutedEventArgs e) {
            // save the settings
            // setting the saveSettings Task to a task variable
            Task task = saveSettings();
            // waiting until the task completes
            task.Wait(1000);
            // after the task has complete exit the app
            Application.Current.Exit();
        }// menubtnExitApp_Click

        // ADD NOTE
        // Add Note exit to menu click event
        private void addnotebtnOpenMenu_Click(object sender, RoutedEventArgs e) {
            hideAddNote();
            showMenu();
        }// addnotebtnOpenMenu_Click

        // Add note button click event
        private void addnotebtnaddNote_Click(object sender, RoutedEventArgs e) {
            if (addnotetxbxNote.Text.Length > 0 && addnotetxbxTitle.Text.Length > 0) {
                appendNote();
                hideAddNote();
                showMenu();
            }// if
            else {
                // title and/or contents blank
                // set the error message to visible
                addnotetxblkError.Visibility = Visibility.Visible;
                // print the error message
                addnotetxblkError.Text = "ERROR: Note Title and/or Contents cannot be blank!";
            }// else
        }// addnotebtnaddNote_Click

        // Append note to file asynchronously
        public async void appendNote() {
            // creating the note object
            Note note = new Note(addnotetxbxTitle.Text, addnotetxbxTag1.Text,
                addnotetxbxTag2.Text, addnotetxbxTag3.Text, addnotetxbxTag4.Text, addnotetxbxNote.Text);
            // sending the note object to the note writer constructor
            NoteWriter nw = new NoteWriter(note);
            // call the asynchronous method to append the note to the file
            await nw.appendToFileAsync();
        }// appendNote

        // VIEW NOTES
        // open menu click event
        private void viewnotesbtnOpenMenu_Click(object sender, RoutedEventArgs e) {
            hideViewNotes();
            // clearing the children of the view notes stack panel
            viewNotesStkPnl.Children.Clear();
            showMenu();
        }// viewnotesbtnOpenMenu_Click

        // creates a new noteReader object
        // the notes are then returned from this object
        // then they are sent to the PrintNotes method
        private void getNotes() {
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
            PrintNotes(notes, numOfNotes);
        }// getNotes

        // populating the stackPanel with the note details
        // tags will only be printed if they are not null
        private void PrintNotes(Note[] n, int numOfNotes) {
            // setting the gloabal variable for the number of notes
            this.amountOfNotes = numOfNotes;

            // for loop that prints each note to the screen
            for (int i = 0; i < numOfNotes; i++) {
                // outer stack panel
                StackPanel outerStkpnl = new StackPanel();

                // outer border
                Border outerBorder = new Border();
                // setting the colour of the border
                outerBorder.Background = new SolidColorBrush(this.applicationMainColour);
                outerBorder.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                // setting the curve of the corner
                outerBorder.CornerRadius = new CornerRadius(10);
                // setting the border line thickness
                outerBorder.BorderThickness = new Thickness(1);
                // setting the border padding
                outerBorder.Padding = new Thickness(0,20,0,0);
                // setting the margin, the distance between each note when displayed
                outerBorder.Margin = new Thickness(100, 0, 100, 20);

                // adding the outerStackpnl as a child of the outerBorder
                outerBorder.Child = outerStkpnl;

                // innerStackpnl declaration
                StackPanel innerStkpnl = new StackPanel();

                Border innerBorder = new Border();
                // setting the colour of the border
                innerBorder.Background = new SolidColorBrush(this.applicationSecondaryColour);
                innerBorder.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                // setting the curve of the corner
                innerBorder.CornerRadius = new CornerRadius(10);
                // setting the border line thickness
                innerBorder.BorderThickness = new Thickness(1);
                // setting the border padding
                innerBorder.Padding = new Thickness(20,0,20,20);

                // using a button to display the title
                TextBlock textBlock = new TextBlock();
                // setting the font colour
                textBlock.Foreground = new SolidColorBrush(this.fontColour);
                textBlock.Text = notes[i].Title;
                // setting the font family
                textBlock.FontFamily = this.fontFamily;
                // setting the font size
                textBlock.FontSize = 18 + this.fontSize;
                textBlock.FontWeight = FontWeights.ExtraBlack;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.Margin = new Thickness(0,0,0,10);

                // adding the textblock as a child of the outer stack panel
                outerStkpnl.Children.Add(textBlock);

                // appending the tags onto a string if they are not null
                string tags = "";
                if (notes[i].Tag1 != null)
                    tags += notes[i].Tag1 + " ";
                if (notes[i].Tag2 != null)
                    tags += notes[i].Tag2 + " ";
                if (notes[i].Tag3 != null)
                    tags += notes[i].Tag3 + " ";
                if (notes[i].Tag4 != null)
                    tags += notes[i].Tag4;

                // if there are tags to display
                if(tags.Length >= 4) {
                    textBlock = new TextBlock();
                    textBlock.Text = "(Tags: " + tags + ")";
                    // setting the font family
                    textBlock.FontFamily = this.fontFamily;
                    // setting the font size
                    textBlock.FontSize = 12 + this.fontSize;
                    textBlock.FontWeight = FontWeights.Thin;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    // add the tag textblock to the inner stack panel
                    innerStkpnl.Children.Add(textBlock);
                }// if
                
                // grid row and column span for contents display
                textBlock = new TextBlock();
                textBlock.Text = notes[i].Contents;
                // setting the font family
                textBlock.FontFamily = this.fontFamily;
                // setting the fot size
                textBlock.FontSize = 18 + this.fontSize;
                // setting the text to wrap if too long
                textBlock.TextWrapping = TextWrapping.Wrap;
                // setting the text to center
                textBlock.HorizontalTextAlignment = TextAlignment.Center;
                // setting the margin
                textBlock.Margin = new Thickness(100,20,100,10);
                // adding the textblock to the inner stack pannel
                innerStkpnl.Children.Add(textBlock);

                // delete button
                Button button = new Button();
                // setting the background colour
                button.Background = new SolidColorBrush(this.applicationSecondaryColour);
                button.Foreground = new SolidColorBrush(this.fontColour);
                button.Content = "Delete Note";
                // setting the delete note name
                button.Name = i.ToString();
                // setting the font family
                button.FontFamily = this.fontFamily;
                // setting the font size
                button.FontSize = 18 + this.fontSize;
                // setting the alignment to center
                button.HorizontalAlignment = HorizontalAlignment.Center;
                // setting a margin 
                button.Margin = new Thickness(5);
                // button click event
                button.Click += new RoutedEventHandler(deleteNoteAsync);

                // making the stkpanel a child of the border
                innerBorder.Child = innerStkpnl;

                // adding the innerBorder as a child of the outerStkpnl
                outerStkpnl.Children.Add(innerBorder);
                // adding the button as a child of the outerStkpnl
                outerStkpnl.Children.Add(button);
                
                // add the new stack panel to the stack panel on the xaml
                viewNotesStkPnl.Children.Add(outerBorder);
            }// for
        }// printNotes

        // VIEW TAGGED NOTES
        // open menu click event
        private void viewtaggedbtnOpenMenu_Click(object sender, RoutedEventArgs e) {
            hideViewTaggedNotes();
            // clearing all children of the stack pannel
            viewtaggedStkPnl.Children.Clear();
            showMenu();
        }// viewtaggedbtnOpenMenu_Click

        // search click event
        private void viewtaggedbtnSearch_Click(object sender, RoutedEventArgs e) {
            string searchTag = viewtaggedtxbxSearch.Text;

            // if the searchTag has a value
            if (searchTag != null || searchTag.Length > 0) {
                // call the getTaggedNotes method based on the search string
                getTaggedNotes(searchTag);
            }// if
            else {
                // display an error
                viewtaggedtxblkError.Visibility = Visibility.Visible;
                viewtaggedtxblkError.Text += "ERROR: Cannot search for an empty tag value!";
            }// else
        }// viewtaggedbtnSearch_Click

        // creates a new noteReader object using the search string
        // the notes are then returned from this object
        // then they are sent to the PrintNotes method
        private void getTaggedNotes(string searchTag) {
            // read in all the notes
            NoteReader nr = new NoteReader(searchTag);
            // read in the notes from the file 
            nr.readFile();
            // populating the notes array
            notes = nr.GetNotes();

            // amount of notes in file
            int numOfNotes = nr.getNumOfTaggedNotes();

            PrintTaggedNotes(notes, numOfNotes);
        }// getNotes

        // populating the stackPanel with the tagged note details
        // tags will only be printed if they are not null
        private void PrintTaggedNotes(Note[] notes, int numOfNotes) {
            // setting the gloabal variable for the number of notes
            this.amountOfNotes = numOfNotes;

            // for loop that prints each note to the screen
            for (int i = 0; i < numOfNotes; i++) {
                // outer stack panel
                StackPanel outerStkpnl = new StackPanel();

                // outer border
                Border outerBorder = new Border();
                // setting the colour of the border
                outerBorder.Background = new SolidColorBrush(this.applicationMainColour);
                outerBorder.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                // setting the curve of the corner
                outerBorder.CornerRadius = new CornerRadius(10);
                // setting the border line thickness
                outerBorder.BorderThickness = new Thickness(1);
                // setting the border padding
                outerBorder.Padding = new Thickness(0, 20, 0, 0);
                // setting the margin, the distance between each note when displayed
                outerBorder.Margin = new Thickness(100, 0, 100, 20);

                // adding the outerStackpnl as a child of the outerBorder
                outerBorder.Child = outerStkpnl;

                // innerStackpnl declaration
                StackPanel innerStkpnl = new StackPanel();

                Border innerBorder = new Border();
                // setting the colour of the border
                innerBorder.Background = new SolidColorBrush(this.applicationSecondaryColour);
                innerBorder.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                // setting the curve of the corner
                innerBorder.CornerRadius = new CornerRadius(10);
                // setting the border line thickness
                innerBorder.BorderThickness = new Thickness(1);
                // setting the border padding
                innerBorder.Padding = new Thickness(20, 0, 20, 20);

                // using a button to display the title
                TextBlock textBlock = new TextBlock();
                // setting the font colour
                textBlock.Foreground = new SolidColorBrush(this.fontColour);
                textBlock.Text = notes[i].Title;
                // setting the font family
                textBlock.FontFamily = this.fontFamily;
                // setting the font size
                textBlock.FontSize = 18 + this.fontSize;
                textBlock.FontWeight = FontWeights.ExtraBlack;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.Margin = new Thickness(0, 0, 0, 10);

                // adding the textblock as a child of the outer stack panel
                outerStkpnl.Children.Add(textBlock);

                // appending the tags onto a string if they are not null
                string tags = "";
                if (notes[i].Tag1 != null)
                    tags += notes[i].Tag1 + " ";
                if (notes[i].Tag2 != null)
                    tags += notes[i].Tag2 + " ";
                if (notes[i].Tag3 != null)
                    tags += notes[i].Tag3 + " ";
                if (notes[i].Tag4 != null)
                    tags += notes[i].Tag4;

                // if there are tags to display
                if (tags.Length >= 4) {
                    textBlock = new TextBlock();
                    textBlock.Text = "(Tags: " + tags + ")";
                    // setting the font family
                    textBlock.FontFamily = this.fontFamily;
                    // setting the font size
                    textBlock.FontSize = 12 + this.fontSize;
                    textBlock.FontWeight = FontWeights.Thin;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    innerStkpnl.Children.Add(textBlock);
                }// if

                // grid row and column span for contents display
                textBlock = new TextBlock();
                textBlock.Text = notes[i].Contents;
                // setting the font family
                textBlock.FontFamily = this.fontFamily;
                // setting the fot size
                textBlock.FontSize = 18 + this.fontSize;
                // setting the text to wrap if too long
                textBlock.TextWrapping = TextWrapping.Wrap;
                // setting the text to center
                textBlock.HorizontalTextAlignment = TextAlignment.Center;
                // setting the margin
                textBlock.Margin = new Thickness(100, 20, 100, 10);
                innerStkpnl.Children.Add(textBlock);

                // delete button
                Button button = new Button();
                // setting the background colour
                button.Background = new SolidColorBrush(this.applicationSecondaryColour);
                button.Foreground = new SolidColorBrush(this.fontColour);
                button.Content = "Delete Note";
                // setting the delete note name
                button.Name = i.ToString();
                // setting the font family
                button.FontFamily = this.fontFamily;
                // setting the font size
                button.FontSize = 18 + this.fontSize;
                // setting the alignment to center
                button.HorizontalAlignment = HorizontalAlignment.Center;
                // setting a margin 
                button.Margin = new Thickness(5);
                // button click event
                button.Click += new RoutedEventHandler(deleteNoteAsync);

                // making the stkpanel a child of the border
                innerBorder.Child = innerStkpnl;

                // adding the innerBorder as a child of the outerStkpnl
                outerStkpnl.Children.Add(innerBorder);
                // adding the button as a child of the outerStkpnl
                outerStkpnl.Children.Add(button);

                // add the new stack panel to the stack panel on the xaml
                viewtaggedStkPnl.Children.Add(outerBorder);
            }// for

            // displaying the tagged notes
            viewtaggedbtnSearch.Visibility = Visibility.Collapsed;
            viewtaggedtxbxSearch.Visibility = Visibility.Collapsed;
            viewtaggedtxblkError.Visibility = Visibility.Collapsed;
            viewtaggedbtnOpenMenu.Visibility = Visibility.Collapsed;
            viewtaggedbtnOpenMenuView.Visibility = Visibility.Visible;
            viewtaggedScrollViewer.Visibility = Visibility.Visible;
            viewtaggedStkPnl.Visibility = Visibility.Visible;
        }// PrintTaggedNotes

        // SETTINGS
        // open menu click event
        private void settingsbtnOpenMenu_Click(object sender, RoutedEventArgs e) {
            hideSettings();
            showMenu();
        }// settingsbtnOpenMenu_Click

        // colour scheme click events
        private void settingsColourDefault_Click(object sender, RoutedEventArgs e) {
            this.applicationMainColour = (Windows.UI.Color)this.Resources["SystemAccentColor"];
            this.applicationSecondaryColour = (Windows.UI.Color)this.Resources["SystemAccentColorLight2"];
            applyColourScheme();
        }// settingsColourDefault_Click
        private void settingsColour1_Click(object sender, RoutedEventArgs e) {
            // setting the font colour to white
            this.fontColour = Windows.UI.Colors.White;
            setColour(Windows.UI.Colors.Black, Windows.UI.Colors.Gray);
        }// settingsColour1_Click
        private void settingsColour2_Click(object sender, RoutedEventArgs e) {
            // setting the font colour to white
            this.fontColour = Windows.UI.Colors.White;
            setColour(Windows.UI.Colors.Black, Windows.UI.Colors.Red);
        }// settingsColour1_Click
        private void settingsColour3_Click(object sender, RoutedEventArgs e) {
            // setting the font colour to black
            this.fontColour = Windows.UI.Colors.Black;
            setColour(Windows.UI.Colors.Navy, Windows.UI.Colors.LightBlue);
        }// settingsColour1_Click
        private void settingsColour4_Click(object sender, RoutedEventArgs e) {
            // setting the font colour to black
            this.fontColour = Windows.UI.Colors.Black;
            setColour(Windows.UI.Colors.Purple, Windows.UI.Colors.Pink);
        }// settingsColour1_Click
        private void settingsColour5_Click(object sender, RoutedEventArgs e) {
            // setting the font colour to black
            this.fontColour = Windows.UI.Colors.Black;
            setColour(Windows.UI.Colors.Navy, Windows.UI.Colors.Gray);
        }// settingsColour1_Click

        // font scheme click events
        private void settingsFontSizeSmall_Click(object sender, RoutedEventArgs e) {
            this.fontSize = -5;
            applyFontScheme();
        }// settingsFontSizeSmall_Click
        private void settingsFontSizeMedium_Click(object sender, RoutedEventArgs e) {
            this.fontSize = 0;
            applyFontScheme();
        }// settingsFontSizeMedium_Click
        private void settingsFontSizeLarge_Click(object sender, RoutedEventArgs e) {
            this.fontSize = 5;
            applyFontScheme();
        }// settingsFontSizeLarge_Click
        private void settingsFontFamilyArial_Click(object sender, RoutedEventArgs e) {
            this.fontFamily = new FontFamily("Arial");
            applyFontScheme();
        }// settingsFontFamilyArial_Click
        private void settingsFontFamilyCalibri_Click(object sender, RoutedEventArgs e) {
            this.fontFamily = new FontFamily("Calibri");
            applyFontScheme();
        }// settingsFontFamilyCalibri_Click
        private void settingsFontFamilyTimes_Click(object sender, RoutedEventArgs e) {
            this.fontFamily = new FontFamily("Times New Roman");
            applyFontScheme();
        }// settingsFontFamilyTimes_Click
        private void settingsFontFamilyGeorgia_Click(object sender, RoutedEventArgs e) {
            this.fontFamily = new FontFamily("Georgia");
            applyFontScheme();
        }// settingsFontFamilyGeorgia_Click
        private void settingsFontFamilyVerdana_Click(object sender, RoutedEventArgs e) {
            this.fontFamily = new FontFamily("Verdana");
            applyFontScheme();
        }// settingsFontFamilyVerdana_Click

        // SHOW AND HIDE METHODS
        // these methods will be used to set the visibility
        // value of objects displayed to the user and the
        // header text displayed to the user
        private void showMenu() {
            // changing the header text
            headerText.Text = "Note Tracker";

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
            headerText.Text = "Add Note";

            // setting all the add note items to visible
            addnotestkpnlTags.Visibility = Visibility.Visible;
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
            // reset the input fields
            resetAddNoteFields();

            // setting all the add Note items to collapsed 
            // which does not show the element or reserve space for it
            addnotestkpnlTags.Visibility = Visibility.Collapsed;
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
            headerText.Text = "View Notes";

            // setting all the add note items to visible
            viewnotestxblkError.Visibility = Visibility.Visible;
            viewNotesScrollViewer.Visibility = Visibility.Visible;
            viewNotesStkPnl.Visibility = Visibility.Visible;
            viewnotesbtnOpenMenu.Visibility = Visibility.Visible;
        }// hideViewNotes
        private void hideViewNotes() {
            // resetting the stackPanel
            viewNotesStkPnl.Children.Clear();

            // setting all the add Note items to collapsed 
            // which does not show the element or reserve space for it
            viewnotestxblkError.Visibility = Visibility.Collapsed;
            viewNotesScrollViewer.Visibility = Visibility.Collapsed;
            viewNotesStkPnl.Visibility = Visibility.Collapsed;
            viewnotesbtnOpenMenu.Visibility = Visibility.Collapsed;
        }// hideViewNotes
        private void showViewTaggedNotes() {
            // changing the header text
            headerText.Text = "View Tagged Notes";

            viewtaggedbtnOpenMenu.Visibility = Visibility.Visible;
            viewtaggedbtnSearch.Visibility = Visibility.Visible;
            viewtaggedtxbxSearch.Visibility = Visibility.Visible;
            viewtaggedbtnOpenMenuView.Visibility = Visibility.Collapsed;
        }// showViewTaggedNotes
        private void hideViewTaggedNotes() {
            viewtaggedbtnOpenMenu.Visibility = Visibility.Collapsed;
            viewtaggedbtnSearch.Visibility = Visibility.Collapsed;
            viewtaggedtxbxSearch.Visibility = Visibility.Collapsed;
            viewtaggedtxblkError.Visibility = Visibility.Collapsed;
            viewtaggedScrollViewer.Visibility = Visibility.Collapsed;
            viewtaggedStkPnl.Visibility = Visibility.Collapsed;
            viewtaggedbtnOpenMenuView.Visibility = Visibility.Collapsed;
        }// hideViewTaggedNotes
        private void showSettings() {
            // changing the header text
            headerText.Text = "Settings";

            settingsbtnOpenMenu.Visibility = Visibility.Visible;
            settingsStkpnl.Visibility = Visibility.Visible;
        }// showSettings
        private void hideSettings() {
            settingsbtnOpenMenu.Visibility = Visibility.Collapsed;
            settingsStkpnl.Visibility = Visibility.Collapsed;
        }// hideSettings

        // method to reset all the input fields to blank
        private void resetAddNoteFields() {
            // set all the input fields to blank
            addnotetxbxTitle.Text = "";
            addnotetxbxTag1.Text = "";
            addnotetxbxTag2.Text = "";
            addnotetxbxTag3.Text = "";
            addnotetxbxTag4.Text = "";
            addnotetxbxNote.Text = "";
        }// resetAddNoteFields

        // COLOUR AND FONT SCHEMES
        // setting the application colour scheme
        public void setColour(Windows.UI.Color maincolour, Windows.UI.Color secondarycolour) {
            this.applicationMainColour = maincolour;
            this.applicationSecondaryColour = secondarycolour;
            // applying the colour scheme
            applyColourScheme();
        }// setColour
        // applying the colour scheme to all objects in the application
        public void applyColourScheme() {
            // header
            headerBorder.Background = new SolidColorBrush(this.applicationMainColour);
            // setting the font colour to stop text colour matching background colour
            headerText.Foreground = new SolidColorBrush(this.fontColour);
            // menu
            menubtnAddNote.Background = new SolidColorBrush(this.applicationSecondaryColour);
            menubtnExitApp.Background = new SolidColorBrush(this.applicationSecondaryColour);
            menubtnSettings.Background = new SolidColorBrush(this.applicationSecondaryColour);
            menubtnViewNotes.Background = new SolidColorBrush(this.applicationSecondaryColour);
            menubtnViewTaggedNotes.Background = new SolidColorBrush(this.applicationSecondaryColour);
            // addnote
            addnotebtnaddNote.Background = new SolidColorBrush(this.applicationSecondaryColour);
            addnotebtnOpenMenu.Background = new SolidColorBrush(this.applicationSecondaryColour);
            // viewnotes
            viewnotesbtnOpenMenu.Background = new SolidColorBrush(this.applicationSecondaryColour);
            // viewtaggednotes
            viewtaggedbtnOpenMenu.Background = new SolidColorBrush(this.applicationSecondaryColour);
            viewtaggedbtnOpenMenuView.Background = new SolidColorBrush(this.applicationSecondaryColour);
            viewtaggedbtnSearch.Background = new SolidColorBrush(this.applicationSecondaryColour);
            // settings
            settingsBrdr1.Background = new SolidColorBrush(this.applicationSecondaryColour);
            settingsBrdr2.Background = new SolidColorBrush(this.applicationSecondaryColour);
            settingsBrdr3.Background = new SolidColorBrush(this.applicationSecondaryColour);
            settingsbtnOpenMenu.Background = new SolidColorBrush(this.applicationSecondaryColour);
        }// applyColourScheme
        // this method applies the font scheme to all objects
        public void applyFontScheme() {
            // header
            headerText.FontSize = 60 + this.fontSize;
            headerText.FontFamily = this.fontFamily;
            // menu
            menubtnAddNote.FontSize = 18 + this.fontSize;
            menubtnAddNote.FontFamily = this.fontFamily;
            menubtnExitApp.FontSize = 18 + this.fontSize;
            menubtnExitApp.FontFamily = this.fontFamily;
            menubtnSettings.FontSize = 18 + this.fontSize;
            menubtnSettings.FontFamily = this.fontFamily;
            menubtnViewNotes.FontSize = 18 + this.fontSize;
            menubtnViewNotes.FontFamily = this.fontFamily;
            menubtnViewTaggedNotes.FontSize = 18 + this.fontSize;
            menubtnViewTaggedNotes.FontFamily = this.fontFamily;
            // add note
            addnotebtnaddNote.FontSize = 18 + this.fontSize;
            addnotebtnaddNote.FontFamily = this.fontFamily;
            addnotebtnOpenMenu.FontSize = 18 + this.fontSize;
            addnotebtnOpenMenu.FontFamily = this.fontFamily;
            addnotetxblkError.FontSize = 18 + this.fontSize;
            addnotetxblkError.FontFamily = this.fontFamily;
            // view notes
            viewnotesbtnOpenMenu.FontFamily = this.fontFamily;
            viewnotesbtnOpenMenu.FontSize = 18 + this.fontSize;
            viewnotestxblkError.FontFamily = this.fontFamily;
            viewnotestxblkError.FontSize = 18 + this.fontSize;
            // view tagged notes
            viewtaggedbtnOpenMenu.FontFamily = this.fontFamily;
            viewtaggedbtnOpenMenu.FontSize = 18 + this.fontSize;
            viewtaggedbtnSearch.FontFamily = this.fontFamily;
            viewtaggedbtnSearch.FontSize = 18 + this.fontSize;
            viewtaggedtxblkError.FontFamily = this.fontFamily;
            viewtaggedtxblkError.FontSize = 18 + this.fontSize;
            viewtaggedbtnOpenMenuView.FontFamily = this.fontFamily;
            viewtaggedbtnOpenMenuView.FontSize = 18 + this.fontSize;
            // settings
            settingsbtnOpenMenu.FontFamily = this.fontFamily;
            settingsbtnOpenMenu.FontSize = 18 + this.fontSize;
            settingsColourTxtblk.FontFamily = this.fontFamily;
            settingsColourTxtblk.FontSize = 25 + this.fontSize;
            settingsFontFamilyTxtblk.FontFamily = this.fontFamily;
            settingsFontFamilyTxtblk.FontSize = 25 + this.fontSize;
            settingsFontSizeTxtblk.FontFamily = this.fontFamily;
            settingsFontSizeTxtblk.FontSize = 25 + this.fontSize;
        }// applyFontScheme

        // SAVE AND LOADING SETTINGS
        // asynchronous method that saves the current settings by writing them to a file
        public async Task saveSettings() {
            // sending the current settings to the SettingsWriter constructor
            SettingsWriter sw = new SettingsWriter(this.applicationMainColour, this.applicationSecondaryColour, this.fontColour, this.fontFamily.Source.ToString(), this.fontSize);
            // call the asynchronous method to write the settings to file
            await sw.writeToFileAsync();
        }// saveSettings

        // a method that loads the saved settings by reading them in from file
        // sets the scheme member variables and then applies the settings
        public void loadSettings() {
            SettingsReader sr = new SettingsReader();
            try {
                // read in the file
                sr.readSettingsFile();
                // setting the font and colour schemes
                this.fontColour = sr.fontColour;
                this.fontSize = sr.fontSize;
                this.fontFamily = new FontFamily(sr.fontFamily);
                this.applicationMainColour = sr.applicationMainColour;
                this.applicationSecondaryColour = sr.applicationSecondaryColour;
            } catch {
                // if the file cannot be found or read
                // setting the default application colour and font
                this.applicationMainColour = (Windows.UI.Color)this.Resources["SystemAccentColor"];
                this.applicationSecondaryColour = (Windows.UI.Color)this.Resources["SystemAccentColorLight2"];
                this.fontColour = Windows.UI.Colors.Black;
                this.fontFamily = new FontFamily("Arial");
                this.fontSize = 0;
            }// try/catch
        }// loadSettings

        // DELETE NOTE
        // to delete first get the note position
        // then update the notes array without that note at position n
        // then overwrite the notes.txt file with the updated notes array
        public async void deleteNoteAsync(object sender, RoutedEventArgs e) {
            Note[] updatedNotes = new Note[this.amountOfNotes - 1];
            // getting a handle on the sender button
            Button button = sender as Button;
            // getting the position of the button based on its name
            int buttonPosition = Int32.Parse(button.Name);
            int counter = 0;

            // loops throught all the notes
            for (int i = 0; i < this.amountOfNotes; i++) {
                // if the button to delete is not found add to the updated array
                if (buttonPosition != i) {
                    updatedNotes[counter] = notes[i];
                    counter++;
                }// if
            }// for

            // create a new noteWriter object
            NoteWriter nw = new NoteWriter();
            
            // await writing the notes to file
            await nw.updateNotesFile(updatedNotes, counter);

            // when deleted hide view notes and show the menu
            hideViewNotes();
            showMenu();
        }// deleteNote
    }// MainPage
}// NoteApplication