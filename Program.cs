using gcai.Data;
using gcai.Models;
using gcai.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.AspNetCore.Identity.UI.Services;
using gcio.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Builder;
using gcai.Areas.Identity.Services;
using gcia.Areas.Identity.Services;
using Azure;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
//required for Apache reverse proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor;
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.RequireHeaderSymmetry = false;
    options.ForwardLimit = 2;
    options.KnownProxies.Add(IPAddress.Parse("127.0.0.1")); //reverse proxy, Kestrel defaults to port 5000 which is also set in apsettings.json
    options.KnownProxies.Add(IPAddress.Parse("162.205.232.101")); //server IP public
    
});
Environment.SetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED", "true");
//configure listen protocals and assert SSL/TLS requirement
builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.ConfigureHttpsDefaults(listenOptions =>
    {
        listenOptions.SslProtocols = SslProtocols.Tls13;
        listenOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;//requires certificate from client
    });
});
//configure connection string from environment variables thus hidding it from production
var environ = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var connectionString = "";
var GC_Email_Pass = "";
if (environ == "Production")
{
    //pulls connection string from environment variables
    connectionString = Environment.GetEnvironmentVariable("MariaDbConnectionStringLocal");
    GC_Email_Pass = Environment.GetEnvironmentVariable(GC_Email_Pass);
}
else
{
    //pulls connection string from development local version of secrets.json
    connectionString = builder.Configuration.GetConnectionString("MariaDbConnectionStringRemote");
    GC_Email_Pass = builder.Configuration.GetConnectionString("GC_Email_Pass");


}
Environment.SetEnvironmentVariable("DbConnectionString", connectionString);//this is used in services to access the string
Environment.SetEnvironmentVariable("GC_Email_Pass", GC_Email_Pass);

builder.Services.AddDbContext<gcai.Data.ApplicationDbContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 6, 11)), options => options.EnableRetryOnFailure())); 
//UseMySql can be configured in the following ways.  Ignore option must be enabled to perform code first migrations with the MySql database 
//options => options.EnableRetryOnFailure() 
//options => options.SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Ignore)

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});



builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = false;
});
builder.Services.AddAuthorization();
//addition of encryption methods for deployment on linux
builder.Services.AddDataProtection().UseCryptographicAlgorithms(
    new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });
builder.Services.AddMvc();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
 options.MimeTypes = ResponseCompressionDefaults
 .MimeTypes.Concat(new[] { "application/octet-stream:" })
);


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Error");
    app.UseForwardedHeaders();
    //app.UseHttpsRedirection();//appache handles.  Do not enable!
    //app.UseHsts();//not required SSL/TLS.  Untested configuration!
}
else
{
    //app.UseDeveloperExceptionPage();
}

var configuration = builder.Configuration;
var value = configuration.GetValue<string>("value");
app.UseResponseCompression();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); //All controllers including the API are mapped here.
});

app.UseResponseCompression();
//app.UseHsts();
app.UseStaticFiles();
//required for same site cookies
app.UseCookiePolicy();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "portal",
    pattern: "{controller=PortalController}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Terms}/");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Privacy}/");
app.MapControllerRoute(
    name: "contactform",
    pattern: "/Contact",
    defaults: new { controller = "ContactController", action = "post" });
app.MapControllerRoute(
    name: "contactform-get",
    pattern: "/Contact",
    defaults: new { controller = "ContactController", action = "get" });
app.MapControllerRoute(
	name: "portal",
	pattern: "{controller=PortalController}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "portal",
    pattern: "{controller=PortalController}/{action=RemoveFavorite}/{id?}");


app.MapRazorPages();
app.MapHub<PostHub>("/posthub");
app.Run();