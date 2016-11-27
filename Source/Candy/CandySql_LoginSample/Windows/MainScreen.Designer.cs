// Project by Bauss
namespace CandySql_LoginSample.Windows
{
	partial class MainScreen
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
			this.loginButton = new System.Windows.Forms.Button();
			this.registerButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// loginButton
			// 
			this.loginButton.Location = new System.Drawing.Point(3, 3);
			this.loginButton.Name = "loginButton";
			this.loginButton.Size = new System.Drawing.Size(75, 23);
			this.loginButton.TabIndex = 0;
			this.loginButton.Text = "Login";
			this.loginButton.UseVisualStyleBackColor = true;
			this.loginButton.Click += new System.EventHandler(this.LoginButtonClick);
			// 
			// registerButton
			// 
			this.registerButton.Location = new System.Drawing.Point(3, 32);
			this.registerButton.Name = "registerButton";
			this.registerButton.Size = new System.Drawing.Size(75, 23);
			this.registerButton.TabIndex = 1;
			this.registerButton.Text = "Register";
			this.registerButton.UseVisualStyleBackColor = true;
			this.registerButton.Click += new System.EventHandler(this.RegisterButtonClick);
			// 
			// MainScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.registerButton);
			this.Controls.Add(this.loginButton);
			this.Name = "MainScreen";
			this.Size = new System.Drawing.Size(389, 273);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button registerButton;
		private System.Windows.Forms.Button loginButton;
	}
}
