using Customer.Api.Features.Customer.Data;
using Customer.Api.Infrastructure.Data;
using Customer.Api.Infrastructure.Documentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(XmlDocFilePath.XmlCommentsFilePath());
});

builder.Services.AddSingleton(c => DbConnectionProviderFactory.GetDbConnectionProvider());
builder.Services.AddSingleton<ICustomerQueries, CustomerQueries>();
builder.Services.AddSingleton<ICustomerCommands, CustomerCommands>();

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

