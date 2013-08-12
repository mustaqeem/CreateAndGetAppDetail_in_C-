using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plivo.API;
using RestSharp;
using dict = System.Collections.Generic.Dictionary<string, string>;
using System.Reflection;
using System.Threading;

namespace App_Detail
{
    class Program
    {
        static void Main(string[] args)
        {
            RestAPI r = new RestAPI("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", "YYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");      
            try
            {
                Application app = CreateAndGetDetailApplication(r, new dict { { "app_name", "appname" }, { "answer_url", "http://answer_url" } });
                if (app != null)
                {
                    Console.WriteLine("App Id: " + app.app_id);
                    Console.WriteLine("SIP URI: " + app.sip_uri);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: "+ex.Message);
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
            bool flag = false;
            if (resp.Data != null)
            {
                PropertyInfo[] proplist = resp.Data.GetType().GetProperties();
                foreach (PropertyInfo property in proplist)
                {
                    string er = resp.Data.error;
                    if (property.Name == "error" && er!= null)
                    {
                        flag = true;
                    }
                }
                Thread.Sleep(500);
                if(flag)
                    throw new PlivoException("An application with same name already exist");    
                return GetApplicationDetailByName(r, parameters["app_name"]);
            }
            else
            {
                throw new PlivoException(resp.ErrorMessage);
            }
        }
    }
}
