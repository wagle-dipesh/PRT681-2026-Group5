IF DB_ID('TaskManagerMvcDb') IS NULL
BEGIN
    CREATE DATABASE TaskManagerMvcDb;
END
GO

USE TaskManagerMvcDb;
GO

IF OBJECT_ID('dbo.Tasks', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tasks
    (
        Id            INT IDENTITY(1,1) PRIMARY KEY,
        Title         NVARCHAR(200)  NOT NULL,
        Description   NVARCHAR(1000) NULL,
        IsCompleted   BIT            NOT NULL DEFAULT (0),
        CreatedAt     DATETIME       NOT NULL DEFAULT (GETDATE())
    );
END
GO

CREATE OR ALTER PROCEDURE dbo.spTasks_Manager
    @Action        CHAR(2),
    @Id            INT            = NULL,
    @Title         NVARCHAR(200)  = NULL,
    @Description   NVARCHAR(1000) = NULL,
    @IsCompleted   BIT            = NULL,
    @InsertedId    INT            = NULL OUTPUT,
    @RowsAffected  INT            = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'i'
    BEGIN
        INSERT INTO dbo.Tasks (Title, Description, IsCompleted, CreatedAt)
        VALUES (@Title, @Description, ISNULL(@IsCompleted, 0), GETDATE());

        SET @InsertedId = SCOPE_IDENTITY();
    END

    ELSE IF @Action = 'sa'
    BEGIN
        SELECT Id, Title, Description, IsCompleted, CreatedAt
        FROM dbo.Tasks
        ORDER BY CreatedAt DESC;
    END

    ELSE IF @Action = 's'
    BEGIN
        SELECT Id, Title, Description, IsCompleted, CreatedAt
        FROM dbo.Tasks
        WHERE Id = @Id;
    END

    ELSE IF @Action = 'u'
    BEGIN
        UPDATE dbo.Tasks
        SET Title = @Title,
            Description = @Description,
            IsCompleted = ISNULL(@IsCompleted, 0)
        WHERE Id = @Id;

        SET @RowsAffected = @@ROWCOUNT;
    END

    ELSE IF @Action = 'd'
    BEGIN
        DELETE FROM dbo.Tasks
        WHERE Id = @Id;

        SET @RowsAffected = @@ROWCOUNT;
    END
END
GO
