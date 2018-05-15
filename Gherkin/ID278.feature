Feature: Store files
    User should be able to store files onto the server of a defined type

Scenario: Store .ODT file
    Given user is logged in and has prepared a document for Upload
    And user has filled out Store submit form
    When user submits Store form
    Then .ODT file is converted to binary stream and uploaded into the server

Scenario: Store .PDF file
    Given user is logged in and has prepared a document for Upload
    And user has filled out Store submit form
    When user submits Store form
    Then .PDF file is converted to binary stream and uploaded into the server
    
Scenario: Store .DOC file
    Given user is logged in and has prepared a document for Upload
    And user has filled out Store submit form
    When user submits Store form
    Then .DOC file is converted to binary stream and uploaded into the server

Scenario: Store .DOCX file
    Given user is logged in and has prepared a document for Upload
    And user has filled out Store submit form
    When user submits Store form
    Then .DOCX file is converted to binary stream and uploaded into the server

Scenario: Store .HTML file
    Given user is logged in and has prepared a document for Upload
    And user has filled out Store submit form
    When user submits Store form
    Then .HTML file is converted to binary stream and uploaded into the server

Scenario: Validation
    Given user is logged in
    And is on the main feed page
    When user looks for previously uploaded documents
    Then an entry on the main feed page should be displayed for the previously uploaded document

Scenario: Re-Upload
    Given user is logged in and wants to edit previously uploaded file
    And user has downloaded the file
    And user has edited the file
    When user wants to re-upload the file, user should have access to a form where the file can be overwritten
    Then the file gets re-uploaded to the database and overwrites the old file