using PTManagementSystem.Models;

namespace PTManagementSystem.Services
{
    public interface IProfileDataService
    {
        List<CoachModel> GetCoachProfileById(int CoachId);
        //List<ClientModel> SearchClients(string searchTerm);

        //ClientModel GetClientById(int ClientId);

    }
}
