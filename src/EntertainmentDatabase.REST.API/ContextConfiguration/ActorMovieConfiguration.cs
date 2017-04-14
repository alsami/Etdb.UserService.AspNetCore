using EntertainmentDatabase.REST.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.ContextConfiguration
{
    public class ActorMovieConfiguration : EntityMappingConfiguration<ActorMovies>
    {
        protected override void Configure(EntityTypeBuilder<ActorMovies> builder)
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
