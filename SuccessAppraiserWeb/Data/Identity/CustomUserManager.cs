using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Areas.Identity.models;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Identity
{
    public class CustomUserManager : UserManager<ApplicationUser>
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger, ApplicationDbContext dbContext) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this._dbContext = dbContext;
        }

        public List<GoalItem>? GetUserGoals(ClaimsPrincipal claimsPrincipal)
        {

            if (claimsPrincipal.Identity == null) { return null; }

            return (from u in _dbContext.Users?.Include(u => u.Goals)
                    where u.UserName
                    == claimsPrincipal.Identity.Name
                    select u.Goals).FirstOrDefault();
        }
    }
}
