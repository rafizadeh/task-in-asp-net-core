using System.Collections.Generic;

namespace Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Cloth> Clothes { get; set; }
    }
}