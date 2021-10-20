using System.Linq;
using System.Windows.Forms;

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
            
            if (Input.IsMouseButtonUp(MouseButtons.Left) || ShouldIgnoreInput)
            {
                Wall placed = GameManager.GameState.RequestPlacedWallForPlayerTwo();
                if (!GameManager.PlayerTwo.Walls.Contains(placed))
                {
                    if (Wall.ActiveWall == null &&
                        GameManager.PlayerTwo.Move(GameManager.GameState.RequestNextCellForPlayerTwo()))
                    {
                        ChangeTurn();
                        Input.Flush();
                    }
                }
                else
                {
                    GameManager.PlayerTwo.Walls = GameManager.PlayerTwo.Walls.Where(wall => wall != placed).ToArray();
                    ChangeTurn();
                }
            }
        }
    }
}