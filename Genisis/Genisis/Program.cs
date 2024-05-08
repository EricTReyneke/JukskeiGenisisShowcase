using Business.DynamicModelReflector.DataOperations;
using Business.DynamicModelReflector.Helpers;
using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.ModelReflectors;
using Business.DynamicModelReflector.QueryBuilders;
using Business.GalaxiaWordle.Interfaces;
using Business.GalaxiaWordle.Login.Logins;
using Business.GalaxiaWordle.Registrations;
using Business.Genesis.Scheduler.Interfaces;
using Business.Genesis.Scheduler.Stratagies;
using Business.Genisis.Data.Contexts;
using Business.Genisis.Data.Interfaces;
using Business.Genisis.DataAccess.DataAccess;
using Business.Genisis.DataAccess.DataTransactions;
using Business.Genisis.DataAccess.Interfaces;
using Business.Genisis.EmailService.Interfaces;
using Business.Genisis.EmailVerification.Mailers;
using Business.Genisis.Encryption;
using Business.Genisis.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Service Injection
// Data Access Services
builder.Services.AddScoped<IDataOperations, SqlDataOperations>();
builder.Services.AddScoped<IDataOperationHelper, SqlDataOperationHelper>();
builder.Services.AddScoped<IQueryBuilder, SqlQueryBuilder>();
builder.Services.AddScoped<IModelReflector, SqlModelReflector>();

// Authentication and Security Services
builder.Services.AddScoped<ILogin, HashedLogin>();
builder.Services.AddScoped<IRegistration, HashedRegistration>();
builder.Services.AddScoped<IEncryption, AesEncryptionService>();

// Domain Specific Data Operations
builder.Services.AddScoped<ITournamentDataOperations, TournamentReflector>();
builder.Services.AddScoped<IUserInformationDataOperations, UserInformationReflector>();
builder.Services.AddScoped<ICategoryDataOperations, CategoryReflector>();
builder.Services.AddScoped<IPlayerTeamDataOperations, PlayerTeamReflector>();
builder.Services.AddScoped<IScoresAllocationsDataOperations, ScoresAllocationsReflector>();
builder.Services.AddScoped<IResetPasswordTokenDataOperations, ResetPasswordTokenReflector>();

// Mailer Services
builder.Services.AddScoped<IMailerContext, GenisisMailerContext>();
builder.Services.AddScoped<IMailerSerivce, BasicMailer>();

//Schedular Injections
builder.Services.AddScoped<IMatchmakingStrategy, MatchmakingStrategy>();
#endregion

builder.Services.AddRazorPages();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();