using HappyBrides.Database;
using System;
using System.Web;

namespace HappyBrides.Authentication
{

    /// <summary>
    /// A class representing a user, identified as a Guest, currently browsing the website.
    /// </summary>
    public sealed class GuestAgent : UserAgent
    {

        private const string SESSION_NAME_KEY = "GuestAgent_Name";
        private const string SESSION_WISHLIST_KEY = "GuestAgent_Wishlist";

        private string backing_field_name;
        private WishlistHandle backing_field_wishlist;

        public string Name
        {
            get
            {
                if (backing_field_name == null)
                {
                    backing_field_name = m_session[SESSION_NAME_KEY] as string;
                }

                return backing_field_name;
            }

            private set
            {
                backing_field_name = value;

                m_session[SESSION_NAME_KEY] = backing_field_name;
            }
        }

        public WishlistHandle Wishlist
        {
            get
            {
                if (backing_field_wishlist == null)
                {
                    backing_field_wishlist = m_session[SESSION_WISHLIST_KEY] as WishlistHandle;
                }

                return backing_field_wishlist;
            }

            private set
            {
                backing_field_wishlist = value;

                m_session[SESSION_WISHLIST_KEY] = backing_field_wishlist;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GuestAgent"/>, representing a guest.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the user does not have the proper identity to be represented by <see cref="GuestAgent"/></exception>
        /// <param name="session">The <see cref="HttpSessionStateBase"/> associated with the guest.</param>
        public GuestAgent(HttpSessionStateBase session)
            : base(session)
        {
            if (Identity != Identity.Guest && Identity != Identity.Unknown)
                throw new InvalidOperationException("The current user must have been identified as a Guest or must have an unknown identity to be represented by a GuestAgent!");
        }

        /// <summary>
        /// Attempts to identify a user as a Guest using the given name and guestcode.
        /// </summary>
        /// <param name="name">The name to assign to the user.</param>
        /// <param name="guestCode">The guest code providing the user access to the website.</param>
        public void Login(string name, string guestCode)
        {
            if (Identity != Identity.Unknown)
                throw new InvalidOperationException("Attempted to login while the user was already logged in!");

            Wishlist = HappyBridesDB.RetrieveWishlist(guestCode);

            if (Wishlist == null)
                return;

            Name = name;

            Login(Identity.Guest);
        }

        public override void Logout()
        {
            if (Identity != Identity.Guest)
                throw new InvalidOperationException("Attempted to logout while the user was not logged in as a Guest!");

            base.Logout();

            Name = null;
            Wishlist = null;
        }

    }

}
