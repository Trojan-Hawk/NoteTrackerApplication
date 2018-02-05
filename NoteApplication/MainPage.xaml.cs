using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace NoteApplication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Note[] notes;
        private int amountOfNotes;
        private Windows.UI.Color applicationMainColour;
        private Windows.UI.Color applicationSecondaryColour;
        private Windows.UI.Color fontColour = Windows.UI.Colors.Black;
        private Windows.UI.Xaml.Media.FontFamily fontFamily;
        private int fontSize;

        public MainPage()
        {
            this.InitializeComponent();

            loadSettings();

            // applying the colour scheme
            applyColourScheme();
            // applying the font scheme
            applyFontScheme();

            showMenu();
            hideAddNote();
            hideViewNotes();
            hideViewTaggedNotes();
            hideSettings();
        }// MainPage

        // MAIN MENU
        // Add Note main menu option
        private void menubtnAddNote_Click(object sender, RoutedEventArgs e)
        {
            hideMenu();
            showAddNote();
        }// menubtnAddNote_Click
        private void menubtnViewNotes_Click(object sender, RoutedEventArgs e)
        {
            hideMenu();
            showViewNotes();
            getNotes();
        }// menubtnViewNotes_Click
        private void menubtnViewTaggedNotes_Click(object sender, RoutedEventArgs e)
        {
            hideMenu();
            showViewTaggedNotes();
        }// menubtnViewTaggedNotes_Click
        private void menubtnSettings_Click(object sender, RoutedEventArgs e)
        {
            hideMenu();
            showSettings();
        }// menubtnSettings_Click
        private void menubtnExitApp_Click(object sender, RoutedEventArgs e)
        {
            // save the settings
            // setting the saveSettings Task to a task variable
            Task task = saveSettings();
            // waiting until the task completes
            task.Wait(1000);
            // after the task has complete exit the app
            Application.Current.Exit();
        }// menubtnExitApp_Click

        // ADD NOTE
        // Add Note exit to menu
        private void addnotebtnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            hideAddNote();
            showMenu();
        }// addnotebtnOpenMenu_Click
        // Add note button clicked
        private void addnotebtnaddNote_Click(object sender, RoutedEventArgs e)
        {
            if (addnotetxbxNote.Text.Length > 0 && addnotetxbxTitle.Text.Length > 0)
            {
                appendNote();
                hideAddNote();
                showMenu();
            }// if
            else
            {
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
        private void viewnotesbtnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            hideViewNotes();
            showMenu();
        }// viewnotesbtnOpenMenu_Click
        private void getNotes()
        {
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
        // populating the inner stackPanel with the note details
        // tags will only be printed if they are not null
        private void PrintNotes(Note[] n, int numOfNotes)
        {
            // setting the gloabal variable for the number of notes
            this.amountOfNotes = numOfNotes;

            // for loop that prints each note to the screen
            for (int i = 0; i < numOfNotes; i++)
            {
                StackPanel stkpnl = new StackPanel();

                Border myBorder = new Border();
                // setting the colour of the border
                myBorder.Background = new SolidColorBrush(this.applicationSecondaryColour);
                myBorder.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                // setting the curve of the corner
                myBorder.CornerRadius = new CornerRadius(10);
                // setting the border line thickness
                myBorder.BorderThickness = new Thickness(1);
                // setting the border padding
                myBorder.Padding = new Thickness(20);
                // setting the margin, the distance between each note when displayed
                myBorder.Margin = new Thickness(100, 0, 100, 10);

                // using a button to display the title
                Button button = new Button();
                // setting the background colour
                button.Background = new SolidColorBrush(this.applicationMainColour);
                button.Foreground = new SolidColorBrush(this.fontColour);
                button.Content = "Title: " + notes[i].Title;
                // setting the font family
                button.FontFamily = this.fontFamily;
                // setting the font size
                button.FontSize = 18 + this.fontSize;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                stkpnl.Children.Add(button);

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

                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Tags: " + tags;
                // setting the font family
                textBlock.FontFamily = this.fontFamily;
                // setting the font size
                textBlock.FontSize = 18 + this.fontSize;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                stkpnl.Children.Add(textBlock);

                // grid row and column span for contents display
                textBlock = new TextBlock();
                textBlock.Text = notes[i].Contents;
                // setting the font family
                textBlock.FontFamily = this.fontFamily;
                // setting the fot size
                textBlock.FontSize = 18 + this.fontSize;
                // setting the text to wrap if too long
                textBlock.TextWrapping = TextWrapping.Wrap;
                stkpnl.Children.Add(textBlock);

                // delete button
                button = new Button();
                // setting the background colour
                button.Background = new SolidColorBrush(this.applicationMainColour);
                button.Foreground = new SolidColorBrush(this.fontColour);
                button.Content = "Delete Note";
                // setting the delete note name
                button.Name = i.ToString();
                // setting the font family
                button.FontFamily = this.fontFamily;
                // setting the font size
                button.FontSize = 18 + this.fontSize;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.Click += new RoutedEventHandler(deleteNoteAsync);
                stkpnl.Children.Add(button);

                // making the stackpanel a child of the border
                myBorder.Child = stkpnl;

                // add the new stack panel to the stack panel on the xaml
                viewNotesStkPnl.Children.Add(myBorder);
            }// for
        }// printNotes

        // VIEW TAGGED NOTES
        private void viewtaggedbtnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            hideViewTaggedNotes();
            showMenu();
        }// viewtaggedbtnOpenMenu_Click
        private void viewtaggedbtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTag = viewtaggedtxbxSearch.Text;

            // if the searchTag has a value
            if (searchTag != null || searchTag.Length > 0)
            {
                getTaggedNotes(searchTag);
            }// if
            else
            {
                viewtaggedtxblkError.Visibility = Visibility.Visible;
                viewtaggedtxblkError.Text += "ERROR: Cannot search for an empty tag value!";
            }// else

        }// viewtaggedbtnSearch_Click
        private void getTaggedNotes(string searchTag)
        {
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
        private void PrintTaggedNotes(Note[] notes, int numOfNotes)
        {
            // setting the gloabal variable for the number of notes
            this.amountOfNotes = numOfNotes;

            // for loop that prints each note to the screen
            for (int i = 0; i < numOfNotes; i++)
            {
                StackPanel stkpnl = new StackPanel();

                Border myBorder = new Border();
                // setting the colour of the border
                myBorder.Background = new SolidColorBrush(this.applicationSecondaryColour);
                myBorder.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                // setting the curve of the corner
                myBorder.CornerRadius = new CornerRadius(10);
                // setting the border line thickness
                myBorder.BorderThickness = new Thickness(1);
                // setting the border padding
                myBorder.Padding = new Thickness(20);
                // setting the margin, the distance between each note when displayed
                myBorder.Margin = new Thickness(100, 0, 100, 10);

                // using a button to display the title
                Button button = new Button();
                // setting the background colour
                button.Background = new SolidColorBrush(this.applicationMainColour);
                button.Foreground = new SolidColorBrush(this.fontColour);
                button.Content = "Title: " + notes[i].Title;
                // setting the font family
                button.FontFamily = this.fontFamily;
                // setting the font size
                button.FontSize = 18 + this.fontSize;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                stkpnl.Children.Add(button);

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

                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Tags: " + tags;
                // setting the font family
                textBlock.FontFamily = this.fontFamily;
                // setting the font size
                textBlock.FontSize = 18 + this.fontSize;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                stkpnl.Children.Add(textBlock);

                // grid row and column span for contents display
                textBlock = new TextBlock();
                textBlock.Text = notes[i].Contents;
                // setting the font family
                textBlock.FontFamily = this.fontFamily;
                // setting the font size
                textBlock.FontSize = 18 + this.fontSize;
                // setting the text to wrap if too long
                textBlock.TextWrapping = TextWrapping.Wrap;
                stkpnl.Children.Add(textBlock);

                // making the stackpanel a child of the border
                myBorder.Child = stkpnl;

                // add the new stack panel to the stack panel on the xaml
                viewtaggedStkPnl.Children.Add(myBorder);
            }// for

            // displaying the tagged notes
            viewtaggedbtnSearch.Visibility = Visibility.Collapsed;
            viewtaggedtxbxSearch.Visibility = Visibility.Collapsed;
            viewtaggedtxblkError.Visibility = Visibility.Collapsed;
            viewtaggedScrollViewer.Visibility = Visibility.Visible;
            viewtaggedStkPnl.Visibility = Visibility.Visible;

        }// PrintTaggedNotes

        // SETTINGS
        private void settingsbtnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            hideSettings();
            showMenu();
        }// settingsbtnOpenMenu_Click
        private void settingsColourDefault_Click(object sender, RoutedEventArgs e)
        {
            this.applicationMainColour = (Windows.UI.Color)this.Resources["SystemAccentColor"];
            this.applicationSecondaryColour = (Windows.UI.Color)this.Resources["SystemAccentColorLight2"];
            applyColourScheme();
        }// settingsColourDefault_Click
        private void settingsColour1_Click(object sender, RoutedEventArgs e)
        {
            // setting the font colour to white
            this.fontColour = Windows.UI.Colors.White;
            setColour(Windows.UI.Colors.Black, Windows.UI.Colors.Gray);
        }// settingsColour1_Click
        private void settingsColour2_Click(object sender, RoutedEventArgs e)
        {
            // setting the font colour to white
            this.fontColour = Windows.UI.Colors.White;
            setColour(Windows.UI.Colors.Black, Windows.UI.Colors.Red);
        }// settingsColour1_Click
        private void settingsColour3_Click(object sender, RoutedEventArgs e)
        {
            // setting the font colour to black
            this.fontColour = Windows.UI.Colors.Black;
            setColour(Windows.UI.Colors.Navy, Windows.UI.Colors.LightBlue);
        }// settingsColour1_Click
        private void settingsColour4_Click(object sender, RoutedEventArgs e)
        {
            // setting the font colour to black
            this.fontColour = Windows.UI.Colors.Black;
            setColour(Windows.UI.Colors.Purple, Windows.UI.Colors.Pink);
        }// settingsColour1_Click
        private void settingsColour5_Click(object sender, RoutedEventArgs e)
        {
            // setting the font colour to black
            this.fontColour = Windows.UI.Colors.Black;
            setColour(Windows.UI.Colors.Navy, Windows.UI.Colors.Gray);
        }// settingsColour1_Click
        private void settingsFontSizeSmall_Click(object sender, RoutedEventArgs e)
        {
            this.fontSize = -5;
            applyFontScheme();
        }// settingsFontSizeSmall_Click
        private void settingsFontSizeMedium_Click(object sender, RoutedEventArgs e)
        {
            this.fontSize = 0;
            applyFontScheme();
        }// settingsFontSizeMedium_Click
        private void settingsFontSizeLarge_Click(object sender, RoutedEventArgs e)
        {
            this.fontSize = 5;
            applyFontScheme();
        }// settingsFontSizeLarge_Click
        private void settingsFontFamilyArial_Click(object sender, RoutedEventArgs e)
        {
            this.fontFamily = new FontFamily("Arial");
            applyFontScheme();
        }// settingsFontFamilyArial_Click
        private void settingsFontFamilyCalibri_Click(object sender, RoutedEventArgs e)
        {
            this.fontFamily = new FontFamily("Calibri");
            applyFontScheme();
        }// settingsFontFamilyCalibri_Click
        private void settingsFontFamilyTimes_Click(object sender, RoutedEventArgs e)
        {
            this.fontFamily = new FontFamily("Times New Roman");
            applyFontScheme();
        }// settingsFontFamilyTimes_Click
        private void settingsFontFamilyGeorgia_Click(object sender, RoutedEventArgs e)
        {
            this.fontFamily = new FontFamily("Georgia");
            applyFontScheme();
        }// settingsFontFamilyGeorgia_Click
        private void settingsFontFamilyVerdana_Click(object sender, RoutedEventArgs e)
        {
            this.fontFamily = new FontFamily("Verdana");
            applyFontScheme();
        }// settingsFontFamilyVerdana_Click

        // SHOW AND HIDE METHODS
        private void showMenu()
        {
            // changing the header text
            headerText.Text = "Note Tracker";

            // setting all the menu items to visible
            menubtnAddNote.Visibility = Visibility.Visible;
            menubtnViewNotes.Visibility = Visibility.Visible;
            menubtnViewTaggedNotes.Visibility = Visibility.Visible;
            menubtnSettings.Visibility = Visibility.Visible;
            menubtnExitApp.Visibility = Visibility.Visible;
        }// showMenu
        private void hideMenu()
        {
            // setting all the menu items to collapsed
            // which does not show the element or reserve space for it
            menubtnAddNote.Visibility = Visibility.Collapsed;
            menubtnViewNotes.Visibility = Visibility.Collapsed;
            menubtnViewTaggedNotes.Visibility = Visibility.Collapsed;
            menubtnSettings.Visibility = Visibility.Collapsed;
            menubtnExitApp.Visibility = Visibility.Collapsed;
        }// hideMenu
        private void showAddNote()
        {
            // changing the header text
            headerText.Text = "Add Note";

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
        private void hideAddNote()
        {
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
        private void showViewNotes()
        {
            // changing the header text
            headerText.Text = "View Notes";

            // setting all the add note items to visible
            viewnotestxblkError.Visibility = Visibility.Visible;
            viewNotesScrollViewer.Visibility = Visibility.Visible;
            viewNotesStkPnl.Visibility = Visibility.Visible;
            viewnotesbtnOpenMenu.Visibility = Visibility.Visible;
        }// hideViewNotes
        private void hideViewNotes()
        {
            // resetting the stackPanel
            viewNotesStkPnl.Children.Clear();

            // setting all the add Note items to collapsed 
            // which does not show the element or reserve space for it
            viewnotestxblkError.Visibility = Visibility.Collapsed;
            viewNotesScrollViewer.Visibility = Visibility.Collapsed;
            viewNotesStkPnl.Visibility = Visibility.Collapsed;
            viewnotesbtnOpenMenu.Visibility = Visibility.Collapsed;
        }// hideViewNotes
        private void showViewTaggedNotes()
        {
            // changing the header text
            headerText.Text = "View Tagged Notes";

            viewtaggedbtnOpenMenu.Visibility = Visibility.Visible;
            viewtaggedbtnSearch.Visibility = Visibility.Visible;
            viewtaggedtxbxSearch.Visibility = Visibility.Visible;
        }// showViewTaggedNotes
        private void hideViewTaggedNotes()
        {
            viewtaggedbtnOpenMenu.Visibility = Visibility.Collapsed;
            viewtaggedbtnSearch.Visibility = Visibility.Collapsed;
            viewtaggedtxbxSearch.Visibility = Visibility.Collapsed;
            viewtaggedtxblkError.Visibility = Visibility.Collapsed;
            viewtaggedScrollViewer.Visibility = Visibility.Collapsed;
            viewtaggedStkPnl.Visibility = Visibility.Collapsed;
        }// hideViewTaggedNotes
        private void showSettings()
        {
            // changing the header text
            headerText.Text = "Settings";

            settingsbtnOpenMenu.Visibility = Visibility.Visible;
            settingsStkpnl.Visibility = Visibility.Visible;
        }// showSettings
        private void hideSettings()
        {
            settingsbtnOpenMenu.Visibility = Visibility.Collapsed;
            settingsStkpnl.Visibility = Visibility.Collapsed;
        }// hideSettings

        private void resetAddNoteFields()
        {
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
        public void setColour(Windows.UI.Color maincolour, Windows.UI.Color secondarycolour)
        {
            this.applicationMainColour = maincolour;
            this.applicationSecondaryColour = secondarycolour;
            // applying the colour scheme
            applyColourScheme();
        }// setColour

        public void applyColourScheme()
        {
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
            viewtaggedbtnSearch.Background = new SolidColorBrush(this.applicationSecondaryColour);
            // settings
            settingsBrdr1.Background = new SolidColorBrush(this.applicationSecondaryColour);
            settingsBrdr2.Background = new SolidColorBrush(this.applicationSecondaryColour);
            settingsBrdr3.Background = new SolidColorBrush(this.applicationSecondaryColour);
            settingsbtnOpenMenu.Background = new SolidColorBrush(this.applicationSecondaryColour);
        }// applyColourScheme

        public void applyFontScheme()
        {
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
        public async Task saveSettings()
        {
            // sending the current settings to the SettingsWriter constructor
            SettingsWriter sw = new SettingsWriter(this.applicationMainColour, this.applicationSecondaryColour, this.fontColour, this.fontFamily.Source.ToString(), this.fontSize);
            // call the asynchronous method to write the settings to file
            await sw.writeToFileAsync();
        }// saveSettings
        public void loadSettings()
        {
            SettingsReader sr = new SettingsReader();
            try
            {
                // read in the file
                sr.readSettingsFile();
                // setting the font and colour schemes
                this.fontColour = sr.fontColour;
                this.fontSize = sr.fontSize;
                this.fontFamily = new FontFamily(sr.fontFamily);
                this.applicationMainColour = sr.applicationMainColour;
                this.applicationSecondaryColour = sr.applicationSecondaryColour;
            }
            catch
            {
                // if the file cannot be found or read
                // setting the default application colour and font
                this.applicationMainColour = Windows.UI.Colors.LightBlue;
                this.applicationSecondaryColour = Windows.UI.Colors.Gray;
                this.fontColour = Windows.UI.Colors.Black;
                this.fontFamily = new FontFamily("Arial");
                this.fontSize = 0;
            }// try/catch

        }// loadSettings

        // DELETE NOTE
        // to delete first get the note position
        // then update the notes array without that note at position n
        // then overwrite the notes.txt file with the updated notes array
        public async void deleteNoteAsync(object sender, RoutedEventArgs e)
        {
            Note[] updatedNotes = new Note[this.amountOfNotes - 1];
            // getting a handle on the sender button
            Button button = sender as Button;
            // getting the position of the button based on its name
            int buttonPosition = Int32.Parse(button.Name);
            int counter = 0;

            for (int i = 0; i < this.amountOfNotes; i++)
            {
                if (buttonPosition == i)
                {
                    // do nothing
                }// if
                else
                {
                    updatedNotes[counter] = notes[i];
                    counter++;
                }// else
            }// for

            NoteWriter nw = new NoteWriter();

            await nw.updateNotesFile(updatedNotes, counter);

            // when deleted hide view notes and show the menu
            hideViewNotes();
            showMenu();
        }// deleteNote

        // EDIT NOTE
        // overlay a stackpanel with the note details
        // have a textbox with a placeholder of the note titls
        // have textboxes with placeholders of the note tags
        // have a textbox with a placeholder of the note contents
        // have an update button 
        // have a cancel button
        public void editNote()
        {

        }// editNote
    }// MainPage
}// NoteApplication