namespace TicTacToe.Players
{
    public abstract class Player
    {
        public string Name;
        public FieldState Symbol;
        
        //TODO: Replace Fieldstate Symbol with Char, for easier understanding <- only on construction
        protected Player(string name, FieldState symbol)
        {
            Name = name;
            Symbol = symbol;
        }

        public abstract int MakeMove(TicTacToeBoard board);

        public abstract void CleanUp();
    }
}