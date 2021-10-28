namespace Domain.Entities
{
    public class Cloth
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }

        public virtual Shelf Shelf { get; set; }
    }
}