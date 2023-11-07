using Microsoft.VisualBasic;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        public PaintForm()
        {
            InitializeComponent();

            InitializeUI();
        }

        private void InitializeUI()
        {

            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem("File");
            editToolStripMenuItem = new ToolStripMenuItem("Edit");
            helpToolStripMenuItem = new ToolStripMenuItem("Help");
            newToolStripMenuItem = new ToolStripMenuItem("New (Ctrl + N)");
            openToolStripMenuItem = new ToolStripMenuItem("Open (Ctrl + O)");
            saveToolStripMenuItem = new ToolStripMenuItem("Save (Ctrl + S)");
            exitToolStripMenuItem = new ToolStripMenuItem("Exit (Ctrl + X)");
            undoToolStripMenuItem = new ToolStripMenuItem("Undo (Ctrl + Z)");
            redoToolStripMenuItem = new ToolStripMenuItem("Redo (Ctrl + Y)");
            penSettingsToolStripMenuItem = new ToolStripMenuItem("Pen Settings (Ctrl + P)");
            aboutToolStripMenuItem = new ToolStripMenuItem("About (Ctrl + A)");
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
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            
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
                using (Graphics g = Graphics.FromImage(pictureBox.Image))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        g.DrawLine(drawingPen, previousPoint, e.Location);
                        previousPoint = e.Location;
                    }
                    else
                    {
                        Pen drawingPen = new Pen(Color.White, trackBar.Value);
                        g.DrawLine(drawingPen, previousPoint, e.Location);
                        previousPoint = e.Location;
                    }
                }
                pictureBox.Invalidate();
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }
        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Изменение толщины пера
            drawingPen.Width = trackBar.Value;
        }
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Обработка действия Undo.
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Обработка действия Redo.
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
            MessageBox.Show("author: Max", "Programmmmm", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}