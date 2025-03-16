using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JwtRefreshTokenBackend.CustomMiddlewares;
using System.Text;
using ApplicationLayer.Repositriy;
using ApplicationLayer.Services;
using InfrastrctureLayer.AppDbContext;
using InfrastrctureLayer.Repositriy;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS (you can adjust the policy as needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // ✅ Allow your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ✅ Needed if using authentication (cookies, tokens)
    });
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthRepositriy, AuthRepositriyImpl>();
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"]
        };
    });

// Add Authorization services
builder.Services.AddAuthorization();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Configure Swagger to include authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Define security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token."
    });

    // Apply security globally
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();


// Enable CORS using the defined policy
app.UseCors("AllowAngularApp"); // ✅ Apply CORS middleware before authentication

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture Jwt Refresh Token V1");
    });
}

// Add custom JWT middleware if needed
app.UseMiddleware<JwtMiddleware>();

// Use Authentication and Authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
