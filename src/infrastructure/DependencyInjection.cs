﻿using infrastructure.Repository;
using infrastructure.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

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
            services.AddTransient<IChatHistoryRepository, ChatHistoryRepository>();
            services.AddTransient<IPastorService, PastorService>();
            services.AddTransient<IBibleService, BibleService>();
            return  services.AddTransient<Kernel>(serviceProvider =>
             {
                 serviceProvider.GetRequiredService<IKernelMemory>();
                 IKernelBuilder kernelBuilder = Kernel.CreateBuilder();

#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                     kernelBuilder.Services.AddGoogleAIGeminiChatCompletion("gemini-2.0-flash", "AIzaSyB__RPh0X68ufHHPE9OLHnlSq4UDnz1z4c", serviceId: "gpt-4-turbo");
#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                 //kernelBuilder.Services.AddAzureOpenAIChatCompletion("gpt-4o",
                 //   "https://ahmar-m7ohej9z-eastus2.cognitiveservices.azure.com/",
                 //   configuration["APIKey"],
                 //   "gpt-4o",
                 //   "gpt-4o");
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
