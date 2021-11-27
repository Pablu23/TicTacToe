using System;
using System.Linq;
using TicTacToe.Players;

namespace TicTacToe
{
    public class TicTacToe : Game
    {
        protected TicTacToeBoard Board;

        protected bool GameFinished;

        private void RemovePlayer(Player player)
        {
            if (Players.Count <= 0) throw new Exception("No Players found cant remove Player");
            Players.Remove(player);
            Scores.Remove(player);
        }

        public void ReplacePlayer(int oldPlayerIndex, Player newPlayer)
        {
            RemovePlayer(Players[oldPlayerIndex]);
            AddPlayer(newPlayer);
        }

        public override void AddPlayer(Player player)
        {
            if (Players.Count >= 2) throw new Exception("Trying to add too many Players");
            Players.Add(player);
            Scores.Add(player, 0);
        }

        public int GetPlayerCount()
        {
            return Players.Count();
        }

        public FieldState GetOpponentSymbol(int playerIndex)
        {
            return Players[playerIndex].Symbol;
        }

        public void ShowPlayers()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Console.WriteLine($"[{i+1}] Name: {Players[i].Name} | Symbol: {Players[i].Symbol} | Type: {Players[i].GetType().ToString().Substring(18)}");
            }   
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
            if (!Board.IsGameFinished(out var winnerState)) return;
            
            GameFinished = true;
            
            // Get the winner (Player? <- ? because the player could be null)
            var winner = Players.FirstOrDefault(x => x.Symbol == winnerState);
            if (winner != null)
            {
                AddScore(winner);
                
                //If one of the Players was a LearningAiPlayer add the Match to the Ai Brain
                var player = Players.FirstOrDefault(x => x.GetType() == typeof(LearningAiPlayer));
                var learningAiPlayer = player as LearningAiPlayer;
                if (learningAiPlayer != null) 
                    learningAiPlayer.SaveToJson(Board);
            }
            CleanUp();
        }

        public override void StartGame()
        {
            if (Players.Count != 2) throw new Exception("Not enough, or too many Players");
            Board = new TicTacToeBoard();
            
            // Draw the Board so the first Player can see it
            Board.DrawBoard();
            GameFinished = false;
            int round = 0;
            
            while (GameFinished != true)
            {
                // For both Players
                for (int i = 0; i < 2; i++)
                {
                    // As long as the Player has not played an allowed move,
                    // he is asked to repeat his move
                    bool placed;
                    do
                    {
                        placed = Board.TryPlace(Players[i].MakeMove(Board), Players[i], round);
                    } while (!placed);
                    
                    round++;
                    Board.DrawBoard();
                    CheckGameState();
                    //Dont let the second Player make his turn if the game is already finished
                    if (GameFinished) break;
                }
            }
        }
    }
}