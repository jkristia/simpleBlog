select * from dbo.posts

insert into dbo.posts (user_id, title, slug, created_at, content) 
	values (4, 'this is a test', 'this is the slug', 0, 'this is the content')

update dbo.posts set created_at = GETDATE()