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
                var key = Console.ReadKey(true);
                loop = int.TryParse(key.KeyChar.ToString(), out result);

            } while (!loop);
            return result-1;
        }

        public override void CleanUp() {}
    }
}