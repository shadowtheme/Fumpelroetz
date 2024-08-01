using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EscapeRoomControlPanel
{
    public static class SceneLoader
    {
        public static void AddLoadedScene(EinstellungenForm mainForm, SceneData sceneData)
        {
            Panel newScenePanel = new Panel
            {
                Size = new Size(760, 400),
                Location = new Point(sceneData.LocationX, sceneData.LocationY),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SystemColors.ControlLight,
                Name = sceneData.Name
            };

            Button newSceneButton = new Button
            {
                Size = new Size(100, 30),
                Text = sceneData.Name
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
                Text = sceneData.Name,
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

            foreach (var panel in mainForm.scenePanels)
            {
                panel.Visible = false;
            }

            mainForm.scenePanels.Add(newScenePanel);
            mainForm.Controls.Add(newScenePanel);
            newScenePanel.BringToFront();

            mainForm.szeneButtons.Add(newSceneButton);
            mainForm.Controls.Add(newSceneButton);

            mainForm.UpdateSceneButtonPositions();
        }
    }
}
