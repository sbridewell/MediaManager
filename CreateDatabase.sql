USE [master]
GO
/****** Object:  Database [MediaManager]    Script Date: 30/09/2017 12:43:18 ******/
CREATE DATABASE [MediaManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MediaManager', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\MediaManager.mdf' , SIZE = 7232KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MediaManager_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\MediaManager_log.ldf' , SIZE = 1856KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MediaManager] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MediaManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MediaManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MediaManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MediaManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MediaManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MediaManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [MediaManager] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MediaManager] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [MediaManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MediaManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MediaManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MediaManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MediaManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MediaManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MediaManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MediaManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MediaManager] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MediaManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MediaManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MediaManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MediaManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MediaManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MediaManager] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [MediaManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MediaManager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MediaManager] SET  MULTI_USER 
GO
ALTER DATABASE [MediaManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MediaManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MediaManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MediaManager] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [MediaManager]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Attribute]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attribute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[DataType_Id] [int] NULL,
	[MediaFile_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Attribute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DataType]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.DataType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExpectedFolder]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpectedFolder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ExpectedFolder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FileMetadata]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileMetadata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[MediaFile_Id] [int] NULL,
 CONSTRAINT [PK_dbo.FileMetadata] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FileType]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileType](
	[Extension] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.FileType] PRIMARY KEY CLUSTERED 
(
	[Extension] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaFile]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[Folder] [nvarchar](max) NULL,
	[SizeInBytes] [bigint] NOT NULL,
	[CreatedTimestamp] [datetime] NOT NULL,
	[ModifiedTimestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.MediaFile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PodcastChannel]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PodcastChannel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Link] [nvarchar](max) NULL,
	[RssUrl] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL,
	[Copyright] [nvarchar](max) NULL,
	[LastBuildDate] [datetime] NULL,
	[PublishedDate] [datetime] NULL,
	[Categories] [nvarchar](max) NULL,
	[Author] [nvarchar](max) NULL,
	[OwnerName] [nvarchar](max) NULL,
	[OwnerEmail] [nvarchar](max) NULL,
	[Image_RemoteUrl] [nvarchar](max) NULL,
	[ImageTitle] [nvarchar](max) NULL,
	[ImageLink] [nvarchar](max) NULL,
	[Keywords] [nvarchar](max) NULL,
	[ITunesExplicit] [bit] NOT NULL,
	[Generator] [nvarchar](max) NULL,
	[Documents] [nvarchar](max) NULL,
	[ManagingEditor] [nvarchar](max) NULL,
	[Subtitle] [nvarchar](max) NULL,
	[Subscribed] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PodcastChannel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PodcastEpisode]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PodcastEpisode](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Link] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Author] [nvarchar](max) NULL,
	[EnclosureUrl] [nvarchar](max) NULL,
	[EnclosureSize] [bigint] NOT NULL,
	[EnclosureContentType] [nvarchar](max) NULL,
	[Guid] [nvarchar](max) NULL,
	[PublishDate] [datetime] NULL,
	[Subtitle] [nvarchar](max) NULL,
	[ITunesExplicit] [bit] NOT NULL,
	[Duration] [time](7) NOT NULL,
	[Image_RemoteUrl] [nvarchar](max) NULL,
	[ChannelId] [int] NOT NULL,
	[Downloaded] [bit] NOT NULL,
	[Ignored] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PodcastEpisode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PossibleValue]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PossibleValue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[Attribute_Id] [int] NULL,
 CONSTRAINT [PK_dbo.PossibleValue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 30/09/2017 12:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RootFolder] [nvarchar](max) NULL,
	[PodcastSubfolder] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_DataType_Id]    Script Date: 30/09/2017 12:43:18 ******/
CREATE NONCLUSTERED INDEX [IX_DataType_Id] ON [dbo].[Attribute]
(
	[DataType_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_MediaFile_Id]    Script Date: 30/09/2017 12:43:18 ******/
CREATE NONCLUSTERED INDEX [IX_MediaFile_Id] ON [dbo].[Attribute]
(
	[MediaFile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_MediaFile_Id]    Script Date: 30/09/2017 12:43:18 ******/
CREATE NONCLUSTERED INDEX [IX_MediaFile_Id] ON [dbo].[FileMetadata]
(
	[MediaFile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChannelId]    Script Date: 30/09/2017 12:43:18 ******/
CREATE NONCLUSTERED INDEX [IX_ChannelId] ON [dbo].[PodcastEpisode]
(
	[ChannelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Attribute_Id]    Script Date: 30/09/2017 12:43:18 ******/
CREATE NONCLUSTERED INDEX [IX_Attribute_Id] ON [dbo].[PossibleValue]
(
	[Attribute_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Attribute]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Attribute_dbo.DataType_DataType_Id] FOREIGN KEY([DataType_Id])
REFERENCES [dbo].[DataType] ([Id])
GO
ALTER TABLE [dbo].[Attribute] CHECK CONSTRAINT [FK_dbo.Attribute_dbo.DataType_DataType_Id]
GO
ALTER TABLE [dbo].[Attribute]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Attribute_dbo.MediaFile_MediaFile_Id] FOREIGN KEY([MediaFile_Id])
REFERENCES [dbo].[MediaFile] ([Id])
GO
ALTER TABLE [dbo].[Attribute] CHECK CONSTRAINT [FK_dbo.Attribute_dbo.MediaFile_MediaFile_Id]
GO
ALTER TABLE [dbo].[FileMetadata]  WITH CHECK ADD  CONSTRAINT [FK_dbo.FileMetadata_dbo.MediaFile_MediaFile_Id] FOREIGN KEY([MediaFile_Id])
REFERENCES [dbo].[MediaFile] ([Id])
GO
ALTER TABLE [dbo].[FileMetadata] CHECK CONSTRAINT [FK_dbo.FileMetadata_dbo.MediaFile_MediaFile_Id]
GO
ALTER TABLE [dbo].[PodcastEpisode]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PodcastEpisode_dbo.PodcastChannel_ChannelId] FOREIGN KEY([ChannelId])
REFERENCES [dbo].[PodcastChannel] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PodcastEpisode] CHECK CONSTRAINT [FK_dbo.PodcastEpisode_dbo.PodcastChannel_ChannelId]
GO
ALTER TABLE [dbo].[PossibleValue]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PossibleValue_dbo.Attribute_Attribute_Id] FOREIGN KEY([Attribute_Id])
REFERENCES [dbo].[Attribute] ([Id])
GO
ALTER TABLE [dbo].[PossibleValue] CHECK CONSTRAINT [FK_dbo.PossibleValue_dbo.Attribute_Attribute_Id]
GO
USE [master]
GO
ALTER DATABASE [MediaManager] SET  READ_WRITE 
GO

use MediaManager
go

insert into FileType (Extension, Name, Description)
values 
('avi', 'AVI', 'AVI'),
('flv', 'FLV', 'Flash video'),
('m4a', 'M4A', 'M4A'),
('mov', 'MOV', 'Quicktime'),
('mp3', 'MP3', 'MPEG layer 3'),
('mp4', 'MP4', 'MP4'),
('mpeg', 'MPEG', 'Motion Picture Expert Group'),
('mpg', 'MPG', 'Motion Picture Expert Group'),
('wma', 'WMA', 'WMA')