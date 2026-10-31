-- CareerTrack Minor Project 5
-- SQLite schema reconstructed from EF Core InitialCreate migration.
-- Run on a NEW, EMPTY SQLite database. Do not run against the existing careertrack.db.
-- EF Core maxLength metadata on TEXT columns is not enforced by SQLite here.
PRAGMA foreign_keys = ON;

CREATE TABLE "ApplicationStatuses" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ApplicationStatuses" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL
);

CREATE TABLE "Companies" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Companies" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Industry" TEXT NULL,
    "Location" TEXT NULL,
    "Website" TEXT NULL
);

CREATE TABLE "Contacts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Contacts" PRIMARY KEY AUTOINCREMENT,
    "FullName" TEXT NOT NULL,
    "Email" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "JobTitle" TEXT NULL,
    "Notes" TEXT NULL,
    "CompanyId" INTEGER NOT NULL,
    CONSTRAINT "FK_Contacts_Companies_CompanyId" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON DELETE CASCADE
);

CREATE TABLE "JobApplications" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_JobApplications" PRIMARY KEY AUTOINCREMENT,
    "JobTitle" TEXT NOT NULL,
    "DateApplied" TEXT NOT NULL,
    "Location" TEXT NULL,
    "JobUrl" TEXT NULL,
    "Salary" TEXT NULL,
    "Notes" TEXT NULL,
    "CompanyId" INTEGER NOT NULL,
    "ApplicationStatusId" INTEGER NOT NULL,
    CONSTRAINT "FK_JobApplications_ApplicationStatuses_ApplicationStatusId" FOREIGN KEY ("ApplicationStatusId") REFERENCES "ApplicationStatuses" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_JobApplications_Companies_CompanyId" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Interviews" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Interviews" PRIMARY KEY AUTOINCREMENT,
    "InterviewType" TEXT NOT NULL,
    "InterviewDate" TEXT NOT NULL,
    "Location" TEXT NULL,
    "Outcome" TEXT NULL,
    "Notes" TEXT NULL,
    "JobApplicationId" INTEGER NOT NULL,
    CONSTRAINT "FK_Interviews_JobApplications_JobApplicationId" FOREIGN KEY ("JobApplicationId") REFERENCES "JobApplications" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Contacts_CompanyId" ON "Contacts" ("CompanyId");
CREATE INDEX "IX_Interviews_JobApplicationId" ON "Interviews" ("JobApplicationId");
CREATE INDEX "IX_JobApplications_ApplicationStatusId" ON "JobApplications" ("ApplicationStatusId");
CREATE INDEX "IX_JobApplications_CompanyId" ON "JobApplications" ("CompanyId");
