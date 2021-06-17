GENERAL README:
---------------
This project is meant to serve as a working example of how to use ADO.NET
along with stored procedures to perform CRUD operations in an ASP.NET application.

It does NOT use Entity Framework, and instead opts for the "old school" way of
performing Create, Read, Update, and Delete operations on a given model.  This 
is known as ADO.NET, or often just simply "queries in the code".

If you would like to run this project up on your machine, please first execute
the database scripts seen below.  You will need to create the database and table, as
well as modify your connection string in appsettings.json (Follow instructions below)

DATABASE AND STORED PROCEDURE SCRIPTS:
--------------------------------------
-- Run these in Microsoft SQL Management Studio to use this project on your machine.
-- Open a new Query window and execute the following:

-- Script creates a new database called CrudWithoutEntityFramework
-- and populates a new table called Books under this new db:

USE [master]
GO
/****** Object:  Database [CrudWithoutEntityFramework]    Script Date: 6/17/2021 5:21:58 PM ******/
CREATE DATABASE [CrudWithoutEntityFramework]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CrudWithoutEntityFramework', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\CrudWithoutEntityFramework.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CrudWithoutEntityFramework_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\CrudWithoutEntityFramework_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CrudWithoutEntityFramework].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET ARITHABORT OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET  MULTI_USER 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET QUERY_STORE = OFF
GO
USE [CrudWithoutEntityFramework]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [CrudWithoutEntityFramework]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 6/17/2021 5:21:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[BookId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NULL,
	[Author] [varchar](100) NULL,
	[Price] [int] NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[BookAddOrEdit]    Script Date: 6/17/2021 5:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- This stored procedure creation script will create the appropriate stored procedure for adding or editing a book.
-- Notice, it takes in the same parameters as what we were passing down through our BookViewModel into the AddOrEdit method under BookController.

CREATE PROCEDURE [dbo].[BookAddOrEdit]
	@BookID INT,
	@Title VARCHAR(200),
	@Author VARCHAR(100),
	@Price INT
AS
BEGIN
	SET NOCOUNT ON; 

	IF @BookID = 0
	BEGIN 
		INSERT INTO Books(Title,Author,Price)
		VALUES (@Title,@Author,@Price)
	END
	ELSE
	BEGIN
		UPDATE Books
		SET
			Title = @Title,
			Author = @Author,
			Price = @Price
		WHERE BookID = @BookID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[BookDeleteByID]    Script Date: 6/17/2021 5:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BookDeleteByID]
	@BookID INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM Books WHERE BookID = @BookID
END
GO
/****** Object:  StoredProcedure [dbo].[BookViewAll]    Script Date: 6/17/2021 5:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[BookViewAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Books
END
GO
/****** Object:  StoredProcedure [dbo].[BookViewByID]    Script Date: 6/17/2021 5:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BookViewByID]
	@BookID INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Books WHERE BookID = @BookID
END
GO
USE [master]
GO
ALTER DATABASE [CrudWithoutEntityFramework] SET  READ_WRITE 
GO


 
