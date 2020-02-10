using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace v2.Model
{
    public partial class allcompanyContext : DbContext
    {
        public allcompanyContext()
        {
        }

        public allcompanyContext(DbContextOptions<allcompanyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Companydata> Companydata { get; set; }
        public virtual DbSet<Holdingdaily> Holdingdaily { get; set; }
        public virtual DbSet<Stockcompany> Stockcompany { get; set; }
        public IConfiguration Configuration { get; }

        // Unable to generate entity type for table 'public.backup_holdingdaily_2019_07_10'. Please see the warning messages.
        // Unable to generate entity type for table 'public.stockcompany_copy'. Please see the warning messages.
        // Unable to generate entity type for table 'public.part_holdingdaily'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Companydata>(entity =>
            {
                entity.ToTable("companydata");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Companyaddress).HasColumnName("companyaddress");

                entity.Property(e => e.Companyname)
                    .HasColumnName("companyname")
                    .HasMaxLength(100);

                entity.Property(e => e.Govcompanyid)
                    .HasColumnName("govcompanyid")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Holdingdaily>(entity =>
            {
                entity.HasKey(e => new { e.Companyid, e.Stockid, e.Keepdata })
                    .HasName("holdingdaily_pkey");

                entity.ToTable("holdingdaily");

                entity.Property(e => e.Companyid).HasColumnName("companyid");

                entity.Property(e => e.Stockid).HasColumnName("stockid");

                entity.Property(e => e.Keepdata)
                    .HasColumnName("keepdata")
                    .HasColumnType("date");

                entity.Property(e => e.Holdingcount)
                    .HasColumnName("holdingcount")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<Stockcompany>(entity =>
            {
                entity.ToTable("stockcompany");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Act).HasColumnName("act");

                entity.Property(e => e.Credate)
                    .HasColumnName("credate")
                    .HasColumnType("date");

                entity.Property(e => e.SName)
                    .HasColumnName("s_name")
                    .HasMaxLength(40);

                entity.Property(e => e.SNo)
                    .HasColumnName("s_no")
                    .HasMaxLength(11);

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");
            });

            modelBuilder.HasSequence("all_company_index_quene").HasMax(2478);

            modelBuilder.HasSequence("companydatasequene");

            modelBuilder.HasSequence("shareholdingdailysequene");

            modelBuilder.HasSequence("stocksequene");
        }
    }
}
