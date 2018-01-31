-- Drop from least dependent up
IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Record'

)
BEGIN
	DROP TABLE dbo.Record
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Workout'

)
BEGIN
	DROP TABLE dbo.Workout
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Athlete'

)
BEGIN
	DROP TABLE dbo.Athlete
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Coach'

)
BEGIN
	DROP TABLE dbo.Coach
END