using PTManagementSystem.Models;
using ptmanagementsystem.services;

namespace PTManagementSystem.Services
{
    public class SecurityService
    {
        SecurityDAO securityDAO = new SecurityDAO();

        public SecurityService()
        {

        }

        public bool IsValid(UserLogin user)
        {
            return SecurityDAO.FindUserByNameAndPassword(user);

        }
    }
}
