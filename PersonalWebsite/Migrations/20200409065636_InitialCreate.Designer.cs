﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalWebsite.GithubService;

namespace PersonalWebsite.Migrations
{
    [DbContext(typeof(GithubRepositoryContext))]
    [Migration("20200409065636_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PersonalWebsite.GithubService.AdditionalRepositoryData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustomExperience")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastEdit")
                        .HasColumnType("datetime2");

                    b.Property<int>("RepositoryId")
                        .HasColumnType("int");

                    b.Property<int>("TextFormat")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AdditionalData");
                });

            modelBuilder.Entity("PersonalWebsite.GithubService.CustomExperienceImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdditionalRepositoryDataId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Content")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdditionalRepositoryDataId");

                    b.ToTable("CustomExperienceImages");
                });

            modelBuilder.Entity("PersonalWebsite.GithubService.GithubRepository", b =>
                {
                    b.Property<int>("RepositoryId")
                        .HasColumnType("int");

                    b.Property<int>("AdditionalRepositoryDataId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Readme")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("RepositoryId");

                    b.HasIndex("AdditionalRepositoryDataId");

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("PersonalWebsite.GithubService.CustomExperienceImage", b =>
                {
                    b.HasOne("PersonalWebsite.GithubService.AdditionalRepositoryData", null)
                        .WithMany("CustomExperienceImages")
                        .HasForeignKey("AdditionalRepositoryDataId");
                });

            modelBuilder.Entity("PersonalWebsite.GithubService.GithubRepository", b =>
                {
                    b.HasOne("PersonalWebsite.GithubService.AdditionalRepositoryData", "AdditionalRepositoryData")
                        .WithMany()
                        .HasForeignKey("AdditionalRepositoryDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
