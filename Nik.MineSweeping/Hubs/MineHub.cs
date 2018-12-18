using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Nik.MineSweeping.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nik.MineSweeping.Hubs
{
    [HubName("mine")]
    public class MineHub : Hub
    {
        static ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();
        static readonly List<MineCell> historys = new List<MineCell>();
        static Mine game;

        /// <summary>
        /// 游戏开始
        /// </summary>
        /// <param name="width"></param>
        /// <param name="heigth"></param>
        /// <param name="mineNum"></param>
        [HubMethodName("start")]
        public void Start(int width, int heigth, int mineNum)
        {
            if (game == null)
            {
                game = new Mine(width, heigth, mineNum);
            }

            game.Start();
            Clients.All.start(width, heigth, game.Map);
        }

        /// <summary>
        /// 游戏点击
        /// </summary>
        /// <param name="act"></param>
        /// <param name="cell"></param>
        [HubMethodName("click")]
        public void Click(UserAction act, MineCell cell)
        {
            var result = game.Click(act, cell);
            Clients.All.click(result);
        }

        /// <summary>
        /// 链接
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            string user = Context.QueryString["name"];
            users.AddOrUpdate(Context.ConnectionId, user, (k, v) => user);

            // 可以发送用户加入信息

            return base.OnConnected();
        }

        /// <summary>
        /// 断开链接
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            users.TryRemove(Context.ConnectionId, out string user);

            // 发送用户退出消息

            return base.OnDisconnected(stopCalled);
        }
    }
}