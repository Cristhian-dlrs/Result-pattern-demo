﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechDemo.Infrastructure.EntityFramework;

#nullable disable

namespace TechDemo.Infrastructure.EntityFramework.migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240823194344_AddDeferredEvents")]
    partial class AddDeferredEvents
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TechDemo.Domain.Permissions.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("EmployeeForename")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmployeeSurname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("PermissionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PermissionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Permissions", (string)null);
                });

            modelBuilder.Entity("TechDemo.Domain.Permissions.Models.PermissionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("PermissionTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "owner"
                        },
                        new
                        {
                            Id = 2,
                            Description = "admin"
                        },
                        new
                        {
                            Id = 3,
                            Description = "editor"
                        },
                        new
                        {
                            Id = 4,
                            Description = "publisher"
                        },
                        new
                        {
                            Id = 5,
                            Description = "moderator"
                        });
                });

            modelBuilder.Entity("TechDemo.Infrastructure.EntityFramework.Outbox.DeferredEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OcurredOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProcessedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DeferredMessages", (string)null);
                });

            modelBuilder.Entity("TechDemo.Domain.Permissions.Models.Permission", b =>
                {
                    b.HasOne("TechDemo.Domain.Permissions.Models.PermissionType", null)
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
