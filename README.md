
# Welcome to the SquareOne Site 

This Readme will contain information and links to information on LandingPad and Penfolio. To view SquareOnes logo click [here](./Docs/SquareOneLogoV1.0.jpg).

**Disclaimer:** This is a senior project for Western Oregon computer science Software Engineering classes of Winter and Spring terms of 2018. 

## The Founding Members

**Aaron Earl**

Contact : aearl16@wou.edu

* [View Resume](./Docs/Resumes/AaronResume.jpg)

* [View Portfolio]()

* Computer Science major
* Entrepreneur minor

**Melissa Calawa**

Contact : mcalawa15@wou.edu

* [View Resume](./Docs/Resumes/MelissaResume.jpg)

* [View Portfolio]()

* English Post-Bach
* Computer Science major

**Rahevin Slade**

Contact : rpotterclark15@wou.edu

* [View Resume](./Docs/Resumes/RahevinResume.jpg)

* [View Portfolio](https://rahevinslade.github.io/)

* Dual Computer Science and Mathematic major


## Our Vision

Our project provides a web application called LandingPad that centralizes bringing multiple social media platforms into one place. Unlike Hootsuite and Everypost that offer a similar package, LandingPad will display all news feeds on a single page format. Users will also be able to edit how they prefer to view their newsfeed, from endless scroll feed to next page formatting. Another one of these features include a writing platform, that allows writers to create, share, and receive feedback from friends which diverges from apps like Wattpad and Medium. Finally, users will be able to create a customizable experience by modifying options for profiles, connections, and display. LandingPad will save users time to catch up on all their news feeds on the most popular social media sites.

## Our Motto

Back to the basics until we get it right!

## Our song

[Back to Basics by 4 Strings](https://www.youtube.com/watch?v=vy__TNYgar4)

## Naming Conventions for Programming

* C# and MVC Naming Schemes please refer to MSDN Documentation: [MSDN Styleguide](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
* For other Naming conventions examples please visit and use [Wiki Naming convention](https://en.wikipedia.org/wiki/Naming_convention_(programming)#C_and_C++)

## Rules for Joining

* Must be respectful, do not plagiarize or alter code without administrative approval.
* Do not change file structure without recieving permission from one of the founding members.
* Use XML comments, a shortcut that will auto comment out a piece is "CTRL+K" followed by  "CTRL+C".

## Tools and Procedures

Agile Methodoligies (Scrum)
Visual Studio 2017
Bitbucket (with Git)
MVC 5
ASP.Net
Bootstrap 3

## Links and Resources

[Rich Text Editor](https://code.tutsplus.com/tutorials/using-squire-a-lightweight-html5-rich-text-editor--cms-22934)

Link to LandingPad on Azure [here](https://landingpad.azurewebsites.net/)

Link to LandingPad's Visual Studio Team Services [here](https://squareonelandingpad.visualstudio.com/LandingPad/LandingPad%20Team/_backlogs?level=Epics&_a=backlog)

SquareOne bitbucket [here](https://bitbucket.org/aearl16/squareone)

## Git Commands

* First create your own bitbucket, then go to the the SquareOne bitbucket overview [page](https://bitbucket.org/aearl16/squareone) and click Fork.  

* Now create a folder and accesses git, now clone your repo with 
```
git clone "your repo's url"
```
then,
```
git fetch && git checkout Dev
```

* Adding a remote upstream
```
git remote add upstream https://"your name" @bitbucket.org/aearl16/squareone.git
```
To make sure everything is correct use 
```
git remote -v
```
Should display,
```
origin  https://bitbucket.org/"your name"/"your repo's name".git (fetch)
origin  https://bitbucket.org/"your name"/"your repo's name".git (push)
upstream        https://"your name"@bitbucket.org/aearl16/squareone.git (fetch)
upstream        https://"your name"@bitbucket.org/aearl16/squareone.git (push)

```

## When making a change perform the following each time you make a change

* To update your repo to the current version, be sure to be on a Dev branch then,
```
git pull upstream Dev
```

* Make sure to create a new branch after you update, then checkout your new branch,
```
git branch "your branches name"
```
```
git checkout "your branches name"
```

* Now you may make your adjustments or add ons, then you may view which files with,

```
git status
```

* To add and commite your changes to your repo you must, be sure you add a comment in quotes after the "-m" when commiting,
```
git add.
git commit -m "summary of what you did"
```
Now push it to your repo

```
git push
git push --set-upstream origin "your branches name"
```

* Now enter your bitbucket repo, and click on the "Pull request" button

* Click "create a pull request"

* Be sure that "your branches name" is merging with SquareOne's "Dev" branch, include a description of all the changes or add ons you created, check the box that states "close "your branches name" after the pull request is merged" 

* Click "Create pull request", your done, Thanks for you assistance!

~ Powered by SquareOne 2018

 ----