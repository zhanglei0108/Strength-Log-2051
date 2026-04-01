using SQLite;

namespace Strength_Log;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    public async Task InitAsync()
    {
        if (_database is not null)
            return;

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "strengthlog.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<UserProfileRecord>();
        await _database.CreateTableAsync<WorkoutRecord>();
    }

    // ========== User Profile ==========
    public async Task<int> SaveUserProfileAsync(UserProfileRecord profile)
    {
        await InitAsync();

        if (profile.Id != 0)
            return await _database!.UpdateAsync(profile);

        return await _database!.InsertAsync(profile);
    }

    public async Task<UserProfileRecord?> GetLatestUserProfileAsync()
    {
        await InitAsync();

        return await _database!
            .Table<UserProfileRecord>()
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> DeleteAllUserProfilesAsync()
    {
        await InitAsync();
        return await _database!.DeleteAllAsync<UserProfileRecord>();
    }

    // ========== Workout Records ==========
    public async Task<int> SaveWorkoutAsync(WorkoutRecord workout)
    {
        await InitAsync();

        if (workout.Id != 0)
            return await _database!.UpdateAsync(workout);

        return await _database!.InsertAsync(workout);
    }

    public async Task<WorkoutRecord?> GetLatestWorkoutAsync()
    {
        await InitAsync();

        return await _database!
            .Table<WorkoutRecord>()
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();
    }
    public async Task<List<WorkoutRecord>> GetAllWorkoutsAsync()
    {
        await InitAsync();

        return await _database!
            .Table<WorkoutRecord>()
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<int> DeleteAllWorkoutsAsync()
    {
        await InitAsync();
        return await _database!.DeleteAllAsync<WorkoutRecord>();
    }

    public async Task<int> ClearAllDataAsync()
    {
        await InitAsync();

        await _database!.DeleteAllAsync<UserProfileRecord>();
        await _database!.DeleteAllAsync<WorkoutRecord>();

        return 1;
    }
}