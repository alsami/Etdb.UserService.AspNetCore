using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.WebService.Data.Configuration
{
    internal class MovieActorConfiguration : EntityMappingConfiguration<MovieActor>
    {
        public MovieActorConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            base.Configure(builder);

            //builder.ToTable($"{nameof(MovieActor)}s");

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
