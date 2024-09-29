namespace WindowsFormsApp
{
    partial class Greeting
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
            this.lblGreeating = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblGreeating
            // 
            this.lblGreeating.AutoSize = true;
            this.lblGreeating.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.lblGreeating.Location = new System.Drawing.Point(10, 29);
            this.lblGreeating.Name = "lblGreeating";
            this.lblGreeating.Size = new System.Drawing.Size(23, 25);
            this.lblGreeating.TabIndex = 0;
            this.lblGreeating.Text = "g";
            // 
            // Greeting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(42, 97);
            this.Controls.Add(this.lblGreeating);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Greeting";
            this.Text = "Greeting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGreeating;
    }
}