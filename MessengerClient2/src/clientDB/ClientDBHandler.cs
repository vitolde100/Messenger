using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessengerClient2.src.clientDB
{
    internal static class ClientDBHandler
    {
        public  class HandledData
        {
            public string serverIp { get; set; }
            public string serverPort { get; set; }
            public string login { get; set; }
            public string password { get; set; }
        }
        public static HandledData data { get; set; }

        public static bool IsDBExists()
        {
            return File.Exists("userdata.json");
        }

        public static void Save()
        {
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText("userdata.json", jsonString);
        }
        public static void Load()
        {
            string jsonString = File.ReadAllText("userdata.json");
            data = JsonSerializer.Deserialize<HandledData>(jsonString);
        }
        public static void Delete()
        {
            File.Delete("userdata.json");
        }
    }
}
