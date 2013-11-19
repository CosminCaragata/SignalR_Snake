using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SingleRDemo
{
    public abstract class SnakeGameRunner : SnakeGame
    {
        /// <summary>
        /// Contructor of game
        /// </summary>
        /// <param name="sizex">Inits the size x of the table</param>
        /// <param name="sizey">Inits the size Y of the table</param>
        public SnakeGameRunner(int sizex, int sizey)
            : base(sizex, sizey)
        {
            GameStatus = SnakeGameStatus.Stopped;
        }

        /// <summary>
        ///  Method that handles how the gamestate is sent to the users
        /// </summary>
        public abstract void SendToUsersGameState();
        public SnakeGameStatus GameStatus;


        /// <summary>
        /// The while loop that runs the core of the game.Only one instance per server.
        /// </summary>
        public void RunGameCore()
        {
            if (GameStatus == SnakeGameStatus.Running)
                return;
            var sg = HttpContext.Current.Application["SnakeGame"] as SnakeGame;
            GameStatus = SnakeGameStatus.Running;
            while (true && GameStatus != SnakeGameStatus.BeingStopped)
            {
                if (HttpContext.Current.Application["SnakeGameStarted"] != "stop")
                {
                    foreach (var CurrentSnake in sg.SnakePlayers.Where(x=>x.PlayerStatus == SnakePlayerStatus.Alive))
                    {

                        if (!SnakeHeadsTowardsTail(CurrentSnake, CurrentSnake.Direction))
                        {
                            ContinueMoveSnake(CurrentSnake);
                        }
                        else
                        {
                            if (CurrentSnake.Direction != SnakeDirection.Top)
                            {
                                sg.MoveSnakeTop(CurrentSnake);
                            }
                            else
                            {
                                sg.MoveSnakeBottom(CurrentSnake);
                            }
                        }

                        if (SnakeCollidedWithSomething(CurrentSnake))
                        {
                            KillSnake(CurrentSnake);
                        }
                    }
                  

                    SendToUsersGameState();
                    Thread.Sleep(100);                    
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Method that stops the game core - stops the game.
        /// </summary>
        public void StopGameCore()
        {
            if (GameStatus == SnakeGameStatus.Running)
            {
                GameStatus = SnakeGameStatus.BeingStopped;
            }
        }

    }
}