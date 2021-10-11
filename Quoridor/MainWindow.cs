using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace Quoridor
{
    public partial class MainWindow : Form
    {
        private readonly Board board;
        public MainWindow()
        {
            InitializeComponent();
            board = new Board(this);
            BackColor = Color.Bisque;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        
        private void OnPaint(object sender, PaintEventArgs e)
        {
            board.Draw(e.Graphics);
        }
        
        private void OnGameTimerTick(object sender, ElapsedEventArgs e)
        {
            board?.Update();
            Refresh();
        }
    }
}