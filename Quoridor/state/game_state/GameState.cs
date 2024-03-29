﻿using System.Drawing;

namespace Quoridor
{
    public abstract class GameState : IState
    {
        public abstract void Update();

        public abstract void Draw(Graphics g);

        public static void CheckWin()
        {
            if (GameManager.PlayerOne?.CurrentCell.Index.X == 9 || GameManager.PlayerTwo?.CurrentCell.Index.X == 1)
            {
                GameManager.GameReset();
            }
        }

        public abstract void OnTurnChange(TurnState state = null);

        public abstract Cell RequestNextCellForPlayerOne(); 
        
        public abstract Cell RequestNextCellForPlayerTwo();

        public abstract Cell RequestInitialCellForPlayerOne();
        public abstract Cell RequestInitialCellForPlayerTwo();

        public abstract Wall RequestPlacedWallForPlayerOne();
        public abstract Wall RequestPlacedWallForPlayerTwo();
    }
}