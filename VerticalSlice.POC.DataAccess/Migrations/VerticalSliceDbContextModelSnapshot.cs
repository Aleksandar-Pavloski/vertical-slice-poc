﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VerticalSlice.POC.DataAccess;

namespace VerticalSlice.POC.DataAccess.Migrations
{
    [DbContext(typeof(VerticalSliceDbContext))]
    partial class VerticalSliceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VerticalSlice.POC.DataAccess.Entities.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("VerticalSlice.POC.DataAccess.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.Property<Guid?>("SubscriptionId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VerticalSlice.POC.DataAccess.Entities.User", b =>
                {
                    b.HasOne("VerticalSlice.POC.DataAccess.Entities.Subscription")
                        .WithMany("Users")
                        .HasForeignKey("SubscriptionId");
                });
#pragma warning restore 612, 618
        }
    }
}
