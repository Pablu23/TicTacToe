using System.Collections.Generic;
using TicTacToe.Players;

namespace TicTacToe
{
    //Class on which every game bases on (Only TicTacToe atm)
    public abstract class Game
    {
        protected readonly List<Player> Players = new List<Player>();
        protected readonly Dictionary<Player, int> Scores = new Dictionary<Player, int>();
        public abstract void StartGame();
        public abstract void DrawScore();
        public abstract void CheckGameState();
        public abstract void AddScore(Player player);
        public abstract void AddPlayer(Player player);
    }
}