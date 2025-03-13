using client;
using infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSemanticKernelMemory(builder.Configuration)
    .AddSemanticKernel(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
