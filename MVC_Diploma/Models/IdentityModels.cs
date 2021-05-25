using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVC_Diploma.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public Guid OfficeId { get; set; }
        public bool UserStatus { get; set; }
        public Guid CounterId { get; set; }
        public Guid ReputationId { get; set; }
        public decimal AccountMoney { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class Office
    {
        public Guid OfficeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool OfficeStatus { get; set; }
    }

    public class Counters
    {
        [Key]
        public Guid CounterId { get; set; }
        public Guid CounterTypeId { get; set; }
        public bool CounterStatus { get; set; }

    }

    public class Requests
    {
        [Key]
        public Guid RequestId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid UserId { get; set; }
        public Guid ManagerId { get; set; }
        public bool RequestStatus { get; set; }

    }

    public class CounterType
    {
        public Guid CounterTypeId { get; set; }
        public decimal FirstMeterReading { get; set; }
        public decimal SecondMeterReading { get; set; }
        public decimal MoneyPerMeasure { get; set; }
    }

    public class Service
    {
        [Key]
        public Guid ServiceId { get; set; }
        public Guid ServiceTypeId { get; set; }
        public string Description { get; set; }
        public decimal MoneyForService { get; set; }
    }

    public class ServiceType
    {
        [Key]
        public Guid ServiceTypeId { get; set; }
        public string Type { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Office> Office { get; set; }
        public DbSet<Counters> Counters { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<CounterType> CounterType { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceType> ServiceType { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}