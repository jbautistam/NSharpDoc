using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Microsoft.Win32;

namespace Bau.Controls.ToolTipExtend
{
	/// <summary>
	/// 	Control ToolTip extendido
	/// </summary>
	[ ToolboxBitmapAttribute(typeof(ToolTipExtend),"ToolTipExtend.bmp"),
		ProvideProperty("Title", typeof(Control)),
	 	ProvideProperty("Icon", typeof(Control)),
	 	ProvideProperty("Text", typeof(Control)),
	 	ProvideProperty("Image", typeof(Control)),
	 	ProvideProperty("Footer", typeof(Control))]
	public class ToolTipExtend : System.Windows.Forms.ToolTip, IExtenderProvider
	{ // Constantes privadas
			private int  cnstIntWidthSeparator = 10;
		// Enumerados
			/// <summary>
			///		Tipo de sección
			/// </summary>
			private enum enumSection : int
				{ SectionTitle = 0,
					SectionIcon,
					SectionText,
					SectionImage,
					SectionFoot
				}
   
	    /// <summary>
	    /// 	Clase que almacena los datos de una sección
	    /// </summary>
	  	private class clsSection
	  	{ private Hashtable htControls = new Hashtable();
	  		private Font fntFont;
	  		private Point pntPosition = new Point(0, 0);
	  		
	  		public clsSection(bool blnBold)
	  		{ if (blnBold)
	  				fntFont = new Font(SystemFonts.DialogFont, FontStyle.Bold);
	  			else
	  				fntFont = new Font(SystemFonts.DialogFont, FontStyle.Regular);
	  		}
	  		
	  		public Hashtable Controls
	  		{ get { return htControls; }
	  			set { htControls = value; }
	  		}
	  		
	  		public Font FontText
	  		{ get { return fntFont; }
	  			set { fntFont = value; }
	  		}
	  		
	  		public Point Position
	  		{ get { return pntPosition; }
	  			set { pntPosition = value; }
	  		}
	  	}

	  // Variables privadas
	  	private clsSection [] arrObjSection = { new clsSection(true), new clsSection(false), new clsSection(false),
	  																					new clsSection(false), new clsSection(true)};
	  	private Size szToolTip;
	    private int intPositionLine;
	    private bool blnOwnerDraw;
		
		public ToolTipExtend()
		{ IsToolTipExtend = true;
			base.Draw += new DrawToolTipEventHandler(ToolTipExtend_Draw);
			base.Popup += new PopupEventHandler(ToolTipExtend_Popup);
		}

		/// <summary>
		/// 	Obtiene la cadena asociada a una de las propiedades extendidas del toolTip
		/// </summary>
		private string GetStringControl(Control ctlControl, enumSection intSection)
		{ if (arrObjSection[(int) intSection].Controls.Contains(ctlControl))
    		return arrObjSection[(int) intSection].Controls[ctlControl].ToString();
	    else
	    	return "";
		}
		
		/// <summary>
		/// 	Modifica la cadena asociada a una de las propiedades extendidas del toolTip
		/// </summary>
		private void SetStringControl(Control ctlControl, enumSection intSection, string strText)
		{ // Inicializa la cadena
				if (strText == null)
		    	strText = "";
			// Añade o elimina la cadena del control
		  	if (string.IsNullOrEmpty(strText))
		    	arrObjSection[(int) intSection].Controls.Remove(ctlControl);
				else if (arrObjSection[(int) intSection].Controls.Contains(ctlControl))
					arrObjSection[(int) intSection].Controls[ctlControl] = strText;
		    else
		    	arrObjSection[(int) intSection].Controls.Add(ctlControl, strText);
		}

		/// <summary>
		/// 	Obtiene la imagen asociada a una de las propiedades extendidas del toolTip
		/// </summary>
    private Image GetImageControl(Control ctlControl, enumSection intSection)
	  {	if (arrObjSection[(int) intSection].Controls.Contains(ctlControl))
    		return (Image) arrObjSection[(int) intSection].Controls[ctlControl];
	    else
		    return null;
	  }

    /// <summary>
    /// 	Asigna la imagen asociada a una de las propiedades extendidas del toolTip
    /// </summary>
    private void SetImageControl(Control ctlControl, enumSection intSection, Image imgImage)
	  {	if (imgImage == null)
				arrObjSection[(int) intSection].Controls.Remove(ctlControl);
	  	else if (arrObjSection[(int) intSection].Controls.Contains(ctlControl))
	  		arrObjSection[(int) intSection].Controls[ctlControl] = imgImage;
			else
			  arrObjSection[(int) intSection].Controls.Add(ctlControl, imgImage);
	  }

/*
		[ Description("Determina el texto descriptivo que tendrá el ToolTipExtend"),
	   	DefaultValue(""), 
			Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    public string GetText(Control ctlControl)
    {	return GetStringControl(ctlControl, enumSection.SectionText);
	  }

    public void SetText(Control ctlControl, string strText)
	  {	// Modifica la cadena asociada a la propiedad Text del control asociado al toolTip
    		SetStringControl(ctlControl, enumSection.SectionText, strText);
	    // Si la propiedad extensora ToolTip no tiene nada, se agrega el mismo texto
	    // de la propiedad extensora ToolTipExtendText, ya que la propiedad ToolTip 
	    // es la que indica que el control tiene asociado un ToolTip
				if (string.IsNullOrEmpty(GetToolTip(ctlControl)) && strText != ctlControl.Name)
				  SetToolTip(ctlControl, strText);
	  }
 */
	
	  [	Description("Determina el titulo que tendrá el ToolTipExtend"),
	   	DefaultValue("")]
    public string GetTitle(Control ctlControl)
    {	return GetStringControl(ctlControl, enumSection.SectionTitle);
	  }

    public void SetTitle(Control ctlControl, string strTitle)
    {	SetStringControl(ctlControl, enumSection.SectionTitle, strTitle);
	  }

		[ Description("Determina el icono que tendrá el ToolTipExtend en la parte superior derecha"),
   		DefaultValue(typeof(Object), "")]
    public Image GetIcon(Control ctlControl)
    {	return GetImageControl(ctlControl, enumSection.SectionIcon);
	  }

    public void SetIcon(Control ctlControl, Image imgIcon)
    {	SetImageControl(ctlControl, enumSection.SectionIcon, imgIcon);
	  }
  
	  [	Description("Determina la imagen que tendrá el ToolTipExtend"),
	   	DefaultValue(typeof(Object), "")]
    public Image GetImage(Control ctlControl)
    {	return GetImageControl(ctlControl, enumSection.SectionImage);
	  }

    public void SetImage(Control ctlControl, Image imgImage)
	  {	SetImageControl(ctlControl, enumSection.SectionImage, imgImage);
	  }

	  [	Description("Determina el pie que tendrá el ToolTipExtend"),
	   	DefaultValue("")]
    public string GetFooter(Control ctlControl)
    {	return GetStringControl(ctlControl, enumSection.SectionFoot);
	  }

    public void SetFooter(Control ctlControl, string strFoot)
    {	SetStringControl(ctlControl, enumSection.SectionFoot, strFoot);
	  }
  
    /// <summary>
    /// 	Obtiene el tema de Windows XP
    /// </summary>
    private string GetTheme()
	  {	RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\ThemeManager");

    		// Devuelve el valor de la clave si ha encontrado alguna
	  			if (key != null && (string) key.GetValue("ThemeActive") == "1")
		  			return (string) key.GetValue("ColorName");
	  		// Si no ha encontrado ningún valor devuelve una cadena vacía
	    		return "";
	  }

    /// <summary>
    /// 	Comprueba si un control está asociado al toolTip
    /// </summary>
    private bool IsAssignedControl(Control ctlControl)
    { return arrObjSection[(int) enumSection.SectionTitle].Controls.Contains(ctlControl) ||
    				 arrObjSection[(int) enumSection.SectionIcon].Controls.Contains(ctlControl) ||
    				 arrObjSection[(int) enumSection.SectionText].Controls.Contains(ctlControl) ||
    				 arrObjSection[(int) enumSection.SectionImage].Controls.Contains(ctlControl) ||
    				 arrObjSection[(int) enumSection.SectionFoot].Controls.Contains(ctlControl);
    }
    
    /// <summary>
    ///		Trata el evento de dibujo del toolTip
    /// </summary>
    private void ToolTipExtend_Draw(Object sender, DrawToolTipEventArgs e)
    {	if (IsToolTipExtend && IsAssignedControl(e.AssociatedControl))
    		{ // Dibujar fondo con un gradiente de acuerdo al tema de Windows XP
						Color color1 = SystemColors.Info, color2 = SystemColors.Info;
		
					// Identificar que tema se esta usando
	      		switch (GetTheme())
		      		{	case "Metallic":
										color1 = Color.FromArgb(249, 249, 255);
										color2 = Color.FromArgb(164, 163, 190);
			        		break;
							 case "NormalColor":
										color1 = Color.FromArgb(227, 239, 255);
										color2 = Color.FromArgb(121, 161, 220);
			        		break;
							 case "HomeStead":
										color1 = Color.FromArgb(250, 251, 230);
										color2 = Color.FromArgb(164, 180, 120);
			        		break;
		      		}
					// Pintar el color de fondo
						e.Graphics.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(0, szToolTip.Height), 
	        																									 color1, color2), 
	        													 new Rectangle(0, 0, szToolTip.Width, szToolTip.Height));
					// Dibujar borde con apariencia 3D
						e.Graphics.DrawLines(SystemPens.ControlLightLight, 
																 new Point[] { new Point(0, szToolTip.Height - 1), new Point(0, 0),
	        																		 new Point(szToolTip.Width - 1, 0)});
						e.Graphics.DrawLines(SystemPens.ControlDarkDark, new Point[] {new Point(0, szToolTip.Height - 1),
	        																																new Point(szToolTip.Width - 1, szToolTip.Height - 1),
	        																																new Point(szToolTip.Width - 1, 0)});
					// Imprimir titulo, si existe
						if (!string.IsNullOrEmpty(GetTitle(e.AssociatedControl)))
							e.Graphics.DrawString(GetTitle(e.AssociatedControl), arrObjSection[(int) enumSection.SectionTitle].FontText, 
	      														new SolidBrush(Color.Black), arrObjSection[(int) enumSection.SectionTitle].Position);
					// Dibujar icono en la esquina superior derecha, si existe
						Bitmap icono = (Bitmap) GetIcon(e.AssociatedControl);
						if (icono != null)
							{	icono.MakeTransparent(Color.Magenta);
								e.Graphics.DrawImage(icono, arrObjSection[(int) enumSection.SectionIcon].Position);
							}
					// Dibujar imagen descriptiva, si existe
					// Si es una imagen ancha se mostrará horizontalmente y el texto descriptivo estará debajo de la imagen
					// Si es una imagen alta se mostrará verticalmente y el texto descriptivo estará en la parte derecha de la imagen
						Image imagen = GetImage(e.AssociatedControl);
						if (imagen != null)
							e.Graphics.DrawImage(imagen, new Rectangle(arrObjSection[(int) enumSection.SectionImage].Position.X, arrObjSection[(int) enumSection.SectionImage].Position.Y, imagen.Width, imagen.Height));
					// Imprimir texto descriptivo
					// El texto se imprime de acuerdo a si existe o no una imagen descriptiva
					// La orientación del texto depende de donde se coloque la imagen descriptiva
						e.Graphics.DrawString(GetToolTip(e.AssociatedControl), arrObjSection[(int) enumSection.SectionText].FontText, new SolidBrush(Color.Black), 
		      												arrObjSection[(int) enumSection.SectionText].Position);
					// Dibujar línea de separación e imprimir texto de pie de ToolTipExtend, si existe
						if (!string.IsNullOrEmpty(GetFooter(e.AssociatedControl)))
							{	e.Graphics.DrawLine(new Pen(Color.Black), new Point(1, intPositionLine), 
			      												new Point(szToolTip.Width - 1, intPositionLine));
								e.Graphics.DrawString(GetFooter(e.AssociatedControl), arrObjSection[(int) enumSection.SectionFoot].FontText, 
																			new SolidBrush(Color.Black), arrObjSection[(int) enumSection.SectionFoot].Position);
							}
    		}
	    else
				{ e.DrawBackground();
					e.DrawBorder();
					e.DrawText(TextFormatFlags.NoFullWidthCharacterBreak | TextFormatFlags.VerticalCenter);
				}
    }

		/// <summary>
		///		Trata el evento PopUp del toolTip
		/// </summary>
    private void ToolTipExtend_Popup(Object sender, System.Windows.Forms.PopupEventArgs e) // Handles Me.Popup
    {	// Si OwnerDraw es verdadero y el texto de la propiedad ToolTipExtendText no esta vacía
    	if (IsToolTipExtend && OwnerDraw && IsAssignedControl(e.AssociatedControl))
    	{	bool hayTitulo = false, hayIcono = false, hayTexto = false, hayImagen = false, hayPie = false;

      // Calcular el tamaño del ToolTipExtend
      // Obtener el tamaño individual de los elementos
      Size tamTitulo = new Size(0, 0), tamTexto = new Size(0, 0), tamPie = new Size(0, 0);
      Size tamImagen = new Size(0, 0), tamIcono = new Size(0, 0);
      int tamTabSeccion2 = cnstIntWidthSeparator;

      if (!string.IsNullOrEmpty(GetTitle(e.AssociatedControl)))
	      {	tamTitulo = TextRenderer.MeasureText(GetTitle(e.AssociatedControl), arrObjSection[(int) enumSection.SectionTitle].FontText);
	        hayTitulo = true;
	      }

      Image icono;
      icono = GetIcon(e.AssociatedControl);
      if (icono != null)
	      {	tamIcono = icono.Size;
	        hayIcono = true;
	      }
      tamTexto = TextRenderer.MeasureText(GetToolTip(e.AssociatedControl), 
																					arrObjSection[(int) enumSection.SectionText].FontText);
      hayTexto = true;

      Image imagen;
      imagen = GetImage(e.AssociatedControl);
      if (imagen != null)
	      {	tamImagen = imagen.Size;
	        hayImagen = true;
	    	}
    	if (!string.IsNullOrEmpty(GetFooter(e.AssociatedControl)))
	    	{	tamPie = TextRenderer.MeasureText(GetFooter(e.AssociatedControl), arrObjSection[(int) enumSection.SectionFoot].FontText);
	        hayPie = true;
	    	}
    	if (hayTitulo || hayPie)
        tamTabSeccion2 = cnstIntWidthSeparator + cnstIntWidthSeparator;

      // Calcular altura del ToolTipExtend
				szToolTip.Height = cnstIntWidthSeparator;
      // Calcular altura del ToolTipExtend - Sección 1
				if (hayTitulo || hayIcono)
					{	if (tamTitulo.Height >= tamIcono.Height)
	        		szToolTip.Height += tamTitulo.Height + cnstIntWidthSeparator;
						else
	        		szToolTip.Height += tamIcono.Height + cnstIntWidthSeparator;
					}
      // Calcular altura del ToolTipExtend - Sección 2
      if (hayTexto && hayImagen)
	      {	// Identificar la orientación de la imagen
	      	if (tamImagen.Height >= tamImagen.Width) // Orientación vertical
		        {	if (tamImagen.Height >= tamTexto.Height)
			          szToolTip.Height += tamImagen.Height + cnstIntWidthSeparator;
			        else
			          szToolTip.Height += tamTexto.Height + cnstIntWidthSeparator;
		        }
	        else	// Orientación horizontal
		        szToolTip.Height += tamImagen.Height + cnstIntWidthSeparator + tamTexto.Height + cnstIntWidthSeparator;
	      }
      else if (hayTexto)
        szToolTip.Height += tamTexto.Height + cnstIntWidthSeparator;
      else
        szToolTip.Height += tamImagen.Height + cnstIntWidthSeparator;

      // Calcular altura del ToolTipExtend - Sección 3
				if (hayPie)
					szToolTip.Height += tamPie.Height + cnstIntWidthSeparator;
      // Calcular ancho del ToolTipExtend
	      int anchoSeccion1 = 0, anchoSeccion2 = 0, anchoSeccion3 = 0;
      // Calcular ancho del ToolTipExtend - Sección 1
				if (hayTitulo && hayIcono)
					anchoSeccion1 = cnstIntWidthSeparator + tamTitulo.Width + tamIcono.Width + cnstIntWidthSeparator;
				else if (hayTitulo)
					anchoSeccion1 = cnstIntWidthSeparator + tamTitulo.Width + cnstIntWidthSeparator;
				else
					anchoSeccion1 = cnstIntWidthSeparator + tamIcono.Width + cnstIntWidthSeparator;
      // Calcular ancho del ToolTipExtend - Sección 2
				if (hayTexto && hayImagen)
					{	// Identificar la orientación de la imagen
	      		if (tamImagen.Height >= tamImagen.Width)	// Orientación vertical
	        		anchoSeccion2 = tamTabSeccion2 + tamImagen.Width + cnstIntWidthSeparator + tamTexto.Width + cnstIntWidthSeparator;
						else	// Orientación horizontal
							{	if (tamImagen.Width >= tamTexto.Width)
									anchoSeccion2 = tamTabSeccion2 + tamImagen.Width + cnstIntWidthSeparator;
								else
									anchoSeccion2 = tamTabSeccion2 + tamTexto.Width + cnstIntWidthSeparator;
							}
					}
				else if (hayTexto)
					anchoSeccion2 = tamTabSeccion2 + tamTexto.Width + cnstIntWidthSeparator;
				else
					anchoSeccion2 = tamTabSeccion2 + tamImagen.Width + cnstIntWidthSeparator;
      // Calcular ancho del ToolTipExtend - Seccion 3
				if (hayPie)
					anchoSeccion3 = cnstIntWidthSeparator + tamPie.Width + cnstIntWidthSeparator;
      // El ancho del ToolTipExtend depende de la sección mas ancha
				if (anchoSeccion1 >= anchoSeccion2 && anchoSeccion1 >= anchoSeccion3)
					szToolTip.Width = anchoSeccion1;
				else if (anchoSeccion2 >= anchoSeccion1 && anchoSeccion2 >= anchoSeccion3)
					szToolTip.Width = anchoSeccion2;
				else
					szToolTip.Width = anchoSeccion3;
      // Calcular las posiciones de los elementos del ToolTipExtend
      Point inicioSeccion1 = new Point(cnstIntWidthSeparator, cnstIntWidthSeparator);
      Point inicioSeccion2 = new Point(tamTabSeccion2, cnstIntWidthSeparator);
      Point inicioSeccion3;

      // Calcular las posiciones de los elementos - Sección 1
      if (hayTitulo || hayIcono)
	      {	if (tamTitulo.Height >= tamIcono.Height)
		        {	arrObjSection[(int) enumSection.SectionTitle].Position = inicioSeccion1;
			        arrObjSection[(int) enumSection.SectionIcon].Position = new Point(szToolTip.Width - cnstIntWidthSeparator - tamIcono.Width, arrObjSection[(int) enumSection.SectionTitle].Position.Y + tamTitulo.Height / 2 - tamIcono.Height / 2);
			        inicioSeccion2 = new Point(tamTabSeccion2, arrObjSection[(int) enumSection.SectionTitle].Position.Y + tamTitulo.Height + cnstIntWidthSeparator);
		        }
	        else
		        { arrObjSection[(int) enumSection.SectionIcon].Position = new Point(szToolTip.Width - cnstIntWidthSeparator - tamIcono.Width, inicioSeccion1.Y);
			        arrObjSection[(int) enumSection.SectionTitle].Position = new Point(inicioSeccion1.X, arrObjSection[(int) enumSection.SectionIcon].Position.Y + tamIcono.Height / 2 - tamTitulo.Height / 2);
			        inicioSeccion2 = new Point(tamTabSeccion2, arrObjSection[(int) enumSection.SectionIcon].Position.Y + tamIcono.Height + cnstIntWidthSeparator);
		        }
	      }

      // Calcular las posiciones de los elementos - Sección 2
      if (hayTexto && hayImagen)
	      {	if (tamImagen.Height >= tamImagen.Width) // Orientación vertical
		      	{	arrObjSection[(int) enumSection.SectionImage].Position = inicioSeccion2;
			        arrObjSection[(int) enumSection.SectionText].Position = new Point(arrObjSection[(int) enumSection.SectionImage].Position.X + tamImagen.Width + cnstIntWidthSeparator, arrObjSection[(int) enumSection.SectionImage].Position.Y);
			      	if (tamImagen.Height >= tamTexto.Height)
			          inicioSeccion3 = new Point(cnstIntWidthSeparator, arrObjSection[(int) enumSection.SectionImage].Position.Y + tamImagen.Height + cnstIntWidthSeparator);
			        else
			          inicioSeccion3 = new Point(cnstIntWidthSeparator, arrObjSection[(int) enumSection.SectionText].Position.Y + tamTexto.Height + cnstIntWidthSeparator);
		      	}
	        else	// Orientación horizontal
		        {	if (tamImagen.Width >= tamTexto.Width)
			      		{ arrObjSection[(int) enumSection.SectionImage].Position = inicioSeccion2;
				          arrObjSection[(int) enumSection.SectionText].Position = new Point(arrObjSection[(int) enumSection.SectionImage].Position.X, arrObjSection[(int) enumSection.SectionImage].Position.Y + tamImagen.Height + cnstIntWidthSeparator);
			      		}
			        else
				      	{	arrObjSection[(int) enumSection.SectionText].Position = new Point(inicioSeccion2.X, inicioSeccion2.Y + tamImagen.Height + cnstIntWidthSeparator);
				          arrObjSection[(int) enumSection.SectionImage].Position = new Point(arrObjSection[(int) enumSection.SectionText].Position.X + tamTexto.Width / 2 - tamImagen.Width / 2, inicioSeccion2.Y);
				      	}
			        inicioSeccion3 = new Point(cnstIntWidthSeparator, arrObjSection[(int) enumSection.SectionText].Position.Y + tamTexto.Height + cnstIntWidthSeparator);
		      	}
	      }
      else if (hayTexto)
        {	arrObjSection[(int) enumSection.SectionText].Position = inicioSeccion2;
        	inicioSeccion3 = new Point(cnstIntWidthSeparator, arrObjSection[(int) enumSection.SectionText].Position.Y + tamTexto.Height + cnstIntWidthSeparator);
        }
      else
	      {	arrObjSection[(int) enumSection.SectionImage].Position = inicioSeccion2;
	        inicioSeccion3 = new Point(cnstIntWidthSeparator, arrObjSection[(int) enumSection.SectionImage].Position.Y + tamImagen.Height + cnstIntWidthSeparator);
	      }

      // Calcular las posiciones de los elementos - Sección 3
      if (hayPie)
				{	intPositionLine = inicioSeccion3.Y - cnstIntWidthSeparator / 2;
					inicioSeccion3.Y += 1;
					arrObjSection[(int) enumSection.SectionFoot].Position = inicioSeccion3;
				}
      e.ToolTipSize = szToolTip;
    }
    else if (IsToolTipExtend && !IsAssignedControl(e.AssociatedControl))
      e.Cancel = true;
    }

		/// <summary>
		///		Indica si el ToolTip tomará la forma de un ToolTipExtend
		/// </summary>
	  [	Description("Indica si el ToolTip tomará la forma de un ToolTipExtend"),
	  	DefaultValue(true)]
    public bool IsToolTipExtend
	  { get { return blnOwnerDraw; }
	  	set {	OwnerDraw = blnOwnerDraw = value; }
	  }
	}
}