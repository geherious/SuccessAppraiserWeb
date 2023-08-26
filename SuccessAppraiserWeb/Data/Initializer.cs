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
                    context.Database.EnsureCreated();
                    TemplateWithStatesInitializer.Initialize(context);
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
