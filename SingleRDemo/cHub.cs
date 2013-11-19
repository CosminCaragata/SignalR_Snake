using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SingleRDemo
{
    /// <summary>
    ///  The hub(server) where the SignalR clients connect.
    /// </summary>
    public class CHub : Hub
    {
        public void ShowGameBoard()
        {
            Clients.All.showTableOnJavascript((HttpContext.Current.Application["SnakeGame"] as SnakeGame).GetGameTable());
        }

        //public void InitiateGameBoard()
        //{
        //    SnakeGameRunnerSignalR sg = new SnakeGameRunnerSignalR(50, 50, Clients);
        //    sg.AddPlayerToBoard();
        //    HttpContext.Current.Application["SnakeGame"] = (SnakeGame)sg;
        //    //RunGameCore();

        //}

        //public void InitiateGameBoard(int i = 50, int j = 50)
        //{

        //}


        public void MoveSnakes()
        {
            var sg = HttpContext.Current.Application["SnakeGame"] as SnakeGame;
            sg.MoveSnakeTop(sg.SnakePlayers.First());
            // needs implementation
            this.ShowGameBoard();
        }


        public void RunGameCore(string x, string y)
        {
            SnakeGameRunnerSignalR sg;
            try
            {
                sg = new SnakeGameRunnerSignalR(int.Parse(x),int.Parse(y), Clients);
            }
            catch
            {
                sg = new SnakeGameRunnerSignalR(50, 50, Clients);
            }
            //sg.AddPlayerToBoard(Guid.NewGuid());
            HttpContext.Current.Application["SnakeGame"] = (SnakeGame)sg;
            sg.RunGameCore();

        }

        public void AddNewSnake(string playername)
        {            
            SnakeGame sg = HttpContext.Current.Application["SnakeGame"] as SnakeGame;
            sg.AddPlayerToBoard(this.Context.ConnectionId , playername);                      
        }

        public void StopGameCore()
        {
            var sg = HttpContext.Current.Application["SnakeGame"] as SnakeGame;
            var sgr = (SnakeGameRunner)sg;
            sgr.StopGameCore();
        }

        public void SetDirectionForSnake(string direction)
        {
            SnakeDirection NewDirection = SnakeDirection.Top;
            switch (direction)
            {
                case "left": NewDirection = SnakeDirection.Left; break;
                case "up": NewDirection = SnakeDirection.Top; break;
                case "right": NewDirection = SnakeDirection.Right; break;
                case "down": NewDirection = SnakeDirection.Bottom; break;
            }
            var sg = HttpContext.Current.Application["SnakeGame"] as SnakeGame;
            var ClientSnake = sg.SnakePlayers.First(x => x.PlayerConnectionId == this.Context.ConnectionId);
            sg.ChangeDirection(ClientSnake, NewDirection);            
            
        }
    }
}