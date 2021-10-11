using System;
using System.Windows.Forms;

namespace Quoridor
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GameTimer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize) (this.GameTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // UpdateTimer
            // 
            this.GameTimer.Enabled = true;
            this.GameTimer.Interval = 50D;
            this.GameTimer.SynchronizingObject = this;
            this.GameTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnGameTimerTick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Quoridor";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.MouseDown += new MouseEventHandler(Input.OnMouseDown);
            this.MouseUp += new MouseEventHandler(Input.OnMouseUp);
            this.MouseMove += new MouseEventHandler(Input.OnMouseMoved);
            this.KeyDown += new KeyEventHandler(Input.OnKeyDown);
            this.KeyUp += new KeyEventHandler(Input.OnKeyUp);
            ((System.ComponentModel.ISupportInitialize) (this.GameTimer)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Timers.Timer GameTimer;

        #endregion
    }
}