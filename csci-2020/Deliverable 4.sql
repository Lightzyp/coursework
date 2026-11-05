/*
-----------------------------------------------------------------------------
Programmer's Name: Daniel Macharia
Course: CSCI 2020 Section Number: 201
Creation Date: 12/02/2024 Date of Last Modification: 12/06/2024
E-mail Address: [REDACTED EMAIL]
-----------------------------------------------------------------------------
Purpose - Implementation of Deliverable 3  
Lab 2 – Basic SELECT Statements  
-----------------------------------------------------------------------------
Identifier dictionary -  
Not Applicable  
-----------------------------------------------------------------------------
Notes and Assumptions –  
1. This implementation assumes that the database schema and data are correctly set up.   
-----------------------------------------------------------------------------
*/


DROP TABLE BILLING CASCADE CONSTRAINTS;
DROP TABLE APPOINTMENT CASCADE CONSTRAINTS;
DROP TABLE PATIENT CASCADE CONSTRAINTS;

CREATE TABLE PATIENT (
    PatientID NUMBER(8) PRIMARY KEY,
    Name VARCHAR2(100) NOT NULL
);

CREATE TABLE APPOINTMENT (
    AppointmentID NUMBER(8) PRIMARY KEY,
    PatientID NUMBER(8) NOT NULL,
    "Date" DATE NOT NULL,
    "Time" VARCHAR2(10) NOT NULL,
    CONSTRAINT fk_patient FOREIGN KEY (PatientID) REFERENCES PATIENT(PatientID)
);

CREATE TABLE BILLING (
    BillingID NUMBER(8) PRIMARY KEY,
    AppointmentID NUMBER(8),
    Amount NUMBER(10, 2) NOT NULL,
    BillingDate DATE NOT NULL,
    CONSTRAINT fk_appointment FOREIGN KEY (AppointmentID) REFERENCES APPOINTMENT(AppointmentID)
);

INSERT INTO PATIENT (PatientID, Name) VALUES (101, 'John Doe');
INSERT INTO PATIENT (PatientID, Name) VALUES (102, 'Jane Doe');

INSERT INTO APPOINTMENT (AppointmentID, PatientID, "Date", "Time") 
VALUES (1, 101, TO_DATE('2024-12-01', 'YYYY-MM-DD'), '10:00 AM');
INSERT INTO APPOINTMENT (AppointmentID, PatientID, "Date", "Time") 
VALUES (2, 102, TO_DATE('2024-12-02', 'YYYY-MM-DD'), '11:00 AM');

INSERT INTO BILLING (BillingID, AppointmentID, Amount, BillingDate)
VALUES (1, 1, 150.00, TO_DATE('2024-12-03', 'YYYY-MM-DD'));
INSERT INTO BILLING (BillingID, AppointmentID, Amount, BillingDate)
VALUES (2, 2, 200.00, TO_DATE('2024-12-04', 'YYYY-MM-DD'));

UPDATE APPOINTMENT SET "Time" = '9:00 AM' WHERE AppointmentID = 1;

DELETE FROM BILLING WHERE BillingID = 1;
DELETE FROM APPOINTMENT WHERE AppointmentID = 1;

SELECT * FROM PATIENT;
SELECT * FROM APPOINTMENT;
SELECT * FROM BILLING;
