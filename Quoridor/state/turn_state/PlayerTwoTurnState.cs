using System.Linq;

namespace Quoridor
{
    public class PlayerTwoTurnState : TurnState
    {
        public override void Update()
        {
            if (GameManager.PlayerTwo == null) 
            {
                Cell cell = GameManager.GameState.RequestInitialCellForPlayerTwo();
                if (cell?.Index.X == 9) GameManager.PlacePlayerTwo(cell);
            }
            else
            {
                foreach (Wall wall in GameManager.PlayerTwo.Walls) wall.Update();
            }
        }
        
        public override void ChangeTurn()
        {
            GameManager.TurnState = new PlayerOneTurnState();
        }

        public override void MakeMove()
        {
            if (GameManager.PlayerTwo == null) return;
            
            if (Wall.ActiveWall == null && GameManager.PlayerTwo.Move(GameManager.GameState.RequestNextCellForPlayerTwo()))
            {
                ChangeTurn();
                return;
            }

            Wall placed = GameManager.GameState.RequestPlacedWallForPlayerTwo();
            if (!GameManager.PlayerTwo.Walls.Contains(placed)) return;
            GameManager.PlayerTwo.Walls = GameManager.PlayerTwo.Walls.Where(wall => wall != placed).ToArray();
            ChangeTurn();
        }
    }
}