using Microsoft.CodeAnalysis.CSharp.Syntax;
using PTManagementSystem.Models;

namespace PTManagementSystem.Helpers
{
    public class ClientHelper
    {
        public static List<ClientModel> GetClients()
        {
            List<ClientModel> clients = new();
            clients.Add(new ClientModel { ClientFirstName="Priya"});

            return clients;
        }
    }
}
