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
            // Just make a random move
            // It doesnt need to get checked because the TicTacToe Class handles the input by itself
            return _random.Next(9);
        }

        // No Cleanup is needed
        public override void CleanUp() { }
    }
}