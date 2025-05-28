using FinalProject.Models;
using Microsoft.Data.Sqlite;

namespace FinalProject.Services
{
	public class UserService
	{
		private readonly string _connectionString;

		// פעולה בונה ליוזר סרוויס. יוצרת את נתיב החיבור לבסיס הנתונים ומוודא שהוא קיים באמצעות
		// EnsureData();
		public UserService(string connectionString)
		{
			_connectionString = connectionString;
			EnsureDatabase();
		}

		// פונקציה שמטרתה לוודא שקיים קובץ בסיס נתונים. אם אין כזה הפונקציה תיצור אותו
		// הפונקציה איננה מקבלת או מחזירה דבר
		// שימו לב לפקודת יצירת טבלת המשתמשים בבסיס הנתונים במחרוזת
		// createTableSQL
		private void EnsureDatabase()
		{
			using var connection = new SqliteConnection(_connectionString);
			connection.Open();
			string createTableSQL = @"CREATE TABLE IF NOT EXISTS Users (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT NOT NULL,
                                    Age INTEGER,
                                    Username TEXT NOT NULL,
                                    Password TEXT NOT NULL,
                                    Role TEXT NOT NULL
                                  );";
			using var command = new SqliteCommand(createTableSQL, connection);
			command.ExecuteNonQuery();
		}

		// פונקציה להוספת משתמש חדש
		// הפונקציה מקבלת משתנה מסוג יוזר ומעדכנת את בסיס הנתונים באמצעות המחרוזת 
		// inserSql
		public void AddUser(User newUser)
		{
			using var connection = new SqliteConnection(_connectionString);
			connection.Open();
			string insertSQL = "INSERT INTO Users (Name, Age, Username, Password, Role) VALUES (@name, @age, @username, @passwordHash, @role);";
			using var command = new SqliteCommand(insertSQL, connection);
			command.Parameters.AddWithValue("@name", newUser.Name);
			command.Parameters.AddWithValue("@age", newUser.Age);
			command.Parameters.AddWithValue("@username", newUser.Username);
			command.Parameters.AddWithValue("@passwordHash", newUser.Password);
			command.Parameters.AddWithValue("@role", newUser.Role);
			command.ExecuteNonQuery();
		}

		// פונקציה שפונה לשירות היוזרים ומבקשת בסיס הנתונים את רשימת כל המשתמשים 
		public List<User> GetAllUsers()
		{
			var users = new List<User>();
			using var connection = new SqliteConnection(_connectionString);
			connection.Open();
			string selectSQL = "SELECT Id, Name, Age, Username, Password, Role FROM Users";
			using var command = new SqliteCommand(selectSQL, connection);
			using var reader = command.ExecuteReader();

			while (reader.Read())
			{
				var user = new User
				{
					Id = reader.GetInt32(0),
					Name = reader.GetString(1),
					Age = reader.GetInt32(2),
					Username = reader.GetString(3),
					Password = reader.GetString(4),
					Role = reader.GetString(5)
				};
				users.Add(user);
			}

			return users;
		}

		// מחיקה של משתמש מבסיס הנתונים דרך המספר המזהה שלו
		public void DeleteUser(int userId)
		{
			using var connection = new SqliteConnection(_connectionString);
			connection.Open();
			string deleteSQL = "DELETE FROM Users WHERE Id = @id;";
			using var command = new SqliteCommand(deleteSQL, connection);
			command.Parameters.AddWithValue("@id", userId);
			command.ExecuteNonQuery();
		}

		// פונקציה המשמשת לעדכון משתמש קיים
		public void UpdateUser(User updatedUser)
		{
			using var connection = new SqliteConnection(_connectionString);
			connection.Open();
			string updateSQL = "UPDATE Users SET Name = @name, Age = @age, Username = @username, Password = @password, Role = @role WHERE Id = @id;";
			using var command = new SqliteCommand(updateSQL, connection);
			command.Parameters.AddWithValue("@id", updatedUser.Id);
			command.Parameters.AddWithValue("@name", updatedUser.Name);
			command.Parameters.AddWithValue("@age", updatedUser.Age);
			command.Parameters.AddWithValue("@username", updatedUser.Username);
			command.Parameters.AddWithValue("@password", updatedUser.Password);
			command.Parameters.AddWithValue("@role", updatedUser.Role);
			command.ExecuteNonQuery();
		}
	}

}

