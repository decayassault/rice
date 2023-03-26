﻿use master
IF (EXISTS(SELECT TOP 1 1 FROM sys.sql_logins WHERE [name] = 'forumadminuser'))
    DROP LOGIN forumadminuser;
drop database if exists TotalForum;
create database TotalForum;
EXEC sp_configure 'CONTAINED DATABASE AUTHENTICATION', 1
GO
RECONFIGURE
GO
USE [master]
GO
ALTER DATABASE TotalForum SET CONTAINMENT = PARTIAL
GO
go
use TotalForum;
drop user if exists forumadminuser;
drop table if exists Account;
drop table if exists Forum;
drop table if exists Thread;
drop table if exists Msg;
drop table if exists Endpoint_;
drop table if exists PrivateMessage;
drop table if exists LoginLog;
create login forumadminuser with password = 'Dctv1ghbdtn';
go
create user forumadminuser for login forumadminuser;
grant all to forumadminuser;
exec sp_addrolemember 'db_owner', 'forumadminuser'
go
create table Account
(
Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
Nick NVARCHAR(25) NOT NULL UNIQUE,
Identifier INT NOT NULL,
Passphrase INT NOT NULL,
EmailHash INT NOT NULL UNIQUE
);
go
ALTER TABLE Account   
ADD CONSTRAINT UQ_Account_LoginPassword UNIQUE (Identifier, Passphrase); 
go
create nonclustered index GetAccounts on Account (Id, Identifier, Passphrase);
create nonclustered index GetNicks on Account(Nick);
create nonclustered index GetNick on Account(Id,Nick);
insert into Account values (N'Тестовый пользователь', 1474365462, - 1959206685, - 73528281);
GO
create table Endpoint_
(
	Id int identity(1,1) not null primary key,
	ForumId int not null,
	Name NVARCHAR(39) NOT NULL
);
create nonclustered index GetEndpointsTop5 on Endpoint_(Id,Name,ForumId);
GO
INSERT INTO Endpoint_ VALUES (1,N'Ищу девушку для брака'),(1,N'Ищу девушку для отношений'),
(1,N'Ищу девушку без прошлого'),(1,N'Ищу девушку постарше'),(1,N'Ищу девушку на ночь'),
(2,N'Ищу парня для брака'),(2,N'Ищу парня для отношений'),
(2,N'Ищу парня без прошлого'),(2,N'Ищу парня постарше'),(2,N'Ищу парня на ночь'),
(3,N'Ищу энтузиастов'),(3,N'Ищу компаньона для съёма жилья'),
(3,N'Ищу компаньона для мероприятий'),(3,N'Ищу попутчика'),(3,N'Ищу благотворителя'),
(4,N'Ищу друзей для игр'),(4,N'Ищу друзей для общения'),
(4,N'Ищу друзей для прогулок'),(4,N'Ищу друзей за рубежом'),(4,N'Ищу друзей для детей'),
(5,N'Юмор'),(5,N'Развлечения'),
(5,N'Семья'),(5,N'Прошу совета'),(5,N'Общее');

create table Forum
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name NVARCHAR(24) NOT NULL UNIQUE
);
create nonclustered index GetForums on Forum(Id,Name);
GO
INSERT INTO Forum VALUES (N'Ищу девушку'),(N'Ищу парня'),
(N'Ищу компаньона'),(N'Ищу друзей'),(N'Беседы');

create table Thread
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name Nvarchar(99) NOT NULL UNIQUE,
	EndpointId int NOT NULL
);
create nonclustered index GetThreads on Thread(Id,Name,EndpointId);
create nonclustered index ThreadsCountByEndpointId on Thread(EndpointId);
create nonclustered index GetThreadName on Thread(Id,Name);
create nonclustered index GetThreadsCount on Thread(Id, EndpointId);
GO
create table Msg
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ThreadId INT NOT NULL,
	AccountId int NOT NULL,
	MsgText NVARCHAR(1000) NOT NULL
);
create nonclustered index GetMessages on Msg(Id,ThreadId,AccountId);
create nonclustered index GetMessagesCount on Msg(ThreadId);
GO
create table PrivateMessage
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SenderAccountId INT NOT NULL,
	AcceptorAccountId int NOT NULL,
	PrivateText NVARCHAR(1000) NOT NULL	
);
create nonclustered index GetPrivateMessagesCount
	on PrivateMessage(Id,SenderAccountId,AcceptorAccountId);
go
insert into PrivateMessage values (1,1,N'Тестовое сообщение');
go
create table LoginLog
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	AccountIdentifier INT NOT NULL,
	IpHash int NOT NULL
);
create nonclustered index GetAccountIdentifiersWithIpHashes
	on LoginLog(AccountIdentifier,IpHash);
