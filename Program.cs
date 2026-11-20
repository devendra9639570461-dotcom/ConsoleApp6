using System;
using System.Data;
using System.Data.SqlClient;

namespace LabAssignment6
{
    class Program
    {
        // SQL Server Connection String
        static string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=StudentDB;
              Integrated Security=True;";

        static void Main(string[] args)
        {
            int choice;

            do
            {
                Console.WriteLine("\n===== LAB ASSIGNMENT 6 =====");
                Console.WriteLine("ADO.NET Connected & Disconnected Architecture");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("1. Display Records (Connected)");
                Console.WriteLine("2. Insert Record (Connected)");
                Console.WriteLine("3. Update Record (Connected)");
                Console.WriteLine("4. Delete Record (Connected)");
                Console.WriteLine("5. Display Records (Disconnected)");
                Console.WriteLine("6. Insert Record (Disconnected)");
                Console.WriteLine("7. Update Record (Disconnected)");
                Console.WriteLine("8. Delete Record (Disconnected)");
                Console.WriteLine("9. CRUD using SqlCommandBuilder");
                Console.WriteLine("10. Compare Both Architectures");
                Console.WriteLine("0. Exit");
                Console.Write("\nEnter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid choice!");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            DisplayConnected();
                            break;

                        case 2:
                            InsertConnected();
                            break;

                        case 3:
                            UpdateConnected();
                            break;

                        case 4:
                            DeleteConnected();
                            break;

                        case 5:
                            DisplayDisconnected();
                            break;

                        case 6:
                            InsertDisconnected();
                            break;

                        case 7:
                            UpdateDisconnected();
                            break;

                        case 8:
                            DeleteDisconnected();
                            break;

                        case 9:
                            CommandBuilderCRUD();
                            break;

                        case 10:
                            CompareArchitecture();
                            break;

                        case 0:
                            Console.WriteLine("Program Closed.");
                            break;

                        default:
                            Console.WriteLine("Invalid Choice!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

            } while (choice != 0);

            Console.ReadLine();
        }

        // QUESTION 1:
        // Display all records using SqlConnection,
        // SqlCommand and SqlDataReader.

        static void DisplayConnected()
        {
            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Student";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                Console.WriteLine("\nStudent Records:");
                Console.WriteLine("--------------------------------");

                while (dr.Read())
                {
                    Console.WriteLine(
                        "ID: " + dr["StudentID"] +
                        " | Name: " + dr["Name"] +
                        " | Age: " + dr["Age"] +
                        " | Course: " + dr["Course"]);
                }

                dr.Close();
            }
        }

        // QUESTION 2:
        // Insert a new student using Connected Architecture.

        static void InsertConnected()
        {
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Course: ");
            string course = Console.ReadLine();

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                string query =
                    "INSERT INTO Student(Name, Age, Course) " +
                    "VALUES(@Name, @Age, @Course)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Course", course);

                con.Open();

                int result = cmd.ExecuteNonQuery();

                Console.WriteLine(result > 0
                    ? "Student Inserted Successfully!"
                    : "Insert Failed!");
            }
        }

        // QUESTION 3:
        // Update student details using Connected Architecture.

        static void UpdateConnected()
        {
            Console.Write("Enter Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter New Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Course: ");
            string course = Console.ReadLine();

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                string query =
                    "UPDATE Student SET Name=@Name, Age=@Age, " +
                    "Course=@Course WHERE StudentID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Course", course);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();

                int result = cmd.ExecuteNonQuery();

                Console.WriteLine(result > 0
                    ? "Student Updated Successfully!"
                    : "Student ID Not Found!");
            }
        }

        // QUESTION 4:
        // Delete student using Connected Architecture.

        static void DeleteConnected()
        {
            Console.Write("Enter Student ID to Delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                string query =
                    "DELETE FROM Student WHERE StudentID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();

                int result = cmd.ExecuteNonQuery();

                Console.WriteLine(result > 0
                    ? "Student Deleted Successfully!"
                    : "Student ID Not Found!");
            }
        }

        // QUESTION 5:
        // Display records using SqlDataAdapter and DataSet.

        static void DisplayDisconnected()
        {
            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Student";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, con);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                Console.WriteLine("\nStudent Records:");

                foreach (DataRow row in ds.Tables["Student"].Rows)
                {
                    Console.WriteLine(
                        "ID: " + row["StudentID"] +
                        " | Name: " + row["Name"] +
                        " | Age: " + row["Age"] +
                        " | Course: " + row["Course"]);
                }
            }
        }

        // QUESTION 6:
        // Insert record using DataSet and SqlDataAdapter.

        static void InsertDisconnected()
        {
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Course: ");
            string course = Console.ReadLine();

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                SqlDataAdapter da =
                    new SqlDataAdapter("SELECT * FROM Student", con);

                SqlCommandBuilder builder =
                    new SqlCommandBuilder(da);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                DataRow row = ds.Tables["Student"].NewRow();

                row["Name"] = name;
                row["Age"] = age;
                row["Course"] = course;

                ds.Tables["Student"].Rows.Add(row);

                da.Update(ds, "Student");

                Console.WriteLine(
                    "Student Inserted Successfully using DataSet!");
            }
        }

        // QUESTION 7:
        // Update record using DataSet and SqlDataAdapter.

        static void UpdateDisconnected()
        {
            Console.Write("Enter Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter New Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Course: ");
            string course = Console.ReadLine();

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                SqlDataAdapter da =
                    new SqlDataAdapter("SELECT * FROM Student", con);

                SqlCommandBuilder builder =
                    new SqlCommandBuilder(da);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                DataRow row =
                    ds.Tables["Student"].Rows.Find(id);

                if (row == null)
                {
                    Console.WriteLine("Student ID Not Found!");
                    return;
                }

                row["Name"] = name;
                row["Age"] = age;
                row["Course"] = course;

                da.Update(ds, "Student");

                Console.WriteLine(
                    "Student Updated Successfully using DataSet!");
            }
        }

        // QUESTION 8:
        // Delete record using DataSet and SqlDataAdapter.

        static void DeleteDisconnected()
        {
            Console.Write("Enter Student ID to Delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                SqlDataAdapter da =
                    new SqlDataAdapter("SELECT * FROM Student", con);

                SqlCommandBuilder builder =
                    new SqlCommandBuilder(da);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                DataRow row =
                    ds.Tables["Student"].Rows.Find(id);

                if (row == null)
                {
                    Console.WriteLine("Student ID Not Found!");
                    return;
                }

                row.Delete();

                da.Update(ds, "Student");

                Console.WriteLine(
                    "Student Deleted Successfully using DataSet!");
            }
        }

        // QUESTION 9:
        // CRUD operations using SqlCommandBuilder.

        static void CommandBuilderCRUD()
        {
            Console.WriteLine("\n--- SqlCommandBuilder CRUD ---");
            Console.WriteLine("1. Insert");
            Console.WriteLine("2. Update");
            Console.WriteLine("3. Delete");
            Console.Write("Enter Choice: ");

            int ch = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                SqlDataAdapter da =
                    new SqlDataAdapter("SELECT * FROM Student", con);

                SqlCommandBuilder builder =
                    new SqlCommandBuilder(da);

                DataSet ds = new DataSet();

                da.Fill(ds, "Student");

                DataTable table = ds.Tables["Student"];

                if (ch == 1)
                {
                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter Age: ");
                    int age = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter Course: ");
                    string course = Console.ReadLine();

                    DataRow row = table.NewRow();

                    row["Name"] = name;
                    row["Age"] = age;
                    row["Course"] = course;

                    table.Rows.Add(row);

                    da.Update(ds, "Student");

                    Console.WriteLine("Record Inserted!");
                }
                else if (ch == 2)
                {
                    Console.Write("Enter Student ID: ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    DataRow row = table.Rows.Find(id);

                    if (row == null)
                    {
                        Console.WriteLine("Record Not Found!");
                        return;
                    }

                    Console.Write("Enter New Name: ");
                    row["Name"] = Console.ReadLine();

                    Console.Write("Enter New Age: ");
                    row["Age"] = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter New Course: ");
                    row["Course"] = Console.ReadLine();

                    da.Update(ds, "Student");

                    Console.WriteLine("Record Updated!");
                }
                else if (ch == 3)
                {
                    Console.Write("Enter Student ID: ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    DataRow row = table.Rows.Find(id);

                    if (row == null)
                    {
                        Console.WriteLine("Record Not Found!");
                        return;
                    }

                    row.Delete();

                    da.Update(ds, "Student");

                    Console.WriteLine("Record Deleted!");
                }
                else
                {
                    Console.WriteLine("Invalid Choice!");
                }
            }
        }

        // QUESTION 10:
        // Complete CRUD using both architectures
        // and comparison of both approaches.

        static void CompareArchitecture()
        {
            Console.WriteLine("\n===== ARCHITECTURE COMPARISON =====");

            Console.WriteLine("\nCONNECTED ARCHITECTURE");
            Console.WriteLine("1. Uses SqlConnection.");
            Console.WriteLine("2. Uses SqlCommand.");
            Console.WriteLine("3. Uses SqlDataReader.");
            Console.WriteLine("4. Requires active connection.");
            Console.WriteLine("5. Suitable for real-time operations.");

            Console.WriteLine("\nDISCONNECTED ARCHITECTURE");
            Console.WriteLine("1. Uses SqlDataAdapter.");
            Console.WriteLine("2. Uses DataSet and DataTable.");
            Console.WriteLine("3. Connection is not required during data processing.");
            Console.WriteLine("4. Changes are saved using Update().");
            Console.WriteLine("5. Suitable for offline data processing.");

            Console.WriteLine("\n===== CRUD OPERATIONS =====");

            Console.WriteLine("Connected Architecture:");
            Console.WriteLine("Create  - INSERT using ExecuteNonQuery()");
            Console.WriteLine("Read    - SELECT using ExecuteReader()");
            Console.WriteLine("Update  - UPDATE using ExecuteNonQuery()");
            Console.WriteLine("Delete  - DELETE using ExecuteNonQuery()");

            Console.WriteLine("\nDisconnected Architecture:");
            Console.WriteLine("Create  - Add DataRow");
            Console.WriteLine("Read    - DataAdapter.Fill()");
            Console.WriteLine("Update  - Modify DataRow");
            Console.WriteLine("Delete  - DataRow.Delete()");
            Console.WriteLine("Save    - DataAdapter.Update()");

            Console.WriteLine("\nBoth architectures support complete CRUD operations.");
        }
    }
}
