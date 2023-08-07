using FluentValidation;
using FluentValidation.AspNetCore;
using Tasks.Api.DependencyInjection;
using Tasks.Api.Mappers;
using Tasks.Api.Validators;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(GetTaskDtoProfile));
builder.Services.AddValidatorsFromAssembly(typeof(CreateTaskDtoValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddTaskDependencies();

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