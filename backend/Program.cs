using Microsoft.EntityFrameworkCore;
public class Purchase
{
    public int Id { get; set; }          // Primary Key
    public string? Name { get; set; }     // Название покупки
    public decimal Price { get; set; }   // Цена
}

public class AppDbContext : DbContext
{
    public DbSet<Purchase> Purchases => Set<Purchase>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=app.db");
    }
}

class Program
{
    static void Main()
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();

        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("Введите покупку: название цена");
            Console.WriteLine("Например: хлеб 180");

            string? enter = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(enter))
            {
                Console.WriteLine("Ошибка: строка пустая.");
                continue;
            }

            string[] words = enter.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 2)
            {
                var purchases = db.Purchases.ToList();

                switch (words[0])
                {
                    case "показать":
                        foreach (var item in purchases)
                        {
                            Console.WriteLine($"{item.Id}: {item.Name} - {item.Price} RSD");
                        }
                        break;

                    case "очистить":
                        db.Purchases.RemoveRange(purchases);
                        db.SaveChanges();
                        break;
                    case "выход":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Ошибка: нужно ввести название и цену.");
                        break;

                }
                continue;
            }

            if (!decimal.TryParse(words[1], out decimal price))
            {
                Console.WriteLine("Ошибка: цена должна быть числом.");
                continue;
            }

            var purchase = new Purchase
            {
                Name = words[0],
                Price = price
            };

            try
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();

                Console.WriteLine("Покупка сохранена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении в базу:");
                Console.WriteLine(ex.Message);
                continue;
            }
        }
    }
}