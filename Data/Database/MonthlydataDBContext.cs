﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MetaData;

namespace Data.Database
{
    public partial class MonthlydataDBContext : DbContext
    {
        public MonthlydataDBContext()
        {
        }

        public MonthlydataDBContext(DbContextOptions<MonthlydataDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MonthlyDataLogging> MonthlyDataLogging { get; set; }
        public virtual DbSet<TrafficDataDaily> TrafficDataDaily { get; set; }
        public virtual DbSet<TrafficDataMonthly> TrafficDataMonthly { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonthlyDataLogging>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__MonthlyD__9A8D5625FC560FDE");

                entity.ToTable("MonthlyData_Logging");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("Transaction_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ApiMethod)
                    .HasColumnName("API_Method")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ApplicationName)
                    .HasColumnName("Application_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Debug1)
                    .HasColumnName("Debug_1")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Debug2)
                    .HasColumnName("Debug_2")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorMessage)
                    .HasColumnName("Error_Message")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.RequestMessage)
                    .HasColumnName("Request_Message")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.RequestingEndDt)
                    .HasColumnName("Requesting_End_DT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RequestingStartDt)
                    .HasColumnName("Requesting_Start_DT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RequestingSystem)
                    .HasColumnName("Requesting_System")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseMessage)
                    .HasColumnName("Response_Message")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnCode)
                    .HasColumnName("Return_Code")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrafficDataDaily>(entity =>
            {
                entity.HasKey(e => new { e.CalendarDay, e.CalendarMonthIndex, e.CalendarYear })
                    .HasName("PK_Data_Daily");

                entity.ToTable("Traffic_Data_Daily");

                entity.Property(e => e.CalendarDay).HasColumnName("Calendar_Day");

                entity.Property(e => e.CalendarMonthIndex).HasColumnName("Calendar_Month_Index");

                entity.Property(e => e.CalendarYear).HasColumnName("Calendar_Year");

                entity.Property(e => e.CalendarMonth)
                    .HasColumnName("Calendar_Month")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CalendarWeekDay)
                    .HasColumnName("Calendar_WeekDay")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DailyDataIn).HasColumnName("Daily_Data_In");

                entity.Property(e => e.DailyDataOut).HasColumnName("Daily_Data_Out");
            });

            modelBuilder.Entity<TrafficDataMonthly>(entity =>
            {
                entity.HasKey(e => new { e.CalendarMonthIndex, e.CalendarYear })
                    .HasName("PK_Data_Monthly");

                entity.ToTable("Traffic_Data_Monthly");

                entity.Property(e => e.CalendarMonthIndex).HasColumnName("Calendar_Month_Index");

                entity.Property(e => e.CalendarYear).HasColumnName("Calendar_Year");

                entity.Property(e => e.CalendarMonth)
                    .HasColumnName("Calendar_Month")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MonthlyDataIn).HasColumnName("Monthly_Data_In");

                entity.Property(e => e.MonthlyDataOut).HasColumnName("Monthly_Data_Out");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}