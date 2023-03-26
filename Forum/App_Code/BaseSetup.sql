use ForumBase;
drop table Account;
drop table Forum;
drop table Thread;
drop table Msg;
drop table Endpoint_;
drop procedure GetForums;
drop procedure GetThreadsTop5;
drop procedure GetMessages;
drop procedure GetAccounts;
drop procedure GetThreadsCount;
drop procedure GetThreadsAll;
drop procedure GetAllThreadsCount;
drop procedure GetMessagesCount;
drop procedure GetNick;
drop procedure GetThreadName;
drop procedure GetEndpointsTop5;
drop procedure GetThreadSection;
drop procedure Register;
drop procedure GetNicks;
drop procedure PutMessage;
drop procedure GetAccountId;
drop procedure StartTopic;

create table Account
(
Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
Nick NVARCHAR(25) NOT NULL UNIQUE DEFAULT N'Пользователь',
Identifier INT NOT NULL DEFAULT 0,
Passphrase INT NOT NULL DEFAULT 0,
check (len(Nick)<=25),
unique (Identifier,Passphrase) 
);
create nonclustered index GetAccounts on Account (Id, Identifier, Passphrase);
create nonclustered index Register on Account(Nick,Identifier,Passphrase);
create nonclustered index GetNicks on Account(Nick);
create nonclustered index GetNick on Account(Id,Nick);
insert into Account values (N'Хороший',-1665853436,1396495841),
(N'Владелец',786985486,2057336495);
GO
CREATE PROCEDURE GetAccounts 
AS 
BEGIN
	select Identifier, Passphrase from Account;
   set nocount on;
end
GO
CREATE PROCEDURE GetNick (@AccountId int =1)
AS 
BEGIN
	select Nick from Account where Id=@AccountId;
   set nocount on;
end
GO
CREATE PROCEDURE Register (@LoginHash int =1,@PasswordHash int=1,
@Email nvarchar(50)=N'Электронная почта',@Nick nvarchar(25)=N'Ник')
AS 
BEGIN transaction
	insert into Account values(@Nick,@LoginHash,@PasswordHash);
   set nocount on;
commit
GO
CREATE PROCEDURE GetNicks 
AS 
BEGIN
	select Nick from Account;
   set nocount on;
end
GO
CREATE PROCEDURE GetAccountId (@LoginHash int=1,@PasswordHash int=1)
AS 
BEGIN
	select Id from Account where Identifier=@LoginHash and Passphrase=@PasswordHash;
   set nocount on;
end
GO

create table Endpoint_
(
	Id int identity(1,1) not null primary key,
	ForumId int not null,
	Name NVARCHAR(39) NOT NULL default N'КонечнаяТочка',
	check (len(Name)<40)
);
create nonclustered index GetEndpointsTop5 on Endpoint_(Id,Name,ForumId);
GO
CREATE PROCEDURE GetEndpointsTop5 (@ForumId int =1)
AS 
BEGIN
	SELECT TOP 5 Id,Name FROM Endpoint_ where ForumId=@ForumId ORDER BY Id;
	set nocount on;
end
GO
INSERT INTO Endpoint_ VALUES (1,N'Знакомства в Москве'),(1,N'Религиозные знакомства'),
(1,N'Знакомства для отношений'),(1,N'Знакомства в Европе'),(1,N'Прочие знакомства'),
(2,N'Знакомства в Москве'),(2,N'Религиозные знакомства'),
(2,N'Знакомства для отношений'),(2,N'Знакомства в Европе'),(2,N'Прочие знакомства'),
(3,N'Знакомства в Москве'),(3,N'Религиозные знакомства'),
(3,N'Знакомства для бизнеса'),(3,N'Знакомства в Европе'),(3,N'Прочие знакомства'),
(4,N'Знакомства в Москве'),(4,N'Религиозные знакомства'),
(4,N'Знакомства путешественников'),(4,N'Знакомства в Европе'),(4,N'Прочие знакомства'),
(5,N'Юмор'),(5,N'Религия'),
(5,N'Семья'),(5,N'Путешествия'),(5,N'Общие темы');

create table Forum
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name NVARCHAR(24) NOT NULL UNIQUE DEFAULT N'Форум',
	check (len(Name)<25)
);
create nonclustered index GetForums on Forum(Id,Name);
GO
CREATE PROCEDURE GetForums
AS 
BEGIN
	SELECT TOP 5 Id,Name FROM Forum ORDER BY Id;
	set nocount on;
end
GO

INSERT INTO Forum VALUES (N'Ищу девушку'),(N'Ищу парня'),
(N'Ищу компаньона'),(N'Ищу друзей'),(N'Общение');

create table Thread
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name Nvarchar(99) NOT NULL UNIQUE DEFAULT N'Поток',
	EndpointId int NOT NULL default 1,
	check (len(Name)<100)
);
create nonclustered index GetThreads on Thread(Id,Name,EndpointId);
create nonclustered index GetAllThreadsCount on Thread(Id);
create nonclustered index GetThreadName on Thread(Id,Name);
create nonclustered index GetThreadsCount on Thread(Id, EndpointId);
create nonclustered index StartTopic on Thread(Name, EndpointId);
GO
CREATE PROCEDURE GetThreadsTop5 (@EndpointId int=1)
AS 
BEGIN
	SELECT TOP 5 Id,Name FROM Thread WHERE EndpointId=@EndpointId ORDER BY Id DESC;
	set nocount on;
END
GO
CREATE PROCEDURE GetThreadSection (@ThreadId int=1)
AS 
BEGIN
	SELECT EndpointId FROM Thread WHERE Id=@ThreadId;
	set nocount on;
END
GO
CREATE PROCEDURE GetThreadName (@ThreadId int=1)
AS 
BEGIN
	SELECT Name FROM Thread WHERE Id=@ThreadId;
	set nocount on;
END
GO
CREATE PROCEDURE GetThreadsAll (@EndpointId int=1)
AS 
BEGIN
	SELECT Id,Name FROM Thread WHERE EndpointId=@EndpointId ORDER BY Id DESC;
	set nocount on;
END
GO
CREATE PROCEDURE GetThreadsCount (@EndpointId int=1)
AS 
BEGIN
	SELECT Count(Id) FROM Thread WHERE EndpointId=@EndpointId;
	set nocount on;
END
GO
CREATE PROCEDURE GetAllThreadsCount
AS 
BEGIN
	SELECT Count(Id) FROM Thread;
	set nocount on;
END
GO
INSERT INTO Thread VALUES (N'Познакомлюсь с красоткой в Кемерово',1),(N'Оксана, 30 лет, Пермь',2),(N'Ищем партнеров в строительный бизнес, Ялта',3),(N'Подружусь с программистом из Конаково',4),
(N'В чём смысл жизни ?',5),(N'Хочу встретить очаровательную даму, Юра 40 лет',1),(N'Познакомлюсь с состоятельным мужчиной до 45 лет',2),(N'Партнер по торговле на рынках Севастополя',3),(N'Ищу подруг',4),
(N'Как найти себя ?',5),(N'Москва, даёшь обнимашки',1),(N'Ищу любовь, Марина из Саратова',2),(N'Ищу попутчика, Алушта',3),(N'Хочу найти друзей',4),
(N'Где вы впервые познакомились ?',5),(N'Ищу девушку без детей, хочу жениться',1),(N'Познакомлюсь со спортивным мужчиной для совместного туризма',2),(N'Ищем поручителей по кредитам, Тверь',3),(N'С друзьями куда угодно, Полина',4),
(N'Знакомство на работе',5),(N'Найду ту самую, разведен, Тула 20 лет',1),(N'Обаятельная и привлекательная девушка ищет доброго и хозяйственного парня',2),(N'Застройщику требуются риэлтеры',3),(N'Ищу порядочного собеседника, Зеленоград',4),
(N'Знакомство в пути',5),(N'Дамы, вы где, жду встречи, Лёня 17',1),(N'Ищу жениха, Мытищи, 18 лет',2),(N'Нужны врачи на детский праздник',3),(N'Душа компании ищет друзей во Владивостоке',4),
(N'Добрачное знакомство',5),(N'Женюсь в третий раз',1),(N'Познакомлюсь с неженатым мужчиной 20-25 лет, Питер',2),(N'Ищу сожителя',3),(N'Ищу друга для походов в бассейн, Париж',4),
(N'Знакомство с родителями',5),(N'Ищу заботливую, любящую, 37 лет Павел',1),(N'Мужчины, приеду в ваш город, Айнур',2),(N'Продам долю в цветочном бизнесе, Комарово',3),(N'Познакомлюсь с хорошими людьми в Сочи',4),
(N'Вечные вопросы',5),(N'Познакомлюсь с девушками без комплексов',1),(N'Ищу помощника по хозяйству, Иваново',2),(N'Ищем волонтеров по всему СНГ',3),(N'Ищу хороших друзей',4),
(N'Отношения',5),(N'Мужчина без комплексов ищет жену',1),(N'В Мурманск нужны работники от 18 до 50 лет',2),(N'Нужны девушки в группу поддержки, Крым',3),(N'Познакомлюсь с преданными друзьями',4),
(N'Романтика',5),(N'Саратов, 44 года, ищу любовницу',1),(N'Познакомлюсь с сильным и успешным юношей, Даша, в самом расцвете сил',2),(N'Ищу партнера для игры в баскетбол, Москва',3),(N'Познакомлюсь с подругой',4),
(N'Семья',5),(N'Володя, 16 лет, девчонки, люблю вас',1),(N'Мальчики, ждём вас на спортивной площадке, Юрмала',2),(N'Ищу попутчика, Вологда',3),(N'Ищу новых подруг, познакомлю со своими друзьями',4),
(N'Дети',5),(N'Хозяйственный парень ищет девушку, Владивосток 31 год',1),(N'Познакомлюсь с рыболовом, Юля Новгород',2),(N'Нужен социальный работник, Комсомольск',3),(N'Познакомлюсь с артистом',4),
(N'Дача',5),(N'Познакомлюсь со спортсменкой',1),(N'Ищу фанатов Спартака, Клава Псков 34 года',2),(N'Ищу партнера по рукопашке, Тагил',3),(N'Ищу мудрого мужчину - хочу дружить',4),
(N'Товарищи и друзья',5),(N'Ищу шахматистку',1),(N'Нужен строитель дач, Рима 56 лет',2),(N'Ищу фотографов для совместных экскурсий, Питер',3),(N'Ищу надёжных и добрых друзей, Киев',4),
(N'Бог',5),(N'Весь в ожидании любви, Игнат 54 года',1),(N'Ищем автомеханика, Таня и все',2),(N'Нужны работящие парни, Солекамск',3),(N'Друзья, когда же мы встретимся ? Норильск',4),
(N'Сколько вам лет ?',5);

create table Msg
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ThreadId INT NOT NULL DEFAULT 1,
	AccountId int NOT NULL DEFAULT 1,
	MsgText NVARCHAR(1000) NOT NULL DEFAULT N'Сообщение'
);
create nonclustered index GetMessages on Msg(Id,ThreadId,AccountId);
create nonclustered index GetMessagesCount on Msg(Id,ThreadId);
create nonclustered index PutMessage on Msg(ThreadId,AccountId);
GO
create procedure PutMessage
	(@ThreadId int=1, @AccountId int=1, @Message nvarchar(1000)=N'Сообщение')
as
begin transaction
	insert into Msg values (@ThreadId, @AccountId, @Message);
	set nocount on;
commit
go
create procedure StartTopic
	(@ThreadName nvarchar(99)=N'Поток', @EndpointId int=1,
		@AccountId int=1,
		 @Message nvarchar(1000)=N'Сообщение')
as
begin transaction
	declare @ThreadId int;
	insert into Thread values (@ThreadName, @EndpointId);
	select @ThreadId = max(Id) from Thread;
	exec PutMessage @ThreadId, @AccountId, @Message;
	set nocount on;
commit
go
CREATE PROCEDURE GetMessages (@ThreadId int=1)
AS 
BEGIN
	SELECT Id, AccountId, MsgText FROM Msg WHERE ThreadId=@ThreadId ORDER BY Id;
	set nocount on;
END
GO
CREATE PROCEDURE GetMessagesCount (@ThreadId int=1)
AS 
BEGIN
	SELECT COUNT(Id) FROM Msg WHERE ThreadId=@ThreadId;
	set nocount on;
END
GO


insert into Msg values 
(1,1,'test 1'),(2,1,N'тест 2'),
(4,1,N'тест4'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(76,1,N'Some text.'),
(5,1,'test 1'),(6,2,N'тест 2'),(7,2,N'тест3'),(8,1,N'тест4'),
(9,1,'test 1'),(10,2,N'тест 2'),(11,2,N'тест3'),(12,1,N'тест4'),
(13,1,'test 1'),(14,2,N'тест 2'),(15,2,N'тест3'),(16,1,N'тест4'),
(17,1,'test 1'),(18,2,N'тест 2'),(19,2,N'тест3'),(20,1,N'тест4'),
(1,1,'test 1'),(2,2,N'тест 2'),(3,2,N'тест3'),(4,1,N'тест4'),
(5,1,'test 1'),(6,2,N'тест 2'),(7,2,N'тест3'),(8,1,N'тест4'),
(9,1,'test 1'),(10,2,N'тест 2'),(11,2,N'тест3'),(12,1,N'тест4'),
(13,1,'test 1'),(14,2,N'тест 2'),(15,2,N'тест3'),(16,1,N'тест4'),
(17,1,'test 1'),(18,2,N'тест 2'),(19,2,N'тест3'),(20,1,N'тест4'),
(1,1,'test 1'),(2,2,N'тест 2'),(3,2,N'тест3'),(4,1,N'тест4'),
(5,1,'test 1'),(6,2,N'тест 2'),(7,2,N'тест3'),(8,1,N'тест4'),
(9,1,'test 1'),(10,2,N'тест 2'),(11,2,N'тест3'),(12,1,N'тест4'),
(13,1,'test 1'),(14,2,N'тест 2'),(15,2,N'тест3'),(16,1,N'тест4'),
(17,1,'test 1'),(18,2,N'тест 2'),(19,2,N'тест3'),(20,1,N'тест4');