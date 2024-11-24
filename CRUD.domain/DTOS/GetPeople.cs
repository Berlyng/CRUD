using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.domain.DTOS
{
	public class GetPeople
	{
        public int  id { get; set; }
		public string name { get; set; }
		public int age { get; set; }
    }
}
