using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EntertainmentDatabase.REST.API.DataAccess;
using EntertainmentDatabase.REST.API.Domain.Enums;

namespace EntertainmentDatabase.REST.API.DataAccess.Migrations
{
    [DbContext(typeof(EntertainmentDatabaseContext))]
    [Migration("20170430111732_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EntertainmentDatabase.REST.API.Domain.Entities.Actor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Actor");
                });

            modelBuilder.Entity("EntertainmentDatabase.REST.API.Domain.Entities.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<int>("ConsumerMediaType");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("ReleasedOn");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("EntertainmentDatabase.REST.API.Domain.Entities.MovieActors", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<Guid>("ActorId");

                    b.Property<Guid>("MovieId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("ActorId", "MovieId")
                        .IsUnique();

                    b.ToTable("MovieActors");
                });

            modelBuilder.Entity("EntertainmentDatabase.REST.API.Domain.Entities.MovieFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<byte[]>("File");

                    b.Property<bool>("IsCover");

                    b.Property<int>("MediaFileType");

                    b.Property<Guid>("MovieId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieFile");
                });

            modelBuilder.Entity("EntertainmentDatabase.REST.API.Domain.Entities.MovieActors", b =>
                {
                    b.HasOne("EntertainmentDatabase.REST.API.Domain.Entities.Actor", "Actor")
                        .WithMany("ActorMovies")
                        .HasForeignKey("ActorId");

                    b.HasOne("EntertainmentDatabase.REST.API.Domain.Entities.Movie", "Movie")
                        .WithMany("ActorMovies")
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("EntertainmentDatabase.REST.API.Domain.Entities.MovieFile", b =>
                {
                    b.HasOne("EntertainmentDatabase.REST.API.Domain.Entities.Movie", "Movie")
                        .WithMany("MovieFiles")
                        .HasForeignKey("MovieId");
                });
        }
    }
}
