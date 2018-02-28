-- User Table
CREATE TABLE dbo.LPUser
(
	UserID INT Identity(1,1), -- TO CONNECT TO ASP.NET IDENTITY
	Email VARCHAR(MAX) NOT NULL,
	Birthdate DATETIME,
	FirstName VARCHAR(MAX) NOT NULL,
	LastName VARCHAR(MAX) NOT NULL,
	PhoneNumber VARCHAR(MAX),
	Username VARCHAR(MAX),
	CONSTRAINT [PK_dbo.Users] PRIMARY KEY (UserID)
);

-- Profile Table
CREATE TABLE dbo.LPProfile
(
	ProfileID INT IDENTITY(1,1) NOT NULL,
	UserID INT NOT NULL,
	LPDescription VarChar(120), 
	ProfilePhoto VARBINARY(MAX),
	DisplayRealName BIT NOT NULL DEFAULT 0, --Defualt off
	Friends INT,
	Followers INT,
	Writers INT,
	CONSTRAINT [PK_dbo.LPProfile] PRIMARY KEY (ProfileID),
	CONSTRAINT [FK_dbo.LPUser] FOREIGN KEY (UserID)
	REFERENCES dbo.LPUser (UserID)
);

-- Writings Table
CREATE TABLE dbo.Writing
(
	WritingID INT IDENTITY(1,1) NOT NULL,
	--FolderID INT,
	ProfileID INT NOT NULL,
	Title VARCHAR(MAX) NOT NULL,
	Document VARBINARY(MAX) NOT NULL, --Either varbvinary or xml. Not sure which would work better
	AddDate DATETIME NOT NULL,
	EditDate DATETIME,
	LikesOn BIT NOT NULL, -- THERE IS NO BOOLEAN DATA TYPE
	CommentsOn BIT NOT NULL,
	CritiqueOn BIT NOT NULL,
	DocType VARCHAR(MAX) NOT NULL,
	DescriptionText VARCHAR(MAX) NOT NULL
	CONSTRAINT [PK_dbo.Writing] PRIMARY KEY (WritingID),
	--CONSTRAINT [FK_dbo.FolderID] FOREIGN KEY (FolderID) --key added but left out until folder table is added
	--REFERENCES dbo.Folder (FolderID)
);

-- Pseudonym Table
CREATE TABLE dbo.Pseudonym
(
	PseudonymID INT IDENTITY(1,1) NOT NULL,
	ProfileID INT NOT NULL,
	Pseudonym VARCHAR(MAX) NOT NULL
	CONSTRAINT [PK_dbo.PseudonymID] PRIMARY KEY (PseudonymID),
	CONSTRAINT [FK_dbo.LPProfile] FOREIGN KEY (ProfileID)
	REFERENCES dbo.LPProfile (ProfileID),
);

INSERT INTO dbo.LPUser ( Email, Birthdate, FirstName, LastName, PhoneNumber, Username) VALUES
('dude@dude.com', '2010-04-12 12:00', 'Dude', 'Crush', '555-555-5555', 'RandomDude01'),
( 'saltshaker@oldnsalty.net', '1999-09-09 12:00','Phil', 'Forrest', '555-555-5555', ''),
( 'thestanza@gc.org','1978-06-09 12:00', 'George', 'Castanzna', '', 'TheBubbleBoy');

INSERT INTO dbo.LPProfile(UserID,LPDescription, ProfilePhoto, DisplayRealName, Friends, Followers, Writers) VALUES
(1,'I like to ride bikes', NULL, 0, 1, 1, 1),
(2,'I dont like to ride bikes', NULL, 0, 1, 1, 1),
(3,'', NULL, 0, 1, 1, 1);

INSERT INTO dbo.Writing (ProfileID, Title, Document, AddDate, EditDate, LikesOn, CommentsOn, CritiqueOn, DocType, DescriptionText) VALUES
(1, 'Lord of the Things', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 0, 0, 'DOCX', 'A humorous play on lord of the rings'),
(2, 'Ballad of The Trees', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 1, 1, 'RTF', 'Ballad About Trees'),
(3, 'Hokey Folk Tales', CONVERT(VARBINARY(MAX), 'ABCD'), 4/10/1991, GETDATE(), 1, 1, 1, 'ODT', 'A collection of old forgotten tales: second edition');

INSERT INTO dbo.Pseudonym (ProfileID, Pseudonym) VALUES
(1, 'ComedyClubbed'),
(1, 'FunnyMan'),
(1, 'DoomsDayDumb'),
(2, 'CrustyCrab'),
(2, 'RustyRed'),
(3, 'Treed'),
(3, 'JustGeorge'),
(3, 'NoPirates'),
(3, 'FestivusFreak');