using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class VsComputerGameState : GameState
    {
        private bool IsOkClicked { get; set; }
        
        public override void Update()
        {
            if (!IsOkClicked) return;
            if (GameManager.TurnState == null) GameManager.TurnState = new PlayerOneTurnState();
        }

        public override void Draw(Graphics g)
        {
            if (IsOkClicked) return;
            Size s = new Size(300, 100);
            Point p = new Point(GameManager.Window.Size.Width / 2 - s.Width / 2, GameManager.Window.Size.Height / 2 - s.Height / 2);
            PopupMessage.ShowBeginGameMessage(p, s, () => IsOkClicked = true, g);
        }
        
        public override Cell RequestInitialCellForPlayerOne()
        {
            return GameManager.PressedCell;
        }

        public override Cell RequestInitialCellForPlayerTwo()
        {
            int idx = Rng.GetRandomIntInRange(0, 8);
            return GameManager.GetCellAt(9, idx);
        }

        public override void OnTurnChange(TurnState state = null)
        {
            if (state != null) state.ShouldIgnoreInput = true;
        }

        public override Cell RequestNextCellForPlayerOne()
        {
            return GameManager.PressedCell;
        }

        public override Cell RequestNextCellForPlayerTwo()
        {
            while (true)
            {
                int count = GameManager.PlayerTwo.CurrentCell.Neighbours.Count;
                int idx = Rng.GetRandomIntInRange(0, count);
                Cell cell = GameManager.PlayerTwo.CurrentCell.Neighbours[idx];
                if (!cell.IsPlayable) continue;
                return GameManager.PlayerTwo.CurrentCell.Neighbours[idx];
            }
        }

        public override Wall RequestPlacedWallForPlayerOne()
        {
            return GameManager.PlacedWall;
        }

        public override Wall RequestPlacedWallForPlayerTwo()
        {
            if (Rng.GetRandomIntInRange(0, 100) > 70) return null;
            int idx = Rng.GetRandomIntInRange(0, GameManager.PlayerTwo.Walls.Length);
            if (GameManager.PlayerTwo.Walls.Length == 0) return null;
            Wall wall = GameManager.PlayerTwo.Walls[idx];
            if (Rng.GetRandomIntInRange(0, 100) <= 50) wall.Rotate();
            List<Point> availablePoints = wall.IsHorizontal() ? GameManager.GetAvailablePointsHorizontal() : GameManager.GetAvailablePointsVertical();
            idx = Rng.GetRandomIntInRange(0, availablePoints.Count);
            wall.Position = availablePoints[idx];
            wall.Select();
            GameManager.OnWallPlaced(wall);
            return wall;
        }
    }
}