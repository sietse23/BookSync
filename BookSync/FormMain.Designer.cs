namespace BookSync
{
    partial class FormMain
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
            this.ofdDialog = new System.Windows.Forms.OpenFileDialog();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnOpenDBOud = new System.Windows.Forms.Button();
            this.btnOpenDBNieuw = new System.Windows.Forms.Button();
            this.lblVoortgang = new System.Windows.Forms.Label();
            this.lblVoortgangOms = new System.Windows.Forms.Label();
            this.pbVoortgang = new System.Windows.Forms.ProgressBar();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblDriveStatus = new System.Windows.Forms.Label();
            this.lblDriveStatusOms = new System.Windows.Forms.Label();
            this.lblPlacesFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdDialog
            // 
            this.ofdDialog.Filter = "Places|places.sqlite";
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnOpenDBOud);
            this.pnlTop.Controls.Add(this.btnOpenDBNieuw);
            this.pnlTop.Controls.Add(this.lblVoortgang);
            this.pnlTop.Controls.Add(this.lblVoortgangOms);
            this.pnlTop.Controls.Add(this.pbVoortgang);
            this.pnlTop.Controls.Add(this.btnUpload);
            this.pnlTop.Controls.Add(this.btnDownload);
            this.pnlTop.Controls.Add(this.btnConnect);
            this.pnlTop.Controls.Add(this.lblDriveStatus);
            this.pnlTop.Controls.Add(this.lblDriveStatusOms);
            this.pnlTop.Controls.Add(this.lblPlacesFile);
            this.pnlTop.Controls.Add(this.txtFile);
            this.pnlTop.Controls.Add(this.btnOpenFile);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(877, 218);
            this.pnlTop.TabIndex = 34;
            // 
            // btnOpenDBOud
            // 
            this.btnOpenDBOud.Location = new System.Drawing.Point(483, 80);
            this.btnOpenDBOud.Name = "btnOpenDBOud";
            this.btnOpenDBOud.Size = new System.Drawing.Size(129, 23);
            this.btnOpenDBOud.TabIndex = 46;
            this.btnOpenDBOud.Text = "OpenDB Oud";
            this.btnOpenDBOud.UseVisualStyleBackColor = true;
            this.btnOpenDBOud.Click += new System.EventHandler(this.btnOpenDBOud_Click);
            // 
            // btnOpenDBNieuw
            // 
            this.btnOpenDBNieuw.Location = new System.Drawing.Point(483, 109);
            this.btnOpenDBNieuw.Name = "btnOpenDBNieuw";
            this.btnOpenDBNieuw.Size = new System.Drawing.Size(129, 23);
            this.btnOpenDBNieuw.TabIndex = 45;
            this.btnOpenDBNieuw.Text = "OpenDB Nieuw";
            this.btnOpenDBNieuw.UseVisualStyleBackColor = true;
            this.btnOpenDBNieuw.Click += new System.EventHandler(this.btnOpenDBNieuw_Click);
            // 
            // lblVoortgang
            // 
            this.lblVoortgang.AutoSize = true;
            this.lblVoortgang.Location = new System.Drawing.Point(544, 45);
            this.lblVoortgang.Name = "lblVoortgang";
            this.lblVoortgang.Size = new System.Drawing.Size(34, 13);
            this.lblVoortgang.TabIndex = 44;
            this.lblVoortgang.Text = "Begin";
            // 
            // lblVoortgangOms
            // 
            this.lblVoortgangOms.AutoSize = true;
            this.lblVoortgangOms.Location = new System.Drawing.Point(461, 45);
            this.lblVoortgangOms.Name = "lblVoortgangOms";
            this.lblVoortgangOms.Size = new System.Drawing.Size(56, 13);
            this.lblVoortgangOms.TabIndex = 43;
            this.lblVoortgangOms.Text = "Voortgang";
            // 
            // pbVoortgang
            // 
            this.pbVoortgang.Location = new System.Drawing.Point(15, 130);
            this.pbVoortgang.Name = "pbVoortgang";
            this.pbVoortgang.Size = new System.Drawing.Size(288, 23);
            this.pbVoortgang.TabIndex = 42;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(231, 79);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 41;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(117, 79);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 40;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(15, 79);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 39;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblDriveStatus
            // 
            this.lblDriveStatus.AutoSize = true;
            this.lblDriveStatus.Location = new System.Drawing.Point(95, 45);
            this.lblDriveStatus.Name = "lblDriveStatus";
            this.lblDriveStatus.Size = new System.Drawing.Size(37, 13);
            this.lblDriveStatus.TabIndex = 38;
            this.lblDriveStatus.Text = "Status";
            // 
            // lblDriveStatusOms
            // 
            this.lblDriveStatusOms.AutoSize = true;
            this.lblDriveStatusOms.Location = new System.Drawing.Point(12, 45);
            this.lblDriveStatusOms.Name = "lblDriveStatusOms";
            this.lblDriveStatusOms.Size = new System.Drawing.Size(63, 13);
            this.lblDriveStatusOms.TabIndex = 37;
            this.lblDriveStatusOms.Text = "Drive status";
            // 
            // lblPlacesFile
            // 
            this.lblPlacesFile.AutoSize = true;
            this.lblPlacesFile.Location = new System.Drawing.Point(12, 10);
            this.lblPlacesFile.Name = "lblPlacesFile";
            this.lblPlacesFile.Size = new System.Drawing.Size(80, 13);
            this.lblPlacesFile.TabIndex = 36;
            this.lblPlacesFile.Text = "Places bestand";
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(98, 7);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(738, 20);
            this.txtFile.TabIndex = 35;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFile.Location = new System.Drawing.Point(840, 5);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(25, 23);
            this.btnOpenFile.TabIndex = 34;
            this.btnOpenFile.TabStop = false;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.Location = new System.Drawing.Point(0, 285);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(877, 478);
            this.rtbLog.TabIndex = 35;
            this.rtbLog.Text = "";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 762);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.pnlTop);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdDialog;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblPlacesFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Label lblDriveStatus;
        private System.Windows.Forms.Label lblDriveStatusOms;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ProgressBar pbVoortgang;
        private System.Windows.Forms.Label lblVoortgang;
        private System.Windows.Forms.Label lblVoortgangOms;
        private System.Windows.Forms.Button btnOpenDBOud;
        private System.Windows.Forms.Button btnOpenDBNieuw;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}