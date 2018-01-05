using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.API.Extensions.Tests
{

	public class UserModel
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public DateTime CreateDate { get; set; }

		/// <summary></summary>
		public virtual UserModel Clone()
		{
			return (UserModel)this.MemberwiseClone();
		}
	}

}
