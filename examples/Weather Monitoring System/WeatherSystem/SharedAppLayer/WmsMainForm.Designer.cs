namespace SharedAppLayer
{
    partial class WmsMainForm
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
            this._startStopButton = new System.Windows.Forms.Button();
            this._serverList = new System.Windows.Forms.ListView();
            this.serverProcessIdHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.serverEndPointHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.serversLabel = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.processIdLabel = new System.Windows.Forms.Label();
            this.processId = new System.Windows.Forms.Label();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.dsdiLabel = new System.Windows.Forms.Label();
            this.dsdiCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _startStopButton
            // 
            this._startStopButton.Location = new System.Drawing.Point(22, 12);
            this._startStopButton.Name = "_startStopButton";
            this._startStopButton.Size = new System.Drawing.Size(75, 23);
            this._startStopButton.TabIndex = 0;
            this._startStopButton.Text = "Start";
            this._startStopButton.UseVisualStyleBackColor = true;
            this._startStopButton.Click += new System.EventHandler(this._startStopButton_Click);
            // 
            // _serverList
            // 
            this._serverList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.serverProcessIdHeader,
            this.serverEndPointHeader});
            this._serverList.Enabled = false;
            this._serverList.Location = new System.Drawing.Point(22, 67);
            this._serverList.Name = "_serverList";
            this._serverList.Size = new System.Drawing.Size(303, 132);
            this._serverList.TabIndex = 1;
            this._serverList.UseCompatibleStateImageBehavior = false;
            this._serverList.View = System.Windows.Forms.View.Details;
            // 
            // serverProcessIdHeader
            // 
            this.serverProcessIdHeader.Text = "Process Id";
            this.serverProcessIdHeader.Width = 78;
            // 
            // serverEndPointHeader
            // 
            this.serverEndPointHeader.Text = "Server End Point";
            this.serverEndPointHeader.Width = 206;
            // 
            // serversLabel
            // 
            this.serversLabel.AutoSize = true;
            this.serversLabel.Location = new System.Drawing.Point(19, 51);
            this.serversLabel.Name = "serversLabel";
            this.serversLabel.Size = new System.Drawing.Size(108, 13);
            this.serversLabel.TabIndex = 2;
            this.serversLabel.Text = "Known Data Servers:";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.Location = new System.Drawing.Point(522, 17);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(49, 13);
            this.status.TabIndex = 3;
            this.status.Text = "(status)";
            this.status.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // processIdLabel
            // 
            this.processIdLabel.AutoSize = true;
            this.processIdLabel.Location = new System.Drawing.Point(350, 17);
            this.processIdLabel.Name = "processIdLabel";
            this.processIdLabel.Size = new System.Drawing.Size(60, 13);
            this.processIdLabel.TabIndex = 8;
            this.processIdLabel.Text = "Process Id:";
            // 
            // processId
            // 
            this.processId.AutoSize = true;
            this.processId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processId.Location = new System.Drawing.Point(411, 17);
            this.processId.Name = "processId";
            this.processId.Size = new System.Drawing.Size(25, 13);
            this.processId.TabIndex = 9;
            this.processId.Text = "(id)";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 200;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // dsdiLabel
            // 
            this.dsdiLabel.Location = new System.Drawing.Point(350, 67);
            this.dsdiLabel.Name = "dsdiLabel";
            this.dsdiLabel.Size = new System.Drawing.Size(137, 34);
            this.dsdiLabel.TabIndex = 10;
            this.dsdiLabel.Text = "# of Data Server Discovery Conversations as Initiator";
            // 
            // dsdiCount
            // 
            this.dsdiCount.AutoSize = true;
            this.dsdiCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dsdiCount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.dsdiCount.Location = new System.Drawing.Point(548, 67);
            this.dsdiCount.Name = "dsdiCount";
            this.dsdiCount.Size = new System.Drawing.Size(23, 13);
            this.dsdiCount.TabIndex = 11;
            this.dsdiCount.Text = "(#)";
            this.dsdiCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // WmsMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 222);
            this.Controls.Add(this.dsdiCount);
            this.Controls.Add(this.dsdiLabel);
            this.Controls.Add(this.processId);
            this.Controls.Add(this.processIdLabel);
            this.Controls.Add(this.status);
            this.Controls.Add(this.serversLabel);
            this.Controls.Add(this._serverList);
            this.Controls.Add(this._startStopButton);
            this.Name = "WmsMainForm";
            this.Text = "Wms App Work";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WmsMainForm_FormClosed);
            this.Load += new System.EventHandler(this.WmsMainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _startStopButton;
        private System.Windows.Forms.ListView _serverList;
        private System.Windows.Forms.ColumnHeader serverProcessIdHeader;
        private System.Windows.Forms.ColumnHeader serverEndPointHeader;
        private System.Windows.Forms.Label serversLabel;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label processIdLabel;
        private System.Windows.Forms.Label processId;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Label dsdiLabel;
        private System.Windows.Forms.Label dsdiCount;
    }
}

