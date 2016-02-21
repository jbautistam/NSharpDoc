namespace Bau.Controls.ListRepeat
{
  partial class ListRepeatControls 
  {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose( bool disposing ) {
          if( disposing && ( components != null ) ) {
              components.Dispose();
          }
          base.Dispose( disposing );
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
				this.pnlContainer = new System.Windows.Forms.Panel();
				this.scbVertical = new System.Windows.Forms.VScrollBar();
				this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
				this.tableLayoutPanel1.SuspendLayout();
				this.SuspendLayout();
				// 
				// pnlContainer
				// 
				this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
				this.pnlContainer.Location = new System.Drawing.Point(0, 0);
				this.pnlContainer.Margin = new System.Windows.Forms.Padding(0);
				this.pnlContainer.Name = "pnlContainer";
				this.pnlContainer.Size = new System.Drawing.Size(487, 494);
				this.pnlContainer.TabIndex = 0;
				// 
				// scbVertical
				// 
				this.scbVertical.Dock = System.Windows.Forms.DockStyle.Fill;
				this.scbVertical.Location = new System.Drawing.Point(487, 0);
				this.scbVertical.Name = "scbVertical";
				this.scbVertical.Size = new System.Drawing.Size(17, 494);
				this.scbVertical.TabIndex = 1;
				this.scbVertical.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scbVertical_Scroll);
				// 
				// tableLayoutPanel1
				// 
				this.tableLayoutPanel1.ColumnCount = 2;
				this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
				this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
				this.tableLayoutPanel1.Controls.Add(this.scbVertical, 1, 0);
				this.tableLayoutPanel1.Controls.Add(this.pnlContainer, 0, 0);
				this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
				this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
				this.tableLayoutPanel1.Name = "tableLayoutPanel1";
				this.tableLayoutPanel1.RowCount = 1;
				this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
				this.tableLayoutPanel1.Size = new System.Drawing.Size(504, 494);
				this.tableLayoutPanel1.TabIndex = 2;
				// 
				// ListRepeatControls
				// 
				this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				this.Controls.Add(this.tableLayoutPanel1);
				this.Name = "ListRepeatControls";
				this.Size = new System.Drawing.Size(504, 494);
				this.SizeChanged += new System.EventHandler(this.ListBoxControls_SizeChanged);
				this.tableLayoutPanel1.ResumeLayout(false);
				this.ResumeLayout(false);

      }

      #endregion

		private System.Windows.Forms.Panel pnlContainer;
		private System.Windows.Forms.VScrollBar scbVertical;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;


		}
}
