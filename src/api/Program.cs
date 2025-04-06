using infrastructure;

using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSemanticKernelMemory(builder.Configuration)
    .AddSemanticKernel(builder.Configuration).AddPastor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
}).AddHealthCheck();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapHealthChecks("/health/ready").AllowAnonymous();
app.MapHealthChecks("/health/live").AllowAnonymous();
app.MapControllers();

app.Run();
public static class DependecyInjection
{
    internal static IServiceCollection AddHealthCheck(this IServiceCollection self)
    {

        self.AddHealthChecks()
             .AddCheck("Live Probe", _ => HealthCheckResult.Healthy());

        return self;
    }
}
