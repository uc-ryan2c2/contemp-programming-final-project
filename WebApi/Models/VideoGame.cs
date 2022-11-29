namespace WebApi.Models{
    public class VideoGame
    {
        public int Id { get; set; }
        public string? Name {get; set;}
        public DateTime ReleaseDate {get; set;}
        public string? Genre {get; set;}
        public int NumberOfPLayers {get; set;}
    }
}