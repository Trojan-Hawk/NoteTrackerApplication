# 		Mobile Application Project 2018

##	Student Name:	Timothy Cassidy
##	Student ID:	G00333333


This project is developed using VisualStudio2017 IDE, using both xaml and c# languages to
create a UWP(Universal Windows Platform) application. The application is a note tracker,
it handles the addition and deletion of notes, the application can also either display all
notes or search for specific notes based on a tag string attached to it.

The application contains a basic uwp application skeleton with the MainPage.xaml and the 
MainPage.xaml.cs edited and the addition of five more c# files. These are Note.cs, 
NoteReader.cs, NoteWriter.cs, SettingsReader.cs and SettingsWriter.cs. Below is a short
description of what each of these files do, for more detail on these read the comments
which break down each individual part or the project.

##	MainPage.xaml
The MainPage.cs contains the main skeleton for the project. Each element on this page has
a visability attribute which is hidden or shown based the button selected. This gives the
user the effect of page navigation.

##	MainPage.xaml.cs
This c# file is linked to the MainPage.cs, It accesses the various elements on the xaml page
and alters, changes or removes elements dynamically.

##	Note.cs
This c# file is a note object, instances can be made of this object based on this.

##	NoteReader.cs
This c# file reads in a file in local storage, Notes.txt, and creates Note objects based 
on the content in the file. It also handles the creation of the Notes.txt file if it does
not exist, this prevents a fileNotFound Exception.

##	NoteWriter.cs
This c# file handles the creation, deletion and updating of the Notes.txt file based on
what the user has selected.

##	SettingsReader.cs
This c# file handles the reading in of the Settings.txt file, then it alters the application
colour, font style and font size.

##	SettingsWriter.cs
This c# file handles tthe creation and updating of the Settings.txt file based on the users
selected preferance.
