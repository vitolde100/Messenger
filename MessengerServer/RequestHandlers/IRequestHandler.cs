using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerServer.RequestHandlers
{
    internal interface IRequestHandler
    {
        string RequestType { get; }
        string Data { get; }
    }
}
