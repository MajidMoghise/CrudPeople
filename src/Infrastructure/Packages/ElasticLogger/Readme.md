Add this config json in appsettings.json's project

 "Serilog": {
    "MinimumLevel": "Information",
    "Override": {
      "Microsoft.AspNetCore": "Information"
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "Elasticsearch",
        "Args": {
          "AutoRegisterTemplate": true,
          "Formatter": "Serilog.Formatting.Json.JsonFormatter, Serilog",
          "IndexFormat": "serilog-{0:yyyy.MM.dd}",
          "NodeUris": "http://192.168.240.128:9200",
          "Password": "Adm1n123",
          "RestrictedToMinimumLevel": "Information",
          "Username": "elastic"
        }
      }
    ],
    "Enrich": [
      "FromLogContext",
      "WithMachineName",
      "WithProcessId",
      "WithThreadId"
    ]
  },

  using LogCall attribute in over class or method

  add AddScoped LoggMiddelware in registeration service 
  add LoggMiddelware in UseMiddleware in highest priority