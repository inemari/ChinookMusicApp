
create table Assistant(
ID int not null primary key identity(1,1), 
Name varChar(30),

);

create table Superhero(
ID int not null primary key identity(1,1),
Name varChar(30) ,
Alias varChar(30),
Origin varChar(30)
);

create table Power( 
 Id int not null primary key identity(1,1),
 Name varChar(50), 
 Description varChar(50)
 );