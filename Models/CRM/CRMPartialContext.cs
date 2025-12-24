using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

namespace 水水水果API.Models.CRM
{
    public partial class TWCRM_TESTContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

            modelBuilder.HasSequence<int>("Customer_Seq").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Customer>()
                    .Property(c => c.Id)
                    .HasDefaultValueSql($"NEXT VALUE FOR Customer_Seq")
                    .ValueGeneratedOnAdd();

            modelBuilder.HasSequence<int>("Member_Seq").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Member>()
                    .Property(c => c.Id)
                    .HasDefaultValueSql($"NEXT VALUE FOR Member_Seq")
                    .ValueGeneratedOnAdd(); ;

            modelBuilder.HasSequence<int>("MemberTier_Seq").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<MemberTier>()
                    .Property(c => c.Id)
                    .HasDefaultValueSql($"NEXT VALUE FOR MemberTier_Seq")
                    .ValueGeneratedOnAdd();
        }
    }
}
