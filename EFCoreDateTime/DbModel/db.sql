CREATE TABLE [dbo].[DateTimeTest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[dt1] [datetime] NOT NULL,
	[dt2] [datetime] NOT NULL,
	[dt3] [datetime] NOT NULL,
	[dfoff1] [datetimeoffset](7) NOT NULL,
	[dfoff2] [datetimeoffset](7) NOT NULL,
	[dfoff3] [datetimeoffset](7) NOT NULL,
	[dfoffMSKDateTime] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_DateTimeTest] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

