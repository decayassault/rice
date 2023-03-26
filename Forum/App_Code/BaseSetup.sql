/*use AlterDatabase;*/
use ForumBase;
drop table Msg;
drop table PrivateMessage;
drop table ContactInfo;
drop table Addresses;
drop table Request;
drop table AddressInfo;
drop table Descriptions;
drop table Account;
drop table Thread;
drop table Endpoint_;
drop table Forum;
drop table Cities;
drop table Countries;
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
drop procedure GetPrivateMessagesCompanions;
drop procedure GetPrivateMessagesTexts;
drop procedure GetPrivateMessagesAuthorsCount;
drop procedure GetPrivateDialogsCount;
drop procedure CheckNickIfExists;

create table Account
(
Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
Nick NVARCHAR(25) NOT NULL UNIQUE DEFAULT N'Пользователь',
Identifier INT NOT NULL DEFAULT 0,
Passphrase INT NOT NULL DEFAULT 0,
Deleted bit not null default 0,
check (len(Nick)<=25),
unique (Identifier,Passphrase) 
);
create nonclustered index GetAccounts on Account (Identifier, Passphrase,Deleted);
create nonclustered index GetAccountId on Account (Id,Identifier,Passphrase,Deleted);
create nonclustered index Register on Account(Nick,Identifier,Passphrase,Deleted);
create nonclustered index GetNicks on Account(Nick,Deleted);
create nonclustered index GetNick on Account(Id,Nick,Deleted);
insert into Account values (N'Хороший',-1665853436,1396495841,0),
(N'Собеседник',1576036729,-690057382,0),
(N'Персона',-2060384975,-396126386,0),
(N'Влад45елец',78698,2057339,0),
(N'Вла2345делец',7869856,205336495,0),
(N'Влад56елец',786985,205736495,0),
(N'Влад53елец',78,2057336495,0),
(N'Вл132аделец',7869856,2057336495,0),
(N'Владе3124лец',7869,205795,0),
(N'Владелafец',78698586,20573495,0),
(N'Влаghдеasdfлец',7869886,2036495,0),
(N'Влаdfhgделец',786985486,57336495,0),
(N'Влаdделец',7869896,2057336495,0),
(N'Владелiuец',78698996,2057336495,0),
(N'Влuytiаделец',786985486,2336495,0),
(N'Влtyuiаделец',7869486,2057336495,0),
(N'Владелtuiец',78486,20536495,0),
(N'Владеtyuiлец',985486,2336495,0),
(N'Влtuiаделец',786486,20336495,0),
(N'Влasdfаделец',785486,20573495,0),
(N'Влаasdделaец',786486,2036495,0),
(N'Владелaец',7886,2057395,0),
(N'Влfаделец',7866,2336495,0),
(N'Владduелец',78698546,27336495,0),
(N'Владеsлец',78698548,205733495,0),
(N'Владdелец',78698546,207336495,0),
(N'Влfаде0лец',7869856,20573365,0),
(N'Владеdwrлец',78685486,257336495,0),
(N'Владеweлец',7898586,20573645,0),
(N'Владеqлец',7869886,20573395,0),
(N'Владweелец',78695486,205733649,0),
(N'Влаtyделец',78685486,205336495,0),
(N'Владfgелец',78486,205736495,0),
(N'Владfgе4лец',784186,205736495,0),
(N'Вла2дfgелец',784286,205736495,0),
(N'Вл3адfgелец',784386,205736495,0),
(N'Владfgе41лец',784486,205736495,0),
(N'Владfgе5лец',784586,205736495,0),
(N'Вл6адfgелец',784686,205736495,0),
(N'Влад7fgелец',784786,205736495,0),
(N'Владfgеле8ц',784886,205736495,0),
(N'Владfg9елец',784986,205736495,0),
(N'Владfg10елец',784086,205736495,0),
(N'Тест', 4543263,76475865,0);
GO
CREATE PROCEDURE GetAccounts 
AS 
BEGIN
	select Identifier, Passphrase from Account 
		where Deleted=0;
   set nocount on;
end
GO
create procedure CheckNickIfExists(@Nick nvarchar(25) = N'Пользователь')
as
begin
	select Id from Account where Nick=@Nick and Deleted=0;
end
go
CREATE PROCEDURE GetNick (@AccountId int =1)
AS 
BEGIN tran
	select Nick from Account where Id=@AccountId and Deleted=0;
   set nocount on;
commit
GO
CREATE PROCEDURE Register (@LoginHash int =1,@PasswordHash int=1,
@Email nvarchar(50)=N'Электронная почта',@Nick nvarchar(25)=N'Ник')
AS 
BEGIN transaction
	insert into Account values(@Nick,@LoginHash,@PasswordHash,0);
   set nocount on;
commit
GO
CREATE PROCEDURE GetNicks 
AS 
BEGIN
	select Nick from Account where Deleted=0;
   set nocount on;
end
GO
CREATE PROCEDURE GetAccountId (@LoginHash int=1,@PasswordHash int=1)
AS 
BEGIN
	select Id from Account 
	where Identifier=@LoginHash and Passphrase=@PasswordHash and Deleted=0;
   set nocount on;
end
GO
CREATE PROCEDURE GetAccountsCount
AS 
BEGIN
	select Count(Id) from Account where Deleted=0;
   set nocount on;
end
GO
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
go

create table Endpoint_
(
	Id int identity(1,1) not null primary key,
	ForumId int not null,
	Name NVARCHAR(39) NOT NULL default N'КонечнаяТочка',
	check (len(Name)<40),
	foreign key(ForumId)
	references Forum(Id)
	on delete cascade
	on update cascade
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


create table Thread
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name Nvarchar(99) NOT NULL UNIQUE DEFAULT N'Поток',
	EndpointId int NOT NULL default 1,
	check (len(Name)<100),
	foreign key(EndpointId)
	references Endpoint_(Id)
	on delete cascade
	on update cascade
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
	MsgText NVARCHAR(1000) NOT NULL DEFAULT N'Сообщение',
	foreign key(ThreadId)
	references Thread(Id)
	on delete cascade
	on update cascade,
	foreign key(AccountId)
	references Account(Id)
	on delete cascade
	on update cascade
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
	PrivateText NVARCHAR(1000) NOT NULL DEFAULT N'Сообщение',
	foreign key(SenderAccountId)
	references Account(Id)
	on delete cascade
	on update cascade,
	foreign key(AcceptorAccountId)
	references Account(Id)
	on delete no action
	on update no action
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
	(select distinct Account.Id,Nick 
	from Account 
	inner join PrivateMessage 
	on Account.Id=SenderAccountId
	where AcceptorAccountId=@AccountId)
	union 
	(select distinct Account.Id,Nick 
	from Account 
	inner join PrivateMessage 
	on Account.Id=AcceptorAccountId
	where SenderAccountId=@AccountId);
	set nocount on;
end
go
CREATE PROCEDURE GetPrivateMessagesCompanions(@AccountId int=1)
AS 
BEGIN	
	(select distinct Account.Id from Account 
	inner join PrivateMessage on Account.Id=SenderAccountId
	where AcceptorAccountId=@AccountId)union
	(select distinct Account.Id from Account 
	inner join PrivateMessage on Account.Id=AcceptorAccountId
	where SenderAccountId=@AccountId);
	set nocount on;
end
go
CREATE PROCEDURE GetPrivateMessagesAuthorsCount(@AccountId int=1, @CompanionId int=2)
AS 
BEGIN	
set nocount off;
	select count(Id)
	from PrivateMessage 
	where ((SenderAccountId=@AccountId and AcceptorAccountId=@CompanionId)
	or (SenderAccountId=@CompanionId and AcceptorAccountId=@AccountId));
	
end
go
CREATE PROCEDURE GetPrivateMessagesTexts(@AccountId int=1,@CompanionId int=2)
AS 
BEGIN	
	select SenderAccountId,PrivateText
	from PrivateMessage 
	where ((SenderAccountId=@AccountId and AcceptorAccountId=@CompanionId)
	or (SenderAccountId=@CompanionId and AcceptorAccountId=@AccountId))
	order by Id;
	set nocount on;
end
go
CREATE PROCEDURE GetPrivateDialogsCount(@AccountId int=1)
AS 
BEGIN
	select count(Id) 
	from Account
	where Id in (	
	(select SenderAccountId
	from PrivateMessage 
	where AcceptorAccountId=@AccountId)
	union
	(select AcceptorAccountId
	from PrivateMessage 
	where SenderAccountId=@AccountId));
	set nocount on;
end
go
insert into PrivateMessage values (1,2,N'Первое'),
(2,1,N'Второе'),
(1,3,'asdfaf'),
(1,2,'fgfdsg'),
(1,2,'fgfdsg'),
(1,2,'fgfdsg'),
(1,2,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
(3,1,'fgfdsg'),
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
(44,1,'fgfdsg'),
(44,1,'fgfdsg'),
(44,1,'fgfdsg'),
(44,1,'fgfdsg'),
(44,1,'fgfdsg'),
(44,1,'fgfdsg'),
(44,1,'fgfdsg'),
(44,1,'fgfdsg'),
(44,1,'fgfdsg'),
(1,2,N'Третье'),
(2,1,N'Текст'),
(1,2,N'Текст'),
(2,1,N'Текст'),
(1,2,N'Текст'),
(2,1,N'Текст'),
(1,2,N'Текст'),
(2,1,N'Текст'),
(1,2,N'Текст'),
(2,1,N'Текст'),
(1,2,N'Текст'),
(2,1,N'Текст'),
(1,2,N'Текст'),
(2,1,N'Текст'),
(1,2,N'Текст'),
(2,1,N'Текст'),
(1,2,N'Текст'),
(2,1,N'Последнее'),
(4,3,'fgfdsg'),
(5,3,'fgfdsg'),
(6,3,'fgfdsg'),
(7,3,'fgfdsg'),
(8,3,'fgfdsg'),
(11,3,'fgfdsg'),
(12,3,'fgfdsg'),
(4,2,N'Текст'),
(5,2,N'Текст'),
(6,2,N'Текст'),
(7,2,N'Текст'),
(8,2,N'Текст'),
(9,2,N'Текст'),
(10,2,N'Текст'),
(11,2,N'Текст'),
(2,44,'dafasf'),
(3,44,'gfsgsdfg');

create table ContactInfo
(
RecordId int identity(1,1) not null primary key,
UserId int not null default 1,
Phone1 bigint not null default 0,
Phone2 bigint not null default 0,
Phone3 bigint not null default 0,
Deleted bit not null default 0,
check(UserId>0),
check(Phone1>=0),
check(Phone2>=0),
check(Phone3>=0),
unique(Phone1,Phone2,Phone3,Deleted),
foreign key(UserId)
	references Account(Id)
	on delete cascade
	on update cascade
);
insert into ContactInfo values
(1,123,321,213,0),
(2,432,234,423,0);

create table Countries
(
Id int identity(1,1) not null primary key,
Name nvarchar(max) not null,
Deleted bit not null default 0
);


create table Cities
(
Id int identity(1,1) not null primary key,
CountryId int not null default 1,
Name nvarchar(max) not null,
Deleted bit not null default 0,
foreign key(CountryId)
references Countries(Id)
on delete cascade
on update cascade
);

create table Descriptions
(
DescriptionId int identity(1,1) not null primary key,
FullAddress nvarchar(100) not null,
Deleted bit not null default 0,
Airplane bit not null default 0,
Train bit not null default 0,
Gender bit not null default 0,
IdentityCard bit not null default 1,
Conviction bit not null default 0,
Linen bit not null default 0,
FreeInternet bit not null default 1,
OwnComputer bit not null default 0,
Nutrition bit not null default 0,
TravelByCar bit not null default 0,
StreetToilet bit not null default 0,
NeighborsWC bit not null default 0,
HomeToilet bit not null default 0,
Brazier bit not null default 0,
Grill bit not null default 0,
Bathhouse bit not null default 0,
Heating bit not null default 0,
CleanRiver bit not null default 0,
SportsGround bit not null default 0,
Forest bit not null default 0,
Playground bit not null default 0,
NightNoise bit not null default 0,
Park bit not null default 0,
KitchenGarden bit not null default 0,
Garden bit not null default 0,
Weapon bit not null default 0,
AllReligions bit not null default 0,
Above2Rooms bit not null default 0,
DomesticHelp bit not null default 0,
GardenHelp bit not null default 0,
GoodNeighbors bit not null default 0,
Electricity bit not null default 0,
Smokers bit not null default 0,
Drinkers bit not null default 0,
Kitchen bit not null default 0,
ElectricFire bit not null default 0,
GasFurnace bit not null default 0,
WoodStove bit not null default 0,
Cellar bit not null default 0,
Photo bit not null default 0,
Video bit not null default 0,
Guarantor bit not null default 0,
Pets bit not null default 0,
TV bit not null default 0,
Radio bit not null default 0,
Phone bit not null default 0,
Shower bit not null default 0,
Bathroom bit not null default 0,
Washbasin bit not null default 0,
HotWater bit not null default 0,
WaterTower bit not null default 0,
CarSpace bit not null default 0,
Lake bit not null default 0,
Shop bit not null default 0,
Market bit not null default 0,
Sea bit not null default 0,
Mountains bit not null default 0,
Hospital bit not null default 0,
Clinic bit not null default 0,
Kindergarten bit not null default 0,
School bit not null default 0,
Police bit not null default 0,
Court bit not null default 0,
Above2Guests bit not null default 0,
MinAge tinyint not null default 18,
MaxAge tinyint not null default 60,
VisitStart date not null default '2015-10-10',
VisitEnd date not null default '2015-12-10',
Pay bit not null default 0,
HousingArea tinyint not null default 30,
Directions bit not null default 0,
Medicine bit not null default 1
);

create table AddressInfo
(
AddressId int identity(1,1) not null primary key,
CountryId int not null,
CityId int not null,
DescriptionId int not null,
Deleted bit not null default 0,
foreign key (CountryId)
references Countries(Id)
on delete cascade
on update cascade,
foreign key (CityId)
references Cities(Id)
on delete no action
on update no action,
foreign key(DescriptionId)
references Descriptions(DescriptionId)
on delete no action
on update no action
);

create table Addresses
(
RecordId int identity(1,1) not null primary key,
UserId int not null default 1,
AddressId int not null default 0,
Deleted bit not null default 0,
check(UserId>0),
check(AddressId>0),
foreign key(UserId)
references Account(Id)
on delete cascade
on update cascade,
foreign key(AddressId)
references AddressInfo(AddressId)
on delete cascade
on update cascade
);


create table Request
(
RecordId int identity(1,1) not null primary key,
SenderUserId int not null default 1,
AddressId int not null default 1,
Accepted bit not null default 0,
Deleted bit not null default 0,
check(SenderUserId>0),
check(AddressId>0),
foreign key(SenderUserId)
references Account(Id)
on delete cascade
on update cascade,
foreign key(AddressId)
references AddressInfo(AddressId)
on delete no action
on update no action
);







