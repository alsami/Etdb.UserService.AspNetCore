using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EntertainmentDatabase.REST.API.DatabaseContext;

namespace EntertainmentDatabase.REST.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170313090522_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EntertainmentDatabase.REST.Domain.Entities.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("SqlServer:ComputedColumnSql", "newid()");

                    b.HasKey("Id");

                    b.ToTable("Movie");
                });
        }
    }
}
