﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server.Model;

namespace Server.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200901021820_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Server.Model.Department", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Is_Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("ts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Server.Model.Employee", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created_Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("Dept_Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<DateTime>("End_Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Phone")
                        .HasColumnType("integer");

                    b.Property<string>("PhotoName")
                        .HasColumnType("text");

                    b.Property<long>("Sub_Dept_Id")
                        .HasColumnType("bigint");

                    b.Property<long>("User_Id")
                        .HasColumnType("bigint");

                    b.Property<string>("User_Name")
                        .HasColumnType("text");

                    b.Property<bool>("isActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Dept_Id");

                    b.HasIndex("Sub_Dept_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Server.Model.Menu", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created_Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Menu_Name")
                        .HasColumnType("text");

                    b.Property<string>("RoutePath")
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<long>("Parent_Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Updated_Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Menu");
                });

            modelBuilder.Entity("Server.Model.MenuRole", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created_Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("Menu_Id")
                        .HasColumnType("bigint");

                    b.Property<long>("Role_Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Updated_Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("isActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Menu_Id");

                    b.HasIndex("Role_Id");

                    b.ToTable("MenuRole");
                });

            modelBuilder.Entity("Server.Model.Role", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<bool>("isActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Server.Model.SubDepartment", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("Dept_Id")
                        .HasColumnType("bigint");

                    b.Property<long>("Is_Active")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("ts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Dept_Id");

                    b.ToTable("SubDepartments");
                });

            modelBuilder.Entity("Server.Model.ThankCard", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("From_Employee_Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ReplyDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ReplyText")
                        .HasColumnType("text");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SendText")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<long>("To_Employee_Id")
                        .HasColumnType("bigint");

                    b.Property<bool>("isActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("ThankCards");
                });

            modelBuilder.Entity("Server.Model.User", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created_Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<long>("Role_ID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Updated_Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("User_Name")
                        .HasColumnType("text");

                    b.Property<bool>("isActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Role_ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Server.Model.Employee", b =>
                {
                    b.HasOne("Server.Model.Department", "Department")
                        .WithMany()
                        .HasForeignKey("Dept_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Model.SubDepartment", "SubDepartment")
                        .WithMany()
                        .HasForeignKey("Sub_Dept_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Server.Model.MenuRole", b =>
                {
                    b.HasOne("Server.Model.Menu", "Menu")
                        .WithMany()
                        .HasForeignKey("Menu_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Model.Role", "Role")
                        .WithMany()
                        .HasForeignKey("Role_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Server.Model.SubDepartment", b =>
                {
                    b.HasOne("Server.Model.Department", "Department")
                        .WithMany()
                        .HasForeignKey("Dept_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Server.Model.User", b =>
                {
                    b.HasOne("Server.Model.Role", "Role")
                        .WithMany()
                        .HasForeignKey("Role_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
