namespace int3306.Repository.Models
{
    public class ProductSearchModel
    {
        public string? Query { get; set; } = null;
        
        public int? ProductType { get; set; } = null;
        public List<int>? ProductTags { get; set; } = new();

        public int? MinPrice { get; set; } = null;
        public int? MaxPrice { get; set; } = null;

        public int? GrowingSeasonMin { get; set; } = null;
        public int? GrowingSeasonMax { get; set; } = null;
    }
}