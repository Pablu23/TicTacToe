using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Players;

namespace TicTacToe
{
    public class TicTacToeBoard
    {
        public Field[] _fields = new Field[9];

        public TicTacToeBoard()
        {
            for (int i = 0; i < _fields.Length; i++)
            {
                _fields[i] = new Field(Convert.ToChar((i+1).ToString()));
            }
        }

        public void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine(
                $"{_fields[0].Symbol} | " +
                $"{_fields[1].Symbol} | " +
                $"{_fields[2].Symbol}\n--------\n" +
                $"{_fields[3].Symbol} | " +
                $"{_fields[4].Symbol} | " +
                $"{_fields[5].Symbol}\n--------\n" +
                $"{_fields[6].Symbol} | " +
                $"{_fields[7].Symbol} | " +
                $"{_fields[8].Symbol}");
        }

        public bool TryPlace(int pos, Player player, int round)
        {
            if (pos < 0 || pos > 9) return false;
            if (_fields[pos].State != FieldState.Empty) return false;

            switch (player.Symbol)
            {
                case FieldState.PlayerX:
                    _fields[pos].State = FieldState.PlayerX;
                    _fields[pos].Symbol = 'X';
                    _fields[pos].Round = round;
                    break;
                case FieldState.PlayerO:
                    _fields[pos].State = FieldState.PlayerO;
                    _fields[pos].Symbol = 'O';
                    _fields[pos].Round = round;
                    break;
                default:
                    throw new Exception("Player Symbol was not recognised");
            }

            return true;
        }

        public int[] GetBoardHistory()
        {
            int[] history = new int[_fields.Length];
            for (int i = 0; i < history.Length; i++)
            {
                history[i] = -1;
            }
            
            int index = 0;
            foreach (var field in _fields)
            {
                if (field.Round >= 0 && field.Round <= history.Length)
                {
                    history[field.Round] = index;
                    index++;
                }
            }

            return history;
        }
        
        private bool IsBoardFull()
        {
            //return !_fields.Any(x => x.State == FieldState.Empty);
            bool isBoardFull = true;
            foreach (var field in _fields)
            {
                if (field.State == FieldState.Empty)
                {
                    isBoardFull = false;
                }
            }

            return isBoardFull;
        }

        private bool Horizontal(out FieldState winner)
        {
            winner = FieldState.Empty;
            for (int i = 0; i < 3; i++)
            {
                FieldState first = _fields[i * 3].State;
                winner = first;
                if (first == FieldState.Empty) continue;
                for (int j = 0; j < 3; j++)
                {
                    if (_fields[i * 3 + j].State == first)
                    {
                        if (j == 2)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;
        }

        private bool Vertical(out FieldState winner)
        {
            winner = FieldState.Empty;
            for (int i = 0; i < 3; i++)
            {
                FieldState first = _fields[i].State;
                winner = first;
                if (first == FieldState.Empty) continue;
                for (int j = 0; j < 3; j++)
                {
                    if (_fields[j * 3 + i].State == first)
                    {
                        if (j == 2)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            return false;
        }

        private bool Diagonal(out FieldState winner)
        {
            winner = _fields[4].State;
            return _fields[4].State != FieldState.Empty && 
                   (_fields[0].State == _fields[4].State && _fields[4].State == _fields[8].State ||
                    _fields[2].State == _fields[4].State && _fields[4].State == _fields[6].State);
        }

        public bool IsGameFinished(out FieldState winner)
        {
            if (Horizontal(out winner))
            {
                return true;
            }

            if (Vertical(out winner))
            {
                return true;
            }

            if (Diagonal(out winner))
            {
                return true;
            }

            winner = FieldState.Empty;
            return IsBoardFull();
        }


        public FieldState[] GetFieldStates()
        {
            var output = new FieldState[9];
            int index = 0;
            foreach (var field in _fields)
            {
                output[index] = field.State;
                index++;
            }

            return output;
        }
        /*public IList<Field> GetBoardCopy()
        {
            return Array.AsReadOnly(_fields);
        }*/

        public object GetBoardCopy()
        {
            return this.MemberwiseClone();
        }
        
        public void SetBoard(IList<Field> fields)
        {
            for (int i = 0; i < 9; i++)
            {
                _fields[i] = fields[i];
            }
        }
    }
}