using System;

namespace TicTacToe.Players
{
    // The advanced Player uses the MiniMax Algorithm to chose its best Placement option
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
            // The best Value for the Player is decided if it wants to min or to max the Score
            int bestVal = (int) Symbol * -1000;
            
            // The Move it makes on default | TODO: maybe this should be random 0-8, if the algorithm goes into a loop
            int bestMove = -1;

            // For every Field on the board Calculate a value of how good it would be to place there
            for (int i = 0; i < 9; i++)
            {
                if (b.Fields[i].State == FieldState.Empty)
                {
                    // Place the Piece on the Field
                    b.Fields[i].State = Symbol;
                    
                    // See how likely it is to win with this move
                    int val = MiniMax(b, -1, _enemyFieldState);
                    
                    // Undo the move
                    b.Fields[i].State = FieldState.Empty;
                    
                    // If it is the best move so far, mark it and continue on to the next field
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
        
        // MiniMax is a tree based Algorithm to find the best Outcome for the starting Player,
        // if both Players play perfectly
        // Reference: https://www.youtube.com/watch?v=l-hh51ncgDI
        private int MiniMax(TicTacToeBoard b, int depth, FieldState player)
        {
            // If the game is Over return the winner value O = -1, No Winner = 0, X = 1
            if (depth == 0 | b.IsGameFinished(out var winner))
            {
                return (int) winner;
            }
            
            // The PlayerX wants to maximize the Value
            if (player == FieldState.PlayerX)
            {
                int maxEval = int.MinValue;
                for (int i = 0; i < 9; i++)
                {
                    if (b.Fields[i].State == FieldState.Empty)
                    {
                        // Place the Piece on the Field
                        b.Fields[i].State = FieldState.PlayerX;
                        
                        // See how the perfect Enemy will respond to that placement
                        int eval = MiniMax(b, depth - 1, FieldState.PlayerO);
                        
                        // Undo the placement
                        b.Fields[i].State = FieldState.Empty;
                        
                        // Update the max evaluated Value
                        maxEval = Math.Max(maxEval, eval);
                    }
                }
                
                return maxEval;
            }
            // The PlayerO wants to minimize the Value
            else
            {
                int minEval = int.MaxValue;
                for (int i = 0; i < 9; i++)
                {
                    if (b.Fields[i].State == FieldState.Empty)
                    {
                        // Place the Piece on the Field
                        b.Fields[i].State = FieldState.PlayerO;
                        
                        // See how the perfect Enemy will respond to that placement
                        int eval = MiniMax(b, depth - 1, FieldState.PlayerX);
                        
                        // Undo the placement
                        b.Fields[i].State = FieldState.Empty;
                        
                        // Update the min evaluated Value
                        minEval = Math.Min(minEval, eval);
                    }
                }

                return minEval;
            }
            
        }

        // No Cleanup needed
        public override void CleanUp()
        {
        }
    }
}