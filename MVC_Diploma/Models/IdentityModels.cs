using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVC_Diploma.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string OfficeId { get; set; }
        public bool UserStatus { get; set; }
        public string CounterId { get; set; }
        public string ReputationId { get; set; }
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
        public string OfficeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool OfficeStatus { get; set; }
    }

    public class Counters
    {
        [Key]
        public string CounterId { get; set; }
        public string CounterTypeId { get; set; }
        public bool CounterStatus { get; set; }

    }

    public class Requests
    {
        [Key]
        public string RequestId { get; set; }
        public string ServiceId { get; set; }
        public string UserId { get; set; }
        public string ManagerId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public bool RequestStatus { get; set; }
        public string MasterAdmit { get; set; }
        public string ManagerAdmit { get; set; }
        public bool UserMark { get; set; }
        public DateTime Date { get; set; }

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
        public string ServiceId { get; set; }
        public string ServiceTypeId { get; set; }
        public string Description { get; set; }
        public decimal MoneyForService { get; set; }
    }

    public class ServiceType
    {
        [Key]
        public string ServiceTypeId { get; set; }
        public string Type { get; set; }
    }
    public class Reputation
    {
        [Key]
        public string ReputationId { get; set; }
        public decimal Value { get; set; }
        public int NumberOfVotes { get; set; }
    }
    public class Order
    {
        public string Id { get; set; } // id заказа
        public DateTime? Date { get; set; } // дата
        public decimal Sum { get; set; } // сумма заказа
        public string Sender { get; set; } // отправитель - кошелек в ЯД
        public string Operation_Id { get; set; } // id операции в ЯД
        public decimal? Amount { get; set; } // сумма, которую заплатали с учетом комиссии
        public decimal? WithdrawAmount { get; set; } // сумма, которую заплатали без учета комиссии
        public int? UserId { get; set; } // id пользователя в системе, который сделал заказ
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Office> Office { get; set; }
        public DbSet<Counters> Counters { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<CounterType> CounterType { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceType> ServiceType { get; set; }
        public DbSet<Reputation> Reputation { get; set; }
        public DbSet<Order> Orders { get; set; }

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