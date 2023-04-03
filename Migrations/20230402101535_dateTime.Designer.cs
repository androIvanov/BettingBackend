﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Betting.Migrations
{
    [DbContext(typeof(XmlSportsContext))]
    [Migration("20230402101535_dateTime")]
    partial class dateTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Users.Bet", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<bool>("IsLive")
                        .HasColumnType("bit");

                    b.Property<int?>("MatchID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("MatchID");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("Users.Event", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<bool>("IsLive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SportID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("SportID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Users.Match", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<int?>("EventID")
                        .HasColumnType("int");

                    b.Property<string>("MatchType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("EventID");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Users.Odd", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<int?>("BetID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("SpecialBetValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ID");

                    b.HasIndex("BetID");

                    b.ToTable("Odds");
                });

            modelBuilder.Entity("Users.Sport", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("Users.Bet", b =>
                {
                    b.HasOne("Users.Match", null)
                        .WithMany("Bets")
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Users.Event", b =>
                {
                    b.HasOne("Users.Sport", null)
                        .WithMany("Events")
                        .HasForeignKey("SportID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Users.Match", b =>
                {
                    b.HasOne("Users.Event", null)
                        .WithMany("Matches")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Users.Odd", b =>
                {
                    b.HasOne("Users.Bet", null)
                        .WithMany("Odds")
                        .HasForeignKey("BetID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Users.Bet", b =>
                {
                    b.Navigation("Odds");
                });

            modelBuilder.Entity("Users.Event", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("Users.Match", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("Users.Sport", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}