namespace WordGuessMonitor
{
    partial class MonitorForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gameCount = new System.Windows.Forms.Label();
            this.playerCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.playersListView = new System.Windows.Forms.ListView();
            this.idHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.aliasHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gameCountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.guessHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hintCountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.exitCountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.heartbeatCountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastMessageHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.highScoreHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.startStopButton = new System.Windows.Forms.Button();
            this.serverEndPointLabel = new System.Windows.Forms.Label();
            this.serverEndPointError = new System.Windows.Forms.Label();
            this.serverEndPoint = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(317, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Active Number of Games:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(317, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "Number of Players:";
            // 
            // gameCount
            // 
            this.gameCount.AutoSize = true;
            this.gameCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameCount.Location = new System.Drawing.Point(510, 25);
            this.gameCount.Name = "gameCount";
            this.gameCount.Size = new System.Drawing.Size(29, 17);
            this.gameCount.TabIndex = 12;
            this.gameCount.Text = "(#)";
            // 
            // playerCount
            // 
            this.playerCount.AutoSize = true;
            this.playerCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerCount.Location = new System.Drawing.Point(510, 48);
            this.playerCount.Name = "playerCount";
            this.playerCount.Size = new System.Drawing.Size(29, 17);
            this.playerCount.TabIndex = 13;
            this.playerCount.Text = "(#)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Players:";
            // 
            // playersListView
            // 
            this.playersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.idHeader,
            this.aliasHeader,
            this.gameCountHeader,
            this.guessHeader,
            this.hintCountHeader,
            this.exitCountHeader,
            this.heartbeatCountHeader,
            this.lastMessageHeader,
            this.highScoreHeader});
            this.playersListView.Location = new System.Drawing.Point(28, 106);
            this.playersListView.Name = "playersListView";
            this.playersListView.Size = new System.Drawing.Size(813, 369);
            this.playersListView.TabIndex = 15;
            this.playersListView.UseCompatibleStateImageBehavior = false;
            this.playersListView.View = System.Windows.Forms.View.Details;
            // 
            // idHeader
            // 
            this.idHeader.Text = "Id";
            // 
            // aliasHeader
            // 
            this.aliasHeader.Text = "Alias";
            this.aliasHeader.Width = 106;
            // 
            // gameCountHeader
            // 
            this.gameCountHeader.Text = "Game Count";
            this.gameCountHeader.Width = 77;
            // 
            // guessHeader
            // 
            this.guessHeader.Text = "Guess Count";
            this.guessHeader.Width = 79;
            // 
            // hintCountHeader
            // 
            this.hintCountHeader.Text = "Hint Count";
            this.hintCountHeader.Width = 71;
            // 
            // exitCountHeader
            // 
            this.exitCountHeader.Text = "Exit Count";
            // 
            // heartbeatCountHeader
            // 
            this.heartbeatCountHeader.Text = "Heartbear Count";
            this.heartbeatCountHeader.Width = 93;
            // 
            // lastMessageHeader
            // 
            this.lastMessageHeader.Text = "Last Messsage";
            this.lastMessageHeader.Width = 152;
            // 
            // highScoreHeader
            // 
            this.highScoreHeader.Text = "High Score";
            this.highScoreHeader.Width = 80;
            // 
            // startStopButton
            // 
            this.startStopButton.Location = new System.Drawing.Point(766, 12);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(75, 23);
            this.startStopButton.TabIndex = 16;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.startStop_Click);
            // 
            // serverEndPointLabel
            // 
            this.serverEndPointLabel.AutoSize = true;
            this.serverEndPointLabel.Location = new System.Drawing.Point(35, 27);
            this.serverEndPointLabel.Name = "serverEndPointLabel";
            this.serverEndPointLabel.Size = new System.Drawing.Size(90, 13);
            this.serverEndPointLabel.TabIndex = 17;
            this.serverEndPointLabel.Text = "Server End Point:";
            // 
            // serverEndPointError
            // 
            this.serverEndPointError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverEndPointError.ForeColor = System.Drawing.Color.Red;
            this.serverEndPointError.Location = new System.Drawing.Point(40, 47);
            this.serverEndPointError.Name = "serverEndPointError";
            this.serverEndPointError.Size = new System.Drawing.Size(247, 43);
            this.serverEndPointError.TabIndex = 19;
            this.serverEndPointError.Visible = false;
            // 
            // serverEndPoint
            // 
            this.serverEndPoint.FormattingEnabled = true;
            this.serverEndPoint.Location = new System.Drawing.Point(136, 24);
            this.serverEndPoint.Name = "serverEndPoint";
            this.serverEndPoint.Size = new System.Drawing.Size(151, 21);
            this.serverEndPoint.TabIndex = 20;
            this.serverEndPoint.SelectedIndexChanged += new System.EventHandler(this.serverEndPoint_SelectedIndexChanged);
            this.serverEndPoint.Leave += new System.EventHandler(this.serverEndPoint_Leave);
            // 
            // MonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 502);
            this.Controls.Add(this.serverEndPoint);
            this.Controls.Add(this.serverEndPointError);
            this.Controls.Add(this.serverEndPointLabel);
            this.Controls.Add(this.startStopButton);
            this.Controls.Add(this.playersListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.playerCount);
            this.Controls.Add(this.gameCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Name = "MonitorForm";
            this.Text = "Word Guess Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitorForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label gameCount;
        private System.Windows.Forms.Label playerCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView playersListView;
        private System.Windows.Forms.ColumnHeader aliasHeader;
        private System.Windows.Forms.ColumnHeader gameCountHeader;
        private System.Windows.Forms.ColumnHeader guessHeader;
        private System.Windows.Forms.ColumnHeader hintCountHeader;
        private System.Windows.Forms.ColumnHeader exitCountHeader;
        private System.Windows.Forms.ColumnHeader heartbeatCountHeader;
        private System.Windows.Forms.ColumnHeader lastMessageHeader;
        private System.Windows.Forms.ColumnHeader highScoreHeader;
        private System.Windows.Forms.ColumnHeader idHeader;
        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.Label serverEndPointLabel;
        private System.Windows.Forms.Label serverEndPointError;
        private System.Windows.Forms.ComboBox serverEndPoint;
    }
}

