using HelpCenter;
using HelpCenter.Core.Configuration;
using HelpCenter.Core.Middleware;
using HelpCenter.Core.Services.CompanyService;
using HelpCenter.Core.Services.CompanyUserService;
using HelpCenter.Core.Services.FeedbackService;
using HelpCenter.Core.Services.HelpCenterService;
using HelpCenter.Core.Services.MailService;
using HelpCenter.Core.Services.ProductService;
using HelpCenter.Core.Utilities.Localization;
using HelpCenter.Models;
using HelpCenter.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// or using the index property (which always returns a string)
var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<APIDbContext>(options =>
options.UseSqlServer(connectionString), ServiceLifetime.Transient);

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<APIDbContext>().AddDefaultTokenProviders();


builder.Services.AddScoped<ExceptionMiddleware>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IHelpCenterService, HelpCenterService>();
builder.Services.AddScoped<ICompanyUserService, CompanyUserService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
});

//Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
#pragma warning disable CS8604 // Possible null reference argument.
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        ClockSkew = TimeSpan.Zero
    };
#pragma warning restore CS8604 // Possible null reference argument.
});

//Mail Settings
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IMailService, MailService>();

//Localization
builder.Services.AddLocalization();
builder.Services.AddSingleton<LocalizerMiddleware>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// Api Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});
// ApiExplorer to discover versions
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
// Add Rate Limiting
builder.Services.AddRateLimiting(builder.Configuration);

var app = builder.Build();

// Use Rate Limiting
app.UseRateLimiting();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

var options = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
};
app.UseGlobalExceptionHandler();

app.UseRequestLocalization(options);
app.UseStaticFiles();
app.UseMiddleware<LocalizerMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<APIDbContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
}
app.MapControllers();

app.Run();
