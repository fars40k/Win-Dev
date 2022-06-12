USE WinTaskManager
GO

-- PersonID, FirstName, SurName, LastName, Division, Occupation,
INSERT INTO Personel
VALUES 
(NEWID(),'Name1','Sur1','Last1','Division1','SEO'),
(NEWID(),'Name2','','Last2','Division1','Driver'),
(NEWID(),'Name3','Sur3','Last3','Division2','Manager1'),
(NEWID(),'Name3','Sur3','Last4','Division2','Manager2'),
(NEWID(),'Name4','Sur4','Last4','Division3','Manager3'),
(NEWID(),'Name5','','','','FreelanceGuy'),
(NEWID(),'Name6','Sur6','Last6','Division2','Operator'),
(NEWID(),'Name7','Sur7','Last7','Division3','Seller'),
(NEWID(),'','','','','');

--GoalID, Name, Description, CreationDate, ExpireDate, Percentage, Priority, StatusKey,
INSERT INTO Goal
VALUES 

--ProjectID, Name, Description, CreationDate, ExpireDate, Percentage, StatusKey,


USE master
GO