using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingleRDemo
{
    /// <summary>
    ///  The core of the game. Snake Game can be used only by using a type of this object   
    /// </summary>
    public class SnakeGame
    {
        private SnakeCell[,] PlayGround1;

        /// <summary>
        /// The actual table where the snakes "move"
        /// </summary>
        public SnakeCell[,] PlayGround
        {
            get { return PlayGround1; }
            set { PlayGround1 = value; }
        }

        /// <summary>
        ///  List of players in the game at one point
        /// </summary>
        public List<SnakePlayer> SnakePlayers = new List<SnakePlayer>();

        private int BorderTop = 2;
        private int BorderRight = 2;
        private int BorderBottom = 2;
        private int BorderLeft = 2;
        private int SnakeLength = 12;

        private int SizeX;
        private int SizeY;

        /// <summary>
        /// Initializes the game board 
        /// </summary>
        /// <param name="sizex">Game board size X</param>
        /// <param name="sizey">Game board size Y</param>
        public SnakeGame(int sizex, int sizey)
        {
            SizeX = sizex;
            SizeY = sizey;

            PlayGround = new SnakeCell[SizeX, SizeY];
            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
                {
                    PlayGround[i, j] = new SnakeCell();
                    PlayGround[i, j].PlayerId = 0;  // PlayerId = 0;
                }
            }
        }

        /// <summary>
        /// Method responsible to add a new player in the game and put it on the board.
        /// </summary>
        public void AddPlayerToBoard(string playerConnectionId , string PlayerName)
        {
            if (PlayGround == null || SizeX <= 6 || SizeY <= 6)
            {
                return;
            }

            bool ValidPlace = false;

            while (!ValidPlace)
            {
                Random random = new Random();
                int randomRow = random.Next(BorderTop + 1, SizeX - BorderBottom - 1 - SnakeLength);
                int randomColumn = random.Next(BorderLeft + 1, SizeY - BorderRight - 1);
                if (PlayGround[randomRow, 4].PlayerId == 0)
                {
                    StartPlayerToLocation(randomRow, randomColumn, playerConnectionId , PlayerName);
                }
                ValidPlace = true;
            }
        }

        /// <summary>
        /// Returns the map of the game table
        /// </summary>
        /// <returns>Map of the table</returns>
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

        /// <summary>
        /// To be implemented
        /// </summary>
        public void DoTurn()
        {

        }

        /// <summary>
        /// Moves a the snake in the specified direction
        /// </summary>
        /// <param name="snakePlayer">The snake player on the board that moves</param>
        public void MoveSnakeBottom(SnakePlayer snakePlayer)
        {
            RemoveLastTailItemAndMoveTailToHead(snakePlayer);
            if (snakePlayer.SnakeHeadLocation.PosX < SizeX - BorderBottom - 1)
            {
                snakePlayer.SnakeHeadLocation.PosX++;
            }
            else
            {
                snakePlayer.SnakeHeadLocation.PosX = BorderTop;
            }
            PlayGround[snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY].PlayerId = snakePlayer.PlayerId;
        }

        /// <summary>
        /// Moves a the snake in the specified direction
        /// </summary>
        /// <param name="snakePlayer">The snake player on the board that moves</param>
        public void MoveSnakeTop(SnakePlayer snakePlayer)
        {
            RemoveLastTailItemAndMoveTailToHead(snakePlayer);
            if (snakePlayer.SnakeHeadLocation.PosX > BorderTop)
            {
                snakePlayer.SnakeHeadLocation.PosX = snakePlayer.SnakeHeadLocation.PosX - 1;
            }
            else
            {
                snakePlayer.SnakeHeadLocation.PosX = SizeX - BorderBottom - 1;
            }
            PlayGround[snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY].PlayerId = snakePlayer.PlayerId;
        }

        /// <summary>
        /// Moves a the snake in the specified direction
        /// </summary>
        /// <param name="snakePlayer">The snake player on the board that moves</param>
        public void MoveSnakeLeft(SnakePlayer snakePlayer)
        {
            RemoveLastTailItemAndMoveTailToHead(snakePlayer);
            if (snakePlayer.SnakeHeadLocation.PosY > 0 + BorderLeft)
            {
                snakePlayer.SnakeHeadLocation.PosY = snakePlayer.SnakeHeadLocation.PosY - 1;
            }
            else
            {
                snakePlayer.SnakeHeadLocation.PosY = SizeY - BorderRight - 1;
            }
            PlayGround[snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY].PlayerId = snakePlayer.PlayerId;
        }

        /// <summary>
        /// Moves a the snake in the specified direction
        /// </summary>
        /// <param name="snakePlayer">The snake player on the board that moves</param>
        public void MoveSnakeRight(SnakePlayer snakePlayer)
        {
            RemoveLastTailItemAndMoveTailToHead(snakePlayer);
            if (snakePlayer.SnakeHeadLocation.PosY < SizeY - BorderRight - 1)
            {
                snakePlayer.SnakeHeadLocation.PosY = snakePlayer.SnakeHeadLocation.PosY + 1;
            }
            else
            {
                snakePlayer.SnakeHeadLocation.PosY = BorderLeft;
            }
            PlayGround[snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY].PlayerId = snakePlayer.PlayerId;
        }

        /// <summary>
        /// A method that checks if a snake heads backwards, to his tail
        /// </summary>
        /// <param name="snakePlayer">snakePlayer</param>
        /// <param name="direction">direction</param>
        /// <returns></returns>
        public bool SnakeHeadsTowardsTail(SnakePlayer snakePlayer, SnakeDirection direction)
        {
            Position checkLocation = new Position(0, 0);
            switch (direction)
            {
                case SnakeDirection.Left:
                    checkLocation = new Position(snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY - 1);
                    break;
                case SnakeDirection.Right:
                    checkLocation = new Position(snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY + 1);
                    break;
                case SnakeDirection.Top:
                    checkLocation = new Position(snakePlayer.SnakeHeadLocation.PosX - 1, snakePlayer.SnakeHeadLocation.PosY);
                    break;
                case SnakeDirection.Bottom:
                    checkLocation = new Position(snakePlayer.SnakeHeadLocation.PosX + 1, snakePlayer.SnakeHeadLocation.PosY);
                    break;
            }
            foreach (var tailItem in snakePlayer.Tail)
            {
                if (tailItem.PosX == checkLocation.PosX && tailItem.PosY == checkLocation.PosY)
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Continues movign snake to its assigned direction.
        /// </summary>
        /// <param name="snakePlayer"></param>
        public void ContinueMoveSnake(SnakePlayer snakePlayer)
        {
            MoveSnakeToDirection(snakePlayer, snakePlayer.Direction);
        }

        /// <summary>
        ///  change direction of a snake is heading
        /// </summary>
        /// <param name="snakePlayer"></param>
        /// <param name="newDirection"></param>
        public void ChangeDirection(SnakePlayer snakePlayer, SnakeDirection newDirection)
        {
            if (!SnakeHeadsTowardsTail(snakePlayer, newDirection))
            {
                snakePlayer.Direction = newDirection;
            }

        }

        /// <summary>
        /// Returns true if the given snake player shares a position on the board with other snake player
        /// </summary>
        /// <param name="snakePlayer"></param>
        /// <returns></returns>
        public bool SnakeCollidedWithSomething(SnakePlayer snakePlayer)
        {
            //very fast lambda expression. Hard to debug.
            //return SnakePlayers.Any(otherSnake => otherSnake.Tail.Any(snakeTailPart => snakePlayer.SnakeHeadLocation.HasSameLocationWith(snakeTailPart)) || snakePlayer.SnakeHeadLocation.HasSameLocationWith(otherSnake.SnakeHeadLocation));
            foreach (var otherSnake in SnakePlayers)
            {
                foreach (var tailPart in otherSnake.Tail)
                {
                    if (snakePlayer.SnakeHeadLocation.HasSameLocationWith(tailPart))
                        return true;
                }
                if (snakePlayer.SnakeHeadLocation.HasSameLocationWith(otherSnake.SnakeHeadLocation) && snakePlayer != otherSnake)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Actualy kil la snake on the board ;)
        /// </summary>
        /// <param name="snakePlayer"></param>
        public void KillSnake(SnakePlayer snakePlayer)
        {
            snakePlayer.PlayerStatus = SnakePlayerStatus.Dead;
            snakePlayer.PlayerColor = "#aaaaaa";
            snakePlayer.SnakeHeadLocation = snakePlayer.Tail.First();
        }

        /// <summary>
        /// Moves the snake to a specific direction
        /// </summary>
        /// <param name="snakePlayer"></param>
        /// <param name="Direction"></param>
        private void MoveSnakeToDirection(SnakePlayer snakePlayer, SnakeDirection Direction)
        {
            switch (Direction)
            {
                case SnakeDirection.Top: MoveSnakeTop(snakePlayer); break;
                case SnakeDirection.Right: MoveSnakeRight(snakePlayer); break;
                case SnakeDirection.Left: MoveSnakeLeft(snakePlayer); break;
                case SnakeDirection.Bottom: MoveSnakeBottom(snakePlayer); break;
            }
        }

        private void RemoveLastTailItemAndMoveTailToHead(SnakePlayer snakePlayer)
        {
            if (snakePlayer.Tail.Count >= snakePlayer.SnakeLength - 1)
            {
                // demark on playboard the last tail
                PlayGround[snakePlayer.Tail.Last().PosX, snakePlayer.Tail.Last().PosY].PlayerId = 0;
                // renove last segment of tail
                snakePlayer.Tail.Remove(snakePlayer.Tail.Last());
            }
            // add tail to head
            snakePlayer.Tail.AddFirst(new Position(snakePlayer.SnakeHeadLocation.PosX, snakePlayer.SnakeHeadLocation.PosY));
        }

        /// <summary>
        /// Display playground on console.Testing pourposes.
        /// </summary>
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

        /*
        /// <summary>
        /// Starts the game core and runs it.
        /// </summary>
        public void RunGameCore()
        {
            HttpContext.Current.Application["SnakeGameStarted"] = "start";
            while (true)
            {
                if (HttpContext.Current.Application["SnakeGameStarted"] != "stop")
                {
                    var sg = HttpContext.Current.Application["SnakeGame"] as SnakeGame;
                    //sg.AddPlayerToBoard(Guid.NewGuid());
                    sg.MoveSnakeTop(sg.SnakePlayers.First());
                    System.Threading.Thread.Sleep(500);
                }
            }
        }*/


        private SnakeGameStatus StartPlayerToLocation(int PosX, int PosY, string ConnectionId, string PlayerName)
        {
            if (PlayGround[PosX, PosY].PlayerId != 0 || PosX <= 3)
                return SnakeGameStatus.Error;
            SnakePlayer sp = new SnakePlayer();
            Random random = new Random();
            sp.PlayerId = random.Next(0, int.MaxValue);
            sp.SnakeHeadLocation = new Position(PosX, PosY);
            PlayGround[PosX, PosY].PlayerId = sp.PlayerId;
            CreatePlayerTail(PosX, PosY, sp, SnakeLength);
            sp.SnakeLength = SnakeLength;
            sp.PlayerConnectionId = ConnectionId;
            //http://stackoverflow.com/questions/730625/how-do-i-create-a-random-hex-string-that-represents-a-color
            sp.PlayerColor = String.Format("#{0:X6}", random.Next(0x1000000));
            if (!string.IsNullOrEmpty(PlayerName))
            {
                sp.PlayerName = PlayerName;
            }
            else
            {
                sp.PlayerName = ConnectionId;
            }
            SnakePlayers.Add(sp);
            return SnakeGameStatus.Running;
        }

        private bool CreatePlayerTail(int HeadPosX, int HeadPosY, SnakePlayer sp, int TailLength)
        { 
            Position currentPosition = new Position(HeadPosX,HeadPosY);
            for (int i=0;i < TailLength;i++)
            {
                bool TailPointIsAdded;

                currentPosition = new Position(currentPosition.PosX + 1, currentPosition.PosY);
                TailPointIsAdded = CreatePlayerTailPoint(currentPosition.PosX, currentPosition.PosY, sp);

                if (!TailPointIsAdded)
                {
                    currentPosition = new Position(currentPosition.PosX - 1, currentPosition.PosY);
                    TailPointIsAdded = CreatePlayerTailPoint(currentPosition.PosX, currentPosition.PosY,sp);
                    
                }
                if (!TailPointIsAdded)
                {
                    currentPosition = new Position(currentPosition.PosX, currentPosition.PosY + 1);
                    TailPointIsAdded = CreatePlayerTailPoint(currentPosition.PosX, currentPosition.PosY, sp);

                }
                if (!TailPointIsAdded)
                {
                    currentPosition = new Position(currentPosition.PosX, currentPosition.PosY - 1);
                    TailPointIsAdded = CreatePlayerTailPoint(currentPosition.PosX, currentPosition.PosY, sp);
                }

                if (!TailPointIsAdded)
                {
                    return false;
                }
                else 
                {
                }
            }
            return true;
        }

        private bool CreatePlayerTailPoint(int PosX, int PosY, SnakePlayer sp)
        {
            if (PlayGround[PosX, PosY].PlayerId == 0)
            {
                PlayGround[PosX, PosY].PlayerId = sp.PlayerId;
                sp.Tail.AddLast(new Position(PosX, PosY));
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Represents a cell on the gameboard.
    /// If the _PlayerId value is 0 then the cell is empty, else it contains the Id of the player
    /// </summary>
    public class SnakeCell
    {
        private int _PlayerId;

        /// <summary>
        /// The Id of the player in the cell, 0 if empty
        /// </summary>
        public int PlayerId
        {
            get { return this._PlayerId; }
            set { this._PlayerId = value; }
        }
    }

    /// <summary>
    /// Contains the info about the player
    /// </summary>
    public class SnakePlayer
    {
        /// <summary>
        /// Player id
        /// </summary>
        public int PlayerId;

        /// <summary>
        /// Player's connection id to the client
        /// </summary>
        public string PlayerConnectionId;

        /// <summary>
        /// Length of the snake of the player
        /// </summary>
        public int SnakeLength;
        /// <summary>
        /// Location of the head on the board
        /// </summary>
        public Position SnakeHeadLocation;
        /// <summary>
        /// All the position of the components of the snake except the head.
        /// </summary>
        public LinkedList<Position> Tail = new LinkedList<Position>();
        /// <summary>
        ///  The direction where the snake is headed. By default top.
        /// </summary>
        public SnakeDirection Direction = SnakeDirection.Top;
        /// <summary>
        /// The color of the player which is shown on the client.HTML format . "#" included.
        /// </summary>
        public string PlayerColor;
        /// <summary>
        /// The status of the snake of the player.Starts  with Alive. If collied with something shoule be set to dead
        /// </summary>
        public SnakePlayerStatus PlayerStatus = SnakePlayerStatus.Alive;
        /// <summary>
        /// The name of the player.
        /// </summary>
        public string PlayerName;


        
    }
    /// <summary>
    /// A position on the gameboard defined by X and Y coordinates.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// X position on the table
        /// </summary>
        public int PosX;
        /// <summary>
        /// Y position on the table
        /// </summary>
        public int PosY;
        public Position(int posx, int posy)
        {
            this.PosX = posx;
            this.PosY = posy;
        }

        public bool HasSameLocationWith(Position otherPosition)
        { 
            return this.PosX == otherPosition.PosX && PosY == otherPosition.PosY;
        }
    }
}
