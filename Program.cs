using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
//using BCryptNet;
using BCrypt.Net;

using var db = new LibraryContext();

//db.Add(new User());
db.Add(new User{Name = "John", Surname = "Doe", Username = "admin", Employee = true, Email = "john.doe@lib.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")});
await db.SaveChangesAsync();

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
