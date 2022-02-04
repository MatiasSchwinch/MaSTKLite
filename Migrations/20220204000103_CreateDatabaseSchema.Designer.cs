﻿// <auto-generated />
using System;
using MaSTK_Lite.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MaSTK_Lite.Migrations
{
    [DbContext(typeof(DBConnector))]
    [Migration("20220204000103_CreateDatabaseSchema")]
    partial class CreateDatabaseSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("MaSTK_Lite.Model.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("CategoryID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("MaSTK_Lite.Model.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("CategoryID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(240)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(40)");

                    b.Property<float>("Price")
                        .HasColumnType("money");

                    b.Property<string>("ProductSKU")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("WarehouseID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("WarehouseID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("MaSTK_Lite.Model.Warehouse", b =>
                {
                    b.Property<int>("WarehouseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(240)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("WarehouseID");

                    b.ToTable("Warehouse");
                });

            modelBuilder.Entity("MaSTK_Lite.Model.Product", b =>
                {
                    b.HasOne("MaSTK_Lite.Model.Category", "Category")
                        .WithMany("Product")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MaSTK_Lite.Model.Warehouse", "Warehouse")
                        .WithMany("Products")
                        .HasForeignKey("WarehouseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("MaSTK_Lite.Model.Category", b =>
                {
                    b.Navigation("Product");
                });

            modelBuilder.Entity("MaSTK_Lite.Model.Warehouse", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
