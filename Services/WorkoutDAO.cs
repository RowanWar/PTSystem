using Npgsql;
using PTManagementSystem.Models;

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

                bool foundWorkout = false;
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
                                foundWorkout = true; // P sure this is redundant
                                while (result.Read())
                                {
                                    //val = (int)result.GetValue(0);
                                    foundWorkouts.Add(new WorkoutModel
                                    {
                                        WorkoutId = (int)result["workout_id"],
                                        UserId = (int)result["user_id"],
                                        WorkoutDate = (DateTime)result["workout_date"],
                                        WorkoutDuration = (TimeSpan)result["duration"],
                                        WorkoutCreatedAt = (DateTime)result["created_at"]

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
                                        sc.set_category_type
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
                                        SetReps = (int)result["reps"],
                                        SetCategory = (string)result["set_category_type"]

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
    }
}
