using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingleRDemo
{
    public class SnakeGameRunnerSignalR : SnakeGameRunner
    {

        public HubConnectionContext Clients;

        public SnakeGameRunnerSignalR(int sizex, int sizey, HubConnectionContext clients ) : base(sizex,sizey)
        {
            Clients = clients;
        }

        public override void SendToUsersGameState() 
        {            
            var SnakeGame = (HttpContext.Current.Application["SnakeGame"] as SnakeGame);
            Clients.All.showTableOnJavascript(SnakeGame.GetGameTable(), SnakeGame.SnakePlayers);
        }

        
    }
}