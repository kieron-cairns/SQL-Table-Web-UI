# SQL-Table-Web-UI
A web UI that displays an SQL table, allowing users to search, edit, add and delete table rows in real time

Create a database in SQL server named ItemInfoDb and execute the following command:

```
Create TABLE Stock (ItemId int IDENTITY(1,1) PRIMARY KEY,
Name varchar(255),
Description varchar(255))

INSERT INTO Stock VALUES
('Screw Driver', 'Tools'),
('Allen Key', 'Tools'),
('2mm Bolt x 50', 'DIY'),
('150mm x 6mm 500g bag nails', 'DIY')
```
