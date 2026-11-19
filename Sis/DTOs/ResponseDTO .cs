namespace Sis.DTOs
{
    public class ResponseDTO
    {
        public string? message { set; get; }
        public int code { set; get; } 
        public dynamic? data { set; get; }
        public bool isSuccess { set; get; }
    }
}
