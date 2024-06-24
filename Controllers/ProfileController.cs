using Microsoft.AspNetCore.Mvc;
using PTManagementSystem.Models;
using PTManagementSystem.Services;

namespace PTManagementSystem.Controllers
{
    public class ProfileController : Controller
    {
        static List<CoachModel> profile = new List<CoachModel>();
        public IActionResult Index(int CoachId)
        {
            ProfileDAO profile = new ProfileDAO();
            List<CoachModel> coachProfile = profile.GetCoachProfileById(CoachId);
            return View(coachProfile);
        }
    }
}


//WorkoutDAO report = new WorkoutDAO();
//List<ClientWeeklyReportModel> weeklyReport = report.GetAllWeeklyReportsByUserId(ClientUserId);

//return View("ClientReport", weeklyReport);