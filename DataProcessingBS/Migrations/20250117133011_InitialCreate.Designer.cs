﻿// <auto-generated />
using System;
using DataProcessingBS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataProcessingBS.Migrations
{
    [DbContext(typeof(AppDbcontext))]
    [Migration("20250117133011_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataProcessingBS.Entities.Account", b =>
                {
                    b.Property<int>("Account_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Account_Id"));

                    b.Property<string>("Blocked")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Is_Invited")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Payment_Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("Trial_End_Date")
                        .HasColumnType("date");

                    b.HasKey("Account_Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.ApiKey", b =>
                {
                    b.Property<int>("ApiKey_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApiKey_Id"));

                    b.Property<int>("Account_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Is_Active")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApiKey_Id");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Episode", b =>
                {
                    b.Property<int>("Episode_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Episode_Id"));

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Episode_Number")
                        .HasColumnType("int");

                    b.Property<int>("Media_Id")
                        .HasColumnType("int");

                    b.Property<int>("Season_Number")
                        .HasColumnType("int");

                    b.Property<int>("Series_Id")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Episode_Id");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Genre", b =>
                {
                    b.Property<int>("Genre_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Genre_Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Genre_Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Invitation", b =>
                {
                    b.Property<int>("Invitation_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Invitation_Id"));

                    b.Property<int>("Invitee_Id")
                        .HasColumnType("int");

                    b.Property<int>("Inviter_Id")
                        .HasColumnType("int");

                    b.HasKey("Invitation_Id");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Media", b =>
                {
                    b.Property<int>("Media_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Media_Id"));

                    b.Property<string>("Age_Rating")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Genre_Id")
                        .HasColumnType("int");

                    b.Property<string>("Quality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Media_Id");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Movie", b =>
                {
                    b.Property<int>("Movie_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Movie_Id"));

                    b.Property<string>("Has_Subtitles")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<int>("Media_Id")
                        .HasColumnType("int");

                    b.HasKey("Movie_Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Profile", b =>
                {
                    b.Property<int>("Profile_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Profile_Id"));

                    b.Property<int>("Account_Id")
                        .HasColumnType("int");

                    b.Property<bool>("Child_Profile")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profile_Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("User_Age")
                        .HasColumnType("int");

                    b.HasKey("Profile_Id");

                    b.HasIndex("Account_Id");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Series", b =>
                {
                    b.Property<int>("Series_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Series_Id"));

                    b.Property<string>("Age_Rating")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Genre_Id")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Series_Id");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Subscription", b =>
                {
                    b.Property<int>("Subscription_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Subscription_Id"));

                    b.Property<int>("Account_Id")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Renewal_Date")
                        .HasColumnType("date");

                    b.Property<DateOnly>("Start_Date")
                        .HasColumnType("date");

                    b.Property<decimal?>("Subscription_Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Subscription_Id");

                    b.HasIndex("Account_Id");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Subtitle", b =>
                {
                    b.Property<int>("Subtitle_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Subtitle_Id"));

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Media_Id")
                        .HasColumnType("int");

                    b.HasKey("Subtitle_Id");

                    b.ToTable("Subtitles");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Watch", b =>
                {
                    b.Property<int>("Watch_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Watch_Id"));

                    b.Property<int>("Media_Id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Pause_Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("Profile_Id")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("Watch_Date")
                        .HasColumnType("date");

                    b.HasKey("Watch_Id");

                    b.ToTable("Watches");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.WatchList", b =>
                {
                    b.Property<int>("WatchList_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WatchList_Id"));

                    b.Property<DateOnly>("Added_Date")
                        .HasColumnType("date");

                    b.Property<int>("Media_Id")
                        .HasColumnType("int");

                    b.Property<int>("Profile_Id")
                        .HasColumnType("int");

                    b.HasKey("WatchList_Id");

                    b.ToTable("WatchLists");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Profile", b =>
                {
                    b.HasOne("DataProcessingBS.Entities.Account", "Account")
                        .WithMany("Profiles")
                        .HasForeignKey("Account_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Subscription", b =>
                {
                    b.HasOne("DataProcessingBS.Entities.Account", "Account")
                        .WithMany("Subscriptions")
                        .HasForeignKey("Account_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("DataProcessingBS.Entities.Account", b =>
                {
                    b.Navigation("Profiles");

                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
