using SQLite;

namespace Strength_Log;

public class WorkoutRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string ExerciseType { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
    public string DateText { get; set; } = string.Empty;
}