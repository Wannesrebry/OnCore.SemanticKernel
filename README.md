# Semantic Kernel Example Project

## What is Semantic Kernel?

Semantic Kernel is a powerful tool designed to enhance the capabilities of your applications by leveraging semantic understanding. It allows you to process and analyze text data, extract meaningful insights, and perform various natural language processing (NLP) tasks with ease.

## Why Use Semantic Kernel?

- **Enhanced Text Understanding**: Gain deeper insights from text data by understanding the context and semantics.
- **Improved NLP Tasks**: Perform tasks such as text classification, sentiment analysis, and entity recognition more effectively.
- **Scalability**: Easily integrate and scale semantic processing in your applications.
- **Flexibility**: Customize and extend the kernel to fit your specific needs.

## How to Use Semantic Kernel?

### Prerequisites

- Ensure you have .NET 8 installed on your system.
- Install the required NuGet package:
```sh
  dotnet add package SemanticKernel
  
### Configuration

- Set up the configuration in the `appsettings.json` file:
```json
{
  "AzureOpenAi": { 
    "DeploymentName": "[The name of your deployment]",
    "Endpoint": "[Your Azure endpoint]",
    "ApiKey": "[Your Azure OpenAI API key]",
    "ModelId": "[The name of the model]" 
  }
}
```

Configure Api-Key using Dotnet User Secrets:
```sh
dotnet user-secrets init
dotnet user-secrets set "AzureOpenAi:ApiKey" "your_api_key_here"
dotnet user-secrets set "Seq:ApiKey" "your_api_key_here"
```