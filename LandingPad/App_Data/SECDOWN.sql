-- Drop from least dependent up
IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'AspNetUserClaims'

)
BEGIN
	DROP TABLE dbo.AspNetUserClaims
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'AspNetUserLogins'

)
BEGIN
	DROP TABLE dbo.AspNetUserLogins
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'AspNetUserRoles'

)
BEGIN
	DROP TABLE dbo.AspNetUserRoles
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'AspNetRoles'

)
BEGIN
	DROP TABLE dbo.AspNetRoles
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'AspNetUsers'

)
BEGIN
	DROP TABLE dbo.AspNetUsers
END






