using Serilog;
using SuccessAppraiserWeb.Data.Goal.Initialize;
using SuccessAppraiserWeb.Data.Goal.Initialize.Templates;

namespace SuccessAppraiserWeb.Data
{
    public static class Initializer
    {
        public static WebApplication Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                try
                {
                    Log.Debug("Seeding");
                    context.Database.EnsureCreated();
                    TemplateWithStatesInitializer.Initialize(context);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return app;
            }
        }
    }
}
