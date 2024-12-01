using Microsoft.AspNetCore.Authorization;
using MiniApp1.API.Requirements;
using SharedLibrary.Configuration;
using SharedLibrary.Extensions;
using  MiniApp1.API.Requirements;

var builder = WebApplication.CreateBuilder(args);

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
builder.Services.AddCustomTokenAuth(tokenOptions!);
builder.Services.AddSingleton<IAuthorizationHandler, BirthDateRequirement.BirthDateRequirementHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BishkekPolicy", policy => { policy.RequireClaim("city", "Bishkek"); });
    options.AddPolicy("AgePolicy", policy => { policy.Requirements.Add(new BirthDateRequirement(18)); });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();