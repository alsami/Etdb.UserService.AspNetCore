using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    internal class MovieActorsConfiguration : EntityMappingConfiguration<MovieActors>
    {
        public MovieActorsConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<MovieActors> builder)
        {
            builder.HasIndex(actorMovie => new {actorMovie.ActorId, actorMovie.MovieId})
                .IsUnique();

            builder
                .HasOne(actorMovie => actorMovie.Actor)
                .WithMany(actor => actor.ActorMovies)
                .HasForeignKey(actorMovie => actorMovie.ActorId);

            builder
                .HasOne(actorMovie => actorMovie.Movie)
                .WithMany(movie => movie.ActorMovies)
                .HasForeignKey(actorMovie => actorMovie.MovieId);
        }
    }
}
