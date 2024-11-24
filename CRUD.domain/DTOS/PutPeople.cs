using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.domain.DTOS
{
	public class PutPeople:PostPeople
	{
        public int id { get; set; }
    }
}
