using System;
using TicTacToe.Players;

namespace TicTacToe
{
    class Program
    {
        public static void Main(string[] args)
        {
            MainMenu();   
            Console.ReadKey(true);
        }

        private static void MainMenu()
        {
            TicTacToe ticTacToe = new TicTacToe();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to TicTacToe");
                Console.WriteLine("[C]onfig Players");
                //Console.WriteLine("[N]ew Game");
                Console.WriteLine("[S]tart Game");
                Console.WriteLine("[E]nd");

                char input = GetCharInput(new[] { 'c', 's', 'e', 'n' });

                switch (input)
                {
                    case 'c':
                        ConfigPlayers(ticTacToe);
                        break;
                    case 's':
                        if(ticTacToe.GetPlayerCount() < 2)
                        {
                            Console.WriteLine("There are not enough Players to start the game");
                            break;
                        }
                        ticTacToe.StartGame();
                        ticTacToe.DrawScore();
                        break;
                    case 'e':
                        Environment.Exit(0);
                        break;
                    case 'n':
                        ticTacToe = new TicTacToe();
                        break;
                }

                Console.WriteLine("To continue press any Key...");
                Console.ReadKey(true);
            }
        }

        private static void ConfigPlayers(TicTacToe ticTacToe)
        {

            //Show Players if any exist
            Console.Clear();
            if(ticTacToe.GetPlayerCount() == 0)
            {
                Player p1 = CreatePlayer();
                Player p2 = CreatePlayer(p1.Symbol);

                ticTacToe.AddPlayer(p1);
                ticTacToe.AddPlayer(p2);
            }
            else
            {
                Console.WriteLine("Which Player to edit");
                ticTacToe.ShowPlayers();

                Console.WriteLine("[E]xit");

                bool correctInput = false;
                int playerToEdit = 0;
                while (!correctInput)
                {
                    string input = Console.ReadLine();
                    if (input?.ToLower() == "e") return;
                    correctInput = int.TryParse(input, out playerToEdit);
                    playerToEdit -= 1;
                    if (correctInput) correctInput = playerToEdit <= ticTacToe.GetPlayerCount();
                }

                ticTacToe.ReplacePlayer(playerToEdit, CreatePlayer(ticTacToe.GetOpponentSymbol(playerToEdit == 0 ? 1 : 0)));
            }
        }


        private static Player CreatePlayer(FieldState otherPlayer = FieldState.Empty)
        {
            Console.Clear();
            Console.WriteLine("Create new Player\n----------------");
            Console.WriteLine("Name of the Player:");
            string name = Console.ReadLine();

            FieldState sym = FieldState.Empty;

            if (otherPlayer == FieldState.Empty)
            {
                Console.WriteLine("Symbole of the Player");
                string symbol = Console.ReadLine();

                switch (symbol?.ToUpper())
                {
                    case "X":
                        sym = FieldState.PlayerX;
                        break;
                    case "O":
                        sym = FieldState.PlayerO;
                        break;
                    default:
                        throw new Exception("Symbol was not recognised");
                }
            }
            else
            {
                switch (otherPlayer)
                {
                    case FieldState.PlayerO:
                        sym = FieldState.PlayerX;
                        break;
                    case FieldState.PlayerX:
                        sym = FieldState.PlayerO;
                        break;
                }
            }

            Console.WriteLine("Type of Player");
            Console.WriteLine("[1] Human");
            Console.WriteLine("[2] Basic (Random)");
            Console.WriteLine("[3] Advanced (Unbeatable)");
            Console.WriteLine("[4] Learning (Stupid)");

            int ki = int.Parse(Console.ReadLine());

            Player player;

            switch (ki)
            {
                case 1:
                    player = new HumanPlayer(name, sym);
                    break;
                case 2:
                    player = new BasicAiPlayer(name, sym);
                    break;
                case 3:
                    player = new AdvancedAiPlayer(name, sym);
                    break;
                case 4:
                    player = new LearningAiPlayer(name, sym);
                    break;
                default:
                    throw new Exception("The given Number was wrong");
            }

            return player;

        }

        private static char GetCharInput(char[] acceptedChars, string message = null)
        {
            if (message != null)
                Console.WriteLine(message);

            char result;
            bool loop = true;
            do
            {

                // This is because the highest number is 9 so we always only need one Key to play
                var key = Console.ReadKey(true);
                result = key.KeyChar;
                foreach (var item in acceptedChars)
                {
                    if (result == item)
                        loop = false;
                }

            } while (loop);
            return result;
        }

    }
}