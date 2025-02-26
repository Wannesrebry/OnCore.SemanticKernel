using Microsoft.Extensions.AI;

namespace OnCore.SemanticKernel.Cli2.Middleware;

public static class LanguageMiddleware
{
    public static ChatClientBuilder UseLanguage(this ChatClientBuilder builder, string language)
        => builder.Use(inner => new UseLanguageChatClient(inner, language));

    private class UseLanguageChatClient(IChatClient chatClient, string language): DelegatingChatClient(chatClient)
    {
        public override async Task<ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var promptEdit = new ChatMessage(ChatRole.User, $"Always reply in the language {language}");
            chatMessages.Add(promptEdit);
            
            try
            {
                return await base.CompleteAsync(chatMessages, options, cancellationToken);
            }
            finally
            {
                chatMessages.Remove(promptEdit);
            }
        }
    }
}
