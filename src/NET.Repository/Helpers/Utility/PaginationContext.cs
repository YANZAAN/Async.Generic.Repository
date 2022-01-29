using System.Text.Json.Serialization;

namespace GenericRepository.Helpers.Utility
{
    public class PaginationContext
    {
        const ushort _maxPageSize = 24;
        public ushort PageNumber { get; set; } = 1;
        protected ushort _pageSize = 10;
        public ushort PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
            }
        }

        [JsonIgnore]
        public static PaginationContext All { get; } = new PaginationContext() { _pageSize = ushort.MaxValue };

        [JsonIgnore]
        public static PaginationContext Single { get; } = new PaginationContext() { _pageSize = 1 };
    }
}