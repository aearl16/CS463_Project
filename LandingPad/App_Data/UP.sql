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
	DisplayRealName BIT NOT NULL DEFAULT 0, --Default off
	Friends INT,
	Followers INT,
	Writers INT,
	CONSTRAINT [PK_dbo.LPProfile] PRIMARY KEY (ProfileID),
	CONSTRAINT [FK_dbo.LPUser] FOREIGN KEY (UserID)
	REFERENCES dbo.LPUser (UserID)
);

-- Writing Table
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
	CONSTRAINT [FK_dbo.ProfileID] FOREIGN KEY (ProfileID)
	REFERENCES dbo.LPProfile (ProfileID),
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
	CategoryType VARCHAR(MAX) NOT NULL,
	FormatType VARCHAR(MAX) NOT NULL,
	Explanation VARCHAR(MAX),
	CONSTRAINT [PK_dbo.FormatTag] PRIMARY KEY (FormatID)
);

--GenreTag Table
--CREATE TABLE dbo.GenreTag
--(
--	GenreID INT IDENTITY(1,1) NOT NULL,
--	GenreName VARCHAR(MAX) NOT NULL,
--	GenreType VARCHAR(MAX) NOT NULL,
--	Explanation VARCHAR(MAX),
--	CONSTRAINT [PK_dbo.GenreTag] PRIMARY KEY (GenreID)
--);

--ThemeTag Table
--CREATE TABLE dbo.ThemeTag
--(
--	ThemeID INT IDENTITY(1,1) NOT NULL,
--	ThemeName VARCHAR(MAX) NOT NULL,
--	ThemeType VARCHAR(MAX) NOT NULL,
--	Explanation VARCHAR(MAX),
--	CONSTRAINT [PK_dbo.ThemeTag] PRIMARY KEY (ThemeID)
--);

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

--WritingGenre Table
--CREATE TABLE dbo.WritingGenre
--(
--	WritingGenreID INT IDENTITY(1,1) NOT NULL,
--	WritingID INT NOT NULL,
--	GenreID INT NOT NULL,
--	CONSTRAINT [PK_dbo.WritingGenre] PRIMARY KEY (WritingGenreID),
--	CONSTRAINT [FK_dbo.Writing] FOREIGN KEY (WritingID)
--	REFERENCES dbo.Writing (WritingID),
--	CONSTRAINT [FK_dbo.GenreTag] FOREIGN KEY (GenreID)
--	REFERENCES dbo.GenreTag (GenreID)
--);

--WritingTheme Table
--CREATE TABLE dbo.WritingTheme
--(
--	WritingThemeID INT IDENTITY(1,1) NOT NULL,
--	WritingID INT NOT NULL,
--	ThemeID INT NOT NULL,
--	CONSTRAINT [PK_dbo.WritingTheme] PRIMARY KEY (WritingThemeID),
--	CONSTRAINT [FK_dbo.Writing] FOREIGN KEY (WritingID)
--	REFERENCES dbo.Writing (WritingID),
--	CONSTRAINT [FK_dbo.ThemeTag] FOREIGN KEY (ThemeID)
--	REFERENCES dbo.ThemeTag (ThemeID)
--);

INSERT INTO dbo.LPUser ( Email, Birthdate, FirstName, LastName, PhoneNumber, Username) VALUES
('dude@dude.com', '2010-04-12 12:00', 'Dude', 'Crush', '555-555-5555', 'RandomDude01'),
( 'saltshaker@oldnsalty.net', '1999-09-09 12:00','Phil', 'Forrest', '555-555-5555', ''),
( 'thestanza@gc.org','1978-06-09 12:00', 'George', 'Castanzna', '', 'TheBubbleBoy');

INSERT INTO dbo.LPProfile(UserID,LPDescription, ProfilePhoto, DisplayRealName, Friends, Followers, Writers) VALUES
(1,'I like to ride bikes', NULL, 0, 1, 1, 1),
(2,'I dont like to ride bikes', NULL, 0, 1, 1, 1),
(3,'', NULL, 0, 1, 1, 1);

INSERT INTO dbo.Writing (ProfileID, Title, Document, AddDate, EditDate, LikesOn, CommentsOn, CritiqueOn, DocType, DescriptionText) VALUES
(1, 'Lord of the Things', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 0, 0, 'DOCX', 'A humorous play on lord of the rings'), --1
(2, 'Ballad of The Trees', CONVERT(VARBINARY(MAX), 'ABCD'), GETDATE(), NULL, 0, 1, 1, 'RTF', 'Ballad About Trees'), --2
(3, 'Hokey Folk Tales', CONVERT(VARBINARY(MAX), 'ABCD'), '1991-04-10', GETDATE(), 1, 1, 1, 'ODT', 'A collection of old forgotten tales: second edition'); --3

INSERT INTO dbo.Pseudonym (ProfileID, Pseudonym) VALUES
(1, 'ComedyClubbed'), --1
(1, 'FunnyMan'), --2
(1, 'DoomsDayDumb'), --3
(2, 'CrustyCrab'), --4
(2, 'RustyRed'), --5
(3, 'Treed'), --6
(3, 'JustGeorge'), --7
(3, 'NoPirates'), --8
(3, 'FestivusFreak'); --9

INSERT INTO dbo.WritingPseudonym (WritingID, PseudonymID) VALUES
(1, 1),
(1, 2),
(2, 5),
(3, 6),
(3, 7);

INSERT INTO dbo.FormatTag (FormatName, CategoryType, FormatType, Explanation) VALUES
--Top category
('Poetry', 'Category', 'Top', 'Writing that uses meter and/or concrete form to enhance the impact of the language.'), --1
('Prose', 'Category', 'Top', 'The ordinary form of written language that lacks the metrical structure of poetry. If you aren''t sure, it''s probably prose.'), --2
('Script', 'Category', 'Top', 'A mix of dialogue and direction meant to be performed or incorporate other media into a final product like a comic or video game. Sometimes known as drama.'), --3

--Crosscategories that require the user to select two categories in order to show up
('Prose poetry', 'Crosscategory', 'Poetry/Prose', 'Poetry written in prose rather than verse, which preserves poetic qualities like short sentences and heightened imagery.'), --4
('Rhymed prose', 'Crosscategory', 'Poetry/Prose', 'Prose that is written in unmetrical rhymes.'), --5
('Uta monogatari (poem tale)', 'Crosscategory', 'Poetry/Prose', 'A type of literature originating in Japan that intersperses poetry with narrative prose.'), --6

--Subcategories that show up when the user selects Poetry
('Rhyming poetry', 'Subcategory', 'Poetry', 'A poem that repeats a similar sound in two or more words across or within the lines of the poem.'), --7
('Unrhymed poetry', 'Subcategory', 'Poetry', 'A poem that doesn''t have a rhyme scheme.'), --8

--Subcategories that show up when the user selects Prose
('Fiction prose', 'Subcategory', 'Prose', 'A piece of prose writing that tells a story derived from the creator''s imagination.'), --9
('Nonfiction prose', 'Subcategory', 'Prose', 'A piece of prose writing where the creator assumes good faith responsibility for the accuracy of the events, people, and information contained within.'), --10

--Subcategories that show up when the user selects Script; these don't have any categories under them
('Comic script', 'Subcategory', 'Script', 'A document that uses detail to describe the dialogue and narrative of a comic. A script is typically used rather than a story board when the art and the writing of the comic are done by different people.'), --11
('Musical', 'Subcategory', 'Script', 'A play or screenplay that combines traditional spoken dialogue with songs with lyrics that are relevant to the plot and help advance the narrative or develop the characters.'), --12
('One act', 'Subcategory', 'Script', 'A short play with only a single act. Typically has limited or no scene or location changes.'), --13
('Opera', 'Subcategory', 'Script', 'A type of drama in which all of the interactions between characters are sung.'), --14
('Play', 'Subcategory', 'Script', 'A piece of writing meant to be performed on stage.'), --15
('Screenplay', 'Subcategory', 'Script', 'A piece of writing meant to be acted, filmed (or animated), and edited into a final product like a TV show or movie.'), --16

--"Tercategories" that show up when the user selects one of the subcategories for Poetry
('Concrete poetry', 'Tercategory', 'Unrhymed poetry', 'A poem that displays one of its elements typographically through the rearrangement of letter within words or arranging words into a shape.'), --17
('Couplet', 'Tercategory', 'Rhyming poetry/Unrhymed poetry', 'A two line poem or part of a poem that can be rhymed or unrhymed.'), --18
('Iambic pentameter', 'Tercategory', 'Rhyming poetry/Unrhymed poetry', 'A metrical line structure of five sets of an unstressed syllable followed by a stressed syllable.'), --19
('Japanese poetry', 'Tercategory', 'Unrhymed poetry', 'A poem written in a form originating in Japan such as a haiku, senryu, or tanka. Doesn''t rhyme and generally uses a set number of syllables.'), --20
('Ode', 'Tercategory', 'Rhyming poetry/Unrhymed poetry', 'A lengthy lyric poem with an elevated style and formal stanza structure.'), --21
('Sonnet', 'Tercategory', 'Rhyming poetry/Unrhymed poetry', 'A fourteen-line poem that typically contains one or more conventional rhyme schemes.'), --22
('Quatrain', 'Tercategory', 'Rhyming poetry', 'A four-line stanza or poem where lines 2 and 4 have a similar number of syllables and rhyme.'), --23

--"Tercategories" that show up when the user selects one of the subcategories for Prose
('Article', 'Tercategory', 'Nonfiction prose', 'A short piece of writing such as might be found in a journalism publication that discusses a subject in an accessable format.'), --24
('Creative nonfiction', 'Tercategory', 'Nonfiction prose', 'A piece of writing that uses literary techniques and styles to discuss factual information.'), --25
('Essay', 'Tercategory', 'Nonfiction prose', 'A piece of writing in which the writer gives an argument on the essay''s subject.'), --26
('Personal journal', 'Tercategory', 'Nonfiction prose', 'An informal piece of writing that covers the writer''s thoughts and impressions on events in their life.'), --27
('Technical writing', 'Tercategory', 'Nonfiction prose', 'Writing intended to share information within a professional setting.'), --28

--"Teritems" that show up when the user selects one of the subcategories for Poetry; the difference between a tercategory and a teritem is that tercategories have items underneath them and teritems don't
('ABC', 'Teritem', 'Unrhymed poetry', 'A five-line poem where lines 1-4 are phrases or clauses that begin with words in alphabetical order and line 5 is a complete sentence beginning with any letter.'), --29
('Acrostic', 'Teritem', 'Unrhymed poetry', 'A poem where each line begins with a letter that spell a phrase or word when read in order.'), --30
('Ballade', 'Teritem', 'Rhyming poetry', 'A poem with three stanzas of seven, eight, or ten lines each followed by a final stanza of four or five lines. All four stanzas end with the same one line refrain.'), --31
('Canzone', 'Teritem', 'Rhyming poetry', 'A poem with five or six stanzas followed by a shorter ending stanza. Originated in medieval Italy.'), --32
('Cinquain', 'Teritem', 'Unrhymed poetry', 'A five-line poem where line 1 is a one word title, line 2 is a two word title description, line 3 tells the action in three words, line 4 expresses the feeling in four words, and line 5 recalls the title in a single word.'), --33
('Free verse', 'Teritem', 'Rhyming poetry/Unrhymed poetry', 'A poem made up of rhymed or unrhymed lines that lack a fixed metrical pattern.'), --34
('Found poetry', 'Teritem', 'Unrhymed poetry', 'A poem made up of words, phrases, and passages from other sources and re-framed through text alterations, rearrangement, or text and line additions.'), --35
('Limerick', 'Teritem', 'Rhyming poetry', 'A poem with five anapestic lines where lines 1, 2, and 5 rhyme and have seven to ten syllables and a consistent verbal rhythm that differs from the rhyme and consistent verbal rhythm found in the five-to-seven-syllable line 3 and line 4.'), --36
('List poetry', 'Teritem', 'Rhyming poetry/Unrhymed poetry', 'A poem made of a list of rhymed or unrhymed list of items or events.'), --37
('Rondeau', 'Teritem', 'Rhyming poetry', 'A lyric poem with ten or thirteen lines with two rhymes and an opening line that is repeated twice as a refrain. Originated in France.'), --38
('Sestina', 'Teritem', 'Unrhymed poetry', 'A poem with six six-line stanzas and a three-line envoy. The final words of the first stanza get repeated in a variable order as ending lines of other stanzas and as part of the envoy.'), --39
('Sound poetry', 'Teritem', 'Rhyming poetry/Unrhymed poetry', 'Sometimes called "verse without words", a sound poem uses the phonetics of human speech to form a poem. They are generally intended for performance.'), --40
('Terza Rima', 'Teritem', 'Rhyming poetry', 'A poem made up of three-line tercets with ten or eleven syllables a line.'), --41
('Villanelle', 'Teritem', 'Rhyming poetry', 'A nineteen-line poem made from five tercets and a concluding quatrain on two rhymes. The first and third lines from the first tercet are alternately repeated as the closing refrain for the succeeding stanzas and joined as the quatrain''s final couplet.'), --42

--"Teritems" that show up when the user selects one of the subcategories for Prose; the difference between a tercategory and a teritem is that tercategories have items underneath them and teritems don't
('Chapter', 'Teritem', 'Fiction prose', 'A subsection of a larger work of fictional prose narrative. Unlike a short story, a chapter does not tell a complete story on its own.'), --43
('Short story', 'Teritem', 'Fiction prose', 'A fictional prose narrative that is typically under 7,500 words.'), --44
('Novelette', 'Teritem', 'Fiction prose', 'A fictional prose narrative that is typically 7,500 to 17,499 words.'), --45
('Novella', 'Teritem', 'Fiction prose', 'A fictional prose narrative that is typically 17,500 to 39,999 words.'), --46
('Novel', 'Teritem', 'Fiction prose', 'A fictional prose narrative that is typically at least 40,000 words.'), --47

--Items that show up when the user selects one of the subcategories or one of the tercategories for Poetry
('Ballad', 'Tercategory/Item', 'Rhyming poetry/Quatrain', 'A poem with an often repeated refrain that tells a story in the style of a folk tale or legend.'), --48
('Blank verse', 'Tercategory/Item', 'Unrhymed poetry/Iambic pentameter', 'A poem written in unrhymed iambic pentameter.'), --49
('Ghazal', 'Tercategory/Item', 'Rhyming poetry/Couplet', 'A lyrical poem of five to fifteen couplets of equal length that all express their own contained poetic thought. A rhyme established in the first couplet and continued in the second line of each connects the couplets. The form is of Arabian origin.'), --50
('Memoriam stanza', 'Tercategory/Item', 'Rhyming poetry/Quatrain', 'A quatrain written in iambic tetrameter with an abba rhyme scheme.'), --51
('Petrachan', 'Tercategory/Item', 'Rhyming poetry/Sonnet', 'A sonnet with fourteen lines made up of an octave with the rhyme scheme abbaabba followed by a sestet of cddcee or cdecde.'), --52
('Rhyme royal', 'Tercategory/Item', 'Rhyming poetry/Ode/Iambic pentameter', 'A poem made of seven-line stanzas where the lines are in iambic pentameter.'), --53

--Items that show up when the user selects one of the subcategories or one of the tercategories for Prose
('Research paper', 'Tercategory/Item', 'Essay/Article/Nonfiction prose', 'A piece of writing that presents new or compiled research on a topic and comes to some sort of conclusion based on it.'), --54
('Review', 'Tercategory/Item', 'Article/Personal journal/Nonfiction prose', 'A piece of writing that discusses a place, product, service, or piece of media, judges its merits, and gives it a rating.'), --55

--Items that show up when the user selects one of the tercategories for Poetry
('Haiku', 'Item', 'Japanese poetry', 'A type of poem originating in Japan that is made up of three unrhymed lines where the first and third lines are five morae (or syllables) each and the second line is seven. Features a seasonal word known as a kigo.'), --56
('Horatian ode', 'Item', 'Ode', 'A short lyric poem made up of two- or four-lined stanzas with a common metrical pattern.'), --57
('Irregular ode', 'Item', 'Ode', 'A type of ode characterized by irregularity of structure and verse and a lack of correspondence between parts. Cannot be classified as either a pindaric or Horatian ode.'), --58
('Italian sonnet', 'Item', 'Sonnet', 'A sonnet made from an octave with an abbaabba rhyme scheme that is followed by six lines that have a rhyme scheme of cdecde or cdccdc.'), --59
('Pindaric ode', 'Item', 'Ode', 'A poem that starts with a strophe, or two or more lines repeated as a unit, and is followed by an antistrophe that uses the same metrical pattern. The poem concludes with a summary line (known as an epode) in a different meter.'), --60
('Senryu', 'Item', 'Japanese poetry', 'A Japanese poem type that has the same unrhymed three lines of five, seven, and five morae or syllables each found in a haiku. Unlike a haiku, a senryu will not have a seasonal word (kigo) and is about humanity rather than nature.'), --70
('Shakespearean sonnet', 'Item', 'Sonnet/Iambic pentameter', 'A fourteen-line sonnet made up of three quatrains with the rhyme scheme abab cdcd efef and a final couplet gg. Usually written in iambic pentameter'), --71
('Shape poetry', 'Item', 'Concrete poetry', 'A type of concrete poem that is written in the shape or form of an object.'), --72
('Tanka', 'Item', 'Japanese poetry', 'A five-line poem of Japanese origin that uses the syllable pattern of five, seven, five, seven, seven in its various lines.'), --73
('Visual poetry', 'Item', 'Concrete poetry', 'A poem that arranges text, images, and symbols in a way that conveys the meaning of the work. A form of concrete poetry.'), --74

--Items that show up when the user selects one of the tercategories for Prose
('Blog post', 'Item', 'Article/Personal journal', 'A first person, informal account of something that the writer finds of interest.'), --75
('Descriptive essay', 'Item', 'Essay', 'An essay in which the writer describes a person, place, object, or memory using evocative details.'), --76
('Documentation and manuals', 'Item', 'Technical writing', 'Information on how a problem was solved, how an item works, or the steps required in a job.'), --77
('Expository essay', 'Item', 'Essay', 'An essay that informs the reader about a topic in a fair and balanced manner.'), --78
('Literary Analysis', 'Item', 'Essay', 'A piece of writing that uses evidence from a text and outside sources to analyze an aspect of a piece of fiction.'), --79
('Narrative essay', 'Item', 'Essay', 'An essay in which the writer shares a story about a real-life experience.'), --80
('News article', 'Item', 'Article/Essay', 'A piece of writing which gives an impartial account for a general audience by covering the facts about a recent news item.'), --81
('Opinion piece (editorial)', 'Item', 'Article/Personal journal', 'A piece of writing where the author talks about their opinion on a topic, often in an impassioned way.'), --82
('Persuasive essay', 'Item', 'Essay', 'An essay in which the writer takes a stance on an issue and attempts to use logic and arguments to convince the reader to agree with their conclusion.'), --83
('Self-help', 'Item', 'Article/Creative nonfiction', 'A piece of writing in which the author attempts to provide the reader with information and advice that will allow them to help themselves.'), --84
('Textbook', 'Item', 'Technical writing', 'A book for use in academic settings that aims to teach the reader a new skill or about a subject.'), --85
('Travelogue', 'Item', 'Article/Creative nonfiction/Personal journal', 'An account of the writer''s travels to different locations.'); --86

INSERT INTO dbo.WritingFormat (WritingID, FormatID) VALUES
(1, 2),
(1, 9),
(1, 46),
(2, 1),
(2, 7),
(2, 23),
(2, 48),
(3, 1),
(3, 2),
(3, 6),
(3, 7),
(3, 9),
(3, 35),
(3, 44);