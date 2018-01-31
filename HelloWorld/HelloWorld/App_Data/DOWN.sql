-- Drop from least dependent up
IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Records'

)
BEGIN
	DROP TABLE dbo.Records
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Workouts'

)
BEGIN
	DROP TABLE dbo.Workouts
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Athletes'

)
BEGIN
	DROP TABLE dbo.Athletes
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Coachs'

)
BEGIN
	DROP TABLE dbo.Coachs
END