Feature: Edit Profile Page

Background: User is registerd, user is logged in.

Scenario: User accesses Profile page
Given 
When User clicks show profile button 
Then User is directed to their profile page, user's id is retrieved internally
Then User is directed to their profile page

Scenario: User tries to access a profile that isn't theirs
Given user knows the ID of another profile that is valid
When user manually enters the id into URL: /LPProfiles/Edit?id=5
Then User is redirected back to their profile, access denail system prevents user from accessing other's profile internally

Scenario: Profile Edit
Given User clicks the profile settings button at the top of the page
When User changes items in profile
Then User clicks save, and changes are stored