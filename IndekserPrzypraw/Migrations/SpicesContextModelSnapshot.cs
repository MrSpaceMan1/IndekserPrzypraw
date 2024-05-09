﻿// <auto-generated />
using System;
using IndekserPrzypraw.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IndekserPrzypraw.Migrations
{
    [DbContext(typeof(SpicesContext))]
    partial class SpicesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IndekserPrzypraw.Models.Drawer", b =>
                {
                    b.Property<int>("DrawerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DrawerId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DrawerId");

                    b.ToTable("Drawers");
                });

            modelBuilder.Entity("IndekserPrzypraw.Models.Spice", b =>
                {
                    b.Property<int>("SpiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SpiceId"));

                    b.Property<DateOnly?>("ExpirationDate")
                        .HasColumnType("date");

                    b.Property<int>("SpiceGroupId")
                        .HasColumnType("integer");

                    b.HasKey("SpiceId");

                    b.HasIndex("SpiceGroupId");

                    b.ToTable("Spices");
                });

            modelBuilder.Entity("IndekserPrzypraw.Models.SpiceGroup", b =>
                {
                    b.Property<int>("SpiceGroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SpiceGroupId"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DrawerId")
                        .HasColumnType("integer");

                    b.Property<int>("Grams")
                        .HasColumnType("integer");

                    b.Property<int?>("MinimumCount")
                        .HasColumnType("integer");

                    b.Property<int?>("MinimumGrams")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SpiceGroupId");

                    b.HasIndex("DrawerId");

                    b.ToTable("SpiceGroups");
                });

            modelBuilder.Entity("IndekserPrzypraw.Models.Spice", b =>
                {
                    b.HasOne("IndekserPrzypraw.Models.SpiceGroup", "SpiceGroup")
                        .WithMany("Spices")
                        .HasForeignKey("SpiceGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SpiceGroup");
                });

            modelBuilder.Entity("IndekserPrzypraw.Models.SpiceGroup", b =>
                {
                    b.HasOne("IndekserPrzypraw.Models.Drawer", "Drawer")
                        .WithMany("SpiceGroups")
                        .HasForeignKey("DrawerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Drawer");
                });

            modelBuilder.Entity("IndekserPrzypraw.Models.Drawer", b =>
                {
                    b.Navigation("SpiceGroups");
                });

            modelBuilder.Entity("IndekserPrzypraw.Models.SpiceGroup", b =>
                {
                    b.Navigation("Spices");
                });
#pragma warning restore 612, 618
        }
    }
}
