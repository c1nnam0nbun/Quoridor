
using System.Linq;

namespace Quoridor
{
    public class PlayerOneTurnState : TurnState
    {
        public override void Update()
        {
            if (GameManager.PlayerOne == null)
            {
                Cell cell = GameManager.GameState.RequestInitialCellForPlayerOne();
                if (cell?.Index.X == 1) GameManager.PlacePlayerOne(cell);
            }
            else
            {
                foreach (Wall wall in GameManager.PlayerOne.Walls) wall.Update();
            }
        }

        public override void ChangeTurn()
        {
            GameManager.TurnState = new PlayerTwoTurnState();
        }

        public override void MakeMove()
        {
            if (GameManager.PlayerOne == null) return;
            
            if (Wall.ActiveWall == null && GameManager.PlayerOne.Move(GameManager.GameState.RequestNextCellForPlayerOne()))
            {
                ChangeTurn();
                return;
            }

            Wall placed = GameManager.GameState.RequestPlacedWallForPlayerOne();
            if (!GameManager.PlayerOne.Walls.Contains(placed)) return;
            GameManager.PlayerOne.Walls = GameManager.PlayerOne.Walls.Where(wall => wall != placed).ToArray();
            ChangeTurn();
        }
    }
}