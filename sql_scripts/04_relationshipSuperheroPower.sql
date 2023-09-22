CREATE TABLE SuperheroPower (
    SuperheroId INT,
    PowerId INT,
    PRIMARY KEY (SuperheroId, PowerId),
    FOREIGN KEY (SuperheroId) REFERENCES Superhero(ID),
    FOREIGN KEY (PowerId) REFERENCES Power(ID)
);
