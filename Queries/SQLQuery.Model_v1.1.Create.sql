CREATE DATABASE WinTaskManager;

USE WinTaskManager
GO

CREATE TABLE Personel
(  
PersonID uniqueidentifier PRIMARY KEY NOT NULL,
FirstName NCHAR(30),
SurName NCHAR(30),
LastName NCHAR(30),
Division NCHAR(50),
Occupation NCHAR(30),
);
GO

CREATE TABLE Goals
(  
GoalID uniqueidentifier PRIMARY KEY NOT NULL,
Name NCHAR(50) NOT NULL,
Description VARCHAR(4000) NOT NULL,
CreationDate DATETIME NOT NULL,
ExpireDate DATETIME NOT NULL,
Percentage TINYINT NOT NULL,
Priority INT NOT NULL,
StatusKey INT NOT NULL,
);
GO

CREATE TABLE Projects
(  
ProjectID uniqueidentifier PRIMARY KEY NOT NULL,
Name NCHAR(50) NOT NULL,
Description VARCHAR(4000) NOT NULL,
CreationDate DATETIME NOT NULL,
ExpireDate DATETIME NOT NULL,
Percentage TINYINT NOT NULL,
StatusKey INT NOT NULL,
);
GO

CREATE TABLE GoalsToProjects
(  
GoalID uniqueidentifier UNIQUE FOREIGN KEY REFERENCES Goals(GoalID),
ProjectID uniqueidentifier FOREIGN KEY REFERENCES Projects(ProjectID),
);
GO

CREATE TABLE PersonelToProjects
( 
PersonID uniqueidentifier FOREIGN KEY REFERENCES Personel(PersonID),
ProjectID uniqueidentifier FOREIGN KEY REFERENCES Projects(ProjectID),
);
GO

CREATE TABLE PersonelToGoals
(  
PersonID uniqueidentifier FOREIGN KEY REFERENCES Personel(PersonID),
GoalID uniqueidentifier FOREIGN KEY REFERENCES Goals(GoalID),
);
GO

USE master
GO