using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace EscapeRoomControlPanel
{
    public static class SceneManager
    {
        private static Panel behaviorPanel;
        private static EventHandler formClickHandler;

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
                Location = new Point(10, 60) // Anfangsposition für den "Verhalten hinzufügen" Button 
            };
            addBehaviorButton.Click += (s, e) => ToggleBehaviorManager(form, newScenePanel);

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

        public static void ToggleBehaviorManager(EinstellungenForm form, Panel scenePanel)
        {
            if (behaviorPanel != null && scenePanel.Controls.Contains(behaviorPanel))
            {
                scenePanel.Controls.Remove(behaviorPanel);
                RemoveFormClickHandler(form, scenePanel);
                return;
            }

            ShowBehaviorManager(form, scenePanel);
        }

        public static void ShowBehaviorManager(EinstellungenForm form, Panel scenePanel)
        {
            // Überprüfen, ob bereits ein behaviorPanel existiert und es entfernen
            if (behaviorPanel != null && scenePanel.Controls.Contains(behaviorPanel))
            {
                scenePanel.Controls.Remove(behaviorPanel);
                return; // Panel ist entfernt, keine weitere Aktion notwendig
            }

            // Finden des "Verhalten hinzufügen" Buttons
            Button addBehaviorButton = scenePanel.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "Verhalten hinzufügen");
            if (addBehaviorButton == null)
            {
                return; // Falls der Button nicht gefunden wird, kehre zurück
            }

            // Positionierung des neuen Panels direkt unter dem "Verhalten hinzufügen" Button
            Point panelLocation = new Point(addBehaviorButton.Location.X, addBehaviorButton.Location.Y + addBehaviorButton.Height + 20);

            // Erstellen und Anzeigen eines Panels statt eines neuen Fensters
            behaviorPanel = new Panel
            {
                Name = "behaviorPanel",
                Location = panelLocation,
                Size = new Size(150, 120),
                BackColor = Color.LightGray
            };

            // Beispielhafte ListBox für die Verhaltensweisen
            ListBox behaviorListBox = new ListBox
            {
                Location = new Point(10, 12),
                Size = new Size(130, 100),
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.None, // Entfernen des Rahmens
                //Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, e.Bounds.Top + 2)
        };
            behaviorListBox.Items.AddRange(new string[] { "PinStatus", "Szene gestartet", "Audio startet", "Audio stoppt", "Aktionbutton" });

            behaviorListBox.SelectedIndexChanged += (s, e) =>
            {
                if (behaviorListBox.SelectedItem != null)
                {
                    string selectedBehavior = behaviorListBox.SelectedItem.ToString();

                    // Berechne die Anzahl der vorhandenen Verhaltensbuttons
                    int behaviorButtonCount = scenePanel.Controls.OfType<Button>().Count(b => b.Text != "Verhalten hinzufügen");

                    // Erstellen des Buttons für das ausgewählte Verhalten
                    Button behaviorButton = new Button
                    {
                        Text = selectedBehavior,
                        Size = new Size(150, 30), // gleiche Größe wie der Button "Verhalten hinzufügen"
                        Location = new Point(10, 60 + 40 * behaviorButtonCount), // Position unterhalb der vorherigen Buttons
                        TextAlign = ContentAlignment.MiddleCenter, // Text zentrieren
                        BackColor = SystemColors.ControlLight 
                    };

                    // Hinzufügen des Buttons zum Panel
                    scenePanel.Controls.Add(behaviorButton);

                    // Neupositionierung des "Verhalten hinzufügen" Buttons
                    addBehaviorButton.Location = new Point(addBehaviorButton.Location.X, behaviorButton.Location.Y + behaviorButton.Height + 10);

                    // Entfernen des behaviorPanels nach Auswahl
                    scenePanel.Controls.Remove(behaviorPanel);
                    RemoveFormClickHandler(form, scenePanel);
                }
            };

            behaviorPanel.Controls.Add(behaviorListBox);
            scenePanel.Controls.Add(behaviorPanel);
            behaviorPanel.BringToFront();

            formClickHandler = (s, e) =>
            {
                if (!behaviorPanel.Bounds.Contains(form.PointToClient(Cursor.Position)))
                {
                    scenePanel.Controls.Remove(behaviorPanel);
                    RemoveFormClickHandler(form, scenePanel);
                }
            };

            form.Click += formClickHandler;
            scenePanel.Click += formClickHandler; 
        }

        private static void RemoveFormClickHandler(Form form, Panel scenePanel)
        {
            if (formClickHandler != null)
            {
                form.Click -= formClickHandler;
                scenePanel.Click -= formClickHandler; 
                formClickHandler = null;
            }
        }
    }
}
