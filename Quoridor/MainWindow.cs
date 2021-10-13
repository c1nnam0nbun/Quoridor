using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace Quoridor
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            BackColor = Color.Bisque;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            GameManager.Init(this);
        }
        
        private void OnPaint(object sender, PaintEventArgs e)
        {
            GameManager.GameDraw(e.Graphics);
            GameManager.GameStart(e.Graphics);
        }
        
        private void OnGameTimerTick(object sender, ElapsedEventArgs e)
        {
            GameManager.GameUpdate();
            Refresh();
        }
    }
}