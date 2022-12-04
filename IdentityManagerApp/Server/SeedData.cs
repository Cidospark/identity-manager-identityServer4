using System;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            // create services instance
            var services = new ServiceCollection();
            services.AddLogging();
            // create connection to db
            services.AddDbContext<AspNetIdentityDbContext>(options => options.UseSqlServer(connectionString));

            // registering IdentityUser and IdentityRole
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AspNetIdentityDbContext>()
                    .AddDefaultTokenProviders();

            // registering operational dbContext 
            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db => db.UseSqlServer(
                        connectionString,
                        asm => asm.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                    );
                }
            );

            // registering configurational dbContext 
            services.AddConfigurationDbContext(
                opt =>
                {
                    opt.ConfigureDbContext = db => db.UseSqlServer(
                        connectionString,
                        asm => asm.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                    );
                }
            );

            // get the context scope
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();

            EnsureSeedData(context);

            var ctx = scope.ServiceProvider.GetService<AspNetIdentityDbContext>();
            ctx.Database.Migrate();
            EnsureUsers(scope);
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var john = userMgr.FindByNameAsync("doe").Result;
            if(john == null)
            {
                john = new IdentityUser
                {
                    UserName = "john",
                    Email = "john.doe@email.com",
                    EmailConfirmed = true
                };

                var result = userMgr.CreateAsync(john, "P@ssw0rd1").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                // add claims to user
                result = userMgr.AddClaimsAsync(
                    john,
                    new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, "John Doe"),
                        new Claim(JwtClaimTypes.GivenName, "John"),
                        new Claim(JwtClaimTypes.FamilyName, "Doe"),
                        new Claim(JwtClaimTypes.WebSite, "http://johndoe.com"),
                        new Claim("location", "somewhere")
                    }
                ).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach(var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach(var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}

