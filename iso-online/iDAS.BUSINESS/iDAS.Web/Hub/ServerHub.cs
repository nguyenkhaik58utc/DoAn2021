using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using iDAS.Core;
namespace iDAS.Web
{
    public class ServerHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        private string getKey()
        {
            var user = Context.User as UserPrincipal;
            var key = string.Empty;
            if (user != null)
            {
                key = user.Code + ',' + user.EmployeeID.ToString();
            }
            return key;
        }

        public void Notify(string key,int id)
        {
            var connectionIDs = _connections.GetConnections(key);
            foreach (var connectionId in connectionIDs)
            {
                Clients.Client(connectionId).notify();
            }
        }

        public void Chat(string key,int id) 
        {
            var connectionIDs = _connections.GetConnections(key);
            foreach (var connectionId in connectionIDs)
            {
                Clients.Client(connectionId).chat(id);
            }
        }

        public void ChatOnline()
        {
            var user = Context.User as UserPrincipal;
            if (user != null)
            {
                var keys = GetKeys(user.Code);
                foreach (var key in keys)
                {
                    var connectionId = _connections.GetConnections(key).FirstOrDefault();
                    Clients.Client(connectionId).chatOnline();
                }
            }
        }

        public void ChatOffline()
        {
            var user = Context.User as UserPrincipal;
            if (user != null)
            {
                var keys = GetKeys(user.Code);
                foreach (var key in keys)
                {
                    var connectionId = _connections.GetConnections(key).FirstOrDefault();
                    Clients.Client(connectionId).chatOffline();
                }
            }
        }

        public List<string> GetKeys(string code) 
        {
            return _connections.GetKeyByCode(code);
        }

        public List<int> GetEmployeeIDs(string code)
        {
            var source = _connections.GetKeyByCode(code)
                        .Select(i => Convert.ToInt32(i.Replace(code + ',', "")))
                        .ToList();
            return source;
        }

        public override Task OnConnected()
        {
            var key = getKey();
            if (!string.IsNullOrEmpty(key))
            {
                ChatOnline();
                _connections.Add(key, Context.ConnectionId);
            }
            
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            var key = getKey();
            if (!string.IsNullOrEmpty(key))
            {
                _connections.Remove(key, Context.ConnectionId);
                ChatOffline();
            }
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            var key = getKey();
            if (!string.IsNullOrEmpty(key) && !_connections.GetConnections(key).Contains(Context.ConnectionId))
            {
                _connections.Add(key, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}