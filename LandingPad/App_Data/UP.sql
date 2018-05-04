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
	CONSTRAINT [PK_dbo.Friendship] PRIMARY KEY (FriendshipID),
	CONSTRAINT [FK_dbo.FirstProfileIDForFriendship] FOREIGN KEY (FirstFriendID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.SecondProfileIDForFriendship] FOREIGN KEY (SecondFriendID)
	REFERENCES dbo.LPProfile (ProfileID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [PK_dbo.FirstPseudonymIDForFriendship] FOREIGN KEY (FirstPseudonymID)
	REFERENCES dbo.Pseudonym (PseudonymID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [PK_dbo.SecondPseudonymIDForFriendship] FOREIGN KEY (SecondPseudonymID)
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
	ON UPDATE NO ACTION
);

--GenreFormat Table
CREATE TABLE dbo.GenreFormat
(
	GenreFormatID INT IDENTITY(1,1) NOT NULL,
	GenreID INT NOT NULL,
	ParentFormatID INT NOT NULL,
	CONSTRAINT [PK_dbo.GenreFormat] PRIMARY KEY (GenreFormatID),
	CONSTRAINT [FK_dbo.GenreIDforGenreFormat] FOREIGN KEY (GenreID)
	REFERENCES dbo.GenreTag (GenreID)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [FK_dbo.ParentFormatID] FOREIGN KEY (ParentFormatID)
	REFERENCES dbo.FormatTag (FormatID)
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

INSERT INTO dbo.LPUser ( Email, Birthdate, GivenName, Surname, PhoneNumber, Username) VALUES
('dude@dude.com', '2010-04-12 12:00', 'Dude', 'Crush', '555-555-5555', 'RandomDude01'), --1
( 'saltshaker@oldnsalty.net', '1999-09-09 12:00','Phil', 'Forrest', '555-555-5555', '100%Salt'), --2
( 'thestanza@gc.org','1978-06-09 12:00', 'George', 'Castanzna', '', 'TheBubbleBoy'), --3
('jsmith@penguin.com', '1966-03-14 12:00', 'Joe', 'Smith', '555-555-5555', 'PublisherJoeSmith'), --4
('agent@literary.com', '2000-02-21 12:00', 'Lilah', 'Agent', '', 'LiteraryAgentLilahAgent'); --5

INSERT INTO dbo.AccessPermission (WritingID, ProfileID, PseudonymID, PublicAccess, FriendAccess, PublisherAccess, MinorAccess) VALUES
(NULL, 1, NULL, 0, 1, 1, 1), --dude@dude.com 1 
(NULL, 2, NULL, 0, 0, 1, 0), --saltshaker@oldnsalty.net 2
(NULL, 3, NULL, 1, 1, 1, 1), --thestanza@gc.org 3
(1, NULL, NULL, 0, 0, 1, 1), --Lord of the Things 4
(2, NULL, NULL, 1, 1, 1, 1), --Ballad of the Trees 5
(3, NULL, NULL, 0, 1, 1, 0), --Hokey Folk Tales 6
(NULL, 4, NULL, 1, 1, 1, 1), --jsmith@penguin.com 7
(NULL, 5, NULL, 1, 1, 1, 0), --literary@agent.com 8
(4, NULL, NULL, 0, 1, 0, 1), --The Test Song 9
(NULL, NULL, 1, 1, 1, 1, 1), --ComedyClubbed 10
(NULL, NULL, 2, 1, 1, 1, 1), --FunnyMan 11
(NULL, NULL, 3, 1, 1, 1, 1), --DoomsDayDumb 12
(NULL, NULL, 4, 1, 1, 1, 1), --CrustyCrab 13
(NULL, NULL, 5, 1, 1, 1, 1), --RustyRed 14
(NULL, NULL, 6, 1, 1, 1, 1), --Treed 15
(NULL, NULL, 7, 1, 1, 1, 1), --JustGeorge 16
(NULL, NULL, 8, 1, 1, 1, 1), --NoPirates 17
(NULL, NULL, 9, 1, 1, 1, 1); --FestivusFreak 18

INSERT INTO dbo.LPProfile(UserID, AccessPermissionID, LPDescription, ProfilePhoto, DisplayRealName) VALUES
(1, 1, 'I like to ride bikes', NULL, 0), --dude@dude.com 1
(2, 2, 'I dont like to ride bikes', NULL, 0), --saltshaker@oldnsalty.net 2
(3, 3, '', NULL, 1), --thestanza@gc.org 3
(4, 7, 'I am a publisher at Penguin Books.', NULL, 1), --jsmith@penguin.com 4
(5, 8, 'My name is Lilah Agent and I''m a literary agent.', NULL, 1); --literary@agent.com 5

INSERT INTO dbo.IndividualAccessGrant (AccessPermissionID, GranteeID) VALUES
(4, 2); --Lord of the Things saltshaker@oldnsalty.net 1

INSERT INTO dbo.IndividualAccessRevoke (AccessPermissionID, RevokeeID) VALUES
(4, 4), --Lord of the Things jsmith@penguin.com 1
(5, 1); --Ballad of the Trees dude@dude.com 2

INSERT INTO dbo.LPRole(RoleName, SecondaryRoleName) VALUES
('Writer', ''), --Writer 1
('Publisher', 'Literary Agent'); --Publisher or Literary Agent 2

INSERT INTO dbo.ProfileRole(ProfileID, RoleID, UseSecondaryRoleName) VALUES
(1, 1, 0), --dude@dude.com Writer 1
(2, 1, 0), --saltshaker@oldnsalty.net Writer 2
(3, 1, 0), --thestanza@gc.org Writer 3
(4, 2, 0), --jmsmith@penguin.com Publisher 4
(5, 2, 1); --agent@literary.com Literary Agent 5

INSERT INTO dbo.Friendship(FirstFriendID, SecondFriendID, FirstPseudonymID, SecondPseudonymID) VALUES
(1, 3, NULL, NULL), --dude@dude.com thestanza@gc.org 1
(3, 1, NULL, NULL), --thestanza@gc.org dude@dude.com 2
(1, 4, NULL, NULL), --dude@dude.com jsmith@penguin.com 3
(4, 1, NULL, NULL), --jsmith@penguin.com dude@dude.com 4
(2, 4, NULL, NULL), --saltshaker@oldnsalty.net jsmith@penguin.com 5
(4, 2, NULL, NULL), --jsmith@penguin.com saltshaker@oldnsalty.net 6
(2, 5, NULL, NULL), --saltshaker@oldnsalty.net agent@literary.com 7
(5, 2, NULL, NULL); --agent@literary.com saltshaker@oldnsalty.net 8

INSERT INTO dbo.Writing (ProfileID, AccessPermissionID, Title, Document, AddDate, EditDate, LikesOn, CommentsOn, CritiqueOn, UsePseudonymsInAdditionToUsername, DocType, DescriptionText, WritingFileName) VALUES
(1, 4, 'Lord of the Things', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 0, 0, 0, '.DOCX', 'A humorous play on lord of the rings', 'Lord_of_the_Things'), --dude@dude.com 1
(2, 5, 'Ballad of The Trees', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 1, 1, 0, '.RTF', 'Ballad About Trees', 'balladofthetrees'), --saltshaker@oldnsalty.net 2
(3, 6, 'Hokey Folk Tales', CONVERT(VARBINARY(MAX), 'ABCD'), '1991-04-10', GETDATE(), 1, 1, 1, 1, '.ODT', 'A collection of old forgotten tales: second edition', 'forgottentales'), --thestanza@gc.org 3
(1, 9, 'The Test Song', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 1, 0, 0, 0, '.HTML', 'I love tests; I love every kind of test.', 'everydayimtesting'); --dude@dude.com 4

INSERT INTO dbo.Pseudonym (ProfileID, AccessPermissionID, Pseudonym) VALUES
(1, 10, 'ComedyClubbed'), --dude@dude.com 1
(1, 11, 'FunnyMan'), --dude@dude.com 2
(1, 12, 'DoomsDayDumb'), --dude@dude.com 3
(2, 13, 'CrustyCrab'), --saltshaker@oldnsalty.net 4
(2, 14, 'RustyRed'), --saltshaker@oldnsalty.net 5
(3, 15, 'Treed'), --thestanza@gc.org 6
(3, 16, 'JustGeorge'), --thestanza@gc.org 7
(3, 17, 'NoPirates'), --thestanza@gc.org 8
(3, 18, 'FestivusFreak'); --thestanza@gc.org 9

INSERT INTO dbo.WritingPseudonym (WritingID, PseudonymID) VALUES
(1, 1), --Lord of the Things, ComedyClubbed 1
(1, 2), --Lord of the Things, FunnyMan 2
(2, 5), --Ballad of the Trees, RustyRed 3
(3, 6), --Hokey Folk Tales, Treed 4
(3, 7); --Hokey Folk Tales, JustGeorge 5

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
('Sound poetry', 'Sometimes called "verse without words", a sound poem uses the phonetics of human speech to form a poem. They are generally intended for performance.'), --40
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

INSERT INTO dbo.WritingFormat (WritingID, FormatID) VALUES
(1, 2), --Lord of the Things, Prose
(1, 9), --Lord of the Things, Fiction prose
(1, 46), --Lord of the Things, Novella
(2, 1), --Ballad of the Trees, Poetry
(2, 7), --Ballad of the Trees, Rhyming poetry
(2, 23), --Ballad of the Trees, Quatrain
(2, 48), --Ballad of the Trees, Ballad
(3, 1), --Hokey Folk Tales, Poetry
(3, 2), --Hokey Folk Tales, Prose
(3, 6), --Hokey Folk Tales, Uta monogatari
(3, 7), --Hokey Folk Tales, Rhyming poetry
(3, 9), --Hokey Folk Tales, Fiction prose
(3, 36), --Hokey Folk Tales, Limerick
(3, 44); --Hokey Folk Tales, Short story

INSERT INTO dbo.GenreTag (GenreName, Explanation) VALUES
--Top tier
('Fiction', ''), --1
('Nonfiction', ''), --2

--Immediate children of fiction ONLY
('Drama', ''), --3
('Historical fiction', ''), --4
('Literary fiction', ''), --5
('Realistic fiction', ''), --6
('Slice of life', ''), --7
('Speculative fiction', ''), --8
('Thriller', ''), --9

--Immediate children of Drama, fiction ONLY
('Courtroom drama', ''), --10
('Medical drama', ''), --11
('Political drama', ''), --12
('Soap opera', ''), --13

--Immediate children of Historical fiction, fiction ONLY
('Docudrama', ''), --14
('Period piece', ''), --15

--Immediate children of Speculative fiction, fiction ONLY
('Alternate history', ''), --16
('Apocalyptic', ''), --17
('Dystopian', ''), --18
('Fantasy', ''), --19
('Horror', ''), --20
('Post-apocalyptic', ''), --21
('Science fiction', ''), --22
('Superhero', ''), --23
('Utopian', ''), --24

--Immediate children of Fantasy
('Fairy tale', ''), --25
('Hard fantasy', ''), --26
('Heroic fantasy', ''), --27
('High fantasy', ''), --28
('Magical realism', ''), --29
('Sword and sorcery', ''), --30
('Urban fantasy', ''), --31

--Immediate children of Horror
('Gothic', ''), --32
('Slasher', ''), --33
('Supernatural', ''), --34
('Survival horror', ''), --35

--Immediate children of Science fiction
('Hard science fiction', ''), --36
('Punk', ''), --37
('Soft science fiction', ''), --38
('Space opera', ''), --39

--Immediate children of Punk
('Atompunk', ''), --40
('Biopunk', ''), --41
('Clockpunk', ''), --42
('Cyberpunk', ''), --43
('Dieselpunk', ''), --44
('Steampunk', ''), --45

--Immediate child of BOTH Fantasy and Science fiction
('Science fantasy', ''), --46

--Immediate children of Thriller, fiction ONLY
('Conspiracy', ''), --47
('Legal thriller', ''), --48
('Psychological thriller', ''), --49

--Immediate children of either fiction or nonfiction
('Action', ''), --50
('Adventure', ''), --51
('Comedy', ''), --52
('Crime', ''), --53
('Family saga', ''), --54
('Inspirational', ''), --55
('Mystery', ''), --56
('Philosophical', ''), --57
('Political', ''), --58
('Religious', ''), --59
('Romance', ''), --60
('Tragedy', ''), --61

--Immediate children of Action or Adventure, fiction ONLY
('Western', ''), --62

--Immediate children of Action or Adventure, fiction or nonfiction
('Military', ''), --63
('Spy', ''), --64
('Survivalism', ''), --65
('War', ''), --66

--Immediate children of Action or Adventure or Crime, fiction or nonfiction
('Gangster', ''), --67

--Immediate children of Comedy, fiction ONLY
('Absurdist', ''), --68
('Comedy of manners', ''), --69
('Parody', ''), --70
('Satire', ''), --71
('Sitcom', ''), --72
('Sketch comedy', ''), --73

--Immediate children of Comedy, fiction or nonfiction
('Black comedy', ''), --74
('Stand-up comedy', ''), --75

--Immediate children of Crime, fiction ONLY
('Gentleman thief', ''), --76
('Hardboiled', ''), --77
('Noir', ''), --78

--Immediate children of Crime, fiction or nonfiction
('Detective story', ''), --79
('Police procedural', ''), --80

--Immediate child of Crime, nonfiction ONLY
('True crime', ''), --81

--Immediate child of Crime or Mystery, fiction or nonfiction
('Murder mystery', ''), --82

--Immediate child of both Comedy and Romance, fiction ONLY
('Romantic comedy', ''), --83

--Immediate children of nonfiction ONLY
('Art', ''), --84
('Biography', ''), --85
('Culture', ''), --86
('Documentary', 'A nonfiction film intended to document true events or teaching.'), --87
('Food', ''), --88
('History', ''), --89
('Language', ''), --90
('Math', ''), --91
('Music', ''), --92
('Nature', ''), --93
('Science', ''), --94
('Society', ''), --95
('Technology', ''), --96
('Travel', ''), --97

--Immediate children of Biography, nonfiction ONLY
('Autobiography', ''), --98
('Memoir', ''); --99

INSERT INTO dbo.AltGenreName(GenreID, AltName) VALUES
(9, 'Suspense'), --Thriller 1
(13, 'Telenovela'), --Soap opera 2
(25, 'Fable'), --Fairy tale 3
(28, 'Epic fantasy'), --High fantasy 4
(68, 'Surreal'); --Absurdist 5

INSERT INTO dbo.GenreCategory(GenreID, ParentID, SecondaryParentID) VALUES
(3, 1, NULL), --1
(4, 1, NULL), --2
(5, 1, NULL), --3
(6, 1, NULL), --4
(7, 1, NULL), --5
(8, 1, NULL), --6
(9, 1, NULL), --7
(10, 3, 1), --8
(11, 3, 1), --9
(12, 3, 1), --10
(13, 3, 1), --11
(14, 4, 1), --12
(15, 4, 1), --13
(16, 8, 1), --14
(17, 8, 1), --15
(18, 8, 1), --16
(19, 8, 1), --17
(20, 8, 1), --18
(21, 8, 1), --19
(22, 8, 1), --20
(23, 8, 1), --21
(24, 8, 1), --22
(25, 19, 1), --23
(26, 19, 1), --24
(27, 19, 1), --25
(28, 19, 1), --26
(29, 19, 1), --27
(30, 19, 1), --28
(31, 19, 1), --29
(32, 20, 1), --30
(33, 20, 1), --31
(34, 20, 1), --32
(35, 20, 1), --33
(36, 22, 1), --34
(37, 22, 1), --35
(38, 22, 1), --36
(39, 22, 1), --37
(40, 37, 1), --38
(41, 37, 1), --39
(42, 37, 1), --40
(43, 37, 1), --41
(44, 37, 1), --42
(45, 37, 1), --43
(46, 19, 22), --44
(46, 22, 19), --45
(47, 9, 1), --46
(48, 9, 1), --47
(49, 9, 1), --48
(50, 1, NULL), --49
(50, 2, NULL), --50
(51, 1, NULL), --51
(51, 2, NULL), --52
(52, 1, NULL), --53
(52, 2, NULL), --54
(53, 1, NULL), --55
(53, 2, NULL), --56
(54, 1, NULL), --57
(54, 2, NULL), --58
(55, 1, NULL), --59
(55, 2, NULL), --60
(56, 1, NULL), --61
(56, 2, NULL), --62
(57, 1, NULL), --63
(57, 2, NULL), --64
(58, 1, NULL), --65
(58, 2, NULL), --66
(59, 1, NULL), --67
(59, 2, NULL), --68
(60, 1, NULL), --69
(60, 2, NULL), --70
(61, 1, NULL), --71
(61, 2, NULL), --72
(62, 50, 1), --73
(62, 51, 1), --74
(63, 50, NULL), --75
(63, 51, NULL), --76
(64, 50, NULL), --77
(64, 51, NULL), --78
(65, 50, NULL), --79
(65, 51, NULL), --80
(66, 50, NULL), --81
(66, 51, NULL), --82
(67, 50, NULL), --83
(67, 51, NULL), --84
(67, 53, NULL), --85
(68, 52, 1), --86
(69, 52, 1), --87
(70, 52, 1), --88
(71, 52, 1), --89
(72, 52, 1), --90
(73, 52, 1), --91
(74, 52, NULL), --92
(75, 52, NULL), --93
(76, 53, 1), --94
(77, 53, 1), --95
(78, 53, 1), --96
(79, 53, NULL), --97
(80, 53, NULL), --98
(81, 53, 2), --99
(82, 53, NULL), --100
(82, 56, NULL), --101
(83, 52, 60), --102
(83, 60, 52), --103
(84, 2, NULL), --104
(85, 2, NULL), --105
(86, 2, NULL), --106
(87, 2, NULL), --107
(88, 2, NULL), --108
(89, 2, NULL), --109
(90, 2, NULL), --110
(91, 2, NULL), --111
(92, 2, NULL), --112
(93, 2, NULL), --113
(94, 2, NULL), --114
(95, 2, NULL), --115
(96, 2, NULL), --116
(97, 2, NULL), --117
(98, 85, 2), --118
(99, 85, 2); --119

INSERT INTO dbo.GenreFormat(GenreID, ParentFormatID) VALUES
(1, 9), --1
(1, 43), --2
(1, 44), --3
(1, 45), --4
(1, 46), --5
(1, 47), --6
(2, 10), --7
(2, 24), --8
(2, 25), --9
(2, 26), --10
(2, 27), --11
(2, 28), --12
(2, 54), --13
(2, 55), --14
(2, 66), --15
(2, 67), --16
(2, 68), --17
(2, 69), --18
(2, 70), --19
(2, 71), --20
(2, 72), --21
(2, 73), --22
(2, 74), --23
(2, 75), --24
(2, 76), --25
(2, 77); --26

INSERT INTO dbo.WritingGenre(WritingID, GenreID) VALUES
(1, 1),
(1, 52),
(1, 70),
(2, 2),
(2, 52),
(2, 93);