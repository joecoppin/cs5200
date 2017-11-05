namespace DataServer
{
    partial class MainForm
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
            this.dsdrCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.wdprCount = new System.Windows.Forms.Label();
            this.wdprLabel = new System.Windows.Forms.Label();
            this.gwdrCount = new System.Windows.Forms.Label();
            this.gwdrLabel = new System.Windows.Forms.Label();
            this.wdsiCount = new System.Windows.Forms.Label();
            this.wdsiLabel = new System.Windows.Forms.Label();
            this.wdsrCount = new System.Windows.Forms.Label();
            this.wdsrLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dsdrCount
            // 
            this.dsdrCount.AutoSize = true;
            this.dsdrCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dsdrCount.Location = new System.Drawing.Point(548, 102);
            this.dsdrCount.Name = "dsdrCount";
            this.dsdrCount.Size = new System.Drawing.Size(23, 13);
            this.dsdrCount.TabIndex = 13;
            this.dsdrCount.Text = "(#)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(350, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 34);
            this.label2.TabIndex = 12;
            this.label2.Text = "# of Data Server Discovery Conversations as Responder";
            // 
            // wdprCount
            // 
            this.wdprCount.AutoSize = true;
            this.wdprCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wdprCount.Location = new System.Drawing.Point(548, 136);
            this.wdprCount.Name = "wdprCount";
            this.wdprCount.Size = new System.Drawing.Size(23, 13);
            this.wdprCount.TabIndex = 15;
            this.wdprCount.Text = "(#)";
            // 
            // wdprLabel
            // 
            this.wdprLabel.Location = new System.Drawing.Point(350, 136);
            this.wdprLabel.Name = "wdprLabel";
            this.wdprLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.wdprLabel.Size = new System.Drawing.Size(157, 34);
            this.wdprLabel.TabIndex = 14;
            this.wdprLabel.Text = "# of Weather Data Publication Conversations as Responder";
            // 
            // gwdrCount
            // 
            this.gwdrCount.AutoSize = true;
            this.gwdrCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gwdrCount.Location = new System.Drawing.Point(548, 170);
            this.gwdrCount.Name = "gwdrCount";
            this.gwdrCount.Size = new System.Drawing.Size(23, 13);
            this.gwdrCount.TabIndex = 17;
            this.gwdrCount.Text = "(#)";
            // 
            // gwdrLabel
            // 
            this.gwdrLabel.Location = new System.Drawing.Point(350, 170);
            this.gwdrLabel.Name = "gwdrLabel";
            this.gwdrLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gwdrLabel.Size = new System.Drawing.Size(147, 34);
            this.gwdrLabel.TabIndex = 16;
            this.gwdrLabel.Text = "# of Get Weather Data Conversations as Responder";
            // 
            // wdsiCount
            // 
            this.wdsiCount.AutoSize = true;
            this.wdsiCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wdsiCount.Location = new System.Drawing.Point(548, 204);
            this.wdsiCount.Name = "wdsiCount";
            this.wdsiCount.Size = new System.Drawing.Size(23, 13);
            this.wdsiCount.TabIndex = 19;
            this.wdsiCount.Text = "(#)";
            // 
            // wdsiLabel
            // 
            this.wdsiLabel.Location = new System.Drawing.Point(350, 204);
            this.wdsiLabel.Name = "wdsiLabel";
            this.wdsiLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.wdsiLabel.Size = new System.Drawing.Size(157, 34);
            this.wdsiLabel.TabIndex = 18;
            this.wdsiLabel.Text = "# of Weather Data Sync Conversations as Initiator";
            // 
            // wdsrCount
            // 
            this.wdsrCount.AutoSize = true;
            this.wdsrCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wdsrCount.Location = new System.Drawing.Point(548, 238);
            this.wdsrCount.Name = "wdsrCount";
            this.wdsrCount.Size = new System.Drawing.Size(23, 13);
            this.wdsrCount.TabIndex = 21;
            this.wdsrCount.Text = "(#)";
            // 
            // wdsrLabel
            // 
            this.wdsrLabel.Location = new System.Drawing.Point(350, 238);
            this.wdsrLabel.Name = "wdsrLabel";
            this.wdsrLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.wdsrLabel.Size = new System.Drawing.Size(157, 34);
            this.wdsrLabel.TabIndex = 20;
            this.wdsrLabel.Text = "# of Weather Data Sync Conversations as Responder";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 292);
            this.Controls.Add(this.wdsrCount);
            this.Controls.Add(this.wdsrLabel);
            this.Controls.Add(this.wdsiCount);
            this.Controls.Add(this.wdsiLabel);
            this.Controls.Add(this.gwdrCount);
            this.Controls.Add(this.gwdrLabel);
            this.Controls.Add(this.wdprCount);
            this.Controls.Add(this.wdprLabel);
            this.Controls.Add(this.dsdrCount);
            this.Controls.Add(this.label2);
            this.Name = "MainForm";
            this.Text = "Data Server";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dsdrCount, 0);
            this.Controls.SetChildIndex(this.wdprLabel, 0);
            this.Controls.SetChildIndex(this.wdprCount, 0);
            this.Controls.SetChildIndex(this.gwdrLabel, 0);
            this.Controls.SetChildIndex(this.gwdrCount, 0);
            this.Controls.SetChildIndex(this.wdsiLabel, 0);
            this.Controls.SetChildIndex(this.wdsiCount, 0);
            this.Controls.SetChildIndex(this.wdsrLabel, 0);
            this.Controls.SetChildIndex(this.wdsrCount, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label dsdrCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label wdprCount;
        private System.Windows.Forms.Label wdprLabel;
        private System.Windows.Forms.Label gwdrCount;
        private System.Windows.Forms.Label gwdrLabel;
        private System.Windows.Forms.Label wdsiCount;
        private System.Windows.Forms.Label wdsiLabel;
        private System.Windows.Forms.Label wdsrCount;
        private System.Windows.Forms.Label wdsrLabel;
    }
}

