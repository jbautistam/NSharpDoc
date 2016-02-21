using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.PopupWindow
{
	/// <summary>
	///		Componente para mostrar una ventana popup
	/// </summary>
	public class PopUpWindow : Component
	{ // Enumerados públicos
			public enum PlacementType
				{	Left = 1,
					Right = 2,
					Top = 4,
					Bottom = 8,
					TopLeft = Top | Left,
					TopRight = Top | Right,
					BottomLeft = Bottom | Left,
					BottomRight = Bottom | Right
				 }
		// Delegados
			public delegate void DropDownHandler(object objSender, EventArgs e);
			public delegate void DropDownClosedHandler(object objSender, EventArgs e);
		// Eventos
			public event DropDownHandler DropDown;
			public event DropDownClosedHandler DropDownClosed;
     // Variables privadas
			private bool blnResizable = false;
			private Control ctlUserControl;
			private Control ctlParent;
			private PlacementType intPlacement = PlacementType.BottomLeft;
			private Color clrBorderColor = Color.DarkGray;
			private int intAnimationSpeed = 150;
			private bool blnShowShadow = true;
			private clsPopupForm wndPopUp;
	
		public PopUpWindow() : this(null, null)
		{
		}
		
		public PopUpWindow(Control ctlUserControl, Control ctlParent)
		{	this.ctlParent = ctlParent;
      this.ctlUserControl = ctlUserControl;
    }

		public void Show()
		{ // Cierra la ventana hija si no estaba cerrada
				if (wndPopUp != null)
					wndPopUp.DoClose();
			// Muestra la ventana hija
				wndPopUp = new clsPopupForm(this);
				wndPopUp.ShowShadow = ShowShadow;
			// Asigna el manejador de eventos
				wndPopUp.OnDropDownClosed += new clsPopupForm.DropDownClosedHandler(wndPopUp_DropDownClosed);
			// Lanza el evento
				OnDropDown(ctlParent, new EventArgs());
    }
    
    public void Hide()
    {	if (wndPopUp != null)
				{ wndPopUp.DoClose();
					wndPopUp = null;
				}
		}

    protected void OnDropDown(object Sender, EventArgs e)
    {	if (DropDown != null)
				DropDown(Sender, e);
    }

    protected void OnDropDownClosed(object Sender, EventArgs e)
    { if (DropDownClosed != null)
        DropDownClosed(Sender, e);
    }

    [DefaultValue(false)]
    public bool Resizable
    { get { return blnResizable; }
			set { blnResizable = value; }
		}

    [Browsable(false)]
    public Control UserControl
    {	get { return ctlUserControl; }
			set { ctlUserControl = value; }
		}

    [Browsable(false)]
    public Control Parent
    { get { return ctlParent; }
			set { ctlParent = value; }
		}

    public PlacementType Placement
    {	get { return intPlacement; }
      set { intPlacement = value; }
    }

    public Color BorderColor
    { get { return clrBorderColor; }
			set { clrBorderColor = value; }
		}

    [DefaultValue(true)]
    public bool ShowShadow
		{ get { return blnShowShadow; }
			set { blnShowShadow = value; }
		}

    [DefaultValue(150)]
    public int AnimationSpeed
    { get { return intAnimationSpeed; }
			set { intAnimationSpeed = value; }
		}

		/// <summary>
		///		Evento al que se llama cuando se cierra una ventana hija
		/// </summary>
		private void wndPopUp_DropDownClosed(object objSender, EventArgs objEvent)
		{ wndPopUp = null;
			if (DropDownClosed != null)
				DropDownClosed(this, new EventArgs());
		}
	}
}
