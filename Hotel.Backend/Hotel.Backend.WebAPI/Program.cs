using CloudinaryDotNet;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO.OptionDTOs;
using Hotel.Backend.WebAPI.Repositories;
using Hotel.Backend.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
	var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services
    .AddOptions<JwtTokensOptions>()
    .BindConfiguration(nameof(JwtTokensOptions))
    .ValidateDataAnnotations();

    string connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("No connectionString");
	builder.Services.AddDbContext<HotelDbContext>(options => options.UseSqlServer(connectionString));

	builder.Services.AddScoped<IRoomService, RoomService>();
	builder.Services.AddScoped<IRoomRepository, RoomRepository>();
	builder.Services.AddScoped<IUserService, UserService>();
	builder.Services.AddScoped<IUserRepository, UserRepository>();
	builder.Services.AddScoped<IJwtService, JwtService>();
	builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
	builder.Services.AddScoped<IReservationService, ReservationService>();
	builder.Services.AddScoped<IPostService, PostService>();
	builder.Services.AddScoped<IPostRepository, PostRepository>();
	builder.Services.AddScoped<IEventRepository, EventRepository>();
	builder.Services.AddScoped<IEventService, EventService>();
	builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
	builder.Services.AddScoped<IEquipmentService, EquipmentService>();
	builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
	builder.Services.AddScoped<IEmailService, EmailService>();
	builder.Services.AddScoped<ICalendarService, CalendarService>();
    builder.Services.AddScoped<IStatisticsService, StatisticsService>();

    builder.Services.AddHostedService<UserCleanupService>();


	builder.Services.AddCorsRules();
    builder.Services.AddMemoryCache();
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

	//app.UseExceptionHandler();

	app.UseHttpsRedirection();

	app.UseDefaultFiles();
	app.UseStaticFiles();

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();

	await app.UseAuthAsync();

	app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

