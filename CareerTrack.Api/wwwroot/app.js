const API_BASE = "/api";

let companiesData = [];
let applicationsData = [];
let interviewsData = [];

// ==========================================
// API Communication
// ==========================================

async function apiRequest(endpoint, options = {}) {
    const response = await fetch(`${API_BASE}/${endpoint}`, options);

    if (!response.ok) {
        const message = await response.text();
        throw new Error(message || `HTTP ${response.status}`);
    }

    if (response.status === 204) {
        return null;
    }

    return await response.json();
}

// ==========================================
// Navigation
// ==========================================

function showSection(sectionId) {
    document.querySelectorAll(".content-section").forEach(section => {
        section.classList.add("hidden");
    });

    document.getElementById(sectionId).classList.remove("hidden");
}

// ==========================================
// Load Dashboard Data
// ==========================================

async function loadDashboard() {
    try {
        const [companies, applications, interviews] = await Promise.all([
            apiRequest("companies"),
            apiRequest("jobapplications"),
            apiRequest("interviews")
        ]);

        companiesData = companies;
        applicationsData = applications;
        interviewsData = interviews;

        document.getElementById("companyCount").textContent =
            companies.length;

        document.getElementById("applicationCount").textContent =
            applications.length;

        document.getElementById("interviewCount").textContent =
            interviews.length;

        renderCompanies();
        renderApplications();
        renderInterviews();
        renderRecentApplications();

    } catch (error) {
        console.error("Error loading dashboard:", error);
        alert("Unable to load CareerTrack data.");
    }
}

// ==========================================
// Helper Functions
// ==========================================

function getCompanyName(companyId) {
    const company = companiesData.find(c => c.id === companyId);

    return company ? company.name : `Company #${companyId}`;
}

function formatSalary(value) {
    if (value === null || value === undefined) {
        return "Not specified";
    }

    return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        maximumFractionDigits: 0
    }).format(value);
}

function formatDate(value) {
    if (!value) {
        return "Not specified";
    }

    return new Date(value).toLocaleDateString("en-US", {
        year: "numeric",
        month: "short",
        day: "numeric"
    });
}

function showMessage(message, type = "success") {
    const element = document.getElementById("companyMessage");

    element.textContent = message;
    element.className = `message ${type}`;
}

function createElement(tag, text, className = "") {
    const element = document.createElement(tag);

    element.textContent = text ?? "";

    if (className) {
        element.className = className;
    }

    return element;
}

function createButton(label, handler, className = "") {
    const button = document.createElement("button");

    button.type = "button";
    button.textContent = label;
    button.className = className;

    button.addEventListener("click", handler);

    return button;
}

function addCell(row, value) {
    const cell = document.createElement("td");

    cell.textContent = value ?? "";
    row.appendChild(cell);
}

// ==========================================
// Table Rendering
// ==========================================

function renderTable(containerId, headers, records, rowBuilder) {
    const container = document.getElementById(containerId);

    container.replaceChildren();

    if (records.length === 0) {
        container.appendChild(
            createElement("p", "No records found.")
        );
        return;
    }

    const table = document.createElement("table");
    const thead = document.createElement("thead");
    const headerRow = document.createElement("tr");

    headers.forEach(header => {
        headerRow.appendChild(createElement("th", header));
    });

    thead.appendChild(headerRow);
    table.appendChild(thead);

    const tbody = document.createElement("tbody");

    records.forEach(record => {
        const row = document.createElement("tr");

        rowBuilder(row, record);

        tbody.appendChild(row);
    });

    table.appendChild(tbody);
    container.appendChild(table);
}

// ==========================================
// Companies
// ==========================================

function renderCompanies() {
    const search = document.getElementById("companySearch")
        .value.toLowerCase();

    const filtered = companiesData.filter(company =>
        `${company.name} ${company.industry ?? ""} ${company.location ?? ""}`
            .toLowerCase()
            .includes(search)
    );

    renderTable(
        "companiesTable",
        ["ID", "Company", "Industry", "Location", "Actions"],
        filtered,
        (row, company) => {
            addCell(row, company.id);
            addCell(row, company.name);
            addCell(row, company.industry);
            addCell(row, company.location);

            const actions = document.createElement("td");

            actions.appendChild(
                createButton("View", () => viewCompany(company.id))
            );

            actions.appendChild(
                createButton("Edit", () => editCompany(company.id))
            );

            actions.appendChild(
                createButton(
                    "Delete",
                    () => deleteCompany(company.id),
                    "btn-danger"
                )
            );

            row.appendChild(actions);
        }
    );
}

// ==========================================
// Company Form
// ==========================================

function showCompanyForm() {
    document.getElementById("companyForm").reset();
    document.getElementById("companyId").value = "";

    document.getElementById("companyFormTitle").textContent =
        "Add Company";

    document.getElementById("companyForm").classList.remove("hidden");

    document.getElementById("companyName").focus();
}

function hideCompanyForm() {
    document.getElementById("companyForm").classList.add("hidden");
}

// ==========================================
// View Company
// ==========================================

async function viewCompany(id) {
    try {
        const company = await apiRequest(`companies/${id}`);

        showDetails("Company Details", [
            ["Company ID", company.id],
            ["Name", company.name],
            ["Industry", company.industry],
            ["Location", company.location]
        ]);

    } catch (error) {
        showMessage(`Unable to retrieve company: ${error.message}`, "error");
    }
}

// ==========================================
// Edit Company
// ==========================================

async function editCompany(id) {
    try {
        const company = await apiRequest(`companies/${id}`);

        document.getElementById("companyId").value = company.id;
        document.getElementById("companyName").value = company.name;
        document.getElementById("companyIndustry").value =
            company.industry ?? "";
        document.getElementById("companyLocation").value =
            company.location ?? "";

        document.getElementById("companyFormTitle").textContent =
            "Edit Company";

        document.getElementById("companyForm").classList.remove("hidden");

        document.getElementById("companyName").focus();

    } catch (error) {
        showMessage(`Unable to edit company: ${error.message}`, "error");
    }
}

// ==========================================
// Delete Company
// ==========================================

async function deleteCompany(id) {
    const company = companiesData.find(c => c.id === id);

    const confirmed = confirm(
        `Are you sure you want to delete ${company?.name ?? "this company"}?`
    );

    if (!confirmed) {
        return;
    }

    try {
        await apiRequest(`companies/${id}`, {
            method: "DELETE"
        });

        await loadDashboard();

        showMessage("Company deleted successfully.");

    } catch (error) {
        showMessage(`Delete failed: ${error.message}`, "error");
    }
}

// ==========================================
// Add / Update Company
// ==========================================

document.getElementById("companyForm").addEventListener(
    "submit",
    async function (event) {
        event.preventDefault();

        const id = document.getElementById("companyId").value;
        const isEditing = id !== "";

        const company = {
            id: isEditing ? Number(id) : 0,
            name: document.getElementById("companyName").value.trim(),
            industry: document.getElementById("companyIndustry").value.trim(),
            location: document.getElementById("companyLocation").value.trim()
        };

        const endpoint = isEditing
            ? `companies/${id}`
            : "companies";

        try {
            await apiRequest(endpoint, {
                method: isEditing ? "PUT" : "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(company)
            });

            hideCompanyForm();

            await loadDashboard();

            showMessage(
                isEditing
                    ? "Company updated successfully."
                    : "Company created successfully."
            );

        } catch (error) {
            showMessage(`Save failed: ${error.message}`, "error");
        }
    }
);

// ==========================================
// Job Applications
// ==========================================

function renderApplications() {
    const search = document.getElementById("applicationSearch")
        .value.toLowerCase();

    const filtered = applicationsData.filter(application =>
        `${application.jobTitle} ${getCompanyName(application.companyId)}`
            .toLowerCase()
            .includes(search)
    );

    renderTable(
        "applicationsTable",
        ["ID", "Job Title", "Company", "Date Applied", "Salary", "Actions"],
        filtered,
        (row, application) => {
            addCell(row, application.id);
            addCell(row, application.jobTitle);
            addCell(row, getCompanyName(application.companyId));
            addCell(row, formatDate(application.dateApplied));
            addCell(row, formatSalary(application.salary));

            const actions = document.createElement("td");


            actions.appendChild(
                createButton(
                    "View Details",
                    () => viewApplication(application.id)
                )
            );

            actions.appendChild(
                createButton(
                    "Calculate Score",
                    () => calculateApplicationScore(application.id)
                )
            );

            actions.appendChild(
                createButton(
                    "Follow-Up",
                    () => generateFollowUpRecommendation(application.id)
                )
            );

            actions.appendChild(
                createButton(
                    "Generate Report",
                    () => generateApplicationReport(application.id)
                )
            );

            row.appendChild(actions);

        }
    );
}

async function viewApplication(id) {
    try {
        const application = await apiRequest(`jobapplications/${id}`);

        showDetails("Job Application Details", [
            ["Application ID", application.id],
            ["Job Title", application.jobTitle],
            ["Company", getCompanyName(application.companyId)],
            ["Status ID", application.applicationStatusId],
            ["Date Applied", formatDate(application.dateApplied)],
            ["Location", application.location],
            ["Salary", formatSalary(application.salary)],
            ["Job URL", application.jobUrl],
            ["Notes", application.notes]
        ]);

    } catch (error) {
        alert(`Unable to retrieve job application: ${error.message}`);
    }
}


// ==========================================
// Azure Functions - Minor Project 6
// ==========================================

function getApplicationStatus(statusId) {
    const statuses = {
        1: "Saved",
        2: "Applied",
        3: "Under Review",
        4: "Interview",
        5: "Offer",
        6: "Rejected",
        7: "Withdrawn"
    };

    return statuses[statusId] ?? "Unknown";
}

function getDaysSinceApplied(dateApplied) {
    const appliedDate = new Date(dateApplied);
    const today = new Date();

    appliedDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    return Math.max(
        0,
        Math.floor(
            (today - appliedDate) / (1000 * 60 * 60 * 24)
        )
    );
}

// Function 1: Calculate Application Score
async function calculateApplicationScore(id) {
    try {
        const application = applicationsData.find(a => a.id === id);

        if (!application) {
            throw new Error("Job application not found.");
        }

        const status = getApplicationStatus(
            application.applicationStatusId
        );

        const days = getDaysSinceApplied(application.dateApplied);
        const salary = application.salary ?? 0;

        const params = new URLSearchParams({
            status: status,
            daysSinceApplied: days,
            salary: salary
        });

        const result = await apiRequest(
            `jobapplications/application-score?${params}`
        );

        showDetails("Azure Function - Application Score", [
            ["Job Title", application.jobTitle],
            ["Company", getCompanyName(application.companyId)],
            ["Application Status", result.status],
            ["Days Since Applied", result.daysSinceApplied],
            ["Salary", formatSalary(result.salary)],
            ["Status Score", result.statusScore],
            ["Days Score", result.daysScore],
            ["Salary Score", result.salaryScore],
            ["Total Application Score", `${result.totalScore}/100`]
        ]);

    } catch (error) {
        alert(`Unable to calculate application score: ${error.message}`);
    }
}

// Function 2: Generate Follow-Up Recommendation
async function generateFollowUpRecommendation(id) {
    try {
        const application = applicationsData.find(a => a.id === id);

        if (!application) {
            throw new Error("Job application not found.");
        }

        const status = getApplicationStatus(
            application.applicationStatusId
        );

        const days = getDaysSinceApplied(application.dateApplied);

        const params = new URLSearchParams({
            status: status,
            daysSinceApplied: days
        });

        const result = await apiRequest(
            `jobapplications/follow-up-recommendation?${params}`
        );

        showDetails("Azure Function - Follow-Up Recommendation", [
            ["Job Title", application.jobTitle],
            ["Company", getCompanyName(application.companyId)],
            ["Application Status", result.status],
            ["Days Since Applied", result.daysSinceApplied],
            ["Should Follow Up", result.shouldFollowUp ? "Yes" : "No"],
            ["Recommendation", result.recommendation]
        ]);

    } catch (error) {
        alert(`Unable to generate follow-up recommendation: ${error.message}`);
    }
}

// Function 3: Generate Application Report
async function generateApplicationReport(id) {
    try {
        const application = applicationsData.find(a => a.id === id);

        if (!application) {
            throw new Error("Job application not found.");
        }

        const result = await apiRequest(
            `jobapplications/${id}/upload-report`,
            {
                method: "POST"
            }
        );

        showDetails("Azure Blob Storage - Report Uploaded", [
            ["Job Title", application.jobTitle],
            ["Company", getCompanyName(application.companyId)],
            ["Application ID", result.applicationId],
            ["Status", result.message],
            ["Container", result.containerName],
            ["Report Filename", result.blobName]
        ]);

    } catch (error) {
        alert(`Unable to generate report: ${error.message}`);
    }
}



// ==========================================
// Interviews
// ==========================================

function renderInterviews() {
    const search = document.getElementById("interviewSearch")
        .value.toLowerCase();

    const filtered = interviewsData.filter(interview =>
        `${interview.interviewType} ${interview.outcome ?? ""}`
            .toLowerCase()
            .includes(search)
    );

    renderTable(
        "interviewsTable",
        ["ID", "Interview Type", "Date", "Outcome", "Actions"],
        filtered,
        (row, interview) => {
            addCell(row, interview.id);
            addCell(row, interview.interviewType);
            addCell(row, formatDate(interview.interviewDate));
            addCell(row, interview.outcome ?? "Pending");

            const actions = document.createElement("td");

            actions.appendChild(
                createButton(
                    "View Details",
                    () => viewInterview(interview.id)
                )
            );

            row.appendChild(actions);
        }
    );
}

async function viewInterview(id) {
    try {
        const interview = await apiRequest(`interviews/${id}`);

        const application = applicationsData.find(
            a => a.id === interview.jobApplicationId
        );

        showDetails("Interview Details", [
            ["Interview ID", interview.id],
            ["Interview Type", interview.interviewType],
            ["Date", new Date(interview.interviewDate).toLocaleString()],
            ["Job Application", application?.jobTitle ?? "Unknown"],
            ["Location", interview.location],
            ["Outcome", interview.outcome],
            ["Notes", interview.notes]
        ]);

    } catch (error) {
        alert(`Unable to retrieve interview: ${error.message}`);
    }
}

// ==========================================
// Recent Job Applications
// ==========================================

function renderRecentApplications() {
    const recent = [...applicationsData]
        .sort(
            (a, b) =>
                new Date(b.dateApplied) - new Date(a.dateApplied)
        )
        .slice(0, 5);

    renderTable(
        "recentApplications",
        ["Job Title", "Company", "Date Applied"],
        recent,
        (row, application) => {
            addCell(row, application.jobTitle);
            addCell(row, getCompanyName(application.companyId));
            addCell(row, formatDate(application.dateApplied));
        }
    );
}

// ==========================================
// Record Details Dialog
// ==========================================

function showDetails(title, fields) {
    document.getElementById("detailsTitle").textContent = title;

    const container = document.getElementById("detailsContent");

    container.replaceChildren();

    fields.forEach(([label, value]) => {
        const row = createElement("div", "", "detail-row");

        row.appendChild(
            createElement("span", label, "detail-label")
        );

        row.appendChild(
            createElement(
                "span",
                value ?? "Not specified",
                "detail-value"
            )
        );

        container.appendChild(row);
    });

    document.getElementById("detailsModal").classList.remove("hidden");
}

function hideDetails() {
    document.getElementById("detailsModal").classList.add("hidden");
}

// ==========================================
// Search Event Listeners
// ==========================================

document.getElementById("companySearch")
    .addEventListener("input", renderCompanies);

document.getElementById("applicationSearch")
    .addEventListener("input", renderApplications);

document.getElementById("interviewSearch")
    .addEventListener("input", renderInterviews);

// ==========================================
// Initialize Application
// ==========================================

showSection("dashboard");
loadDashboard();
