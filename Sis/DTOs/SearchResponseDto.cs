namespace Sis.DTOs
{
    public class SearchResponseDto
    {
        public int page { get; set; }
        public int pageSize{ get; set; }
         public int totalCount { get; set; }
        public int totalPages { get; set; }

        public dynamic? items { get; set; }

    }
}
