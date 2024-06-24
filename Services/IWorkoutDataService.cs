using PTManagementSystem.Models;

namespace PTManagementSystem.Services
{
    public interface IWorkoutDataService
    {
        List<WorkoutModel> GetAllWorkouts();
        List<WorkoutModel> SearchWorkouts(string searchTerm);

        WorkoutModel GetWorkoutById(int id);

        // Lists all of a specific users workouts for a coach to view their clients training.
        List<WorkoutModel> GetAllWorkoutsByUserId(int UserId);

        List<ClientWeeklyReportModel> GetAllWeeklyReportsByUserId(int UserId);


        List<ImageModel> GetAllImagesByWeeklyReportId(int UserId);
        //int Insert(WorkoutModel workout);
        //int Update(WorkoutModel workout);

        //int Delete(WorkoutModel workout);
    }
}
