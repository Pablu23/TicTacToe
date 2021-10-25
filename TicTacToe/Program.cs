using System;
using TicTacToe.Players;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            TicTacToe ticTacToe = new TicTacToe();
            Player player1 = new LearningAiPlayer("Learning", FieldState.PlayerX);
            Player player2 = new AdvancedAiPlayer("MinMax", FieldState.PlayerO);
            Player player3 = new BasicAiPlayer("Basic", FieldState.PlayerO);
            Player player4 = new HumanPlayer("Human", FieldState.PlayerO);

            ticTacToe.AddPlayer(player1);
            ticTacToe.AddPlayer(player4);

            //TODO: Menu
            //TODO: Learning Players properties, dont reset, after more than one round = crash

            ticTacToe.StartGame();
            ticTacToe.DrawScore();
            
            Console.ReadKey(true);
        }
    }
}