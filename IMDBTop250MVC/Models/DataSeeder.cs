using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

namespace IMDBTop250MVC.Models
{
    public class DataSeeder
    {
        
            private readonly MovieContext _context;

            public DataSeeder(MovieContext context)
            {
                _context = context;
            }

            public void SeedData()
            {
                // Eğer veritabanında veri varsa tekrar yüklemeyin
                if (_context.Movies.Any()) return;

                using (var reader = new StreamReader("Data/IMDB Top 250 Movies.csv"))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
                {
                    // CSV dosyasındaki her bir satırı MovieCsvRecord olarak okuyun
                    var records = csv.GetRecords<MovieCsvRecord>().ToList();
                    foreach (var record in records)
                    {
                        var movie = new Movie
                        {
                            Title = record.Title,
                            Director = record.Director,
                            Metascore = int.TryParse(record.Metascore, out int metascore) ? metascore : 0,
                            ReleaseDate = DateTime.TryParse(record.ReleaseDate, out DateTime releaseDate) ? releaseDate : DateTime.MinValue,
                            MovieGenres = record.Genre
                                .Split(',')
                                .Select(g => new MovieGenre { Genre = new Genre { Name = g.Trim() } })
                                .ToList()
                        };
                        _context.Movies.Add(movie);
                    }
                    _context.SaveChanges();
                }
            }
        }

        public class MovieCsvRecord
        {
            public string Title { get; set; }
            public string Director { get; set; }
            public string Metascore { get; set; }
            public string ReleaseDate { get; set; }
            public string Genre { get; set; }
        }
    }
