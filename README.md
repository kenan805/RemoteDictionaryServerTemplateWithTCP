# RemoteDictionaryServerTemplateWithTCP

Database script 
USE [UsersRedis]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 03/02/2022 02:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Username] [nvarchar](50) NOT NULL,
	[Likes] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Users] ([Username], [Likes]) VALUES (N'Amiraslan', 10743)
INSERT [dbo].[Users] ([Username], [Likes]) VALUES (N'Kenan805', 8500)
INSERT [dbo].[Users] ([Username], [Likes]) VALUES (N'Nebi50', 7250)
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Likes]
GO

