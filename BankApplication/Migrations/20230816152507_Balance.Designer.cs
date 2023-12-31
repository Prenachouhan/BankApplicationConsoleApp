﻿// <auto-generated />
using System;
using BankApplication.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankApplication.Migrations
{
    [DbContext(typeof(BankDBContext))]
    [Migration("20230816152507_Balance")]
    partial class Balance
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankApplication.Entities.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("AccountInfo")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("accountInfo");

                    b.HasKey("AccountId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("BankApplication.Entities.Rules", b =>
                {
                    b.Property<int>("RuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RuleId"));

                    b.Property<double>("Interest")
                        .HasColumnType("float")
                        .HasColumnName("interest");

                    b.Property<DateTime>("RuleDate")
                        .HasColumnType("datetime")
                        .HasColumnName("ruleDate");

                    b.Property<string>("RuleName")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("ruleName");

                    b.HasKey("RuleId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("BankApplication.Entities.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<double>("Amount")
                        .HasColumnType("float")
                        .HasColumnName("amount");

                    b.Property<double?>("Balance")
                        .HasColumnType("float")
                        .HasColumnName("balance");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("remarks");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime")
                        .HasColumnName("transactionDate");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("transactionType");

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("BankApplication.Entities.Transaction", b =>
                {
                    b.HasOne("BankApplication.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("BankApplication.Entities.Account", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
