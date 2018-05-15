Feature: Upload Edit
    User shoule be able to upload a file
    After the file is uploaded the file is converted to html for use in the Penfolio editor
    The only files that need conversion are DOCX, DOC, and PDF

Scenario: Upload .DOCX
    Given User is logged it and has a document prepared for upload
    When user selects upload with edit option
    Then user gets prompted to upload a file 
    Then and fills out the information, the file is then converted to html and uploaded into the database

Scenario: Upload .DOC
    Given User is logged it and has a document prepared for upload
    When user selects upload with edit option
    Then user gets prompted to upload a file 
    Then and fills out the information, the file is then converted to html and uploaded into the database

Scenario: Upload .PDF
    Given User is logged it and has a document prepared for upload
    When user selects upload with edit option
    Then user gets prompted to upload a file 
    Then and fills out the information, the file is then converted to html and uploaded into the database