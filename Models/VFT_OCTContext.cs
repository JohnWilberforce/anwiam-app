using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VFT.OCT.Models
{
    public partial class VFT_OCTContext : DbContext
    {
        public VFT_OCTContext()
        {
        }

        public VFT_OCTContext(DbContextOptions<VFT_OCTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Oct> Octs { get; set; } = null!;
        public virtual DbSet<Total> Totals { get; set; } = null!;
        public virtual DbSet<Vft> Vfts { get; set; } = null!;
        public virtual DbSet<OCTreport> OCTreports { get; set; } = null!;
        public virtual DbSet<VFTreport> VFTreports { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB; database=VFT_OCT; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Oct>(entity =>
            {
                entity.HasKey(e => e.ScanId)
                    .HasName("PK__OCT__48D3E22AFB26F10C");

                entity.ToTable("OCT");

                entity.Property(e => e.ScanId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("scanId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Macula)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Onh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ONH");

                entity.Property(e => e.Pachymetry)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("pachymetry");

                entity.Property(e => e.PatientName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("patientName");

                entity.Property(e => e.ReFfacility)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("reFfacility");

                entity.Property(e => e.ReferredDrName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("referredDrName");
            });


            modelBuilder.Entity<Vft>(entity =>
            {
                entity.HasKey(e => e.ScanId)
                    .HasName("PK__VFT__48D3E22A075F4554");

                entity.ToTable("VFT");

                entity.Property(e => e.ScanId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("scanId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.PatientName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("patientName");

                entity.Property(e => e.ReFfacility)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("reFfacility");

                entity.Property(e => e.ReferredDrName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("referredDrName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
