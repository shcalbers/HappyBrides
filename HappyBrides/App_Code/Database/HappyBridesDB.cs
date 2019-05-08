using HappyBrides.Random;
using System;
using System.Diagnostics;
using Connection = WebMatrix.Data.Database;

namespace HappyBrides.Database
{

    /// <summary>
    /// 
    /// </summary>
    public static class HappyBridesDB
    {

	public const string DATABASE_NAME = "HappyBrides";

	/* CREATE */

	/// <summary>
	/// 
	/// </summary>
	/// <param name="email"></param>
	/// <param name="password"></param>
	/// <param name="name"></param>
	/// <param name="partnerName"></param>
	/// <returns></returns>
	public static CoupleHandle CreateCouple(string email, string password, string name, string partnerName)
	{
	    //Create a new account for this couple.
	    AccountHandle account = CreateAccount(email, password);
	    bool accountAlreadyExisted = (account == null);

	    if (accountAlreadyExisted)
		throw new InvalidOperationException("A Couple with an account using the provided email address does already exist!");

	    //Create a new wishlist for this couple.
	    WishlistHandle wishlist = CreateWishlist();

	    const string CREATE_COUPLE_STATEMENT = "INSERT INTO Couple (name, partner_name, account_id, wishlist_id) VALUES (@0, @1, @2, @3)";

	    //Create the actual couple.
	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(CREATE_COUPLE_STATEMENT, name, partnerName, account.id, wishlist.id);

		return new CoupleHandle((long)connection.GetLastInsertId());
	    }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="email"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	private static AccountHandle CreateAccount(string email, string password)
	{
	    const string EXISTS_ACCOUNT_STATEMENT = "SELECT 1 FROM Account WHERE email = @0";
	    const string CREATE_ACCOUNT_STATEMENT = "INSERT INTO Account (email, password) VALUES (@0, @1)";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		bool accountExists = (connection.QueryValue(EXISTS_ACCOUNT_STATEMENT, email) != null);

		if (accountExists)
		{
		    return null;
		}

		int rowsInserted = connection.Execute(CREATE_ACCOUNT_STATEMENT, email, password);
		bool accountWasCreated = (rowsInserted != 0);

		if (accountWasCreated)
		{
		    return new AccountHandle((long)connection.GetLastInsertId());
		}
		else
		{
		    return null;
		}
	    }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	private static WishlistHandle CreateWishlist()
	{
	    const string CREATE_WISHLIST_STATEMENT = "INSERT INTO Wishlist (guestcode) VALUES (@0)";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		//Define a maximum amount of iterations to prevent blocking the system.
		const int MAX_ITERATIONS = ushort.MaxValue;
		int CURRENT_ITERATION = 0;

		//Continue attempting to create a new wishlist with a unique guest code till one was made.
		bool wishlistNotCreated = true;

		while (wishlistNotCreated)
		{
		    if (CURRENT_ITERATION++ == MAX_ITERATIONS)
			throw new TimeoutException("Attempt to create a new wishlist took too long!");

		    string guestCode = CodeGenerator.GenerateCode();

		    int rowsInserted = connection.Execute(CREATE_WISHLIST_STATEMENT, guestCode);
		    wishlistNotCreated = (rowsInserted == 0);
		}

		return new WishlistHandle((long)connection.GetLastInsertId());
	    }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="wishlist"></param>
	/// <param name="name"></param>
	/// <param name="description"></param>
	/// <returns></returns>
	public static GiftHandle CreateGift(WishlistHandle wishlist, string name, string description)
	{
	    const string RETRIEVE_MAX_GIFTPRIORITY_STATEMENT = "SELECT TOP 1 priority FROM Gift WHERE wishlist_id = @0 ORDER BY priority DESC";
	    const string CREATE_GIFT_STATEMENT = "INSERT INTO Gift (wishlist_id, priority, name, description) VALUES (@0, @1, @2, @3)";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		int max_priority = (connection.QueryValue(RETRIEVE_MAX_GIFTPRIORITY_STATEMENT, wishlist.id) ?? -1);

		connection.Execute(CREATE_GIFT_STATEMENT, wishlist.id, (max_priority + 1), name, description);

		return new GiftHandle((long)connection.GetLastInsertId());
	    }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="gift"></param>
	/// <param name="sender"></param>
	/// <param name="content"></param>
	/// <returns></returns>
	public static CommentHandle CreateComment(GiftHandle gift, string sender, string content)
	{
	    throw new NotImplementedException();
	}

	/* RETRIEVE */

	/// <summary>
	/// Retrieves a couple from the database.
	/// </summary>
	/// <param name="email">The email of the account belonging to the couple.</param>
	/// <param name="password">The password of the account belonging to the couple.</param>
	/// <returns>A handle to the couple if one was found, null otherwise.</returns>
	public static CoupleHandle RetrieveCouple(string email, string password)
	{
	    const string RETRIEVE_ACCOUNTID_STATEMENT = "SELECT id FROM Account WHERE email = @0 AND password = @1";
	    const string RETRIEVE_COUPLEID_STATEMENT = "SELECT id FROM Couple WHERE account_id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		long? accountId = connection.QueryValue(RETRIEVE_ACCOUNTID_STATEMENT, email, password);
		long? coupleId = connection.QueryValue(RETRIEVE_COUPLEID_STATEMENT, accountId);

		if (coupleId != null)
		{
		    return new CoupleHandle(coupleId.Value);
		}
		else
		{
		    return null;
		}
	    }
	}

	/// <summary>
	/// Retrieves a wishlist from the database.
	/// </summary>
	/// <param name="guestCode">The guest code of the wishlist to look-up.</param>
	/// <returns>A handle to the wishlist if one was found, null otherwise.</returns>
	public static WishlistHandle RetrieveWishlist(string guestCode)
	{
	    const string RETRIEVE_WISHLISTID_STATEMENT = "SELECT id FROM Wishlist WHERE guestcode = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		long? wishlistId = connection.QueryValue(RETRIEVE_WISHLISTID_STATEMENT, guestCode);

		if (wishlistId != null)
		{
		    return new WishlistHandle(wishlistId.Value);
		}
		else
		{
		    return null;
		}
	    }
	}

	/* DELETE */

	public static void DeleteGift(GiftHandle gift)
	{
	    const string LEFT_SHIFT_PRIORITIES_STATEMENT = "UPDATE Gift SET priority = (priority - 1) WHERE wishlist_id = @0 AND priority > @1";
	    const string DELETE_GIFT_STATEMENT = "DELETE FROM Gift WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(LEFT_SHIFT_PRIORITIES_STATEMENT, gift.GetOwningWishlist().id, gift.GetPriority());
		connection.Execute(DELETE_GIFT_STATEMENT, gift.id);
	    }
	}
	
    }

}