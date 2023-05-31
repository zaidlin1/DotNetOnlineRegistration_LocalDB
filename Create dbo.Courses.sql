USE [CoursesDB]
GO

/****** Object: Table [dbo].[Courses] Script Date: 31/05/2023 16:49:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Courses] (
    [Id]           INT           NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [TeacherName]  NVARCHAR (50) NOT NULL,
    [IsMandatory]  BIT           NOT NULL,
    [LessonsCount] INT           NOT NULL
);


