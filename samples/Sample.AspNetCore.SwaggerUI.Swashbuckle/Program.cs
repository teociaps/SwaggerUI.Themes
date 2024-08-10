using AspNetCore.SwaggerUI.Themes;
using Sample.AspNetCore.SwaggerUI.Swashbuckle;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerGenConfigurer.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ModernStyle.Dark, c => c.DocumentTitle = "Sample Title");
}

app.UseHttpsRedirection();

app.AddEndpoints();
app.MapControllers();

await app.RunAsync();