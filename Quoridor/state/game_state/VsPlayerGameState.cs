﻿using System.Drawing;

namespace Quoridor
{
    public class VsPlayerGameState : GameState
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

        public override void OnCellPressed(Cell cell)
        {
            
        }
    }
}