using CSharpComposer.Generator;
using CSharpComposer.Generator.Builders;
using CSharpComposer.Generator.Generators;
using CSharpComposer.Generator.Registries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();

builder.Services
    .AddSingleton<CSharpRegistry>()
    .AddSingleton<DocumentRegistry>()
    .AddSingleton<EnumRegistry>()
    .AddTransient<ArgumentsBuilder>()
    .AddTransient<ImplementationBuilder>()
    .AddTransient<InterfaceBuilder>()
    .AddTransient<MethodBuilder>()
    .AddTransient<ParametersBuilder>()
    .AddTransient<BuilderGenerator>()
    .AddTransient<CSharpFactoryGenerator>()
    .AddTransient<EnumGenerator>()
    .AddHostedService<GeneratorService>();

await builder.Build().RunAsync();