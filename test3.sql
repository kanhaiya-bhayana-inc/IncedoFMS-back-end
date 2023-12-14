Use IncedoFMSDb;

SELECT * FROM FileDetailsMaster;

ALTER TABLE FileDetailsMaster Add StartPosition int NULL, EndPosition int NULL;
ALTER TABLE FileDetailsMaster Add Stage char(1), Curated char(1), Header char(1);


CREATE PROCEDURE UpsertIntoFileDetailsMaster4(
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
    @DbNotebook VARCHAR(100),
	@StartPosition INT,
	@EndPosition INT,
	@Stage CHAR(1),
	@Curated CHAR(1),
	@Header CHAR(1)
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
            FileDate, InsertionMode, IsActive, DbNotebook, StartPosition, EndPosition, Stage, Curated, Header
        )
        VALUES (
            @Id, @FileName, @SourcePath, @DestinationPath, @FileTypeID,
            @Delimiter, @FixedLength, @TemplateName, @EmailID, @ClientID,
            @FileDate, @InsertionMode, @IsActive, @DbNotebook, @StartPosition, @EndPosition, @Stage, @Curated, @Header
        )
    WHEN MATCHED THEN
        UPDATE SET
            FileName = @FileName, SourcePath = @SourcePath, DestinationPath = @DestinationPath,
            FileTypeID = @FileTypeID, Delimiter = @Delimiter, FixedLength = @FixedLength,
            TemplateName = @TemplateName, EmailID = @EmailID, ClientID = @ClientID,
            FileDate = @FileDate, InsertionMode = @InsertionMode, IsActive = @IsActive,
            DbNotebook = @DbNotebook, StartPosition = @StartPosition, EndPosition=@EndPosition,
			Stage = @Stage, Curated= @Curated, Header = @Header;

    SELECT * FROM FileDetailsMaster WHERE FileMasterId = @Id;
END;

Use IncedoFMSDb;
EXEC UpsertIntoFileDetailsMaster8 '1E5F32E5-58DE-45A8-AB12-CB19E1E104D2','juice.txt','source/file/sample102.txt','//',3,'3','N','template101.json','sample@gmail.com',1,'2','Append','N','mynotebook',12,15,'Y','Y','N';

CREATE PROCEDURE UpsertIntoFileDetailsMaster8(
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
    @DbNotebook VARCHAR(100),
    @StartPosition INT,
    @EndPosition INT,
    @Stage CHAR(1),
    @Curated CHAR(1),
    @Header CHAR(1)
)
AS
BEGIN
	
    SET NOCOUNT ON;
    DECLARE @Id UNIQUEIDENTIFIER;
    SET @Id = NEWID();

    -- Concatenate GUID and Filename for TemplateName

    MERGE FileDetailsMaster AS target
    USING (
        SELECT @FileMasterId AS FileMasterId
    ) AS source
    ON target.FileMasterId = source.FileMasterId
    WHEN NOT MATCHED BY target THEN
        INSERT (
            FileMasterId, [FileName], SourcePath, DestinationPath, FileTypeID,
            Delimiter, FixedLength, TemplateName, EmailID, ClientID,
            FileDate, InsertionMode, IsActive, DbNotebook, StartPosition, EndPosition, Stage, Curated, Header
        )
        VALUES (
            @Id, @FileName, @SourcePath, @DestinationPath, @FileTypeID,
            @Delimiter, @FixedLength, CONVERT(VARCHAR(36), @Id) + '_' + @FileName, @EmailID, @ClientID,
            @FileDate, @InsertionMode, @IsActive, @DbNotebook, @StartPosition, @EndPosition, @Stage, @Curated, @Header
        )
    WHEN MATCHED THEN
        UPDATE SET
            FileName = @FileName, SourcePath = @SourcePath, DestinationPath = @DestinationPath,
            FileTypeID = @FileTypeID, Delimiter = @Delimiter, FixedLength = @FixedLength,
            TemplateName = CONVERT(VARCHAR(36), @FileMasterId) + '_' + @FileName, EmailID = @EmailID, ClientID = @ClientID,
            FileDate = @FileDate, InsertionMode = @InsertionMode, IsActive = @IsActive,
            DbNotebook = @DbNotebook, StartPosition = @StartPosition, EndPosition=@EndPosition,
			Stage = @Stage, Curated= @Curated, Header = @Header;

    SELECT * FROM FileDetailsMaster WHERE FileMasterId = @Id;
END;
