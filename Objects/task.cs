using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Task
{
  private int _id;
  private string _description;
  private int _categoryId;
  private DateTime _duedate;

  public Task(string Description, int CategoryId, DateTime DueDate, int Id = 0)
  {
    _id = Id;
    _description = Description;
    _categoryId = CategoryId;
    _duedate = DueDate;
  }
    public DateTime GetDueDate()
    {
    return _duedate;
    }
    public void SetDueDate(DateTime newDueDate)
    {
      _duedate = newDueDate;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
    }

    public override bool Equals(System.Object otherTask)
    {
     if (!(otherTask is Task))
     {
       return false;
     }
     else
     {
       Task newTask = (Task) otherTask;
       bool idEquality = this.GetId() == newTask.GetId();
       bool descriptionEquality = this.GetDescription() == newTask.GetDescription();
       bool categoryEquality = this.GetCategoryId() == newTask.GetCategoryId();
       return (idEquality && descriptionEquality && categoryEquality);
     }
   }
    // public override int GetHashCode()
    // {
    //   return 0;
    // }
    public int GetCategoryId()
    {
      return _categoryId;
    }
    public void SetCategoryId(int newCategoryId)
    {
      _categoryId = newCategoryId;
    }
    public static List<Task> GetAll()
    {
      List<Task> AllTasks = new List<Task>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks order by duedate ASC;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        int taskCategoryId = rdr.GetInt32(2);
        DateTime taskDueDate = rdr.GetDateTime(3).Date;
        Task newTask = new Task(taskDescription, taskCategoryId, taskDueDate, taskId);
        AllTasks.Add(newTask);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllTasks;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description, category_id, duedate) OUTPUT INSERTED.id VALUES (@TaskDescription, @TaskCategoryId, @DueDate);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@TaskDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@TaskCategoryId";
      categoryIdParameter.Value = this.GetCategoryId();

      SqlParameter duedateParameter = new SqlParameter();
      duedateParameter.ParameterName = "@DueDate";
      duedateParameter.Value = this.GetDueDate();

      cmd.Parameters.Add(duedateParameter);
      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(categoryIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;
      int foundTaskCategoryId = 0;
      DateTime foundDueDate = new DateTime();
      
      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
        foundTaskCategoryId = rdr.GetInt32(2);
        foundDueDate = rdr.GetDateTime(3).Date;
      }
      Task foundTask = new Task(foundTaskDescription, foundTaskCategoryId, foundDueDate, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundTask;
    }
  }
}
