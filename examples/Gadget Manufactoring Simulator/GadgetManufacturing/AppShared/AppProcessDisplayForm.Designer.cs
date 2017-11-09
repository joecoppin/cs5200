namespace AppShared
{
    partial class AppProcessDisplayForm
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
            this.components = new System.ComponentModel.Container();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.publicEndPoint = new System.Windows.Forms.Label();
            this.publicEndPointLabel = new System.Windows.Forms.Label();
            this.processId = new System.Windows.Forms.Label();
            this.processIdLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // refreshTimer
            // 
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // publicEndPoint
            // 
            this.publicEndPoint.AutoSize = true;
            this.publicEndPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.publicEndPoint.Location = new System.Drawing.Point(188, 9);
            this.publicEndPoint.Name = "publicEndPoint";
            this.publicEndPoint.Size = new System.Drawing.Size(0, 24);
            this.publicEndPoint.TabIndex = 9;
            // 
            // publicEndPointLabel
            // 
            this.publicEndPointLabel.AutoSize = true;
            this.publicEndPointLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.publicEndPointLabel.Location = new System.Drawing.Point(12, 9);
            this.publicEndPointLabel.Name = "publicEndPointLabel";
            this.publicEndPointLabel.Size = new System.Drawing.Size(154, 24);
            this.publicEndPointLabel.TabIndex = 8;
            this.publicEndPointLabel.Text = "Public End Point:";
            // 
            // processId
            // 
            this.processId.AutoSize = true;
            this.processId.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processId.Location = new System.Drawing.Point(188, 48);
            this.processId.Name = "processId";
            this.processId.Size = new System.Drawing.Size(20, 24);
            this.processId.TabIndex = 6;
            this.processId.Text = "0";
            // 
            // processIdLabel
            // 
            this.processIdLabel.AutoSize = true;
            this.processIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processIdLabel.Location = new System.Drawing.Point(12, 48);
            this.processIdLabel.Name = "processIdLabel";
            this.processIdLabel.Size = new System.Drawing.Size(103, 24);
            this.processIdLabel.TabIndex = 5;
            this.processIdLabel.Text = "Process Id:";
            // 
            // AppProcessDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 85);
            this.Controls.Add(this.publicEndPoint);
            this.Controls.Add(this.publicEndPointLabel);
            this.Controls.Add(this.processId);
            this.Controls.Add(this.processIdLabel);
            this.Name = "AppProcessDisplayForm";
            this.Text = "AppProcessDisplayForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppProcessDisplayForm_FormClosing);
            this.Load += new System.EventHandler(this.AppProcessDisplayForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Label publicEndPoint;
        private System.Windows.Forms.Label publicEndPointLabel;
        private System.Windows.Forms.Label processId;
        private System.Windows.Forms.Label processIdLabel;
    }
}