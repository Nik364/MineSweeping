using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Nik.MineSweeping.Hubs
{
    [HubName("mine")]
    public class MineHub : Hub
    {
        [HubMethodName("hello")]
        public void Hello()
        {
            Clients.All.hello("lalalla");
        }
    }
}