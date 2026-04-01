using SQLite;

namespace Strength_Log;

public class UserProfileRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public double Height { get; set; }
    public double Weight { get; set; }
    public string Gender { get; set; } = string.Empty;
    public double BMI { get; set; }
}