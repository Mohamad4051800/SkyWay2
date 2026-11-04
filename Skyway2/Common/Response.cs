namespace Skyway2.Common;
public class Response<T>
{
    public int statusCode { set; get; }
    public String message { set; get; }
    public String title { set; get; }
    public MessageLevel messageLevel { set; get; }
    public String informationLink { set; get; }
    public String devMessage { set; get; }
    public String parameters { set; get; }
    public Response()
    {
    }
    public T payload { set; get; }


    public Response(T payload)
    {
        this.payload = payload;
    }

}
public enum MessageLevel { Admin = 1, Cif = 2, Tech = 3 }

