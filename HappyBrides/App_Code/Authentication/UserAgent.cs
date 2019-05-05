using System;
using System.Web;

namespace HappyBrides.Authentication
{

    /// <summary>
    /// A class representing a person currently browsing the website.
    /// </summary>
    public class UserAgent
    {

	private const string SESSION_IDENTITY_KEY = "UserAgent_Identity";

	/// <summary>
	/// The <see cref="HttpSessionStateBase"/> associated with the user.
	/// </summary>
	protected readonly HttpSessionStateBase m_session;

	/// <summary>
	/// The User's current <see cref="HappyBrides.Authentication.Identity"/>
	/// </summary>
	public Identity Identity
	{
	    get
	    {
		return ((m_session[SESSION_IDENTITY_KEY] as Identity?) ?? Identity.Unknown);
	    }

	    private set
	    {
		m_session[SESSION_IDENTITY_KEY] = value;
	    }
	}

	/// <summary>
	/// Constructs a new <see cref="UserAgent"/>, representing a user.
	/// </summary>
	/// <param name="session">The <see cref="HttpSessionStateBase"/> associated with the user.</param>
	public UserAgent(HttpSessionStateBase session)
	{
	    m_session = session;
	}

	/// <summary>
	/// Assigns the given <see cref="HappyBrides.Authentication.Identity"/> to the user.
	/// </summary>
	/// <exception cref="System.ArgumentException">Thrown when the given identity equals <see cref="Identity.Unknown"/></exception>
	/// <exception cref="System.InvalidOperationException">Thrown when the user already has an <see cref="HappyBrides.Authentication.Identity"/></exception>
	/// <param name="identity">The identity to assign to the user.</param>
	protected void Login(Identity identity)
	{
	    if (identity == Identity.Unknown)
		throw new ArgumentException("A User cannot login without a known identity!");

	    if (this.Identity != Identity.Unknown)
		throw new InvalidOperationException("A User cannot login when the user is already logged in!");

	    Identity = identity;
	}

	/// <summary>
	/// Resets the user's identity to <see cref="Identity.Unknown"/>
	/// </summary>
	public virtual void Logout()
	{
	    Identity = Identity.Unknown;
	}

    }

}