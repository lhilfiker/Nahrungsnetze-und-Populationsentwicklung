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

            // Background
            Image backgroundImage = Image.FromFile("background.png");

            PictureBox backgroundPictureBox = new PictureBox();
            backgroundPictureBox.Image = backgroundImage;
            backgroundPictureBox.Dock = DockStyle.Fill;
            backgroundPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Controls.Add(backgroundPictureBox);
            this.Controls.SetChildIndex(backgroundPictureBox, 0);


            // Erstelle einen Button für den Dateiauswahldialog
            Button filePickerButton = new Button();
            filePickerButton.Text = "Nahrungsnetz Datei auswählen";
            filePickerButton.Size = new System.Drawing.Size(200, 30);
            filePickerButton.Location = new System.Drawing.Point((this.ClientSize.Width - filePickerButton.Width) / 2,
                (this.ClientSize.Height - filePickerButton.Height) / 2);
            filePickerButton.Click += FilePickerButton_Click;
            filePickerButton.FlatStyle = FlatStyle.Flat;

            Button newFolderButton = new Button();
            newFolderButton.Text = "Neues Nahrungsnetz";
            newFolderButton.Size = new System.Drawing.Size(200, 30);
            newFolderButton.Location = new System.Drawing.Point((this.ClientSize.Width - filePickerButton.Width) / 2,
                ((this.ClientSize.Height - filePickerButton.Height) / 2) + 40); // Adjust location as needed
            newFolderButton.Click += NewFolderButton_Click;
            newFolderButton.FlatStyle = FlatStyle.Flat;

            Button helpButton = new Button();
            helpButton.Text = "?";
            helpButton.Size = new System.Drawing.Size(20, 20);
            helpButton.Location = new System.Drawing.Point(this.ClientSize.Width - helpButton.Width,
                this.ClientSize.Height - helpButton.Height);
            helpButton.Click += HelpButton_Click; // Attach event handler


            this.Controls.Add(newFolderButton);
            this.Controls.Add(filePickerButton);
            this.Controls.Add(helpButton);
            filePickerButton.BringToFront();
            newFolderButton.BringToFront();
            helpButton.BringToFront();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            string helpText = "Nahrungsnetz-App\n\n" +
                              $"Version: {data.Version}\n" +
                              "Lizenz: Apache 2.0 (ohne Gewährleistung)\n" +
                              "Entwickler: RebelCoderJames & bettercallmilan (Milan)\n" +
                              "GitHub: https://github.com/RebelCoderJames/Nahrungsnetze-und-Populationsentwicklung";
            MessageBox.Show(helpText, "Hilfeinformationen");
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
                        data.path = Path.Combine(folderPath, fileName);
                        if (Path.GetExtension(data.path) != ".json") data.path += ".json";
                        List<string> Names = new();
                        List<string> Eats = new();
                        List<float> Quantity = new();
                        List<float> EatsHowMany = new();
                        List<float> DeathsPerDay = new();
                        List<float> Replication = new();
                        List<float> Multiplier = new();


                        Database.SaveToDatabase(Names, Eats, Quantity, EatsHowMany, DeathsPerDay, Replication,
                            Multiplier,
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
                (data.Names, data.Eats, data.Quantity, data.EatsHowMany, data.DeathsPerDay, data.Replication,
                        data.Multiplier) =
                    result.Value;
            }

            InitializeComponent();
            InitializePictureBox();
            InitializeEditAnimalButton();
            InitializeAddAnimalButton();
            InitializeQuizButton();
            InitializeSimulateButton();
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

        private void InitializeSimulateButton()
        {
            Button btnSimulate = new Button();
            btnSimulate.Text = "Simulate";
            btnSimulate.Size = new Size(100, 30);
            btnSimulate.Location = new Point(390, this.ClientSize.Height - 40); // Anpassen Sie die Position
            btnSimulate.Click += new EventHandler(SimulateButton_Click);

            this.Controls.Add(btnSimulate);
        }

        private void SimulateButton_Click(object sender, EventArgs e)
        {
            Form SimulateSettingsForm = new Form
            {
                Width = 300,
                Height = 150,
                Text = "Simulation Einstellungen"
            };

            Label lblAnzahlTage = new Label { Text = "Anzahl Tage:", Left = 20, Top = 20 };
            NumericUpDown numAnzahlTage = new NumericUpDown { Left = 150, Top = 20, Width = 100 };
            Button btnStartSimulation = new Button { Text = "Simulation starten", Left = 50, Top = 70, Width = 100 };
            btnStartSimulation.Click += (sender, e) =>
            {
                SimulateSettingsForm.Hide();
                StartSimulation((int)numAnzahlTage.Value);
                SimulateSettingsForm.Close();
            };

            SimulateSettingsForm.Controls.Add(lblAnzahlTage);
            SimulateSettingsForm.Controls.Add(numAnzahlTage);
            SimulateSettingsForm.Controls.Add(btnStartSimulation);
            SimulateSettingsForm.ShowDialog();
        }

        public static void StartSimulation(int anzahlTage)
        {
            Form simulationForm = new Form
            {
                Width = 800,
                Height = 600,
                Text = "Simulation - " + data.Version
            };

            // Annahme: Die Simulate-Methode gibt eine Liste von float zurück
            var simulatedResults = OperationHelper.Simulate(data.Names, data.Eats, data.Quantity, data.EatsHowMany,
                data.DeathsPerDay, data.Replication, data.Multiplier, anzahlTage);

            ListView listView = new ListView
            {
                View = View.Details,
                Dock = DockStyle.Fill
            };

            listView.Columns.Add(new ColumnHeader { Text = "Name", Width = 300 });
            listView.Columns.Add(new ColumnHeader { Text = "Simulationsergebnis", Width = 300 });
            listView.Columns.Add(new ColumnHeader { Text = "Veränderung", Width = 300 });

            for (int i = 0; i < data.Names.Count; i++)
            {
                var change = simulatedResults[i] - data.Quantity[i];
                listView.Items.Add(new ListViewItem(new string[]
                {
                    data.Names[i],
                    simulatedResults[i].ToString("F2"),
                    change.ToString("F2")
                }));
            }
            
            simulationForm.Resize += (sender, e) =>
            {
                int totalColumnWidth = listView.Width - 4;
                int columnWidth = totalColumnWidth / listView.Columns.Count;
                foreach (ColumnHeader column in listView.Columns)
                {
                    column.Width = columnWidth;
                }
            };

            // Hinzufügen von Schließ- und Anwenden-Buttons
            Button closeButton = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom
            };
            closeButton.Click += (sender, e) => { simulationForm.Close(); };

            Button applyButton = new Button
            {
                Text = "Apply",
                Dock = DockStyle.Bottom
            };
            applyButton.Click += (sender, e) =>
            {
                data.Quantity = simulatedResults; // Aktualisieren der Quantity-Liste mit den Simulationsergebnissen
                simulationForm.Close();
            };

            // Hinzufügen der Elemente zum Formular
            simulationForm.Controls.Add(listView);
            simulationForm.Controls.Add(closeButton);
            simulationForm.Controls.Add(applyButton);

            // Anzeigen des Formulars
            simulationForm.ShowDialog();
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

            Label lblEats = new Label { Text = "Isst:", Left = 20, Top = 50, Size = new Size(180, 20) };
            TextBox txtEats = new TextBox { Left = 200, Top = 50, Width = 180 };

            Label lblQuantity = new Label { Text = "Anzahl:", Left = 20, Top = 80, Size = new Size(180, 20) };
            NumericUpDown numQuantity = new NumericUpDown { Left = 200, Top = 80, Width = 180 };
            numQuantity.DecimalPlaces = 6;
            numQuantity.Increment = 0.000001M;
            numQuantity.Maximum = 9999999999999999999;


            Label lblEatsHowMany = new Label
                { Text = "Isst wie viele:", Left = 20, Top = 110, Size = new Size(180, 20) };
            NumericUpDown numEatsHowMany = new NumericUpDown { Left = 200, Top = 110, Width = 180 };
            numEatsHowMany.DecimalPlaces = 6;
            numEatsHowMany.Increment = 0.000001M;
            numEatsHowMany.Maximum = 9999999999999999999;


            Label lblDeathsPerDay = new Label
                { Text = "Todesfälle pro Tag:", Left = 20, Top = 140, Size = new Size(180, 20) };
            NumericUpDown numDeathsPerDay = new NumericUpDown { Left = 200, Top = 140, Width = 180 };
            numDeathsPerDay.DecimalPlaces = 6;
            numDeathsPerDay.Increment = 0.000001M;
            numDeathsPerDay.Maximum = 9999999999999999999;


            Label lblReplication = new Label { Text = "Replikation:", Left = 20, Top = 170, Size = new Size(180, 20) };
            NumericUpDown numReplication = new NumericUpDown { Left = 200, Top = 170, Width = 180 };
            numReplication.DecimalPlaces = 6;
            numReplication.Increment = 0.000001M;
            numReplication.Maximum = 9999999999999999999;


            Label lblMultiplier = new Label { Text = "Multiplikator:", Left = 20, Top = 200, Size = new Size(180, 20) };
            NumericUpDown numMultiplier = new NumericUpDown { Left = 200, Top = 200, Width = 180 };
            numMultiplier.DecimalPlaces = 6;
            numMultiplier.Increment = 0.000001M;
            numMultiplier.Maximum = 9999999999999999999;


            // Hinzufügen-Button im Popup
            Button add = new Button
                { Text = "Hinzufügen", Left = 50, Width = 100, Top = 240, DialogResult = DialogResult.OK };
            add.Click += (sender, e) =>
            {
                string name = txtName.Text;
                string eats = txtEats.Text;
                float quantity = (float)numQuantity.Value;
                float eatsHowMany = (float)numEatsHowMany.Value;
                float deathsPerDay = (float)numDeathsPerDay.Value;
                float replication = (float)numReplication.Value;
                float multiplier = (float)numMultiplier.Value;

                data.Names.Add(name);
                data.Eats.Add(eats);
                data.Quantity.Add(quantity);
                data.EatsHowMany.Add(eatsHowMany);
                data.DeathsPerDay.Add(deathsPerDay);
                data.Replication.Add(replication);
                data.Multiplier.Add(multiplier);
                Database.SaveToDatabase(data.Names, data.Eats, data.Quantity, data.EatsHowMany, data.DeathsPerDay,
                    data.Replication, data.Multiplier,
                    data.path);

                var sortedLayers =
                    OperationHelper.SortByLayer(data.Names, data.Eats);
                (layerIndexes, layerBoundaries) = sortedLayers;
                pictureBox.Invalidate();

                addForm.Close();
            };

            // Abbrechen-Button im Popup
            Button cancel = new Button
                { Text = "Abbrechen", Left = 200, Width = 100, Top = 240, DialogResult = DialogResult.Cancel };
            cancel.Click += (sender, e) => { addForm.Close(); };

            // Fügen Sie die Steuerelemente zum Popup-Formular hinzu
            addForm.Controls.Add(lblName);
            addForm.Controls.Add(txtName);
            addForm.Controls.Add(lblEats);
            addForm.Controls.Add(txtEats);
            addForm.Controls.Add(lblQuantity);
            addForm.Controls.Add(numQuantity);
            addForm.Controls.Add(lblEatsHowMany);
            addForm.Controls.Add(numEatsHowMany);
            addForm.Controls.Add(lblDeathsPerDay);
            addForm.Controls.Add(numDeathsPerDay);
            addForm.Controls.Add(lblReplication);
            addForm.Controls.Add(numReplication);
            addForm.Controls.Add(lblMultiplier);
            addForm.Controls.Add(numMultiplier);
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

            if (index < 0 || index >= data.Names.Count)
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

            Label lblEats = new Label { Text = "Isst:", Left = 20, Top = 50 };
            TextBox txtEats = new TextBox { Left = 200, Top = 50, Width = 180, Text = data.Eats[index] };

            Label lblQuantity = new Label { Text = "Anzahl:", Left = 20, Top = 80 };
            NumericUpDown numQuantity = new NumericUpDown
                { Left = 200, Top = 80, Width = 180, Value = Convert.ToDecimal(data.Quantity[index]) };
            numQuantity.DecimalPlaces = 6;
            numQuantity.Increment = 0.000001M;
            numQuantity.Maximum = 9999999999999999999;

            Label lblEatsHowMany = new Label { Text = "Isst wie viele:", Left = 20, Top = 110 };
            NumericUpDown numEatsHowMany = new NumericUpDown
                { Left = 200, Top = 110, Width = 180, Value = Convert.ToDecimal(data.EatsHowMany[index]) };
            numEatsHowMany.DecimalPlaces = 6;
            numEatsHowMany.Increment = 0.000001M;
            numEatsHowMany.Maximum = 9999999999999999999;


            Label lblDeathsPerDay = new Label { Text = "Todesfälle pro Tag:", Left = 20, Top = 140 };
            NumericUpDown numDeathsPerDay = new NumericUpDown
                { Left = 200, Top = 140, Width = 180, Value = Convert.ToDecimal(data.DeathsPerDay[index]) };
            numDeathsPerDay.DecimalPlaces = 6;
            numDeathsPerDay.Increment = 0.000001M;
            numDeathsPerDay.Maximum = 9999999999999999999;


            Label lblReplication = new Label { Text = "Replikation pro Tag:", Left = 20, Top = 170 };
            NumericUpDown numReplication = new NumericUpDown
                { Left = 200, Top = 170, Width = 180, Value = Convert.ToDecimal(data.Replication[index]) };
            numReplication.DecimalPlaces = 6;
            numReplication.Increment = 0.000001M;
            numReplication.Maximum = 9999999999999999999;


            Label lblMultiplier = new Label { Text = "Multiplikator:", Left = 20, Top = 200 };
            NumericUpDown numMultiplier = new NumericUpDown
                { Left = 200, Top = 200, Width = 180, Value = Convert.ToDecimal(data.Multiplier[index]) };
            numMultiplier.DecimalPlaces = 6;
            numMultiplier.Increment = 0.000001M;
            numMultiplier.Maximum = 9999999999999999999;


            // OK und Abbrechen Buttons
            Button confirmation = new Button
                { Text = "Ok", Left = 50, Width = 100, Top = 240, DialogResult = DialogResult.OK };
            Button cancel = new Button
                { Text = "Abbrechen", Left = 160, Width = 100, Top = 240, DialogResult = DialogResult.Cancel };
            Button delete = new Button { Text = "Löschen", Left = 270, Width = 100, Top = 240 };
            delete.Click += (sender, e) =>
            {
                // Entfernen des Tieres aus allen Listen
                data.Names.RemoveAt(index);
                data.Eats.RemoveAt(index);
                data.Quantity.RemoveAt(index);
                data.EatsHowMany.RemoveAt(index);
                data.DeathsPerDay.RemoveAt(index);
                data.Replication.RemoveAt(index);
                data.Multiplier.RemoveAt(index);

                // Speichern der Änderungen in der Datenbank und Aktualisierung der Darstellung
                Database.SaveToDatabase(data.Names, data.Eats, data.Quantity, data.EatsHowMany, data.DeathsPerDay,
                    data.Replication, data.Multiplier,
                    data.path);

                var sortedLayers = OperationHelper.SortByLayer(data.Names, data.Eats);
                (layerIndexes, layerBoundaries) = sortedLayers;
                pictureBox.Invalidate();

                editForm.Close();
            };


            confirmation.Click += (sender, e) =>
            {
                // Aktualisieren der Daten
                data.Names[index] = txtName.Text;
                data.Eats[index] = txtEats.Text;
                data.Quantity[index] = (float)numQuantity.Value;
                data.EatsHowMany[index] = (float)numEatsHowMany.Value;
                data.DeathsPerDay[index] = (float)numDeathsPerDay.Value;
                data.Replication[index] = (float)numReplication.Value;
                data.Multiplier[index] = (float)numMultiplier.Value;


                Database.SaveToDatabase(data.Names, data.Eats, data.Quantity, data.EatsHowMany, data.DeathsPerDay,
                    data.Replication, data.Multiplier,
                    data.path);

                var sortedLayers =
                    OperationHelper.SortByLayer(data.Names, data.Eats);
                (layerIndexes, layerBoundaries) = sortedLayers;
                pictureBox.Invalidate();

                editForm.Close();
            };
            cancel.Click += (sender, e) => { editForm.Close(); };

            editForm.Controls.AddRange(new Control[]
            {
                lblName,
                txtName,
                lblEats,
                txtEats,
                lblQuantity,
                numQuantity,
                lblEatsHowMany,
                numEatsHowMany,
                lblDeathsPerDay,
                numDeathsPerDay,
                lblReplication,
                numReplication,
                lblMultiplier,
                numMultiplier,
                confirmation,
                cancel,
                delete
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
            Brush backgroundBrush = Brushes.White;
            Pen linePen = new Pen(Brushes.Black, 2);
            int itemDiameter = 20; // Diameter of the circle

            Dictionary<string, Point> animalPositions = new Dictionary<string, Point>();

            // Get sorted layer data
            var sortedLayersData = OperationHelper.SortByLayer(data.Names, data.Eats);
            (var layerIndexes, var layerBoundaries) = sortedLayersData;

            // First, calculate and store positions of all animals
            for (int layerNumber = 1; layerNumber <= layerBoundaries.Count; layerNumber++)
            {
                var currentLayerIndexes = OperationHelper.GetLayer(layerIndexes, layerBoundaries, layerNumber);
                int verticalSpacing = pictureBox.Height / (currentLayerIndexes.Count + 1);
                int posY = verticalSpacing / 2; // Start position for y
                int totalLayers = layerBoundaries.Count;
                int horizontalSpacing = pictureBox.Width / (totalLayers + 1);

                // Calculate the x-Position for each layer
                int posX = layerNumber * horizontalSpacing - (itemDiameter / 2); // Centering the layer

                foreach (var index in currentLayerIndexes)
                {
                    string currentAnimal = data.Names[index];
                    // Store the position for drawing connections later
                    animalPositions[currentAnimal] = new Point(posX + itemDiameter / 2, posY + itemDiameter / 2);
                    posY += verticalSpacing;
                }
            }

            // Then, draw connections (lines) based on feeding relationships
            foreach (var name in data.Names)
            {
                int index = data.Names.IndexOf(name);
                string preyList = data.Eats[index];
                if (!string.IsNullOrEmpty(preyList))
                {
                    var preys = preyList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim()).ToList();

                    foreach (var prey in preys)
                    {
                        if (animalPositions.ContainsKey(prey))
                        {
                            Point preyPos = animalPositions[prey];
                            Point currentPos = animalPositions[name];
                            g.DrawLine(linePen, currentPos.X, currentPos.Y, preyPos.X, preyPos.Y);
                        }
                    }
                }
            }

            // Draw dots (circles) and text within white boxes
            foreach (var kvp in animalPositions)
            {
                string currentAnimal = kvp.Key;
                Point position = kvp.Value;
                int scaledItemDiameter =
                    CalculateDiameter(
                        data.Quantity[data.Names.IndexOf(currentAnimal)]); // Calculate the scaled diameter

                // Draw the item (dot/circle) with scaled diameter
                Rectangle drawRect = new Rectangle(position.X - scaledItemDiameter / 2,
                    position.Y - scaledItemDiameter / 2, scaledItemDiameter, scaledItemDiameter);
                if (data.Eats[data.Names.IndexOf(currentAnimal)] == "") g.FillEllipse(Brushes.Green, drawRect);
                else g.FillEllipse(Brushes.Red, drawRect);
                g.DrawEllipse(linePen, drawRect);

                // Draw text within white boxes
                SizeF textSize = g.MeasureString(currentAnimal, font);
                Rectangle textRect = new Rectangle(position.X + scaledItemDiameter / 2 + 5,
                    position.Y - (int)(textSize.Height / 2), (int)textSize.Width, (int)textSize.Height);
                g.FillRectangle(backgroundBrush, textRect);
                g.DrawString(currentAnimal, font, textBrush, position.X + scaledItemDiameter / 2 + 5,
                    position.Y - (int)(textSize.Height / 2));
            }
        }


        private int CalculateDiameter(float quantity)
        {
            // Example scaling logic (modify as needed)
            int baseDiameter = 2; // Base diameter for the smallest quantity
            return baseDiameter + (int)Math.Log(quantity + 1) * 8; // Scale diameter based on quantity
        }
    }
}