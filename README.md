# reblGreen.AI.JohnE5 #

JohnE5 is a text/document classification or taxonomization classifier library for .NET and is written in C#. While JohnE5 is primarily designed for text classification, it is quite versatile in the sense that strings passed to the classifier as tokens can be in any form and will be categorized using standard methods. This means that you can train a classification model on integers, hashes, etc... Experimentation is key!



This project is open source so please feel free to contribute suggestions, make modifications and pull requests back to the repository.
___

### What and why is reblGreen.AI.JohnE5? ###

reblGreen.AI.JohnE5 is an [MIT license](https://tldrlegal.com/license/mit-license) .NET Standard 2.0 C# Class Library which offers an open-source system for creating/training classification models and categorizing documents or text objects.

Its name is derrived from the original project developer John Earnshaw, and is based on the Johnny 5 character from the Short Circuit movie series. The reasoning behind this is that text classification works similarly to the movie clip where Johhny 5 reads books to learn, "input, more input!".


___

### How do I get set up? ###

Take a look at the [reblGreen.AI.JohnE5.TestGUI](https://github.com/reblGreen/reblGreen.AI.JohnE5/blob/master/reblGreen.AI.JohnE5.TestGUI/frmTest.cs) project in the source code. Set this project as the "start up porject" in Visual studio for a GUI which enables you to see how training and classification works.

1. Input text into the text box in the application.
2. Press the "Classify" button.
3. Press the "Add Category" button and create a new classification category.
4. Ensure the "train" checkbox is selected on the category you which to train on the input text.
5. Press the "Train" button.
6. Repeat for more classification categories changing the input text each time.

If you press the "Classify" button a second time on the same input text, you should see the trained category appear in the categories list. This will display the accuracy probability 

The core project and examples will be well documented and if you get stuck or have any questions, please contact us and we'll be glad to help out.

For further documentation please see the [repository wiki](https://github.com/reblGreen/reblGreen.AI.JohnE5/wiki).
___

### Contribution guidelines ###

* Fork [reblGreen.AI.JohnE5](https://github.com/reblGreen/reblGreen.AI.JohnE5), make some changes, make a pull request. Simple!
* Code will be reviewed when a pull request is made.
___

### Who do I talk to? ###

* reblGreen.AI.JohnE5 repo owner via message or the [issues board](https://github.com/reblGreen/reblGreen.AI.JohnE5/issues).
* Or contact us via our website at [reblgreen.com](https://reblgreen.com/).
___

### License ###

* [The MIT License (MIT)](https://tldrlegal.com/license/mit-license) - You are free to use reblGreen.AI.JohnE5 in commercial projects and modify/redistribute the source code provided the copyright notice is not removed.
* If you use reblGreen.AI.JohnE5 in your own project we would love to hear about it, so drop us a line (and even a credit to reblGreen.AI.JohnE5 in your project if you feel like being really generous). We would be very happy to hear about your experiences using our reblGreen.AI.JohnE5 class library in your projects and any suggestions you may have for us to make it better.
