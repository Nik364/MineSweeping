﻿using Microsoft.AspNet.SignalR;
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
        /// <param name="height"></param>
        /// <param name="mineNum"></param>
        [HubMethodName("start")]
        public void Start(int width, int height, int mineNum)
        {
            if (game == null)
            {
                game = new Mine(width, height, mineNum);
            }

            game.Start();
            // 只通知请求开始的客户端
            //Clients.Caller.start(width, height, game.Map);
            Clients.All.start(width, height, game.Map);
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

            if (!game.IsPalying)
            {
                this.GameOver();
            }
        }

        /// <summary>
        /// 游戏重置
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mineNum"></param>
        [HubMethodName("reset")]
        public void Reset(int width, int height, int mineNum)
        {
            game = null;
            this.Start(width, height, mineNum);
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        private void GameOver()
        {
            Clients.All.gameOver();
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