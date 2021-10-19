namespace Quoridor
{
    public abstract class TurnState : IState
    {
        public abstract void Update();

        public abstract void ChangeTurn();

        public abstract void MakeMove();
    }
}