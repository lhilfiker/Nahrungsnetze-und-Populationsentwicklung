using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Initialisiere die Steuerelemente des Begrüßungsbildschirms
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Name = "WelcomeForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Welcome";
            this.Load += new EventHandler(WelcomeForm_Load);
            this.ResumeLayout(false);
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            // Setze die Größe und Position des Begrüßungsbildschirms
            this.Size = new System.Drawing.Size(400, 300);
            this.CenterToScreen(); // Zentriere das Begrüßungsfenster auf dem Bildschirm

            // Erstelle einen Button für den Wechsel zum Hauptformular
            Button switchButton = new Button();
            switchButton.Text = "Zum Hauptformular wechseln";
            switchButton.Size = new System.Drawing.Size(200, 30);
            switchButton.Location = new System.Drawing.Point(100, 120);
            switchButton.Click += SwitchButton_Click;

            // Füge den Button zum Begrüßungsbildschirm hinzu
            this.Controls.Add(switchButton);
        }

        private void SwitchButton_Click(object sender, EventArgs e)
        {
            // Erstelle das Hauptformular und zeige es an
            MainForm mainForm = new MainForm();
            mainForm.Show(); // Verwende Show(), nicht ShowDialog()

            // Schließe das Begrüßungsfenster
            this.Hide(); // Verwende Hide(), um das Begrüßungsfenster zu verbergen
        }
    }

    public partial class MainForm : Form
    {
        public List<string> Names = new List<string>
            {
                "Blume", "Käfer", "Spinne", "Kleiner Vogel", "Wurm",
                "Frosch", "Maus", "Schlange", "Großer Vogel", "Fuchs",
                "Hase", "Eule", "Hirsch", "Wolf", "Pilz"
            };

        public List<string> GetsEatenBy = new List<string>
            {
                "", "Kleiner Vogel", "Kleiner Vogel", "Großer Vogel", "Kleiner Vogel",
                "Schlange", "Fuchs", "Großer Vogel", "Eule", "Wolf",
                "Fuchs", "Wolf", "Wolf", "", "Käfer"
            };

        public List<string> Eats = new List<string>
            {
                "", "Pilz", "Käfer", "Wurm", "",
                "Wurm", "Blume", "Frosch", "Maus", "Hase",
                "Blume", "Kleiner Vogel", "Blume", "Hirsch", ""
            };

        public List<float> Quantity = new List<float>
            {
                1000, 500, 300, 200, 600,
                150, 400, 100, 150, 70,
                350, 60, 80, 50, 900
            };

        public List<float> EatsHowMany = new List<float>
            {
                0, 0.05f, 0.1f, 0.2f, 0.1f,
                0.15f, 0.1f, 0.25f, 0.3f, 0.35f,
                0.1f, 0.4f, 0.5f, 0.6f, 0
            };

        public List<bool> FoodOrEater = new List<bool>
            {
                true, false, false, false, false,
                false, false, false, false, false,
                false, false, false, false, true
            };

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();
            Application.Run(new WelcomeForm());
        }

        private List<int> layerIndexes;
        private List<int> layerBoundaries;
        private PictureBox pictureBox;

        public MainForm()
        {
            InitializeComponent();

            var sortedLayers = OperationHelper.SortByLayer(Names, GetsEatenBy, Eats, FoodOrEater);
            (layerIndexes, layerBoundaries) = sortedLayers;

            InitializePictureBox();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // Initialisiere das Formular
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawFoodWeb(e.Graphics);
        }

        private void InitializePictureBox()
        {
            pictureBox = new PictureBox();
            pictureBox.Size = new Size(800, 600); // Anpassen der Größe
            pictureBox.Paint += PictureBox_Paint;

            this.Controls.Add(pictureBox); // PictureBox zum Formular hinzufügen
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            DrawFoodWeb(e.Graphics);
        }

        private void DrawFoodWeb(Graphics g)
        {
            Font font = new Font("Arial", 10);
            Brush textBrush = Brushes.Black;
            Pen linePen = new Pen(Brushes.Black, 2);

            int horizontalSpacing = 150;
            int verticalSpacing = 120;
            int itemWidth = 30;

            int totalWidth = (layerBoundaries.Count - 1) * horizontalSpacing + itemWidth;
            int totalHeight = Names.Count * (itemWidth + verticalSpacing) - verticalSpacing;

            int startX = Math.Max((pictureBox.ClientSize.Width - totalWidth) / 2, 0);
            int startY = Math.Max((pictureBox.ClientSize.Height - totalHeight) / 2, 0);

            Dictionary<string, Point> animalPositions = new Dictionary<string, Point>();

            for (int i = 0; i < layerBoundaries.Count; i++)
            {
                int boundary = layerBoundaries[i];

                int totalItemsWidth = (boundary - ((i == 0) ? 0 : layerBoundaries[i - 1])) * (itemWidth + verticalSpacing) - verticalSpacing;
                int currentLayerHeight = totalItemsWidth;

                if (currentLayerHeight > pictureBox.ClientSize.Height)
                {
                    startY = 0;
                    totalHeight = pictureBox.ClientSize.Height;
                }
                else
                {
                    startY = Math.Max((pictureBox.ClientSize.Height - currentLayerHeight) / 2, 0);
                    totalHeight = currentLayerHeight;
                }

                for (int j = (i == 0) ? 0 : layerBoundaries[i - 1]; j < boundary; j++)
                {
                    int index = layerIndexes[j];
                    string currentAnimal = Names[index];

                    int posX = startX + (i * horizontalSpacing);
                    int posY = startY;

                    g.FillEllipse(Brushes.Red, posX, posY, itemWidth, itemWidth);
                    g.DrawString(currentAnimal, font, textBrush, posX - 10, posY - 20);

                    animalPositions[currentAnimal] = new Point(posX + itemWidth / 2, posY + itemWidth / 2);

                    startY += itemWidth + verticalSpacing;
                }
            }

            // Zeichne Verbindungen basierend auf Fressbeziehungen
            for (int i = 0; i < Names.Count; i++)
            {
                string currentAnimal = Names[i];
                string predator = GetsEatenBy[i];
                string prey = Eats[i];

                if (!string.IsNullOrEmpty(predator) && animalPositions.ContainsKey(predator) && animalPositions.ContainsKey(currentAnimal))
                {
                    Point predatorPos = animalPositions[predator];
                    Point currentPos = animalPositions[currentAnimal];

                    g.DrawLine(linePen, predatorPos.X, predatorPos.Y, currentPos.X, currentPos.Y);
                }

                if (!string.IsNullOrEmpty(prey) && animalPositions.ContainsKey(prey) && animalPositions.ContainsKey(currentAnimal))
                {
                    Point preyPos = animalPositions[prey];
                    Point currentPos = animalPositions[currentAnimal];

                    g.DrawLine(linePen, currentPos.X, currentPos.Y, preyPos.X, preyPos.Y);
                }
            }
        }
    }
}
