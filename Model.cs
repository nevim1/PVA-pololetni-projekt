using Microsoft.EntityFrameworkCore;


public class Book {
	public int BookId{ get; set; }
	public required string Name{ get; set; }
	public required string Autor{ get; set; }
	public required int ReleaseYear{ get; set; }
	public string Genre{ get; set; } = "";
	public required string ISBN{ get; set; }
}

public class User {
	public required string Username{ get; set; }
	public required string PasswordHash { get; set; }
	public required string Name{ get; set; }
	public required string Surname{ get; set; }
	public required string Email{ get; set; }
	public required bool Employee{ get; set; }
}

public class Borrow {
	public int BorrowId{ get; set; }
	public required Book BorrowedBook{ get; set; }
	public required User Borrower{ get; set; }
	public DateTime DateBorrowed{ get; set; }
	public DateTime DateReserved{ get; set; }
	public DateTime ReturnDate{ get; set; }
	public DateTime DateReturned{ get; set; }
	public required BorrowStatus Status{ get; set; }
	public required bool CanExtend{ get; set; }
}

public enum BorrowStatus {
	Borrowed,
	Reserved,
	Returned
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

