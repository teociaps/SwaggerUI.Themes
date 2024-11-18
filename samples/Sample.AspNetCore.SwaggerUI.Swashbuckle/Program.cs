using AspNetCore.Swagger.Themes;
using Sample.AspNetCore.SwaggerUI.Swashbuckle;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerGenConfigurer.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI(CustomModernStyle.CustomModern, c => c.DocumentTitle = "Sample Title");
    //app.UseSwaggerUI(Assembly.GetExecutingAssembly(), "modern.custom.css", c =>
    app.UseSwaggerUI(ModernStyle.Dark, c =>
    {
        c.DocumentTitle = "Sample Title";
        c.EnableAllAdvancedOptions();
        //c.EnablePinnableTopbar();
        //c.ShowBackToTopButton();
        //c.EnableStickyOperations();
        //c.EnableExpandOrCollapseAllOperations();
    });
}

app.UseHttpsRedirection();

app.AddEndpoints();
app.MapControllers();

await app.RunAsync();