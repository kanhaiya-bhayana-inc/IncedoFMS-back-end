Use IncedoFMSDb;

SELECT * FROM FileDetailsMaster;


CREATE PROCEDURE InsertIntoFileDetailsMaster2(

    @FileMasterId UNIQUEIDENTIFIER,
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
    MERGE FileDetailsMaster as target
		USING(
            SELECT @FileMasterId as FileMasterId
        ) AS source
			ON target.FileMasterId = source.FileMasterId
	WHEN NOT MATCHED BY target THEN
    INSERT (FileMasterId,[FileName], SourcePath,DestinationPath,FileTypeID,Delimeter,FixedLength,TemplateName,EmailID,ClientID,FileDate,InsertionMode,IsActive,DbNotebook)
    VALUES (@Id,@FileName,@SourcePath,@DestinationPath,@FileTypeID,@Delimeter,@FixedLength,@TemplateName,@EmailID,@ClientID,@FileDate,@InsertionMode,@IsActive,@DbNotebook);

    WHEN MATCHED THEN
    UPDATE SET
    FILENAME=@FileName, SourcePath = @SourcePath, DestinationPath = @DestinationPath,FileTypeID =@FileTypeID,Delimiter=@Delimiter,FixedLength = @FixedLength,TemplateName=@TemplateName,EmailID=@EmailID,ClientID= @ClientID,FileDate=@FileDate,InsertionMode=@InserionMode,IsActive=@IsActive,DbNotebook=@DbNotebook;

    SELECT * FROM FileDetailsMaster WHERE FileMasterId = @Id;
END;


CREATE PROCEDURE dbo.MergePerson (  
		@Id INT,
		@Name NVARCHAR(100),
		@LastName NVARCHAR(255)
	)  
AS   
BEGIN

	MERGE FileDetailsMaster as target
		USING FileDetailsMaster AS source
			ON target.FileMasterId = source.FileMasterId

	WHEN NOT MATCHED BY target
		THEN
			INSERT (
				Id,
				Name,
				LastName
			) 
			VALUES
			(
				@Id,
				@Name,
				@LastName
			)
	
	WHEN MATCHED
		THEN  
			UPDATE SET  
				Name = @Name,
				LastName = @LastName,
				ModifiedDate = GETUTCDATE()
	;

END







CREATE PROCEDURE InsertIntoFileDetailsMaster2(
    @FileMasterId UNIQUEIDENTIFIER,
    @FileName VARCHAR(100),
    @SourcePath VARCHAR(255),
    @DestinationPath VARCHAR(255),
    @FileTypeID INT,
    @Delimeter VARCHAR(2), -- Corrected parameter name from @Delimeter to @Delimiter
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
            Delimeter, FixedLength, TemplateName, EmailID, ClientID,
            FileDate, InsertionMode, IsActive, DbNotebook
        )
        VALUES (
            @FileMasterId, @FileName, @SourcePath, @DestinationPath, @FileTypeID,
            @Delimeter, @FixedLength, @TemplateName, @EmailID, @ClientID,
            @FileDate, @InsertionMode, @IsActive, @DbNotebook
        )
    WHEN MATCHED THEN
        UPDATE SET
            FileName = @FileName, SourcePath = @SourcePath, DestinationPath = @DestinationPath,
            FileTypeID = @FileTypeID, Delimeter = @Delimeter, FixedLength = @FixedLength,
            TemplateName = @TemplateName, EmailID = @EmailID, ClientID = @ClientID,
            FileDate = @FileDate, InsertionMode = @InsertionMode, IsActive = @IsActive,
            DbNotebook = @DbNotebook;

    SELECT * FROM FileDetailsMaster WHERE FileMasterId = @Id;
END;



CREATE PROCEDURE InsertIntoFileDetailsMaster3(
    @FileMasterId UNIQUEIDENTIFIER,
    @FileName VARCHAR(100),
    @SourcePath VARCHAR(255),
    @DestinationPath VARCHAR(255),
    @FileTypeID INT,
    @Delimeter VARCHAR(2), -- Corrected parameter name from @Delimeter to @Delimiter
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
            Delimeter, FixedLength, TemplateName, EmailID, ClientID,
            FileDate, InsertionMode, IsActive, DbNotebook
        )
        VALUES (
            @Id, @FileName, @SourcePath, @DestinationPath, @FileTypeID,
            @Delimeter, @FixedLength, @TemplateName, @EmailID, @ClientID,
            @FileDate, @InsertionMode, @IsActive, @DbNotebook
        )
    WHEN MATCHED THEN
        UPDATE SET
            FileName = @FileName, SourcePath = @SourcePath, DestinationPath = @DestinationPath,
            FileTypeID = @FileTypeID, Delimeter = @Delimeter, FixedLength = @FixedLength,
            TemplateName = @TemplateName, EmailID = @EmailID, ClientID = @ClientID,
            FileDate = @FileDate, InsertionMode = @InsertionMode, IsActive = @IsActive,
            DbNotebook = @DbNotebook;

    SELECT * FROM FileDetailsMaster WHERE FileMasterId = @Id;
END;

EXEC UpsertIntoFileDetailsMaster null,'Sample101-1.txt','source/file/sample102.txt','//',3,'3','N','template101.json','sample@gmail.com',1,'2','Append','N','mynotebook';


CREATE PROCEDURE InsertIntoFileDetailsMaster4(
    @FileMasterId UNIQUEIDENTIFIER NULL,
    @FileName VARCHAR(100),
    @SourcePath VARCHAR(255),
    @DestinationPath VARCHAR(255),
    @FileTypeID INT,
    @Delimeter VARCHAR(2), -- Corrected parameter name from @Delimeter to @Delimiter
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
            Delimeter, FixedLength, TemplateName, EmailID, ClientID,
            FileDate, InsertionMode, IsActive, DbNotebook
        )
        VALUES (
            @Id, @FileName, @SourcePath, @DestinationPath, @FileTypeID,
            @Delimeter, @FixedLength, @TemplateName, @EmailID, @ClientID,
            @FileDate, @InsertionMode, @IsActive, @DbNotebook
        )
    WHEN MATCHED THEN
        UPDATE SET
            FileName = @FileName, SourcePath = @SourcePath, DestinationPath = @DestinationPath,
            FileTypeID = @FileTypeID, Delimeter = @Delimeter, FixedLength = @FixedLength,
            TemplateName = @TemplateName, EmailID = @EmailID, ClientID = @ClientID,
            FileDate = @FileDate, InsertionMode = @InsertionMode, IsActive = @IsActive,
            DbNotebook = @DbNotebook;

    SELECT * FROM FileDetailsMaster WHERE FileMasterId = @Id;
END;
