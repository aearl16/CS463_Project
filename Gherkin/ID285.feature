Feature: Newsfeed
    Twitter and Penfolio recent items are displayed on the main page

Scenario: User Accesses Main Feed Page
    Given user is logged in
    When user selects or is redirected to main feed page
    Then Twitter and Penfolio items should display in order by date created