using System.Text;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Services;
using BookManagementSystem.Application.Validators.Users;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Data.UnitOfWork;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Data.Seed;
using BookManagementSystem.Infrastructure.Repositories.Book;
using BookManagementSystem.Infrastructure.Repositories.Customer;
using BookManagementSystem.Infrastructure.Repositories.User;
using BookManagementSystem.Infrastructure.Repositories.DebtReport;
using BookManagementSystem.Infrastructure.Repositories.DebtReportDetail;
using BookManagementSystem.Infrastructure.Repositories.PaymentReceipt;
using BookManagementSystem.Infrastructure.Repositories.Invoice;
using BookManagementSystem.Infrastructure.Repositories.InvoiceDetail;
using BookManagementSystem.Infrastructure.Repositories.BookEntry;
using BookManagementSystem.Infrastructure.Repositories.BookEntryDetail;
using BookManagementSystem.Middlewares;
using BookManagementSystem.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using BookManagementSystem.Infrastructure.Repositories.InventoryReport;
using BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail;
using BookManagementSystem.Infrastructure.Repositories.Regulation;
using Microsoft.Extensions.Options;
using BookManagementSystem.Configuration.Settings;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<AzureConfig>(builder.Configuration.GetSection("AzureStorage"));
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
}).AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
    };
    options.IncludeErrorDetails = true;
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IDebtReportRepository, DebtReportRepository>();
builder.Services.AddScoped<IDebtReportDetailRepository, DebtReportDetailRepository>();
builder.Services.AddScoped<IInventoryReportDetailRepository, InventoryReportDetailRepository>();
builder.Services.AddScoped<IInventoryReportRepository, InventoryReportRepository>();
builder.Services.AddScoped<IRegulationRepository, RegulationRepository>();
builder.Services.AddScoped<IPaymentReceiptRepository, PaymentReceiptRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
builder.Services.AddScoped<IBookEntryRepository, BookEntryRepository>();
builder.Services.AddScoped<IBookEntryDetailRepository, BookEntryDetailRepository>();
builder.Services.AddSingleton<AzureBlobService>();
// Register services 
builder.Services.AddSingleton<IUriService>(o =>
{
    IHttpContextAccessor accessor = o.GetRequiredService<IHttpContextAccessor>();
    HttpRequest? request = accessor.HttpContext.Request;
    string? uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});

builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDebtReportService, DebtReportService>();
builder.Services.AddScoped<IDebtReportDetailService, DebtReportDetailService>();
builder.Services.AddScoped<IPaymentReceiptService, PaymentReceiptService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceDetailService, InvoiceDetailService>();
builder.Services.AddScoped<IInventoryReportDetailService, InventoryReportDetailService>();
builder.Services.AddScoped<IInventoryReportService, InventoryReportService>();
builder.Services.AddScoped<IBookEntryService, BookEntryService>();
builder.Services.AddScoped<IBookEntryDetailService, BookEntryDetailService>();
builder.Services.AddScoped<IRegulationService, RegulationService>();
builder.Services.AddScoped<IBookService, BookService>();

// DI Container
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddHostedService<BookManagementSystem.Services.InventoryReportMonthlyCreateBackgroundService>();
builder.Services.AddHostedService<BookManagementSystem.Services.DebtReportBackgroundService>();


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var regulationService = services.GetRequiredService<IRegulationService>();
    SeedData.SeedEssentialsAsync(userManager, roleManager).Wait();
    SeedData.SeedRegulations(regulationService).Wait();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();
app.MapControllers();
app.Run();