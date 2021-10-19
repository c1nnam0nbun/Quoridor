namespace Quoridor
{
    public abstract class TurnState : State
    {
        public abstract void Update();

        public abstract void ChangeTurn();

        public abstract void MakeMove();
    }
}