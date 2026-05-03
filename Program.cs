using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Text.RegularExpressions;

using var db = new LibraryContext();

User user = null;

Console.WriteLine("Hello, welcome to the library system");

while(true){
	if(user == null){
		Console.WriteLine("q - Quit");
		Console.WriteLine("1 - Log in");
		Console.WriteLine("2 - Sign in");
		Console.WriteLine("3 - Browse books");
		switch(Console.ReadLine()){
			case "q":
				return 0;
			case "1":
				GetUser(ref user);
				Console.WriteLine($"You're now logged in as {user.Name} {user.Surname}");
				break;
			case "2":
				user = AddUser(false);
				Console.WriteLine($"You're now logged in as {user.Name} {user.Surname}");
				break;
			case "3":
				BrowseBooks(ref user);
				break;
default:
				break;
		}
	} else {
		Console.WriteLine("q - Quit");
		Console.WriteLine("1 - Log out");
		Console.WriteLine("2 - Browse books");
		Console.WriteLine("3 - Show my profile");
		if(user.Employee) Console.WriteLine("4 - Admin");
		switch(Console.ReadLine()){
			case "q":
				return 0;
			case "1":
				user = null;
				Console.WriteLine("Logging out");
				break;
			case "2":
				BrowseBooks(ref user);
				break;
			case "3":
				Console.WriteLine("Your profile:");
				Console.WriteLine($"Full name: {user.Name} {user.Surname}");
				Console.WriteLine($"Email: {user.Email}\n");
				bool goAgain = true;
				while(goAgain){
					Console.WriteLine("1 - Change Name");
					Console.WriteLine("2 - Change Surname");
					Console.WriteLine("3 - Change Email");
					Console.WriteLine("4 - Change Password");
					Console.WriteLine("b - Go back");
					switch(Console.ReadLine()){
						case "1":
							user.Name = GetNonEmpty("your name");
							break;
						case "2":
							user.Surname = GetNonEmpty("your surname");
							break;
						case "3":
							user.Email = GetValidEmail();
							break;
						case "4":
							user.PasswordHash = NewPassword();
							break;
						case "b":
							goAgain = false;
							break;
					}
				}
				break;
			case "4":
				if(!user.Employee){
					break;
				}

				goAgain = true;
				while(goAgain){
					Console.WriteLine("1 - Add book");
					Console.WriteLine("2 - Add employee");
					Console.WriteLine("b - Go back");
					switch(Console.ReadLine()){
						case "1":
							AddBook();
							break;
						case "2":
							AddUser(true);
							break;
						case "b":
							goAgain = false;
							break;
					}
				}

				break;
			default:
				break;
		}
	}
}

void BrowseBooks(ref User user){
	int perPage = 10;

	sorting:
	int page = 0;

	Console.WriteLine("Sort by:");
	Console.WriteLine("  1 - Name");
	Console.WriteLine("  2 - Autor");
	Console.WriteLine("  3 - Relealse date");

	List<Book> books = new List<Book>();
	bool goAgain = true;
	while(goAgain){
		goAgain = false;
		switch(Console.ReadLine()){
			case "1":
				books = db.Books.OrderBy(b => b.Name).Skip(page*perPage).Take(perPage).ToList();
				break;
			case "2":
				books = db.Books.OrderBy(b => b.Autor).Skip(page*perPage).Take(perPage).ToList();
				break;
			case "3":
				books = db.Books.OrderBy(b => b.ReleaseYear).Skip(page*perPage).Take(perPage).ToList();
				break;
			default:
				goAgain = true;
				break;
		}
	}

	pageView:
	Console.WriteLine($"\nPage: {page+1}");

	int i = 0;
	foreach(var book in books){
		i++;
		Console.WriteLine($"{i}) {book.Name} {book.Autor} {book.ReleaseYear} {book.Genre} {book.ISBN}");
	}
	bool lastPage = i < perPage;
	Console.WriteLine($"\n1-{(lastPage ? i : perPage)} - More options for book");
	Console.WriteLine("p - Prevoius page");
	Console.WriteLine("n - Next page");
	Console.WriteLine("s - Change sorting");
	Console.WriteLine("b - Go back");

	string pick = Console.ReadLine();
	switch(pick){
		case "p":
			page = page == 0 ? 0 : page - 1;
			break;
		case "n":
			page = lastPage ? page : page + 1;
			break;
		case "s":
			goto sorting;
		case "b":
			return;
		default:
			int bookIdx;
			if(Int32.TryParse(pick, out bookIdx)){
				bookIdx--;
				if(bookIdx <= perPage){
					Book book = books[bookIdx];
					Console.WriteLine($"Name: {book.Name}");
					Console.WriteLine($"Autor: {book.Autor}");
					Console.WriteLine($"Release year: {book.ReleaseYear}");
					Console.WriteLine($"ISBN: {book.ISBN}");

					goAgain = true;
					while(goAgain){
						Console.WriteLine("1 - Borrow");
						Console.WriteLine("2 - Reserve");
						Console.WriteLine("3 - Return");
						//TODO: add Extend option and logic
						if(user.Employee){
							Console.WriteLine("4 - Edit");
							Console.WriteLine("5 - Remove");
						}
						Console.WriteLine("b - Back");

						switch(Console.ReadLine()){
							case "1":
								//TODO: check if there is already exiting Borrow for this user in reserved state
								//TODO: chick for existing reservations to correctly set CanExtend
								db.Borrows.Add(new Borrow{BorrowedBook = book, Borrower = user, DateBorrowed = DateTime.Now, ReturnDate = DateTime.Now.AddMonths(1), Status = BorrowStatus.Borrowed, CanExtend = true});
								break;
							case "2":
								var colidingBorrow = db.Borrows
									.Where(b => b.BorrowedBook == book)
									.Where(b => b.Status == BorrowStatus.Borrowed)
									.Single();
								colidingBorrow.CanExtend = false;
								
								db.Borrows.Add(new Borrow{BorrowedBook = book, Borrower = user, DateReserved = DateTime.Now, Status = BorrowStatus.Reserved, CanExtend = true});
								break;
							case "3":
								User userCapture = user;
								var borrow = db.Borrows
									.Where(b => b.BorrowedBook == book)
									.Where(b => b.Borrower == userCapture)
									.Single();
								borrow.Status = BorrowStatus.Returned;
								borrow.DateReturned = DateTime.Now;
								db.SaveChanges();
								break;
							case "4":
								while(goAgain){
									Console.WriteLine("1 - Edit name");
									Console.WriteLine("2 - Edit autor");
									Console.WriteLine("3 - Edit release year");
									Console.WriteLine("4 - Edit ISBN");
									Console.WriteLine("b - Go back");
									switch(Console.ReadLine()){
										case "1":
											book.Name = GetNonEmpty("name");
											break;
										case "2":
											book.Autor = GetNonEmpty("autor");
											break;
										case "3":
											book.ReleaseYear = GetInt("release year");
											break;
										case "4":
											book.ISBN = GetNonEmpty("ISBN");
											break;
										case "b":
											goAgain = false;
											break;
										default:
											break;
									}
								}
								goAgain = true;
								break;
							case "5":
								Console.WriteLine("Are you sure? [N/y]");
								if(Regex.IsMatch(Console.ReadLine(), @"[Yy][Ee][Ss]")){
									db.Remove(book);
									db.SaveChanges();
									goAgain = false;
								}
								break;
							case "b":
								goAgain = false;
								break;
							default:
								break;
						}
					}
				}
			}
			break;
	}
	goto pageView;
}

// functions

void GetUser(ref User user){
	Console.Write("Write your username for this system: ");
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


string NewPassword(){
	Console.Write("Enter new password: ");
	string password;
	while(true){
		password = Console.ReadLine();
		if(string.IsNullOrEmpty(password)){
			Console.WriteLine("Your password cannot be empty.");
			Console.Write("Try again: ");
			continue;
		}
		Console.Write("Retype new password: ");
		if(Console.ReadLine() == password){
			break;
		}
		Console.WriteLine("Passwords don't match.");
		Console.Write("Enter again new password: ");
	}
	return BCrypt.Net.BCrypt.HashPassword(password);
}


User AddUser(bool employee){
	Console.Write("Write your username: ");
	string username;

	while(true){
		username = Console.ReadLine();
		if(string.IsNullOrEmpty(username)){
			Console.WriteLine("Your username cannot be empty.");
			Console.Write("Try again: ");
			continue;
		}

		try{
			User _ = db.Users.SingleAsync(x => x.Username == username).Result;
		} catch(Exception ex){
			if(ex is InvalidOperationException || ex is AggregateException){
				break;
			} else throw;
		}
		Console.WriteLine("Sorry, there is anither user with that user name.");
		Console.Write("Try again: ");
	}

	string hasedPassword = NewPassword();

	string name = GetNonEmpty("your name");

	string surname = GetNonEmpty("your surname");

	string email = GetValidEmail();


	User newUser = new User{Username = username, Employee = employee, PasswordHash = hasedPassword, Email = email, Name = name, Surname = surname};

	db.Add(newUser);
	db.SaveChanges();
	return newUser; // if there will be problems with updating the users data, try returning it by quering it
}


string GetNonEmpty(string what){
	Console.Write($"Enter {what}: ");
	string giveBack;
	while(true){
		giveBack = Console.ReadLine();
		if(string.IsNullOrEmpty(giveBack)){
			Console.WriteLine($"{CapitalizeFirstLetter(what)} cannot be empty.");
			Console.Write("Try again: ");
		} else break;
	}
	return giveBack;
}


string GetValidEmail(){
	Console.Write("Enter your email: ");
	string email;
	while(true){
		email = Console.ReadLine();
		if(string.IsNullOrEmpty(email)){
			Console.WriteLine("Your email cannot be empty.");
			Console.Write("Try again: ");
		} else if(!new Regex(@"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$").IsMatch(email)){
			Console.WriteLine("Your email has wrong format.");
			Console.Write("Try again: ");
		} else break;
	}
	return email;
}


int GetInt(string what){
	Console.Write($"Enter {what}: ");
	int giveBack;
	while(true){
		if(int.TryParse(Console.ReadLine(), out giveBack)){
			break;
		} else {
			Console.WriteLine($"{what} must be a number");
			Console.Write("Try again: ");
		}
	}
	return giveBack;
}


void AddBook(){
	string name = GetNonEmpty("name");
	string autor = GetNonEmpty("autor");
	string isbn = GetNonEmpty("ISBN");
	int releaseYear = GetInt("Release year");

	db.Add(new Book{Name=name, ISBN=isbn, Autor=autor, ReleaseYear=releaseYear});
	db.SaveChanges();
}


string CapitalizeFirstLetter(string s){
	if (String.IsNullOrEmpty(s))
		return s;
	if (s.Length == 1)
		return s.ToUpper();
	return s.Remove(1).ToUpper() + s.Substring(1);
}
