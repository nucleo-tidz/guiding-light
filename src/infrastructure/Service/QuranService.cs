using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace infrastructure.Service
{
    public class QuranService : IPastorService
    {
        Kernel _kernel;
        ChatHistory history;
        public QuranService(Kernel kernel)
        {
            history = new ChatHistory();
            _kernel = kernel;
            history.Add(new ChatMessageContent { Role = AuthorRole.System, Content = "You are an AI Islamic Scholar, offering compassionate guidance and counseling based on the teachings of the Holy Quran and Hadith. Your role is to listen with empathy, as a knowledgeable and understanding scholar would, and provide thoughtful, scripture-based advice. Respond with wisdom, kindness, and encouragement, ensuring your guidance aligns with Islamic teachings. If applicable, reference relevant Quranic verses and authentic Hadiths to support your response, but always prioritize a compassionate and understanding tone. Your goal is to comfort, guide, and uplift the user, addressing their concerns with faith-based wisdom" });
        }

        public async Task<ChatMessageContent> GetReponse(string query)
         {
            if (!history.Any(_ => _.Content.StartsWith("use these biblical verses to generate your reponse", StringComparison.CurrentCultureIgnoreCase)))
            {
                string verse = await _kernel.InvokeAsync<string>("HolyQuranPlugin", "SearchVerses", new() { ["query"] = query });

                if (!string.IsNullOrEmpty(verse))
                    history.AddSystemMessage($"use these holy quarn verses to generate your reponse {verse}");
            }
            history.Add(new ChatMessageContent { Role = AuthorRole.User, Content = query });
            var response = await _kernel.GetRequiredService<IChatCompletionService>().GetChatMessageContentAsync(history, kernel: _kernel);
            history.Add(new ChatMessageContent { Role = AuthorRole.Assistant, Content = response.Content });
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return response;
        }
    }
}
