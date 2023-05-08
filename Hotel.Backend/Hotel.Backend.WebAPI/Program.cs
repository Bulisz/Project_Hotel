using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Repositories;
using Hotel.Backend.WebAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("No connectionString");
builder.Services.AddDbContext <HotelDbContext> (options => options.UseSqlServer(connectionString));

//builder.Services.AddScoped<IRoomService, RoomService>();
//builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddCorsRules();
builder.Services.AddAuth(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.UseAuthAsync();

app.Run();

