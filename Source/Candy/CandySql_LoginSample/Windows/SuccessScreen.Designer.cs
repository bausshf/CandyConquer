// Project by Bauss
namespace CandySql_LoginSample.Windows
{
	partial class SuccessScreen
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.loginLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// loginLabel
			// 
			this.loginLabel.AutoSize = true;
			this.loginLabel.Location = new System.Drawing.Point(4, 4);
			this.loginLabel.Name = "loginLabel";
			this.loginLabel.Size = new System.Drawing.Size(183, 13);
			this.loginLabel.TabIndex = 0;
			this.loginLabel.Text = "Welcome {0}. Your last login was {1}.";
			this.loginLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SuccessScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.loginLabel);
			this.Name = "SuccessScreen";
			this.Size = new System.Drawing.Size(364, 252);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label loginLabel;
	}
}
