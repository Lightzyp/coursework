/*
-----------------------------------------------------------------------------
Programmer's Name: Dnaiel Macharia
Course: CSCI 2020 Section Number: 201
Creation Date: 04/18/2024 Date of Last Modification: 09/18/2024
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

-- Query 1: Select employees whose last name starts with 'M'
SELECT * 
FROM HR.Employees 
WHERE last_name LIKE 'M%';

-- Query 2: Select employees with phone numbers in ###.###.#### format
SELECT * 
FROM HR.Employees 
WHERE phone_number LIKE '___.___.____';

-- Query 3: Select employees whose phone numbers follow the pattern ###.##.####.######
SELECT * 
FROM HR.Employees 
WHERE REGEXP_LIKE(phone_number, '^\d{3}\.\d{2}\.\d{4}\.\d{6}$');

--Activity B: String Manipulation Queries

-- Query 1: Select full names in the format "Last, First"
SELECT last_name || ', ' || first_name AS full_name
FROM HR.Employees;

-- Query 2: Select employee ID in format "L-F-###"
SELECT SUBSTR(last_name, 1, 1) || SUBSTR(first_name, 1, 1) || '-' || employee_id AS emp_id_format
FROM HR.Employees;

-- Query 3: Pad city names with '*' and order by city length
SELECT LPAD(city, LENGTH(city) + 1, '*') AS padded_city
FROM HR.Locations
ORDER BY LENGTH(city);

-- Query 4: Display full address with indentation and line breaks
SELECT street_address || CHR(13) || '     ' || city || ', ' || state_province || ', ' || postal_code || ', ' || country_id AS full_address
FROM HR.Locations;

-- Query 5: Select employee last name and formatted salary
SELECT last_name, TO_CHAR(salary, '$9999.99') AS formatted_salary
FROM HR.Employees;

-- Query 6: Display today's date in six formats
SELECT 
   TO_CHAR(SYSDATE, 'MMYYYY') AS date_format1,
   TO_CHAR(SYSDATE, 'MON-YYYY') AS date_format2,
   TO_CHAR(SYSDATE, 'YYYY/MM/DD') AS date_format3,
   TO_CHAR(SYSDATE, 'Month DD, YYYY') AS date_format4,
   TO_CHAR(SYSDATE, 'Day, Month DD, YYYY') AS date_format5,
   TO_CHAR(SYSDATE, 'Month DD, YYYY HH:MI:SS PM') AS date_format6
FROM DUAL;

/* 
Reflection:
This activity demonstrated the importance of string manipulation and matching in SQL, which is crucial for data querying and reporting. By learning how to manipulate strings, I can better format and extract data to meet specific business requirements. 

I struggled slightly with the regular expressions but reviewing the syntax more and practicing will help improve my understanding. Additional practice and examples would assist me in mastering the REGEXP_LIKE function. 
*/
