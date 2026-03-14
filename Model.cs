using Microsoft.EntityFrameworkCore;


public class Book {
	public int BookId{ get; set; }
	public required string Name{ get; set; }
	public required string Autor{ get; set; }
	public required int ReleaseYear{ get; set; }
	public required string Genre{ get; set; }
	public required int Count{ get; set; }
	public required string ISBN{ get; set; }
}

public class User {
	public required string Username{ get; set; }
	public required string PasswordHash { get; set; }
	public required string Name{ get; set; }
	public required string Surname{ get; set; }
	public required string Email{ get; set; }
	public required bool Employee{ get; set; }
	public List<Borrow> BorrowedBooks { get; } = [];
}

public class Borrow {
	public int BorrowId { get; set; }
	public required Book BorrowedBook{ get; set; }
	public required DateTime DateBorrowed{ get; set; }
	public required DateTime ReturnDate{ get; set; }
	public DateTime DateReturned{ get; set; }
}


public class LibraryContext : DbContext {
	public DbSet<Book> Books { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Borrow> Borrows { get; set; }

	public string DbPath { get; }

	public LibraryContext() {
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = System.IO.Path.Join(path, "library.db");
	}

	// The following configures EF to create a Sqlite database file in the
	// special "local" folder for your platform.
	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data Source={DbPath}");

	protected override void OnModelCreating(ModelBuilder modelBuilder){
		modelBuilder.Entity<User>().HasKey(x => x.Username);
	}
}

