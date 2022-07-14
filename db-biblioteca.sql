CREATE TABLE [dbo].[books] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [isbn]        VARCHAR (50)  NOT NULL,
    [title]       VARCHAR (MAX) NOT NULL,
    [author]      VARCHAR (MAX) NOT NULL,
    [year]        INT           NOT NULL,
    [numberpages] INT           NULL,
    [genre]       VARCHAR (50)  NOT NULL,
    [shelf]       VARCHAR (50)  NOT NULL,
    [state]       BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[rentals] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [book_id]    INT      NOT NULL,
    [user_id]    INT      NOT NULL,
    [start_date] DATETIME NOT NULL,
    [end_date]   DATETIME NOT NULL,
    [state]      BIT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[users] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [name]       VARCHAR (MAX) NOT NULL,
    [surname]    VARCHAR (MAX) NOT NULL,
    [email]      VARCHAR (MAX) NOT NULL,
    [password]   VARCHAR (MAX) NOT NULL,
    [cellNumber] VARCHAR (50)  NOT NULL,
    [register]   BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


SET IDENTITY_INSERT [dbo].[books] ON
INSERT INTO [dbo].[books] ([Id], [isbn], [title], [author], [year], [numberpages], [genre], [shelf], [state]) VALUES (1, N'978-8831007023', N'Harry Potter - La serie completa', N'J.K.Rowling', 2021, 3808, N'Fantasy', N'F20', 0)
INSERT INTO [dbo].[books] ([Id], [isbn], [title], [author], [year], [numberpages], [genre], [shelf], [state]) VALUES (4, N'978-8804598909', N'Il visconte dimenticato', N'Italo Calvino', 2010, 133, N'Letteratura', N'L14', 0)
INSERT INTO [dbo].[books] ([Id], [isbn], [title], [author], [year], [numberpages], [genre], [shelf], [state]) VALUES (5, N'978-8807900587', N'Il ritratto di Dorian Gray', N'Oscar Wilde', 2013, 272, N'Letteratura', N'L8', 0)
INSERT INTO [dbo].[books] ([Id], [isbn], [title], [author], [year], [numberpages], [genre], [shelf], [state]) VALUES (7, N'978-8367385204', N'Il Signore degli Anelli', N'J. R. R. Tolkien', 2001, 285, N'Fantasy', N'F2', 0)
INSERT INTO [dbo].[books] ([Id], [isbn], [title], [author], [year], [numberpages], [genre], [shelf], [state]) VALUES (9, N'977-5638242020', N'Sistemi informatici', N'Rondano', 2019, 420, N'Istruzione', N'I6', 0)
SET IDENTITY_INSERT [dbo].[books] OFF


SET IDENTITY_INSERT [dbo].[users] ON
INSERT INTO [dbo].[users] ([Id], [name], [surname], [email], [password], [cellNumber], [register]) VALUES (1, N'Angelo', N'Guarnieri', N'ag@gmail.com', N'12345a', N'3274657890', 1)
INSERT INTO [dbo].[users] ([Id], [name], [surname], [email], [password], [cellNumber], [register]) VALUES (3, N'Marco', N'Rossi', N'mc@gmail.com', N'12345m', N'3257867544', 1)
INSERT INTO [dbo].[users] ([Id], [name], [surname], [email], [password], [cellNumber], [register]) VALUES (5, N'Alessandro', N'Verdi', N'av@gmail.com', N'12345v', N'3298716278', 1)
INSERT INTO [dbo].[users] ([Id], [name], [surname], [email], [password], [cellNumber], [register]) VALUES (6, N'Test', N'Prova', N'aaa1', N'12345', N'0000000000', 1)
SET IDENTITY_INSERT [dbo].[users] OFF

