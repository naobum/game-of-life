using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life_Game
{
    public partial class Form1 : Form
    {
        private bool isPaused;
        private Graphics graphics;
        private int resolution;
        private GameLogic engine;
        public Form1()
        {
            InitializeComponent();
        }
        private void StartGame()
        {
            if (timer1.Enabled || isPaused) 
                return;

            bPause.Enabled = true;
            bClear.Enabled = true;
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;

            engine = new GameLogic
                (
                    rows: pictureBox1.Height / resolution,
                    cols: pictureBox1.Width / resolution,
                    density: (int)nudDensity.Minimum + (int)nudDensity.Maximum - (int)nudDensity.Value
                );

            Text = $"Generation {engine.CurrentGeneration}";

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }
        private void DrawCurrentGeneration()
        {         
            graphics.Clear(Color.DarkBlue);

            var field = engine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.LightGreen, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }
            pictureBox1.Refresh();
            Text = $"Generation {engine.CurrentGeneration}";
        }
        
        private void PauseGame()
        {
            isPaused = true;
            bStart.Enabled = false;
            bStop.Enabled = false;
            timer1.Enabled = false;
            bPause.Text = "Continue";
        }
        private void ContinueGame()
        {
            isPaused = false;
            bStart.Enabled = true;
            bStop.Enabled = true;
            timer1.Enabled = true;
            bPause.Text = "Pause";
        }

        private void StopGame()
        {
            if (!timer1.Enabled && !isPaused)
                return;
            isPaused = false;
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
            bPause.Enabled = false;
            bClear.Enabled = false;
        }
        private void ClearField()
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            engine.UpdateGeneration();
            DrawCurrentGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                engine.AddCell(x, y);
                if (!timer1.Enabled)
                    DrawCurrentGeneration();
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;   
                engine.RemoveCell(x, y);
                if (!timer1.Enabled)
                    DrawCurrentGeneration();
            }
        }


        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void bPause_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled) PauseGame();
            else ContinueGame();
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            engine.ClearField();
            DrawCurrentGeneration();
        }
    }
}
