namespace Quoridor
{
    public abstract class TurnState : IState
    {
        public bool ShouldIgnoreInput { get; set; }
        
        public abstract void Update();

        public abstract void ChangeTurn();

        public abstract void MakeMove();
    }
}