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
                GameManager.UpdateCellCallback();
            }, () =>
            {
                GameManager.GameState = new VsComputerGameState();
                GameManager.UpdateCellCallback();
            }, g);
        }

        public override void OnCellPressed(Cell cell)
        {
           
        }
    }
}