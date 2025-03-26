create database DbEcommerce;
use DbEcommerce;
create table Category(
Id INT PRIMARY KEY AUTO_INCREMENT,
Name VARCHAR(50) NOT NULL,
CreationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL
);

create table Product(
Id INT PRIMARY KEY AUTO_INCREMENT,
Name VARCHAR(50) NOT NULL,
Description VARCHAR(5000),
CategoryId INT,
Price DECIMAL (10,2)NOT NULL,
OfferPrice DECIMAL(10,2) NOT NULL,
Stock int not null,
Image varchar(255),
CreationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
FOREIGN KEY (categoryId) REFERENCES Category(Id) ON DELETE SET NULL
);

create table User(
Id int primary key auto_increment,
FullName varchar(50) not null,
Email varchar(50) not null,
Password varchar(50) not null,
Role varchar(50) not null,
CreationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL
);

create table Sale(
Id int primary key auto_increment,
userId int not null,
Total decimal(10,2) not null,
CreationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
FOREIGN KEY (UserId) REFERENCES User(Id)
);

create table SaleDetail
(
Id int primary key  auto_increment,
SaleId int not null,
ProductId int not null,
ProductName varchar(50),
Quantity int not null,
Total decimal(10,2) not null,
FOREIGN KEY (SaleId) REFERENCES Sale(Id)
);


--insertamos un usuario para poder iniciar sesion

insert into User(FullName,Email,Password,Role) values
('admin','admin@example.com','123','Admin');









