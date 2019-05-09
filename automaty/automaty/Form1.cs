using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automaty
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Pen pen;
        private SolidBrush brush;
        private SolidBrush brush2;
        private Color color = new Color();
        private int width;
        private List<int> rowFirst = new List<int>();
        private List<int> rowComputed = new List<int>();
        private int[,] lifeTab;
        private int[,] lifeTabComputed;
        private int startPosition;
        private int startPositionHeight;
        private BackgroundWorker worker = null;

        public int PIXEL_SIZE;
        public readonly int DEFAULT_SIZE = 1;
        public readonly int[] RULE30 = { 0, 0, 0, 1, 1, 1, 1, 0 };
        public readonly int[] RULE60 = { 0, 0, 1, 1, 1, 1, 0, 0 };
        public readonly int[] RULE90 = { 0, 1, 0, 1, 1, 0, 1, 0 };
        public readonly int[] RULE120 = { 0, 1, 1, 1, 1, 0, 0, 0 };
        public readonly int[] RULE250 = { 1, 1, 1, 1, 1, 0, 1, 0 };


        public Form1()
        {
            InitializeComponent();
            pen = new Pen(Color.Black);
            g = pictureBox1.CreateGraphics();
            brush = new SolidBrush(Color.Black);
            brush2 = new SolidBrush(Color.White);

            comboBox1.Items.Add("Rule 30");
            comboBox1.Items.Add("Rule 60");
            comboBox1.Items.Add("Rule 90");
            comboBox1.Items.Add("Rule 120");
            comboBox1.Items.Add("Rule 250");
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Add("Rozrost 1D");
            comboBox2.Items.Add("Gra w życie");
            comboBox2.SelectedIndex = 0;
            textBox1.Text = 200.ToString();
            width = int.Parse(textBox1.Text);
            
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            width = int.Parse(textBox1.Text);
            if (comboBox2.SelectedIndex == 0)
                compute1D();
            else if (comboBox2.SelectedIndex == 1)
                gameOfLife();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            showButtons();
            worker.CancelAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
        }










        private void gameOfLife()
        {
            g.Clear(Color.White);
            PIXEL_SIZE = 7;
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            int prefAmount = 0;
            if (textBox2.TextLength > 0)
                prefAmount = int.Parse(textBox2.Text);
            int amount = 0;
            int neighbourNumber = 0;

            lifeTab = new int[width, width];
            int[,] emptyTab = new int[width, width];
            lifeTabComputed = new int[width, width];
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < width; ++j)
                {
                    lifeTab[i, j] = 0;
                    emptyTab[i, 1] = 0;
                }

            /* ------Stable structure 
            lifeTab[3, 2] = 1;
            lifeTab[4, 2] = 1;
            lifeTab[2, 3] = 1;
            lifeTab[5, 3] = 1;
            lifeTab[3, 4] = 1;
            lifeTab[4, 4] = 1;*/

            /* ----Oscylatory
            lifeTab[4, 2] = 1;
            lifeTab[4, 3] = 1;
            lifeTab[4, 4] = 1;*/

            /* ----Glider  
            lifeTab[3, 2] = 1;
            lifeTab[4, 2] = 1;
            lifeTab[2, 3] = 1;
            lifeTab[3, 3] = 1;
            lifeTab[4, 4] = 1;*/

            /*--- Glider Gun */
            lifeTab[1,10] = 1;
            lifeTab[2,10] = 1;
            lifeTab[1,11] = 1;
            lifeTab[2,11] = 1;

            lifeTab[13, 9] = 1;
            lifeTab[13, 10] = 1;
            lifeTab[13, 11] = 1;
            lifeTab[13, 12] = 1;
            lifeTab[13, 13] = 1;
            lifeTab[14, 8] = 1;
            lifeTab[14, 10] = 1;
            lifeTab[14, 11] = 1;
            lifeTab[14, 12] = 1;
            lifeTab[14, 14] = 1;
            lifeTab[15, 9] = 1;
            lifeTab[15, 13] = 1;
            lifeTab[16, 10] = 1;
            lifeTab[16, 11] = 1;
            lifeTab[16, 12] = 1;
            lifeTab[17, 11] = 1;

            lifeTab[21, 9] = 1;
            lifeTab[22, 9] = 1;
            lifeTab[23, 8] = 1;
            lifeTab[23, 10] = 1;
            lifeTab[24, 7] = 1;
            lifeTab[24, 8] = 1;
            lifeTab[24, 10] = 1;
            lifeTab[24, 11] = 1;
            lifeTab[25, 6] = 1;
            lifeTab[25, 12] = 1;
            lifeTab[26, 9] = 1;
            lifeTab[27, 6] = 1;
            lifeTab[27, 7] = 1;
            lifeTab[27, 11] = 1;
            lifeTab[27, 12] = 1;

            lifeTab[30, 10] = 1;
            lifeTab[31, 10] = 1;
            lifeTab[32, 11] = 1;

            lifeTab[35, 8] = 1;
            lifeTab[35, 9] = 1;
            lifeTab[36, 8] = 1;
            lifeTab[36, 9] = 1; 



            /* ----Something 
            lifeTab[7, 3] = 1;    
            lifeTab[7, 5] = 1;    
            lifeTab[9, 7] = 1;    
            lifeTab[9, 6] = 1;    
            lifeTab[9, 8] = 1;    
            lifeTab[5, 2] = 1;      
            lifeTab[6, 9] = 1;    
            lifeTab[5, 8] = 1;    
            lifeTab[4, 8] = 1;    
            lifeTab[5, 9] = 1;    
            lifeTab[3, 4] = 1; */

            /*random 
            Random rnd = new Random();
            for(int i =0; i< width*4; ++i)
            {
                int x = rnd.Next(0, width);
                int y = rnd.Next(0, width);
                lifeTab[x, y] = 1;
            }*/

            startPosition = computeStartPosition(width);
            startPositionHeight = computeStartPositionHeight(width);

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < width; ++j)
                    drawCurrent(lifeTab, i, j);

            worker.DoWork += new DoWorkEventHandler((state, args) =>
            {
                do
                {
                    
                    if (amount % 5 == 0)
                        color = Color.FromArgb(255, 53, 53);
                    if (amount % 5 == 1)
                        color = Color.FromArgb(255, 251, 53);
                    if (amount % 5 == 2)
                        color = Color.FromArgb(53, 255, 80);
                    if (amount % 5 == 3)
                        color = Color.FromArgb(53, 147, 255);
                    if (amount % 5 == 4)
                        color = Color.FromArgb(181, 53, 255);

                    brush.Color = color;

                    //System.Threading.Thread.Sleep(100);

                    amount++;
                    if (worker.CancellationPending)
                        break;

                    for (int i = 0; i < width; ++i)
                        for (int j = 0; j < width; ++j)
                        {
                            neighbourNumber = computeNeighbours(lifeTab, i, j);
                            lifeTabComputed[i, j] = checkIfAlive(neighbourNumber, lifeTab[i,j]);
                            drawCurrent(lifeTabComputed, i, j);
                        }
                    for (int i = 0; i < width; ++i)
                        for (int j = 0; j < width; ++j)
                            lifeTab[i, j] = lifeTabComputed[i, j];

                    if (prefAmount != 0)
                        if (amount >= prefAmount)
                            break;

                } while (true);
            });

            worker.RunWorkerAsync();
            hideButtons();
            comboBox1.Enabled = false;
            this.Invalidate(); 
        }


        public void compute1D()
        {
            g.Clear(Color.White);
            brush.Color = Color.Black;
            PIXEL_SIZE = DEFAULT_SIZE;
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;

            int prefAmount = 0;
            if (textBox2.TextLength > 0 && int.Parse(textBox2.Text) >0 ) 
                prefAmount = int.Parse(textBox2.Text);

            if (rowFirst.Count < width)
                for (int i = 0; i < width; ++i)
                    rowFirst.Add(0);
            for (int i = 0; i < width; ++i)
                rowFirst[i] = 0;

            int[] rule = new int[8];
            if (width % 2 != 0) width--;
            rowFirst[width / 2] = 1;

            startPosition = computeStartPosition(width);
            int selected = this.comboBox1.SelectedIndex;

            if (selected == 0)
                for (int i = 0; i < 8; ++i)
                    rule[i] = RULE30[i];
            if (selected == 1)
                for (int i = 0; i < 8; ++i)
                    rule[i] = RULE60[i];
            if (selected == 2)
                for (int i = 0; i < 8; ++i)
                    rule[i] = RULE90[i];
            if (selected == 3)
                for (int i = 0; i < 8; ++i)
                    rule[i] = RULE120[i];
            if (selected == 4)
                for (int i = 0; i < 8; ++i)
                    rule[i] = RULE250[i];

            if (textBox3.TextLength > 0 && int.Parse(textBox3.Text) <=255 && int.Parse(textBox3.Text) > 0)
            {
                int rul = int.Parse(textBox3.Text);
                rule = computeRule(rul);
            }


            drawRow(rowFirst, 0);
            int iteracja = 0;
            int amount = 0;

            worker.DoWork += new DoWorkEventHandler((state, args) =>
            {
                do
                {
                    if (worker.CancellationPending)
                        break;

                    iteracja++;
                    amount++;
                    rowComputed = computeNextRow(rowFirst, rule);
                    drawRow(rowComputed, iteracja);
                    for (int i = 0; i < width; ++i)
                        rowFirst[i] = rowComputed[i];
                   

                    if (iteracja * PIXEL_SIZE >= pictureBox1.Width)
                    {
                        g.Clear(Color.White);
                        iteracja = 0;
                    }

                    if (prefAmount != 0)
                        if (amount >= prefAmount)
                            break;
                } while ( true);
            });

            worker.RunWorkerAsync();
            hideButtons();
        }













        private int[] computeRule(int value)
        {
            int[] rule = new int[8];
            string binary = Convert.ToString(value, 2);
            
            for (int i = 0; i < 8-(binary.Length) ; ++i)
                rule[i] = 0;

            int j = 0;
            for (int i = 8 - (binary.Length) ; i < 8; ++i)
            {
                rule[i] = binary[j] - '0';
                j++;
            }

            for (int i = 0; i < 8; ++i)
                Console.WriteLine(rule[i]);
            return rule;
        }

        private int checkIfAlive(int count, int state)
        {
            if (state == 0 && count == 3)
                return 1;
            else if ((state == 1 && count == 2) || (state == 1 && count == 3))
                return 1;
            else if (state == 1 && count > 3)
                return 0;
            else if (state == 1 && count < 2)
                return 0;
            return 0;
        }

        private int computeNeighbours(int[,] tab, int pos, int iteracja)
        {
            int count = 0;
            if (iteracja == 0)
            {
                count += lookForNeighbour(tab, pos, iteracja);
                count += lookForNeighbour(tab, pos, iteracja +1);
            }
            else if (iteracja == (width - 1))
            {
                count += lookForNeighbour(tab, pos, iteracja);
                count += lookForNeighbour(tab, pos, iteracja - 1);
            }
            else
            {
                count += lookForNeighbour(tab, pos, iteracja - 1);
                count += lookForNeighbour(tab, pos, iteracja);
                count += lookForNeighbour(tab, pos, iteracja + 1);
            }
            if (tab[pos, iteracja] == 1)
                count--;
            return count;
        }

        private int lookForNeighbour(int[,] tab, int pos, int iteracja)
        {
            int count = 0;
            if(pos == 0)
            {
                if (tab[pos, iteracja] == 1)
                    count++;
                if (tab[pos +1 , iteracja] == 1)
                    count++;
            }
            else if(pos == (width - 1))
            {
                if (tab[pos, iteracja] == 1)
                    count++;
                if (tab[pos - 1, iteracja] == 1)
                    count++;
            }
            else
            {
                if (tab[pos -1, iteracja] == 1)
                    count++;
                if (tab[pos, iteracja] == 1)
                    count++;
                if (tab[pos + 1, iteracja] == 1)
                    count++;
            }

            return count;
        }

        private void hideButtons()
        {
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
            comboBox1.Enabled = false;
            textBox2.Enabled = false;
            Invalidate();
        }

        private void showButtons()
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            comboBox2.Enabled = true;
            textBox1.Enabled = true;
            comboBox1.Enabled = true;
            textBox2.Enabled = true;

        }


        private void drawCurrent(int[,] tab, int pos, int iteracja)
        {
            if(tab[pos,iteracja] == 1)
            {
                g.FillRectangle(brush, (startPosition + pos * PIXEL_SIZE), ( startPositionHeight + iteracja * PIXEL_SIZE), PIXEL_SIZE, PIXEL_SIZE);
            }
            else
                g.FillRectangle(brush2, (startPosition + pos * PIXEL_SIZE), (startPositionHeight + iteracja * PIXEL_SIZE), PIXEL_SIZE, PIXEL_SIZE);

        }

        private void drawRow(List<int> row, int iteracja)
        {
            int col = 0;
            foreach (int val in row)
            {
                if (val == 1)
                    g.FillRectangle(brush, (startPosition + col * PIXEL_SIZE), (iteracja * PIXEL_SIZE), PIXEL_SIZE, PIXEL_SIZE);
                col++;
            }
        }

        private int computeStartPosition(int width)
        {
            int position;
            int size = pictureBox1.Width;
            if (width % 2 != 0) width--;
            position = size / 2 - (PIXEL_SIZE * (width/2));
            return position;
        }

        private int computeStartPositionHeight(int height)
        {
            int position;
            int size = pictureBox1.Height;
            if (width % 2 != 0) width--;
            position = size / 2 - (PIXEL_SIZE * (height / 2));
            return position;
        }

        private List<int> computeNextRow(List<int> rowStart, int[] rule)
        {
            List<int> rowNew = new List<int>();
            int[] tab = new int[3];
            for (int i = 0; i < width; ++i)
            {
                if (i == 0)
                {
                    tab[0] = rowStart[width - 1];
                    tab[1] = rowStart[i];
                    tab[2] = rowStart[i + 1];
                }
                else if (i == (width - 1))
                {
                    tab[0] = rowStart[i - 1];
                    tab[1] = rowStart[i];
                    tab[2] = rowStart[0];
                }
                else
                {
                    tab[0] = rowStart[i - 1];
                    tab[1] = rowStart[i];
                    tab[2] = rowStart[i + 1];
                }

                rowNew.Add(computeNextValue(tab, rule));
            }
            return rowNew;
        }

        private int computeNextValue(int[] tab, int[] rule)
        {
            if (tab[0] == 1 && tab[1] == 1 && tab[2] == 1)
                return rule[0];
            if (tab[0] == 1 && tab[1] == 1 && tab[2] == 0)
                return rule[1];
            if (tab[0] == 1 && tab[1] == 0 && tab[2] == 1)
                return rule[2];
            if (tab[0] == 1 && tab[1] == 0 && tab[2] == 0)
                return rule[3];
            if (tab[0] == 0 && tab[1] == 1 && tab[2] == 1)
                return rule[4];
            if (tab[0] == 0 && tab[1] == 1 && tab[2] == 0)
                return rule[5];
            if (tab[0] == 0 && tab[1] == 0 && tab[2] == 1)
                return rule[6];
            if (tab[0] == 0 && tab[1] == 0 && tab[2] == 0)
                return rule[7];
            return 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 1)
            {
                comboBox1.Enabled = false;
                textBox1.Text = 50.ToString();
                textBox2.Text = 50.ToString();
                textBox3.Enabled = false;
                textBox3.Text = "";
            }
            else
            {
                comboBox1.Enabled = true;
                textBox3.Enabled = true;
                textBox1.Text = 200.ToString();
                textBox2.Text = "";
                textBox3.Text = "";
            }
        }
        
    }
}
