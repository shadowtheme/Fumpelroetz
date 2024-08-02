using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using NAudio.Wave;

namespace EscapeRoomControlPanel
{
    public partial class MediaForm : Form
    {
        internal List<SoundButton> soundButtons;
        private List<string> filePaths;
        private WaveOutEvent waveOut;
        private AudioFileReader audioFileReader;
        private SoundPlayer soundPlayer;

        public MediaForm()
        {
            InitializeComponent();
            this.Text = "Media";

            soundButtons = new List<SoundButton>();
            filePaths = new List<string>();

            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(MediaForm_DragEnter);
            this.DragDrop += new DragEventHandler(MediaForm_DragDrop);

            LoadExistingFiles();
        }

        private void MediaForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(SoundButton)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MediaForm_DragDrop(object sender, DragEventArgs e)
        {
            SoundButton draggedButton = (SoundButton)e.Data.GetData(typeof(SoundButton));
            Point dropPoint = this.PointToClient(new Point(e.X, e.Y));
            int dropIndex = (dropPoint.Y - this.ClientRectangle.Top) / draggedButton.Height;

            if (dropIndex >= 0 && dropIndex < soundButtons.Count)
            {
                MoveSoundButton(draggedButton, dropIndex);
            }
        }

        public void MoveSoundButton(SoundButton button, int newIndex)
        {
            int oldIndex = soundButtons.IndexOf(button);
            if (oldIndex < 0 || newIndex < 0 || newIndex >= soundButtons.Count || oldIndex == newIndex)
                return;

            soundButtons.RemoveAt(oldIndex);
            if (newIndex > oldIndex) newIndex--; // Adjust the index if moving downwards
            soundButtons.Insert(newIndex, button);
            ReorderSoundButtons();
        }

        private void ReorderSoundButtons()
        {
            for (int i = 0; i < soundButtons.Count; i++)
            {
                soundButtons[i].Location = new Point(12, 50 + i * 40); // Abstand zwischen den Buttons
            }
        }


        private void LoadExistingFiles()
        {
            string destinationFolder = Path.Combine(Application.StartupPath, "AudioFiles");
            if (Directory.Exists(destinationFolder))
            {
                var files = Directory.GetFiles(destinationFolder);
                foreach (var file in files)
                {
                    AddSoundButton(file);
                }
            }
        }

        private void AddSoundButton(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            filePaths.Add(filePath);

            SoundButton soundButton = new SoundButton(fileName, filePath, PlaySound, StopSound, DeleteFile);
            soundButton.Location = new Point(12, 50 + soundButtons.Count * 40); // Höherer Abstand für bessere Lesbarkeit
            this.Controls.Add(soundButton);
            soundButtons.Add(soundButton);
        }

        private void PlaySound(string filePath, Button playStopButton)
        {
            StopSound(null, null);

            if (Path.GetExtension(filePath).ToLower() == ".wav")
            {
                soundPlayer = new SoundPlayer(filePath);
                soundPlayer.Play();
            }
            else if (Path.GetExtension(filePath).ToLower() == ".mp3")
            {
                audioFileReader = new AudioFileReader(filePath);
                waveOut = new WaveOutEvent();
                waveOut.Init(audioFileReader);
                waveOut.Play();
            }

            playStopButton.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Resources", "stop.png"));
            playStopButton.Tag = "stop";
            foreach (var soundButton in soundButtons)
            {
                if (soundButton.PlayStopButton != playStopButton)
                {
                    soundButton.PlayStopButton.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Resources", "play.png"));
                    soundButton.PlayStopButton.Tag = "play";
                }
            }
        }

        private void StopSound(string filePath, Button playStopButton)
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }
            if (soundPlayer != null)
            {
                soundPlayer.Stop();
                soundPlayer = null;
            }
            if (playStopButton != null)
            {
                playStopButton.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Resources", "play.png"));
                playStopButton.Tag = "play";
            }
        }

        private void DeleteFile(string filePath)
        {
            StopSound(null, null);
            int index = filePaths.IndexOf(filePath);
            if (index >= 0 && File.Exists(filePath))
            {
                File.Delete(filePath);
                soundButtons[index].Visible = false;
                soundButtons.RemoveAt(index);
                filePaths.RemoveAt(index);
                MessageBox.Show("Datei erfolgreich gelöscht.");
            }
            else
            {
                MessageBox.Show("Die Datei konnte nicht gefunden werden.");
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFilePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(sourceFilePath);
                    string destinationFolder = Path.Combine(Application.StartupPath, "AudioFiles");
                    string destinationFilePath = Path.Combine(destinationFolder, fileName);

                    if (File.Exists(destinationFilePath))
                    {
                        MessageBox.Show("Die Datei existiert bereits.");
                        return;
                    }

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    File.Copy(sourceFilePath, destinationFilePath, true);
                    AddSoundButton(destinationFilePath);
                }
            }
        }
    }

    public class SoundButton : Panel
    {
        private Button playStopButton;
        private Button deleteButton;
        private Button dragHandleButton; // Ändere das Panel zu einem Button
        private Label fileNameLabel;
        private string filePath;
        private Action<string, Button> playAction;
        private Action<string, Button> stopAction;
        private Action<string> deleteAction;

        public SoundButton(string fileName, string filePath, Action<string, Button> playAction, Action<string, Button> stopAction, Action<string> deleteAction)
        {
            this.filePath = filePath;
            this.playAction = playAction;
            this.stopAction = stopAction;
            this.deleteAction = deleteAction;

            this.Size = new Size(420, 40); // Größer für bessere Lesbarkeit
            this.BorderStyle = BorderStyle.FixedSingle; // Rahmen hinzufügen
            this.BackColor = Color.LightGray; // Hintergrundfarbe ändern

            deleteButton = new Button
            {
                Size = new Size(23, 23),
                Location = new Point(0, 8),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Image = Image.FromFile(Path.Combine(Application.StartupPath, "Resources", "delete.png")) // Bild für den Lösch-Button
            };
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            deleteButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            deleteButton.Click += DeleteButton_Click;

            playStopButton = new Button
            {
                Size = new Size(23, 23),
                Location = new Point(360, 8),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Image = Image.FromFile(Path.Combine(Application.StartupPath, "Resources", "play.png")), // Bild für den Play-Button
                Tag = "play" // Hinzufügen eines Tags zur Unterscheidung zwischen Play und Stop
            };
            playStopButton.FlatAppearance.BorderSize = 0;
            playStopButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            playStopButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            playStopButton.Click += PlayStopButton_Click;

            fileNameLabel = new Label
            {
                Text = fileName,
                Size = new Size(240, 23),
                Location = new Point(30, 8),
                TextAlign = ContentAlignment.MiddleLeft
            };

            dragHandleButton = new Button
            {
                Size = new Size(28, 36),
                Location = new Point(390, 1),
                BackColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                Image = Image.FromFile(Path.Combine(Application.StartupPath, "Resources", "dragnDrop.png")), // Bild für den Drag-Button
            };
            dragHandleButton.FlatAppearance.BorderSize = 0;
            dragHandleButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            dragHandleButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            dragHandleButton.MouseDown += new MouseEventHandler(DragHandle_MouseDown);

            this.Controls.Add(deleteButton);
            this.Controls.Add(playStopButton);
            this.Controls.Add(fileNameLabel);
            this.Controls.Add(dragHandleButton);
        }

        private void PlayStopButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag.ToString() == "play")
            {
                playAction(filePath, button);
                button.Tag = "stop";
            }
            else
            {
                stopAction(filePath, button);
                button.Tag = "play";
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            deleteAction(filePath);
        }

        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.DoDragDrop(this, DragDropEffects.Move);
            }
        }

        private void SoundButton_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(SoundButton)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void SoundButton_DragDrop(object sender, DragEventArgs e)
        {
            SoundButton draggedButton = (SoundButton)e.Data.GetData(typeof(SoundButton));
            MediaForm parentForm = (MediaForm)this.Parent;
            Point dropPoint = parentForm.PointToClient(new Point(e.X, e.Y));
            int dropIndex = Math.Max(0, Math.Min(parentForm.soundButtons.Count - 1, dropPoint.Y / this.Height));

            if (draggedButton != this)
            {
                parentForm.MoveSoundButton(draggedButton, dropIndex);
            }
        }

        public Button PlayStopButton => playStopButton;
    }
}
