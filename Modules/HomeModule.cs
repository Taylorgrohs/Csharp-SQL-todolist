using Nancy;
using ToDoList;
using System.Collections.Generic;
using System;

namespace ToDoProj
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
      return  View ["index.cshtml"];
    };
      Get["/other_page"] = _ => {
      return View["template.cshtml"];
      };
    }
  }
}
