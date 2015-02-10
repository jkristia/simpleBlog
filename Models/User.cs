using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
	public class User
	{
		// NHibernate must have all members and properties virtual <-- !! very important !!
		public virtual int Id { get; set; }
		public virtual string Username { get; set; }
		public virtual string Email { get; set; }
		public virtual string PasswordHash { get; set; }
	}
	public class UserMap : ClassMapping<User>
	{
		public UserMap()
		{
			Table("users");
			Id(x => x.Id, x => x.Generator(Generators.Identity));	 // <-- id is unique and handled by the database
			Property(x => x.Username, x => x.NotNullable(true));
			Property(x => x.Email, x => x.NotNullable(true));
			Property(x => x.PasswordHash, x => 
				{
					x.Column("password_hash");
					x.NotNullable(true);
				});
		}
	}
}