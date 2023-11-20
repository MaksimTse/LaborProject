using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LaborProject
{
    public partial class PaintForm : Form
    {
        private bool drawing;
        private Point previousPoint;
        private Pen drawingPen = new Pen(Color.Black, 2);

        private float zoomFactor = 1.0f; // Initial zoom factor
        private const float zoomStep = 0.1f;

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem penSettingsToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip toolStrip;
        private PictureBox pictureBox;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabelCoordinates;
        private TrackBar trackBar;
        private List<Bitmap> history = new List<Bitmap>();
        private int historyIndex = -1;

        public PaintForm()
        {
            InitializeComponent();
            InitializeUI();
            DarkTheme();
            NewToolStripMenuItem_Click(this, EventArgs.Empty);
            this.Text = "Paint";
        }
        private void InitializeUI()
        {
            this.Height = 920;
            this.Width = 1080;

            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem("Fail");
            fileToolStripMenuItem.Image = Image.FromFile("document.png");

            editToolStripMenuItem = new ToolStripMenuItem("Redigeeri");
            editToolStripMenuItem.Image = Image.FromFile("file-edit.png");

            helpToolStripMenuItem = new ToolStripMenuItem("Abi");
            helpToolStripMenuItem.Image = Image.FromFile("interrogation.png");

            newToolStripMenuItem = new ToolStripMenuItem("Uus");
            newToolStripMenuItem.Image = Image.FromFile("add-document.png");

            openToolStripMenuItem = new ToolStripMenuItem("Ava");
            openToolStripMenuItem.Image = Image.FromFile("folder-open.png");

            saveToolStripMenuItem = new ToolStripMenuItem("Salvesta");
            saveToolStripMenuItem.Image = Image.FromFile("disk.png");

            exitToolStripMenuItem = new ToolStripMenuItem("Välja");
            exitToolStripMenuItem.Image = Image.FromFile("exit.png");

            undoToolStripMenuItem = new ToolStripMenuItem("Tagasi");
            undoToolStripMenuItem.Image = Image.FromFile("undo.png");

            redoToolStripMenuItem = new ToolStripMenuItem("Edasi");
            redoToolStripMenuItem.Image = Image.FromFile("redo.png");

            penSettingsToolStripMenuItem = new ToolStripMenuItem("Pliiatsi seaded");
            penSettingsToolStripMenuItem.Image = Image.FromFile("pen-clip.png");

            aboutToolStripMenuItem = new ToolStripMenuItem("Program kohta");
            aboutToolStripMenuItem.Image = Image.FromFile("information.png");

            toolStrip = new ToolStrip();
            pictureBox = new PictureBox();
            statusStrip = new StatusStrip();
            toolStripStatusLabelCoordinates = new ToolStripStatusLabel();
            trackBar = new TrackBar();

            newToolStripMenuItem.Click += NewToolStripMenuItem_Click;
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            exitToolStripMenuItem.Click += CloseApplicationToolStripMenuItem_Click;
            undoToolStripMenuItem.Click += UndoToolStripMenuItem_Click;
            redoToolStripMenuItem.Click += RedoToolStripMenuItem_Click;
            penSettingsToolStripMenuItem.Click += PenSettingsToolStripMenuItem_Click;
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;

            fileToolStripMenuItem.DropDownItems.Add(newToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(openToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(saveToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(exitToolStripMenuItem);
            editToolStripMenuItem.DropDownItems.Add(undoToolStripMenuItem);
            editToolStripMenuItem.DropDownItems.Add(redoToolStripMenuItem);
            editToolStripMenuItem.DropDownItems.Add(penSettingsToolStripMenuItem);
            helpToolStripMenuItem.DropDownItems.Add(aboutToolStripMenuItem);
            menuStrip.Items.Add(fileToolStripMenuItem);
            menuStrip.Items.Add(editToolStripMenuItem);
            menuStrip.Items.Add(helpToolStripMenuItem);
            Controls.Add(menuStrip);

            pictureBox.MouseWheel += PictureBox_MouseWheel;

            pictureBox.Dock = DockStyle.Fill;
            pictureBox.BackColor = Color.White;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            Controls.Add(pictureBox);

            statusStrip.Items.Add(toolStripStatusLabelCoordinates);
            Controls.Add(statusStrip);

            trackBar.Minimum = 1;
            trackBar.Maximum = 20;
            trackBar.Value = 2;
            trackBar.Dock = DockStyle.Bottom;
            trackBar.ValueChanged += TrackBar_ValueChanged;
            Controls.Add(trackBar);

            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            exitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Z;
            penSettingsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
            aboutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;

            ToolStrip imageToolStrip = new ToolStrip();
            imageToolStrip.Dock = DockStyle.Left;
            Controls.Add(imageToolStrip);

            ToolStripButton saveButton = new ToolStripButton();
            saveButton.Image = Image.FromFile("Save-Disk.png");
            saveButton.Click += SaveToolStripMenuItem_Click;
            saveButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            imageToolStrip.Items.Add(saveButton);

            ToolStripButton openButton = new ToolStripButton();
            openButton.Image = Image.FromFile("Open-file.png");
            openButton.Click += OpenToolStripMenuItem_Click;
            openButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            imageToolStrip.Items.Add(openButton);

            ToolStripButton penSettingsButton = new ToolStripButton();
            penSettingsButton.Image = Image.FromFile("Pen.png");
            penSettingsButton.Click += PenSettingsToolStripMenuItem_Click;
            penSettingsButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            imageToolStrip.Items.Add(penSettingsButton);

            ToolStripButton newButton = new ToolStripButton();
            newButton.Image = Image.FromFile("New-file.png");
            newButton.Click += NewToolStripMenuItem_Click;
            newButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            imageToolStrip.Items.Add(newButton);

            ToolStripButton clearButton = new ToolStripButton();
            clearButton.Image = Image.FromFile("close-file.png");
            clearButton.Click += CloseApplicationToolStripMenuItem_Click;
            clearButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            imageToolStrip.Items.Add(clearButton);
        }
        private void ApplyZoom()
        {
            int newWidth = (int)(pictureBox.Image.Width * zoomFactor);
            int newHeight = (int)(pictureBox.Image.Height * zoomFactor);
            Bitmap zoomedBitmap = new Bitmap(pictureBox.Image, newWidth, newHeight);

            pictureBox.Image = zoomedBitmap;
            pictureBox.Invalidate();
        }
        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                zoomFactor += zoomStep;
            }
            else
            {
                zoomFactor = Math.Max(zoomStep, zoomFactor - zoomStep);
            }
            ApplyZoom();

            toolStripStatusLabelCoordinates.Text = $"Zoom: {zoomFactor:P0}";
        }

        private void DarkTheme()
        {
            Color darkBackColor = Color.FromArgb(31, 31, 31);
            Color darkForeColor = Color.White;

            this.BackColor = darkBackColor;
            statusStrip.BackColor = darkBackColor;
            statusStrip.ForeColor = darkForeColor;
            trackBar.BackColor = darkBackColor;
            trackBar.ForeColor = darkForeColor;
        }
        private void PushToHistory(Bitmap image)
        {
            history = history.GetRange(0, historyIndex + 1);
            history.Add(new Bitmap(image));
            historyIndex++;
        }
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap newImage = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = newImage;
            history.Clear();
            historyIndex = -1;
            PushToHistory(newImage);
        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = new Bitmap(dialog.FileName);
                }
            }
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "BMP Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image.Save(dialog.FileName);
                }
            }
        }
        private void SelectColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    drawingPen.Color = dialog.Color;
                }
            }
        }
        private void CloseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                previousPoint = e.Location;
            }
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabelCoordinates.Text = $"X: {e.X}, Y: {e.Y}";

            if (drawing)
            {
                PushToHistory(new Bitmap(pictureBox.Image));
                if (e.Button == MouseButtons.Left)
                {
                    using (Graphics g = Graphics.FromImage(pictureBox.Image))
                    {
                        g.DrawLine(drawingPen, previousPoint, e.Location);
                    }
                    previousPoint = e.Location;
                }
                else
                {
                    using (Graphics g = Graphics.FromImage(pictureBox.Image))
                    {
                        Pen eraserPen = new Pen(Color.White, trackBar.Value);
                        g.DrawLine(eraserPen, previousPoint, e.Location);
                    }
                    previousPoint = e.Location;
                }
                pictureBox.Invalidate();
            }
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            ToolStripButton clickedButton = (ToolStripButton)sender;

            int imageIndex = clickedButton.Owner.Items.IndexOf(clickedButton) + 1;
            string imagePath = $"image{imageIndex}.png"; // Adjust the naming convention
            pictureBox.Image = Image.FromFile(imagePath);
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }
        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            drawingPen.Width = trackBar.Value;
        }
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (historyIndex > 0)
            {
                historyIndex--;
                pictureBox.Image = new Bitmap(history[historyIndex]);
                pictureBox.Invalidate();
            }
        }
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (historyIndex < history.Count - 1)
            {
                historyIndex++;
                pictureBox.Image = new Bitmap(history[historyIndex]);
                pictureBox.Invalidate();
            }
        }
        private void PenSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PenSettingsForm penSettingsForm = new PenSettingsForm(drawingPen))
            {
                if (penSettingsForm.ShowDialog() == DialogResult.OK)
                {
                    drawingPen.Color = penSettingsForm.SelectedColor;
                }
            }
        }
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("author: Max", "Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}