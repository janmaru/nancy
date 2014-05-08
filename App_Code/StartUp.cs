using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StartUp
/// </summary>
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.UseNancy(); //use Nancy!!!
        ItemsRepository.create(); //create our repository
        UsersRepository.create(); //load users
    }
}