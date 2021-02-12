using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Creating the Field

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(28, 28);
                    btn.Location = new Point(252 + (i * 28), 84 + (j * 28));
                    btn.Name = $"button{i}x{j}";
                    btn.Click += new System.EventHandler(btn_Click);
                    btn.Enabled = true;
                    btn.Font = new Font("Tahoma", 10.0F, FontStyle.Bold);
                    this.Controls.Add(btn);
                }
            }

            // Creating Random Mines 
            int mineCount = 0;

            Random rnd = new Random();

            do
            {
                int mine_x = rnd.Next(0, 10);
                int mine_y = rnd.Next(0, 10);

                foreach (Button btn in this.Controls)
                {
                    if (btn.Name == $"button{mine_x}x{mine_y}" && btn.Text != "M")
                    {
                        btn.Text = "M";
                        btn.ForeColor = Color.FromArgb(255, 0, 0);
                        mineCount++;
                        continue;
                    }
                }

            } while (mineCount < 10);


            // Giving Buttons values about how many Mines around them;

            foreach (Button btn in this.Controls)
            {
                int howManyMines = 0;
                if (btn.Text != "M")
                {
                    foreach (Button btnIn in this.Controls)
                    {
                        // Checking NW
                        if (btn.Location.X - 28 == btnIn.Location.X && btn.Location.Y + 28 == btnIn.Location.Y && btnIn.Text == "M")
                        {
                            howManyMines += 1;
                        }
                        //Checking N
                        if (btn.Location.X == btnIn.Location.X && btn.Location.Y + 28 == btnIn.Location.Y && btnIn.Text == "M")
                        {
                            howManyMines += 1;
                        }
                        //Checking NE
                        if (btn.Location.X + 28 == btnIn.Location.X && btn.Location.Y + 28 == btnIn.Location.Y && btnIn.Text == "M")
                        {
                            howManyMines += 1;
                        }
                        //Checking E
                        if (btn.Location.X + 28 == btnIn.Location.X && btn.Location.Y == btnIn.Location.Y && btnIn.Text == "M")
                        {
                            howManyMines += 1;
                        }
                        //Checking SE
                        if (btn.Location.X + 28 == btnIn.Location.X && btn.Location.Y - 28 == btnIn.Location.Y && btnIn.Text == "M")
                        {
                            howManyMines += 1;
                        }
                        //Checking S
                        if (btn.Location.X == btnIn.Location.X && btn.Location.Y - 28 == btnIn.Location.Y && btnIn.Text == "M")
                        {
                            howManyMines += 1;
                        }
                        //Checking SW
                        if (btn.Location.X - 28 == btnIn.Location.X && btn.Location.Y - 28 == btnIn.Location.Y && btnIn.Text == "M")
                        {
                            howManyMines += 1;
                        }
                        //Checking W
                        if (btn.Location.X - 28 == btnIn.Location.X && btn.Location.Y == btnIn.Location.Y && btnIn.Text == "M")
                        {
                            howManyMines += 1;
                        }

                        btn.Text = howManyMines.ToString();



                    }
                }
            }
            foreach (Button btn in this.Controls)
            {
                if (btn.Text == "0")
                {
                    btn.Text = string.Empty;
                }
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            // Using sender as Button because i need to reach that buttons properties.
            Button btn = sender as Button;
            if (btn.Text == "M")
            {
                btn.BackColor = Color.FromArgb(255, 0, 0);
                foreach (Button button in this.Controls)
                {
                    button.Enabled = false;
                }
            }
            else if (btn.Text == string.Empty)
            {
                btn.Enabled = false;
                LoopControlEmpty(btn, GetAllButtons());
            }
            else
                btn.Enabled = false;
        }

        
        public IList<Button> GetAllButtons()
        {
            var buttonsToProcess = new List<Button>();
            foreach (Button btnIn in this.Controls)
            {
                buttonsToProcess.Add(btnIn);
            }
            return buttonsToProcess;
        }
        

        public void LoopControlEmpty(Button btn, IList<Button> buttonsToProcess)
        {
            var neighbors = new List<Button>();
            neighbors.Clear();

            btn.Enabled = false;
            buttonsToProcess.Remove(btn);

            // Button.Enable = false; if buttons have no text inside and neighbor to another empty button and create a recursion function for it.

            foreach(Button btnNeighbors in buttonsToProcess)
            {
                
                if (btn.Location.X == btnNeighbors.Location.X && btn.Location.Y+28 == btnNeighbors.Location.Y && btnNeighbors.Text == "" && btnNeighbors.Enabled == true)
                {
                    neighbors.Add(btnNeighbors);
                }
                if (btn.Location.X+28 == btnNeighbors.Location.X && btn.Location.Y == btnNeighbors.Location.Y && btnNeighbors.Text == "" && btnNeighbors.Enabled == true)
                {
                    neighbors.Add(btnNeighbors);
                }
               
                if (btn.Location.X == btnNeighbors.Location.X && btn.Location.Y-28 == btnNeighbors.Location.Y && btnNeighbors.Text == "" && btnNeighbors.Enabled == true)
                {
                    neighbors.Add(btnNeighbors);
                }
                
                if (btn.Location.X-28 == btnNeighbors.Location.X && btn.Location.Y == btnNeighbors.Location.Y && btnNeighbors.Text == "" && btnNeighbors.Enabled == true)
                {
                    neighbors.Add(btnNeighbors);
                }
               
            }

            foreach (Button btns in neighbors)
            {
                buttonsToProcess.Remove(btns);
            }

            foreach (Button btns in neighbors)
            {
                btns.Enabled = false;
            }

            foreach (Button btns in neighbors)
            {
                LoopControlEmpty(btns, buttonsToProcess);
            }

            
            // So now we disable every empty buttons around where i click. But i need to disable neigbors which have text in it.

            foreach (Button btnEmpty in this.Controls)
            {
                if (btnEmpty.Text == "" && btnEmpty.Enabled == false)
                {
                    foreach (Button btnTextNeigbors in this.Controls)
                    {
                        // N
                        if (btnEmpty.Location.X == btnTextNeigbors.Location.X && btnEmpty.Location.Y + 28 == btnTextNeigbors.Location.Y && btnTextNeigbors.Enabled == true)
                        {
                            if (btnTextNeigbors.Text != "" && btnTextNeigbors.Text != "M")
                            {
                                btnTextNeigbors.Enabled = false;
                                buttonsToProcess.Remove(btnTextNeigbors);
                            }
                        }
                        if (btnEmpty.Location.X == btnTextNeigbors.Location.X && btnEmpty.Location.Y - 28 == btnTextNeigbors.Location.Y && btnTextNeigbors.Enabled == true)
                        {
                            if (btnTextNeigbors.Text != "" && btnTextNeigbors.Text != "M")
                            {
                                btnTextNeigbors.Enabled = false;
                                buttonsToProcess.Remove(btnTextNeigbors);
                            }
                        }
                        if (btnEmpty.Location.X + 28 == btnTextNeigbors.Location.X && btnEmpty.Location.Y == btnTextNeigbors.Location.Y && btnTextNeigbors.Enabled == true)
                        {
                            if (btnTextNeigbors.Text != "" && btnTextNeigbors.Text != "M")
                            {
                                btnTextNeigbors.Enabled = false;
                                buttonsToProcess.Remove(btnTextNeigbors);
                            }
                        }
                        if (btnEmpty.Location.X - 28 == btnTextNeigbors.Location.X && btnEmpty.Location.Y == btnTextNeigbors.Location.Y && btnTextNeigbors.Enabled == true)
                        {
                            if (btnTextNeigbors.Text != "" && btnTextNeigbors.Text != "M")
                            {
                                btnTextNeigbors.Enabled = false;
                                buttonsToProcess.Remove(btnTextNeigbors);
                            }
                        }
                    }
                }
            }

        return;

        }
    }
}
