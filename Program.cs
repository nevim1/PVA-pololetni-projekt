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
							user.Name = GetNonEmpty("name");
							break;
						case "2":
							user.Surname = GetNonEmpty("surname");
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
					Console.WriteLine("1 - Manage books");
					Console.WriteLine("2 - Manage users");
					Console.WriteLine("b - Go back");
					switch(Console.ReadLine()){
						case "1":
							while(goAgain){
								Console.WriteLine("1 - Add book");
								Console.WriteLine("2 - Modify book");
								Console.WriteLine("3 - Remove book");
								Console.WriteLine("b - Go back");
								switch(Console.ReadLine()){
									case "1":
										break;
									case "2":
										break;
									case "3":
										break;
									case "b":
										goAgain = false;
										break;
								}
							}
							break;
						case "2":
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

	string name = GetNonEmpty("name");

	string surname = GetNonEmpty("surname");
	
	string email = GetValidEmail();


	User newUser = new User{Username = username, Employee = employee, PasswordHash = hasedPassword, Email = email, Name = name, Surname = surname};

	db.Add(newUser);
	db.SaveChanges();
	return newUser; // if there will be problems with updating the users data, try returning it by quering it
}


string GetNonEmpty(string what){
	Console.Write($"Enter your {what}: ");
	string giveBack;
	while(true){
		giveBack = Console.ReadLine();
		if(string.IsNullOrEmpty(giveBack)){
			Console.WriteLine($"Your {what} cannot be empty.");
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
}
