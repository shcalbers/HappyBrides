using static HappyBrides.Database.HappyBridesDB;

using Connection = WebMatrix.Data.Database;

namespace HappyBrides.Database
{

    public sealed class AccountHandle
    {

	public readonly long id;

	public AccountHandle(long id)
	{
	    this.id = id;
	}

	public string GetEmail()
	{
	    const string RETRIEVE_EMAIL_STATEMENT = "SELECT email FROM Account WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_EMAIL_STATEMENT, id);
	    }
	}

	public string GetPassword()
	{
	    const string RETRIEVE_PASSWORD_STATEMENT = "SELECT password FROM Account WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_PASSWORD_STATEMENT, id);
	    }
	}

	public void SetEmail(string new_email)
	{
	    const string UPDATE_EMAIL_STATEMENT = "UPDATE Account SET email = @0 WHERE id = @1";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(UPDATE_EMAIL_STATEMENT, new_email, id);
	    }
	}

	public void SetPassword(string new_password)
	{
	    const string UPDATE_PASSWORD_STATEMENT = "UPDATE Account SET password = @0 WHERE id = @1";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		connection.Execute(UPDATE_PASSWORD_STATEMENT, new_password, id);
	    }
	}

    }

}