use ForumBase;
drop table Account;
drop table Forum;
drop table Thread;
drop table Msg;
drop table Endpoint_;
drop table PrivateMessage;
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
drop procedure AddPrivateMessage;
drop procedure GetAccountsCount;
drop procedure GetPrivateMessagesCount;
drop procedure GetPrivateMessagesAuthors;

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
(N'Владеле34ц',78698586,57336495),
(N'Владел1324ец',78695486,205733495),
(N'Влад45елец',78698,2057339),
(N'Вла2345делец',7869856,205336495),
(N'Влад56елец',786985,205736495),
(N'Влад53елец',78,2057336495),
(N'Вл132аделец',7869856,2057336495),
(N'Владе3124лец',7869,205795),
(N'Владелafец',78698586,20573495),
(N'Влаghдеasdfлец',7869886,2036495),
(N'Влаdfhgделец',786985486,57336495),
(N'Влаdделец',7869896,2057336495),
(N'Владелiuец',78698996,2057336495),
(N'Влuytiаделец',786985486,2336495),
(N'Влtyuiаделец',7869486,2057336495),
(N'Владелtuiец',78486,20536495),
(N'Владеtyuiлец',985486,2336495),
(N'Влtuiаделец',786486,20336495),
(N'Влasdfаделец',785486,20573495),
(N'Влаasdделaец',786486,2036495),
(N'Владелaец',7886,2057395),
(N'Влfаделец',7866,2336495),
(N'Владduелец',78698546,27336495),
(N'Владеsлец',78698548,205733495),
(N'Владdелец',78698546,207336495),
(N'Влfаде0лец',7869856,20573365),
(N'Владеdwrлец',78685486,257336495),
(N'Владеweлец',7898586,20573645),
(N'Владеqлец',7869886,20573395),
(N'Владweелец',78695486,205733649),
(N'Влаtyделец',78685486,205336495),
(N'Владfgелец',78486,205736495),
(N'Владfgе4лец',784186,205736495),
(N'Вла2дfgелец',784286,205736495),
(N'Вл3адfgелец',784386,205736495),
(N'Владfgе41лец',784486,205736495),
(N'Владfgе5лец',784586,205736495),
(N'Вл6адfgелец',784686,205736495),
(N'Влад7fgелец',784786,205736495),
(N'Владfgеле8ц',784886,205736495),
(N'Владfg9елец',784986,205736495),
(N'Владfg10елец',784086,205736495),
(N'Тест', 4543263,76475865);
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
CREATE PROCEDURE GetAccountsCount
AS 
BEGIN
	select Count(Id) from Account;
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
INSERT INTO Thread VALUES (N'Познакомлюсь с красоткой в Кемерово',2),(N'Оксана, 30 лет, Пермь',2),(N'Ищем партнеров в строительный бизнес, Ялта',3),(N'Подружусь с программистом из Конаково',4),
(N'В чём смысл жизни ?',1),(N'Хочу встретить очаровательную даму, Юра 40 лет',2),(N'Познакомлюсь с состоятельным мужчиной до 45 лет',2),(N'Партнер по торговле на рынках Севастополя',3),(N'Ищу подруг',4),
(N'Как найти себя ?',1),(N'Москва, даёшь обнимашки',1),(N'Ищу любовь, Марина из Саратова',2),(N'Ищу попутчика, Алушта',3),(N'Хочу найти друзей',4),
(N'Где вы впервые познакомились ?',1),(N'Ищу девушку без детей, хочу жениться',1),(N'Познакомлюсь со спортивным мужчиной для совместного туризма',2),(N'Ищем поручителей по кредитам, Тверь',3),(N'С друзьями куда угодно, Полина',4),
(N'Знакомство на работе',1),(N'Найду ту самую, разведен, Тула 20 лет',1),(N'Обаятельная и привлекательная девушка ищет доброго и хозяйственного парня',2),(N'Застройщику требуются риэлтеры',3),(N'Ищу порядочного собеседника, Зеленоград',4),
(N'Знакомство в пути',1),(N'Дамы, вы где, жду встречи, Лёня 17',1),(N'Ищу жениха, Мытищи, 18 лет',1),(N'Нужны врачи на детский праздник',3),(N'Душа компании ищет друзей во Владивостоке',4),
(N'Добрачное знакомство',1),(N'Женюсь в третий раз',1),(N'Познакомлюсь с неженатым мужчиной 20-25 лет, Питер',1),(N'Ищу сожителя',3),(N'Ищу друга для походов в бассейн, Париж',4),
(N'Знакомство с родителями',1),(N'Ищу заботливую, любящую, 37 лет Павел',1),(N'Мужчины, приеду в ваш город, Айнур',1),(N'Продам долю в цветочном бизнесе, Комарово',3),(N'Познакомлюсь с хорошими людьми в Сочи',4),
(N'Вечные вопросы',1),(N'Познакомлюсь с девушками без комплексов',1),(N'Ищу помощника по хозяйству, Иваново',1),(N'Ищем волонтеров по всему СНГ',3),(N'Ищу хороших друзей',4),
(N'Отношения',1),(N'Мужчина без комплексов ищет жену',1),(N'В Мурманск нужны работники от 18 до 50 лет',1),(N'Нужны девушки в группу поддержки, Крым',3),(N'Познакомлюсь с преданными друзьями',4),
(N'Романтика',5),(N'Саратов, 44 года, ищу любовницу',1),(N'Познакомлюсь с сильным и успешным юношей, Даша, в самом расцвете сил',2),(N'Ищу партнера для игры в баскетбол, Москва',3),(N'Познакомлюсь с подругой',4),
(N'Семья',5),(N'Володя, 16 лет, девчонки, люблю вас',1),(N'Мальчики, ждём вас на спортивной площадке, Юрмала',2),(N'Ищу попутчика, Вологда',3),(N'Ищу новых подруг, познакомлю со своими друзьями',4),
(N'Дети',5),(N'Хозяйственный парень ищет девушку, Владивосток 31 год',1),(N'Познакомлюсь с рыболовом, Юля Новгород',1),(N'Нужен социальный работник, Комсомольск',3),(N'Познакомлюсь с артистом',4),
(N'Дача',5),(N'Познакомлюсь со спортсменкой',1),(N'Ищу фанатов Спартака, Клава Псков 34 года',1),(N'Ищу партнера по рукопашке, Тагил',1),(N'Ищу мудрого мужчину - хочу дружить',4),
(N'Товарищи и друзья',5),(N'Ищу шахматистку',1),(N'Нужен строитель дач, Рима 56 лет',1),(N'Ищу фотографов для совместных экскурсий, Питер',3),(N'Ищу надёжных и добрых друзей, Киев',4),
(N'Бог',5),(N'Весь в ожидании любви, Игнат 54 года',1),(N'Ищем автомеханика, Таня и все',1),(N'Нужны работящие парни, Солекамск',3),(N'Друзья, когда же мы встретимся ? Норильск',4),
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
		 @Message nvarchar(1000)=N'Сообщение', @ThreadId int output)
as
begin transaction
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
(77,1,N'Всем привет!'),
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

create table PrivateMessage
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SenderAccountId INT NOT NULL DEFAULT 1,
	AcceptorAccountId int NOT NULL DEFAULT 1,
	PrivateText NVARCHAR(1000) NOT NULL DEFAULT N'Сообщение'	
);
create nonclustered index AddPrivateMessage
 on PrivateMessage(SenderAccountId,AcceptorAccountId);
create nonclustered index GetPrivateMessagesCount
	on PrivateMessage(Id,SenderAccountId,AcceptorAccountId);
go
create procedure AddPrivateMessage
	(@SenderAccountId int=1, @AcceptorAccountId int=1, 
		@PrivateText nvarchar(1000)=N'Сообщение')
as
begin transaction
	insert into PrivateMessage 
		values (@SenderAccountId, @AcceptorAccountId, @PrivateText);
	set nocount on;
commit
go
create procedure GetPrivateMessagesCount
	(@AccountId int=1)
as
begin 
	select Count(Id) from PrivateMessage 
		where SenderAccountId=@AccountId or AcceptorAccountId=@AccountId;
	set nocount on;
end
go
CREATE PROCEDURE GetPrivateMessagesAuthors(@AccountId int=1)
AS 
BEGIN	
	(select distinct Account.Id,Nick from Account 
	inner join PrivateMessage on Account.Id=SenderAccountId
	where AcceptorAccountId=@AccountId)union
	(select distinct Account.Id,Nick from Account 
	inner join PrivateMessage on Account.Id=AcceptorAccountId
	where SenderAccountId=@AccountId);
	set nocount on;
end
go
insert into PrivateMessage values (1,2,'adfsadf'),
(2,1,'rtrewet'),
(3,1,'fgfdsg'),
(4,1,'fgfdsg'),
(5,1,'fgfdsg'),
(6,1,'fgfdsg'),
(7,1,'fgfdsg'),
(8,1,'fgfdsg'),
(9,1,'fgfdsg'),
(10,1,'fgfdsg'),
(11,1,'fgfdsg'),
(12,1,'fgfdsg'),
(13,1,'fgfdsg'),
(14,1,'fgfdsg'),
(15,1,'fgfdsg'),
(16,1,'fgfdsg'),
(17,1,'fgfdsg'),
(18,1,'fgfdsg'),
(19,1,'fgfdsg'),
(20,1,'fgfdsg'),
(21,1,'fgfdsg'),
(22,1,'fgfdsg'),
(23,1,'fgfdsg'),
(24,1,'fgfdsg'),
(25,1,'fgfdsg'),
(26,1,'fgfdsg'),
(27,1,'fgfdsg'),
(28,1,'fgfdsg'),
(29,1,'fgfdsg'),
(30,1,'fgfdsg'),
(31,1,'fgfdsg'),
(32,1,'fgfdsg'),
(33,1,'fgfdsg'),
(34,1,'fgfdsg'),
(35,1,'fgfdsg'),
(36,1,'fgfdsg'),
(37,1,'fgfdsg'),
(38,1,'fgfdsg'),
(39,1,'fgfdsg'),
(40,1,'fgfdsg'),
(41,1,'fgfdsg'),
(42,1,'fgfdsg'),
(43,1,'fgfdsg'),
(44,1,'fgfdsg'),
(1,2,'trwertewr');