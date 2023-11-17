

CREATE DATABASE IncedoFMSDb;

Use IncedoFMSDb;
Drop Database IncedoFMSDb;
use master;

Drop TABLE FileDetailsMaster;

CREATE TABLE FileDetailsMaster (
		FileMasterId UNIQUEIDENTIFIER PRIMARY KEY
		, [FileName] varchar (100) NULL
		, SourcePath varchar (255) NOT NULL
		, DestinationPath varchar (255) NOT NULL
		, FileTypeID int NULL
		, Delimeter varchar (2) NULL
		, FixedLength char(1) 
		, TemplateName varchar (50) NULL
		, EmailID varchar(50) NULL
		, ClientID bigint NULL
		, FileDate char(15) NULL
		, InsertionMode char(10) NULL
		, IsActive char(1)
		, DbNotebook varchar(100) NULL
	);

     ALTER TABLE FileDetailsMaster ALTER COLUMN FileDate char(15);

SELECT *FROM FileDetailsMaster;
CREATE PROCEDURE InsertIntoFileDetailsMaster(

    @FileName varchar (100),
    @SourcePath varchar (255),
	@DestinationPath varchar (255),
    @FileTypeID int,
    @Delimeter varchar (2),
    @FixedLength char(1),
    @TemplateName varchar (50),
    @EmailID varchar(50),
    @ClientID bigint,
    @FileDate char(20),
    @InsertionMode char(10),
    @IsActive char(1),
	@DbNotebook varchar(100)
)
AS
DECLARE @Id UNIQUEIDENTIFIER;
SET @Id = NEWID();
BEGIN
    SET NOCOUNT ON;

    INSERT INTO FileDetailsMaster (FileMasterId,[FileName], SourcePath,DestinationPath,FileTypeID,Delimeter,FixedLength,TemplateName,EmailID,ClientID,FileDate,InsertionMode,IsActive,DbNotebook)
    VALUES (@Id,@FileName,@SourcePath,@DestinationPath,@FileTypeID,@Delimeter,@FixedLength,@TemplateName,@EmailID,@ClientID,@FileDate,@InsertionMode,@IsActive,@DbNotebook);

    SELECT * FROM FileDetailsMaster WHERE FileMasterId = @Id;
END;


EXEC InsertIntoFileDetailsMaster 'Sample1.txt','source/file/sample1.txt',101,'//','N','template1.json','sample@gmail.com',1001,'12 Jul 2023','SP','N';




Create table VendorDetailsMaster(
	Id int primary key identity,
	Name varchar(30)
)

SELECT *FROM VendorDetailsMaster;
INSERT INTO VendorDetailsMaster VALUES('BlackDiamond'),('Bloomberg'),('CIBC'),('Fidelity'),('Pershing')



Create table DelimiterDetailsMaster(
	Id int primary key identity,
	Name varchar(30),
	Symbol varchar(5)
)
SELECT *FROM DelimiterDetailsMaster;

INSERT INTO DelimiterDetailsMaster VALUES('Semicolon','{;}'),('tab','{t}'),('Comma','{,}'),('vertical bar','{|}'),('Colon','{:}')

Create table FiledateDetailsMaster(
	Id int primary key identity,
	Name varchar(100)
)

Create table FiletypeDetailsMaster(
	Id int primary key identity,
	Name varchar(100)
)

SELECT *FROM FiletypeDetailsMaster;

INSERT INTO FiletypeDetailsMaster VALUES('.csv'),('.pdf'),('.docs'),('.xlsx')



CREATE TABLE FileDetailsMaster (
		FileMasterId UNIQUEIDENTIFIER PRIMARY KEY
		, [FileName] varchar (100) NULL
		, SourcePath varchar (255) NOT NULL
		, DestinationPath varchar (255) NOT NULL
		, FileTypeID int NULL
		, Delimiter varchar (2) NULL
		, FixedLength char(1) 
		, TemplateName varchar (50) NULL
		, EmailID varchar(50) NULL
		, ClientID bigint NULL
		, FileDate char(15) NULL
		, InsertionMode char(10) NULL
		, IsActive char(1)
		, DbNotebook varchar(100) NULL
	);

	CREATE PROCEDURE UpsertIntoFileDetailsMaster2(
    @FileMasterId UNIQUEIDENTIFIER,
    @FileName VARCHAR(100),
    @SourcePath VARCHAR(255),
    @DestinationPath VARCHAR(255),
    @FileTypeID INT,
    @Delimiter VARCHAR(2), 
    @FixedLength CHAR(1),
    @TemplateName VARCHAR(50),
    @EmailID VARCHAR(50),
    @ClientID BIGINT,
    @FileDate CHAR(20),
    @InsertionMode CHAR(10),
    @IsActive CHAR(1),
    @DbNotebook VARCHAR(100)
)
AS
BEGIN
	
    SET NOCOUNT ON;
    DECLARE @Id UNIQUEIDENTIFIER;
    SET @Id = NEWID();
    MERGE FileDetailsMaster AS target
    USING (
        SELECT @FileMasterId AS FileMasterId
    ) AS source
    ON target.FileMasterId = source.FileMasterId
    WHEN NOT MATCHED BY target THEN
        INSERT (
            FileMasterId, [FileName], SourcePath, DestinationPath, FileTypeID,
            Delimiter, FixedLength, TemplateName, EmailID, ClientID,
            FileDate, InsertionMode, IsActive, DbNotebook
        )
        VALUES (
            @Id, @FileName, @SourcePath, @DestinationPath, @FileTypeID,
            @Delimiter, @FixedLength, @TemplateName, @EmailID, @ClientID,
            @FileDate, @InsertionMode, @IsActive, @DbNotebook
        )
    WHEN MATCHED THEN
        UPDATE SET
            FileName = @FileName, SourcePath = @SourcePath, DestinationPath = @DestinationPath,
            FileTypeID = @FileTypeID, Delimiter = @Delimiter, FixedLength = @FixedLength,
            TemplateName = @TemplateName, EmailID = @EmailID, ClientID = @ClientID,
            FileDate = @FileDate, InsertionMode = @InsertionMode, IsActive = @IsActive,
            DbNotebook = @DbNotebook;

    SELECT * FROM FileDetailsMaster WHERE FileMasterId = @Id;
END;


EXEC UpsertIntoFileDetailsMaster null,'Sample101-1.txt','source/file/sample102.txt','//',3,'3','N','template101.json','sample@gmail.com',1,'2','Append','N','mynotebook';

