-- User Table
CREATE TABLE dbo.LPUser
(
	UserID INT Identity(1,1), -- TO CONNECT TO ASP.NET IDENTITY
	Email VARCHAR(MAX) NOT NULL,
	Birthdate DATETIME,
	FirstName VARCHAR(MAX) NOT NULL,
	LastName VARCHAR(MAX) NOT NULL,
	PhoneNumber VARCHAR(MAX),
	Username VARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_dbo.Users] PRIMARY KEY (UserID)
);

-- Profile Table
CREATE TABLE dbo.LPProfile
(
	ProfileID INT IDENTITY(1,1) NOT NULL,
	UserID INT NOT NULL,
	LPDescription VarChar(120), 
	ProfilePhoto VARBINARY(MAX),
	DisplayRealName BIT NOT NULL DEFAULT 0, --Default off
	Friends INT,
	Followers INT,
	Writers INT,
	CONSTRAINT [PK_dbo.LPProfile] PRIMARY KEY (ProfileID),
	CONSTRAINT [FK_dbo.LPUser] FOREIGN KEY (UserID)
	REFERENCES dbo.LPUser (UserID)
);

--AccessPermission Table
CREATE TABLE dbo.AccessPermission
(
	AccessPermissionID INT IDENTITY(1,1) NOT NULL,
	WritingID INT,
	ProfileID INT,
	PublicAccess BIT NOT NULL,
	FriendAccess BIT NOT NULL,
	PublisherAccess BIT NOT NULL,
	MinorAccess BIT NOT NULL,
	CONSTRAINT [PK_dbo.AccessPermission] PRIMARY KEY (AccessPermissionID)
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
	DocType VARCHAR(MAX) NOT NULL,
	DescriptionText VARCHAR(MAX) NOT NULL,
	WritingFileName VARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_dbo.Writing] PRIMARY KEY (WritingID),
	CONSTRAINT [FK_dbo.ProfileID] FOREIGN KEY (ProfileID)
	REFERENCES dbo.LPProfile (ProfileID),
	CONSTRAINT [FK_dbo.AccessPermissionForWriting] FOREIGN KEY (AccessPermissionID)
	REFERENCES dbo.AccessPermission (AccessPermissionID)
	--CONSTRAINT [FK_dbo.FolderID] FOREIGN KEY (FolderID) --key added but left out until folder table is added
	--REFERENCES dbo.Folder (FolderID)
);

-- Pseudonym Table
CREATE TABLE dbo.Pseudonym
(
	PseudonymID INT IDENTITY(1,1) NOT NULL,
	ProfileID INT NOT NULL,
	Pseudonym VARCHAR(MAX) NOT NULL
	CONSTRAINT [PK_dbo.Pseudonym] PRIMARY KEY (PseudonymID),
	CONSTRAINT [FK_dbo.LPProfile] FOREIGN KEY (ProfileID)
	REFERENCES dbo.LPProfile (ProfileID),
);

-- WritingPseudonym Table
CREATE TABLE dbo.WritingPseudonym
(
	WritingPseudonymID INT IDENTITY(1,1) NOT NULL,
	WritingID INT NOT NULL,
	PseudonymID INT NOT NULL,
	CONSTRAINT [PK_dbo.WritingPseudonym] PRIMARY KEY (WritingPseudonymID),
	CONSTRAINT [FK_dbo.WritingIDforWP] FOREIGN KEY (WritingID)
	REFERENCES dbo.Writing (WritingID),
	CONSTRAINT [FK_dbo.Pseudonym] FOREIGN KEY (PseudonymID)
	REFERENCES dbo.Pseudonym (PseudonymID)
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
	REFERENCES dbo.FormatTag (FormatID),
	CONSTRAINT [FK_dbo.ParentID] FOREIGN KEY (ParentID)
	REFERENCES dbo.FormatTag (FormatID),
	CONSTRAINT [FK_dbo.SecondaryParentID] FOREIGN KEY (SecondaryParentID)
	REFERENCES dbo.FormatTag (FormatID)
);

--WritingFormat Table
CREATE TABLE dbo.WritingFormat 
(
	WritingFormatID INT IDENTITY(1,1) NOT NULL,
	WritingID INT NOT NULL,
	FormatID INT NOT NULL,
	CONSTRAINT [PK_dbo.WritingFormat] PRIMARY KEY (WritingFormatID),
	CONSTRAINT [FK_dbo.WritingIDforWF] FOREIGN KEY (WritingID)
	REFERENCES dbo.Writing (WritingID),
	CONSTRAINT [FK_dbo.FormatTag] FOREIGN KEY (FormatID)
	REFERENCES dbo.FormatTag (FormatID)
);

INSERT INTO dbo.LPUser ( Email, Birthdate, FirstName, LastName, PhoneNumber, Username) VALUES
('dude@dude.com', '2010-04-12 12:00', 'Dude', 'Crush', '555-555-5555', 'RandomDude01'), --1
( 'saltshaker@oldnsalty.net', '1999-09-09 12:00','Phil', 'Forrest', '555-555-5555', '100%Salt'), --2
( 'thestanza@gc.org','1978-06-09 12:00', 'George', 'Castanzna', '', 'TheBubbleBoy'); --3

INSERT INTO dbo.LPProfile(UserID,LPDescription, ProfilePhoto, DisplayRealName, Friends, Followers, Writers) VALUES
(1,'I like to ride bikes', NULL, 0, 1, 1, 1), --dude@dude.com 1
(2,'I dont like to ride bikes', NULL, 0, 1, 1, 1), --saltshaker@oldnsalty.net 2
(3,'', NULL, 0, 1, 1, 1); --thestanza@gc.org 3

INSERT INTO dbo.AccessPermission (WritingID, ProfileID, PublicAccess, FriendAccess, PublisherAccess, MinorAccess) VALUES
(NULL, 1, 0, 1, 1, 1), --dude@dude.com 1 
(NULL, 2, 0, 0, 1, 0), --saltshaker@oldnsalty.net 2
(NULL, 3, 1, 1, 1, 1), --thestanza@gc.org 3
(NULL, NULL, 0, 0, 1, 1), --Lord of the Things 4
(NULL, NULL, 1, 1, 1, 1), --Ballad of the Trees 5
(NULL, NULL, 0, 1, 1, 0); --Hokey Folk Tales 6

INSERT INTO dbo.Writing (ProfileID, AccessPermissionID, Title, Document, AddDate, EditDate, LikesOn, CommentsOn, CritiqueOn, DocType, DescriptionText, WritingFileName) VALUES
(1, 4, 'Lord of the Things', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 0, 0, 'DOCX', 'A humorous play on lord of the rings', 'Lord_of_the_Things'), --dude@dude.com 1
(2, 5, 'Ballad of The Trees', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 1, 1, 'RTF', 'Ballad About Trees', 'balladofthetrees'), --saltshaker@oldnsalty.net 2
(3, 6, 'Hokey Folk Tales', CONVERT(VARBINARY(MAX), 'ABCD'), '1991-04-10', GETDATE(), 1, 1, 1, 'ODT', 'A collection of old forgotten tales: second edition', 'forgottentales'); --thestanza@gc.org 3

UPDATE dbo.AccessPermission
SET WritingID = 1
WHERE AccessPermissionID = 4

UPDATE dbo.AccessPermission
SET WritingID = 2
WHERE AccessPermissionID = 5

UPDATE dbo.AccessPermission
SET WritingID = 3
WHERE AccessPermissionID = 6

INSERT INTO dbo.Pseudonym (ProfileID, Pseudonym) VALUES
(1, 'ComedyClubbed'), --dude@dude.com 1
(1, 'FunnyMan'), --dude@dude.com 2
(1, 'DoomsDayDumb'), --dude@dude.com 3
(2, 'CrustyCrab'), --saltshaker@oldnsalty.net 4
(2, 'RustyRed'), --saltshaker@oldnsalty.net 5
(3, 'Treed'), --thestanza@gc.org 6
(3, 'JustGeorge'), --thestanza@gc.org 7
(3, 'NoPirates'), --thestanza@gc.org 8
(3, 'FestivusFreak'); --thestanza@gc.org 9

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
(34, 8, NULL), --Free verse, Unrhymed poetry 38
(35, 8, NULL), --Found poetry, Unrhymed poetry 39
(36, 7, NULL), --Limerick, Rhyming poetry 40
(37, 7, NULL), --List poetry, Rhyming poetry 41
(37, 8, NULL), --List poetry, Unrhymed poetry 42
(38, 7, NULL), --Rondeau, Rhyming poetry 43
(39, 8, NULL), --Sestina, Unrhymed poetry 44
(40, 7, NULL), --Sound poetry, Rhyming poetry 45
(40, 8, NULL), --Sound poetry, Unrhymed poetry 46
(41, 7, NULL), --Terza Rima, Rhyming poetry 47
(42, 7, NULL), --Villanelle, Rhyming poetry 48
(43, 9, NULL), --Chapter, Fiction prose 49
(44, 9, NULL), --Short story, Fiction prose 50
(45, 9, NULL), --Novelette, Fiction prose 51
(46, 9, NULL), --Novella, Fiction prose 52
(47, 9, NULL), --Novel, Fiction prose 53
(48, 7, NULL), --Ballad, Rhyming poetry 54
(48, 23, NULL), --Ballad, Quatrain 55
(49, 8, NULL), --Blank verse, Unrhymed poetry 56
(49, 19, NULL), --Blank verse, Iambic pentameter 57
(50, 7, NULL), --Ghazal, Rhyming poetry 58
(50, 18, NULL), --Ghazal, Couplet 59
(51, 7, NULL), --Memoriam stanza, Rhyming poetry 60
(51, 23, NULL), --Memoriam stanza, Quatrain 61
(52, 7, NULL), --Petrachan, Rhyming poetry 62
(52, 22, NULL), --Petrachan, Sonnet 63
(53, 7, NULL), --Rhyme royal, Rhyming poetry 64
(53, 19, NULL), --Rhyme royal, Iambic pentameter 65
(53, 21, NULL), --Rhyme royal, Ode 66
(54, 10, NULL), --Research paper, Nonfiction prose 67
(54, 24, NULL), --Research paer, Article 68
(54, 26, NULL), --Research paper, Essay 69
(55, 10, NULL), --Review, Nonfiction prose 70
(55, 24, NULL), --Review, Article 71
(55, 27, NULL), --Review, Personal journal 72
(56, 20, NULL), --Haiku, Japanese poetry 73
(57, 21, NULL), --Horatian ode, Ode 74
(58, 21, NULL), --Irregular ode, Ode 75
(59, 22, NULL), --Italian sonnet, Sonnet 76
(60, 21, NULL), --Pindaric ode, Ode 77
(61, 20, NULL), --Senryu, Japanese poetry 78
(62, 19, NULL), --Shakespearean sonnet, Iambic pentameter 79
(62, 22, NULL), --Shakespearean sonnet, Sonnet 80
(63, 17, NULL), --Shape poetry, Concrete poetry 81
(64, 20, NULL), --Tanka, Japanese poetry 82
(65, 17, NULL), --Visual poetry, Concrete poetry 83
(66, 24, NULL), --Blog post, Article 84
(66, 27, NULL), --Blog post, Personal journal 85
(67, 26, NULL), --Descriptive essay, Essay 86
(68, 28, NULL), --Documentation, Technical writing 87
(69, 26, NULL), --Expository essay, Essay 88
(70, 26, NULL), --Literary analysis, Essay 89
(71, 26, NULL), --Narrative essay, Essay 90
(72, 24, NULL), --News article, Article 91
(72, 26, NULL), --News article, Essay 92
(73, 24, NULL), --Opinion piece, Article 93
(73, 27, NULL), --Opinion piece, Personal journal 94
(74, 26, NULL), --Persuasive essay, Essay 95
(75, 24, NULL), --Self-help, Article 96
(75, 25, NULL), --Self-help, Creative nonfiction 97
(76, 28, NULL), --Textbook, Technical writing 98
(77, 24, NULL), --Travelogue, Article 99
(77, 25, NULL), --Travelogue, Creative nonfiction 100
(77, 27, NULL); --Travelogue, Personal journal 101

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