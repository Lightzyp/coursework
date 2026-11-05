# MachariaD_Del3 (1)

Completed database-design workbook, extracted as tables. Student ID and email removed; layout and workbook metadata are omitted.

## sheet1

CSCI 2020 201 – Database Fundamentals
Deliverable 3– Physical Design
NAME: Daniel Macharia | Date:11/19/24 | EMAIL: [REDACTED_EMAIL]
ENUMBER:[REDACTED_STUDENT_ID]
Table Name | Field Name | Data Type | Nulls Allowed | Primary Key | Unique | Foreign Key  | Description
APPOINTMENT | AppointmentID | NUMBER(8) | NOT NULL | Y | BILLING 
APPOINTMENT | PatientID | NUMBER(8) | NOT NULL | PATIENT.PatientID | Foreign key referencing patient.
APPOINTMENT | Date | DATE | NOT NULL | Date of the appointment.
APPOINTMENT | Time | VARCHAR2(10) | NOT NULL | Time of the appointment.
BILLING | BillingID | NUMBER(8) | NOT NULL | Y | Primary key for billing records.
BILLING | AppointmentID | NUMBER(8) | NOT NULL | APPOINTMENT.AppointmentID | Foreign key referencing appointment.
BILLING | Amount | NUMBER(10,2) | NOT NULL | Billing amount with two decimal precision.
BILLING | Status | VARCHAR2(20) | NOT NULL | Payment status of the bill.
BILLING | PaymentDate | DATE | Date the payment was made, if applicable.
CONSULTATION | ConsultationID | NUMBER(8) | NOT NULL | Y | Primary key for consultations.
CONSULTATION | AppointmentID | NUMBER(8) | NOT NULL | APPOINTMENT.AppointmentID | Foreign key referencing appointment.
CONSULTATION | DoctorID | NUMBER(8) | NOT NULL | DOCTOR.DoctorID | Foreign key referencing doctor.
CONSULTATION | Diagnosis | VARCHAR2(255) | Diagnosis details.
CONSULTATION | Notes | VARCHAR2(500) | Additional consultation notes.
CONSULTATION | TreatmentPlanID | NUMBER(8) | NOT NULL | TREATMENT_PLAN.TreatmentPlanID | Foreign key referencing treatment plan.
DOCTOR | DoctorID | NUMBER(8) | NOT NULL | Y | Primary key for doctors.
DOCTOR | Name | VARCHAR2(100) | NOT NULL | Name of the doctor.
DOCTOR | Specialty | VARCHAR2(100) | NOT NULL | Specialization area of the doctor.
DOCTOR | ContactInfo | VARCHAR2(255) | Contact information for the doctor.
MEDICATION | MedicationID | NUMBER(8) | NOT NULL | Y | Primary key for medications.
MEDICATION | Name | VARCHAR2(100) | NOT NULL | Name of the medication.
MEDICATION | Dosage | VARCHAR2(50) | NOT NULL | Dosage instructions.
MEDICATION | Frequency | VARCHAR2(50) | NOT NULL | Frequency of administration.
MEDICATION | PatientID | NUMBER(8) | NOT NULL | PATIENT.PatientID | Foreign key referencing patient.
PATIENT | PatientID | NUMBER(8) | NOT NULL | Y | Primary key for patients.
PATIENT | Name | VARCHAR2(100) | NOT NULL | Name of the patient.
PATIENT | Address | VARCHAR2(255) | Address of the patient.
PATIENT | ContactInfo | VARCHAR2(255) | NOT NULL | Contact information for the patient.
PATIENT | DateOfBirth | DATE | NOT NULL | Patient's date of birth.
TREATMENT_PLAN | TreatmentPlanID | NUMBER(8) | NOT NULL | Y | Primary key for treatment plans.
TREATMENT_PLAN | ConsultationID | NUMBER(8) | NOT NULL | CONSULTATION.ConsultationID | Foreign key referencing consultation.
TREATMENT_PLAN | MedicationID | NUMBER(8) | NOT NULL | MEDICATION.MedicationID | Foreign key referencing medication.
TREATMENT_PLAN | Duration | NUMBER(3) | NOT NULL | Duration of the treatment plan in days.

## sheet2

Index Name | Table Name | Field(s) | Reason
idx_appointment_patientid | APPOINTMENT | PatientID | Field will be commonly used in lookups.
idx_billing_appointmentid | BILLING | AppointmentID | Field will be commonly used in lookups.
idx_consultation_appointmentid | CONSULTATION | AppointmentID | Field will be commonly used in lookups.
idx_consultation_doctorid | CONSULTATION | DoctorID | Field will be commonly used in lookups.
idx_medication_patientid | MEDICATION | PatientID | Field will be commonly used in lookups.
idx_treatmentplan_consultationid | TREATMENT_PLAN | ConsultationID | Field will be commonly used in lookups.

