Feature: Make the LandingPad profile pretty and editable be the user
As a LandingPad user I would like to edit and maintain my profile account, so that I may personalize my page
General Case

Scenario: User wants to change their profile information
Given User wants to update their profile page
When The user clicks on the edit button on the profile page
Then The user should be able to edit all their information that is displayed

Scenario: User is on the Profile page
Given A user is on the Profile page
When the Website has loaded
Then The profile page is displaying a different layout then that of the rest of the website