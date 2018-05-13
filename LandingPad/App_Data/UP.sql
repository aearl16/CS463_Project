-- User Table
CREATE TABLE dbo.LPUser
(
	UserID INT Identity(1,1), -- TO CONNECT TO ASP.NET IDENTITY
	Email VARCHAR(MAX) NOT NULL,
	Birthdate DATETIME,
	GivenName VARCHAR(MAX),
	Surname VARCHAR(MAX),
	PhoneNumber VARCHAR(MAX),
	Username VARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_dbo.Users] PRIMARY KEY (UserID)
);

--Twitter Table
CREATE TABLE dbo.Twitter --Same Twitter data to access through project and use creds to provide information
(
	TwitterID INT Identity(1,1) NOT NULL,
	UserID Int NOT Null,	
	Date DateTime Not NUll,
	EndDate DateTime Not Null,
	TwName VARCHAR(60),
	TwTag VARCHAR(60),
	TwOauth Varchar(MAX),
	TwVOauth Varchar(MAX)
	Constraint [PK_dbo.Twitter] Primary Key (TwitterID),
	constraint [FK_dbo.LPUserTwiter] Foreign key (UserID)
	References dbo.LPUser (UserID)
);

-- AccessPermission Table
CREATE TABLE dbo.AccessPermission
(
	AccessPermissionID INT IDENTITY(1,1) NOT NULL,
	WritingID INT,
	ProfileID INT,
	PseudonymID INT,
	PublicAccess BIT NOT NULL,
	FriendAccess BIT NOT NULL,
	PublisherAccess BIT NOT NULL,
	MinorAccess BIT NOT NULL,
	CONSTRAINT [PK_dbo.AccessPermission] PRIMARY KEY (AccessPermissionID)
);

-- Profile Table
CREATE TABLE dbo.LPProfile
(
	ProfileID INT IDENTITY(1,1) NOT NULL,
	AccessPermissionID INT NOT NULL,
	UserID INT NOT NULL,
	LPDescription VarChar(120), 
	ProfilePhoto VARBINARY(MAX),
	DisplayRealName BIT NOT NULL DEFAULT 0, --Default off
	Followers INT,
	Writers INT,
	CONSTRAINT [PK_dbo.LPProfile] PRIMARY KEY (ProfileID),
	CONSTRAINT [FK_dbo.AccessPermissionForProfile] FOREIGN KEY (AccessPermissionID)
	REFERENCES dbo.AccessPermission (AccessPermissionID)
	ON DELETE NO ACTION
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.LPUser] FOREIGN KEY (UserID)
	REFERENCES dbo.LPUser (UserID)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

--IndividualAccessGrant Table
CREATE TABLE dbo.IndividualAccessGrant
(
	IndividualAccessGrantID INT IDENTITY(1,1) NOT NULL,
	AccessPermissionID INT NOT NULL,
	GranteeID INT NOT NULL,
	CONSTRAINT [PK_dbo.IndividualAccessGrant] PRIMARY KEY (IndividualAccessGrantID),
	CONSTRAINT [FK_dbo.AccessPermissionForAccessGrant] FOREIGN KEY (AccessPermissionID)
	REFERENCES dbo.AccessPermission (AccessPermissionID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.ProfileIDForIndividualAccessGrant] FOREIGN KEY (GranteeID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE CASCADE
	ON UPDATE NO ACTION
);

--IndividualAccessRevoke Table
CREATE TABLE dbo.IndividualAccessRevoke
(
	IndividualAccessRevokeID INT IDENTITY(1,1) NOT NULL,
	AccessPermissionID INT NOT NULL,
	RevokeeID INT NOT NULL,
	CONSTRAINT [PK_dbo.IndividualAccessRevoke] PRIMARY KEY (IndividualAccessRevokeID),
	CONSTRAINT [FK_dbo.AccessPermissionForAccessRevoke] FOREIGN KEY (AccessPermissionID)
	REFERENCES dbo.AccessPermission (AccessPermissionID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.ProfileIDForIndividualAccessRevoke] FOREIGN KEY (RevokeeID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE CASCADE
	ON UPDATE NO ACTION
);

-- LPRole Table
CREATE TABLE dbo.LPRole
(
	RoleID INT IDENTITY(1,1) NOT NULL,
	RoleName VARCHAR(MAX) NOT NULL,
	SecondaryRoleName VARCHAR(MAX),
	CONSTRAINT [PK_dbo.LPRole] PRIMARY KEY (RoleID)
);

-- ProfileRole Table
CREATE TABLE dbo.ProfileRole
(
	ProfileRoleID INT IDENTITY(1,1) NOT NULL,
	ProfileID INT NOT NULL,
	RoleID INT NOT NULL,
	UseSecondaryRoleName BIT NOT NULL DEFAULT 0, --Default no
	CONSTRAINT [PK_dbo.ProfileRole] PRIMARY KEY (ProfileRoleID),
	CONSTRAINT [FK_dbo.ProfileIDForProfileRole] FOREIGN KEY (ProfileID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.RoleIDForProfileRole] FOREIGN KEY (RoleID)
	REFERENCES dbo.LPRole (RoleID)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

-- Pseudonym Table
CREATE TABLE dbo.Pseudonym
(
	PseudonymID INT IDENTITY(1,1) NOT NULL,
	ProfileID INT NOT NULL,
	AccessPermissionID INT NOT NULL,
	Pseudonym VARCHAR(MAX) NOT NULL
	CONSTRAINT [PK_dbo.Pseudonym] PRIMARY KEY (PseudonymID),
	CONSTRAINT [FK_dbo.LPProfile] FOREIGN KEY (ProfileID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.AccessPermissionForPseudonym] FOREIGN KEY (AccessPermissionID)
	REFERENCES dbo.AccessPermission (AccessPermissionID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

-- Friendship Table
CREATE TABLE dbo.Friendship
(
	FriendshipID INT IDENTITY(1,1) NOT NULL,
	FirstFriendID INT NOT NULL,
	SecondFriendID INT NOT NULL,
	FirstPseudonymID INT,
	SecondPseudonymID INT,
	AcceptDate DATETIME NOT NULL,
	CONSTRAINT [PK_dbo.Friendship] PRIMARY KEY (FriendshipID),
	CONSTRAINT [FK_dbo.FirstProfileIDForFriendship] FOREIGN KEY (FirstFriendID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.SecondProfileIDForFriendship] FOREIGN KEY (SecondFriendID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.FirstPseudonymIDForFriendship] FOREIGN KEY (FirstPseudonymID)
	REFERENCES dbo.Pseudonym (PseudonymID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.SecondPseudonymIDForFriendship] FOREIGN KEY (SecondPseudonymID)
	REFERENCES dbo.Pseudonym (PseudonymID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

--FriendRequest Table
CREATE TABLE dbo.FriendRequest 
(
	FriendRequestID INT IDENTITY(1,1) NOT NULL,
	RequesterProfileID INT NOT NULL,
	RequesteeProfileID INT NOT NULL,
	RequesterPseudonymID INT,
	RequesteePseudonymID INT,
	RequestDate DATETIME NOT NULL,
	CONSTRAINT [PK_dbo.FriendRequest] PRIMARY KEY (FriendRequestID),
	CONSTRAINT [FK_dbo.RequesterProfileID] FOREIGN KEY (RequesterProfileID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.RequesteeProfileID] FOREIGN KEY (RequesteeProfileID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.RequesterPseudonymID] FOREIGN KEY (RequesterPseudonymID)
	REFERENCES dbo.Pseudonym (PseudonymID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.RequesteePseudonymID] FOREIGN KEY (RequesteePseudonymID)
	REFERENCES dbo.Pseudonym (PseudonymID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

-- Writing Table
CREATE TABLE dbo.Writing
(
	WritingID INT IDENTITY(1,1) NOT NULL,
	--FolderID INT,
	ProfileID INT NOT NULL,
	AccessPermissionID INT NOT NULL,
	Title VARCHAR(MAX) NOT NULL,
	Document VARBINARY(MAX) NOT NULL, --Either varbvinary or xml. Not sure which would work better
	AddDate DATETIME NOT NULL,
	EditDate DATETIME,
	LikesOn BIT NOT NULL, -- THERE IS NO BOOLEAN DATA TYPE
	CommentsOn BIT NOT NULL,
	CritiqueOn BIT NOT NULL,
	UsePseudonymsInAdditionToUsername BIT NOT NULL DEFAULT 0, --Default no
	DocType VARCHAR(MAX) NOT NULL,
	DescriptionText VARCHAR(MAX) NOT NULL,
	WritingFileName VARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_dbo.Writing] PRIMARY KEY (WritingID),
	CONSTRAINT [FK_dbo.ProfileID] FOREIGN KEY (ProfileID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.AccessPermissionForWriting] FOREIGN KEY (AccessPermissionID)
	REFERENCES dbo.AccessPermission (AccessPermissionID) 
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
	--CONSTRAINT [FK_dbo.FolderID] FOREIGN KEY (FolderID) --key added but left out until folder table is added
	--REFERENCES dbo.Folder (FolderID)
);

-- WritingPseudonym Table
CREATE TABLE dbo.WritingPseudonym
(
	WritingPseudonymID INT IDENTITY(1,1) NOT NULL,
	WritingID INT NOT NULL,
	PseudonymID INT NOT NULL,
	CONSTRAINT [PK_dbo.WritingPseudonym] PRIMARY KEY (WritingPseudonymID),
	CONSTRAINT [FK_dbo.WritingIDforWP] FOREIGN KEY (WritingID)
	REFERENCES dbo.Writing (WritingID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.Pseudonym] FOREIGN KEY (PseudonymID)
	REFERENCES dbo.Pseudonym (PseudonymID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

--FormatTag Table
CREATE TABLE dbo.FormatTag
(
	FormatID INT IDENTITY(1,1) NOT NULL,
	FormatName VARCHAR(MAX) NOT NULL,
	Explanation VARCHAR(MAX),
	CONSTRAINT [PK_dbo.FormatTag] PRIMARY KEY (FormatID)
);

--AltFormatName Table
CREATE TABLE dbo.AltFormatName
(
	AltFormatNameID INT IDENTITY(1,1) NOT NULL,
	FormatID INT NOT NULL,
	AltName VARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_dbo.AltFormatName] PRIMARY KEY (AltFormatNameID),
	CONSTRAINT [FK_dbo.FormatTagID] FOREIGN KEY (FormatID)
	REFERENCES dbo.FormatTag (FormatID)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

--FormatCategory Table
CREATE TABLE dbo.FormatCategory
(
	FormatCategoryID INT IDENTITY(1,1) NOT NULL,
	FormatID INT NOT NULL,
	ParentID INT NOT NULL,
	SecondaryParentID INT,
	CONSTRAINT [PK_dbo.FormatCategory] PRIMARY KEY (FormatCategoryID),
	CONSTRAINT [FK_dbo.FormatIDforCategory] FOREIGN KEY (FormatID)
	REFERENCES dbo.FormatTag (FormatID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.ParentIDforFormat] FOREIGN KEY (ParentID)
	REFERENCES dbo.FormatTag (FormatID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.SecondaryParentIDforFormat] FOREIGN KEY (SecondaryParentID)
	REFERENCES dbo.FormatTag (FormatID) 
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

--WritingFormat Table
CREATE TABLE dbo.WritingFormat 
(
	WritingFormatID INT IDENTITY(1,1) NOT NULL,
	WritingID INT NOT NULL,
	FormatID INT NOT NULL,
	CONSTRAINT [PK_dbo.WritingFormat] PRIMARY KEY (WritingFormatID),
	CONSTRAINT [FK_dbo.WritingIDforWF] FOREIGN KEY (WritingID)
	REFERENCES dbo.Writing (WritingID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.FormatIDforWF] FOREIGN KEY (FormatID)
	REFERENCES dbo.FormatTag (FormatID)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

--GenreTag Table
CREATE TABLE dbo.GenreTag
(
	GenreID INT IDENTITY(1,1) NOT NULL,
	GenreName VARCHAR(MAX) NOT NULL,
	Explanation VARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_dbo.GenreTag] PRIMARY KEY (GenreID)
);

--AltGenreName Table
CREATE TABLE dbo.AltGenreName
(
	AltGenreNameID INT IDENTITY(1,1) NOT NULL,
	GenreID INT NOT NULL,
	AltName VARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_dbo.AltGenreName] PRIMARY KEY (AltGenreNameID),
	CONSTRAINT [FK_dbo.GenreIDForAltGenreName] FOREIGN KEY (GenreID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

--GenreCategory Table
CREATE TABLE dbo.GenreCategory
(
	GenreCategoryID INT IDENTITY(1,1) NOT NULL,
	GenreID INT NOT NULL,
	ParentID INT NOT NULL,
	SecondaryParentID INT,
	TertiaryParentID INT,
	CONSTRAINT [PK_dbo.GenreCategory] PRIMARY KEY (GenreCategoryID),
	CONSTRAINT [FK_dbo.GenreIDforCategory] FOREIGN KEY (GenreID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.ParentIDforGenre] FOREIGN KEY (ParentID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.SecondaryParentIDforGenre] FOREIGN KEY (SecondaryParentID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.TertiaryParentIDforGenre] FOREIGN KEY (TertiaryParentID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

--GenreFormat Table
CREATE TABLE dbo.GenreFormat
(
	GenreFormatID INT IDENTITY(1,1) NOT NULL,
	GenreID INT NOT NULL,
	ParentFormatID INT NOT NULL,
	ParentGenreID INT,
	CONSTRAINT [PK_dbo.GenreFormat] PRIMARY KEY (GenreFormatID),
	CONSTRAINT [FK_dbo.GenreIDforGenreFormat] FOREIGN KEY (GenreID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.ParentFormatID] FOREIGN KEY (ParentFormatID)
	REFERENCES dbo.FormatTag (FormatID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.ParentGenreIDforGenreFormat] FOREIGN KEY (ParentGenreID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

--WritingGenre Table
CREATE TABLE dbo.WritingGenre
(
	WritingGenreID INT IDENTITY(1,1) NOT NULL,
	WritingID INT NOT NULL,
	GenreID INT NOT NULL,
	CONSTRAINT [PK_dbo.WritingGenre] PRIMARY KEY (WritingGenreID),
	CONSTRAINT [FK_dbo.WritingIDforWG] FOREIGN KEY (WritingID)
	REFERENCES dbo.Writing (WritingID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.GenreIDforWG] FOREIGN KEY (GenreID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

--INSERT INTO dbo.LPUser ( Email, Birthdate, GivenName, Surname, PhoneNumber, Username) VALUES
--('dude@dude.com', '2010-04-12 12:00', 'Dude', 'Crush', '555-555-5555', 'RandomDude01'), --1
--( 'saltshaker@oldnsalty.net', '1999-09-09 12:00','Phil', 'Forrest', '555-555-5555', '100%Salt'), --2
--( 'thestanza@gc.org','1978-06-09 12:00', 'George', 'Castanzna', '', 'TheBubbleBoy'), --3
--('jsmith@penguin.com', '1966-03-14 12:00', 'Joe', 'Smith', '555-555-5555', 'PublisherJoeSmith'), --4
--('agent@literary.com', '2000-02-21 12:00', 'Lilah', 'Agent', '', 'LiteraryAgentLilahAgent'); --5

--INSERT INTO dbo.AccessPermission (WritingID, ProfileID, PseudonymID, PublicAccess, FriendAccess, PublisherAccess, MinorAccess) VALUES
--(NULL, 1, NULL, 0, 1, 1, 1), --dude@dude.com 1 
--(NULL, 2, NULL, 0, 0, 1, 0), --saltshaker@oldnsalty.net 2
--(NULL, 3, NULL, 1, 1, 1, 1), --thestanza@gc.org 3
--(1, NULL, NULL, 0, 0, 1, 1), --Lord of the Things 4
--(2, NULL, NULL, 1, 1, 1, 1), --Ballad of the Trees 5
--(3, NULL, NULL, 0, 1, 1, 0), --Hokey Folk Tales 6
--(NULL, 4, NULL, 1, 1, 1, 1), --jsmith@penguin.com 7
--(NULL, 5, NULL, 1, 1, 1, 0), --literary@agent.com 8
--(4, NULL, NULL, 0, 1, 0, 1), --The Test Song 9
--(NULL, NULL, 1, 1, 1, 1, 1), --ComedyClubbed 10
--(NULL, NULL, 2, 1, 1, 1, 1), --FunnyMan 11
--(NULL, NULL, 3, 1, 1, 1, 1), --DoomsDayDumb 12
--(NULL, NULL, 4, 1, 1, 1, 1), --CrustyCrab 13
--(NULL, NULL, 5, 1, 1, 1, 1), --RustyRed 14
--(NULL, NULL, 6, 1, 1, 1, 1), --Treed 15
--(NULL, NULL, 7, 1, 1, 1, 1), --JustGeorge 16
--(NULL, NULL, 8, 1, 1, 1, 1), --NoPirates 17
--(NULL, NULL, 9, 1, 1, 1, 1); --FestivusFreak 18

--INSERT INTO dbo.LPProfile(UserID, AccessPermissionID, LPDescription, ProfilePhoto, DisplayRealName) VALUES
--(1, 1, 'I like to ride bikes', NULL, 0), --dude@dude.com 1
--(2, 2, 'I dont like to ride bikes', NULL, 0), --saltshaker@oldnsalty.net 2
--(3, 3, '', NULL, 1), --thestanza@gc.org 3
--(4, 7, 'I am a publisher at Penguin Books.', NULL, 1), --jsmith@penguin.com 4
--(5, 8, 'My name is Lilah Agent and I''m a literary agent.', NULL, 1); --literary@agent.com 5

--INSERT INTO dbo.IndividualAccessGrant (AccessPermissionID, GranteeID) VALUES
--(4, 2); --Lord of the Things saltshaker@oldnsalty.net 1

--INSERT INTO dbo.IndividualAccessRevoke (AccessPermissionID, RevokeeID) VALUES
--(4, 4), --Lord of the Things jsmith@penguin.com 1
--(5, 1); --Ballad of the Trees dude@dude.com 2

INSERT INTO dbo.LPRole(RoleName, SecondaryRoleName) VALUES
('Writer', ''), --Writer 1
('Publisher', 'Literary Agent'); --Publisher or Literary Agent 2

--INSERT INTO dbo.ProfileRole(ProfileID, RoleID, UseSecondaryRoleName) VALUES
--(1, 1, 0), --dude@dude.com Writer 1
--(2, 1, 0), --saltshaker@oldnsalty.net Writer 2
--(3, 1, 0), --thestanza@gc.org Writer 3
--(4, 2, 0), --jmsmith@penguin.com Publisher 4
--(5, 2, 1); --agent@literary.com Literary Agent 5

--INSERT INTO dbo.Friendship(FirstFriendID, SecondFriendID, FirstPseudonymID, SecondPseudonymID) VALUES
--(1, 3, NULL, NULL), --dude@dude.com thestanza@gc.org 1
--(3, 1, NULL, NULL), --thestanza@gc.org dude@dude.com 2
--(1, 4, NULL, NULL), --dude@dude.com jsmith@penguin.com 3
--(4, 1, NULL, NULL), --jsmith@penguin.com dude@dude.com 4
--(2, 4, NULL, NULL), --saltshaker@oldnsalty.net jsmith@penguin.com 5
--(4, 2, NULL, NULL), --jsmith@penguin.com saltshaker@oldnsalty.net 6
--(2, 5, NULL, NULL), --saltshaker@oldnsalty.net agent@literary.com 7
--(5, 2, NULL, NULL); --agent@literary.com saltshaker@oldnsalty.net 8

--INSERT INTO dbo.Writing (ProfileID, AccessPermissionID, Title, Document, AddDate, EditDate, LikesOn, CommentsOn, CritiqueOn, UsePseudonymsInAdditionToUsername, DocType, DescriptionText, WritingFileName) VALUES
--(1, 4, 'Lord of the Things', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 0, 0, 0, '.DOCX', 'A humorous play on lord of the rings', 'Lord_of_the_Things'), --dude@dude.com 1
--(2, 5, 'Ballad of The Trees', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 1, 1, 0, '.RTF', 'Ballad About Trees', 'balladofthetrees'), --saltshaker@oldnsalty.net 2
--(3, 6, 'Hokey Folk Tales', CONVERT(VARBINARY(MAX), 'ABCD'), '1991-04-10', GETDATE(), 1, 1, 1, 1, '.ODT', 'A collection of old forgotten tales: second edition', 'forgottentales'), --thestanza@gc.org 3
--(1, 9, 'The Test Song', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 1, 0, 0, 0, '.HTML', 'I love tests; I love every kind of test.', 'everydayimtesting'); --dude@dude.com 4

--INSERT INTO dbo.Pseudonym (ProfileID, AccessPermissionID, Pseudonym) VALUES
--(1, 10, 'ComedyClubbed'), --dude@dude.com 1
--(1, 11, 'FunnyMan'), --dude@dude.com 2
--(1, 12, 'DoomsDayDumb'), --dude@dude.com 3
--(2, 13, 'CrustyCrab'), --saltshaker@oldnsalty.net 4
--(2, 14, 'RustyRed'), --saltshaker@oldnsalty.net 5
--(3, 15, 'Treed'), --thestanza@gc.org 6
--(3, 16, 'JustGeorge'), --thestanza@gc.org 7
--(3, 17, 'NoPirates'), --thestanza@gc.org 8
--(3, 18, 'FestivusFreak'); --thestanza@gc.org 9

--INSERT INTO dbo.WritingPseudonym (WritingID, PseudonymID) VALUES
--(1, 1), --Lord of the Things, ComedyClubbed 1
--(1, 2), --Lord of the Things, FunnyMan 2
--(2, 5), --Ballad of the Trees, RustyRed 3
--(3, 6), --Hokey Folk Tales, Treed 4
--(3, 7); --Hokey Folk Tales, JustGeorge 5

INSERT INTO dbo.FormatTag (FormatName, Explanation) VALUES
--Top category
('Poetry', 'Writing that uses meter and/or concrete form to enhance the impact of the language.'), --1
('Prose', 'The ordinary form of written language that lacks the metrical structure of poetry. If you aren''t sure, it''s probably prose.'), --2
('Script', 'A mix of dialogue and direction meant to be performed or incorporate other media into a final product like a comic or video game. Sometimes known as drama.'), --3

--Crosscategories that require the user to select two categories in order to show up
('Prose poetry', 'Poetry written in prose rather than verse, which preserves poetic qualities like short sentences and heightened imagery.'), --4
('Rhymed prose', 'Prose that is written in unmetrical rhymes.'), --5
('Uta monogatari', 'A type of literature originating in Japan that intersperses poetry with narrative prose.'), --6

--Subcategories that show up when the user selects Poetry
('Rhyming poetry', 'A poem that repeats a similar sound in two or more words across or within the lines of the poem.'), --7
('Unrhymed poetry', 'A poem that doesn''t have a rhyme scheme.'), --8

--Subcategories that show up when the user selects Prose
('Fiction prose', 'A piece of prose writing that tells a story derived from the creator''s imagination.'), --9
('Nonfiction prose', 'A piece of prose writing where the creator assumes good faith responsibility for the accuracy of the events, people, and information contained within.'), --10

--Subcategories that show up when the user selects Script; these don't have any categories under them
('Comic script', 'A document that uses detail to describe the dialogue and narrative of a comic. A script is typically used rather than a story board when the art and the writing of the comic are done by different people.'), --11
('Musical', 'A play or screenplay that combines traditional spoken dialogue with songs with lyrics that are relevant to the plot and help advance the narrative or develop the characters.'), --12
('One act', 'A short play with only a single act. Typically has limited or no scene or location changes.'), --13
('Opera', 'A type of drama in which all of the interactions between characters are sung.'), --14
('Play', 'A piece of writing meant to be performed on stage.'), --15
('Screenplay', 'A piece of writing meant to be acted, filmed (or animated), and edited into a final product like a TV show or movie.'), --16

--"Tercategories" that show up when the user selects one of the subcategories for Poetry
('Concrete poetry', 'A poem that displays one of its elements typographically through the rearrangement of letter within words or arranging words into a shape.'), --17
('Couplet', 'A two line poem or part of a poem that can be rhymed or unrhymed.'), --18
('Iambic pentameter', 'A metrical line structure of five sets of an unstressed syllable followed by a stressed syllable.'), --19
('Japanese poetry', 'A poem written in a form originating in Japan such as a haiku, senryu, or tanka. Doesn''t rhyme and generally uses a set number of syllables.'), --20
('Ode', 'A lengthy lyric poem with an elevated style and formal stanza structure.'), --21
('Sonnet', 'A fourteen-line poem that typically contains one or more conventional rhyme schemes.'), --22
('Quatrain', 'A four-line stanza or poem where lines 2 and 4 have a similar number of syllables and rhyme.'), --23

--"Tercategories" that show up when the user selects one of the subcategories for Prose
('Article', 'A short piece of writing such as might be found in a journalism publication that discusses a subject in an accessable format.'), --24
('Creative nonfiction', 'A piece of writing that uses literary techniques and styles to discuss factual information.'), --25
('Essay', 'A piece of writing in which the writer gives an argument on the essay''s subject.'), --26
('Personal journal', 'An informal piece of writing that covers the writer''s thoughts and impressions on events in their life.'), --27
('Technical writing', 'Writing intended to share information within a professional setting.'), --28

--"Teritems" that show up when the user selects one of the subcategories for Poetry; the difference between a tercategory and a teritem is that tercategories have items underneath them and teritems don't
('ABC', 'A five-line poem where lines 1-4 are phrases or clauses that begin with words in alphabetical order and line 5 is a complete sentence beginning with any letter.'), --29
('Acrostic', 'A poem where each line begins with a letter that spell a phrase or word when read in order.'), --30
('Ballade', 'A poem with three stanzas of seven, eight, or ten lines each followed by a final stanza of four or five lines. All four stanzas end with the same one line refrain.'), --31
('Canzone', 'A poem with five or six stanzas followed by a shorter ending stanza. Originated in medieval Italy.'), --32
('Cinquain', 'A five-line poem where line 1 is a one word title, line 2 is a two word title description, line 3 tells the action in three words, line 4 expresses the feeling in four words, and line 5 recalls the title in a single word.'), --33
('Free verse', 'A poem made up of rhymed or unrhymed lines that lack a fixed metrical pattern.'), --34
('Found poetry', 'A poem made up of words, phrases, and passages from other sources and re-framed through text alterations, rearrangement, or text and line additions.'), --35
('Limerick', 'A poem with five anapestic lines where lines 1, 2, and 5 rhyme and have seven to ten syllables and a consistent verbal rhythm that differs from the rhyme and consistent verbal rhythm found in the five-to-seven-syllable line 3 and line 4.'), --36
('List poetry', 'A poem made of a list of rhymed or unrhymed list of items or events.'), --37
('Rondeau', 'A lyric poem with ten or thirteen lines with two rhymes and an opening line that is repeated twice as a refrain. Originated in France.'), --38
('Sestina', 'A poem with six six-line stanzas and a three-line envoy. The final words of the first stanza get repeated in a variable order as ending lines of other stanzas and as part of the envoy.'), --39
('Sound poetry', 'Sometimes called “verse without words”, a sound poem uses the phonetics of human speech to form a poem. They are generally intended for performance.'), --40
('Terza Rima', 'A poem made up of three-line tercets with ten or eleven syllables a line.'), --41
('Villanelle', 'A nineteen-line poem made from five tercets and a concluding quatrain on two rhymes. The first and third lines from the first tercet are alternately repeated as the closing refrain for the succeeding stanzas and joined as the quatrain''s final couplet.'), --42

--"Teritems" that show up when the user selects one of the subcategories for Prose; the difference between a tercategory and a teritem is that tercategories have items underneath them and teritems don't
('Chapter', 'A subsection of a larger work of fictional prose narrative. Unlike a short story, a chapter does not tell a complete story on its own.'), --43
('Short story', 'A fictional prose narrative that is typically under 7,500 words.'), --44
('Novelette', 'A fictional prose narrative that is typically 7,500 to 17,499 words.'), --45
('Novella', 'A fictional prose narrative that is typically 17,500 to 39,999 words.'), --46
('Novel', 'A fictional prose narrative that is typically at least 40,000 words.'), --47

--Items that show up when the user selects one of the subcategories or one of the tercategories for Poetry
('Ballad', 'A poem with an often repeated refrain that tells a story in the style of a folk tale or legend.'), --48
('Blank verse', 'A poem written in unrhymed iambic pentameter.'), --49
('Ghazal', 'A lyrical poem of five to fifteen couplets of equal length that all express their own contained poetic thought. A rhyme established in the first couplet and continued in the second line of each connects the couplets. The form is of Arabian origin.'), --50
('Memoriam stanza', 'A quatrain written in iambic tetrameter with an abba rhyme scheme.'), --51
('Petrachan', 'A sonnet with fourteen lines made up of an octave with the rhyme scheme abbaabba followed by a sestet of cddcee or cdecde.'), --52
('Rhyme royal', 'A poem made of seven-line stanzas where the lines are in iambic pentameter.'), --53

--Items that show up when the user selects one of the subcategories or one of the tercategories for Prose
('Research paper', 'A piece of writing that presents new or compiled research on a topic and comes to some sort of conclusion based on it.'), --54
('Review', 'A piece of writing that discusses a place, product, service, or piece of media, judges its merits, and gives it a rating.'), --55

--Items that show up when the user selects one of the tercategories for Poetry
('Haiku', 'A type of poem originating in Japan that is made up of three unrhymed lines where the first and third lines are five morae (or syllables) each and the second line is seven. Features a seasonal word known as a kigo.'), --56
('Horatian ode', 'A short lyric poem made up of two- or four-lined stanzas with a common metrical pattern.'), --57
('Irregular ode', 'A type of ode characterized by irregularity of structure and verse and a lack of correspondence between parts. Cannot be classified as either a pindaric or Horatian ode.'), --58
('Italian sonnet', 'A sonnet made from an octave with an abbaabba rhyme scheme that is followed by six lines that have a rhyme scheme of cdecde or cdccdc.'), --59
('Pindaric ode', 'A poem that starts with a strophe, or two or more lines repeated as a unit, and is followed by an antistrophe that uses the same metrical pattern. The poem concludes with a summary line (known as an epode) in a different meter.'), --60
('Senryu', 'A Japanese poem type that has the same unrhymed three lines of five, seven, and five morae or syllables each found in a haiku. Unlike a haiku, a senryu will not have a seasonal word (kigo) and is about humanity rather than nature.'), --61
('Shakespearean sonnet', 'A fourteen-line sonnet made up of three quatrains with the rhyme scheme abab cdcd efef and a final couplet gg. Usually written in iambic pentameter'), --62
('Shape poetry', 'A type of concrete poem that is written in the shape or form of an object.'), --63
('Tanka', 'A five-line poem of Japanese origin that uses the syllable pattern of five, seven, five, seven, seven in its various lines.'), --64
('Visual poetry', 'A poem that arranges text, images, and symbols in a way that conveys the meaning of the work. A form of concrete poetry.'), --65

--Items that show up when the user selects one of the tercategories for Prose
('Blog post', 'A first person, informal account of something that the writer finds of interest.'), --66
('Descriptive essay', 'An essay in which the writer describes a person, place, object, or memory using evocative details.'), --67
('Documentation', 'Information on how a problem was solved, how an item works, or the steps required to complete a task.'), --68
('Expository essay', 'An essay that informs the reader about a topic in a fair and balanced manner.'), --69
('Literary analysis', 'A piece of writing that uses evidence from a text and outside sources to analyze an aspect of a piece of fiction.'), --70
('Narrative essay', 'An essay in which the writer shares a story about a real-life experience.'), --71
('News article', 'A piece of writing which gives an impartial account for a general audience by covering the facts about a recent news item.'), --72
('Opinion piece', 'A piece of writing where the author talks about their opinion on a topic, often in an impassioned way.'), --73
('Persuasive essay', 'An essay in which the writer takes a stance on an issue and attempts to use logic and arguments to convince the reader to agree with their conclusion.'), --74
('Self-help', 'A piece of writing in which the author attempts to provide the reader with information and advice that will allow them to help themselves.'), --75
('Textbook', 'A book for use in academic settings that aims to teach the reader a new skill or about a subject.'), --76
('Travelogue', 'An account of the writer''s travels to different locations.'); --77

INSERT INTO dbo.AltFormatName (FormatID, AltName) VALUES
(1, 'Verse'), --Poetry (1)
(3, 'Drama'), --Script (2)
(3, 'Performance'), --Script (3)
(6, 'Poem tale'), --Uta monogatari (4)
(20, 'Waka'), --Japanese poetry (5)
(25, 'Literary nonfiction'), --Creative nonfiction (6)
(25, 'Narrative nonfiction'), --Creative nonfiction (7)
(26, 'Article (journal)'), --Essay (8)
(26, 'Paper'), --Essay (9)
(26, 'Academic article'), --Essay (10)
(26, 'Scholarly article'), --Essay (11)
(27, 'Diary'), --Personal journal (12)
(44, 'Flash fiction'), --Short story (13)
(54, 'Academic paper'), --Research paper (14)
(54, 'Academic essay'), --Research paper (15)
(54, 'Scientific paper'), --Research paper (16)
(68, 'How-to'), --Documentation (17)
(68, 'Manual'), --Documentation (18)
(70, 'Literary criticism'), --Literary analysis (19)
(73, 'Editorial'), --Opinion piece (20)
(74, 'Argumentative essay'), --Persuasive essay (21)
(75, 'Self-improvement'), --Self-help (22)
(76, 'Coursebook'), --Textbook (23)
(76, 'Schoolbook'); --Textbook (24)

INSERT INTO dbo.FormatCategory(FormatID, ParentID, SecondaryParentID) VALUES
(4, 1, 2), --Prose poetry, Poetry, Prose 1
(4, 2, 1), --Prose poetry, Prose, Poetry 2
(5, 1, 2), --Rhymed prose, Poetry, Prose 3
(5, 2, 1), --Rhymed prose, Prose, Poetry 4
(6, 1, 2), --Uta monogatari, Poetry, Prose 5
(6, 2, 1), --Uta monogatari, Prose, Poetry 6
(7, 1, NULL), --Rhyming poetry, Poetry 7
(8, 1, NULL), --Unrhymed poetry, Poetry 8
(9, 2, NULL), --Fiction prose, Prose 9
(10, 2, NULL), --Nonfiction prose, Prose 10
(11, 3, NULL), --Comic script, Script 11
(12, 3, NULL), --Musical, Script 12
(13, 3, NULL), --One act, Script 13
(14, 3, NULL), --Opera, Script 14
(15, 3, NULL), --Play, Script 15
(16, 3, NULL), --Screenplay, Script 16
(17, 8, NULL), --Concrete poetry, Unrhymed poetry 17
(18, 7, NULL), --Couplet, Rhyming poetry 18
(18, 8, NULL), --Couplet, Unrhymed poetry 19
(19, 7, NULL), --Iambic pentameter, Rhyming poetry 20
(19, 8, NULL), --Iambic pentameter, Unrhymed poetry 21
(20, 8, NULL), --Japanese poetry, Unrhymed poetry 22
(21, 7, NULL), --Ode, Rhyming poetry 23
(21, 8, NULL), --Ode, Unrhymed poetry 24
(22, 7, NULL), --Sonnet, Rhyming poetry 25
(22, 8, NULL), --Sonnet, Unrhymed poetry 26
(23, 7, NULL), --Quatrain, Rhyming poetry 27
(24, 10, NULL), --Article, Nonfiction prose 28
(25, 10, NULL), --Creative nonfiction, Nonfiction prose 29
(26, 10, NULL), --Essay, Nonfiction prose 30
(27, 10, NULL), --Personal journal, Nonfiction prose 31
(28, 10, NULL), --Technical writing, Nonfiction prose 32
(29, 8, NULL), --ABC, Unrhymed poetry 33
(30, 8, NULL), --Acrostic, Unrhymed poetry 34
(31, 7, NULL), --Ballade, Rhyming poetry 35
(32, 7, NULL), --Canzone, Rhyming poetry 36
(33, 8, NULL), --Cinquain, Unrhymed poetry 37
(34, 7, NULL), --Free verse, Rhyming poetry 38
(34, 8, NULL), --Free verse, Unrhymed poetry 39
(35, 8, NULL), --Found poetry, Unrhymed poetry 40
(36, 7, NULL), --Limerick, Rhyming poetry 41
(37, 7, NULL), --List poetry, Rhyming poetry 42
(37, 8, NULL), --List poetry, Unrhymed poetry 43
(38, 7, NULL), --Rondeau, Rhyming poetry 44
(39, 8, NULL), --Sestina, Unrhymed poetry 45
(40, 7, NULL), --Sound poetry, Rhyming poetry 46
(40, 8, NULL), --Sound poetry, Unrhymed poetry 47
(41, 7, NULL), --Terza Rima, Rhyming poetry 48
(42, 7, NULL), --Villanelle, Rhyming poetry 49
(43, 9, NULL), --Chapter, Fiction prose 50
(44, 9, NULL), --Short story, Fiction prose 51
(45, 9, NULL), --Novelette, Fiction prose 52
(46, 9, NULL), --Novella, Fiction prose 53
(47, 9, NULL), --Novel, Fiction prose 54
(48, 7, NULL), --Ballad, Rhyming poetry 55
(48, 23, NULL), --Ballad, Quatrain 56
(49, 8, NULL), --Blank verse, Unrhymed poetry 57
(49, 19, NULL), --Blank verse, Iambic pentameter 58
(50, 7, NULL), --Ghazal, Rhyming poetry 59
(50, 18, NULL), --Ghazal, Couplet 60
(51, 7, NULL), --Memoriam stanza, Rhyming poetry 61
(51, 23, NULL), --Memoriam stanza, Quatrain 62
(52, 7, NULL), --Petrachan, Rhyming poetry 63
(52, 22, NULL), --Petrachan, Sonnet 64
(53, 7, NULL), --Rhyme royal, Rhyming poetry 65
(53, 19, NULL), --Rhyme royal, Iambic pentameter 66
(53, 21, NULL), --Rhyme royal, Ode 67
(54, 10, NULL), --Research paper, Nonfiction prose 68
(54, 24, NULL), --Research paer, Article 69
(54, 26, NULL), --Research paper, Essay 70
(55, 10, NULL), --Review, Nonfiction prose 71
(55, 24, NULL), --Review, Article 72
(55, 27, NULL), --Review, Personal journal 73
(56, 20, NULL), --Haiku, Japanese poetry 74
(57, 21, NULL), --Horatian ode, Ode 75
(58, 21, NULL), --Irregular ode, Ode 76
(59, 22, NULL), --Italian sonnet, Sonnet 77
(60, 21, NULL), --Pindaric ode, Ode 78
(61, 20, NULL), --Senryu, Japanese poetry 79
(62, 19, NULL), --Shakespearean sonnet, Iambic pentameter 80
(62, 22, NULL), --Shakespearean sonnet, Sonnet 81
(63, 17, NULL), --Shape poetry, Concrete poetry 82
(64, 20, NULL), --Tanka, Japanese poetry 83
(65, 17, NULL), --Visual poetry, Concrete poetry 84
(66, 24, NULL), --Blog post, Article 85
(66, 27, NULL), --Blog post, Personal journal 86
(67, 26, NULL), --Descriptive essay, Essay 87
(68, 28, NULL), --Documentation, Technical writing 88
(69, 26, NULL), --Expository essay, Essay 89
(70, 26, NULL), --Literary analysis, Essay 90
(71, 26, NULL), --Narrative essay, Essay 91
(72, 24, NULL), --News article, Article 92
(72, 26, NULL), --News article, Essay 93
(73, 24, NULL), --Opinion piece, Article 94
(73, 27, NULL), --Opinion piece, Personal journal 95
(74, 26, NULL), --Persuasive essay, Essay 96
(75, 24, NULL), --Self-help, Article 97
(75, 25, NULL), --Self-help, Creative nonfiction 98
(76, 28, NULL), --Textbook, Technical writing 99
(77, 24, NULL), --Travelogue, Article 100
(77, 25, NULL), --Travelogue, Creative nonfiction 101
(77, 27, NULL); --Travelogue, Personal journal 102

--INSERT INTO dbo.WritingFormat (WritingID, FormatID) VALUES
--(1, 2), --Lord of the Things, Prose
--(1, 9), --Lord of the Things, Fiction prose
--(1, 46), --Lord of the Things, Novella
--(2, 1), --Ballad of the Trees, Poetry
--(2, 7), --Ballad of the Trees, Rhyming poetry
--(2, 23), --Ballad of the Trees, Quatrain
--(2, 48), --Ballad of the Trees, Ballad
--(3, 1), --Hokey Folk Tales, Poetry
--(3, 2), --Hokey Folk Tales, Prose
--(3, 6), --Hokey Folk Tales, Uta monogatari
--(3, 7), --Hokey Folk Tales, Rhyming poetry
--(3, 9), --Hokey Folk Tales, Fiction prose
--(3, 36), --Hokey Folk Tales, Limerick
--(3, 44); --Hokey Folk Tales, Short story

INSERT INTO dbo.GenreTag (GenreName, Explanation) VALUES
--Top tier
('Fiction', 'A piece of writing that tells a story derived from the creator''s imagination.'), --1
('Nonfiction', 'A piece of writing where the creator assumes good faith responsibility for the accuracy of the events, people, and information contained within.'), --2

--Immediate children of fiction ONLY
('Drama', 'Fiction that is intended to be more serious than humerous in tone.'), --3
('Fan fiction', 'Writing that uses characters or a fictional setting belonging to another author.'), --4
('Historical fiction', 'A piece of fiction that takes place in the past.'), --5
('Pseudo-documentary', 'A film or video production that takes the form of a documentary but does not cover real events.'), --6
('Realistic fiction', 'Fiction covering present-day events that could actually happen in reality. Lacks supernatural or fantastic elements.'), --7
('Slice of life', 'A genre of fiction that covers the everyday events of the character''s lives. Slice of life narratives are generally character-driven rather than plot-driven.'), --8
('Speculative fiction', 'Fiction with futuristic or supernatural elements or which is intended to answer a “what if” question such as “What if steam-power maintained mainstream useage long enough for technology to become highly advanced?” or “What if magic was real?”'), --9
('Thriller', 'Fiction intended to give the one consuming it heightened feelings of suspense, excitement, surprise, anticipation, and anxiety.'), --10

--Immediate children of Drama, fiction ONLY
('Melodrama', 'A drama subgenre that strives for sensationalism and appeal to emotions over the creation of complex characters.'), --11
('Medical drama', 'A piece of fiction with events that center on a hospital, ambulance staff, or other medical environment.'), --12
('Soap opera', 'A serialized work of fiction with many characters that focuses on the emotional relationships between them, sometimes to the point of melodrama. Similar styles in other parts of the world include Latin America''s telenovelas, Filipino teleseryes, French Canadian téléromans, and Asian or Arabic dramas.'), --13

--Immediate children of Fan fiction, fiction ONLY
('Alternate universe', 'A type of fan fiction that takes the characters from an existing piece of work, puts them in a different setting or gives them different roles, and explores how this changes things.'), --14
('Crossover', 'A type of fanfiction that takes characters or settings from more than one preexisting piece and includes them in a single work.'), --15

--Immediate children of Historical fiction, fiction ONLY
('Docudrama', 'A dramatization of historical events that strives to be accurate where historical fact is clear, but allows the writer to use a combination of general period research and their imagination to fill in the places where there are gaps in the historical record. '), --16

--Immediate children of Speculative fiction, fiction ONLY
('Alternate history', 'A type of speculative fiction that explores how the world would be changed if one or more historical event had occured differently. Will sometimes involve time travel or multiple universes.'), --17
('Apocalyptic', 'A piece of fiction that documents the collapse of human civilization. May also be concerned with the end of the world.'), --18
('Dystopian', 'A type of speculative fiction that is set in a society that with oppressive rules and values that are counter to the author''s worldview.'), --19
('Fantasy', 'A type of speculative fiction where magical or supernatural elements are real and are a main influence on the plot, setting, or theme.'), --20
('Horror', 'A type of fiction intended to invoke fright or disgust. Frequently features supernatural themes, though this is not a requirement of the genre.'), --21
('Post-apocalyptic', 'A genre of fiction that takes place after some sort of catastrophic event that has led to the collapse of known civilization. The story will follow pockets of survivors as they attempt to navigate the utterly changed world.'), --22
('Science fiction', 'A genre of speculative fiction with a focus on advanced science and technology, time travel, spaceflight, or extraterrestrial life.'), --23
('Superhero', 'Works that follow the lives and adventures of costumed fighters with superhuman powers. The source of the superhuman powers can be either fantastic or scientific in origin.'), --24
('Utopian', 'Fiction that is set in the writer''s idea of an ideal society.'), --25

--Immediate children of Fantasy
('Fairy tale', 'A short story often targeted at children that has fantastic elements and is intended to provide a lesson or moral.'), --26
('Hard fantasy', 'A subgenre of fantasy that is characterized by a world with fantastic elements that is nevertheless rational and knowable.'), --27
('Heroic fantasy', 'A subgenre of fantasy characterized by the presence of characters with high ideals and the focus on the individual journey of a single or group of protagonists rather than quests that involve the fate of the world itself.'), --28
('High fantasy', 'A subgenre of fantasy set in a world other than our own. An emphasis is placed on themes of good and evil and events are often epic in scale..'), --29
('Magical realism', 'A type of fantasy that features supernatural or fantastic elements in a setting that is otherwise real-world or mundane.'), --30
('Urban fantasy', 'Fantasy characterized by the urban setting of its narrative. Usually overlaps with magic realism and often has a contemporary setting.'), --31

--Immediate children of Horror
('Gothic', 'A genre characterized by a combination of the elements of fear, horror, death, and gloom with elements of romanticism such as nature, individuality, and high emotion.'), --32
('Slasher', 'A subgenre of horror that is focused on the stalking and murder of a group of people by a violent and psychopathic individual.'), --33
('Supernatural', 'A subgenre of horror dealing with events caused by entities that lie outside of the natural world. Sometimes the events are presented in an ambiguous manner where there is an alternate psychological interpretation for them.'), --34

--Immediate children of Science fiction
('Hard science fiction', 'Science fiction that is characterized by its emphasis on scientific accuracy. All of the technology, phenomena, and scenerios in a hard science fiction story should be within the realm of theoretical possibility.'), --35
('Punk', 'A subgenre of science fiction with origins in the punk movement. Punk fiction is urban in setting and features a world built on a particular technology advanced to highly sophisticated levels. Individual subversion of a corrupt elite is a common theme and to reflect this, the societies of punk fiction are often dystopian in nature.'), --36
('Soft science fiction', 'Science fiction that is not scientifically accurate or which explores soft sciences such as anthropology, sociology, or psychology.'), --37
('Space opera', 'Science fiction that is set primarily in outer space and deals with themes of space warfare, melodramatic adventure, interplanetary battles, and risk and romance.'), --38

--Immediate children of Punk
('Atompunk', 'A subgenre of punk fiction with retro-futuristic technology and aesthetics relating to the period of history between 1945 and 1965. Explores the idea of advanced technologies powered by atomic energy.'), --39
('Biopunk', 'Focuses on near-future unintended consequences of the biotechnogy revolution such as human experimentation and misuse of biotechnologies for the purpose of social control or profiteering.'), --40
('Clockpunk', 'A subgenre of punk fiction with Renaissance-era aesthetics and technology based on the science of the era and premodern designs of individuals like Leonardo da Vinci. As the name would suggest, advanced technologies are often powered by clockwork.'), --41
('Cyberpunk', 'The original genre of punk, cyberpunk explores the underside of a society with advanced computer-based technologies. Has many similarities to hardboiled, noir, and postmodern fiction.'), --42
('Dieselpunk', 'A subgenre of punk fiction with retro-futuristic technology and aesthetics related to the period between World War I and the end of World War II.'), --43
('Steampunk', 'A subgenre of punk fiction with retro-futuristic technology and aesthetics of the Victorian era.'), --44

--Immediate child of BOTH Fantasy and Science fiction
('Science fantasy', 'A subgenre of speculative fiction that combines elements of both fantasy and science fiction.'), --45

--Immediate children of Thriller, fiction ONLY
('Conspiracy', 'A type of thriller in which a conspiracy, or a secret act which a group uses to obtain some sort of goal, is uncovered and explored by the protagonist.'), --46
('Psychological thriller', 'A type of thriller that focuses on the unstable or disillusioned psychological state of its characters. Often has a dissolving sense of reality.'), --47

--Immediate children of either fiction or nonfiction
('Action', 'Characterized by an emphasis on exciting action scenes.'), --48
('Comedy', 'A work meant to be humorous and make people laugh.'), --49
('Crime', 'A work that in some way deals with illegal activity or the justice system.'), --50
('Family saga', 'A work that chronicles the lives and actions of a single family or group of related and interconnected families over a period of time.'), --51
('Inspirational', 'Writing meant to uplift and encourage the reader or listener.'), --52
('Music', 'A piece of music or a piece of writing about music.'), --53
('Mystery', 'Characterized by the presence of a crime, death, or strange event of unknown cause or culprit, a mystery details the search to uncover the truth of what happened.'), --54
('Nature', 'A work concerned with plants, animals, and other natural creatures or phenomena.'), --55
('Philosophical', 'A work that centers around making sense of or inspiring thought on philosophical issues such as ethics and morals, the purpose of life, the role of society, etc.'), --56
('Political', 'A work centered around the process or theory of government and leadership.'), --57
('Religious', 'A work centered around or with themes of faith, theology, and religious doctrine.'), --58
('Romance', 'A work centered around the relationship and romantic love of two (or occasionally more) people. Not to be confused with Romanticism, which was a literary movement during the Romantic era from roughly 1800 to 1850.'), --59
('Tragedy', 'A work focusing on human suffering or a catastrophic event.'), --60

--Immediate children of Action, fiction or nonfiction
('Adventure', 'A work characterized by the presence of danger and excitement.'), --61

--Immediate children of Historical Fictition, Action, or Adventure, fiction ONLY
('Western', 'Fiction set in the Old American Western frontier from roughly the late 18th to late 19th centuries.'), --62

--Immediate children of Action or Adventure, fiction or nonfiction
('Survivalism', 'A work about survival under extreme conditions that can range from anything between subzero temperatures to nuclear fallout or zombie apocalypse.'), --63

--Immediate children of Action or Adventure or Drama, fiction or nonfiction
('Military', 'A work with a focus on armed forces, organized militias, battles, and other related subjects.'), --64
('War', 'A work about the organized, sustained conflict between two powers.'), --65

--Immediate child of Action or Adventure or Thriller, fiction or nonfiction
('Spy', 'A work centered around espionage, undercover work, or secret operations.'), --66

--Immediate children of Action or Adventure or Crime, fiction or nonfiction
('Gangster', 'A work centered around organized crime.'), --67

--Immediate children of Comedy, fiction ONLY
('Absurdist', 'A form of comedy that studies human behavior in pointless or philosophically absurb circumstances.'), --68
('Farce', 'A comedy with highly exaggerated, extravagant, and improbable situations.'), --69
('Parody', 'A work that is an imitation or spoof of another work for the purpose of commenting on, making fun of, or affectionately calling notice to aspects of the original.'), --70
('Sketch comedy', 'A series of short comedy scenes or vignettes, known as “sketches”, which commonly range from one to ten minutes each.'), --71

--Immediate children of Comedy or Pseudo-documentary, fiction ONLY
('Mockumentary', 'A comedic work of fiction that is presented as a documentary.'), --72

--Immediate children of Comedy or Political, fiction ONLY
('Satire', 'A work that uses comedy and sometimes exaggeration to draw attention to and ridicule vices, follies, abuses, and shortcomings with the intention of creating social change. Though they often use similar methods, satire is differentiated from parody in that satire has an intended message behind it and is always meant as criticism, whereas parodies can be affectionate tributes by fans of the original.'), --73

--Immediate children of Satire and Comedy, fiction ONLY
('Comedy of manners', 'A type of comedy which satirizes society''s manners and affections while questioning societal standards.'), --74

--Immediate children of Comedy, fiction or nonfiction
('Black comedy', 'A comedy which makes light of subjects which are normally considered serious or painful to discuss and are therefore generally seen as taboo.'), --75
('Stand-up comedy', 'A comedy act performed before a live audience and spoken directly to them.'), --76

--Immediate children of Comedy and Drama, fiction ONLY
('Comedy-drama', 'A work of fiction that combines elements of both comedy and drama.'), --77

--Immediate children of Comedy and Slice of life, fiction ONLY
('Sitcom', 'Short for situation comedy, a sitcom is a largely episodic comedy that is focused a fixed cast of characters as they go about their daily lives.'), --78

--Immediate children of Crime, fiction ONLY
('Phantom thief', 'A story centered around a charming, well-mannered, courteous, and stylish thief who avoids using physical force or intimidation to steal. Generally steals to either fix a moral wrong that cannot be corrected from inside the law or for the thrill of act rather than to gain material wealth.'), --79

--Immediate child of Crime, nonfiction ONLY
('True crime', 'A work that centers around a real crime and the related investigation as it actually happened.'), --80

--Immediate child of Drama or Crime, fiction ONLY
('Courtroom drama', 'A work of fiction that centers around law enforcement, crime, detective-based mystery solving, and civil and criminal litigation.'), --81
('Noir', 'A stylish crime drama that emphasizes cynical attitudes and sexual motivations.'), --82

--Immediate child of Thriller or Crime, fiction ONLY
('Legal thriller', 'A thriller in which lawyers and their employees are major characters and there is a focus on the justice system.'), --83

--Immediate child of Crime or Mystery, fiction or nonfiction
('Detective story', 'A work centered around the investigation and solving of crime by a detective or investigator.'), --84

--Immediate child of Detective story, fiction ONLY
('Hardboiled', 'A type of detective story with a detective who has been rendered cynical by witnessing the cycle of violence that comes from organized crime and a corrupt legal system. The protagonist of hardboiled fiction is often an antihero.'), --85

--Immediate child of Detective story, fiction or nonfiction
('Police procedural', 'A type of detective story where the culprit is often made known to the audience early into the story and the mystery is not who commited a crime, but how the police will manage to catch the criminal.'), --86

--Immediate child of Drama or Political, fiction ONLY
('Political drama', 'A work of fiction centered around a politician or a series of political events.'), --87

--Immediate child of Romance, fiction or nonfiction
('Erotic', 'A work with sexually explicit content. Selecting erotic as a genre or subject matter will automatically mark your work as not allowing minor access.'), --88

--Immediate child of both Comedy and Romance, fiction ONLY
('Romantic comedy', 'A work of fiction that combines aspects of romance and comedy.'), --89

--Immediate children of nonfiction ONLY
('Art', 'A nonfiction work about art.'), --90
('Biography', 'A nonfiction account about the life of a real person.'), --91
('Culture', 'A nonfiction work about society.'), --92
('Documentary', 'A nonfiction film intended to document true events or teaching.'), --93
('Food', 'A nonfiction work about food.'), --94
('History', 'A nonfiction work about the past.'), --95
('Language', 'A nonfiction work about language, whether a specific language or linguists, grammar, or style in general. Can also be writing about writing.'), --96
('Mathematics', 'A nonfiction work about math and numbers.'), --97
('Science', 'A nonfiction work about some variety of science, whether hard or soft.'), --98
('Technology', 'A nonfiction work about technology and industry.'), --99
('Travel', 'A nonfiction work about travel, whether national or global.'), --100

--Immediate child of Food, nonfiction ONLY
('Cookbook', 'A collection of recipes.'), --101

--Immediate children of Biography, nonfiction ONLY
('Autobiography', 'A self-written account of the author''s life.'), --102
('Memoir', 'A collection of memories relating to moments or events in the author''s life. Differentiated from an autobiography because of its smaller scope or focus on a particular aspect of the life rather than the whole.'); --103

INSERT INTO dbo.AltGenreName(GenreID, AltName) VALUES
(10, 'Suspense'), --Thriller 1
(13, 'Telenovela'), --Soap opera 2
(13, 'Teleserye'), --Soap opera 3
(13, 'Ttéléroman'), --Soap opera 4
(13, 'Dorama'), --Soap opera 5
(13, 'Television drama'), --Soap opera 6
(26, 'Fable'), --Fairy tale 7
(28, 'Sword and sorcery'), --Heroic fantasy 8
(29, 'Epic fantasy'), --High fantasy 9
(39, 'Atomicpunk'), --Atompunk 10
(43, 'Decopunk'), --Dieselpunk 11
(44, 'Neo-Victorianism'), --Steampunk 12
(51, 'Genealogy'), --Family saga 13
(51, 'Family history'), --Family saga 14
(68, 'Surreal'), --Absurdist 15
(72, 'Docucomedy'), --Mockumentary 16
(75, 'Dark comedy'), --Black comedy 17
(75, 'Gallows humor'), --Black comedy 18
(77, 'Dramedy'), --Comedy-drama 19
(77, 'Tragicomedy'), --Comedy-drama 20
(79, 'Gentleman thief'), --Phantom thief 21
(79, 'Kaitou'), --Phantom thief 22
(79, 'Lady thief'), --Phantom thief 23
(92, 'Society'); --Culture 24

INSERT INTO dbo.GenreCategory(GenreID, ParentID, SecondaryParentID, TertiaryParentID) VALUES
(3, 1, NULL, NULL), --Drama, Fiction 1
(4, 1, NULL, NULL), --Fan fiction, Fiction 2
(5, 1, NULL, NULL), --Historical fiction, Fiction 3
(6, 1, NULL, NULL), --Pseudo-documentary, Fiction 4
(7, 1, NULL, NULL), --Realistic fiction, Fiction 5
(8, 1, NULL, NULL), --Slice of life, Fiction 6
(9, 1, NULL, NULL), --Speculative fiction, Fiction 7
(10, 1, NULL, NULL), --Thriller, Fiction 8
(11, 3, 1, NULL), --Melodrama, Drama, Fiction 9
(12, 3, 1, NULL), --Medical drama, Drama, Fiction 10
(13, 3, 1, NULL), --Soap opera, Drama, Fiction 11
(14, 4, 1, NULL), --Alternate universe, Fan fiction, Fiction 12
(15, 4, 1, NULL), --Crossover, Fan fiction, Fiction 13
(16, 5, 1, NULL), --Docudrama, Historical fiction, Fiction 14
(17, 9, 1, NULL), --Alternate history, Speculative fiction, Fiction 15
(18, 9, 1, NULL), --Apocalyptic, Speculative fiction, Fiction 16
(19, 9, 1, NULL), --Dystopian, Speculative fiction, Fiction 17
(20, 9, 1, NULL), --Fantasy, Speculative fiction, Fiction 18
(21, 9, 1, NULL), --Horror, Speculative fiction, Fiction 19
(22, 9, 1, NULL), --Post-apocalyptic, Speculative fiction, Fiction 20
(23, 9, 1, NULL), --Science fiction, Speculative fiction, Fiction 21
(24, 9, 1, NULL), --Superhero, Speculative fiction, Fiction 22
(25, 9, 1, NULL), --Utopian, Speculative fiction, Fiction 23
(26, 20, 1, NULL), --Fairy tale, Fantasy, Fiction 24
(27, 20, 1, NULL), --Hard fantasy, Fantasy, Fiction 25
(28, 20, 1, NULL), --Heroic fantasy, Fantasy, Fiction 26
(29, 20, 1, NULL), --High fantasy, Fantasy, Fiction 27
(30, 20, 1, NULL), --Magic realism, Fantasy, Fiction 28
(31, 20, 1, NULL), --Urban fantasy, Fantasy, Fiction 29
(32, 21, 1, NULL), --Gothic, Horror, Fiction 30
(33, 21, 1, NULL), --Slasher, Horror, Fiction 31
(34, 21, 1, NULL), --Supernatural, Horror, Fiction 32
(35, 23, 1, NULL), --Hard science fiction, Science fiction, Fiction 33
(36, 23, 1, NULL), --Punk, Science fiction, Fiction 34
(37, 23, 1, NULL), --Soft science fiction, Science fiction, Fiction 35
(38, 23, 1, NULL), --Space opera, Science fiction, Fiction 36
(39, 36, 1, NULL), --Atompunk, Punk, Fiction 37
(40, 36, 1, NULL), --Biopunk, Punk, Fiction 38
(41, 36, 1, NULL), --Clockpunk, Punk, Fiction 39
(42, 36, 1, NULL), --Cyberpunk, Punk, Fiction 40
(43, 36, 1, NULL), --Dieselpunk, Punk, Fiction 41
(44, 36, 1, NULL), --Steampunk, Punk, Fiction 42
(45, 20, 23, 1), --Science fantasy, Fantasy, Science fiction, Fiction 43
(45, 23, 20, 1), --Science fantasy, Science fiction, Fantasy, Fiction 44
(46, 10, 1, NULL), --Conspiracy, Thriller, Fiction 45
(47, 10, 1, NULL), --Psychological thriller, Thriller, Fiction 46
(48, 1, NULL, NULL), --Action, Fiction 47
(48, 2, NULL, NULL), --Action, Nonfiction 48
(49, 1, NULL, NULL), --Comedy, Fiction 49
(49, 2, NULL, NULL), --Comedy, Nonfiction 50
(50, 1, NULL, NULL), --Crime, Fiction 51
(50, 2, NULL, NULL), --Crime, Nonfiction 52
(51, 1, NULL, NULL), --Family saga, Fiction 53
(51, 2, NULL, NULL), --Family saga, Nonfiction 54
(52, 1, NULL, NULL), --Inspirational, Fiction 55
(52, 2, NULL, NULL), --Inspirational, Nonfiction 56
(53, 1, NULL, NULL), --Music, Fiction 57
(53, 2, NULL, NULL), --Music, Nonfiction 58
(54, 1, NULL, NULL), --Mystery, Fiction 59
(54, 2, NULL, NULL), --Mystery, Nonfiction 60
(55, 1, NULL, NULL), --Nature, Fiction 61
(55, 2, NULL, NULL), --Nature, Nonfiction 62
(56, 1, NULL, NULL), --Philosophical, Fiction 63
(56, 2, NULL, NULL), --Philosophical, Nonfiction 64
(57, 1, NULL, NULL), --Political, Fiction 65
(57, 2, NULL, NULL), --Political, Nonfiction 66
(58, 1, NULL, NULL), --Religious, Fiction 67
(58, 2, NULL, NULL), --Religious, Nonfiction 68
(59, 1, NULL, NULL), --Romance, Fiction 69
(59, 2, NULL, NULL), --Romance, Nonfiction 70
(60, 1, NULL, NULL), --Tragedy, Fiction 71
(60, 2, NULL, NULL), --Tragedy, Nonfiction 72
(61, 1, NULL, NULL), --Adventure, Fiction 73
(61, 2, NULL, NULL), --Adventure, Nonfiction 74
(62, 5, 1, NULL), --Western, Historical fiction, Fiction 75
(62, 48, 1, NULL), --Western, Action, Fiction 76
(62, 61, 1, NULL), --Western, Adventure, Fiction 77
(63, 48, NULL, NULL), --Survivalism, Action 78
(63, 61, NULL, NULL), --Survivalism, Adventure 79
(64, 3, NULL, NULL), --Military, Drama 80
(64, 48, NULL, NULL), --Military, Action 81
(64, 61, NULL, NULL), --Military, Adventure 82
(65, 3, NULL, NULL), --War, Drama 83
(65, 48, NULL, NULL), --War, Action 84
(65, 61, NULL, NULL), --War, Adventure 85
(66, 10, NULL, NULL), --Spy, Thriller 86
(66, 48, NULL, NULL), --Spy, Action 87
(66, 61, NULL, NULL), --Spy, Adventure 88
(67, 48, NULL, NULL), --Gangster, Action 89
(67, 50, NULL, NULL), --Gangster, Crime 90
(67, 61, NULL, NULL), --Gangster, Adventure 91
(68, 49, 1, NULL), --Absurdist, Comedy, Fiction 92
(69, 49, 1, NULL), --Farce, Comedy, Fiction 93
(70, 49, 1, NULL), --Parody, Comedy, Fiction 94
(71, 49, 1, NULL), --Sketch comedy, Comedy, Fiction 95
(72, 6, 1, NULL), --Mockumentary, Pseudo-documentary, Fiction 96
(72, 49, 1, NULL), --Mockumentary, Comedy, Fiction 97
(73, 49, 1, NULL), --Satire, Comedy, Fiction 98
(73, 57, 1, NULL), --Satire, Political, Fiction 99
(74, 73, 49, 1), --Comedy of manners, Satire, Comedy, Fiction 100
(75, 49, NULL, NULL), --Black comedy, Comedy 101
(76, 49, NULL, NULL), --Stand-up comedy, Comedy 102
(77, 3, 49, 1), --Comedy-drama, Drama, Fiction 103
(77, 49, 3, 1), --Comedy-drama, Comedy, Fiction 104
(78, 8, 1, NULL), --Sitcom, Slice of life, Fiction 105
(78, 49, 1, NULL), --Sitcom, Comedy, Fiction 106
(79, 50, 1, NULL), --Phantom thief, Crime, Fiction 107
(80, 50, 2, NULL), --True crime, Crime, Nonfiction 108
(81, 3, 1, NULL), --Courtroom drama, Drama, Fiction 109
(81, 50, 1, NULL), --Courtroom drama, Crime, Fiction 110
(82, 3, 1, NULL), --Noir, Drama, Fiction 111
(82, 50, 1, NULL), --Noir, Crime, Fiction 112
(83, 10, 1, NULL), --Legal thriller, Thriller, Fiction 113
(83, 50, 1, NULL), --Legal thriller, Crime, Fiction 114
(84, 50, NULL, NULL), --Detective story, Crime 115
(85, 84, 1, NULL), --Hardboiled, Detective story, Fiction 116
(86, 84, NULL, NULL), --Police procedural, Detective story 117
(87, 3, 1, NULL), --Political drama, Drama, Fiction 118
(87, 57, 1, NULL), --Political drama, Political, Fiction 119
(88, 59, NULL, NULL), --Erotic, Romance 120
(89, 49, 59, 1), --Romantic comedy, Comedy, Romance, Fiction 121
(89, 59, 49, 1), --Romantic comedy, Romance, Comedy, Fiction 122
(90, 2, NULL, NULL), --Art, Nonfiction 123
(91, 2, NULL, NULL), --Biography, Nonfiction 124
(92, 2, NULL, NULL), --Culture, Nonfiction 125
(93, 2, NULL, NULL), --Documentary, Nonfiction 126
(94, 2, NULL, NULL), --Food, Nonfiction 127
(95, 2, NULL, NULL), --History, Nonfiction 128
(96, 2, NULL, NULL), --Language, Nonfiction 129
(97, 2, NULL, NULL), --Mathematics, Nonfiction 130
(98, 2, NULL, NULL), --Science, Nonfiction 131
(99, 2, NULL, NULL), --Technology, Nonfiction 132
(100, 2, NULL, NULL), --Travel, Nonfiction 133
(101, 94, 2, NULL), --Cookbook, Food, Nonfiction 134
(102, 91, 2, NULL), --Autobiography, Biography, Nonfiction 135
(103, 91, 2, NULL); --Memoir, Biography, Nonfiction 136

INSERT INTO dbo.GenreFormat(GenreID, ParentFormatID, ParentGenreID) VALUES
(1, 9, NULL), --Fiction, Fiction prose 1
(1, 43, NULL), --Fiction, Chapter 2
(1, 44, NULL), --Fiction, Short story 3
(1, 45, NULL), --Fiction, Novelette 4
(1, 46, NULL), --Fiction, Novella 5
(1, 47, NULL), --Fiction, Novel 6
(2, 10, NULL), --Nonfiction, Nonfiction prose 7
(2, 24, NULL), --Nonfiction, Article 8
(2, 25, NULL), --Nonfiction, Creative nonfiction 9
(2, 26, NULL), --Nonfiction, Essay 10
(2, 27, NULL), --Nonfiction, Personal journal 11
(2, 28, NULL), --Nonfiction, Technical writing 12
(2, 54, NULL), --Nonfiction, Research paper 13
(2, 55, NULL), --Nonfiction, Review 14
(2, 66, NULL), --Nonfiction, Blog post 15
(2, 67, NULL), --Nonfiction, Descriptive essay 16
(2, 68, NULL), --Nonfiction, Documentation 17
(2, 69, NULL), --Nonfiction, Expository essay 18
(2, 70, NULL), --Nonfiction, Literary analysis 19
(2, 71, NULL), --Nonfiction, Narrative essay 20
(2, 72, NULL), --Nonfiction, News article 21
(2, 73, NULL), --Nonfiction, Opinion piece 22
(2, 74, NULL), --Nonfiction, Persuasive essay 23
(2, 75, NULL), --Nonfiction, Self-help 24
(2, 76, NULL), --Nonfiction, Textbook 25
(2, 77, NULL), --Nonfiction, Travelogue 26
(13, 3, 1), --Soap opera, Script, Fiction 27
(13, 16, 1), --Soap opera, Screenplay, Fiction 28
(71, 3, 1), --Sketch comedy, Script, Fiction 29
(71, 13, 1), --Sketch comedy, One act, Fiction 30
(71, 15, 1), --Sketch comedy, Play, Fiction 31
(71, 16, 1), --Sketch comedy, Screenplay, Fiction 32
(72, 3, 1), --Mockumentary, Script, Fiction 33
(72, 16, 1), --Mockumentary, Screenplay, Fiction 34
(76, 3, NULL), --Stand-up comedy, Script 35
(78, 3, 1), --Sitcom, Script, Fiction 36
(78, 16, 1), --Sitcom, Screenplay, Fiction 37
(93, 3, 2), --Documentary, Script, Nonfiction 38
(93, 16, 2); --Documentary, Screenplay, Nonfiction 39

--INSERT INTO dbo.WritingGenre(WritingID, GenreID) VALUES
--(1, 1), --Lord of the Things, Fiction 1
--(1, 49), --Lord of the Things, Comedy 2
--(1, 70), --Lord of the Things, Parody 3
--(2, 2), --Ballad of the Trees, Nonfiction 4
--(2, 49), --Ballad of the Trees, Comedy 5
--(2, 55); --Ballad of the Trees, Nature 6