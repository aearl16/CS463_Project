-- Coach Table
CREATE TABLE dbo.Coachs
(
	CoachID			INT identity(1,1),
	FullName	NVARCHAR(50) NOT NULL,
	DOB			DATETIME NOT NULL,
	CONSTRAINT [PK_dbo.Coachs] PRIMARY KEY CLUSTERED (CoachID ASC)
);

-- Athlete Table
CREATE TABLE dbo.Athletes
(
	AthleteID			int identity(1,1),
	CoachID		INT NOT NULL,
	FullName	NVARCHAR(50) NOT NULL,
	DOB			DATETIME NOT NULL,
	CONSTRAINT	[PK_dbo.Athletes] PRIMARY KEY CLUSTERED (AthleteID ASC),
	CONSTRAINT  [FK_dbo.Athletes] FOREIGN KEY (CoachID) references dbo.Coachs(CoachID)
);

-- Workout Table
CREATE TABLE dbo.Workouts
(
	WorkoutID			INT identity(1,1),
	AthleteID	INT NOT NULL,
	WorkoutType NVARCHAR(255) NOT NULL,
	CONSTRAINT  [PK_dbo.Workouts] PRIMARY KEY CLUSTERED (WorkoutID ASC),
	CONSTRAINT  [FK_dbo.Workouts] FOREIGN KEY (AthleteID) References dbo.Athletes(AthleteID)
);

-- Records: talbe name left unplural for DAL
CREATE TABLE dbo.Records
(
	ID			INT identity(1,1),
	AthleteID	INT NOT NULL,
	WorkoutID	INT NOT NULL,
	WorkoutTime	Nvarchar(64),	--Time in hrs:mins:secs
	Distance	FLOAT, --Allows for fractional miles
	Steps		INT, 
	HeartRate	INT, --IN BPM
	WorkoutDate	NVarchar(64) NOT NULL,
	GPSLog			NVARCHAR(255), -- Unkown Data left as VARCHAR
	CONSTRAINT  [PK_dbo.Records] PRIMARY KEY (ID ASC),
	CONSTRAINT  [FK_dbo.Records] FOREIGN KEY (AthleteID) References dbo.Athletes(AthleteID),
	CONSTRAINT  [FK2_dbo.Records] FOREIGN KEY (WorkoutID) References dbo.Workouts(WorkoutID)
);

-- Seed the data
INSERT INTO dbo.Coachs (FullName, DOB) VALUES
( 'Some Guy', 4/24/1979),
( 'Mike John', 1/1/1977),
( 'Coach Killer', 6/6/1966);

INSERT INTO dbo.Athletes  (CoachID, FullName, DOB) VALUES
( 1, 'John Smith', 3/5/1994),
( 1,'Jane Doe', 4/10/1999),
( 2, 'John Doe', 1/21/1991),
( 3, 'Ripped Guy', 5/10/1992);

INSERT INTO dbo.Workouts (AthleteID, WorkoutType) VALUES
(1, 'Wind Sprints'),
(1, 'Lunges'),
(2, '1.5 Mile Time Trial'),
(2, '6 Mile Jog'),
(3, 'Warmup Routine'),
(4, '1 Mile Run');

INSERT INTO dbo.Records (AthleteID, WorkoutID, WorkoutTime, Distance, Steps, HeartRate, WorkoutDate, GPSLog) VALUES
(1, 1, '01:00:00.0000',  0.4, 6000, 118, '1-29-2018', 'DATA'),
(1, 2, '00:25:00.0000', 0.01, 30, 98, '1-29-2018', 'DATA'),
(2, 3, '00:22:66.2390', 1.5, 15000, 100, '1-29-2018', 'DATA'),
(2, 4, '03:01:29.9990', 6.0, 60000, 107, '1-29-2018', 'DATA'),
(3, 5, '00:15:33:3333', .5, 5000, 99, '1-29-2018', 'DATA'),
(4, 6, '00:16:12:1212', 1.0, 10000, 101, '1-29-2017', 'DATA');