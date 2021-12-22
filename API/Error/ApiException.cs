namespace API.Error
{
    public class ApiException 
    {
    public ApiException(int statusCode, string message = null, string detail = null)
    {
        this.message = message;
        this.detail = detail;
        StatusCode = statusCode;
    }

    public string message { get; set; }
        public string detail { get; set; }
        public int StatusCode { get; set; }

                
    }
}