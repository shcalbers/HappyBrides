using System.Collections.Generic;
using static HappyBrides.Database.HappyBridesDB;

using Connection = WebMatrix.Data.Database;

namespace HappyBrides.Database
{

    public sealed class GiftHandle
    {

	public readonly long id;

	public GiftHandle(long id)
	{
	    this.id = id;
	}

	public WishlistHandle GetOwningWishlist()
	{
	    const string RETRIEVE_WISHLIST_STATEMENT = "SELECT wishlist_id FROM Gift WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		var wishlist_id = connection.QueryValue(RETRIEVE_WISHLIST_STATEMENT, id) as long?;

		return new WishlistHandle(wishlist_id ?? -1);
	    }
	}

	public int GetPriority()
	{
	    const string RETRIEVE_PRIORITY_STATEMENT = "SELECT priority FROM Gift WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_PRIORITY_STATEMENT, id);
	    }
	}

	public string GetName()
	{
	    const string RETRIEVE_NAME_STATEMENT = "SELECT name FROM Gift WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_NAME_STATEMENT, id);
	    }
	}

	public string GetDescription()
	{
	    const string RETRIEVE_DESCRIPTION_STATEMENT = "SELECT description FROM Gift WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_DESCRIPTION_STATEMENT, id);
	    }
	}

	public string GetReservationStatus()
	{
	    const string RETRIEVE_RESERVATIONSTATUS_STATEMENT = "SELECT reserved FROM Gift WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_RESERVATIONSTATUS_STATEMENT, id);
	    }
	}

	public List<CommentHandle> GetComments()
	{
	    const string RETRIEVE_COMMENTS_STATEMENT = "SELECT id FROM Comment WHERE gift_id = @0 ORDER BY id ASC";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		var results = connection.Query(RETRIEVE_COMMENTS_STATEMENT, id);

		List<CommentHandle> comments = new List<CommentHandle>();

		foreach (var result in results)
		{
		    comments.Add(new CommentHandle(result.id));
		}

		return comments;
	    }
	}

	public void SetPriority(int new_priority)
	{
	    const string LEFT_SHIFT_PRIORITIES_STATEMENT = "UPDATE Gift SET priority = (priority - 1) WHERE wishlist_id = @0 AND (priority BETWEEN @1 AND @2)";
	    const string RIGHT_SHIFT_PRIORITIES_STATEMENT = "UPDATE Gift SET priority = (priority + 1) WHERE wishlist_id = @0 AND (priority BETWEEN @1 AND @2)";

	    const string UPDATE_PRIORITY_STATEMENT = "UPDATE Gift SET priority = @0 WHERE id = @1";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		int current_priority = GetPriority();

		if (new_priority > current_priority)
		{
		    connection.Execute(LEFT_SHIFT_PRIORITIES_STATEMENT, GetOwningWishlist().id, current_priority, new_priority);
		}
		else
		{
		    connection.Execute(RIGHT_SHIFT_PRIORITIES_STATEMENT, GetOwningWishlist().id, new_priority, current_priority);
		}

		connection.Execute(UPDATE_PRIORITY_STATEMENT, new_priority, id);
	    }
	}

	public void SetName(string new_name)
	{
	    const string UPDATE_NAME_STATEMENT = "UPDATE Gift SET name = @0 WHERE id = @1";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(UPDATE_NAME_STATEMENT, new_name, id);
	    }
	}

	public void SetDescription(string new_description)
	{
	    const string UPDATE_DESCRIPTION_STATEMENT = "UPDATE Gift SET description = @0 WHERE id = @1";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(UPDATE_DESCRIPTION_STATEMENT, new_description, id);
	    }
	}

	public void SetReservationStatus(bool is_reserved)
	{
	    const string UPDATE_RESERVATIONSTATUS_STATEMENT = "UPDATE Gift SET reserved = @0 WHERE id = @1";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(UPDATE_RESERVATIONSTATUS_STATEMENT, is_reserved, id);
	    }
	}
	
    }

}
