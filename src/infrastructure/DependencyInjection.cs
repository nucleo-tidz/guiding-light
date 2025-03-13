using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel.Memory;

using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.Redis;
using Microsoft.SemanticKernel;
using infrastructure.Plugin;
using System;
using infrastructure.Service;

namespace infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
        public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPastorService, QuranService>();
            return  services.AddTransient<Kernel>(serviceProvider =>
             {
                 serviceProvider.GetRequiredService<IKernelMemory>();
                 IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
                 kernelBuilder.Services.AddAzureOpenAIChatCompletion("gpt-4o",
                    "https://ahmar-m7ohej9z-eastus2.cognitiveservices.azure.com/",
                    configuration["APIKey"],
                    "gpt-4o",
                    "gpt-4o");
                 kernelBuilder.Plugins.AddFromObject(new BibleSearchPlugin(serviceProvider.GetRequiredService<IKernelMemory>()), "BibleSearchPlugin");
                 kernelBuilder.Plugins.AddFromObject(new HolyQuranPlugin(serviceProvider.GetRequiredService<IKernelMemory>()), "HolyQuranPlugin");
                 return kernelBuilder.Build();
             });
         }
        public static IServiceCollection AddSemanticKernelMemory(this IServiceCollection services, IConfiguration configuration)
        {

            return services.AddTransient(serviceProvider =>
            {
                RedisConfig redisConfig = new RedisConfig
                {
                    ConnectionString = configuration.GetConnectionString("Redis"),
                }; 
                redisConfig.Tags.Add("collection", ',');
                redisConfig.Tags.Add("__part_n", ',');
                IKernelMemoryBuilder memoryBuilder = new KernelMemoryBuilder()
                .WithAzureBlobsDocumentStorage(new AzureBlobsConfig
                {
                    ConnectionString = configuration.GetConnectionString("AzureBlobStorage"),
                    Container = "knowledgebase",
                    Auth = AzureBlobsConfig.AuthTypes.ConnectionString

                }).WithAzureOpenAITextEmbeddingGeneration(new AzureOpenAIConfig
                {
                    APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
                    Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                    APIKey = configuration["APIKey"],
                    Endpoint = "https://ahmar-m7ohej9z-eastus2.cognitiveservices.azure.com/",
                    Deployment = "vectoriser"

                }).WithAzureOpenAITextGeneration(new AzureOpenAIConfig
                {
                    APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
                    Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                    APIKey = configuration["APIKey"],
                    Endpoint = "https://ahmar-m7ohej9z-eastus2.cognitiveservices.azure.com/",
                    Deployment = "gpt-4o"
                })
                .WithRedisMemoryDb(redisConfig);
           
                return memoryBuilder.Build();
            });
        }

    }
}
