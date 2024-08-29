using Npgsql;
using NuGet.Protocol.Plugins;
using PTManagementSystem.Models;
using static Azure.Core.HttpHeader;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;

namespace PTManagementSystem.Services
{
    public class WorkoutDAO : IWorkoutDataService
    {
        string dbConnectionString = @"Host=localhost;Username=postgres;Password=BeBetter30;Database=ptsystem;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20;";
        


        public List<WorkoutModel> GetAllWorkouts()
        {
            throw new NotImplementedException();
        }

        public List<WorkoutModel> GetAllWorkoutsByUserId(int UserId)
        {
            {
                List<WorkoutModel> foundWorkouts = new List<WorkoutModel>();

                string sqlStatement = "SELECT * FROM workout WHERE user_id = @UserId";


                using (var connection = new NpgsqlConnection(dbConnectionString))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();


                        // Create a command object
                        using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserId", UserId);
                            var result = cmd.ExecuteReader();

                            System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                            if (result.HasRows)
                            {
                                while (result.Read())
                                {
                                    //val = (int)result.GetValue(0);
                                    foundWorkouts.Add(new WorkoutModel
                                    {
                                        WorkoutId = (int)result["workout_id"],
                                        UserId = (int)result["user_id"],
                                        WorkoutDate = (DateTime)result["workout_date"],
                                        Duration = (TimeSpan)result["duration"],
                                        CreatedAt = (DateTime)result["created_at"]

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

                    return foundWorkouts;
                }
            }
        }



        public List<WorkoutExercisesModel> GetWorkoutDetailsByWorkoutId(int WorkoutId)
        {
            {
                List<WorkoutExercisesModel> workoutDetails = new List<WorkoutExercisesModel>();


                string sqlStatement = @"SELECT 
                                        e.exercise_name,
                                        e.muscle_group,
                                        e.description,
                                        s.set_id,
                                        s.reps,
                                        s.weight,
                                        s.starttime,
                                        s.endtime,
                                        sc.set_category_type,
                                        we.workout_exercise_id
                                    FROM 
                                        exercise e
                                    JOIN 
                                        workout_exercise we ON e.exercise_id = we.exercise_id
                                    JOIN 
                                        set s ON we.workout_exercise_id = s.workout_exercise_id
                                    JOIN 
                                        set_category sc ON s.set_category_id = sc.set_category_id
                                    WHERE 
                                        we.workout_id = @WorkoutId
                                    ORDER BY e.exercise_name;";


                using (var connection = new NpgsqlConnection(dbConnectionString))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();


                        // Create a command object
                        using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                        {
                            cmd.Parameters.AddWithValue("@WorkoutId", WorkoutId);
                            var result = cmd.ExecuteReader();

                            System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                            if (result.HasRows)
                            {

                                while (result.Read())
                                {
                                    //val = (int)result.GetValue(0);
                                    workoutDetails.Add(new WorkoutExercisesModel
                                    {
                                        ExerciseName = (string)result["exercise_name"],
                                        MuscleGroup = (string)result["muscle_group"],
                                        ExerciseDescription = (string)result["description"],
                                        Reps = (int)result["reps"],
                                        SetCategory = (string)result["set_category_type"],
                                        ExerciseGroupId = (int)result["workout_exercise_Id"],
                                        Weight = (decimal)result["weight"],
                                        SetId = (int)result["set_id"]
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

                    return workoutDetails;
                }
            }
        }



        public WorkoutModel GetWorkoutById(int id)
        {
            throw new NotImplementedException();
        }

        public List<WorkoutModel> SearchWorkouts(string searchTerm)
        {
            throw new NotImplementedException();
        }


        public List<ImageModel> GetAllImagesByWeeklyReportId(int ReportId)
        {
            List<ImageModel> foundImages = new List<ImageModel>();

            string sqlStatement = @"SELECT 
                                    i.image_id,    
                                    i.file_path,    
                                    wri.weekly_report_id 
                                FROM 
                                    weekly_report_image wri 
                                JOIN image i ON wri.image_id = i.image_id
                                WHERE
                                    wri.weekly_report_id = @WeeklyReportId
                                    AND 
                                    wri.date_deleted IS NULL
                                    AND
                                    i.date_deleted IS NULL;";

            //string sqlStatement = "SELECT * FROM weekly_report WHERE date_deleted is null and user_id = @UserId";

            using (var connection = new NpgsqlConnection(dbConnectionString))
            {

                try
                {
                    connection.Open();


                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        cmd.Parameters.AddWithValue("@WeeklyReportId", ReportId);
                        var result = cmd.ExecuteReader();

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                foundImages.Add(new ImageModel
                                {
                                    ImageId = (int)result["image_id"],
                                    WeeklyReportId = (int)result["weekly_report_id"],
                                    ImageFilePath = result["file_path"].ToString() // Made this toString instead of at beginning
                                });
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that might have occurred
                    System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

                }
            }
            return foundImages;
        }
     


        public List<ClientWeeklyReportModel> GetAllWeeklyReportsByUserId(int UserId)
        {
            List<ClientWeeklyReportModel> foundReports = new List<ClientWeeklyReportModel>();

                // Does not return weekly_reports if the row has been deleted by the user/coach.
                string sqlStatement = "SELECT * FROM weekly_report WHERE date_deleted is null and user_id = @UserId";


                using (var connection = new NpgsqlConnection(dbConnectionString))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();


                        // Create a command object
                        using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserId", UserId);
                            var result = cmd.ExecuteReader();
                            //int val;

                            System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                            if (result.HasRows)
                            {
                                while (result.Read())
                                {
                                //val = (int)result.GetValue(0);
                                    foundReports.Add(new ClientWeeklyReportModel
                                    {
                                        WeeklyReportId = (int)result["weekly_report_id"],
                                        UserId = (int)result["user_id"],
                                        ReportNotes = (string)result["notes"],
                                        CheckInDate = (DateTime)result["check_in_date"],
                                        CheckInWeight = (Decimal)result["check_in_weight"],
                                        DateCreated= (DateTime)result["date_created"],
                                        // Checks if DateDeleted contains a null value, if so assigns null value to the result instead.
                                        DateDeleted = result["date_deleted"] is DBNull ? (DateTime?)null: (DateTime)result["date_deleted"]
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

                    return foundReports;
                }
            
        }



        public List<WorkoutExercisesModel> GetUsersActiveWorkout(int UserId)
        {
            List<WorkoutExercisesModel> foundWorkout = new List<WorkoutExercisesModel>();

            // Only returns a workout if workout_active == true
            string sqlStatement = "SELECT * FROM workout WHERE workout_active and user_id = @UserId";


            using (var connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();


                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        var result = cmd.ExecuteReader();

                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                //val = (int)result.GetValue(0);
                                foundWorkout.Add(new WorkoutExercisesModel
                                {
                                    WorkoutId = (int)result["workout_id"],
                                    UserId = (int)result["user_id"],
                                    WorkoutDate = (DateTime)result["workout_date"],
                                    Duration = (TimeSpan)result["duration"],
                                    // Checks if notes contains a null value, if so assigns null value to the result instead.
                                    Notes = result["notes"] is DBNull ? (string?)null : (string)result["notes"],
                                    CreatedAt = (DateTime)result["created_at"],
                                    WorkoutActive = (Boolean)result["workout_active"]
                                });
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

                return foundWorkout;
            }

        }





        public async Task<List<int>> AddExercisesToDatabase(int WorkoutId, List<int> ExerciseIds)
        {
            List<WorkoutExercisesModel> foundWorkout = new List<WorkoutExercisesModel>();
            List<int> WorkoutExerciseIds = new List<int>();
            //string sqlStatement = "insert into workout_exercise (workout_exercise_id, workout_id, exercise_id, notes, created_at) values (DEFAULT, @WorkoutId, @ExerciseId, 'This has been added manually', '10/11/2022 17:50:18+0000')";
            int result = 0;

            // Opens an async db connection to allow for efficient insertions/reads in the database
            await using var dataSource = NpgsqlDataSource.Create(dbConnectionString);
            await using var connection = await dataSource.OpenConnectionAsync();


            using var batch = new NpgsqlBatch(connection);

            foreach (var ExerciseId in ExerciseIds)
            {
                batch.BatchCommands.Add(new NpgsqlBatchCommand(
                    "insert into workout_exercise (workout_id, exercise_id, notes, created_at) " +
                    "values (@WorkoutId, @ExerciseId, 'This has been added manually', '10/11/2022 17:50:18+0000') " +
                    "RETURNING workout_exercise_id")

                    {
                    Parameters =
                    {
                        new NpgsqlParameter("@WorkoutId", WorkoutId),
                        new NpgsqlParameter("@ExerciseId", ExerciseId)

                    }

                });
            }

            try
            {
                await using var reader = await batch.ExecuteReaderAsync();

                do
                {
                    while (await reader.ReadAsync())
                    {
                        WorkoutExerciseIds.Add(reader.GetInt32(0));
                    }
                } while (await reader.NextResultAsync()); // Move to the next result set

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            System.Diagnostics.Debug.WriteLine(WorkoutExerciseIds);
            return WorkoutExerciseIds;

        }





        public async Task<int> AddSetsToDatabase(List<int> WorkoutExerciseIds)
        {
            List<WorkoutExercisesModel> workoutSets = new List<WorkoutExercisesModel>();
            //List<int> WorkoutExerciseIds = new List<int>();
            //string sqlStatement = "insert into workout_exercise (workout_exercise_id, workout_id, exercise_id, notes, created_at) values (DEFAULT, @WorkoutId, @ExerciseId, 'This has been added manually', '10/11/2022 17:50:18+0000')";
            int result = 0;

            // Opens an async db connection to allow for efficient insertions/reads in the database
            await using var dataSource = NpgsqlDataSource.Create(dbConnectionString);
            await using var connection = await dataSource.OpenConnectionAsync();


            using var batch = new NpgsqlBatch(connection);

            foreach (var WorkoutExerciseId in WorkoutExerciseIds)
            {
                batch.BatchCommands.Add(new NpgsqlBatchCommand(
                    "insert into set (workout_exercise_id, set_category_id, reps, weight, starttime, endtime) " +
                    "values(@WorkoutExerciseId, 1, 0, 0, '10/11/2000 00:00', '10/11/2000 00:00')")
                {
                Parameters =
                {
                    new NpgsqlParameter("@WorkoutExerciseId", WorkoutExerciseId),
                }
                
                });
            }

            try
            {
                result = await batch.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }


            return result;

        }






        public List<WorkoutModel> CheckActiveWorkoutByUserId(int UserId)
        {
            List<WorkoutModel> foundActiveWorkout = new List<WorkoutModel>();


            string sqlStatement = "SELECT * FROM workout WHERE user_id = @UserId AND workout_active = true LIMIT 1";


            using (var connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();


                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        var result = cmd.ExecuteReader();
                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                //val = (int)result.GetValue(0);
                                foundActiveWorkout.Add(new WorkoutModel
                                {
                                    WorkoutId = (int)result["workout_id"],
                                    Duration = (TimeSpan)result["duration"],
                                    // Checks if notes contains a null value, if so assigns null value to the result instead.
                                    Notes = result["notes"] is DBNull ? (string?)null : (string)result["notes"]
                                });

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

                System.Diagnostics.Debug.WriteLine("Returning fetch request now...");
                return foundActiveWorkout;
            }

        }



        // public async Task<int> CreateWorkoutInDatabase(int UserId)
        // {
        //     List<WorkoutExercisesModel> foundWorkout = new List<WorkoutExercisesModel>();

        //     int result = 0;

        //     // Opens an async db connection to allow for efficient insertions/reads in the database
        //     await using var dataSource = NpgsqlDataSource.Create(dbConnectionString);
        //     await using var connection = await dataSource.OpenConnectionAsync();


        //     using var batch = new NpgsqlBatch(connection);

        //     foreach (var ExerciseId in ExerciseIds)
        //     {
        //         batch.BatchCommands.Add(new NpgsqlBatchCommand(
        //             "insert into workout_exercise (workout_exercise_id, workout_id, exercise_id, notes, created_at) " +
        //             "values (DEFAULT, @WorkoutId, @ExerciseId, 'This has been added manually', '10/11/2022 17:50:18+0000')")
        //         {
        //             Parameters =
        //             {
        //                 new NpgsqlParameter("@WorkoutId", WorkoutId),
        //                 new NpgsqlParameter("@ExerciseId", ExerciseId)

        //             }
        //         });
        //     }

        //     try
        //     {
        //         result = await batch.ExecuteNonQueryAsync();
        //     }
        //     catch (Exception ex)
        //     {
        //         System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
        //         throw;
        //     }


        //     return result;

        //}



        public List<ExerciseModel> GetAllExercisesByUserId()
        {
            List<ExerciseModel> foundExercises = new List<ExerciseModel>();

       
            string sqlStatement = "SELECT * FROM exercise LIMIT 300";


            using (var connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();


                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        //cmd.Parameters.AddWithValue("@UserId", UserId);
                        var result = cmd.ExecuteReader();
                        //int val;

                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                //val = (int)result.GetValue(0);
                                foundExercises.Add(new ExerciseModel
                                {
                                    ExerciseId = (int)result["exercise_id"],
                                    ExerciseName = (string)result["exercise_name"],
                                    MuscleGroup = (string)result["muscle_group"],
                                    Description = (string)result["description"],
                                    IsDefault = (bool)result["is_default"],
                                    // Checks if user_id contains a null value, if so assigns null value to the result instead.
                                    UserId = result["user_id"] is DBNull ? (int?)null : (int)result["user_id"]
                                });

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

                System.Diagnostics.Debug.WriteLine("Returning fetch request now...");
                return foundExercises;
            }

        }
    }
}
