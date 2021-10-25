using System;
using System.Collections.Generic;
using TicTacToe.Players;

namespace TicTacToe
{
    public class TicTacToeBoard
    {
        public Field[] Fields = new Field[9];

        public TicTacToeBoard()
        {
            for (int i = 0; i < Fields.Length; i++)
            {
                Fields[i] = new Field(Convert.ToChar((i+1).ToString()));
            }
        }

        public void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine(
                $"{Fields[0].Symbol} | " +
                $"{Fields[1].Symbol} | " +
                $"{Fields[2].Symbol}\n--------\n" +
                $"{Fields[3].Symbol} | " +
                $"{Fields[4].Symbol} | " +
                $"{Fields[5].Symbol}\n--------\n" +
                $"{Fields[6].Symbol} | " +
                $"{Fields[7].Symbol} | " +
                $"{Fields[8].Symbol}");
        }

        public bool TryPlace(int pos, Player player, int round)
        {
            if (pos < 0 || pos > 9) return false;
            if (Fields[pos].State != FieldState.Empty) return false;

            switch (player.Symbol)
            {
                case FieldState.PlayerX:
                    Fields[pos].State = FieldState.PlayerX;
                    Fields[pos].Symbol = 'X';
                    Fields[pos].Round = round;
                    break;
                case FieldState.PlayerO:
                    Fields[pos].State = FieldState.PlayerO;
                    Fields[pos].Symbol = 'O';
                    Fields[pos].Round = round;
                    break;
                default:
                    throw new Exception("Player Symbol was not recognised");
            }

            return true;
        }

        public int[] GetBoardHistory()
        {
            int[] history = new int[Fields.Length];
            for (int i = 0; i < history.Length; i++)
            {
                history[i] = -1;
            }
            
            int index = 0;
            foreach (var field in Fields)
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
            foreach (var field in Fields)
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
                FieldState first = Fields[i * 3].State;
                winner = first;
                if (first == FieldState.Empty) continue;
                for (int j = 0; j < 3; j++)
                {
                    if (Fields[i * 3 + j].State == first)
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
                FieldState first = Fields[i].State;
                winner = first;
                if (first == FieldState.Empty) continue;
                for (int j = 0; j < 3; j++)
                {
                    if (Fields[j * 3 + i].State == first)
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
            winner = Fields[4].State;
            return Fields[4].State != FieldState.Empty && 
                   (Fields[0].State == Fields[4].State && Fields[4].State == Fields[8].State ||
                    Fields[2].State == Fields[4].State && Fields[4].State == Fields[6].State);
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
            foreach (var field in Fields)
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
                Fields[i] = fields[i];
            }
        }
    }
}