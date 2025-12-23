using Microsoft.EntityFrameworkCore;

namespace 水水水果API.Models.CRM
{
    public partial class TWCRM_TESTContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            List<string> seqList =
            [
                "Customer_Seq",
                "Member_Seq",
                "MemberTier_Seq"
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
