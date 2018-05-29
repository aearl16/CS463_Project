Feature: Display Twitter Likes and search features in LandingPad
As a User, I would like a feature that allows me to view my likes from Twitter, so that I may see them through LandingPad without switching to a different tab

General Case
Scenario: User visits the Twitters likes page on LandingPad
Given User wants to view their Twitters likes
When The user clicks on the likes button in the dropdown menu for profile
Then A twitter likes widget is displayed providing that users likes

Scenario: User would like to search Twitter with a specific tag
Given A user would like to search Twitter with a tag
When The user has selected Search Twitter in te drop down menu for profile and user has entered something into the search box provided.
Then A widget will be display showing the returned results.