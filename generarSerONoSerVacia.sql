Use [master]
Create DataBase [SerONoSer]
Go

USE [SerONoSer]
GO
/****** Object:  Table [dbo].[Preguntas]     ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Preguntas](
	[NumPregunta] [int] NOT NULL,
	[Enunciado] [nvarchar](100) NOT NULL,
	[Nivel] [int] NOT NULL,
 CONSTRAINT [PK_TPreguntas] PRIMARY KEY CLUSTERED 
(
	[NumPregunta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RespNoValidas]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RespNoValidas](
	[NumPregunta] [int] NOT NULL,
	[NumRespuesta] [int] NOT NULL,
	[Explicacion] [nvarchar](100) NULL,
 CONSTRAINT [PK_TRespNoValidas] PRIMARY KEY CLUSTERED 
(
	[NumPregunta] ASC,
	[NumRespuesta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Respuestas]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Respuestas](
	[NumPregunta] [int] NOT NULL,
	[NumRespuesta] [int] NOT NULL,
	[PosibleRespuesta] [nvarchar](100) NOT NULL,
	[Valida] [bit] NOT NULL,
 CONSTRAINT [PK_TRespuestas] PRIMARY KEY CLUSTERED 
(
	[NumPregunta] ASC,
	[NumRespuesta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Respuestas] ADD  CONSTRAINT [DF_Respuestas_Valida]  DEFAULT ((1)) FOR [Valida]
GO
ALTER TABLE [dbo].[RespNoValidas]  WITH CHECK ADD  CONSTRAINT [FK_TRespNoValidas_TRespuestas] FOREIGN KEY([NumPregunta], [NumRespuesta])
REFERENCES [dbo].[Respuestas] ([NumPregunta], [NumRespuesta])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RespNoValidas] CHECK CONSTRAINT [FK_TRespNoValidas_TRespuestas]
GO
ALTER TABLE [dbo].[Respuestas]  WITH CHECK ADD  CONSTRAINT [FK_Respuestas_Preguntas] FOREIGN KEY([NumPregunta])
REFERENCES [dbo].[Preguntas] ([NumPregunta])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Respuestas] CHECK CONSTRAINT [FK_Respuestas_Preguntas]
GO
