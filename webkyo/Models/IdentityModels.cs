using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace webkyo.Models
{
    // New derived classes
    public class UserRole : IdentityUserRole<int> 
	{
	}

	public class UserClaim : IdentityUserClaim<int>
	{
	}

	public class UserLogin : IdentityUserLogin<int>
	{
	}

	public class Role : IdentityRole<int, UserRole>
	{
		public Role() { }
		public Role(string name) { Name = name; }
	}

	public class UserStore : UserStore<ApplicationUser, Role, int,
		UserLogin, UserRole, UserClaim>
	{
		public UserStore(ApplicationDbContext context)
			: base(context)
		{
		}
	}

	public class RoleStore : RoleStore<Role, int, UserRole>
	{
		public RoleStore(ApplicationDbContext context)
			: base(context)
		{
		}
	}

	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
	{
		public int? DojoId {get;set;}

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}

    //public class DataBaseInitializer
    //    {
    //        public DataBaseInitializer()
    //        {
    //            Database.SetInitializer<ApplicationDbContext>(null);

    //            try
    //            {
    //                using (var context = new ApplicationDbContext())
    //                {
    //                    if (!context.Database.Exists())
    //                    {
    //                        // Create the SimpleMembership database without Entity Framework migration schema
    //                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
    //                    }
    //                }

    //                //WebSecurity.InitializeDatabaseConnection("DefaultConnection", "webpages_UserProfile", "UserId", "UserName", autoCreateTables: false);
    //            }
    //            catch (Exception ex)
    //            {
    //                throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
    //            }
    //        }
    //    }

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int,UserLogin, UserRole, UserClaim>
	{
		public ApplicationDbContext() : base("DefaultConnection")
		{
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

			//modelBuilder.Entity<Kyo.Entidades.Alumno>().HasRequired(a => a.Dojo).WithRequiredPrincipal();
			//modelBuilder.Entity<Kyo.Entidades.Alumno>().HasRequired(a => a.Cinturon).WithRequiredPrincipal();
			//modelBuilder.Entity<Kyo.Entidades.Examen>().HasRequired(a => a.Alumno).WithRequiredPrincipal();
			//modelBuilder.Entity<Kyo.Entidades.Asistencia>().HasRequired(a => a.Alumno).WithRequiredPrincipal();

			//modelBuilder.Entity<Use>().ToTable("Users");
			//modelBuilder.Entity<Role>().ToTable("Roles");
			//modelBuilder.Entity<UserRole>().ToTable("UserRoles");
			//modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
			//modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

		public System.Data.Entity.DbSet<Kyo.Entidades.Dojo> Dojos { get; set; }

		public System.Data.Entity.DbSet<Kyo.Entidades.Cinturon> Cinturones { get; set; }

		public System.Data.Entity.DbSet<Kyo.Entidades.Alumno> Alumnos { get; set; }

		public System.Data.Entity.DbSet<Kyo.Entidades.Asistencia> Asistencias { get; set; }

		public System.Data.Entity.DbSet<Kyo.Entidades.Examen> Examenes { get; set; }

		public System.Data.Entity.DbSet<Kyo.Entidades.Usuario> Usuarios { get; set; }
	}
}