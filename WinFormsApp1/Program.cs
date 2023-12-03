using System;
using System.Collections.Generic;
using System.Data;
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
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
            filePickerButton.Text = "Nahrungsnetz Datei auswählen";
            filePickerButton.Size = new System.Drawing.Size(200, 30);
            filePickerButton.Location = new System.Drawing.Point((this.ClientSize.Width - filePickerButton.Width) / 2,
                (this.ClientSize.Height - filePickerButton.Height) / 2);
            filePickerButton.Click += FilePickerButton_Click;

            Button newFolderButton = new Button();
            newFolderButton.Text = "Neues Nahrungsnetz";
            newFolderButton.Size = new System.Drawing.Size(200, 30);
            newFolderButton.Location = new System.Drawing.Point((this.ClientSize.Width - filePickerButton.Width) / 2,
                ((this.ClientSize.Height - filePickerButton.Height) / 2) + 30); // Adjust location as needed
            newFolderButton.Click += NewFolderButton_Click;

            // Füge den Button zum Begrüßungsbildschirm hinzu
            this.Controls.Add(newFolderButton);
            this.Controls.Add(filePickerButton);
        }

        private void NewFolderButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Wähle einen Ordner aus";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // Save the selected folder path
                    string folderPath = folderBrowserDialog.SelectedPath;

                    // Prompt the user to enter the name of the food web
                    string fileName = Microsoft.VisualBasic.Interaction.InputBox("Gib den Namen des Nahrungsnetzes ein",
                        "Nahrungsnetz Erstellen", "MeinNahrungsnetz.json");

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        // Combine the folder path and file name
                        data.path = Path.Combine(folderPath, fileName);
                        List<string> Names = new();
                        List<string> GetsEatenBy = new();
                        List<string> Eats = new();
                        List<float> Quantity = new();
                        List<float> EatsHowMany = new();
                        List<bool> FoodOrEater = new();

                        Database.SaveToDatabase(Names, GetsEatenBy, Eats, Quantity, EatsHowMany, FoodOrEater,
                            data.path);
                        MessageBox.Show("Eine neue Datei wurde hier erstellt: " + data.path);
                        this.Hide(); // Hide the current form

                        // Create and show the MainForm
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                    }
                }
            }
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
        
        public class QuizFrage
        {
            public string FrageText { get; set; }
            public List<string> AntwortOptionen { get; set; }
            public string RichtigeAntwort { get; set; }

            public QuizFrage(string frageText, List<string> antwortOptionen, string richtigeAntwort)
            {
                FrageText = frageText;
                AntwortOptionen = antwortOptionen;
                RichtigeAntwort = richtigeAntwort;
            }
        }


        public MainForm()
        {
            var result = Database.OpenDatabase(data.path);
            if (result.HasValue)
            {
                // Assign the values from the result to the lists
                (data.Names, data.GetsEatenBy, data.Eats, data.Quantity, data.EatsHowMany, data.FoodOrEater) =
                    result.Value;
            }

            InitializeComponent();
            InitializePictureBox();
            InitializeEditAnimalButton();
            InitializeAddAnimalButton();
            InitializeQuizButton();
        }

        private void InitializeAddAnimalButton()
        {
            Button btnAddAnimal = new Button();
            btnAddAnimal.Text = "Neues Tier hinzufügen";
            btnAddAnimal.Size = new Size(160, 30);
            btnAddAnimal.Location = new Point(10, this.ClientSize.Height - 40); // Positionieren Sie den Button
            btnAddAnimal.Click += new EventHandler(AddAnimalButton_Click);

            this.Controls.Add(btnAddAnimal);
        }
        
        private void InitializeQuizButton()
        {
            Button btnQuiz = new Button();
            btnQuiz.Text = "Quiz";
            btnQuiz.Size = new Size(100, 30);
            btnQuiz.Location = new Point(290, this.ClientSize.Height - 40); // Anpassen Sie die Position
            btnQuiz.Click += new EventHandler(QuizButton_Click);

            this.Controls.Add(btnQuiz);
        }

        private void QuizButton_Click(object sender, EventArgs e)
        {
            Form quizSettingsForm = new Form
            {
                Width = 300,
                Height = 150,
                Text = "Quiz Einstellungen"
            };

            Label lblAnzahlFragen = new Label { Text = "Anzahl Fragen:", Left = 20, Top = 20 };
            NumericUpDown numAnzahlFragen = new NumericUpDown { Left = 150, Top = 20, Width = 100 };
            Button btnStartQuiz = new Button { Text = "Quiz starten", Left = 50, Top = 70, Width = 100 };
            btnStartQuiz.Click += (sender, e) =>
            {
                this.Hide(); // Hauptformular verstecken
                quizSettingsForm.Hide();
                StartQuiz((int)numAnzahlFragen.Value);
                quizSettingsForm.Close();
            };

            quizSettingsForm.Controls.Add(lblAnzahlFragen);
            quizSettingsForm.Controls.Add(numAnzahlFragen);
            quizSettingsForm.Controls.Add(btnStartQuiz);
            quizSettingsForm.ShowDialog();
        }
        
        private void StartQuiz(int anzahlFragen)
        {
            Form quizForm = new Form
            {
                Width = 500,
                Height = 400,
                Text = "Quiz"
            };

            // Logik zum Generieren und Anzeigen von Quizfragen
            // ...

            // Ergebnisanzeige und Schließen-Button
            // ...

            quizForm.ShowDialog();
        }



        private void AddAnimalButton_Click(object sender, EventArgs e)
        {
            Form addForm = new Form
            {
                Width = 400,
                Height = 400,
                Text = "Neues Tier hinzufügen"
            };

            // Erstellen der Eingabefelder und Labels
            Label lblName = new Label { Text = "Name:", Left = 20, Top = 20, Size = new Size(180, 20) };
            TextBox txtName = new TextBox { Left = 200, Top = 20, Width = 180 };

            Label lblIsst = new Label { Text = "Isst:", Left = 20, Top = 50, Size = new Size(180, 20) };
            TextBox txtIsst = new TextBox { Left = 200, Top = 50, Width = 180 };

            Label lblWirdGegessenVon = new Label
                { Text = "Wird gegessen von:", Left = 20, Top = 80, Size = new Size(180, 20) };
            TextBox txtWirdGegessenVon = new TextBox { Left = 200, Top = 80, Width = 180 };

            Label lblAnzahl = new Label { Text = "Anzahl:", Left = 20, Top = 110, Size = new Size(180, 20) };
            NumericUpDown numAnzahl = new NumericUpDown { Left = 200, Top = 110, Width = 180 };

            Label lblIsstWieViele = new Label
                { Text = "Isst wie viele:", Left = 20, Top = 140, Size = new Size(180, 20) };
            NumericUpDown numIsstWieViele = new NumericUpDown { Left = 200, Top = 140, Width = 180 };

            Label lblEssen = new Label { Text = "Ist Essen:", Left = 20, Top = 170, Size = new Size(180, 20) };
            CheckBox chkEssen = new CheckBox { Left = 200, Top = 170 };

            // Hinzufügen-Button im Popup
            Button add = new Button
                { Text = "Hinzufügen", Left = 50, Width = 100, Top = 200, DialogResult = DialogResult.OK };
            add.Click += (sender, e) =>
            {
                // Logik zum Hinzufügen eines neuen Tiers
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

                Database.SaveToDatabase(data.Names, data.GetsEatenBy, data.Eats, data.Quantity, data.EatsHowMany,
                    data.FoodOrEater, data.path);

                var sortedLayers =
                    OperationHelper.SortByLayer(data.Names, data.GetsEatenBy, data.Eats, data.FoodOrEater);
                (layerIndexes, layerBoundaries) = sortedLayers;
                pictureBox.Invalidate();

                addForm.Close();
            };

            // Abbrechen-Button im Popup
            Button cancel = new Button
                { Text = "Abbrechen", Left = 200, Width = 100, Top = 200, DialogResult = DialogResult.Cancel };
            cancel.Click += (sender, e) => { addForm.Close(); };

            // Fügen Sie die Steuerelemente zum Popup-Formular hinzu
            addForm.Controls.Add(lblName);
            addForm.Controls.Add(txtName);
            addForm.Controls.Add(lblIsst);
            addForm.Controls.Add(txtIsst);
            addForm.Controls.Add(lblWirdGegessenVon);
            addForm.Controls.Add(txtWirdGegessenVon);
            addForm.Controls.Add(lblAnzahl);
            addForm.Controls.Add(numAnzahl);
            addForm.Controls.Add(lblIsstWieViele);
            addForm.Controls.Add(numIsstWieViele);
            addForm.Controls.Add(lblEssen);
            addForm.Controls.Add(chkEssen);
            addForm.Controls.Add(add);
            addForm.Controls.Add(cancel);
            addForm.ShowDialog();
        }


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
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

        private void InitializeEditAnimalButton()
        {
            Button btnEditAnimal = new Button();
            btnEditAnimal.Text = "Editiere Tier";
            btnEditAnimal.Size = new Size(120, 30);
            btnEditAnimal.Location = new Point(170, this.ClientSize.Height - 40); // Adjust location as needed
            btnEditAnimal.Click += new EventHandler(EditAnimalButton_Click);

            this.Controls.Add(btnEditAnimal);
        }

        private void EditAnimalButton_Click(object sender, EventArgs e)
        {
            string animalName =
                Microsoft.VisualBasic.Interaction.InputBox("Gib den Namen des Tieres ein", "Tier editieren", "");

            int index = data.Names.IndexOf(animalName);
            if (index == -1)
            {
                MessageBox.Show("Tier nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form editForm = new Form
            {
                Width = 400,
                Height = 400,
                Text = "Tier editieren"
            };

            // Erstellen von Labels und Textfeldern für jedes Attribut
            Label lblName = new Label { Text = "Name:", Left = 20, Top = 20 };
            TextBox txtName = new TextBox { Left = 200, Top = 20, Width = 180, Text = data.Names[index] };

            Label lblIsst = new Label { Text = "Isst:", Left = 20, Top = 50 };
            TextBox txtIsst = new TextBox { Left = 200, Top = 50, Width = 180, Text = data.Eats[index] };

            Label lblWirdGegessenVon = new Label { Text = "Wird gegessen von:", Left = 20, Top = 80 };
            TextBox txtWirdGegessenVon = new TextBox
                { Left = 200, Top = 80, Width = 180, Text = data.GetsEatenBy[index] };

            Label lblAnzahl = new Label { Text = "Anzahl:", Left = 20, Top = 110 };
            NumericUpDown numAnzahl = new NumericUpDown
                { Left = 200, Top = 110, Width = 180, Value = Convert.ToDecimal(data.Quantity[index]) };

            Label lblIsstWieViele = new Label { Text = "Isst wie viele:", Left = 20, Top = 140 };
            NumericUpDown numIsstWieViele = new NumericUpDown
                { Left = 200, Top = 140, Width = 180, Value = Convert.ToDecimal(data.EatsHowMany[index]) };


            CheckBox chkEssen = new CheckBox
                { Text = "Ist Essen", Left = 20, Top = 170, Checked = data.FoodOrEater[index] };

            // OK und Abbrechen Buttons
            Button confirmation = new Button
                { Text = "Ok", Left = 50, Width = 100, Top = 200, DialogResult = DialogResult.OK };
            Button cancel = new Button
                { Text = "Abbrechen", Left = 200, Width = 100, Top = 200, DialogResult = DialogResult.Cancel };

            confirmation.Click += (sender, e) =>
            {
                // Aktualisieren der Daten
                data.Names[index] = txtName.Text;
                data.Eats[index] = txtIsst.Text;
                data.GetsEatenBy[index] = txtWirdGegessenVon.Text;
                data.Quantity[index] = (float)numAnzahl.Value;
                data.EatsHowMany[index] = (float)numIsstWieViele.Value;
                data.FoodOrEater[index] = chkEssen.Checked;

                Database.SaveToDatabase(data.Names, data.GetsEatenBy, data.Eats, data.Quantity, data.EatsHowMany,
                    data.FoodOrEater, data.path);

                var sortedLayers =
                    OperationHelper.SortByLayer(data.Names, data.GetsEatenBy, data.Eats, data.FoodOrEater);
                (layerIndexes, layerBoundaries) = sortedLayers;
                pictureBox.Invalidate();

                editForm.Close();
            };
            cancel.Click += (sender, e) => { editForm.Close(); };

            // Hinzufügen der Steuerelemente zum Formular
            editForm.Controls.AddRange(new Control[]
            {
                lblName, txtName, lblIsst, txtIsst, lblWirdGegessenVon, txtWirdGegessenVon, lblAnzahl, numAnzahl,
                lblIsstWieViele, numIsstWieViele, chkEssen, confirmation, cancel
            });
            editForm.ShowDialog();
        }


        private void InitializePictureBox()
        {
            pictureBox = new PictureBox();

            // Calculate size as 80% of form's width and 90% of form's height
            int pictureBoxWidth = (int)(this.ClientSize.Width * 1);
            int pictureBoxHeight = (int)(this.ClientSize.Height * 0.9);

            pictureBox.Size = new Size(pictureBoxWidth, pictureBoxHeight);
            pictureBox.Location = new Point(0, 0); // Positioned at the top-left corner
            pictureBox.Paint += PictureBox_Paint;

            this.Controls.Add(pictureBox);
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
            int itemDiameter = 20; // Diameter of the circle

            Dictionary<string, Point> animalPositions = new Dictionary<string, Point>();

            // Get sorted layer data
            var sortedLayersData =
                OperationHelper.SortByLayer(data.Names, data.GetsEatenBy, data.Eats, data.FoodOrEater);
            (var layerIndexes, var layerBoundaries) = sortedLayersData;

            for (int layerNumber = 1; layerNumber <= layerBoundaries.Count; layerNumber++)
            {
                var currentLayerIndexes = OperationHelper.GetLayer(layerIndexes, layerBoundaries, layerNumber);
                int verticalSpacing = pictureBox.Height / (currentLayerIndexes.Count + 1);
                int posY = verticalSpacing / 2; // Start position for y

                // Calculate x position for each layer
                int posX = (layerNumber - 1) * itemDiameter * 3 + (int)(pictureBox.Width * 0.02);

                foreach (var index in currentLayerIndexes)
                {
                    string currentAnimal = data.Names[index];

                    // Draw the item
                    Rectangle drawRect = new Rectangle(posX, posY, itemDiameter, itemDiameter);
                    g.FillEllipse(Brushes.Red, drawRect);
                    g.DrawEllipse(linePen, drawRect);
                    g.DrawString(currentAnimal, font, textBrush, posX + itemDiameter + 5, posY);

                    // Store the position for drawing connections later
                    animalPositions[currentAnimal] = new Point(posX + itemDiameter / 2, posY + itemDiameter / 2);

                    posY += verticalSpacing;
                }
            }

            // Draw connections based on feeding relationships
            foreach (var name in data.Names)
            {
                int index = data.Names.IndexOf(name);
                string predator = data.GetsEatenBy[index];
                string prey = data.Eats[index];

                if (!string.IsNullOrEmpty(predator) && animalPositions.ContainsKey(predator))
                {
                    Point predatorPos = animalPositions[predator];
                    Point currentPos = animalPositions[name];
                    g.DrawLine(linePen, predatorPos.X, predatorPos.Y, currentPos.X, currentPos.Y);
                }

                if (!string.IsNullOrEmpty(prey) && animalPositions.ContainsKey(prey))
                {
                    Point preyPos = animalPositions[prey];
                    Point currentPos = animalPositions[name];
                    g.DrawLine(linePen, currentPos.X, currentPos.Y, preyPos.X, preyPos.Y);
                }
            }
        }
    }
}