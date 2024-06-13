using Microsoft.EntityFrameworkCore;

var corsPolicy = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RPSDbContext>( options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")) );
builder.Services.AddCors(options => {
  options.AddPolicy(name: corsPolicy,
    policy => {
      policy.WithOrigins("http://localhost:3000")
      .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors(corsPolicy);

var optionsBuilder = new DbContextOptionsBuilder<RPSDbContext>(); 
optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));