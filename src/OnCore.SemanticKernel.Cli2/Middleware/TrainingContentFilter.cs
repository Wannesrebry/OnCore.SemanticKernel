using Microsoft.Extensions.AI;

namespace OnCore.SemanticKernel.Cli2.Middleware;

public static class TrainingContentFilterMiddleware
{
    public static ChatClientBuilder UseTrainingContentFilter(this ChatClientBuilder builder)
        => builder.Use(inner => new TrainingContentFilterChatClient(inner));

    private class TrainingContentFilterChatClient(IChatClient chatClient): DelegatingChatClient(chatClient)
    {
        public override async Task<ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
            CancellationToken cancellationToken = new())
        {
            var promptEdit = new ChatMessage(ChatRole.System,
                """
                You are a personal trainer. Only provide training-related information.
                If asked about other topics, politely decline.
                """);
            
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