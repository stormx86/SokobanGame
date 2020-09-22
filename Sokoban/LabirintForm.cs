using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sokoban
{
    public delegate void deShowItem(Place place, Cell item);
    public delegate void deShowStat(int placed, int totals);
    public partial class LabirintForm : Form
    {
        int level_nr;
        int last_level_nr;
        int width, height;
        PictureBox[,] box;
        Game game;
        public LabirintForm()
        {
            InitializeComponent();
          
            game = new Game(ShowItem, ShowStat);
        }

        public void OpenLevel(int level_nr)
        {
           
            this.level_nr = level_nr;
            if (!game.Init(level_nr, out width, out height))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                return;
            }
            

            InitPictures();
            game.ShowLevel();
         
           
        }

        public void InitPictures()
        { 
        box = new PictureBox[width, height];

        int bw, bh;
        bw = panel.Width / width;
        bh = panel.Height / height;
        if (bw < bh) bh = bw;
        else bw = bh;

        
        panel.Controls.Clear();
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                PictureBox picture = new PictureBox();
                picture.BackColor = Color.White;
                picture.BorderStyle = BorderStyle.FixedSingle;
                picture.Location = new Point(x * (bw - 1), y * (bh - 1));
                picture.Size = new Size(bw, bh);
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.Tag = new Place(x, y);
                panel.Controls.Add(picture);
                box[x, y] = picture;
            }
        }

        public void ShowItem(Place place, Cell item)
        {
            box[place.x, place.y].Image = CellToPicture(item);
        }

       

        public void ShowStat(int placed, int totals)
        {
            toolStat.Text = placed.ToString() + " of " + totals.ToString();
            toolLevel.Text = level_nr.ToString();
            toolDone.Visible = placed == totals;
        }

        private Image CellToPicture(Cell cell)
        {
            switch (cell)
            {
                case Cell.none: return Properties.Resources.none;
                case Cell.abox: return Properties.Resources.abox;
                case Cell.done: return Properties.Resources.done;
                case Cell.here: return Properties.Resources.here;
                case Cell.wall: return Properties.Resources.wall;
                case Cell.user: return Properties.Resources.user;
                default: return Properties.Resources.none;
            }
        }

        private void toolPrevLevel_Click(object sender, EventArgs e)
        {
            if (level_nr > 1)
                OpenLevel(level_nr - 1);
        }

        private void toolNextLevel_Click(object sender, EventArgs e)
        {
            OpenLevel(level_nr + 1);
        }

        private void LabirintForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: game.Step(-1, 0); break;
                case Keys.Right: game.Step(1, 0); break;
                case Keys.Up: game.Step(0, -1); break;
                case Keys.Down: game.Step(0, 1); break;
                case Keys.Escape: RestartLevel(); break;
            }
        }

        private void RestartLevel()
        {
            game.Init(level_nr, out width, out height);
            game.ShowLevel();   
        }

        private void toolRestart_Click(object sender, EventArgs e)
        {
            RestartLevel();
        }

        private void LabirintForm_Resize(object sender, EventArgs e)
        {
            int bw, bh;
            bw = panel.Width / width;
            bh = panel.Height / height;
            if (bw < bh) bh = bw;
            else bw = bh;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                  
                    box[x,y].Location = new System.Drawing.Point(x * (bw - 1), y * (bh - 1));
                    box[x,y].Size = new System.Drawing.Size(bw, bh);
                   
                }
        }
    }
}
