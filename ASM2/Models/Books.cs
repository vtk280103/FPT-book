using System.ComponentModel.DataAnnotations;

namespace ASM2.Models
{
	public class Books
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string NameOfBook { get; set; }
		[Required]
		public string Details { get; set; }
		[Required]
		public string NameAuthor { get; set; }
		[Required]
		public DateTime PublicDay { get; set; }
		[Required]
		public decimal Price { get; set; }
		[Required] 
		public string Picture { get; set; }
		[Required]
		public string Category { get; set; } 
	}
}
