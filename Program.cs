using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

using var db = new LibraryContext();

User user = null;

Console.WriteLine("Hello, welcome to the library system");

while(true){
	if(user == null){
		Console.WriteLine("q - Quit");
		Console.WriteLine("1 - Log in");
		Console.WriteLine("2 - Browse books");
		switch(Console.ReadLine()){
			case "q":
				return 0;
			case "1":
				GetUser(ref user);
				Console.WriteLine($"You're now logged in as {user.Name} {user.Surname}");
				break;
			case "2":
			default:
				break;
		}
	} else if(!user.Employee){
		Console.WriteLine("q - Quit");
		Console.WriteLine("1 - Log out");
		Console.WriteLine("2 - Browse books");
		Console.WriteLine("3 - Show my profile");
		switch(Console.ReadLine()){
			case "q":
				return 0;
			case "1":
				user = null;
				Console.WriteLine("Logging out");
				break;
			case "2":
			case "3":
			default:
				break;
		}
	} else {
		Console.WriteLine("q - Quit");
		Console.WriteLine("1 - Log out");
		Console.WriteLine("2 - Browse books");
		Console.WriteLine("3 - Show my profile");
		Console.WriteLine("4 - Admin");
		switch(Console.ReadLine()){
			case "q":
				return 0;
			case "1":
				user = null;
				Console.WriteLine("Logging out");
				break;
			case "2":
			case "3":
			case "4":
			default:
				break;
		}
	}
}

// functions

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
		} catch(Exception ex){
			if(ex is InvalidOperationException || ex is AggregateException){
				Console.WriteLine("There is no user with that username.");
				Console.Write("Try again: ");
				continue;
			} else throw;
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
}

//db.Add(new User{Name = "John", Surname = "Doe", Username = "admin", Employee = true, Email = "john.doe@lib.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("")});
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
