-- Coach Table
CREATE TABLE dbo.Coach
(
	ID			INT NOT NULL,
	FullName	NVARCHAR(50) NOT NULL,
	DOB			DATETIME NOT NULL,
	CONSTRAINT [PK_dbo.Coach] PRIMARY KEY CLUSTERED (ID ASC)
);

-- Athlete Table
CREATE TABLE dbo.Athlete
(
	ID			INT NOT NULL,
	CoachID		INT NOT NULL,
	FullName	NVARCHAR(50) NOT NULL,
	DOB			DATETIME NOT NULL,
	CONSTRAINT	[PK_dbo.Athlete] PRIMARY KEY CLUSTERED (ID ASC),
	CONSTRAINT  [FK_dbo.Athlete] FOREIGN KEY (CoachID) 
	REFERENCES	dbo.Coach (ID)
);

-- Workout Table
CREATE TABLE dbo.Workout
(
	ID			INT NOT NULL,
	AthleteID	INT NOT NULL,
	WorkoutType NVARCHAR(255) NOT NULL,
	CONSTRAINT  [PK_dbo.Workout] PRIMARY KEY CLUSTERED (ID ASC),
	CONSTRAINT  [FK_dbo.Workout] FOREIGN KEY (AthleteID)
	REFERENCES  [dbo.Athlete] (ID)
);

-- Records: talbe name left unplural for DAL
CREATE TABLE dbo.Record
(
	ID			INT NOT NULL,
	AthleteID	INT NOT NULL,
	WorkoutID	INT NOT NULL,
	WorkoutTime	NVARCHAR,	--Time in hrs:mins:secs
	Distance	FLOAT, --Allows for fractional miles
	Steps		INT, 
	HeartRate	INT, --IN BPM
	WorkoutDate	DATETIME NOT NULL,
	GPSLog			NVARCHAR(255), -- Unkown Data left as VARCHAR
	CONSTRAINT  [PK_dbo.Record] PRIMARY KEY (ID ASC),
	CONSTRAINT  [FK_dbo.Record] FOREIGN KEY (AthleteID)
	REFERENCES  [dbo.Athlete] (ID),
	CONSTRAINT  [FK2_dbo.Record] FOREIGN KEY (WorkoutID)
	REFERENCES  [dbo.Workout] (ID)
);

-- Seed the data
INSERT INTO dbo.Coach (ID, FullName, DOB) VALUES
(1, 'Some Guy', 4/24/1979),
(2, 'Mike John', 1/1/1977),
(3, 'Coach Killer', 6/6/1966);

INSERT INTO dbo.Athlete (ID, CoachID, FullName, DOB) VALUES
(1, 1, 'John Smith', 3/5/1994),
(2, 1,'Jane Doe', 4/10/1999),
(3, 2, 'John Doe', 1/21/1991),
(4, 3, 'Ripped Guy', 5/10/1992);

INSERT INTO dbo.Workout (ID, AthleteID, WorkoutType) VALUES
(1, 1, 'Wind Sprints'),
(2, 1, 'Lunges'),
(3, 2, '1.5 Mile Time Trial'),
(4, 2, '6 Mile Jog'),
(5, 3, 'Warmup Routine'),
(6, 4, '1 Mile Run');

INSERT INTO dbo.Record (ID, AthleteID, WorkoutID, WorkoutTime, Distance, Steps, HeartRate, WorkoutDate, GPSLog) VALUES
(1, 1, 1, '01:00:00.0000',  0.4, 6000, 118, 1/29/2018, 'DATA'),
(2, 1, 2, '00:25:00.0000', 0.01, 30, 98, 1/29/2018, 'DATA'),
(3, 2, 3, '00:22:66.2390', 1.5, 15000, 100, 1/29/2018, 'DATA'),
(4, 2, 4, '03:01:29.9990', 6.0, 60000, 107, 1/29/2018, 'DATA'),
(5, 3, 5, '00:15:33:3333', .5, 5000, 99, 1/29/2018, 'DATA'),
(6, 4, 6, '00:16:12:1212', 1.0, 10000, 101, 1/29/2017, 'DATA');