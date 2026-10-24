using Microsoft.EntityFrameworkCore;
using RetailPayments.Api.Data;
using RetailPayments.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISourceSystemAdapter, StoreUptownAdapter>();
builder.Services.AddScoped<ISourceSystemAdapter, StoreDowntownAdapter>();
builder.Services.AddScoped<ISourceSystemAdapter, OnlineAdapter>();
builder.Services.AddScoped<TransactionImportService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();