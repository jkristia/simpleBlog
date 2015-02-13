using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SimpleBlog.Migrations
{
	[Migration(2)]
	public class _002_Posts : Migration
	{
		public override void Up()
		{
			Create.Table("posts")
				.WithColumn("id").AsInt32().Identity().PrimaryKey()
				.WithColumn("user_id").AsInt32().ForeignKey("users", "id")
				.WithColumn("title").AsString(128)
				.WithColumn("slug").AsString(128)
				.WithColumn("created_at").AsDateTime()
				.WithColumn("updated_at").AsDateTime().Nullable()
				.WithColumn("deleted_at").AsDateTime().Nullable();

			Create.Table("tags")
				.WithColumn("id").AsInt32().Identity().PrimaryKey()
				.WithColumn("slug").AsString(128)
				.WithColumn("name").AsString(128);

			Create.Table("posts_tags")
				.WithColumn("posts_id").AsInt32().ForeignKey("posts", "id").OnDelete(Rule.Cascade)
				.WithColumn("tag_id").AsInt32().ForeignKey("tags", "id").OnDelete(Rule.Cascade);
		}

		public override void Down()
		{
			Delete.Table("posts_tags");
			Delete.Table("tags");
			Delete.Table("posts");
		}
	}
}