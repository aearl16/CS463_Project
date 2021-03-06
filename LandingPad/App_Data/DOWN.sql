﻿-- Drop from least dependent up
IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'FriendRequest'

)
BEGIN
	DROP TABLE dbo.FriendRequest
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'Friendship'

)
BEGIN
	DROP TABLE dbo.Friendship
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'WritingPseudonym'

)
BEGIN
	DROP TABLE dbo.WritingPseudonym
END

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
	WHERE tables.name = 'WritingFormat'

)
BEGIN
	DROP TABLE dbo.WritingFormat
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'WritingGenre'

)
BEGIN
	DROP TABLE dbo.WritingGenre
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
	WHERE tables.name = 'AltFormatName'

)
BEGIN
	DROP TABLE dbo.AltFormatName
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'AltGenreName'

)
BEGIN
	DROP TABLE dbo.AltGenreName
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'FormatCategory'

)
BEGIN
	DROP TABLE dbo.FormatCategory
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'GenreCategory'

)
BEGIN
	DROP TABLE dbo.GenreCategory
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'GenreFormat'

)
BEGIN
	DROP TABLE dbo.GenreFormat
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'FormatTag'

)
BEGIN
	DROP TABLE dbo.FormatTag
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'GenreTag'

)
BEGIN
	DROP TABLE dbo.GenreTag
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'ProfileRole'

)
BEGIN
	DROP TABLE dbo.ProfileRole
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'LPRole'

)
BEGIN
	DROP TABLE dbo.LPRole
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'IndividualAccessRevoke'

)
BEGIN
	DROP TABLE dbo.IndividualAccessRevoke
END

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'IndividualAccessGrant'

)
BEGIN
	DROP TABLE dbo.IndividualAccessGrant
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
	WHERE tables.name = 'Twitter'

)
BEGIN
	DROP TABLE dbo.Twitter
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

IF EXISTS
(
	SELECT *
	FROM sys.tables
	WHERE tables.name = 'AccessPermission'

)
BEGIN
	DROP TABLE dbo.AccessPermission
END
