namespace Client.Utilities;

public class ResponseHandlerClient<TEntity>
{
    public int Code { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }

    public TEntity? Data { get; set; }
}
