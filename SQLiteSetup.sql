-- 1. dotnet tool install -g dotnet-ef OR dotnet tool update -g dotnet-ef
-- 2. dotnet ef migrations add InitialCreate
-- 3. dotnet ef database update
-- 4. Run this script 

insert into Account values (1,'Тестовый пользователь', 1474365462, - 1959206685, - 73528281);
INSERT INTO Endpoint_ VALUES (1,1,'Для спортивных игр'),(2,1,'Для компьютерных игр'),
(3,1,'Для танцев'),(4,1,'Для прогулок'),(5,1,'Для бесед'),
(6,2,'Для спортивных игр'),(7,2,'Для компьютерных игр'),
(8,2,'Для танцев'),(9,2,'Для прогулок'),(10,2,'Для бесед'),
(11,3,'Ищу энтузиастов'),(12,3,'Ищу компаньона для съёма жилья'),
(13,3,'Ищу компаньона для мероприятий'),(14,3,'Ищу попутчика'),(15,3,'Ищу мастера'),
(16,4,'Ищу пенсионеров для игр'),(17,4,'Ищу пенсионеров для общения'),
(18,4,'Ищу пенсионеров для прогулок'),(19,4,'Ищу друзей за рубежом'),(20,4,'Ищу няню'),
(21,5,'Прошу совета'),(22,5,'Прошу помощи'),
(23,5,'Предлагаю помощь'),(24,5,'Обменяю или подарю'),(25,5,'Другое');
INSERT INTO Forum VALUES (1,'Ищу девушку'),(2,'Ищу парня'),
(3,'Ищу компаньона'),(4,'Ищу мудрых'),(5,'Беседы');
insert into PrivateMessage values (1,1,1,'Тестовое сообщение');