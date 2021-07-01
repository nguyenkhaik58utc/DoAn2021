using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace iDAS.Web
{
    /// <summary>
    /// Manager Notify
    /// </summary>
    public class ClientHub
    {
        public static void Notify(string code, List<int> employeeIDs,int id=0)
        {
            callFunction(code, employeeIDs, "Notify", id);
        }
        public static void Chat(string code, List<int> employeeIDs, int id=0)
        {
            callFunction(code, employeeIDs, "Chat", id);
        }

        private static IHubProxy hubProxy;
        private static void initHubProxy() { 
            if( hubProxy ==null){
                var request = HttpContext.Current.Request;
                var hubConnection = new HubConnection(request.Url.Scheme + "://" + request.Url.Authority);
                hubProxy = hubConnection.CreateHubProxy("ServerHub");
                hubConnection.Start().Wait();    
            }
        }
        private static void callFunction(string code, List<int> employeeIDs, string function, int id=0) 
        {
            initHubProxy();
            foreach (var employeeID in employeeIDs)
            {
                var key = code + ',' + employeeID.ToString();
                hubProxy.Invoke(function, key, id);
            }
        }
    }
}