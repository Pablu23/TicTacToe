using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;


namespace TicTacToe.Players
{
    //TODO: Rename
    public class LearnJson
    {
        public int Weight;
        public FieldState[] Board;
        public int[] BoardHistory;

        public IList<FieldState> GetBoardAtTime(int round)
        {
            var newBoard = new FieldState[9];
            if (BoardHistory.Length <= round) throw new Exception("Round out of reach from BoardHistory");
            for (int i = 0; i < round; i++)
            {
                if (BoardHistory[i] < 0) break;
                newBoard[BoardHistory[i]] = Board[BoardHistory[i]];
            }

            return Array.AsReadOnly(newBoard);
        }

        public int GetNextMove(int round)
        {
            if (BoardHistory.Length <= round + 1) throw new Exception("Round out of reach from BoardHistory");
            return BoardHistory[round + 1];
        }
    }

    //TODO: Rename
    public class AiBrain
    {
        public List<LearnJson> AllOutcomes;
    }

    //TODO: Save Board history somehow
    public class LearningAiPlayer : Player
    {
        private string _pathToLearningDir = @"E:\Programieren\csharp\TicTacToe\TicTacToe\Players\LearningAiDir\ai.json";
        private int _round;
        private AiBrain _brain;
        private Random _random;
        private FieldState _enemyFieldState;
        
        
        public LearningAiPlayer(string name, FieldState symbol) : base(name, symbol)
        {
            _round = 0;
            _random = new Random();
            
            string allJsonText = File.ReadAllText(_pathToLearningDir);
            _brain = JsonConvert.DeserializeObject<AiBrain>(allJsonText);
            
            if (_brain is null || _brain.AllOutcomes is null)
            {
                _brain = new AiBrain()
                {
                    AllOutcomes = new List<LearnJson>()
                };
            }

            Symbol = symbol;
            
            if (symbol == FieldState.PlayerO)
            {
                _enemyFieldState = FieldState.PlayerX;
            }
            else
            {
                _enemyFieldState = FieldState.PlayerO;
            }
        }

        public override int MakeMove(TicTacToeBoard board)
        {
            var copyOfBoard = board.GetFieldStates();
            int move;
            do
            {
                move = _random.Next(9);
            } while (copyOfBoard[move] != FieldState.Empty);

            if (_round == 0)
            {
                //TODO: with 2 Learning Players, they might open the file at the same time, leading to a crash
                
                if (copyOfBoard.Where(x => x == _enemyFieldState).Count() > 0)
                {
                    _round++;
                }
            }

            if (_random.Next(100) >= 95)
            {
                _round += 2;
                return move;
            }
            
            if (_brain is not null)
            {
                var allPlayedRounds = _brain.AllOutcomes.FindAll(x =>
                    Equals(x.GetBoardAtTime(_round), copyOfBoard));
                if (_round == 0)
                {
                    var bestOption = _brain.AllOutcomes.Aggregate((i, j) => i.Weight > j.Weight ? i : j);
                    if (bestOption is not null)
                    {
                        move = bestOption.GetNextMove(_round);
                    }
                }
                if (allPlayedRounds.Count != 0)
                {
                    var bestOption = allPlayedRounds.Aggregate((i, j) => i.Weight > j.Weight ? i : j);
                    if (bestOption is not null)
                    {
                        move = bestOption.GetNextMove(_round);
                    }
                }
            }
            
            _round += 2;
            return move;
        }

        public override void CleanUp()
        {
            _round = 0;
            _random = new Random();
            if (Symbol == FieldState.PlayerO)
            {
                _enemyFieldState = FieldState.PlayerX;
            }
            else
            {
                _enemyFieldState = FieldState.PlayerO;
            }
            
            string allJsonText = File.ReadAllText(_pathToLearningDir);
            _brain = JsonConvert.DeserializeObject<AiBrain>(allJsonText);
        }

        public void SaveToJson(TicTacToeBoard board)
        {
            var newRound = new LearnJson()
            {
                Board = board.GetFieldStates(),
                BoardHistory = board.GetBoardHistory(),
                Weight = 1
            };

            var exists = _brain.AllOutcomes.FirstOrDefault(x => x.BoardHistory.SequenceEqual(newRound.BoardHistory));
            if (exists != null) exists.Weight++;
            else _brain.AllOutcomes.Add(newRound);
            
            using (var file = File.CreateText(_pathToLearningDir))
            {
                string json = JsonConvert.SerializeObject(_brain);
                file.Write(json);
            }
        }
    }
}