namespace WebApi.Models
{
    public class Show
    {
        public int Id {get; set;}
        public string? Name {get; set;}
        public DateTime DateOfRelease {get; set;}
        public int Seasons {get; set;}
        public string? Genre {get; set;}
    }
    
}