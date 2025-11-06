namespace TextEffects.Forms
{
    partial class AboutForm
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
            this.paypay = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.contentLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // contentLabel
            // 
            this.contentLabel.Location = new System.Drawing.Point(12, 9);
            this.contentLabel.Name = "contentLabel";
            this.contentLabel.Size = new System.Drawing.Size(329, 134);
            this.contentLabel.TabIndex = 4;

            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(175, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Donate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Donate_Click);

            // 
            // paypay
            // 
            this.paypay.Location = new System.Drawing.Point(269, 160);
            this.paypay.Name = "paypay";
            this.paypay.Size = new System.Drawing.Size(75, 23);
            this.paypay.TabIndex = 1;
            this.paypay.Text = "Close";
            this.paypay.UseVisualStyleBackColor = true;
            this.paypay.Click += new System.EventHandler(this.Close_Click);

            // 
            // CustomAboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 190);
            this.ControlBox = false;
            this.Controls.Add(this.contentLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.paypay);
            this.Name = "CustomAboutBox";
            this.ShowIcon = false;
            this.Text = "About TextEffects";
            this.Load += new System.EventHandler(this.CustomAboutBox_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button paypay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label contentLabel;
    }
}