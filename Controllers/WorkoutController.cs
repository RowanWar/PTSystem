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
            // this data should prolly not be static. Look to move this to an API pull for a maintainable list of categories in the database.
            //var combinedModel = new WorkoutExercisesModel
            //{
                
            //        MuscleGroupList = new List<SelectListItem>
            //        {
            //            new SelectListItem { Value = "Chest", Text = "Chest" },
            //            new SelectListItem { Value = "Back", Text = "Back" },
            //            new SelectListItem { Value = "Legs", Text = "Legs" },
            //            new SelectListItem { Value = "Glutes", Text = "Glutes" }
            //        },
             

            //        SetCategoryList = new List<SelectListItem>
            //            {
            //                new SelectListItem { Value = "Working Weight", Text = "Working Weight" },
            //                new SelectListItem { Value = "Warm-Up", Text = "Warm-Up" },
            //                new SelectListItem { Value = "Cluster", Text = "Cluster" },
            //                new SelectListItem { Value = "Dropset", Text = "Dropset" },
            //                new SelectListItem { Value = "Rest Pause", Text = "Rest Pause" },

            //            }
            //};

            //was returning combinedModel
            return View();
        }


        public IActionResult InsertExercises(int WorkoutId, int ExerciseId)
        {
            WorkoutDAO exercise = new WorkoutDAO();
            int result = exercise.AddExercisesToDatabase(WorkoutId, ExerciseId);

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
