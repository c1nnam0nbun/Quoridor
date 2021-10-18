using System.Drawing;

namespace Quoridor
{
    public abstract class GameState : State
    {
        public abstract void Update();

        public abstract void Draw(Graphics g);

        public abstract void OnCellPressed(Cell cell);
    }
}