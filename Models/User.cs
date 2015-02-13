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
		const int WorkFactor = 13;
		// NHibernate must have all members and properties virtual <-- !! very important !!
		public virtual int Id { get; set; }
		public virtual string Username { get; set; }
		public virtual string Email { get; set; }
		public virtual string PasswordHash { get; set; }
		public virtual void SetPassword(string password)
		{
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
		}
		public virtual bool CheckPassword(string password)
		{
			return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
		}
		public virtual IList<Role> Roles { get; set; }

		public User()
		{
			Roles = new List<Role>();
		}
		static public void FakeHash()
		{
			// to prevent timing attack, if user doesnt exist, calculate a fake hash,
			// this way the time it takes to check existing user and non-existing user is the same
			// BCrypt is slow ~ 500ms
			BCrypt.Net.BCrypt.HashPassword("", WorkFactor);
		}
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

			// this is telling nHibernate the relation between 'users' and 'roles' through the pivot table 'role_users'
			Bag( x => x.Roles, x => 
			{
				x.Table("role_users");
				x.Key(k => k.Column("user_id"));
			}, x => x.ManyToMany(k => k.Column("role_id")));
		}
	}
}