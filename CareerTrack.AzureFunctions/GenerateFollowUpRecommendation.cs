using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CareerTrack.AzureFunctions;

public class GenerateFollowUpRecommendation
{
    private readonly ILogger<GenerateFollowUpRecommendation> _logger;

    public GenerateFollowUpRecommendation(ILogger<GenerateFollowUpRecommendation> logger)
    {
        _logger = logger;
    }

    [Function("GenerateFollowUpRecommendation")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        _logger.LogInformation("Generating follow-up recommendation.");

        string status = req.Query["status"].ToString();

        string daysInput = req.Query["daysSinceApplied"].ToString();

        if (string.IsNullOrWhiteSpace(status))
        {
            return new BadRequestObjectResult(
                "Application status is required.");
        }

        if (!int.TryParse(daysInput, out int daysSinceApplied)
            || daysSinceApplied < 0)
        {
            return new BadRequestObjectResult(
                "daysSinceApplied must be a non-negative integer.");
        }

        string normalizedStatus = status.Trim().ToLowerInvariant();

        bool shouldFollowUp = normalizedStatus switch
        {
            "applied" => daysSinceApplied >= 7,

            "under review" => daysSinceApplied >= 14,

            "interview" => daysSinceApplied >= 5,

            "offer" => false,

            "rejected" => false,

            _ => false
        };

        string recommendation;

        if (shouldFollowUp)
        {
            recommendation = "Follow up with the recruiter.";
        }
        else if (normalizedStatus == "offer" || normalizedStatus == "rejected")
        {
            recommendation = "No follow-up needed.";
        }
        else
        {
            recommendation = "Wait before following up.";
        }

        return new OkObjectResult(new
        {
            Status = status.Trim(),
            DaysSinceApplied = daysSinceApplied,
            ShouldFollowUp = shouldFollowUp,
            Recommendation = recommendation
        });
    }
}