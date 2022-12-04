using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server;
using Server.Data;

//=============================================================================//
                    //---- Handle seeding -----//

var seed = args.Contains("/seed");

if (seed)
{
    args = args.Except(new[] { "/seed" }).ToArray();
}

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConn = builder.Configuration.GetConnectionString("DefaultConn");

if (seed)
{
    SeedData.EnsureSeedData(defaultConn);
}


//=================================================================================//


builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
{
    options.UseSqlServer(defaultConn, b => b.MigrationsAssembly(assembly));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AspNetIdentityDbContext>();


builder.Services.AddIdentityServer().AddAspNetIdentity<IdentityUser>()
 .AddConfigurationStore(options =>
{
    options.ConfigureDbContext = b => b.UseSqlServer(defaultConn, opt => opt.MigrationsAssembly(assembly));
}).AddOperationalStore(options =>
{
    options.ConfigureDbContext = b => b.UseSqlServer(defaultConn, opt => opt.MigrationsAssembly(assembly));
}).AddDeveloperSigningCredential();



var app = builder.Build();
app.UseIdentityServer();

app.Run();

