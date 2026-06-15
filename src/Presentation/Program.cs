using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Sample", new OpenApiInfo { Version = "v1", Title = "Sample API" });
    c.SwaggerDoc("TodoList", new OpenApiInfo { Version = "v1", Title = "TodoList API" });
    c.DocInclusionPredicate((groupName, apiDesc) => apiDesc.GroupName == groupName);
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/Sample/swagger.json", "Sample API");
    c.SwaggerEndpoint("/swagger/TodoList/swagger.json", "TodoList API");
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});
app.UseHttpsRedirection();
app.MapControllers();
app.Map("/", () => Results.Redirect("/swagger/index.html"));

app.Run();
