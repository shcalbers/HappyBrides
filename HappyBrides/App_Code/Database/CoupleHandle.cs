using System;
using static HappyBrides.Database.HappyBridesDB;

using Connection = WebMatrix.Data.Database;

namespace HappyBrides.Database
{

    public sealed class CoupleHandle
    {

	public readonly long id;
	
	public CoupleHandle(long id)
	{
	    this.id = id;
	}

	public string GetName()
	{
	    const string RETRIEVE_NAME_STATEMENT = "SELECT name FROM Couple WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_NAME_STATEMENT, id);
	    }
	}

	public string GetPartnerName()
	{
	    const string RETRIEVE_PARTNERNAME_STATEMENT = "SELECT partner_name FROM Couple WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_PARTNERNAME_STATEMENT, id);
	    }
	}

	public AccountHandle GetAccount()
	{
	    const string RETRIEVE_ACCOUNT_STATEMENT = "SELECT account_id FROM Couple WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		var account_id = connection.QueryValue(RETRIEVE_ACCOUNT_STATEMENT, id) as long?;

		return new AccountHandle(account_id ?? -1);
	    }
	}

	public WishlistHandle GetWishlist()
	{
	    const string RETRIEVE_WISHLIST_STATEMENT = "SELECT wishlist_id FROM Couple WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		var wishlist_id = connection.QueryValue(RETRIEVE_WISHLIST_STATEMENT, id) as long?;

		return new WishlistHandle(wishlist_id ?? -1);
	    }
	}

	public void SetName(string new_name)
	{
	    const string UPDATE_NAME_STATEMENT = "UPDATE Couple SET name = @0 WHERE id = @1";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(UPDATE_NAME_STATEMENT, new_name, id);
	    }
	}

	public void SetPartnerName(string new_partnerName)
	{
	    const string UPDATE_PARTNERNAME_STATEMENT = "UPDATE Couple SET partner_name = @0 WHERE id = @1";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(UPDATE_PARTNERNAME_STATEMENT, new_partnerName, id);
	    }
	}

    }

}
