using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SimpleBlog.Migrations
{
	// this was an excample of never editing migration once it has been deployed sto production. Instead write a new migration toadd the changes
	[Migration(3)]
	public class _003_AddContentToPosts : Migration
	{
		public override void Up()
		{
			Create.Column("content").OnTable("posts").AsCustom("TEXT");
		}

		public override void Down()
		{
			Delete.Column("content").FromTable("posts");
		}
	}
}