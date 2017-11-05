using SharedAppLayer;

namespace WeatherStation
{
    partial class MainForm : WmsMainForm
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
            this.wdpiCount = new System.Windows.Forms.Label();
            this.wdpiLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // wdpiCount
            // 
            this.wdpiCount.AutoSize = true;
            this.wdpiCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wdpiCount.Location = new System.Drawing.Point(548, 114);
            this.wdpiCount.Name = "wdpiCount";
            this.wdpiCount.Size = new System.Drawing.Size(23, 13);
            this.wdpiCount.TabIndex = 13;
            this.wdpiCount.Text = "(#)";
            this.wdpiCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // wdpiLabel
            // 
            this.wdpiLabel.Location = new System.Drawing.Point(350, 114);
            this.wdpiLabel.Name = "wdpiLabel";
            this.wdpiLabel.Size = new System.Drawing.Size(159, 32);
            this.wdpiLabel.TabIndex = 12;
            this.wdpiLabel.Text = "# of Weather Data Publication Conversations as Initiator";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 220);
            this.Controls.Add(this.wdpiCount);
            this.Controls.Add(this.wdpiLabel);
            this.Name = "MainForm";
            this.Text = "Weather Station";
            this.Controls.SetChildIndex(this.wdpiLabel, 0);
            this.Controls.SetChildIndex(this.wdpiCount, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label wdpiCount;
        private System.Windows.Forms.Label wdpiLabel;
    }
}

