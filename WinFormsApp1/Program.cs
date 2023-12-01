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
        private TextBox txtName;
        private TextBox txtIsst;
        private TextBox txtWirdGegessenVon;
        private CheckBox chkEssen;
        private NumericUpDown numAnzahl;
        private NumericUpDown numIsstWieViele;
        private Button btnAdd;

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
            InitializeInputFields();
            InitializeAddButton();

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

            // Calculate size as 80% of form's width and 90% of form's height
            int pictureBoxWidth = (int)(this.ClientSize.Width * 0.8);
            int pictureBoxHeight = (int)(this.ClientSize.Height * 0.9);

            pictureBox.Size = new Size(pictureBoxWidth, pictureBoxHeight);
            pictureBox.Location = new Point(0, 0); // Positioned at the top-left corner
            pictureBox.Paint += PictureBox_Paint;

            this.Controls.Add(pictureBox);
        }

        private void InitializeInputFields()
        {
            // Labels erstellen
            Label lblName = new Label();
            lblName.Text = "Name";
            lblName.Location = new Point(580, 50);
            lblName.Size = new Size(70, 20);

            Label lblIsst = new Label();
            lblIsst.Text = "Isst";
            lblIsst.Location = new Point(580, 80);
            lblIsst.Size = new Size(70, 20);

            Label lblWirdGegessenVon = new Label();
            lblWirdGegessenVon.Text = "Wird gegessen von";
            lblWirdGegessenVon.Location = new Point(530, 110);
            lblWirdGegessenVon.Size = new Size(120, 20);

            Label lblEssen = new Label();
            lblEssen.Text = "Essen?";
            lblEssen.Location = new Point(580, 140);
            lblEssen.Size = new Size(70, 20);

            Label lblAnzahl = new Label();
            lblAnzahl.Text = "Anzahl";
            lblAnzahl.Location = new Point(580, 170);
            lblAnzahl.Size = new Size(70, 20);

            Label lblIsstWieViele = new Label();
            lblIsstWieViele.Text = "Isst wie viele";
            lblIsstWieViele.Location = new Point(530, 200);
            lblIsstWieViele.Size = new Size(120, 20);

            // Textfelder und Steuerelemente erstellen
            txtName = new TextBox();
            txtIsst = new TextBox();
            txtWirdGegessenVon = new TextBox();
            chkEssen = new CheckBox();
            numAnzahl = new NumericUpDown();
            numIsstWieViele = new NumericUpDown();

            // Positionen und Größen setzen
            txtName.Location = new Point(650, 50);
            txtIsst.Location = new Point(650, 80);
            txtWirdGegessenVon.Location = new Point(650, 110);
            chkEssen.Location = new Point(650, 140);
            numAnzahl.Location = new Point(650, 170);
            numIsstWieViele.Location = new Point(650, 200);

            // Steuerelemente zum Formular hinzufügen
            this.Controls.Add(lblName);
            this.Controls.Add(lblIsst);
            this.Controls.Add(lblWirdGegessenVon);
            this.Controls.Add(lblEssen);
            this.Controls.Add(lblAnzahl);
            this.Controls.Add(lblIsstWieViele);

            this.Controls.Add(txtName);
            this.Controls.Add(txtIsst);
            this.Controls.Add(txtWirdGegessenVon);
            this.Controls.Add(chkEssen);
            this.Controls.Add(numAnzahl);
            this.Controls.Add(numIsstWieViele);
        }



        private void InitializeAddButton()
        {
            btnAdd = new Button();
            btnAdd.Location = new Point(650, 230);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Text = "Hinzufügen";
            btnAdd.Click += new EventHandler(AddButton_Click);

            this.Controls.Add(btnAdd);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string isst = txtIsst.Text;
            string wirdGegessenVon = txtWirdGegessenVon.Text;
            bool essen = chkEssen.Checked;
            int anzahl = (int)numAnzahl.Value;
            int isstWieViele = (int)numIsstWieViele.Value;

            data.Names.Add(name);
            data.FoodOrEater.Add(essen);
            data.Eats.Add(isst);
            data.EatsHowMany.Add(isstWieViele);
            data.Quantity.Add(anzahl);
            data.GetsEatenBy.Add(wirdGegessenVon);

            // TODO: Validierung
            // Neuzeichnen des Nahrungsnetzes
            InitializePictureBox();
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

            // Dynamically calculate horizontal and vertical spacing
            int horizontalSpacing = pictureBox.Width / (layerBoundaries.Count + 1);
            int verticalSpacing = pictureBox.Height / (data.Names.Count + 1);
            int leftMargin = (int)(pictureBox.Width * 0.02);
            int itemWidth = 20; // Adjust as needed

            Dictionary<string, Point> animalPositions = new Dictionary<string, Point>();

            for (int i = 0; i < layerBoundaries.Count; i++)
            {
                int boundary = layerBoundaries[i];
                int layerHeight = ((boundary - (i == 0 ? 0 : layerBoundaries[i - 1])) * verticalSpacing) + itemWidth;
                int startY = (pictureBox.Height - layerHeight) / 2;

                for (int j = (i == 0) ? 0 : layerBoundaries[i - 1]; j < boundary; j++)
                {
                    int index = layerIndexes[j];
                    string currentAnimal = data.Names[index];

                    // Adjust posX to include the left margin
                    int posX = leftMargin + (i * horizontalSpacing);
                    int posY = startY;

                    // Draw the item
                    g.FillEllipse(Brushes.Red, posX, posY, itemWidth, itemWidth);
                    g.DrawString(currentAnimal, font, textBrush, posX - 10, posY - 20);

                    // Store the position for drawing connections later
                    animalPositions[currentAnimal] = new Point(posX + itemWidth / 2, posY + itemWidth / 2);

                    // Increment startY for the next item in the same layer
                    startY += verticalSpacing;
                }
            }


            // Draw connections based on feeding relationships
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
