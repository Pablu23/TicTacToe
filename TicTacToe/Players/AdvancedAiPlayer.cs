using System;

namespace TicTacToe.Players
{
    public class AdvancedAiPlayer : Player
    {
        private FieldState _enemyFieldState;

        public AdvancedAiPlayer(string name, FieldState symbol) : base(name, symbol)
        {
            if (symbol == FieldState.PlayerO)
            {
                _enemyFieldState = FieldState.PlayerX;
            }
            else
            {
                _enemyFieldState = FieldState.PlayerO;
            }
        }

        public override int MakeMove(TicTacToeBoard b)
        {
            int bestVal = (int) Symbol * -1000;
            
            int bestMove = -1;

            for (int i = 0; i < 9; i++)
            {
                if (b.Fields[i].State == FieldState.Empty)
                {
                    b.Fields[i].State = Symbol;
                    int val = MiniMax(b, -1, _enemyFieldState);
                    b.Fields[i].State = FieldState.Empty;
                    if (Symbol == FieldState.PlayerX && val > bestVal)
                    {
                        bestMove = i;
                        bestVal = val;
                    }
                    if (Symbol == FieldState.PlayerO && val < bestVal)
                    {
                        bestMove = i;
                        bestVal = val;
                    }
                }
            }
            return bestMove;
        }
        
        private int MiniMax(TicTacToeBoard b, int depth, FieldState player)
        {
            if (depth == 0 | b.IsGameFinished(out var winner))
            {
                return (int) winner;
            }

            if (player == FieldState.PlayerX)
            {
                int maxEval = int.MinValue;
                for (int i = 0; i < 9; i++)
                {
                    if (b.Fields[i].State == FieldState.Empty)
                    {
                        b.Fields[i].State = FieldState.PlayerX;
                        int eval = MiniMax(b, depth - 1, FieldState.PlayerO);
                        b.Fields[i].State = FieldState.Empty;
                        maxEval = Math.Max(maxEval, eval);
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                for (int i = 0; i < 9; i++)
                {
                    if (b.Fields[i].State == FieldState.Empty)
                    {
                        b.Fields[i].State = FieldState.PlayerO;
                        int eval = MiniMax(b, depth - 1, FieldState.PlayerX);
                        b.Fields[i].State = FieldState.Empty;
                        minEval = Math.Min(minEval, eval);
                    }
                }

                return minEval;
            }
            
        }

        public override void CleanUp()
        {
        }
    }
}