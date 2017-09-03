using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.API.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.API.DataAccess
{
    public class DataSeeder
    {
        private readonly EntertainmentDatabaseContext entertainmentDatabaseContext;

        public DataSeeder(EntertainmentDatabaseContext entertainmentDatabaseContext)
        {
            this.entertainmentDatabaseContext = entertainmentDatabaseContext;
        }

        public async Task SeedDatabase()
        {
            if (!this.entertainmentDatabaseContext.Movie.Any())
            {
                this.entertainmentDatabaseContext.Movie.AddRange(new[]
                {
                    new Movie
                    {
                        ConsumerMediaType = ConsumerMediaType.Movie,
                        Title = "Alien",
                        Description = "In deep space, the crew of the commercial starship Nostromo is awakened from their cryo-sleep capsules halfway through their journey home to investigate a distress call from an alien vessel. The terror begins when the crew encounters a nest of eggs inside the alien ship. An organism from inside an egg leaps out and attaches itself to one of the crew, causing him to fall into a coma.",
                        ReleasedOn = new DateTime(1979, 5, 25)
                    },
                    new Movie
                    {
                        ConsumerMediaType = ConsumerMediaType.Movie,
                        Title = "Alien 3",
                        Description = "Ellen Ripley (Sigourney Weaver) is the only survivor when she crash lands on Fiorina 161, a bleak wasteland inhabited by former inmates of the planet's maximum security prison. Once again, Ripley must face skepticism and the alien as it hunts down the prisoners and guards. Without weapons or modern technology of any kind, Ripley leads the men into battle against the terrifying creature.",
                        ReleasedOn = new DateTime(1992, 5, 22)
                    },
                    new Movie
                    {
                        ConsumerMediaType = ConsumerMediaType.Movie,
                        Title = "Alien Covenant",
                        Description = "Bound for a remote planet on the far side of the galaxy, members (Katherine Waterston, Billy Crudup) of the colony ship Covenant discover what they think to be an uncharted paradise. While there, they meet David (Michael Fassbender), the synthetic survivor of the doomed Prometheus expedition. The mysterious world soon turns dark and dangerous when a hostile alien life-form forces the crew into a deadly fight for survival"
                    },
                    new Movie
                    {
                        ConsumerMediaType = ConsumerMediaType.Movie,
                        Title = "Pulp Fiction",
                        Description = "Vincent Vega (John Travolta) and Jules Winnfield (Samuel L. Jackson) are hitmen with a penchant for philosophical discussions. In this ultra-hip, multi-strand crime movie, their storyline is interwoven with those of their boss, gangster Marsellus Wallace (Ving Rhames) ; his actress wife, Mia (Uma Thurman) ; struggling boxer Butch Coolidge (Bruce Willis) ; master fixer Winston Wolfe (Harvey Keitel) and a nervous pair of armed robbers, \"Pumpkin\" (Tim Roth) and \"Honey Bunny\" (Amanda Plummer)."
                    }
                });

                await this.entertainmentDatabaseContext.SaveChangesAsync();
            }
        }
    }
}
