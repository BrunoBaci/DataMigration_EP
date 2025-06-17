using Microsoft.EntityFrameworkCore;
using WorkOrderService.Interfaces;
using WorkOrderService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWorkOrderRepository, WorkOrderService.Services.WorkOrderRepository>();
builder.Services.AddSingleton<ITextCheckRepository, TextCheckRepository>();
builder.Services.AddSingleton<IParser, Parser>();
builder.Services.AddHttpClient<IReportClient, ReportClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:52944"); 
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
}); 
builder.Services.AddDbContext<WorkOrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();