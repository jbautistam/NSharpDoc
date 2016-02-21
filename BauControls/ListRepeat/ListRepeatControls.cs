using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.ListRepeat
{ 
	/// <summary>
	///		Control para mostrar una lista de contoles
	/// </summary>
	public partial class ListRepeatControls : UserControl 
	{	// Delegados públicos
			public delegate void ControlSelectedHandler(Control ctlControl);
			public delegate void ControlDeselectedHandler(Control ctlControl);
		// Eventos públicos		
			public event ControlSelectedHandler ControlSelected;
			public event ControlDeselectedHandler ControlDeselected;
		// Variables privadas
			private colListControls objColControls;
			private int intUpdating = 0, intSpacing = 10;
			private Control ctlCurrentSelected = null;

		public ListRepeatControls() 
		{	// Inicializa el componente
				InitializeComponent();
			// Crea la colección de controles
				objColControls = new colListControls(this);
		}

		/// <summary>
		///		Añade un control
		/// </summary>
		public void Add(Control ctlControl) 
		{	Add(0, ctlControl);
		}

		/// <summary>
		///		Añade un control
		/// </summary>
		public void Add(int intPaddingLeft, Control ctlControl) 
		{	// Añade el control al panel
				objColControls.Add(intPaddingLeft, ctlControl);
			// Modifica la barra de scroll
				UpdateScrollBar();
			// Asocia el evento de selección al control
				AttachClickEvent(ctlControl);
			// Dibuja los controles
				PaintControls();
		}

		/// <summary>
		///		Elimina un control
		/// </summary>
		public void RemoveAt(int intIndex)
		{	Remove(objColControls[intIndex].Control);
		}

		/// <summary>
		///		Elimina un control
		/// </summary>
		public void Remove(Control ctlControl) 
		{	// Desasocia el evento de selección
				DetachClickEvent(ctlControl);
			// Elimina el control
				objColControls.Remove(ctlControl);
			// Dibuja los controles
				PaintControls();
		}

		/// <summary>
		///		Limpia la lista
		/// </summary>
		public void Clear() 
		{	// Desasocia todos los eventos de selección
				DetachAllClickEvents();
			// Limpia la lista de controles
				pnlContainer.Controls.Clear();
				objColControls.Clear();
			// Dibuja los controles
				PaintControls();
		}
		
		/// <summary>
		///		Modifica los parámetros de la barra de scroll
		/// </summary>
		private void UpdateScrollBar()
		{ scbVertical.Maximum = objColControls.TotalHeight();
		}
		
		/// <summary>
		///		Asocia el evento de selección
		/// </summary>
		private void AttachClickEvent(Control ctlControl) 
		{	// Asocia el evento al control principal
				ctlControl.Click += new EventHandler(Control_Click);
			// Asocia el evento recursivamente a los controles hijo
				if (ctlControl.Controls != null) 
					foreach (Control ctlChild in ctlControl.Controls) 
						AttachClickEvent(ctlChild);
		}

		/// <summary>
		///		Desasocia el evento de selección
		/// </summary>
		private void DetachClickEvent(Control ctlControl) 
		{	ctlControl.Click -= new EventHandler(Control_Click);
			if (ctlControl.Controls != null) 
				foreach (Control ctlChild in ctlControl.Controls) 
						DetachClickEvent(ctlChild);
		}

		/// <summary>
		///		Desasocia el evento de selección de todos los controles
		/// </summary>
		private void DetachAllClickEvents() 
		{	foreach (clsListControl objControl in objColControls)
				DetachClickEvent(objControl.Control);
		}
		
		/// <summary>
		///		Indica que ha comenzado una modificación sobre la lista de controles
		/// </summary>
		public void BeginUpdate() 
		{	intUpdating++;
		}

		/// <summary>
		///		Indica que ha finalizado una modificación sobre la lista de controles
		/// </summary>
		public void EndUpdate() 
		{	// Si no quedan modificaciones pendientes, regenera el control
				if (--intUpdating == 0)
					PaintControls();
		}

		/// <summary>
		///		Sobrescribe el evento OnPaintBackground. Evita los parpadeos
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs e)
		{ // ... no hace nada. Evita los parpadeos			
		}
		
		/// <summary>
		///		Muestra los controles
		/// </summary>
		private void PaintControls()
		{ // Suspende el repintado del panel
				pnlContainer.SuspendLayout();
			// Cambia el scroll
				scbVertical.LargeChange = pnlContainer.Height;
				scbVertical.SmallChange = 100;
			// Quita los controles
				pnlContainer.Controls.Clear();
			// Si hay algún control a dibujar ...
				if (objColControls.Count > 0)
					{	int intControl = GetControlAt(scbVertical.Value);
					
							if (intControl >= 0)
								{	int intTop = objColControls[intControl].Top - scbVertical.Value;
								
										// Añade los controles mientras quepan
											while (intTop < pnlContainer.Height && intControl < objColControls.Count)
												{ Control ctlControl = objColControls[intControl].Control;
												
														// Cambia la posición
															ctlControl.Left = pnlContainer.Margin.Left + objColControls[intControl].PaddingLeft;
															ctlControl.Top = intTop;
															ctlControl.Width = pnlContainer.Width - ctlControl.Left;
														// Añade el control al panel
															pnlContainer.Controls.Add(ctlControl);
															ctlControl.BringToFront();
															ctlControl.Visible = true;
														// Incrementa la posición donde se va a dibujar el siguiente control y el índice
															intTop += ctlControl.Height + Spacing;
															intControl++;
												}
									}
					}
			// Comienza de nuevo el repintado del panel
				pnlContainer.ResumeLayout();
		}
		
		/// <summary>
		///		Obtiene el primer control que se encuentra en una posición
		/// </summary>
		private int GetControlAt(int intTop)
		{ // Busca entre los controles el que se encuentra en esta posición
				for (int intIndex = 0; intIndex < objColControls.Count; intIndex++)
					if (objColControls[intIndex].Top <= intTop && 
							objColControls[intIndex].Top + objColControls[intIndex].Control.Height >= intTop)
						return intIndex;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
				return -1;
		}
		
		/// <summary>
		///		Selecciona un control
		/// </summary>
		public void SelectControl(Control ctlControl) 
		{	if (intUpdating == 0)
				{ // Lanza el evento de deselección del control
						if (ControlDeselected != null)
							ControlDeselected(ctlCurrentSelected);
					// Coloca el control en pantalla y lanza el evento de selección
						if (ctlControl != null)
							{ // Busca el control base
									while (!pnlContainer.Controls.Contains(ctlControl))
										ctlControl = ctlControl.Parent;
								// Lanza el evento de selección
									if (ControlSelected != null)
										ControlSelected(ctlControl);
							}
					// Guarda el control seleccionado
						ctlCurrentSelected = ctlControl;
				}
		}
		
		private void Control_Click(object sender, EventArgs e) 
		{	Control ctlControl = sender as Control;
			
				SelectControl(ctlControl);
		}

		[Description("Espacio de separación entre controles"), DefaultValue(10)]
		public int Spacing
		{ get { return intSpacing; }
			set { intSpacing = value; }
		}
		
		public Control CurrentSelected 
		{	get { return ctlCurrentSelected; }
			set { SelectControl( value ); }
		}

		public List<Control> ChildControls
		{ get
				{ List<Control> objColItems = new List<Control>();
				
						// Añade los controles
							foreach (clsListControl objControl in objColControls)
								objColItems.Add(objControl.Control);
						// Devuelve la colección
							return objColItems;
				}
		}
		
		public new System.Windows.Forms.BorderStyle BorderStyle
		{ get { return pnlContainer.BorderStyle; }
			set { pnlContainer.BorderStyle = value; }
		}
		
		private void scbVertical_Scroll(object sender, ScrollEventArgs e)
		{ PaintControls();
		}

		private void ListBoxControls_SizeChanged(object sender, EventArgs e)
		{ PaintControls();
		}
	}
}
