using System.Text.Json.Serialization;
using System.Threading.Channels;
using Domain.Configuration;
using Domain.Dto.Email;
using Repository;
using Repository.Interface;
using Repository.Implementation;
using Service.Interface;
using Service.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Service.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeachingAssignmentService, TeachingAssignmentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.Configure<OpenLibraryApiSettings>(
    builder.Configuration.GetSection("OpenLibraryApiSettings"));

builder.Services.AddHttpClient<IOpenLibraryApiClient, OpenLibraryApiClient>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<OpenLibraryApiSettings>>().Value;
    client.BaseAddress = new Uri(settings.BaseUrl + "/");
});

builder.Services.AddScoped<IResourceEtlService, ResourceEtlService>();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddSingleton(Channel.CreateUnbounded<EmailMessage>());
builder.Services.AddSingleton<IEmailQueue, ChannelEmailQueue>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddHostedService<EmailBackgroundService>();

builder.Services.AddQuartz(options =>
{
    var jobKey = new JobKey("etl-sync", "integration");
    options.AddJob<EtlSyncQuartzJob>(o => o.WithIdentity(jobKey));

    options.AddTrigger(o =>
    {
        o.ForJob(jobKey)
            .WithIdentity("etl-sync-trigger")
            .WithCronSchedule("0 0/10 * * * ?")
            .WithDescription("Synchronizes recommended resources from Open Library every 10 minutes");
    });
});

builder.Services.AddQuartzHostedService();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();