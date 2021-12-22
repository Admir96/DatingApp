namespace API.DTOs
{
    public class PhotoDTO
    {
           /*DTO is only used to pass data and does not contain any business logic. They only have simple setters and getters.*/
        public int Id { get; set; }
        public string Url { get; set; }

        public bool IsMain { get; set; }
    }
}