﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MyCoreDAL;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using System;

namespace MyCoreDAL.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("MyCoreDAL.Customer", b =>
                {
                    b.Property<string>("CustomerGuid")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50);

                    b.Property<DateTime?>("AdditionDate");

                    b.Property<long?>("AdditionUnix");

                    b.Property<string>("CreatePeople")
                        .HasMaxLength(50);

                    b.Property<string>("CustomerName")
                        .HasMaxLength(50);

                    b.Property<string>("CustomerPhone")
                        .HasMaxLength(50);

                    b.Property<int>("ID");

                    b.Property<bool?>("IsDelete");

                    b.HasKey("CustomerGuid");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("MyCoreDAL.CustomerAddress", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("AdditionDate");

                    b.Property<long?>("AdditionUnix");

                    b.Property<string>("Address")
                        .HasMaxLength(200);

                    b.Property<string>("AddressGuid")
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .HasMaxLength(50);

                    b.Property<string>("CreatePeople")
                        .HasMaxLength(50);

                    b.Property<string>("CustomerGuid")
                        .HasMaxLength(50);

                    b.Property<string>("District")
                        .HasMaxLength(50);

                    b.Property<bool?>("IsDelete");

                    b.Property<string>("Province")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("CustomerGuid");

                    b.ToTable("CustomerAddress");
                });

            modelBuilder.Entity("MyCoreDAL.CustomerAddress", b =>
                {
                    b.HasOne("MyCoreDAL.Customer")
                        .WithMany("CustomerAddress")
                        .HasForeignKey("CustomerGuid");
                });
#pragma warning restore 612, 618
        }
    }
}
