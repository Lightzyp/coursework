/*
-----------------------------------------------------------------------------
Programmer's Name: Dnaiel Macharia
Course: CSCI 2020 Section Number: 201
Creation Date: 12/1/2024 Date of Last Modification: 12/1/2024
E-mail Address: [REDACTED EMAIL]
----------------------------------------------------------------------------
Purpose -
Lab 3 – Basic SELECT Statements
-----------------------------------------------------------------------------
Identifier dictionary -
Not Applicable
-----------------------------------------------------------------------------
Notes and Assumptions –
-----------------------------------------------------------------------------
*/

-- Clean up any existing tables
DROP TABLE CONGRESSIONAL_RECORD CASCADE CONSTRAINTS;
DROP TABLE CONGRESSPERSON CASCADE CONSTRAINTS;
DROP TABLE BILL CASCADE CONSTRAINTS;
DROP TABLE STATE CASCADE CONSTRAINTS;

-- Creating the STATE table first as it is referenced by other tables
CREATE TABLE STATE (
    Name CHAR(2) PRIMARY KEY,
    Region VARCHAR2(9) NOT NULL
);

-- Creating the CONGRESSPERSON table, which references the STATE table
CREATE TABLE CONGRESSPERSON (
    ID NUMBER PRIMARY KEY,
    First_Name VARCHAR2(15) NOT NULL,
    Last_Name VARCHAR2(15) NOT NULL,
    Start_Date DATE NOT NULL,
    Political_Party VARCHAR2(15) NOT NULL,
    District VARCHAR2(30),
    State CHAR(2) NOT NULL
);

-- Creating the BILL table
CREATE TABLE BILL (
    ID NUMBER PRIMARY KEY,
    Bill_Name VARCHAR2(100) NOT NULL UNIQUE,
    Voting_Date DATE NOT NULL,
    Is_Passed CHAR(3) NOT NULL
);

-- Creating the CONGRESSIONAL_RECORD table, which references both the BILL and CONGRESSPERSON tables
CREATE TABLE CONGRESSIONAL_RECORD (
    Bill_NUMBER NUMBER,
    Congressperson_ID NUMBER,
    Vote VARCHAR2(6) NOT NULL,
    Is_Sponsor CHAR(3),
    PRIMARY KEY (Bill_NUMBER, Congressperson_ID)
);

-- Add foreign key constraints
ALTER TABLE CONGRESSPERSON 
    ADD CONSTRAINT fk_congressperson_state FOREIGN KEY (State) REFERENCES STATE (Name);

ALTER TABLE CONGRESSIONAL_RECORD 
    ADD CONSTRAINT fk_congressperson FOREIGN KEY (Congressperson_ID) REFERENCES CONGRESSPERSON (ID);

ALTER TABLE CONGRESSIONAL_RECORD 
    ADD CONSTRAINT fk_bill FOREIGN KEY (Bill_NUMBER) REFERENCES BILL (ID);
    
    
    
    
/*
Reflection:

- The importance of learning this activity is to understand how to implement a relational database with multiple tables and relationships. This activity covers creating tables, defining primary keys, foreign keys, and understanding their relationships in real-world scenarios.
  
- One of the key implementation concepts covered this week was how to use composite primary keys and how foreign keys reference them. Understanding these relationships is critical in maintaining data integrity and implementing efficient database design.
  
- What I found interesting in this activity was designing the relationships between the tables and understanding how the foreign key constraints work to ensure that the data in related tables remains consistent.

- What I found challenging was ensuring that the foreign key constraints were applied in the correct order and managing any errors related to foreign key references.
*/

