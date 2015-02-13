using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SimpleBlog.Models;

namespace SimpleBlog
{
	public static class Database
	{
		private const string SessionKey = "SimpleBlog.Database.SessionKey";
		static ISessionFactory m_sessionFactory;

		public static ISession Session
		{
			get
			{
				// we do want exception in case of an error, null session or empty session
				return (ISession)HttpContext.Current.Items[SessionKey];
			}
		}

		public static void Configure()
		{
			Configuration config = new Configuration();
			// configure connection string
			config.Configure(); // <-- looks in web.config

			// add our mapping
			ModelMapper mapper = new ModelMapper();
			mapper.AddMapping<UserMap>();
			mapper.AddMapping<RoleMap>();
			config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

			// create session factory
			m_sessionFactory = config.BuildSessionFactory();
		}
		public static void OpenSession()
		{
			HttpContext.Current.Items[SessionKey] = m_sessionFactory.OpenSession();
		}
		public static void CloseSession()
		{
			ISession session = HttpContext.Current.Items[SessionKey] as ISession;
			if (session != null)
				m_sessionFactory.Close();
			HttpContext.Current.Items.Remove(SessionKey);
		}
	}
}