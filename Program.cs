using agency_portal_api.Configurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using agency_portal_api.Data;
using Mailjet.Client;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.ConfigureDatabase(connectionString);

builder.Services.AddScopedServices();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigurePasswordOptions();

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.ConfigureAuthorization();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureSwagger();

builder.Services.ConfigureAppSetting(builder.Configuration);

builder.Services.AddApiVersioningExtension();

builder.Services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
{
    client.DefaultRequestVersion = new Version("3.1");

    client.SetDefaultSettings();

    client.UseBasicAuthentication("35f9fd18e3a2c1679aabae8b8c69a026", "f2de3a17cdc2a28f276621631359f94b");
});

builder.Services.AddApiVersionedExplorerExtension();

builder.Services.AddScopedServices();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseHsts();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.MapControllers();

app.Run();
