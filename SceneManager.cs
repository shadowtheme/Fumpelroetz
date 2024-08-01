using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EscapeRoomControlPanel
{
    public static class SceneManager
    {
        public static void AddNewScene(EinstellungenForm form, ref List<Panel> scenePanels, ref List<Button> szeneButtons, ref int sceneCounter)
        {
            Panel newScenePanel = new Panel
            {
                Size = new System.Drawing.Size(760, 400),
                Location = new System.Drawing.Point(12, 66),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SystemColors.ControlLight,
                Name = $"ScenePanel_{sceneCounter}"
            };

            Button newSceneButton = new Button
            {
                Size = new System.Drawing.Size(100, 30),
                Text = $"Scene {sceneCounter}"
            };
            newSceneButton.Click += (s, e) => form.ShowScene(newScenePanel);

            Panel headerPanel = new Panel
            {
                Size = new System.Drawing.Size(760, 50),
                Dock = DockStyle.Top,
                BackColor = System.Drawing.Color.Gray
            };

            TextBox sceneNameTextBox = new TextBox
            {
                Text = $"Scene {sceneCounter}",
                Dock = DockStyle.Left,
                TextAlign = HorizontalAlignment.Left,
                BorderStyle = BorderStyle.None,
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
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

            newScenePanel.Click += form.EinstellungenForm_Click;
            headerPanel.Click += form.EinstellungenForm_Click;

            Button deleteButton = new Button
            {
                Text = "Szene löschen",
                Dock = DockStyle.Right,
                Width = 60
            };
            deleteButton.Click += (s, e) => form.DeleteScene(newScenePanel, newSceneButton);

            Button addBehaviorButton = new Button
            {
                Text = "Verhalten hinzufügen",
                Location = new System.Drawing.Point(10, 60),
                Size = new System.Drawing.Size(150, 30)
            };
            addBehaviorButton.Click += (s, e) => form.ShowBehaviorManager();

            headerPanel.Controls.Add(sceneNameTextBox);
            headerPanel.Controls.Add(deleteButton);

            newScenePanel.Controls.Add(headerPanel);
            newScenePanel.Controls.Add(addBehaviorButton);

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
    }
}
