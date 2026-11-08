using Azure.Storage.Blobs;
using CareerTrack.Data.Data;
using Microsoft.EntityFrameworkCore;
using CareerTrack.Business.Services;
using CareerTrack.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register SQLite database context
builder.Services.AddDbContext<CareerTrackDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register data repositories
builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<JobApplicationRepository>();
builder.Services.AddScoped<InterviewRepository>();
builder.Services.AddScoped<ApplicationStatusRepository>();


// Register Azure Blob Storage client
builder.Services.AddSingleton(sp =>
{
    var configuration =
        sp.GetRequiredService<IConfiguration>();

    string connectionString =
        configuration["AzureStorage:ConnectionString"]
        ?? throw new InvalidOperationException(
            "Azure Storage connection string is missing.");

    return new BlobServiceClient(connectionString);
});

// Register Blob Report Service
builder.Services.AddScoped<BlobReportService>(sp =>
{
    var blobClient =
        sp.GetRequiredService<BlobServiceClient>();

    var applicationRepository =
        sp.GetRequiredService<JobApplicationRepository>();

    var companyRepository =
        sp.GetRequiredService<CompanyRepository>();

    var configuration =
        sp.GetRequiredService<IConfiguration>();

    string containerName =
        configuration["AzureStorage:ContainerName"]
        ?? "careertrack-reports";

    return new BlobReportService(
        blobClient,
        applicationRepository,
        companyRepository,
        containerName);
});


// Register JobApplicationService with an HTTP client for Azure Functions
builder.Services.AddHttpClient<JobApplicationService>(client =>
{
    client.BaseAddress = new Uri(
        "https://careertrack-mp6-flex-ccf7guddabgwcca6.westus2-01.azurewebsites.net/");

    client.Timeout = TimeSpan.FromSeconds(30);
});

// Register remaining business services
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<InterviewService>();

// Add API controllers
builder.Services.AddControllers();

// Add OpenAPI support
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
