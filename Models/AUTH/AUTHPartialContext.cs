using Microsoft.EntityFrameworkCore;

namespace 水水水果API.Models.AUTH
{

    public partial class TWAUTH_TESTContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            List<string> seqList =
            [
                "User_Seq",
                "RefreshToken_Seq",
            ];

            foreach (string seq in seqList)
            {
                modelBuilder.HasSequence<int>(seq).StartsAt(1).IncrementsBy(1);
                modelBuilder.Entity<Customer>()
                        .Property(c => c.Id)
                        .HasDefaultValueSql($"NEXT VALUE FOR {seq}");
            }
        }
    }
 
}
