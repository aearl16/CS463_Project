Feature: Data Access
    Users should not be able to access files and data that isn't theirs
    Users should be forced to login before accessing any Data
    Users should have to authenticate with a captcha when a form is submitted

Scenario: Register
    Given register page
    When user enters registration information
    Then user uses captcha to prove they are not a robot prior to submission

Scenario: Delete
    Given an id to a document that is to be deleted that isn't theirs
    When user attemts to delete the document
    Then Error page displays notifiying them that the document can't be deleted if they don't own information

Scenario: Edit
    Given an id to a document that isn't theirs
    When user attempts to access the document
    Then an error page displays notifying them that the document can't be edited because they don't own information

Scenario: Create
    Given users has filled out a Create Writing form
    When user submits the form
    Then user cannot submit it using another user's id

Scenario: Upload
    Given user has filled out and Upload Document form
    When user submits the form
    Then user cannot submit it using another user's id

Scenario: Download
    Given the id of a valid file to Download that isn't the user's
    When user attempts to download the file
    Then user is notified that they can't download files they don't own
