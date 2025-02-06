using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
builder.Services.AddDbContext<ToDoDbContext>(options =>

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 
var app = builder.Build();
app.UseCors("AllowAll");
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>{
        c.SwaggerEndpoint("/swagger/v1/swagger.json","ToDo API V1");
        c.RoutePrefix=string.Empty;
    });
}
app.MapGet("/items",async( ToDoDbContext context) =>
{
    return await context.Items.ToListAsync();
});
app.MapGet("/items/{id}", async (int id, ToDoDbContext context) =>
{
    var searchitem = await context.Items.FindAsync(id);
    await context.SaveChangesAsync();
    if(searchitem!=null)
    return Results.Ok(searchitem);
    return Results.NotFound();
}).RequireAuthorization();
app.MapPost("/items", async (Item newItem, ToDoDbContext toDoDbContext) =>
{
    await toDoDbContext.Items.AddAsync(newItem);
    await toDoDbContext.SaveChangesAsync(); 
    return Results.Created($"/items/{newItem.Id}", newItem); 
});

app.MapPut("/items/{id}", async (ToDoDbContext db, int id) =>
{
    try
    {
        var item = await db.Items.FindAsync(id);
        if (item is null) return Results.NotFound();

        item.IsComplete=!item.IsComplete;

        System.Console.WriteLine(item.IsComplete);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem("An error occurred while updating the item: " + ex.Message);
    }
});

app.MapDelete("/items/{id}", async (int id, ToDoDbContext toDoDbContext) =>
{
    // Find the existing item in the database
    var existingItem = await toDoDbContext.Items.FindAsync(id);
    
    if (existingItem is null)
    {
        return Results.NotFound(); // Return 404 if the item is not found
    }

    toDoDbContext.Items.Remove(existingItem); // Remove the item from the DbSet
    await toDoDbContext.SaveChangesAsync(); // Save changes to the database

    return Results.NoContent(); // Return 204 No Content response
});


app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" }, 
                          () => "This is an options or head request ");

app.Run();