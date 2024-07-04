using Microsoft.AspNetCore.Mvc;
using PTManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using PTManagementSystem.Services;
using System.Text.Json; // Might be redundant
namespace PTManagementSystem.Controllers
{
    public class ClientController : Controller
    {
        // Create a list out of the client model so the forEach in the index.cshtml can iterate through all the clients properly.
        static List<ClientModel> clients = new List<ClientModel>();
        public IActionResult Index()
        {
            ClientDAO clients = new ClientDAO();

            return View(clients.GetAllClients());
        }

        // Displays a list of all workouts performed by a specific user based upon their UserId in the DB.
        public IActionResult ClientWorkouts(int ClientUserId)
        {
            WorkoutDAO workout = new WorkoutDAO();
            List<WorkoutModel> workoutList = workout.GetAllWorkoutsByUserId(ClientUserId);

            return View("ClientWorkout", workoutList);
        }

        // Displays the workout details of a user based upon the WorkoutId provided
        public IActionResult WorkoutDetails(int WorkoutId)
        {
            WorkoutDAO workout = new WorkoutDAO();
            List<WorkoutExercisesModel> workoutDetails = workout.GetWorkoutDetailsByWorkoutId(WorkoutId);

            return View("~/Views/Workout/WorkoutDetails.cshtml", workoutDetails);
        }


        public IActionResult WeeklyReport(int ClientUserId)
        {
            WorkoutDAO report = new WorkoutDAO();
            List<ClientWeeklyReportModel> weeklyReport = report.GetAllWeeklyReportsByUserId(ClientUserId);

            return View("ClientReport", weeklyReport);
        }

        
        public IActionResult ViewImage(int ReportId)
        {
            WorkoutDAO report = new WorkoutDAO();
            List<ImageModel> weeklyReportImages = report.GetAllImagesByWeeklyReportId(ReportId);

            string result = JsonSerializer.Serialize(weeklyReportImages);

            //System.Diagnostics.Debug.WriteLine(result);
            return Json(result.ToString());
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(ClientModel client)
        {

            clients.Add(client);
            return View("Details", client);
        }

        public IActionResult Overview()
        {
            return View();
        }
    }
}
