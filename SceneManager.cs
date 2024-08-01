using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EscapeRoomControlPanel
{
    public static class SceneManager
    {
        public static void AddNewScene(EinstellungenForm mainForm, ref List<Panel> scenePanels, ref List<Button> szeneButtons, ref int sceneCounter)
        {
            Panel newScenePanel = new Panel
            {
                Size = new Size(760, 400),
                Location = new Point(12, 66),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SystemColors.ControlLight,
                Name = $"ScenePanel_{sceneCounter}"
            };

            Button newSceneButton = new Button
            {
                Size = new Size(100, 30),
                Text = $"Scene {sceneCounter}"
            };
            newSceneButton.Click += (s, e) => mainForm.ShowScene(newScenePanel);

            Panel headerPanel = new Panel
            {
                Size = new Size(760, 50),
                Dock = DockStyle.Top,
                BackColor = Color.Gray
            };

            TextBox sceneNameTextBox = new TextBox
            {
                Text = $"Scene {sceneCounter}",
                Dock = DockStyle.Left,
                TextAlign = HorizontalAlignment.Left,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Width = 700
            };
            sceneNameTextBox.Enter += (s, e) => sceneNameTextBox.SelectAll();
            sceneNameTextBox.Leave += (s, e) => mainForm.RenameScene(newScenePanel, newSceneButton, sceneNameTextBox.Text);
            sceneNameTextBox.Click += (s, e) =>
            {
                sceneNameTextBox.Focus();
                sceneNameTextBox.SelectAll();
            };
            sceneNameTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    mainForm.ActiveControl = null; // Remove focus from TextBox
                    mainForm.RenameScene(newScenePanel, newSceneButton, sceneNameTextBox.Text);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };

            newScenePanel.Click += (s, e) => mainForm.ActiveControl = null;
            headerPanel.Click += (s, e) => mainForm.ActiveControl = null;

            Button deleteButton = new Button
            {
                Text = "Szene löschen",
                Dock = DockStyle.Right,
                Width = 60
            };
            deleteButton.Click += (s, e) => mainForm.DeleteScene(newScenePanel, newSceneButton);

            headerPanel.Controls.Add(sceneNameTextBox);
            headerPanel.Controls.Add(deleteButton);

            newScenePanel.Controls.Add(headerPanel);

            foreach (var panel in scenePanels)
            {
                panel.Visible = false;
            }

            scenePanels.Add(newScenePanel);
            mainForm.Controls.Add(newScenePanel);
            newScenePanel.BringToFront();

            szeneButtons.Add(newSceneButton);
            mainForm.Controls.Add(newSceneButton);

            sceneCounter++;
            mainForm.UpdateSceneButtonPositions();
        }
    }
}
