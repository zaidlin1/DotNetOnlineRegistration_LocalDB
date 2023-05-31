USE [CoursesDB]
GO

/****** Object: Table [dbo].[MyLessons] Script Date: 31/05/2023 16:49:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MyLessons] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [CourseId] INT           NULL,
    [Title]    NVARCHAR (50) NULL,
    [Day]      NVARCHAR (10) NULL,
    [Time]     NVARCHAR (10) NULL
);


