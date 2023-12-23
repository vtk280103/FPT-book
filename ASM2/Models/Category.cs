using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM2.Models
{
	public class Category
	{

		[Required]
        public string Id { get; set; }

        [Required]
		public string KindOfBooks { get; set; }

		
	}
}
