using static HappyBrides.Database.HappyBridesDB;

using Connection = WebMatrix.Data.Database;

namespace HappyBrides.Database
{

    public sealed class CommentHandle
    {

	public readonly long id;

	public CommentHandle(long id)
	{
	    this.id = id;
	}

	public string GetSender()
	{
	    const string RETRIEVE_SENDER_STATEMENT = "SELECT sender FROM Comment WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_SENDER_STATEMENT, id);
	    }
	}

	public string GetContent()
	{
	    const string RETRIEVE_CONTENT_STATEMENT = "SELECT content FROM Comment WHERE id = @0";

	    using (var connection = Connection.Open(DATABASE_NAME))
	    {
		return connection.QueryValue(RETRIEVE_CONTENT_STATEMENT, id);
	    }
	}

    }

}
