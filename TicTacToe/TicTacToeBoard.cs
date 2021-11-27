using System;
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
                //Initialise all Fields and give them the Symbol on which Position they are
                Fields[i] = new Field(Convert.ToChar((i + 1).ToString()));
            }
        }

        public void DrawBoard()
        {
            Console.Clear();

            for (int i = 0; i < 9; i++)
            {
                if (Fields[i].State == FieldState.PlayerO)
                    Console.ForegroundColor = ConsoleColor.Green;
                if (Fields[i].State == FieldState.PlayerX)
                    Console.ForegroundColor = ConsoleColor.Red;
                if (Fields[i].State == FieldState.Empty)
                    Console.ForegroundColor = ConsoleColor.Gray;

                Console.Write(Fields[i].Symbol);
                Console.ForegroundColor = ConsoleColor.Gray;

                if ((i + 1) % 3 == 0 && i != 8)
                    Console.Write("\n--------\n");
                else if (i == 8)
                    Console.WriteLine();
                else
                    Console.Write(" | ");
            }
        }

        //Tries to mark a position, if the position is not free it returns a false
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
    }
}