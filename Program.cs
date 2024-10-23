using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

class Program
{
    private static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=EFCoreDB;Trusted_Connection=True;";
    static void Main(string[] args)
    {
        AddTask(new TaskModel { Title = "New Task 1", Description = "New Description 1", DueDate = DateTime.Now, IsCompleted = false });
        AddTask(new TaskModel { Title = "New Task 2", Description = "New Description 2", DueDate = DateTime.Now, IsCompleted = true });
        AddTask(new TaskModel { Title = "New Task 3", Description = "New Description 3", DueDate = DateTime.Now, IsCompleted = false });
        DeleteTask(3);
        var tasks = GetTasks();
        UpdateTask(new TaskModel { Id = 2, Title = "Updated", Description = "Updated description", DueDate = DateTime.Now.AddDays(1), IsCompleted = true });
    }

    static void AddTask(TaskModel task)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery = """
            INSERT INTO Tasks (Title, Description, DueDate, IsCompleted)
                   VALUES (@Title, @Description, @DueDate, @IsCompleted);
            SELECT SCOPE_IDENTITY()
            """;
            db.Execute(sqlQuery, task);
        }
    }

    static void DeleteTask(int id)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery = """
            DELETE FROM Tasks WHERE Id = @id;
            """;
            db.Execute(sqlQuery, new { id });
        }
    }
    static IEnumerable<TaskModel> GetTasks()
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery = """
            SELECT * FROM Tasks;
            """;
            return db.Query<TaskModel>(sqlQuery);
        }
    }

    static void UpdateTask(TaskModel task)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery = """
            UPDATE Tasks
            SET Title = @Title, Description = @Description, DueDate = @DueDate, IsCompleted = @IsCompleted
            WHERE Id = @Id
            """;
            db.Execute(sqlQuery, task);
        }
    }
}

public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}