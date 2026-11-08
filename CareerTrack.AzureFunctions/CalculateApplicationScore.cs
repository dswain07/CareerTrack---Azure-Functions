using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CareerTrack.AzureFunctions;

public class CalculateApplicationScore
{
    private readonly ILogger<CalculateApplicationScore> _logger;

    public CalculateApplicationScore(ILogger<CalculateApplicationScore> logger)
    {
        _logger = logger;
    }

    [Function("CalculateApplicationScore")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        _logger.LogInformation("Calculating application score.");

        string status = req.Query["status"].ToString();

        string daysInput = req.Query["daysSinceApplied"].ToString();

        string salaryInput = req.Query["salary"].ToString();

        if (string.IsNullOrWhiteSpace(status))
        {
            return new BadRequestObjectResult("Application status is required.");
        }

        if (!int.TryParse(daysInput, out int daysSinceApplied)
            || daysSinceApplied < 0)
        {
            return new BadRequestObjectResult(
                "daysSinceApplied must be a non-negative integer.");
        }

        if (!decimal.TryParse(
                salaryInput,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal salary)
            || salary < 0)
        {
            return new BadRequestObjectResult(
                "salary must be a non-negative number.");
        }

        int statusScore = status.Trim().ToLowerInvariant() switch
        {
            "applied" => 15,
            "under review" => 25,
            "interview" => 40,
            _ => 5
        };

        int daysScore = daysSinceApplied switch
        {
            >= 21 => 40,
            >= 14 => 25,
            >= 7 => 15,
            _ => 5
        };

        int salaryScore = salary switch
        {
            >= 100000m => 20,
            >= 60000m => 10,
            _ => 5
        };

        int totalScore = Math.Min(
            100,
            statusScore + daysScore + salaryScore);

        return new OkObjectResult(new
        {
            Status = status,
            DaysSinceApplied = daysSinceApplied,
            Salary = salary,
            StatusScore = statusScore,
            DaysScore = daysScore,
            SalaryScore = salaryScore,
            TotalScore = totalScore
        });
    }
}