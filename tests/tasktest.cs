using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Task.DeleteAll();
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      DateTime time = new DateTime(2016, 08, 08);
      //Arrange, Act
      Task firstTask = new Task("Mow the lawn", time);
      Task secondTask = new Task("Mow the lawn", time);

      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save()
    {
        DateTime time = new DateTime(2016, 08, 08);
      //Arrange
      Task testTask = new Task("Mow the lawn", time);
      testTask.Save();

      //Act
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
        DateTime time = new DateTime(2016, 08, 08);
      Task testTask = new Task("Mow the lawn", time);
      testTask.Save();

      //Act
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsTaskInDatabase()
    {
        DateTime time = new DateTime(2016, 08, 08);
      //Arrange
      Task testTask = new Task("Mow the lawn", time);
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }

    [Fact]
    public void Test_Delete_DeletesTaskAssociationsFromDatabase()
    {
      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      string testDescription = "Mow the lawn";
      DateTime time = new DateTime(2016, 08, 08);
      Task testTask = new Task(testDescription, time);
      testTask.Save();

      testTask.AddCategory(testCategory);
      testTask.Delete();

      List<Task> resultCategoryTasks = testCategory.GetTasks();
      List<Task> testCategoryTasks = new List<Task> {};

      Assert.Equal(testCategoryTasks, resultCategoryTasks);
    }
  }
}
