SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HelpDeskTicketDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HelpDeskTicketId] [int] NOT NULL,
	[TicketDetailDate] [datetime] NOT NULL,
	[TicketDescription] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_HelpDeskTicketDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HelpDeskTickets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TicketStatus] [nvarchar](50) NOT NULL,
	[TicketDate] [datetime] NOT NULL,
	[TicketDescription] [nvarchar](max) NOT NULL,
	[TicketRequesterEmail] [nvarchar](500) NOT NULL,
	[TicketGUID] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_HelpDeskTickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[HelpDeskTicketDetails]  WITH CHECK ADD  CONSTRAINT [FK_HelpDeskTicketDetails_HelpDeskTickets] FOREIGN KEY([HelpDeskTicketId])
REFERENCES [dbo].[HelpDeskTickets] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HelpDeskTicketDetails] CHECK CONSTRAINT [FK_HelpDeskTicketDetails_HelpDeskTickets]
GO