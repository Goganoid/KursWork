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
    [Migration("20220413141333_AddCompanyNameRenameSomeFields")]
    partial class AddCompanyNameRenameSomeFields
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

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserOwnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserOwnerId");

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
                    b.Property<int>("Id")
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

                    b.HasKey("Id");

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
                        .HasForeignKey("UserOwnerId")
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
                    b.HasOne("Tenders.Models.Company", "CompanyExecutor")
                        .WithMany("WonTenders")
                        .HasForeignKey("CompanyExecutorId");

                    b.HasOne("Tenders.Models.Company", "CompanyOrganizer")
                        .WithMany("Tenders")
                        .HasForeignKey("CompanyOrganizerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompanyExecutor");

                    b.Navigation("CompanyOrganizer");
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