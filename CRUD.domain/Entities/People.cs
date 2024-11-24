using System.ComponentModel.DataAnnotations;

namespace CRUD.domain.Entities
{
	public class People
	{
		[Key]
        public int id { get; set; }
		[Required]
        
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
