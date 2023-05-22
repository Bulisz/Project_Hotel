using CloudinaryDotNet;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Repositories;
using Hotel.Backend.WebAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("No connectionString");
builder.Services.AddDbContext <HotelDbContext> (options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();


builder.Services.AddCorsRules();
builder.Services.AddAuth(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton(s =>
    new Cloudinary(new Account(
        builder.Configuration.GetValue<string>("CloudinaryConfig:Cloud"),
        builder.Configuration.GetValue<string>("CloudinaryConfig:ApiKey"),
        builder.Configuration.GetValue<string>("CloudinaryConfig:ApiSecret"))));

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

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.UseAuthAsync();

app.Run();

