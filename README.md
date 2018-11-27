# NetStash.Extensions.Logging

Provedor para [Microsoft.Extensions.Logging](https://github.com/aspnet/Extensions) para envio de logs via TCP para [Logstash](https://www.elastic.co/products/logstash)  e outros com suporte para .NET Standart 2.0+ 

## Uso 

## ASP.NET Core 2.x
Na classe `Progam.cs`, importar o método `LoggingBuilder.AddNetStash()` da biblioteca `NetStash.Extensions.Logging` e adicionar o código abaixo para configurar o log.
```csharp

var webHost = WebHost
  .CreateDefaultBuilder(args)
  .UseStartup<Startup>()
  .ConfigureLogging((context, builder) =>
  {
    builder.Services.Configure<NetStashOptions>(context.Configuration.GetSection("NetStash"));

    // possibilidade de adicionais informações adicionais
    builder.Services.PostConfigure<NetStashOptions>(otpions => 
    {
      options.ExtraValues.Add("environment", context.Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT"));
      options.ExtraValues.Add("version",  Assembly.GetEntryAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
    });

    builder.AddConfiguration(context.Configuration.GetSection("Logging"))
      .AddConsole()
      .AddDebug()
      .AddNetStash();
  })
  .Build();
```

Para realizar a configuração basta incluir a seção "NetStash" no arquivo. `appsettings.json` como no exemplo abaixo
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Warning",
      "Microsoft": "Warning"
    },
    "Console": {
      "LogLevel": {
        "Default": "Warning"
      }
    },
    "NetStash": {
      "LogLevel": {
        "Default": "Debug",
        "System": "Warning",
        "Microsoft": "Warning"
      }
    }
  },
  "NetStash": {
      "AppName": "Aplicação Teste",
      "Host": "tcp.localhost.com.br",
      "Port": "5030",
      "ExtraValues": {
        "Environment": "Development",
        "Version": "1.0.0"
      }
  }
}

```