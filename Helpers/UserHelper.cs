using PTManagementSystem.Models;
using Npgsql;
using Microsoft.Data.SqlClient;
using System.Data;
namespace PTManagementSystem.Helpers;

public class UserHelper
{
    public static void ConnectDB()
    {
        // Define the connection string with pooling enabled
        var connectionString = "Host=localhost;Username=postgres;Password=BeBetter300;Database=ptsystem;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20;";

        // Create a connection object
        using (var conn = new NpgsqlConnection(connectionString))
        {
            try
            {
                // Open the connection
                conn.Open();

                // Define a simple query
                string sql = "SELECT * FROM users;";

                // Create a command object
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    // Execute the query and obtain the result
                    //var result = cmd.ExecuteScalar();
                    var result = cmd.ExecuteReader();
                    //Console.WriteLine($"Query result: {result}");
                    System.Diagnostics.Debug.WriteLine($"Query result: {result}");
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            System.Diagnostics.Debug.WriteLine("{0}\t{1}", result.GetInt32(0),
                                result.GetString(1));
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No rows found.");
                    }
                    result.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that might have occurred
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}