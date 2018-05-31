Feature: Loading the LandingPad website should load a presentable login page
As a user, I would like a nice login page that is easy to read and makes sense to the website, so that I am not confused.

General Case
Scenario: User is on the Login page and wants to Sign in with 3rd party
Given The login page or register page is dislpay
When User selects Sign In with
Then a drop down menu will be presented so that the user may sign in with a third party outh

Scenario: User is on the login/Register page
Given A user is on the Login/Register page
When the Website has loaded
Then The login/register page is displaying a different layout then that of the rest of the website