# AustinS_MachariaD_Del3 (1)

Completed database-design workbook, extracted as tables. Student ID and email removed; layout and workbook metadata are omitted.

## sheet1

CSCI 2020 201 – Database Fundamentals
Deliverable 3– Physical Design
NAME: Daniel Macharia | Date: 11/19/23 | 24 | EMAIL: [REDACTED_EMAIL]
ENUMBER:[REDACTED_STUDENT_ID]
Table Name | Field Name | Data Type | Nulls Allowed? | Primary Key? | Unique? | Foreign Key  | Description
APPOINTMENT | AppointmentID | NUMBER(8) | NOT NULL | Y | Customer's phone number.
CUSTOMER | First_Name | VARCHAR2(50) | Customer's first name.
CUSTOMER | Last_Name | VARCHAR2(50) | NOT NULL | Customer's last name.
CUSTOMER | Street_Address | VARCHAR2(50) | Customer's street address.
CUSTOMER | City | VARCHAR2(50) | Customer's home city.
CUSTOMER | State | CHAR(2) | Customer's home state.
CUSTOMER | Employee_ID | NUMBER(3, 0) | Y | EMPLOYEE.Employee_ID | Id number of employee who assisted customer.
EMPLOYEE | Employee_ID | NUMBER(3, 0) | NOT NULL | Y | Y | Employee's id number.
EMPLOYEE | First_Name | VARCHAR2(50) | Employee's first name.
EMPLOYEE | Last_Name | VARCHAR2(50) | NOT NULL | Employee's last name.
EMPLOYEE | Position | VARCHAR2(50) | NOT NULL | Employee's job position.
EMPLOYEE | Salary | NUMBER(8,  2) | NOT NULL | Employee's job salary.
EMPLOYEE | Phone_Number | NUMBER(10, 0) | Y | Employee's phone number.
PRODUCT | Product_ID | NUMBER(2, 0) | NOT NULL | Y | Y | Product's id number.
PRODUCT | Name | VARCHAR2(50) | NOT NULL | Product's name.
PRODUCT | Category | VARCHAR2(50) | Product's category.
PRODUCT | List_Price | NUMBER(7, 2) | Product's listing price.
SALE | Phone_Number | NUMBER(10, 0) | NOT NULL | Y | Y | CUSTOMER.Phone_Number | Phone number of customer who initiated sale.
SALE | Employee_ID | NUMBER(3, 0) | NOT NULL | Y | Y | EMPLOYEE.Employee_ID | Id number of employee who completed sale.
SALE | Product_ID | NUMBER(2, 0) | NOT NULL | Y | Y | PRODUCT.Product_ID | Id number of product bought in sale.
SALE | Date | DATE | NOT NULL | Y | Date of sale.
SALE | Quantity | NUMBER(50, 0) | Quantity of product bought.

## sheet2

Index_Name | Table Name | Field(s) | Reason
Idx_Last_Name | CUSTOMER | Last_Name | A customer's last name is commonly remembered first. 
Idx_City | CUSTOMER | City | A number of customers can be filtered out from a single city.
Idx_State | CUSTOMER | State | A number of customers can be filtered out from a single state.
Idx_Emp_Last_Name | EMPLOYEE | Last_Name | An employee's last name is commonly remembered first. 
Idx_Emp_Position | EMPLOYEE | Position | Employees are often distinguished by their job positions.
Idx_Product_Name | PRODUCT | Name | One common way to find a product is by its name.
Idx_Category | PRODUCT | Category | The category of a product is a common search subject.

