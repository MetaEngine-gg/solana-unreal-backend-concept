using Core.Data_Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DataController>();
builder.Services.AddSingleton<UserDataController>();

builder.Services.AddSingleton<SolnetKeystoreDataController>();
builder.Services.AddSingleton<SolnetRpcDataController>();
builder.Services.AddSingleton<SolnetProgramsDataController>();
builder.Services.AddSingleton<SolnetSerumDataController>();

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
