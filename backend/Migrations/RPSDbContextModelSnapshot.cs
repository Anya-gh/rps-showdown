﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(RPSDbContext))]
    partial class RPSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Level", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("LevelItems");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Beginner"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Intermediate"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Advanced"
                        });
                });

            modelBuilder.Entity("Match", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BotChoice")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("LevelID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerChoice")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SessionID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("LevelID");

                    b.HasIndex("SessionID");

                    b.HasIndex("UserID");

                    b.ToTable("MatchItems");
                });

            modelBuilder.Entity("Session", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("LevelID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("LevelID");

                    b.HasIndex("UserID");

                    b.ToTable("SessionItems");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("UserItems");
                });

            modelBuilder.Entity("Match", b =>
                {
                    b.HasOne("Level", "Level")
                        .WithMany("Matches")
                        .HasForeignKey("LevelID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Session", "Session")
                        .WithMany("Matches")
                        .HasForeignKey("SessionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "User")
                        .WithMany("Matches")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Level");

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Session", b =>
                {
                    b.HasOne("Level", "Level")
                        .WithMany("Sessions")
                        .HasForeignKey("LevelID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Level");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Level", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Session", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
