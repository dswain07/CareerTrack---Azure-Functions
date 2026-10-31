-- CareerTrack Minor Project 5 - DML sample records
-- Derived from DbSeeder.SeedAsync and UpdateSampleApplicationsAsync.
-- Run ONCE on a fresh database after DDL.sql.
-- IDs are explicit to keep foreign-key relationships reproducible.
PRAGMA foreign_keys = ON;
BEGIN TRANSACTION;

INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (1, 'Microsoft', 'Technology', 'Redmond, WA', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (2, 'Google', 'Technology', 'Mountain View, CA', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (3, 'Amazon', 'Technology', 'Seattle, WA', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (4, 'Apple', 'Technology', 'Cupertino, CA', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (5, 'IBM', 'Technology', 'Armonk, NY', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (6, 'NVIDIA', 'Semiconductors', 'Santa Clara, CA', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (7, 'Meta', 'Technology', 'Menlo Park, CA', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (8, 'Adobe', 'Software', 'San Jose, CA', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (9, 'Oracle', 'Software', 'Austin, TX', NULL);
INSERT INTO "Companies" ("Id", "Name", "Industry", "Location", "Website") VALUES (10, 'Salesforce', 'Software', 'San Francisco, CA', NULL);

INSERT INTO "ApplicationStatuses" ("Id", "Name", "Description") VALUES (1, 'Saved', 'Job saved for later');
INSERT INTO "ApplicationStatuses" ("Id", "Name", "Description") VALUES (2, 'Applied', 'Application submitted');
INSERT INTO "ApplicationStatuses" ("Id", "Name", "Description") VALUES (3, 'Under Review', 'Application being reviewed');
INSERT INTO "ApplicationStatuses" ("Id", "Name", "Description") VALUES (4, 'Interview', 'Interview process started');
INSERT INTO "ApplicationStatuses" ("Id", "Name", "Description") VALUES (5, 'Offer', 'Job offer received');
INSERT INTO "ApplicationStatuses" ("Id", "Name", "Description") VALUES (6, 'Rejected', 'Application rejected');
INSERT INTO "ApplicationStatuses" ("Id", "Name", "Description") VALUES (7, 'Withdrawn', 'Application withdrawn');

INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (1, 'Software Engineer', '2026-10-01 00:00:00', 'Redmond, WA', 'https://example.com/jobs/software-engineer', '120000', 'Sample software engineering application', 1, 2);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (2, 'Data Scientist', '2026-10-02 00:00:00', 'Mountain View, CA', 'https://example.com/jobs/data-scientist', '135000', 'Sample data science application', 2, 3);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (3, 'Backend Developer', '2026-10-03 00:00:00', 'Seattle, WA', 'https://example.com/jobs/backend-developer', '110000', 'Sample backend development application', 3, 4);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (4, 'iOS Developer', '2026-10-04 00:00:00', 'Cupertino, CA', 'https://example.com/jobs/ios-developer', '125000', 'Sample iOS development application', 4, 2);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (5, 'Cloud Engineer', '2026-10-05 00:00:00', 'Armonk, NY', 'https://example.com/jobs/cloud-engineer', '115000', 'Sample cloud engineering application', 5, 6);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (6, 'Machine Learning Engineer', '2026-10-06 00:00:00', 'Santa Clara, CA', 'https://example.com/jobs/ml-engineer', '145000', 'Sample machine learning application', 6, 4);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (7, 'Frontend Developer', '2026-10-07 00:00:00', 'Menlo Park, CA', 'https://example.com/jobs/frontend-developer', '105000', 'Sample frontend development application', 7, 2);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (8, 'Full Stack Developer', '2026-10-08 00:00:00', 'San Jose, CA', 'https://example.com/jobs/fullstack-developer', '120000', 'Sample full stack development application', 8, 5);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (9, 'Database Engineer', '2026-10-09 00:00:00', 'Austin, TX', 'https://example.com/jobs/database-engineer', '115000', 'Sample database engineering application', 9, 3);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (10, 'Software Developer', '2026-10-10 00:00:00', 'San Francisco, CA', 'https://example.com/jobs/software-developer', '110000', 'Sample software development application', 10, 2);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (11, 'AI Engineer', '2026-10-11 00:00:00', 'Redmond, WA', 'https://example.com/jobs/ai-engineer', '140000', 'Sample AI engineering application', 1, 4);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (12, 'DevOps Engineer', '2026-10-12 00:00:00', 'Seattle, WA', 'https://example.com/jobs/devops-engineer', '125000', 'Sample DevOps application', 3, 6);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (13, 'Research Engineer', '2026-10-13 00:00:00', 'Santa Clara, CA', 'https://example.com/jobs/research-engineer', '135000', 'Sample research engineering application', 6, 3);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (14, 'Systems Engineer', '2026-10-14 00:00:00', 'Armonk, NY', 'https://example.com/jobs/systems-engineer', '110000', 'Sample systems engineering application', 5, 2);
INSERT INTO "JobApplications" ("Id", "JobTitle", "DateApplied", "Location", "JobUrl", "Salary", "Notes", "CompanyId", "ApplicationStatusId") VALUES (15, 'Security Engineer', '2026-10-15 00:00:00', 'Mountain View, CA', 'https://example.com/jobs/security-engineer', '130000', 'Sample security engineering application', 2, 7);

INSERT INTO "Interviews" ("Id", "InterviewType", "InterviewDate", "Location", "Outcome", "Notes", "JobApplicationId") VALUES (1, 'Phone Screening', '2026-10-20 10:00:00', NULL, 'Passed', NULL, 3);
INSERT INTO "Interviews" ("Id", "InterviewType", "InterviewDate", "Location", "Outcome", "Notes", "JobApplicationId") VALUES (2, 'Technical Interview', '2026-10-25 14:00:00', NULL, 'Pending', NULL, 3);
INSERT INTO "Interviews" ("Id", "InterviewType", "InterviewDate", "Location", "Outcome", "Notes", "JobApplicationId") VALUES (3, 'Technical Interview', '2026-10-27 11:00:00', NULL, 'Pending', NULL, 6);
INSERT INTO "Interviews" ("Id", "InterviewType", "InterviewDate", "Location", "Outcome", "Notes", "JobApplicationId") VALUES (4, 'HR Interview', '2026-10-28 09:00:00', NULL, 'Passed', NULL, 8);
INSERT INTO "Interviews" ("Id", "InterviewType", "InterviewDate", "Location", "Outcome", "Notes", "JobApplicationId") VALUES (5, 'Final Interview', '2026-10-30 15:00:00', NULL, 'Pending', NULL, 11);

INSERT INTO "Contacts" ("Id", "FullName", "Email", "PhoneNumber", "JobTitle", "Notes", "CompanyId") VALUES (1, 'Sarah Johnson', 'sarah@example.com', NULL, 'Recruiter', NULL, 1);
INSERT INTO "Contacts" ("Id", "FullName", "Email", "PhoneNumber", "JobTitle", "Notes", "CompanyId") VALUES (2, 'Michael Brown', 'michael@example.com', NULL, 'Hiring Manager', NULL, 3);
INSERT INTO "Contacts" ("Id", "FullName", "Email", "PhoneNumber", "JobTitle", "Notes", "CompanyId") VALUES (3, 'Emily Davis', 'emily@example.com', NULL, 'Technical Recruiter', NULL, 6);

COMMIT;