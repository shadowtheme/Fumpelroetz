using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EscapeRoomControlPanel
{
    public static class SceneManager
    {
        public static void AddNewScene(EinstellungenForm form, ref List<Panel> scenePanels, ref List<Button> szeneButtons, ref int sceneCounter)
        {
            Panel newScenePanel = new Panel
            {
                Size = new Size(1400, 700),
                Location = new Point(28, 66),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SystemColors.ControlLight,
                Name = $"ScenePanel_{sceneCounter}"
            };

            Button newSceneButton = new Button
            {
                Size = new Size(100, 30),
                Text = "Neue Szene"
            };
            newSceneButton.Click += (s, e) => form.ShowScene(newScenePanel);

            Panel headerPanel = new Panel
            {
                Size = new Size(760, 50),
                Dock = DockStyle.Top,
                BackColor = Color.Gray
            };

            TextBox sceneNameTextBox = new TextBox
            {
                Text = "Neue Szene",
                Dock = DockStyle.Left,
                TextAlign = HorizontalAlignment.Left,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Width = 700
            };
            sceneNameTextBox.Enter += (s, e) => sceneNameTextBox.SelectAll();
            sceneNameTextBox.Leave += (s, e) => form.RenameScene(newScenePanel, newSceneButton, sceneNameTextBox.Text);
            sceneNameTextBox.Click += (s, e) =>
            {
                sceneNameTextBox.Focus();
                sceneNameTextBox.SelectAll();
            };
            sceneNameTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    form.ActiveControl = null; // Remove focus from TextBox
                    form.RenameScene(newScenePanel, newSceneButton, sceneNameTextBox.Text);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };

            // Create Delete Button
            Button deleteButton = new Button
            {
                Text = "Szene löschen",
                Dock = DockStyle.Right,
                Width = 80
            };
            deleteButton.Click += (s, e) => form.DeleteScene(newScenePanel, newSceneButton);

            // Create Behavior Button
            Button addBehaviorButton = new Button
            {
                Text = "Verhalten hinzufügen",
                Width = 150,
                Height = 30,
                Location = new Point(10, 70) // Anfangsposition für den "Verhalten hinzufügen" Button 
            };
            addBehaviorButton.Click += (s, e) => form.ShowBehaviorManager(newScenePanel);

            // Create Vertical Separator
            Panel separatorPanel = new Panel
            {
                Location = new Point(170, 50),
                Size = new Size(2, 700),
                BackColor = Color.Gray
            };

            headerPanel.Controls.Add(sceneNameTextBox);
            headerPanel.Controls.Add(deleteButton);
            newScenePanel.Controls.Add(headerPanel);

            newScenePanel.Controls.Add(addBehaviorButton);
            newScenePanel.Controls.Add(separatorPanel);

            foreach (var panel in scenePanels)
            {
                panel.Visible = false;
            }

            scenePanels.Add(newScenePanel);
            form.Controls.Add(newScenePanel);
            newScenePanel.BringToFront();

            szeneButtons.Add(newSceneButton);
            form.Controls.Add(newSceneButton);

            sceneCounter++;
            form.UpdateSceneButtonPositions();
        }

        public static void ShowBehaviorManager(EinstellungenForm form, Panel scenePanel)
        {
            using (var behaviorManagerForm = new BehaviorManagerForm())
            {
                if (behaviorManagerForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedBehavior = behaviorManagerForm.SelectedBehavior;

                    // Berechne die Anzahl der vorhandenen Verhaltensbuttons
                    int behaviorButtonCount = scenePanel.Controls.OfType<Button>().Count(b => b.Text != "Verhalten hinzufügen");

                    // Erstellen des Buttons für das ausgewählte Verhalten
                    Button behaviorButton = new Button
                    {
                        Text = selectedBehavior,
                        Size = new Size(150, 30), // gleiche Größe wie der Button "Verhalten hinzufügen"
                        Location = new Point(10, 60 + 40 * behaviorButtonCount), // Position unterhalb der vorherigen Buttons
                        TextAlign = ContentAlignment.MiddleCenter, // Text zentrieren
                        BackColor = SystemColors.ControlLight // gleiche Hintergrundfarbe wie das Panel
                    };

                    // Hinzufügen des Buttons zum Panel
                    scenePanel.Controls.Add(behaviorButton);

                    // Neupositionierung des "Verhalten hinzufügen" Buttons
                    foreach (Control control in scenePanel.Controls)
                    {
                        if (control is Button && control.Text == "Verhalten hinzufügen")
                        {
                            control.Location = new Point(control.Location.X, behaviorButton.Location.Y + 40); // 30px Höhe + 10px Abstand
                            break;
                        }
                    }

                    MessageBox.Show($"Gewähltes Verhalten: {selectedBehavior}");
                }
            }
        }
    }
}
