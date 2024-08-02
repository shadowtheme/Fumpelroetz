using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EscapeRoomControlPanel
{
    public partial class EinstellungenForm : Form
    {
        public List<Button> szeneButtons;
        public List<Panel> scenePanels;
        public int sceneCounter;

        public EinstellungenForm()
        {
            InitializeComponent();
            szeneButtons = new List<Button>();
            scenePanels = new List<Panel>();
            sceneCounter = 1; // Start counter for scene numbering
            LoadScenes();
            this.Click += EinstellungenForm_Click; // Event to detect clicks outside TextBox

            foreach (Control control in this.Controls)
            {
                control.Click += EinstellungenForm_Click;
            }
        }

        public void EinstellungenForm_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null; // Remove focus from any control
        }

        private void btnAddScene_Click(object sender, EventArgs e)
        {
            SceneManager.AddNewScene(this, ref scenePanels, ref szeneButtons, ref sceneCounter);
            SaveScenes();
        }

        public void RenameScene(Panel scenePanel, Button sceneButton, string newName)
        {
            scenePanel.Name = newName;
            sceneButton.Text = newName;
            SaveScenes();
        }

        public void DeleteScene(Panel scenePanel, Button sceneButton)
        {
            scenePanels.Remove(scenePanel);
            this.Controls.Remove(scenePanel);
            szeneButtons.Remove(sceneButton);
            this.Controls.Remove(sceneButton);
            UpdateSceneButtonPositions();
            SaveScenes();
            if (scenePanels.Count > 0)
            {
                scenePanels[scenePanels.Count - 1].Visible = true;
            }
            else
            {
                preShowPanel.Visible = true;
            }
        }

        public void ShowScene(Panel scenePanel)
        {
            foreach (var panel in scenePanels)
            {
                panel.Visible = false;
            }
            scenePanel.Visible = true;
            scenePanel.BringToFront();
        }

        public void UpdateSceneButtonPositions()
        {
            int spacing = 10;
            int startX = preshowButton.Right + spacing;

            foreach (var button in szeneButtons)
            {
                button.Location = new Point(startX, preshowButton.Top);
                startX += button.Width + spacing;
            }

            btnAddScene.Location = new Point(startX, preshowButton.Top);
        }

        private void preshowButton_Click(object sender, EventArgs e)
        {
            ShowScene(preShowPanel);
        }

        private void SaveScenes()
        {
            var scenes = new List<SceneData>();
            foreach (var panel in scenePanels)
            {
                // Store the Text of the TextBox (Scene name)
                var textBox = (TextBox)panel.Controls[0].Controls[0];
                var behaviors = panel.Controls.OfType<Button>()
                                   .Where(b => b.Text != "Verhalten hinzufügen")
                                   .Select(b => b.Text)
                                   .ToList();

                scenes.Add(new SceneData
                {
                    Name = textBox.Text,
                    LocationX = panel.Location.X,
                    LocationY = panel.Location.Y,
                    Behaviors = behaviors
                });
            }
            var json = JsonConvert.SerializeObject(scenes);
            File.WriteAllText("scenes.json", json);
        }

        private void LoadScenes()
        {
            if (File.Exists("scenes.json"))
            {
                var json = File.ReadAllText("scenes.json");
                var scenes = JsonConvert.DeserializeObject<List<SceneData>>(json);
                foreach (var scene in scenes)
                {
                    var panel = SceneLoader.AddLoadedScene(this, scene);
                    if (scene.Behaviors != null)
                    {
                        foreach (var behavior in scene.Behaviors)
                        {
                            var behaviorButton = new Button
                            {
                                Text = behavior,
                                Size = new Size(150, 30), // gleiche Größe wie der Button "Verhalten hinzufügen"
                                Location = new Point(10, 60 + 40 * panel.Controls.OfType<Button>().Count(b => b.Text != "Verhalten hinzufügen")), // Position unterhalb der vorherigen Buttons
                                TextAlign = ContentAlignment.MiddleCenter, // Text zentrieren
                                BackColor = SystemColors.ControlLight // gleiche Hintergrundfarbe wie das Panel
                            };
                            panel.Controls.Add(behaviorButton);
                        }
                    }

                    // Neupositionierung des "Verhalten hinzufügen" Buttons nach dem Laden der Verhaltensweisen
                    foreach (Control control in panel.Controls)
                    {
                        if (control is Button && control.Text == "Verhalten hinzufügen")
                        {
                            control.Location = new Point(control.Location.X, 60 + 40 * panel.Controls.OfType<Button>().Count(b => b.Text != "Verhalten hinzufügen")); // 30px Höhe + 10px Abstand
                            break;
                        }
                    }
                }

                // Correct the scene counter based on the number of loaded scenes
                sceneCounter = scenes.Count + 1;

                // Ensure buttons are positioned correctly after loading scenes
                UpdateSceneButtonPositions();
            }
        }

        public void ShowBehaviorManager(Panel scenePanel)
        {
            SceneManager.ShowBehaviorManager(this, scenePanel);
        }
    }
}
