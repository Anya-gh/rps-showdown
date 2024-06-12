using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<_>( options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")) );
builder.Services.AddCors(options => {
  options.AddPolicy(name: MyAllowSpecificOrigins,
    policy => {
      policy.WithOrigins("http://localhost:3000")
      .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

//var optionsBuilder = new DbContextOptionsBuilder<_>(); 
//optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));