using ApprendreDotNet.DbCore;
using ApprendreDotNet.services.task;
using ApprendreDotNet.services.user;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TaskService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyAppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserService, UserService>();
var configuration = builder.Configuration;
var origins = configuration.GetSection("Origins").GetChildren().Select(x => x.Value).ToArray();
var methods = configuration.GetSection("Methods").GetChildren().Select(x => x.Value).ToArray();
var headers = configuration.GetSection("Headers").GetChildren().Select(x => x.Value).ToArray();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(origins!);
        policy.WithMethods(methods!);
        policy.WithHeaders(headers!);
        policy.AllowCredentials();
    });
});
var app = builder.Build();

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
