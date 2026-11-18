namespace MediaAudioController
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBoxMediaOutput = new GroupBox();
            buttonRefresh = new Button();
            comboBoxOnPlayOutput = new ComboBox();
            comboBoxOnPauseOutput = new ComboBox();
            labelPlayOutput = new Label();
            labelPauseOutput = new Label();
            groupBoxMediaControls = new GroupBox();
            buttonPlayPause = new Button();
            buttonPause = new Button();
            buttonPlay = new Button();
            groupBoxMediaOutput.SuspendLayout();
            groupBoxMediaControls.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxMediaOutput
            // 
            groupBoxMediaOutput.Controls.Add(buttonRefresh);
            groupBoxMediaOutput.Controls.Add(comboBoxOnPlayOutput);
            groupBoxMediaOutput.Controls.Add(comboBoxOnPauseOutput);
            groupBoxMediaOutput.Controls.Add(labelPlayOutput);
            groupBoxMediaOutput.Controls.Add(labelPauseOutput);
            groupBoxMediaOutput.Location = new Point(12, 12);
            groupBoxMediaOutput.Name = "groupBoxMediaOutput";
            groupBoxMediaOutput.Size = new Size(746, 149);
            groupBoxMediaOutput.TabIndex = 0;
            groupBoxMediaOutput.TabStop = false;
            groupBoxMediaOutput.Text = "Media Output";
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(609, 43);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(112, 82);
            buttonRefresh.TabIndex = 4;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // comboBoxOnPlayOutput
            // 
            comboBoxOnPlayOutput.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOnPlayOutput.FormattingEnabled = true;
            comboBoxOnPlayOutput.Location = new Point(188, 43);
            comboBoxOnPlayOutput.Name = "comboBoxOnPlayOutput";
            comboBoxOnPlayOutput.Size = new Size(405, 33);
            comboBoxOnPlayOutput.TabIndex = 3;
            // 
            // comboBoxOnPauseOutput
            // 
            comboBoxOnPauseOutput.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOnPauseOutput.FormattingEnabled = true;
            comboBoxOnPauseOutput.Location = new Point(188, 89);
            comboBoxOnPauseOutput.Name = "comboBoxOnPauseOutput";
            comboBoxOnPauseOutput.Size = new Size(405, 33);
            comboBoxOnPauseOutput.TabIndex = 2;
            // 
            // labelPlayOutput
            // 
            labelPlayOutput.AutoSize = true;
            labelPlayOutput.Location = new Point(17, 46);
            labelPlayOutput.Name = "labelPlayOutput";
            labelPlayOutput.Size = new Size(152, 25);
            labelPlayOutput.TabIndex = 1;
            labelPlayOutput.Text = "Switch on Play to:";
            // 
            // labelPauseOutput
            // 
            labelPauseOutput.AutoSize = true;
            labelPauseOutput.Location = new Point(17, 92);
            labelPauseOutput.Name = "labelPauseOutput";
            labelPauseOutput.Size = new Size(165, 25);
            labelPauseOutput.TabIndex = 0;
            labelPauseOutput.Text = "Switch on Pause to:";
            // 
            // groupBoxMediaControls
            // 
            groupBoxMediaControls.Controls.Add(buttonPlayPause);
            groupBoxMediaControls.Controls.Add(buttonPause);
            groupBoxMediaControls.Controls.Add(buttonPlay);
            groupBoxMediaControls.Location = new Point(12, 167);
            groupBoxMediaControls.Name = "groupBoxMediaControls";
            groupBoxMediaControls.Size = new Size(746, 169);
            groupBoxMediaControls.TabIndex = 1;
            groupBoxMediaControls.TabStop = false;
            groupBoxMediaControls.Text = "Media Controls";
            // 
            // buttonPlayPause
            // 
            buttonPlayPause.Location = new Point(520, 42);
            buttonPlayPause.Name = "buttonPlayPause";
            buttonPlayPause.Size = new Size(201, 100);
            buttonPlayPause.TabIndex = 2;
            buttonPlayPause.Text = "Play/Pause";
            buttonPlayPause.UseVisualStyleBackColor = true;
            buttonPlayPause.Click += buttonPlayPause_Click;
            // 
            // buttonPause
            // 
            buttonPause.Location = new Point(267, 42);
            buttonPause.Name = "buttonPause";
            buttonPause.Size = new Size(201, 100);
            buttonPause.TabIndex = 1;
            buttonPause.Text = "Pause";
            buttonPause.UseVisualStyleBackColor = true;
            buttonPause.Click += buttonPause_Click;
            // 
            // buttonPlay
            // 
            buttonPlay.Location = new Point(17, 42);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(201, 100);
            buttonPlay.TabIndex = 0;
            buttonPlay.Text = "Play";
            buttonPlay.UseVisualStyleBackColor = true;
            buttonPlay.Click += buttonPlay_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(774, 358);
            Controls.Add(groupBoxMediaControls);
            Controls.Add(groupBoxMediaOutput);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            groupBoxMediaOutput.ResumeLayout(false);
            groupBoxMediaOutput.PerformLayout();
            groupBoxMediaControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxMediaOutput;
        private Label labelPauseOutput;
        private ComboBox comboBoxOnPlayOutput;
        private ComboBox comboBoxOnPauseOutput;
        private Label labelPlayOutput;
        private Button buttonRefresh;
        private GroupBox groupBoxMediaControls;
        private Button buttonPlayPause;
        private Button buttonPause;
        private Button buttonPlay;
    }
}
