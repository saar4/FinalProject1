namespace FinalProject.Models
{
	public class User
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Age { get; set; }
		public string? Username { get; set; }
		public string? Password { get; set; } //  לאחסון סיסמה
		public string? Role { get; set; }  // הגדרת תפקיד: אדמין או משתמש רגיל

	}
}
