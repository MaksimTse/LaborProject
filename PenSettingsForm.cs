using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaborProject
{
    public partial class PenSettingsForm : Form
    {
        public Pen SelectedPen { get; private set; }
        public Color SelectedColor { get; private set; }

        private ComboBox styleComboBox;
        private TrackBar widthTrackBar;
        private Button applyButton;
        private ColorDialog colorDialog;

        public PenSettingsForm(Pen currentPen)
        {
            InitializeComponent();

            this.Height = 180;
            this.Width = 260;

            SelectedPen = currentPen;

            Label styleLabel = new Label();
            styleLabel.Text = "Style:";
            styleLabel.Location = new Point(10, 10);
            Controls.Add(styleLabel);

            styleComboBox = new ComboBox();
            styleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            styleComboBox.Items.Add("Solid");
            styleComboBox.Items.Add("Dot");
            styleComboBox.Items.Add("DashDotDot");
            styleComboBox.SelectedIndex = styleComboBox.Items.IndexOf(Enum.GetName(typeof(DashStyle), currentPen.DashStyle));
            styleComboBox.Location = new Point(120, 10);
            Controls.Add(styleComboBox);

            Label widthLabel = new Label();
            widthLabel.Text = "Width:";
            widthLabel.Location = new Point(10, 40);
            Controls.Add(widthLabel);

            widthTrackBar = new TrackBar();
            widthTrackBar.Minimum = 1;
            widthTrackBar.Maximum = 20;
            widthTrackBar.Value = (int)currentPen.Width;
            widthTrackBar.LargeChange = 1;
            widthTrackBar.Location = new Point(120, 40);
            Controls.Add(widthTrackBar);

            colorDialog = new ColorDialog();

            applyButton = new Button();
            applyButton.Text = "Apply";
            applyButton.Location = new Point(10, 70);
            applyButton.Click += ApplyButton_Click;
            Controls.Add(applyButton);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (styleComboBox.SelectedItem != null)
            {
                SelectedPen.DashStyle = (DashStyle)Enum.Parse(typeof(DashStyle), styleComboBox.SelectedItem.ToString());
            }

            SelectedPen.Width = widthTrackBar.Value;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedColor = colorDialog.Color;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }

}
