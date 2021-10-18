namespace Quoridor
{
    public class PlayerTwoTurnState : TurnState
    {
        public override void Update()
        {
            if (GameManager.PlayerTwo == null) GameManager.PlacePlayerTwo();
            else
            {
                foreach (Wall wall in GameManager.PlayerTwo.Walls) wall.Update();
            }
        }
        
        public override void ChangeTurn()
        {
            GameManager.TurnState = new PlayerOneTurnState();
        }

        public override bool MovePlayer(Cell cell)
        {
            return GameManager.PlayerTwo?.Move(cell) ?? false;
        }
    }
}