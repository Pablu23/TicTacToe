using System;

namespace TicTacToe.Players
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name, FieldState symbol) : base(name, symbol)
        {
        }
        
        public override int MakeMove(TicTacToeBoard _)
        {
            int result;
            bool loop;
            do
            {
                Console.WriteLine($"Where do you want to put your {Symbol} ?");
                
                // This is because the highest number is 9 so we always only need one Key to play
                var key = Console.ReadKey(true);
                loop = int.TryParse(key.KeyChar.ToString(), out result);

            } while (!loop);
            // Moves are Zero based, but are Shown One based
            // So we subtract one to get to the Zero based Move
            return result-1;
        }

        public override void CleanUp() {}
    }
}