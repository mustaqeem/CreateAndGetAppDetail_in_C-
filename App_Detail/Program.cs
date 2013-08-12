using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plivo.API;
using RestSharp;
using dict = System.Collections.Generic.Dictionary<string, string>;
using System.Reflection;

namespace App_Detail
{
    class Program
    {
        static void Main(string[] args)
        {
            RestAPI r = new RestAPI("XXXXXXXXXXXXXXXXXXXXXXXXXX", "YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
            try
            {
                Application app = CreateAndGetDetailApplication(r, new dict { { "app_name", "lkjg" }, { "answer_url", "http://answer_url" } });
                if (app != null)
                {
                    Console.WriteLine("Answer Method: " + app.answer_method);
                    Console.WriteLine("Answer Url: " + app.answer_url);
                    Console.WriteLine("App Id: " + app.app_id);
                    Console.WriteLine("App Name: " + app.app_name);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
            Console.ReadKey();
        }
        static ApplicationList GetApplicationList(RestAPI r)
        {
            IRestResponse<ApplicationList> resp = r.get_applications();
            return resp.Data;
        }
        static Application GetApplicationDetailByName(RestAPI r, string appname)
        {
            ApplicationList applist = GetApplicationList(r);            
            foreach (Application app in applist.objects)
            {
                if (app.app_name == appname)
                {
                    return app;
                }
            }
            return null;
        }
        static Application CreateAndGetDetailApplication(RestAPI r, dict parameters)
        {
            IRestResponse<GenericResponse> resp = r.create_application(parameters);
            if (resp.Data != null)
            {
                PropertyInfo[] proplist = resp.Data.GetType().GetProperties();
                if (parameters["app_name"] == null)
                    return GetApplicationDetailByName(r, parameters["app_name"]);
                else throw new PlivoException("An application with same name already exist");
            }
            else
            {
                Console.WriteLine("------------->"+resp.ErrorMessage);
            }
            return null;
        }
    }
}
