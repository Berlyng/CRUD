using CRUD.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUD.domain.Data
{
	public class CRUDdbcontext : DbContext
	{
		public CRUDdbcontext(DbContextOptions<CRUDdbcontext> options) : base(options)
		{

		}

        public virtual DbSet<People> People { get; set; }
    }
}
