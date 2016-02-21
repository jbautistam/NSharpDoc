using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.PopupWindow
{
  /// <summary>
  ///		Clase con un formulario sin borde utilizado para mostrar el popup
  /// </summary>
  internal class clsPopupForm : Form
  { // Constantes
			private const int cnstIntBorderMagin = 1;
			private const int cnstIntCSDropShadow = 0x20000;
		// Delegados
			public delegate void DropDownHandler(object objSender, EventArgs objEvent);
			public delegate void DropDownClosedHandler(object objSender, EventArgs objEvent);
		// Eventos
			public event DropDownHandler OnDropDown;
			public event DropDownClosedHandler OnDropDownClosed;
		// Variables públicas
      private bool blnShowShadow;
     // Variables privadas
      private bool blnClosing;
      private Timer trmTimer;
      private Size sizControl, sizWindow;
      private Point pntNormalPos;
      private Rectangle rctCurrentBounds;
      private PopUpWindow wndParent;
      private DateTime dtmTimerStarted;
      private double dblProgress;

		public clsPopupForm(PopUpWindow wndParent)
		{	Form frmParent = wndParent.Parent.FindForm();
			PopUpWindow.PlacementType intPlacement;

				// Asigna las propiedades al formulario		
					this.wndParent = wndParent;
					SetStyle(ControlStyles.ResizeRedraw, true);
					FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
					StartPosition = FormStartPosition.Manual;
					ShowInTaskbar = false;
					DockPadding.All = cnstIntBorderMagin;
					sizControl = wndParent.UserControl.Size;
					wndParent.UserControl.Dock = DockStyle.Fill;
					Controls.Add(wndParent.UserControl);
					sizWindow.Width = sizControl.Width + 2 * cnstIntBorderMagin;
					sizWindow.Height = sizControl.Height + 2 * cnstIntBorderMagin;
					if (frmParent != null)
						frmParent.AddOwnedForm(this);
					intPlacement = wndParent.Placement;
				// Try to place the popup at the asked location
					ReLocate();
				// Check if the form is out of the screen and if yes try to adapt the placement
					Rectangle rctWorkingArea = Screen.FromControl(wndParent.Parent).WorkingArea;
					if (pntNormalPos.X + this.Width > rctWorkingArea.Right)
						{	if ((intPlacement & PopUpWindow.PlacementType.Right) != 0)
								intPlacement = (intPlacement & ~PopUpWindow.PlacementType.Right) | PopUpWindow.PlacementType.Left;
						}
					else if (pntNormalPos.X < rctWorkingArea.Left)
						{	if ((intPlacement & PopUpWindow.PlacementType.Left) != 0)
								intPlacement = (intPlacement & ~PopUpWindow.PlacementType.Left) | PopUpWindow.PlacementType.Right;
						}
					if (pntNormalPos.Y + this.Height > rctWorkingArea.Bottom)
						{ if ((intPlacement & PopUpWindow.PlacementType.Bottom) != 0)
								intPlacement = (intPlacement & ~PopUpWindow.PlacementType.Bottom) | PopUpWindow.PlacementType.Top;
						}
					else if (pntNormalPos.Y < rctWorkingArea.Top)
						{ if ((intPlacement & PopUpWindow.PlacementType.Top) != 0)
								intPlacement = (intPlacement & ~PopUpWindow.PlacementType.Top) | PopUpWindow.PlacementType.Bottom;
						}
					if (intPlacement != wndParent.Placement)
						ReLocate();
				// Check if the form is still out of the screen
				// if (yes just move it back into the screen without changing Placement
					if (pntNormalPos.X + sizWindow.Width > rctWorkingArea.Right)
						pntNormalPos.X = rctWorkingArea.Right - sizWindow.Width;
					else if (pntNormalPos.X < rctWorkingArea.Left)
						pntNormalPos.X = rctWorkingArea.Left;
					if (pntNormalPos.Y + sizWindow.Height > rctWorkingArea.Bottom)
						pntNormalPos.Y = rctWorkingArea.Bottom - sizWindow.Height;
					else if (pntNormalPos.Y < rctWorkingArea.Top)
						pntNormalPos.Y = rctWorkingArea.Top;
				// Initialize the animation
					dblProgress = 0;
					if (wndParent.AnimationSpeed > 0)
						{	trmTimer = new Timer();
							// I always aim 25 images per seconds.. seems to be a good value
							// it looks smooth enough on fast computers and do not drain slower one
							trmTimer.Interval = 1000 / 25;
							dtmTimerStarted = DateTime.Now;
							trmTimer.Tick += new EventHandler(Showing);
							trmTimer.Start();
							Showing(null, null);
						}
					else
						SetFinalLocation();
					Show();
					if (OnDropDown != null)
						OnDropDown(wndParent.Parent, new EventArgs());
    }

    public bool DropShadowSupported()
    { return Environment.OSVersion.Platform == PlatformID.Win32NT && 
						 Environment.OSVersion.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0;
    }

		protected override System.Windows.Forms.CreateParams CreateParams
		{ get
				{ CreateParams objParameters = base.CreateParams;
				
            if (blnShowShadow && DropShadowSupported())
							objParameters.ClassStyle = objParameters.ClassStyle | cnstIntCSDropShadow;
            return objParameters;
        }
    }

		protected override void Dispose(bool disposing)
		{	if (disposing && trmTimer != null)
				trmTimer.Dispose();
			base.Dispose(disposing);
		}
		
    private void ReLocate()
    { int rW = sizWindow.Width, rH = sizWindow.Height;

        pntNormalPos = wndParent.Parent.PointToScreen(new Point());
        switch (wndParent.Placement)
          { case PopUpWindow.PlacementType.Top:
						case PopUpWindow.PlacementType.TopLeft:
						case PopUpWindow.PlacementType.TopRight:
                pntNormalPos.Y -= rH;
							break;
            case PopUpWindow.PlacementType.Bottom:
            case PopUpWindow.PlacementType.BottomLeft:
            case PopUpWindow.PlacementType.BottomRight:
								pntNormalPos.Y += wndParent.Parent.Height;
							break;
						case PopUpWindow.PlacementType.Left: 
						case PopUpWindow.PlacementType.Right:
                pntNormalPos.Y += (wndParent.Parent.Height - rH) / 2;
              break;
					}
        switch (wndParent.Placement)
					{	case PopUpWindow.PlacementType.Left:
                pntNormalPos.X -= rW;
              break;
            case PopUpWindow.PlacementType.Right:
                pntNormalPos.X += wndParent.Parent.Width;
							break;
            case PopUpWindow.PlacementType.TopLeft:
            case PopUpWindow.PlacementType.BottomLeft:
                pntNormalPos.X += wndParent.Parent.Width - rW;
							break;
            case PopUpWindow.PlacementType.Top:
            case PopUpWindow.PlacementType.Bottom:
                pntNormalPos.X += (wndParent.Parent.Width - rW) / 2;
							break;
					}
    }

    private void Showing(object sender, EventArgs e)
    {	dblProgress = DateTime.Now.Subtract(dtmTimerStarted).TotalMilliseconds / wndParent.AnimationSpeed;
			if (dblProgress >= 1)
				{	trmTimer.Stop();
          AnimateForm(1);
        }
      else
				AnimateForm(dblProgress);
    }

		protected override void OnDeactivate(EventArgs e)
		{	base.OnDeactivate(e);
      if (!blnClosing)
				{	if (wndParent.UserControl is IPopupUserControl)
						blnClosing = ((IPopupUserControl) wndParent.UserControl).AcceptPopupClosing();
          else
						blnClosing = true;
          if (blnClosing) 
						DoClose();
				}
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {	e.Graphics.DrawRectangle(new Pen(wndParent.BorderColor), 0, 0, this.Width - 1, this.Height - 1);
    }

    private void SetFinalLocation()
    {	dblProgress = 1;
      AnimateForm(1);
      Invalidate();
    }

    private void AnimateForm(double dblProgress)
    { double x = 0, y = 0, w = 0, h = 0;
    
        if (dblProgress <= 0.1) 
					dblProgress = 0.1;
        switch (wndParent.Placement)
					{	case PopUpWindow.PlacementType.Top:
						case PopUpWindow.PlacementType.TopLeft:
						case PopUpWindow.PlacementType.TopRight:
                y = 1 - dblProgress;
                h = dblProgress;
							break;
            case PopUpWindow.PlacementType.Bottom:
            case PopUpWindow.PlacementType.BottomLeft:
            case PopUpWindow.PlacementType.BottomRight:
                y = 0;
                h = dblProgress;
              break;
            case PopUpWindow.PlacementType.Left:
            case PopUpWindow.PlacementType.Right:
                y = 0;
                h = 1;
              break;
					}
        switch (wndParent.Placement)
					{	case PopUpWindow.PlacementType.TopRight:
						case PopUpWindow.PlacementType.BottomRight:
						case PopUpWindow.PlacementType.Right:
                x = 0;
                w = dblProgress;
              break;
            case PopUpWindow.PlacementType.TopLeft:
            case PopUpWindow.PlacementType.BottomLeft:
            case PopUpWindow.PlacementType.Left:
                x = 1 - dblProgress;
                w = dblProgress;
              break;
            case PopUpWindow.PlacementType.Top:
            case PopUpWindow.PlacementType.Bottom:
                x = 0;
                w = 1;
              break;
          }
        rctCurrentBounds.X = pntNormalPos.X + (int) (x * sizControl.Width);
        rctCurrentBounds.Y = pntNormalPos.Y + (int) (y * sizControl.Height);
        rctCurrentBounds.Width = (int) (w * sizControl.Width) + 2 * cnstIntBorderMagin;
        rctCurrentBounds.Height = (int) (h * sizControl.Height) + 2 * cnstIntBorderMagin;
        this.Bounds = rctCurrentBounds;
    }

    public void DoClose()
    {	wndParent.UserControl.Parent = null;
      wndParent.UserControl.Size = sizControl;
      Form frmParent = wndParent.Parent.FindForm();
      if (frmParent != null)
				frmParent.RemoveOwnedForm(this);
      frmParent.Focus();
      if (OnDropDownClosed != null)
				OnDropDownClosed(this, new EventArgs());
      Close();
    }

    protected override void OnLoad(System.EventArgs e)
    {	base.OnLoad(e);
      // for some reason setbounds do not work well in the constructor
      Bounds = rctCurrentBounds;
    }
    
    public bool ShowShadow
    { get { return blnShowShadow; }
			set { blnShowShadow = value; }
    }
  }
}
