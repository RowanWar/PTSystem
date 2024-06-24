using Microsoft.AspNetCore.Mvc;
using PTManagementSystem.Models;
using PTManagementSystem.Services;

namespace PTManagementSystem.Controllers

    // This is prolly deprecated now.
{
    public class WorkoutController : Controller
    {
        // Create a list out of the workout model so the forEach in the .cshtml can iterate through all the workouts properly.
        static List<WorkoutModel> workouts = new List<WorkoutModel>();
        public IActionResult Index()
        {
            // What does this line do? 
            WorkoutDAO workout = new WorkoutDAO();


            // Currently hard coded to use a user ID here. 
            return View(workout.GetAllWorkoutsByUserId(9));
        }
    }
}
