namespace CatalogService.Models
{
    public class Category : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}
