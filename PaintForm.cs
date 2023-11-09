using Microsoft.VisualBasic;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LaborProject
{
    public partial class PaintForm : Form
    {
        private bool drawing;
        private Point previousPoint;
        private Pen drawingPen = new Pen(Color.Black, 2);

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
        private ToolStripButton createNewCanvasButton;
        private ToolStripButton saveButton;
        private ToolStripButton openButton;
        private ToolStripButton selectColorButton;
        private ToolStripButton closeButton;
        private PictureBox pictureBox;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabelCoordinates;
        private System.Windows.Forms.TrackBar trackBar;
        private Stack<Bitmap> undoStack = new Stack<Bitmap>();
        private Stack<Bitmap> redoStack = new Stack<Bitmap>();

        public PaintForm()
        {
            InitializeComponent();
            InitializeUI();
            NewToolStripMenuItem_Click(this, EventArgs.Empty);

        }

        private void InitializeUI()
        {
            this.Height = 880;
            this.Width = 1620;
            
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem("File");
            editToolStripMenuItem = new ToolStripMenuItem("Edit");
            helpToolStripMenuItem = new ToolStripMenuItem("Help");
            newToolStripMenuItem = new ToolStripMenuItem("New");
            openToolStripMenuItem = new ToolStripMenuItem("Open");
            saveToolStripMenuItem = new ToolStripMenuItem("Save");
            exitToolStripMenuItem = new ToolStripMenuItem("Exit");
            undoToolStripMenuItem = new ToolStripMenuItem("Undo");
            redoToolStripMenuItem = new ToolStripMenuItem("Redo");
            penSettingsToolStripMenuItem = new ToolStripMenuItem("Pen Settings");
            aboutToolStripMenuItem = new ToolStripMenuItem("About");
            toolStrip = new ToolStrip();
            createNewCanvasButton = new ToolStripButton("Blank");
            saveButton = new ToolStripButton("Save");
            openButton = new ToolStripButton("Open");
            selectColorButton = new ToolStripButton("Select Color");
            closeButton = new ToolStripButton("Close");
            pictureBox = new PictureBox();
            statusStrip = new StatusStrip();
            toolStripStatusLabelCoordinates = new ToolStripStatusLabel();
            trackBar = new System.Windows.Forms.TrackBar();

            newToolStripMenuItem.Click += NewToolStripMenuItem_Click;
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            exitToolStripMenuItem.Click += CloseApplicationToolStripMenuItem_Click;
            undoToolStripMenuItem.Click += UndoToolStripMenuItem_Click;
            redoToolStripMenuItem.Click += RedoToolStripMenuItem_Click;
            penSettingsToolStripMenuItem.Click += PenSettingsToolStripMenuItem_Click;
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;

            createNewCanvasButton.Click += NewToolStripMenuItem_Click;
            saveButton.Click += SaveToolStripMenuItem_Click;
            openButton.Click += OpenToolStripMenuItem_Click;
            selectColorButton.Click += SelectColorToolStripMenuItem_Click;
            closeButton.Click += CloseApplicationToolStripMenuItem_Click;

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

            //toolStrip.

            toolStrip.Items.Add(createNewCanvasButton);
            toolStrip.Items.Add(saveButton);
            toolStrip.Items.Add(openButton);
            toolStrip.Items.Add(selectColorButton);
            toolStrip.Items.Add(closeButton);
            Controls.Add(toolStrip);

            //PictureBox

            pictureBox.Dock = DockStyle.Fill;
            pictureBox.BackColor = Color.White;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            Controls.Add(pictureBox);

            //StatusStrip & TrackBar.

            statusStrip.Items.Add(toolStripStatusLabelCoordinates);
            Controls.Add(statusStrip);

            trackBar.Minimum = 1;
            trackBar.Maximum = 10;
            trackBar.Value = 2;
            trackBar.Dock = DockStyle.Bottom;
            trackBar.ValueChanged += TrackBar_ValueChanged;
            Controls.Add(trackBar);

            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N; // Ctrl + N для New
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O; // Ctrl + O для Open
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S; // Ctrl + S для Save
            exitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X; // Ctrl + X для Exit
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z; // Ctrl + Z для Undo
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Z; // Ctrl + Shift + Z для Redo
            penSettingsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P; // Ctrl + P для Pen Settings
            aboutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;


        }

        private void PushToUndoStack()
        {
            Bitmap currentImage = new Bitmap(pictureBox.Image);
            undoStack.Push(currentImage);
        }

        private void PushToRedoStack()
        {
            Bitmap currentImage = new Bitmap(pictureBox.Image);
            redoStack.Push(currentImage);
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap newImage = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = newImage;
            PushToUndoStack();
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
            if (drawing)
            {
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
                    PushToUndoStack();
                    using (Graphics g = Graphics.FromImage(pictureBox.Image))
                    {
                        Pen eraserPen = new Pen(Color.White, trackBar.Value);
                        g.DrawLine(eraserPen, previousPoint, e.Location);
                    }
                    previousPoint = e.Location;
                }
            }
            pictureBox.Invalidate();
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }
        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            using (PenSettingsForm penSettingsForm = new PenSettingsForm(drawingPen))
                drawingPen = penSettingsForm.SelectedPen;
                drawingPen.Width = trackBar.Value;
        }
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                PushToRedoStack();
                Bitmap previousImage = undoStack.Pop();
                pictureBox.Image = new Bitmap(previousImage);
            }
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                PushToUndoStack();
                Bitmap nextImage = redoStack.Pop();
                pictureBox.Image = new Bitmap(nextImage);
            }
        }
        

        private void PenSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PenSettingsForm penSettingsForm = new PenSettingsForm(drawingPen))
            {
                if (penSettingsForm.ShowDialog() == DialogResult.OK)
                {
                    drawingPen = penSettingsForm.SelectedPen;
                }
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("author: Max", "Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
