using System;

namespace TicTacToe.Players
{
    public class BasicAiPlayer : Player
    {
        private readonly Random _random = new Random();
        
        public BasicAiPlayer(string name, FieldState symbol) : base(name, symbol)
        {
        }
        
        public override int MakeMove(TicTacToeBoard _)
        {
            return _random.Next(9);
        }

        public override void CleanUp() { }
    }
}