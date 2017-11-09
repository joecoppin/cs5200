namespace Registry
{
    partial class RegistryDisplay
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
            this.registrationLabel = new System.Windows.Forms.Label();
            this.registrationCount = new System.Windows.Forms.Label();
            this.shutdownButton = new System.Windows.Forms.Button();
            this.publicEndPoint = new System.Windows.Forms.Label();
            this.publicEndPointLabel = new System.Windows.Forms.Label();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // registrationLabel
            // 
            this.registrationLabel.AutoSize = true;
            this.registrationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrationLabel.Location = new System.Drawing.Point(12, 48);
            this.registrationLabel.Name = "registrationLabel";
            this.registrationLabel.Size = new System.Drawing.Size(157, 24);
            this.registrationLabel.TabIndex = 0;
            this.registrationLabel.Text = "# of Registrations:";
            // 
            // registrationCount
            // 
            this.registrationCount.AutoSize = true;
            this.registrationCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrationCount.Location = new System.Drawing.Point(188, 48);
            this.registrationCount.Name = "registrationCount";
            this.registrationCount.Size = new System.Drawing.Size(20, 24);
            this.registrationCount.TabIndex = 1;
            this.registrationCount.Text = "0";
            // 
            // shutdownButton
            // 
            this.shutdownButton.Location = new System.Drawing.Point(179, 91);
            this.shutdownButton.Name = "shutdownButton";
            this.shutdownButton.Size = new System.Drawing.Size(75, 23);
            this.shutdownButton.TabIndex = 2;
            this.shutdownButton.Text = "Shutdown";
            this.shutdownButton.UseVisualStyleBackColor = true;
            this.shutdownButton.Click += new System.EventHandler(this.shutdownButton_Click);
            // 
            // publicEndPoint
            // 
            this.publicEndPoint.AutoSize = true;
            this.publicEndPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.publicEndPoint.Location = new System.Drawing.Point(188, 9);
            this.publicEndPoint.Name = "publicEndPoint";
            this.publicEndPoint.Size = new System.Drawing.Size(0, 24);
            this.publicEndPoint.TabIndex = 4;
            // 
            // publicEndPointLabel
            // 
            this.publicEndPointLabel.AutoSize = true;
            this.publicEndPointLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.publicEndPointLabel.Location = new System.Drawing.Point(12, 9);
            this.publicEndPointLabel.Name = "publicEndPointLabel";
            this.publicEndPointLabel.Size = new System.Drawing.Size(154, 24);
            this.publicEndPointLabel.TabIndex = 3;
            this.publicEndPointLabel.Text = "Public End Point:";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // RegistryDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 126);
            this.Controls.Add(this.publicEndPoint);
            this.Controls.Add(this.publicEndPointLabel);
            this.Controls.Add(this.shutdownButton);
            this.Controls.Add(this.registrationCount);
            this.Controls.Add(this.registrationLabel);
            this.Name = "RegistryDisplay";
            this.Text = "Registry";
            this.Load += new System.EventHandler(this.RegistryDisplay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label registrationLabel;
        private System.Windows.Forms.Label registrationCount;
        private System.Windows.Forms.Button shutdownButton;
        private System.Windows.Forms.Label publicEndPoint;
        private System.Windows.Forms.Label publicEndPointLabel;
        private System.Windows.Forms.Timer refreshTimer;
    }
}

