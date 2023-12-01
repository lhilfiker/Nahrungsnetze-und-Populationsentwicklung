using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Resources;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            this.SuspendLayout();
            // 
            // WelcomeForm
            // 
            this.ClientSize = new System.Drawing.Size(883, 622);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Nahrungsnetz";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nahrungsnetze und Populationsentwicklung";
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.ResumeLayout(false);

        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            // Setze die Größe und Position des Begrüßungsbildschirms
            this.Size = new System.Drawing.Size(800, 600);
            this.CenterToScreen(); // Zentriere das Begrüßungsfenster auf dem Bildschirm

            // Erstelle einen Button für den Dateiauswahldialog
            Button filePickerButton = new Button();
            filePickerButton.Text = "Datei auswählen";
            filePickerButton.Size = new System.Drawing.Size(200, 30);
            filePickerButton.Location = new System.Drawing.Point((this.ClientSize.Width - filePickerButton.Width) / 2, (this.ClientSize.Height - filePickerButton.Height) / 2);
            filePickerButton.Click += FilePickerButton_Click;

            // Füge den Button zum Begrüßungsbildschirm hinzu
            this.Controls.Add(filePickerButton);
        }

        private void FilePickerButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Wähle eine Datei aus";
                openFileDialog.Filter = "Nahrungsnetz Datei (*.json)|*.json";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Hier kannst du die ausgewählte Datei verwenden
                    data.path = openFileDialog.FileName;
                    this.Hide(); // Hide the current form

                    // Create and show the MainForm
                    MainForm mainForm = new MainForm();
                    mainForm.Show();

                }

            }
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
            var result = Database.OpenDatabase(data.path);
            if (result.HasValue)
            {
                // Assign the values from the result to the lists
                (data.Names, data.GetsEatenBy, data.Eats, data.Quantity, data.EatsHowMany, data.FoodOrEater) = result.Value;
            }

            InitializeComponent();

            var sortedLayers = OperationHelper.SortByLayer(data.Names, data.GetsEatenBy, data.Eats, data.FoodOrEater);
            (layerIndexes, layerBoundaries) = sortedLayers;

            InitializePictureBox();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            this.SuspendLayout();
            // Initialisiere das Formular
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "Nahrungsnetz";
            this.Text = "Nahrungsnetze und Populationsentwicklung";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

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
            int totalHeight = data.Names.Count * (itemWidth + verticalSpacing) - verticalSpacing;

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
                    string currentAnimal = data.Names[index];

                    int posX = startX + (i * horizontalSpacing);
                    int posY = startY;

                    g.FillEllipse(Brushes.Red, posX, posY, itemWidth, itemWidth);
                    g.DrawString(currentAnimal, font, textBrush, posX - 10, posY - 20);

                    animalPositions[currentAnimal] = new Point(posX + itemWidth / 2, posY + itemWidth / 2);

                    startY += itemWidth + verticalSpacing;
                }
            }

            // Zeichne Verbindungen basierend auf Fressbeziehungen
            for (int i = 0; i < data.Names.Count; i++)
            {
                string currentAnimal = data.Names[i];
                string predator = data.GetsEatenBy[i];
                string prey = data.Eats[i];

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
