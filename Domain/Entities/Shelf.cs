namespace Domain.Entities
{
    public class Shelf
    {
        public int Id { get; set; }
        public string ShelfNo { get; set; }

        public int? ClothId { get; set; }
        public virtual Cloth Cloth { get; set; }
    }
}