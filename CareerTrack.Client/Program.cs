using CareerTrack.Client.Models;
using System.Net.Http.Json;

using HttpClient client = new HttpClient();

client.BaseAddress = new Uri("http://localhost:5043/");

try
{
    Console.WriteLine("CareerTrack API Client");
    Console.WriteLine("----------------------");

    var companies = await client.GetFromJsonAsync<List<CompanyModel>>(
        "api/companies");

    if (companies != null)
    {
        foreach (var company in companies)
        {
            Console.WriteLine(
                $"{company.Id}: {company.Name} - {company.Location}");
        }
    }

    Console.WriteLine("\n--- Job Applications ---");

    var applications =
        await client.GetFromJsonAsync<List<JobApplicationModel>>(
            "api/jobapplications");

    if (applications != null)
    {
        foreach (var application in applications)
        {
            Console.WriteLine(
                $"{application.Id}: {application.JobTitle} - Salary: {application.Salary}");
        }
    }

    Console.WriteLine("\n--- Interviews ---");

    var interviews =
        await client.GetFromJsonAsync<List<InterviewModel>>(
            "api/interviews");

    if (interviews != null)
    {
        foreach (var interview in interviews)
        {
            Console.WriteLine(
                $"{interview.Id}: {interview.InterviewType} - {interview.InterviewDate}");
        }
    }

    Console.WriteLine("\n--- Get By ID Tests ---");

    // Company by ID
    var companyById = await client.GetFromJsonAsync<CompanyModel>(
        "api/companies/1");

    Console.WriteLine($"Company ID 1: {companyById?.Name}");

    // Job Application by ID
    var applicationById =
        await client.GetFromJsonAsync<JobApplicationModel>(
            "api/jobapplications/1");

    Console.WriteLine($"Application ID 1: {applicationById?.JobTitle}");

    // Interview by ID
    var interviewById =
        await client.GetFromJsonAsync<InterviewModel>(
            "api/interviews/1");

    Console.WriteLine($"Interview ID 1: {interviewById?.InterviewType}");

}
catch (HttpRequestException ex)
{
    Console.WriteLine($"API request failed: {ex.Message}");
}
