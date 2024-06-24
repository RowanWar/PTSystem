using Npgsql;
using PTManagementSystem.Models;

namespace PTManagementSystem.Services
{
    public class ImageDAO
    {
        string dbConnectionString = @"Host=localhost;Username=postgres;Password=BeBetter300;Database=ptsystem;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20;";

        public List<ClientModel> GetImageById()
        {
            List<ClientModel> foundClients = new List<ClientModel>();

            bool foundUsers = false;
            string sqlStatement = "SELECT * FROM users";


            using (var connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();


                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        var result = cmd.ExecuteReader();
                        //int val;

                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {
                            foundUsers = true;
                            while (result.Read())
                            {
                                //val = (int)result.GetValue(0);
                                foundClients.Add(new ClientModel
                                {
                                    ClientUserId = (int)result["user_id"],
                                    ClientUsername = (string)result["username"],
                                    ClientFirstName = (string)result["firstname"],
                                    ClientLastName = (string)result["lastname"],
                                    ClientEmail = (string)result["email"],
                                    ClientPhone = (string)result["mobile_number"]

                                });

                                //result.GetString(1));

                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No rows found.");

                        }

                        result.Close();
                        //return foundClients;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that might have occurred
                    System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

                }

                return foundClients;
            }
        }
    }
}
