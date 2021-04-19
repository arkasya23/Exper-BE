﻿// <auto-generated />
using System;
using ExperBE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExperBE.Migrations
{
    [DbContext(typeof(ExperDbContext))]
    partial class ExperDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExperBE.Models.Entities.GroupExpense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2047)
                        .HasColumnType("nvarchar(2047)");

                    b.Property<bool>("DivideBetweenAllMembers")
                        .HasColumnType("bit");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("TripId");

                    b.ToTable("GroupExpenses");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.GroupExpenseUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupExpenseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("GroupExpenseId", "UserId")
                        .IsUnique();

                    b.ToTable("GroupExpenseUsers");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2047)
                        .HasColumnType("nvarchar(2047)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.PersonalExpense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2047)
                        .HasColumnType("nvarchar(2047)");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("TripId");

                    b.ToTable("PersonalExpenses");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.Trip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TripUser", b =>
                {
                    b.Property<Guid>("TripsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TripsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("TripUser");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.GroupExpense", b =>
                {
                    b.HasOne("ExperBE.Models.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .IsRequired();

                    b.HasOne("ExperBE.Models.Entities.Trip", "Trip")
                        .WithMany("GroupExpenses")
                        .HasForeignKey("TripId")
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.GroupExpenseUser", b =>
                {
                    b.HasOne("ExperBE.Models.Entities.GroupExpense", "GroupExpense")
                        .WithMany("Users")
                        .HasForeignKey("GroupExpenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExperBE.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("GroupExpense");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.Notification", b =>
                {
                    b.HasOne("ExperBE.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.PersonalExpense", b =>
                {
                    b.HasOne("ExperBE.Models.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .IsRequired();

                    b.HasOne("ExperBE.Models.Entities.Trip", "Trip")
                        .WithMany("PersonalExpenses")
                        .HasForeignKey("TripId")
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.Trip", b =>
                {
                    b.HasOne("ExperBE.Models.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .IsRequired();

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("TripUser", b =>
                {
                    b.HasOne("ExperBE.Models.Entities.Trip", null)
                        .WithMany()
                        .HasForeignKey("TripsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExperBE.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExperBE.Models.Entities.GroupExpense", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ExperBE.Models.Entities.Trip", b =>
                {
                    b.Navigation("GroupExpenses");

                    b.Navigation("PersonalExpenses");
                });
#pragma warning restore 612, 618
        }
    }
}
