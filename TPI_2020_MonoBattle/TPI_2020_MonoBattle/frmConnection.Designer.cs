namespace TPI_2020_MonoBattle
{
    partial class frmConnection
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
            if (disposing && ( components != null ))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnection));
            this.btnSendRequest = new System.Windows.Forms.Button();
            this.tbxIpAddressOpponent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConnectLocal = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxSelfIp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Location = new System.Drawing.Point(14, 50);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(161, 21);
            this.btnSendRequest.TabIndex = 1;
            this.btnSendRequest.Text = "Ask for a game";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.BtnSendRequest_Click);
            // 
            // tbxIpAddressOpponent
            // 
            this.tbxIpAddressOpponent.Location = new System.Drawing.Point(14, 24);
            this.tbxIpAddressOpponent.Name = "tbxIpAddressOpponent";
            this.tbxIpAddressOpponent.Size = new System.Drawing.Size(161, 20);
            this.tbxIpAddressOpponent.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ip adress of your opponent :";
            // 
            // btnConnectLocal
            // 
            this.btnConnectLocal.Location = new System.Drawing.Point(181, 50);
            this.btnConnectLocal.Name = "btnConnectLocal";
            this.btnConnectLocal.Size = new System.Drawing.Size(161, 21);
            this.btnConnectLocal.TabIndex = 2;
            this.btnConnectLocal.Text = "Solo mode";
            this.btnConnectLocal.UseVisualStyleBackColor = true;
            this.btnConnectLocal.Click += new System.EventHandler(this.btnConnectLocal_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Your IP adress :";
            // 
            // tbxSelfIp
            // 
            this.tbxSelfIp.Location = new System.Drawing.Point(181, 24);
            this.tbxSelfIp.Name = "tbxSelfIp";
            this.tbxSelfIp.ReadOnly = true;
            this.tbxSelfIp.Size = new System.Drawing.Size(161, 20);
            this.tbxSelfIp.TabIndex = 4;
            // 
            // frmConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 83);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxSelfIp);
            this.Controls.Add(this.btnConnectLocal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxIpAddressOpponent);
            this.Controls.Add(this.btnSendRequest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmConnection";
            this.Text = "MonoBattle";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendRequest;
        private System.Windows.Forms.TextBox tbxIpAddressOpponent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConnectLocal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxSelfIp;
    }
}