// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Asp.Versioning;
using LayeredArchitectureTask1CartService.Business;
using LayeredArchitectureTask1CartService.MessageQueue;
using LayeredArchitectureTask1CartService.Middlewares;
using LayeredArchitectureTask1CartService.Repository;
using LayeredArchitectureTask1CartService.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection(RabbitMQOptions.SectionName));
builder.Services.AddMessageQueueConsumer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddBusinessServices();
builder.Services.AddRepositoryServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cart API", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Cart API", Version = "v2" });

    options.OperationFilter<SwaggerDefaultValues>();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token: Bearer {your token}",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false, // for Keycloak
            ValidateIssuer = false, // allows tokens obtained via localhost when service resolves Keycloak by container name
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context.Principal?.Identity as ClaimsIdentity;

                var realmAccess = context.Principal?.FindFirst("realm_access")?.Value;

                if (realmAccess != null && identity != null)
                {
                    var roles = JsonDocument.Parse(realmAccess)
                        .RootElement
                        .GetProperty("roles");

                    foreach (var role in roles.EnumerateArray())
                    {
                        var roleValue = role.GetString();
                        if (roleValue != null)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                        }
                    }
                }

                return Task.CompletedTask;
            },
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart API V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Cart API V2");
        options.RoutePrefix = "swagger";
        options.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<TokenLoggingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
