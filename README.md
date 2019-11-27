# reblGreen.AI.JohnE5 #

JohnE5 is a text/document classification or taxonomization classifier library for .NET and is written in C#. While JohnE5 is primarily designed for text classification, it is quite versatile in the sense that strings passed to the classifier as tokens can be in any form and will be categorized using standard methods. This means that you can train a classification model on integers, hashes, etc... Experimentation is key!



This project is open source so please feel free to contribute suggestions, make modifications and pull requests back to the repository.
___

### What and why is reblGreen.AI.JohnE5? ###

reblGreen.AI.JohnE5 is an [MIT license](https://tldrlegal.com/license/mit-license) .NET Standard 2.0 C# Class Library which offers an open-source system for creating/training classification models and categorizing documents or text objects.

Its name is derrived from the original project developer John Earnshaw, and is based on the Johnny 5 character from the Short Circuit movie series. The reasoning behind this is that text classification works similarly to the movie clip where Johhny 5 reads books to learn, "input, more input!".

### Weighted Classification ###

Neive Bayes classification algorithm uses a binary approach usually to train only 2 cattegories. This may work well for classifying in a boolean mannor such as true or false, good or bad, male or female, happy or sad, etc...

The concept behind the weighted classification algorithm used in JohnE5 works with floating point values rather a 1/0 binary approach. It could be similar to the way in which quantum computing works, in the sense that there are more than 2 states.

The more a token is found in a category, the stronger the token becomes when calculating classification. Common words are calculated with a diluted ranking for appearing across many categories which automatically creates a "stop word" system where common tokens have less weight.

A good example of the way that this works is to suggest "Soccer" and "Football", much of the content used to train these categories will most deffinately contain the words "football", "goal", "stadium", etc... Due to this, these tokens will be diluted to 50% of their weight across both categories and would require the existence of a unique or series of unique words to more accurately classify between these two categories. For example, unique words to "Soccer" category may be things like "saved", "goalkeeper", "penalty", etc.. and subsequently words like "helmet" for "Football" category.

___

### How do I get set up? ###

Take a look at the [reblGreen.AI.JohnE5.TestGUI](https://github.com/reblGreen/reblGreen.AI.JohnE5/blob/master/reblGreen.AI.JohnE5.TestGUI/frmTest.cs) project in the source code. Set this project as the "start up porject" in Visual studio for a GUI which enables you to see how training and classification works.

1. Input text into the text box in the application.
2. Press the "Classify" button.
3. Press the "Add Category" button and create a new classification category.
4. Ensure the "train" checkbox is selected on the category you which to train on the input text.
5. Press the "Train" button.
6. Repeat for more classification categories changing the input text each time.

If you press the "Classify" button a second time on the same input text, you should see the trained category appear in the categories list. This will display the accuracy probability converted into a percentatge to the top right of the category box.

As you train the model with more categories, the text at the top of the example training application may change to tell you which categories have a less token association count and will suggest that you add text to the category containing the least classified tokens.

Since this is a multi-category training model, it is designed for categories to be added in series and in a linear fashion. This means that training a single category to the maximum level before moving on to train a new category will impact the probability of accuracy for the categories trained prior. It is much more accurate to train a category with a single piece of text and then move on to the next category, itterating back to train the first category once all required categories are added.

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
