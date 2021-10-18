namespace Quoridor
{
    public class PlayerOneTurnState : TurnState
    {
        public override void Update()
        {
            if (GameManager.PlayerOne == null) GameManager.PlacePlayerOne();
            else
            {
                foreach (Wall wall in GameManager.PlayerOne.Walls) wall.Update();
            }
        }

        public override void ChangeTurn()
        {
            GameManager.TurnState = new PlayerTwoTurnState();
        }

        public override bool MovePlayer(Cell cell)
        {
            return GameManager.PlayerOne?.Move(cell) ?? false;
        }
    }
}