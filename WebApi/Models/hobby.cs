namespace WebApi.Models
{
    public class Hobby
    {
        public int Id {get; set;}
        public string? Name {get; set;}
        public string? Category {get; set;}
        public DateTime StartOfHobby {get; set;}
        public int YearsOfHavingHobby {get; set;}
    }
}