﻿// Project by Bauss
namespace CandySql_LoginSample.Windows
{
	partial class RegisterScreen
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
			this.userNameLabel = new System.Windows.Forms.Label();
			this.userNamePanel = new System.Windows.Forms.Panel();
			this.userNameTextBox = new System.Windows.Forms.TextBox();
			this.passwordPanel = new System.Windows.Forms.Panel();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.passwordLabel = new System.Windows.Forms.Label();
			this.registerButton = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.userNamePanel.SuspendLayout();
			this.passwordPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// userNameLabel
			// 
			this.userNameLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.userNameLabel.Location = new System.Drawing.Point(6, 6);
			this.userNameLabel.Name = "userNameLabel";
			this.userNameLabel.Size = new System.Drawing.Size(63, 21);
			this.userNameLabel.TabIndex = 0;
			this.userNameLabel.Text = "User Name:";
			this.userNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// userNamePanel
			// 
			this.userNamePanel.Controls.Add(this.userNameTextBox);
			this.userNamePanel.Controls.Add(this.userNameLabel);
			this.userNamePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.userNamePanel.Location = new System.Drawing.Point(0, 0);
			this.userNamePanel.Name = "userNamePanel";
			this.userNamePanel.Padding = new System.Windows.Forms.Padding(6);
			this.userNamePanel.Size = new System.Drawing.Size(384, 33);
			this.userNamePanel.TabIndex = 1;
			// 
			// userNameTextBox
			// 
			this.userNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userNameTextBox.Location = new System.Drawing.Point(69, 6);
			this.userNameTextBox.Name = "userNameTextBox";
			this.userNameTextBox.Size = new System.Drawing.Size(309, 20);
			this.userNameTextBox.TabIndex = 1;
			// 
			// passwordPanel
			// 
			this.passwordPanel.Controls.Add(this.passwordTextBox);
			this.passwordPanel.Controls.Add(this.passwordLabel);
			this.passwordPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.passwordPanel.Location = new System.Drawing.Point(0, 33);
			this.passwordPanel.Name = "passwordPanel";
			this.passwordPanel.Padding = new System.Windows.Forms.Padding(6);
			this.passwordPanel.Size = new System.Drawing.Size(384, 33);
			this.passwordPanel.TabIndex = 2;
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.passwordTextBox.Location = new System.Drawing.Point(69, 6);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.PasswordChar = '•';
			this.passwordTextBox.Size = new System.Drawing.Size(309, 20);
			this.passwordTextBox.TabIndex = 1;
			// 
			// passwordLabel
			// 
			this.passwordLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.passwordLabel.Location = new System.Drawing.Point(6, 6);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(63, 21);
			this.passwordLabel.TabIndex = 0;
			this.passwordLabel.Text = "Password:";
			this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// registerButton
			// 
			this.registerButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.registerButton.Location = new System.Drawing.Point(6, 6);
			this.registerButton.Name = "registerButton";
			this.registerButton.Size = new System.Drawing.Size(372, 23);
			this.registerButton.TabIndex = 3;
			this.registerButton.Text = "Register";
			this.registerButton.UseVisualStyleBackColor = true;
			this.registerButton.Click += new System.EventHandler(this.RegisterButtonClick);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.registerButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 66);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(6);
			this.panel1.Size = new System.Drawing.Size(384, 33);
			this.panel1.TabIndex = 4;
			// 
			// RegisterScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.passwordPanel);
			this.Controls.Add(this.userNamePanel);
			this.Name = "RegisterScreen";
			this.Size = new System.Drawing.Size(384, 261);
			this.userNamePanel.ResumeLayout(false);
			this.userNamePanel.PerformLayout();
			this.passwordPanel.ResumeLayout(false);
			this.passwordPanel.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button registerButton;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.Panel passwordPanel;
		private System.Windows.Forms.TextBox userNameTextBox;
		private System.Windows.Forms.Panel userNamePanel;
		private System.Windows.Forms.Label userNameLabel;
	}
}
