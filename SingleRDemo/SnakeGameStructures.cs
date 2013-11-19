using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingleRDemo
{
    /// <summary>
    /// Represents the list of statuses in that the game can be
    /// </summary>
    public enum SnakeGameStatus
    {
        Running,
        Stopped,
        Error,
        BeingStopped
    }

    /// <summary>
    /// a List a directions where the snake can head to
    /// </summary>
    public enum SnakeDirection
    { 
        Top,
        Right,
        Bottom,
        Left
    }

    public enum SnakePlayerStatus
    { 
        Alive,
        Dead
    }




}