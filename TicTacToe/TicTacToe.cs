using System;
using System.Linq;
using TicTacToe.Players;

namespace TicTacToe
{
    class TicTacToe : Game
    {
        private TicTacToeBoard _board;

        private bool _gameFinished;
        
        public override void AddPlayer(Player player)
        {
            if (Players.Count >= 2) throw new Exception("Trying to add too many Players");
            Players.Add(player);
            Scores.Add(player, 0);
        }

        public override void AddScore(Player player)
        {
            if (Players.Contains(player))
                Scores[player]++;
        }

        public override void DrawScore()
        {
            foreach (var player in Players)
            {
                Console.WriteLine($"Player {player.Name}: {Scores[player]}");
            }
        }

        //Do CleanUp for every Player (Mostly needed for LearningAi to update the brain)
        private void CleanUp()
        {
            foreach (var player in Players)
            {
                player.CleanUp();
            }
        }
        
        // Check if the game is finished, and if so add Scores and more for LearningAi
        public override void CheckGameState()
        {
            if (!_board.IsGameFinished(out var winnerState)) return;
            
            _gameFinished = true;
            
            // Get the winner (Player? <- ? because the player could be null)
            var winner = Players.FirstOrDefault(x => x.Symbol == winnerState);
            if (winner is not null)
            {
                AddScore(winner);
                
                //If one of the Players was a LearningAiPlayer add the Match to the Ai Brain
                var player = Players.FirstOrDefault(x => x.GetType() == typeof(LearningAiPlayer));
                var learningAiPlayer = player as LearningAiPlayer;
                if (learningAiPlayer is not null) 
                    learningAiPlayer.SaveToJson(_board);
            }
            CleanUp();
        }

        public override void StartGame()
        {
            if (Players.Count != 2) throw new Exception("Not enough, or too many Players");
            _board = new TicTacToeBoard();
            
            // Draw the Board so the first Player can see it
            _board.DrawBoard();
            _gameFinished = false;
            int round = 0;
            
            while (_gameFinished is not true)
            {
                // For both Players
                for (int i = 0; i < 2; i++)
                {
                    // As long as the Player has not played an allowed move,
                    // he is asked to repeat his move
                    bool placed;
                    do
                    {
                        placed = _board.TryPlace(Players[i].MakeMove(_board), Players[i], round);
                    } while (!placed);
                    
                    round++;
                    _board.DrawBoard();
                    CheckGameState();
                    //Dont let the second Player make his turn if the game is already finished
                    if (_gameFinished) break;
                }
            }
        }
    }
}