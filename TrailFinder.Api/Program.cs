using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddScoped<ITrailService, TrailService>();

// CORS for the Web client
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb", policyBuilder =>
        policyBuilder.WithOrigins("http://localhost:5173") // Your Web client URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWeb");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
