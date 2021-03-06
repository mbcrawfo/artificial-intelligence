﻿namespace Minesweeper.UI
{
  sealed partial class MainWindow
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
      this.drawingPanel = new System.Windows.Forms.Panel();
      this.SuspendLayout();
      // 
      // drawingPanel
      // 
      this.drawingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.drawingPanel.Location = new System.Drawing.Point(0, 0);
      this.drawingPanel.Name = "drawingPanel";
      this.drawingPanel.Size = new System.Drawing.Size(1008, 730);
      this.drawingPanel.TabIndex = 0;
      // 
      // MainWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1008, 730);
      this.Controls.Add(this.drawingPanel);
      this.MinimumSize = new System.Drawing.Size(1024, 768);
      this.Name = "MainWindow";
      this.Text = "Minesweeper";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel drawingPanel;
  }
}

