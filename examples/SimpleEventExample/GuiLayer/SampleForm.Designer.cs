namespace GuiLayer
{
    partial class SampleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._startButton = new System.Windows.Forms.Button();
            this._countDisplay = new System.Windows.Forms.Label();
            this._stopButton = new System.Windows.Forms.Button();
            this._name = new System.Windows.Forms.TextBox();
            this._nameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _startButton
            // 
            this._startButton.Location = new System.Drawing.Point(54, 93);
            this._startButton.Name = "_startButton";
            this._startButton.Size = new System.Drawing.Size(156, 63);
            this._startButton.TabIndex = 0;
            this._startButton.Text = "Start";
            this._startButton.UseVisualStyleBackColor = true;
            this._startButton.Click += new System.EventHandler(this._startButton_Click);
            // 
            // _countDisplay
            // 
            this._countDisplay.AutoSize = true;
            this._countDisplay.Location = new System.Drawing.Point(318, 112);
            this._countDisplay.Name = "_countDisplay";
            this._countDisplay.Size = new System.Drawing.Size(0, 26);
            this._countDisplay.TabIndex = 1;
            // 
            // _stopButton
            // 
            this._stopButton.Location = new System.Drawing.Point(54, 183);
            this._stopButton.Name = "_stopButton";
            this._stopButton.Size = new System.Drawing.Size(156, 63);
            this._stopButton.TabIndex = 2;
            this._stopButton.Text = "Stop";
            this._stopButton.UseVisualStyleBackColor = true;
            this._stopButton.Click += new System.EventHandler(this._stopButton_Click);
            // 
            // _name
            // 
            this._name.Location = new System.Drawing.Point(167, 32);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(246, 31);
            this._name.TabIndex = 3;
            // 
            // _nameLabel
            // 
            this._nameLabel.AutoSize = true;
            this._nameLabel.Location = new System.Drawing.Point(49, 37);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new System.Drawing.Size(77, 26);
            this._nameLabel.TabIndex = 4;
            this._nameLabel.Text = "Name:";
            // 
            // SampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 352);
            this.Controls.Add(this._nameLabel);
            this.Controls.Add(this._name);
            this.Controls.Add(this._stopButton);
            this.Controls.Add(this._countDisplay);
            this.Controls.Add(this._startButton);
            this.Name = "SampleForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _startButton;
        private System.Windows.Forms.Label _countDisplay;
        private System.Windows.Forms.Button _stopButton;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.Label _nameLabel;
    }
}

