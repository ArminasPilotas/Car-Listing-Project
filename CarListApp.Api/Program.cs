
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarListApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });

            var dbPath = Path.Join(Directory.GetCurrentDirectory(), "carlist.db");
            var conn = new SqliteConnection("Data Source=C:\\carlistdb\\carlist.db");
            builder.Services.AddDbContext<CarListDbContext>(options => options.UseSqlite(conn));

            builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CarListDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.MapGet("/cars", async (CarListDbContext db) => await db.Cars.ToListAsync());

            app.MapGet("/cars/{id}", async (int id, CarListDbContext db) =>
                await db.Cars.FindAsync(id) is Car car ? Results.Ok(car) : Results.NotFound()
            );

            app.MapPut("/cars/{id}", async (int id, Car car, CarListDbContext db) =>
            {
                var record = await db.Cars.FindAsync(id);
                if (record is null) return Results.NotFound();

                record.Make = car.Make;
                record.Model = car.Model;
                record.Vin = car.Vin;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapDelete("/cars/{id}", async (int id, CarListDbContext db) =>
            {
                var record = await db.Cars.FindAsync(id);
                if (record is null) return Results.NotFound();

                db.Remove(record);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapPost("/cars", async (Car car, CarListDbContext db) =>
            {
                await db.AddAsync(car);
                await db.SaveChangesAsync();

                return Results.Created($"/cars/{car.Id}", car);
            });

            app.MapPost("/login", async (LoginDto loginDto, CarListDbContext db, UserManager<IdentityUser> _userManager) =>
            {
                var user = await _userManager.FindByNameAsync(loginDto.Username);

                if (user is null)
                {
                    return Results.Unauthorized();
                }

                var isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (!isValidPassword)
                {
                    return Results.Unauthorized();
                }

                // Generate an access token

                var response = new AuthResponseDto
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Token = "AccessTokenHere"
                };

                return Results.Ok(response);
            }).AllowAnonymous();

            app.Run();
        }
    }
}
