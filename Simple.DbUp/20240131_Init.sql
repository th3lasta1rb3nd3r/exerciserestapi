IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name='Users' AND SCHEMA_NAME(schema_id)='dbo')
BEGIN
	CREATE TABLE [dbo].[Users](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[FirstName] [nvarchar](100) NOT NULL,
		[LastName] [nvarchar](100) NOT NULL,
		[EmailAddress] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_dbo_Users_Id] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
	)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE [name]='UQ_Users_EmailAddress')
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Users_EmailAddress] ON [dbo].[Users]
(
	[EmailAddress] ASC
) ON [PRIMARY]
GO


IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE [name]='Addresses' AND SCHEMA_NAME(schema_id)='dbo')
CREATE TABLE [dbo].[Addresses](
	[Id] [int] NOT NULL,
	[Street] [nvarchar](255) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[PostCode] [nvarchar](10) NULL,
 CONSTRAINT [PK_dbo_Addresses_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE [name]='FK_Addresses_Users')
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_Users] FOREIGN KEY([Id])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_Users]
GO


IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Employments' AND SCHEMA_NAME(schema_id)='dbo')
CREATE TABLE [dbo].[Employments](
	[Id] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Company] [nvarchar](255) NOT NULL,
	[MonthsOfExperience] [tinyint] NOT NULL,
	[Salary] [float] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_dbo_Employments_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE [name]='FK_Employments_Users')
ALTER TABLE [dbo].[Employments]  WITH CHECK ADD  CONSTRAINT [FK_Employments_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Employments] CHECK CONSTRAINT [FK_Employments_Users]
GO
