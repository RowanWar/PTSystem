using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
using PTManagementSystem.Models;
using PTManagementSystem.Services;
using System.Text.Json;
using System.Text.Json.Nodes; // Might be redundant


namespace PTManagementSystem.Controllers

{
    public class WorkoutController : Controller
    {
        // Create a list out of the workout model so the forEach in the .cshtml can iterate through all the workouts properly.
        static List<WorkoutExerciseModel> workouts = new List<WorkoutExerciseModel>();
        public IActionResult Index()
        {
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
            //List<WorkoutExercisesModel> activeWorkout = workout.GetUsersActiveWorkout(UserId);


            System.Diagnostics.Debug.WriteLine("If statement has run");

            List<WorkoutExercisesModel> viewActiveWorkout = workout.ViewActiveWorkoutByUserId(UserId);
            string HasActiveWorkoutSerialized = JsonSerializer.Serialize(viewActiveWorkout);

            return Json(HasActiveWorkoutSerialized);

            //string resultSerialized = JsonSerializer.Serialize(activeWorkout);

            //return Json(resultSerialized);
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
            List<int> ExerciseIds = JsonSerializer.Deserialize<List<int>>(data.GetProperty("ExerciseIds").ToString());
            WorkoutDAO exercise = new WorkoutDAO();
            List<int> setIdsArr = await exercise.AddExercisesToDatabase(WorkoutId, ExerciseIds);

            // Adds a default empty set to every exercise created by the user for display purposes.
            int result = await exercise.AddSetToDatabase(setIdsArr);
          

            return Json(setIdsArr);
        }

        // Called when the user clicks the "add set" button in the active workout.
        public async Task<IActionResult> InsertSets(int WorkoutExerciseId)
        {

            // Converts the single ID passed to the function into an array of type integer so we can re-use the function already built in workoutDAO. 
            // Creates a new int list and adds the parameter ID to it, then passes it to the DAO function.
            List<int> WorkoutExerciseIdArr = new List<int> { WorkoutExerciseId };
            WorkoutDAO sets = new WorkoutDAO();

            int result = await sets.AddSetToDatabase(WorkoutExerciseIdArr);


            return Json(result);
        }

        public async Task<IActionResult> RemoveSets(int SetIds)
        {

            // Converts the single ID passed to the function into an array of type integer so we can re-use the function already built in workoutDAO. 
            // Creates a new int list and adds the parameter ID to it, then passes it to the DAO function.
            List<int> SetIdsArr = new List<int> { SetIds };
            //WorkoutDAO sets = new WorkoutDAO();
            WorkoutDAO workoutDAO = new WorkoutDAO();

            int result = await workoutDAO.RemoveSetsFromDatabase(SetIdsArr);

            return Json(result);
        }


        public IActionResult SubmitExercises(int UserId)
        {
            WorkoutDAO workout = new WorkoutDAO();
            List<WorkoutExercisesModel> activeWorkout = workout.GetUsersActiveWorkout(UserId);

            string resultSerialized = JsonSerializer.Serialize(activeWorkout);

            return Json(resultSerialized);
        }




        // API endpoint for when the user clicks to submit/finish a workout
        public async Task<IActionResult> SubmitWorkout(int UserId, [FromBody] JsonElement WorkoutData)
        {
            WorkoutDAO workout = new WorkoutDAO();

            int result = await workout.FinishUsersActiveWorkout(WorkoutData);

            // If the submitted object has no sets completed, return a message to the user stating such and exit early.
            if (result == 0)
            {
                return Json("ERROR: No sets were submitted for this workout!");
            }

            // Return a successful result.
            return Json(result);
        }




        //Responds to a fetch request, providing a list of exercises from within the "exercise" table in the db
        //Passes in the UserId within the query to also display any custom exercises created by that user in the db
        public IActionResult ViewExerciseList()
        {
            WorkoutDAO result = new WorkoutDAO();
            List<ExerciseModel> exerciseList = result.GetAllExercisesByUserId();

            string resultSerialized = JsonSerializer.Serialize(exerciseList);

            System.Diagnostics.Debug.WriteLine(result);
            return Json(resultSerialized);
            //return View("CreateWorkout", exerciseList);
        }   
        

        public IActionResult CheckForActiveWorkout(int UserId)
        {
            WorkoutDAO result = new WorkoutDAO();
            List<WorkoutModel> checkIfActiveWorkout = result.CheckActiveWorkoutByUserId(UserId);

            //string resultSerialized = JsonSerializer.Serialize(checkIfActiveWorkout);
            System.Diagnostics.Debug.WriteLine(checkIfActiveWorkout);

            //if (checkIfActiveWorkout != null)
            //{
            //    System.Diagnostics.Debug.WriteLine("If statement has run");

            //    List<WorkoutExercisesModel> viewActiveWorkout = result.ViewActiveWorkoutByUserId(UserId);
            //    string HasActiveWorkoutSerialized = JsonSerializer.Serialize(viewActiveWorkout);

            //    return Json(HasActiveWorkoutSerialized);
            //}
            System.Diagnostics.Debug.WriteLine(result);
            string resultSerialized = JsonSerializer.Serialize(checkIfActiveWorkout);

            return Json(resultSerialized);
            //return View("CreateWorkout", exerciseList);
        }

        //public IActionResult RemoveExerciseFromActiveWorkout(int WorkoutExerciseId)
        //{
        //    // code
        //}

    }
}
