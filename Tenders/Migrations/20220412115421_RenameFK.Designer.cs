﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tenders.Data;

#nullable disable

namespace Tenders.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20220412115421_RenameFK")]
    partial class RenameFK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("Tenders.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyInfo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Tenders.Models.Proposition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TenderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("TenderId");

                    b.ToTable("Propositions");
                });

            modelBuilder.Entity("Tenders.Models.Tender", b =>
                {
                    b.Property<int>("TenderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CompanyExecutorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyOrganizerId")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PubDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TenderId");

                    b.HasIndex("CompanyExecutorId");

                    b.HasIndex("CompanyOrganizerId");

                    b.ToTable("Tenders");
                });

            modelBuilder.Entity("Tenders.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Tenders.Models.Company", b =>
                {
                    b.HasOne("Tenders.Models.User", "Owner")
                        .WithMany("Companies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Tenders.Models.Proposition", b =>
                {
                    b.HasOne("Tenders.Models.Company", "Company")
                        .WithMany("Propositions")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tenders.Models.Tender", "Tender")
                        .WithMany("Propositions")
                        .HasForeignKey("TenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Tender");
                });

            modelBuilder.Entity("Tenders.Models.Tender", b =>
                {
                    b.HasOne("Tenders.Models.Company", "Executor")
                        .WithMany("WonTenders")
                        .HasForeignKey("CompanyExecutorId");

                    b.HasOne("Tenders.Models.Company", "Organizer")
                        .WithMany("Tenders")
                        .HasForeignKey("CompanyOrganizerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Executor");

                    b.Navigation("Organizer");
                });

            modelBuilder.Entity("Tenders.Models.Company", b =>
                {
                    b.Navigation("Propositions");

                    b.Navigation("Tenders");

                    b.Navigation("WonTenders");
                });

            modelBuilder.Entity("Tenders.Models.Tender", b =>
                {
                    b.Navigation("Propositions");
                });

            modelBuilder.Entity("Tenders.Models.User", b =>
                {
                    b.Navigation("Companies");
                });
#pragma warning restore 612, 618
        }
    }
}
