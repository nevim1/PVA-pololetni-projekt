using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

using var db = new LibraryContext();

Console.WriteLine("Hello, welcome to the library system");
User user = null;

GetUser(ref user);

Console.WriteLine($"You're now logged in as {user.Name} {user.Surname}");

return 0;

void GetUser(ref User user){
	Console.Write("Start by writing your username for this system: ");
	string username;

	while(true){
		username = Console.ReadLine();
		if(string.IsNullOrEmpty(username)){
			Console.WriteLine("Your username cannot be empty.");
			Console.Write("Try again: ");
			continue;
		}

		try{
			user = db.Users.SingleAsync(x => x.Username == username).Result;
		} catch(InvalidOperationException){
			Console.WriteLine("There is no user with that username.");
			Console.Write("Try again: ");
			continue;
		}

		int tries = 3;
		for(; tries != 0; tries--){
			Console.Write("Enter your password: ");
			string password = Console.ReadLine();
			if(BCrypt.Net.BCrypt.Verify(password ?? "", user.PasswordHash)){
				break;
			} else {
				Console.WriteLine("Wrong password.");
			}
		}

		if(tries == 0){
			Console.WriteLine("Too many bad tries.");
			return;
			// if you want to redirect user back to login screen, replace return with continue
		}
		break;
	}

	Console.WriteLine($"You're now logged in as {user.Name} {user.Surname}");
}

//db.Add(new User{Name = "John", Surname = "Doe", Username = "admin", Employee = true, Email = "john.doe@lib.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")});
//await db.SaveChangesAsync();

// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

// Create
Console.WriteLine("Inserting a new book");
db.Add(new Book { Name = "R.U.R.", Autor = "Karel Čapek", ReleaseYear = 1928, Genre = "sci-fi", Count = 1, ISBN = "12"});
await db.SaveChangesAsync();

// Read
Console.WriteLine("Querying for a book");
var book = await db.Books
	.OrderBy(b => b.BookId)
	.FirstAsync();

// Update
Console.WriteLine("Updating the book");
book.Name = "Bílá nemoc";
await db.SaveChangesAsync();

// Delete
Console.WriteLine("Delete the book");
db.Remove(book);
await db.SaveChangesAsync();
