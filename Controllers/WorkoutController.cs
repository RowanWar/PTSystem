using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
using PTManagementSystem.Models;
using PTManagementSystem.Services;
using System.Text.Json; // Might be redundant


namespace PTManagementSystem.Controllers

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

        public IActionResult CreateWorkout()
        {

            return View();
        }


        public IActionResult ViewActiveWorkoutByUserId(int UserId)
        {
            WorkoutDAO workout = new WorkoutDAO();
            List<WorkoutExercisesModel> activeWorkout = workout.GetUsersActiveWorkout(UserId);

            string resultSerialized = JsonSerializer.Serialize(activeWorkout);

            return Json(resultSerialized);
        }


        //public async Task<IActionResult> CreateWorkout(int UserId)
        //{



        //    WorkoutDAO exercise = new WorkoutDAO();
        //    int result = await exercise.CreateWorkoutInDatabase(WorkoutId, ExerciseIds);

        //    //string resultSerialized = JsonSerializer.Serialize(activeWorkout);

        //    return Json(result);
        //}



        //Grabs the list of ExerciseIds from the POST body
        public async Task<IActionResult> InsertExercises([FromBody] JsonElement data)
        {

            int WorkoutId = data.GetProperty("WorkoutId").GetInt32();
            List<int> ExerciseIds = JsonSerializer.Deserialize<List<int>>(data.GetProperty("ExerciseIds").GetRawText());
            WorkoutDAO exercise = new WorkoutDAO();
            int result = await exercise.AddExercisesToDatabase(WorkoutId, ExerciseIds);

            //string resultSerialized = JsonSerializer.Serialize(activeWorkout);

            return Json(result);
        }


        public IActionResult SubmitExercises(int UserId)
        {
            WorkoutDAO workout = new WorkoutDAO();
            List<WorkoutExercisesModel> activeWorkout = workout.GetUsersActiveWorkout(UserId);

            string resultSerialized = JsonSerializer.Serialize(activeWorkout);

            return Json(resultSerialized);
        }

        //Responds to a fetch request, providing a list of exercises from within the "exercise" table in the db
        //Passes in the UserId within query to also display any custom exercises created by that user in the db
        public IActionResult ViewExerciseList()
        {
            WorkoutDAO result = new WorkoutDAO();
            List<ExerciseModel> exerciseList = result.GetAllExercisesByUserId();

            string resultSerialized = JsonSerializer.Serialize(exerciseList);

            System.Diagnostics.Debug.WriteLine(result);
            return Json(resultSerialized);
            //return View("CreateWorkout", exerciseList);
        }

    }
}
