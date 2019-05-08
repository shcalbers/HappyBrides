using HappyBrides.Database;
using System;
using System.Web;

namespace HappyBrides.Authentication
{

    /// <summary>
    /// A class representing a user, identified as a Couple, currently browsing the website.
    /// </summary>
    public sealed class CoupleAgent : UserAgent
    {

	private const string SESSION_COUPLE_KEY = "CoupleAgent_Couple";

	private CoupleHandle backing_field_couple;

	public string Name
	{
	    get
	    {
		return Couple.GetName();
	    }

	    set
	    {
		Couple.SetName(value);
	    }
	}

	public string PartnerName
	{
	    get
	    {
		return Couple.GetPartnerName();
	    }

	    set
	    {
		Couple.SetPartnerName(value);
	    }
	}

	public AccountHandle Account
	{
	    get
	    {
		return Couple.GetAccount();
	    }
	}

	public WishlistHandle Wishlist
	{
	    get
	    {
		return Couple.GetWishlist();
	    }
	}

	private CoupleHandle Couple
	{
	    get
	    {
		if (backing_field_couple == null)
		{
		    backing_field_couple = m_session[SESSION_COUPLE_KEY] as CoupleHandle;
		}

		return backing_field_couple;
	    }

	    set
	    {
		backing_field_couple = value;

		m_session[SESSION_COUPLE_KEY] = backing_field_couple;
	    }
	}

	/// <summary>
	/// Constructs a new <see cref="CoupleAgent"/>, representing a couple.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when the user does not have the proper identity to be represented by <see cref="CoupleAgent"/></exception>
	/// <param name="session">The <see cref="HttpSessionStateBase"/> associated with the couple.</param>
	public CoupleAgent(HttpSessionStateBase session)
	    : base(session)
	{
	    if (Identity != Identity.Couple && Identity != Identity.Unknown)
		throw new InvalidOperationException("The current user must have been identified as a Couple or must have an unknown identity to be represented by a CoupleAgent!");
	}

	/// <summary>
	/// Attempts to register and identify a user as a Couple.
	/// </summary>
	/// <param name="email">The email supplied by the user.</param>
	/// <param name="name">The name supplied by the user.</param>
	/// <param name="partnerName">The partner's supplied by the user.</param>
	/// <param name="password">The password supplied by the user.</param>
	public void Register(string email, string password, string name, string partnerName)
	{
	    if (Identity != Identity.Unknown)
		throw new InvalidOperationException("Attempted to register while the user was already logged in!");

	    Couple = HappyBridesDB.CreateCouple(email, password, name, partnerName);

	    if (Couple != null)
	    {
		Login(Identity.Couple);
	    }
	}

	/// <summary>
	/// Attempts to identify a user as a Couple using the given credentials. 
	/// </summary>
	/// <param name="email">The email address supplied by the user.</param>
	/// <param name="password">The password supplied by the user.</param>
	public void Login(string email, string password)
	{
	    if (Identity != Identity.Unknown)
		throw new InvalidOperationException("Attempted to login while the user was already logged in!");

	    Couple = HappyBridesDB.RetrieveCouple(email, password);

	    if (Couple != null)
	    {
		Login(Identity.Couple);
	    }
	}

	public override void Logout()
	{
	    if (Identity != Identity.Couple)
		throw new InvalidOperationException("Attempted to logout while the user was not logged in as a Couple!");

	    base.Logout();

	    Couple = null;
	}

    }

}
