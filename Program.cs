using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace movies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new MoviesDbContext();
            ShowMenu(db);
        }
        public static void ShowMenu(MoviesDbContext db)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.WriteLine("Selecciones el proceso deseado");
            System.Console.WriteLine("1-.Registrar");
            System.Console.WriteLine("2-.Consultar");
            System.Console.WriteLine("3-.Actualizar");
            System.Console.WriteLine("4-.Eliminar");
            Console.ForegroundColor = ConsoleColor.Gray;
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Create(db);
                    break;
                case "2":
                    Show(db);
                    break;
                case "3":
                    Update(db);
                    break;
                case "4":
                    Delete(db);
                    break;
            }
        }
        public static void Create(MoviesDbContext db)
        {
            // se solicitan datos otorgados por el usuario 
            Console.WriteLine("Escriba el nombre de la pelicula");
            // Console.ReadLine() retorna un string 
            var name = Console.ReadLine();
            System.Console.WriteLine("Escriba el año de estreno:");
            var year = Console.ReadLine();
            // seteamos valores en un objeto tipo Movie 
            var model = new Movie();
            model.Name = name;
            model.Year = int.Parse(year);
            // guardamos en bD
            db.Movies.Add(model);
            var resultado = db.SaveChanges();
            if (resultado == 1)
                System.Console.WriteLine("La pelicula se a guardado correctamente");
            else
                System.Console.WriteLine("ocurrio un error");
            System.Console.WriteLine("Preciones cualquier tecla para salir...");
            Console.ReadLine();
        }
        public static void Update(MoviesDbContext db)
        {
            System.Console.WriteLine("Ingresa el nombre de la pelicula");
            var MovieName = Console.ReadLine().ToLower();

            var result = db.Movies.Where(x => x.Name == MovieName).Take(1).FirstOrDefault();
            if (result != null)
            {
                System.Console.WriteLine("Si no desea actualizar algun campo solo dejelo vacio y presione Enter ");
                System.Console.WriteLine();
                System.Console.WriteLine("Escriba el nuevo nombre");
                var newName = Console.ReadLine();
                System.Console.WriteLine("Escriba el la nueva fecha de lanzamiento");
                var year = int.Parse(Console.ReadLine());
                if (newName != "")
                    result.Name = newName;
                if (year > 0)
                    result.Year = year;
                var model = new Movie()
                {
                    Name = result.Name,
                    Year = result.Year
                };
                db.Movies.Update(model);
                db.SaveChanges();
            }
            else
                System.Console.WriteLine("No se encontro ninguna pelicula");
            System.Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadLine();
        }
        public static void Delete(MoviesDbContext db)
        {
            System.Console.WriteLine("Ingresa el nombre de la pelicula que deseas eliminar");
            var NameMovie = Console.ReadLine();
            var model = db.Movies.Where(x => x.Name == NameMovie).Take(1).FirstOrDefault();
            db.Movies.Remove(model);
            db.SaveChanges();
        }
        public static void Show(MoviesDbContext dbContext)
        {
         var result = dbContext.Movies.ToList();
         foreach(var item in result)
         {
         System.Console.WriteLine("la pelicula es {0} que se estreno en el año {1}",item.Name, item.Year);
         }
         System.Console.WriteLine("Presiones cualquier tecla para salir");
         Console.ReadLine();
        }
    }
    #region Clases 
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        [Required]
        [Range(1900, 2040)]
        public int Year { get; set; }
    }
    //se utiliza la clase dbContext para poder traducir todoa la analogia que escribamos en el objeto a la Bd
    public class MoviesDbContext : DbContext
    {
        // se debe expecificar que proveedor de datos se va a utilizar en este caso se utiliza el paquete sql
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=Movies;Trusted_Connection=true;");
        }
        public DbSet<Movie> Movies { get; set; } // representa la tabla movie, esto permite que sql lo reconozca 
    }
    #endregion
}


