using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register HttpClient
builder.Services.AddHttpClient();

// Add authentication using JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourIssuer",
            ValidAudience = "yourAudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Custom routing middleware
app.Map("/{**path}", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    var path = context.Request.Path.Value;
    var method = context.Request.Method;

    // Route requests based on path (you can configure this as needed)
    string? serviceUrl = path switch
    {
        var p when p.StartsWith("/movies") => "http://localhost:5001", // MovieService
        var p when p.StartsWith("/bookings") => "http://localhost:5002", // BookingService
        _ => null
    };

    if (serviceUrl == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Service not found.");
        return;
    }

    // Forward the request to the appropriate microservice
    var client = httpClientFactory.CreateClient();
    var requestMessage = new HttpRequestMessage(new HttpMethod(method), $"{serviceUrl}{path}");

    // Copy headers from the original request
    foreach (var header in context.Request.Headers)
    {
        requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
    }

    // Forward request body if necessary (e.g., for POST or PUT requests)
    if (method == HttpMethod.Post.Method || method == HttpMethod.Put.Method)
    {
        requestMessage.Content = new StreamContent(context.Request.Body);
    }

    // Send the request to the microservice
    var responseMessage = await client.SendAsync(requestMessage);

    // Copy the response from the microservice back to the client
    context.Response.StatusCode = (int)responseMessage.StatusCode;
    foreach (var header in responseMessage.Headers)
    {
        context.Response.Headers[header.Key] = header.Value.ToArray();
    }

    await responseMessage.Content.CopyToAsync(context.Response.Body);
});

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
