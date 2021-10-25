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

        private void CleanUp()
        {
            foreach (var player in Players)
            {
                player.CleanUp();
            }
        }
        
        public override void CheckGameState()
        {
            if (!_board.IsGameFinished(out var winnerState)) return;
            
            _gameFinished = true;
            var winner = Players.FirstOrDefault(x => x.Symbol == winnerState);
            if (winner is not null)
            {
                AddScore(winner);
                var player = Players.FirstOrDefault(x => x.GetType() == typeof(LearningAiPlayer));
                var learningAiPlayer = player as LearningAiPlayer;
                learningAiPlayer?.SaveToJson(_board);
            }
            CleanUp();
        }

        public override void StartGame()
        {
            if (Players.Count != 2) throw new Exception("Not enough, or too many Players");
            _board = new TicTacToeBoard();
            _board.DrawBoard();
            _gameFinished = false;
            int round = 0;
            
            while (_gameFinished is not true)
            {
                for (int i = 0; i < 2; i++)
                {
                    bool placed = true;
                    do
                    {
                        placed = _board.TryPlace(Players[i].MakeMove(_board), Players[i], round);
                    } while (!placed);
                    
                    round++;
                    _board.DrawBoard();
                    CheckGameState();
                    if (_gameFinished) break;
                }
            }
        }
    }
}