using Microsoft.EntityFrameworkCore;

namespace 水水水果API.Models.AUTH
{

    public partial class TWAUTH_TESTContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("User_Seq").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<User>()
                    .Property(c => c.Id)
                    .HasDefaultValueSql($"NEXT VALUE FOR User_Seq")
                    .ValueGeneratedOnAdd(); ;

            modelBuilder.HasSequence<int>("RefreshToken_Seq").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<RefreshToken>()
                    .Property(c => c.Id)
                    .HasDefaultValueSql($"NEXT VALUE FOR RefreshToken_Seq")
                    .ValueGeneratedOnAdd(); ;
        }
    }

}
