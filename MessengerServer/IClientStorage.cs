using MessengerShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessengerServer
{
    internal interface IClientStorage
    {
        public ClientData GetClient(string UID);

        public void SaveClient(ClientData user);
    }
}
