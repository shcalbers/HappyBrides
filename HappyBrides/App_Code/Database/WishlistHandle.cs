using System.Collections.Generic;
using static HappyBrides.Database.HappyBridesDB;

using Connection = WebMatrix.Data.Database;

namespace HappyBrides.Database
{

    public sealed class WishlistHandle
    {

	public readonly long id;

	public WishlistHandle(long id)
	{
	    this.id = id;
	}

	public string GetGuestCode()
	{
	    const string RETRIEVE_GUESTCODE_STATEMENT = "SELECT guestcode FROM Wishlist WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_GUESTCODE_STATEMENT, id);
	    }
	}

	public List<GiftHandle> GetGifts()
	{
	    const string RETRIEVE_GIFTS_STATEMENT = "SELECT id FROM Gift WHERE wishlist_id = @0 ORDER BY priority ASC";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		var results = connection.Query(RETRIEVE_GIFTS_STATEMENT, id);

		List<GiftHandle> gifts = new List<GiftHandle>();

		foreach (var result in results)
		{
		    gifts.Add(new GiftHandle(result.id));
		}

		return gifts;
	    }
	}

	public List<GiftHandle> GetUnreservedGifts()
	{
	    const string RETRIEVE_GIFTS_STATEMENT = "SELECT id FROM Gift WHERE wishlist_id = @0 AND reserved = 0 ORDER BY priority ASC";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		var results = connection.Query(RETRIEVE_GIFTS_STATEMENT, id);

		List<GiftHandle> gifts = new List<GiftHandle>();

		foreach (var result in results)
		{
		    gifts.Add(new GiftHandle(result.id));
		}

		return gifts;
	    }
	}

    }

}