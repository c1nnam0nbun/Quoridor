using System.Drawing;

namespace Quoridor
{
    public class GameStartedState : GameState
    {
        public override void Update()
        {
           
        }

        public override void Draw(Graphics g)
        {
            Size s = new Size(300, 180);
            Point p = new Point(GameManager.Window.Size.Width / 2 - s.Width / 2, GameManager.Window.Size.Height / 2 - s.Height / 2);
            PopupMessage.ShowChooseModeMessage(p, s, () =>
            {
                GameManager.GameState = new VsPlayerGameState();
            }, () =>
            {
                GameManager.GameState = new VsComputerGameState();
            }, g);
        }

        public override Cell RequestNextCellForPlayerOne()
        {
            return null;
        }

        public override Cell RequestNextCellForPlayerTwo()
        {
            return null;
        }

        public override Cell RequestInitialCellForPlayerOne()
        {
            return null;
        }

        public override Cell RequestInitialCellForPlayerTwo()
        {
            return null;
        }

        public override Wall RequestPlacedWallForPlayerOne()
        {
            return null;
        }

        public override Wall RequestPlacedWallForPlayerTwo()
        {
            return null;
        }
    }
}