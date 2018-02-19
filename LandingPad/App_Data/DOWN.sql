-- Drop from least dependent up
IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Pseudonym'

)
BEGIN
	DROP TABLE dbo.Pseudonym
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Writing'

)
BEGIN
	DROP TABLE dbo.Writing
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'LPProfile'

)
BEGIN
	DROP TABLE dbo.LPProfile
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'LPUser'

)
BEGIN
	DROP TABLE dbo.LPUser
END
