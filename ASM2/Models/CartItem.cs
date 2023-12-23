using System.ComponentModel.DataAnnotations;

namespace ASM2.Models
{
    public class CartItem
    {
        [Key] public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        
        public int Quantity { get; set; }

        public float Total { get; set; }    

        public virtual  Books Books { get; set; }
    }
}
