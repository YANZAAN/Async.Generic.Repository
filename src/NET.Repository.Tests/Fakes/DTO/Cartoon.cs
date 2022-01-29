using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NET.Repository.Tests.Fakes.DTO
{
    public class Cartoon
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id { get; set; }
        public string Name { get; set; }

        public IList<Episode> Episodes { get; set; }
    }

    public class CartoonComparer : IEqualityComparer<Cartoon>
    {
        public bool Equals(Cartoon x, Cartoon y) => x.Id == y.Id && x.Name == y.Name;

        public int GetHashCode([DisallowNull] Cartoon obj) => obj.GetHashCode();
    }
}