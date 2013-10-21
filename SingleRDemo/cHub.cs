using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SingleRDemo
{
    public class CHub : Hub
    {
        public void ShowGameBoard()
        {
            Clients.All.showTableOnJavascript((HttpContext.Current.Application["SnakeGame"] as SnakeGame).GetGameTable());
        }


        public void InitiateGameBoard(int i = 50, int j = 50)
        {
            SnakeGame sg = new SnakeGame(i, j);
            sg.AddPlayerToBoard();
            HttpContext.Current.Application["SnakeGame"] = sg;
        }


        public void MoveSnakes()
        {
            var sg = (HttpContext.Current.Application["SnakeGame"] as SnakeGame);
            sg.MoveSnakeTop(sg.SnakePlayers.First());
            //needs implementation
            ShowGameBoard();
        }


        public void RunGameCore()
        {
            HttpContext.Current.Application["SnakeGameStarted"] = "start";
            while (true)
            {
                if (HttpContext.Current.Application["SnakeGameStarted"] != "stop")
                {
                    var sg = (HttpContext.Current.Application["SnakeGame"] as SnakeGame);
                    sg.MoveSnakeTop(sg.SnakePlayers.First());
                    System.Threading.Thread.Sleep(500);
                    Clients.All.showTableOnJavascript((HttpContext.Current.Application["SnakeGame"] as SnakeGame).GetGameTable());
                }
                else { break; }
            }
        }

        public void StopGameCore()
        {
            HttpContext.Current.Application["SnakeGameStarted"] = "stop";
        }

    }
}