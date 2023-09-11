CREATE TABLE [dbo].[Documents] (
    [DocumentId]     UNIQUEIDENTIFIER NOT NULL,
    [DocumentName]   NVARCHAR (500)   NULL,
    [DocumentType]   NVARCHAR (250)   NULL,
    [UploadDateTime] DATETIME2 (7)    NULL,
    [NoOfDownloads]  INT              NULL,
    [IsShareAble]    BIT              NULL,
    [ShareAbleLink]  NVARCHAR (500)   NULL,
    [ShareExpiry]    DATETIME2 (7)    NULL,
    [UploadedBy]     UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([DocumentId] ASC)
);

