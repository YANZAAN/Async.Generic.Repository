using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NET.Repository.Tests.Fakes.DTO
{
    public class Episode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id { get; set; }
        public short? CartoonId { get; set; }
        public string Name { get; set; }

        public Cartoon RelatedCartoon { get; set; }
    }

    public class EpisodeComparer : IEqualityComparer<Episode>
    {
        public bool Equals(Episode x, Episode y) => x.Id == y.Id && x.Name == y.Name;

        public int GetHashCode([DisallowNull] Episode obj) => obj.GetHashCode();
    }
}