using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingleRDemo
{
    public class SnakeGame
    {
        private int SizeX;
        private int SizeY;

        public SnakeCell[,] PlayGround;
        public List<SnakePlayer> SnakePlayers = new List<SnakePlayer>();

        public int BorderTop = 2;
        public int BorderRight = 2;
        public int BorderBottom = 2;
        public int BorderLeft = 2;

        public SnakeGame(int _SizeX, int _SizeY)
        {
            SizeX = _SizeX;
            SizeY = _SizeY;

            PlayGround = new SnakeCell[SizeX, SizeY];
            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
                {
                    PlayGround[i, j] = new SnakeCell();
                    PlayGround[i, j].PlayerId = 0;  //PlayerId = 0;
                }
            }
        }

        public void AddPlayerToBoard()
        {
            if (PlayGround == null || SizeX <= 6 || SizeY <= 6)
            {
                return;
            }

            bool ValidPlace = false;

            while (!ValidPlace)
            {
                Random random = new Random();
                int randomRow = random.Next(3, SizeX);
                int randomColumn = random.Next(3, SizeY);
                if (PlayGround[randomRow, 4].PlayerId == 0)
                {
                    StartPlayerToLocation(randomRow, randomColumn);
                }
                ValidPlace = true;
            }
        }

        private SnakeGameStatus StartPlayerToLocation(int PosX, int PosY)
        {
            if (PlayGround[PosX, PosY].PlayerId != 0 || PosX <= 3)
                return SnakeGameStatus.NotOk;

            SnakePlayer sp = new SnakePlayer();
            Random random = new Random();
            sp.PlayerId = random.Next(0, int.MaxValue);
            sp.SnakeHeadLocation = new Position(PosX, PosY);
            sp.SnakeLength = 3;
            sp.Tail.AddLast(new Position(PosX, PosY - 1));
            sp.Tail.AddLast(new Position(PosX, PosY - 2));
            PlayGround[PosX, PosY].PlayerId = sp.PlayerId;
            PlayGround[PosX, PosY - 1].PlayerId = sp.PlayerId;
            PlayGround[PosX, PosY - 2].PlayerId = sp.PlayerId;

            SnakePlayers.Add(sp);

            return SnakeGameStatus.Ok;
        }

        public int[,] GetGameTable()
        {

            int[,] GameTable = new int[SizeX, SizeY];
            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
                {
                    GameTable[i, j] = PlayGround[i, j].PlayerId;
                }
            }
            return GameTable;
        }


        public void DoTurn()
        {

        }

        public void MoveSnakeDown(SnakePlayer snakePlayer)
        {
            RemoveLastTailItemAndMoveTailToHead(snakePlayer);
            if (snakePlayer.SnakeHeadLocation.PosX < SizeX - BorderBottom - 2)
            {
                snakePlayer.SnakeHeadLocation.PosX++;
            }
            else
            {
                snakePlayer.SnakeHeadLocation.PosX = BorderTop + 1;
            }
            PlayGround[snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY].PlayerId = snakePlayer.PlayerId;
        }

        public void MoveSnakeRight(SnakePlayer snakePlayer)
        {
            RemoveLastTailItemAndMoveTailToHead(snakePlayer);
            if (snakePlayer.SnakeHeadLocation.PosY < SizeY - BorderRight -2)
            {
                snakePlayer.SnakeHeadLocation.PosY = snakePlayer.SnakeHeadLocation.PosY + 1;
            }
            else
            {
                snakePlayer.SnakeHeadLocation.PosY = BorderLeft;             
            }
            PlayGround[snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY].PlayerId = snakePlayer.PlayerId;
        }

        public void MoveSnakeTop(SnakePlayer snakePlayer)
        {
            RemoveLastTailItemAndMoveTailToHead(snakePlayer);
            if (snakePlayer.SnakeHeadLocation.PosX > BorderTop)
            {
                snakePlayer.SnakeHeadLocation.PosX = snakePlayer.SnakeHeadLocation.PosX - 1;
            }
            else
            {
                snakePlayer.SnakeHeadLocation.PosX = SizeX - BorderBottom -2 ;
            }
            PlayGround[snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY].PlayerId = snakePlayer.PlayerId;       
        }


        public void MoveSnakeLeft(SnakePlayer snakePlayer)
        {
            RemoveLastTailItemAndMoveTailToHead(snakePlayer);
            if (snakePlayer.SnakeHeadLocation.PosY > 0)
            {
                snakePlayer.SnakeHeadLocation.PosY = snakePlayer.SnakeHeadLocation.PosY - 1;
            }
            else
            {
                snakePlayer.SnakeHeadLocation.PosY = 0;
            }
            PlayGround[snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY].PlayerId = snakePlayer.PlayerId;
        }


        private void RemoveLastTailItemAndMoveTailToHead(SnakePlayer snakePlayer)
        {
            //demark on playboard the last tail
            PlayGround[snakePlayer.Tail.Last().PosX, snakePlayer.Tail.Last().PosY].PlayerId = 0;
            //renove last segment of tail
            snakePlayer.Tail.Remove(snakePlayer.Tail.Last());
            //add tail to head
            snakePlayer.Tail.AddFirst(new Position(snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY));
        }



        public void TestDisplayPlayground()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    Console.Write(PlayGround[i, j].PlayerId);
                }
                Console.WriteLine();
            }
        }


    }

    public class SnakeCell
    {
        private int _PlayerId;
        public int PlayerId
        {
            get { return _PlayerId; }
            set { _PlayerId = value; }
        }
    }


    public class SnakePlayer
    {
        public int PlayerId;
        public int SnakeLength;
        public Position SnakeHeadLocation;
        public LinkedList<Position> Tail = new LinkedList<Position>();
    }

    public class Position
    {
        public int PosX;
        public int PosY;
        public Position(int PosX, int PosY)
        {

            this.PosX = PosX;
            this.PosY = PosY;
        }
    }

    public enum SnakeGameStatus
    {
        Ok,
        NotOk
    }


}
