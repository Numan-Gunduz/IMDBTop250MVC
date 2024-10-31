namespace IMDBTop250MVC.Models
{

        public class Movie
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Director { get; set; }
            public int Metascore { get; set; }
            public DateTime ReleaseDate { get; set; }

            public ICollection<MovieGenre> MovieGenres { get; set; }
        }
    
}
