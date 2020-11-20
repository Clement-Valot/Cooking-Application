DROP DATABASE if Exists Cooking;
CREATE DATABASE Cooking;

drop table if exists Cooking.client;
drop table if exists Cooking.recette;
drop table if exists Cooking.cdr;
drop table if exists Cooking.produit;
drop table if exists Cooking.fournisseur;
drop table if exists Cooking.necessiter;
drop table if exists Cooking.commande;
drop table if exists Cooking.contenir;

CREATE TABLE Cooking.client (
  identifiant_client INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  mail VARCHAR(30) NOT NULL,
  mdp VARCHAR(20) NOT NULL,
  nom VARCHAR(20),
  prenom VARCHAR(20),
  solde INT DEFAULT 0,
  telephone VARCHAR(10));
  
CREATE TABLE Cooking.cdr (
  identifiant_cdr INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  identifiant_client INT NOT NULL,
  CONSTRAINT fk_id_client FOREIGN KEY(identifiant_client) REFERENCES client(identifiant_client) ON DELETE CASCADE);
 
CREATE TABLE Cooking.commande (
  ref_commande INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  date DATE,
  total INT,
  identifiant_client INT NOT NULL,
  CONSTRAINT fk_id_client_cmd FOREIGN KEY (identifiant_client) REFERENCES client(identifiant_client) ON DELETE NO ACTION);
 
CREATE TABLE Cooking.recette (
  ref_recette INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  nom VARCHAR(50) NOT NULL,
  type VARCHAR(50) NOT NULL,
  descriptif VARCHAR(256) NOT NULL,
  prix INT NOT NULL,
  remuneration INT DEFAULT 2,
  nbr_commande INT DEFAULT 0,
  identifiant_cdr INT NOT NULL,
  CONSTRAINT fk_id_cdr FOREIGN KEY (identifiant_cdr) REFERENCES cdr(identifiant_cdr) ON DELETE CASCADE);
  
CREATE TABLE Cooking.contenir (
  ref_commande INT NOT NULL,
  ref_recette INT NOT NULL,
  nombre INT NOT NULL,
  CONSTRAINT fk_ref_com FOREIGN KEY (ref_commande) REFERENCES commande(ref_commande) ON DELETE NO ACTION,
  CONSTRAINT fk_ref_rec FOREIGN KEY (ref_recette) REFERENCES recette(ref_recette) ON DELETE CASCADE);
  
CREATE TABLE Cooking.fournisseur (
  ref_fournisseur INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  nom VARCHAR(30) NOT NULL,
  tel VARCHAR(10) NOT NULL);
  
CREATE TABLE Cooking.produit (
  ref_produit INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  nom VARCHAR(30),
  categorie ENUM("fruit","legume","condiment","produit laitier") NOT NULL,
  unite ENUM("L","mL","kg","g","c. a soupe","c. a cafe","piece","pincee","goutte"),
  stock_actuel INT DEFAULT 0,
  stock_min INT,
  stock_max INT,
  ref_fournisseur INT NOT NULL,
  CONSTRAINT fk_ref_fournisseur FOREIGN KEY (ref_fournisseur) REFERENCES fournisseur(ref_fournisseur)
  ON DELETE NO ACTION);
  
CREATE TABLE Cooking.necessiter (
  ref_produit INT NOT NULL,
  ref_recette INT NOT NULL,
  quantite INT NOT NULL,
  CONSTRAINT fk_ref_produit FOREIGN KEY (ref_produit) REFERENCES produit(ref_produit) ON DELETE CASCADE,
  CONSTRAINT fk_ref_recette FOREIGN KEY (ref_recette) REFERENCES recette(ref_recette) ON DELETE CASCADE);
  
/*Créer les instances des tables*/
INSERT INTO Cooking.client (`mail`,`mdp`,`nom`,`prenom`,`telephone`) VALUES ('val','az','VALOT','Clement','0781273239');
INSERT INTO Cooking.client (`mail`,`mdp`,`nom`,`prenom`,`telephone`) VALUES ('vince','wx','DELABRE','Vincent','0678901234');
INSERT INTO Cooking.cdr (`identifiant_client`) VALUES (1);

INSERT INTO Cooking.recette (`nom`,`type`,`descriptif`,`prix`, `identifiant_cdr`) VALUES ('potage','plat','très bon plat chaud', 20,1);
INSERT INTO Cooking.recette (`nom`,`type`,`descriptif`,`prix`, `identifiant_cdr`) VALUES ('carottes rapées','entrée','Entrée rafraîchissante', 10,1);


INSERT INTO Cooking.fournisseur (`nom`,`tel`) VALUES ('Carrefour','0566775544');
INSERT INTO Cooking.fournisseur (`nom`,`tel`) VALUES ('Leclerc','0568635914');
INSERT INTO Cooking.fournisseur (`nom`,`tel`) VALUES ('Marchant Croissy','0682413468');

INSERT INTO Cooking.produit (`categorie`,`unite`,`nom`,`stock_actuel`,`stock_min`,`stock_max`,`ref_fournisseur`) VALUES 
('legume','kg','pomme de terre',13,2,20,1),
('legume','kg','carotte',10,5,20,3),
('fruit','kg','tomate',20,2,30,2),
('fruit','piece','citron',5,6,46,3);

INSERT INTO Cooking.necessiter(`ref_recette`,`ref_produit`,`quantite`) VALUES 
(1,2,3),
(1,1,2),
(2,4,1),
(2,2,3);

INSERT INTO Cooking.commande(`date`,`total`,`identifiant_client`) VALUES 
("2020-04-07",40,1);

INSERT INTO Cooking.contenir(`ref_commande`,`ref_recette`,`nombre`) VALUES 
(1,1,3),
(1,2,4);