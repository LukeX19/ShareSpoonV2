using ShareSpoon.Api.Extensions;
using ShareSpoon.Api.Middlewares;
using ShareSpoon.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.RegisterAuthentication();
builder.RegisterBlobStorage();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddMediatR();
builder.Services.AddAutoMapper();
builder.Services.AddRepositories();
builder.Services.AddDbContext(builder);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ShareSpoonReactClient",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom middleware to log request duration
app.UseMiddleware<TimingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("ShareSpoonReactClient");

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
