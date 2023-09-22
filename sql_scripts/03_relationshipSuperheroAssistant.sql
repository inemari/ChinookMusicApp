alter table Assistant
Add SuperID int,
FOREIGN KEY (SuperID) REFERENCES Superhero(ID);