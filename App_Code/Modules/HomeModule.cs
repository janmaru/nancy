using Nancy;
using Nancy.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HomeModule
/// </summary>
public class HomeModule : NancyModule
{ 
    public HomeModule() 
    {
        Get["/"] = _ => { 
            var model = new { title = "Son Quattro Example",
                              items =  ItemsRepository.Items }; 
            return View["index", model]; 
        }; 
    } 
}
