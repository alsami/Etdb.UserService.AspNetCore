using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    internal class MovieActorsConfiguration : EntityMappingConfiguration<EntertainmentDatabase.REST.API.Domain.Entities.MovieActors>
    {
        public MovieActorsConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<EntertainmentDatabase.REST.API.Domain.Entities.MovieActors> builder)
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
