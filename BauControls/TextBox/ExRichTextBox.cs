using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Bau.Controls.TextBox
{
	// Colores RTF
		public enum RtfColor 
			{ Black, 
				Maroon, 
				Green, 
				Olive, 
				Navy, 
				Purple, 
				Teal, 
				Gray, 
				Silver,
				Red, 
				Lime, 
				Yellow, 
				Blue, 
				Fuchsia, 
				Aqua, 
				White
		}

	/// <summary>
	///		Extiende el control RichTextBox para que se pueda insertar o añadir texto plano, imágenes o RTF al contenido
	///		de un control RicthTextBox
	/// </summary>
	/// <remarks>
	///		Definición de RTF v1.6
	///			http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnrtfspec/html/rtfspec.asp
	/// 
	///		Información sobre RichEdit (RichTextBox)
	///			http://msdn.microsoft.com/library/default.asp?url=/library/en-us/shellcc/platform/commctls/richedit/richeditcontrols/aboutricheditcontrols.asp
	/// </remarks>
	public class ExRichTextBox : System.Windows.Forms.RichTextBox 
	{	// Constantes privadas
			private const string RTF_HEADER = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033";
			private const string RTF_DOCUMENT_PRE = @"\viewkind4\uc1\pard\cf1\f0\fs20";
			private const string RTF_DOCUMENT_POST = @"\cf0\fs17}";
			private const int MM_ANISOTROPIC = 8; // Permite que se ajusten independientemente las coordenadas x e y de un metafile
			private const string FF_UNKNOWN = "UNKNOWN"; // Representa una familia de fuentes desconocida
			private const int HMM_PER_INCH = 2540; // Centésimas de milímetro en una pulgada
			private const int TWIPS_PER_INCH = 1440; // Número de twips en una pulgada
		// Declaraciones
			[DllImportAttribute("gdiplus.dll")]
			private static extern uint GdipEmfToWmfBits (IntPtr _hEmf, uint _bufferSize,
				byte[] _buffer, int _mappingMode, EmfToWmfBitsFlags _flags);
		// Enumerados
			private enum EmfToWmfBitsFlags // Opciones para la llamadas al método GDI+ Metafile.EmfToWmfBits()
				{	EmfToWmfBitsFlagsDefault = 0x00000000, // Conversión predeterminada
					EmfToWmfBitsFlagsEmbedEmf = 0x00000001, // Incrusta el archivo EMF en el WMF resultante
					EmfToWmfBitsFlagsIncludePlaceable = 0x00000002, // Coloca una cabecera de 22 bytes en el archivo WMF resultante.
					EmfToWmfBitsFlagsNoXORClip = 0x00000004// No simula clipping utilizando el operador XOR
				}
		// Estructuras
			private struct dicColorDef // Colores de un documento RTF
				{	public const string Black = @"\red0\green0\blue0";
					public const string Maroon = @"\red128\green0\blue0";
					public const string Green = @"\red0\green128\blue0";
					public const string Olive = @"\red128\green128\blue0";
					public const string Navy = @"\red0\green0\blue128";
					public const string Purple = @"\red128\green0\blue128";
					public const string Teal = @"\red0\green128\blue128";
					public const string Gray = @"\red128\green128\blue128";
					public const string Silver = @"\red192\green192\blue192";
					public const string Red = @"\red255\green0\blue0";
					public const string Lime = @"\red0\green255\blue0";
					public const string Yellow = @"\red255\green255\blue0";
					public const string Blue = @"\red0\green0\blue255";
					public const string Fuchsia = @"\red255\green0\blue255";
					public const string Aqua = @"\red0\green255\blue255";
					public const string White = @"\red255\green255\blue255";
				}
			private struct dicFontFamilyDef // Familias de fuentes RTF
				{	public const string Unknown = @"\fnil";
					public const string Roman = @"\froman";
					public const string Swiss = @"\fswiss";
					public const string Modern = @"\fmodern";
					public const string Script = @"\fscript";
					public const string Decor = @"\fdecor";
					public const string Technical = @"\ftech";
					public const string BiDirect = @"\fbidi";
				}
		// Variables privadas
			private RtfColor clrText; // Color de texto predeterminado
			private RtfColor clrBack; // Color de fondo predeterminado
			private HybridDictionary dicColor; // Diccionario que relaciona los enums del color a los códigos RTF
			private HybridDictionary dicFontFamily; // Diccionario que relaciona la fuentes del Framework con las fuentes RTF
		// Elementos necearios para crear un documento 
			private string RTF_IMAGE_POST = @"}";
			
		public ExRichTextBox(RtfColor clrText) : this(clrText, RtfColor.White) {}

		public ExRichTextBox() : this(RtfColor.Black, RtfColor.White) {}
		
		public ExRichTextBox(RtfColor clrText, RtfColor clrBack)
		{	// Inicializa los colores de fondo y texto
				this.clrText = clrText;
				this.clrBack = clrBack;
			// Inicializa el diccionario de colores
				dicColor = new HybridDictionary();
				dicColor.Add(RtfColor.Aqua, dicColorDef.Aqua);
				dicColor.Add(RtfColor.Black, dicColorDef.Black);
				dicColor.Add(RtfColor.Blue, dicColorDef.Blue);
				dicColor.Add(RtfColor.Fuchsia, dicColorDef.Fuchsia);
				dicColor.Add(RtfColor.Gray, dicColorDef.Gray);
				dicColor.Add(RtfColor.Green, dicColorDef.Green);
				dicColor.Add(RtfColor.Lime, dicColorDef.Lime);
				dicColor.Add(RtfColor.Maroon, dicColorDef.Maroon);
				dicColor.Add(RtfColor.Navy, dicColorDef.Navy);
				dicColor.Add(RtfColor.Olive, dicColorDef.Olive);
				dicColor.Add(RtfColor.Purple, dicColorDef.Purple);
				dicColor.Add(RtfColor.Red, dicColorDef.Red);
				dicColor.Add(RtfColor.Silver, dicColorDef.Silver);
				dicColor.Add(RtfColor.Teal, dicColorDef.Teal);
				dicColor.Add(RtfColor.White, dicColorDef.White);
				dicColor.Add(RtfColor.Yellow, dicColorDef.Yellow);
			// Inicializa el diccionario de fuentes
				dicFontFamily = new HybridDictionary();
				dicFontFamily.Add(FontFamily.GenericMonospace.Name, dicFontFamilyDef.Modern);
				dicFontFamily.Add(FontFamily.GenericSansSerif, dicFontFamilyDef.Swiss);
				dicFontFamily.Add(FontFamily.GenericSerif, dicFontFamilyDef.Roman);
				dicFontFamily.Add(FF_UNKNOWN, dicFontFamilyDef.Unknown);
		}

		/// <summary>
		///		Añade un texto RTF al contenido del control
		/// </summary>
		public void AppendRtf(string sbRtf) 
		{	// Desplaza el cursor al final del texto
				Select(TextLength, 0);
			// Dato que SelectedRtf es null, esto añadirá la cadena al final del RTF existente
				SelectedRtf = sbRtf;
		}

		/// <summary>
		///		Inserta un texto RTF en la posición del cursor
		/// </summary>
		public void InsertRtf(string sbRtf) 
		{	SelectedRtf = sbRtf;
		}

		/// <summary>
		///		Añade el texto utilizando la fuente actual
		/// </summary>
		public void AppendTextAsRtf(string _text) 
		{	AppendTextAsRtf(_text, Font);
		}

		/// <summary>
		///		Añade el texto utilizando un color
		/// </summary>
		public void AppendTextAsRtf(string _text, RtfColor clrText)
		{ AppendTextAsRtf(_text, Font, clrText, clrBack);
		}
		
		/// <summary>
		///		Añade el texto utilizando la fuente especificada y el color actual
		/// </summary>
		public void AppendTextAsRtf(string _text, Font fntFont) 
		{	AppendTextAsRtf(_text, fntFont, clrText);
		}
		
		/// <summary>
		///		Añade el texto utilizando la fuente y color especificado
		/// </summary>
		public void AppendTextAsRtf(string _text, Font fntFont, RtfColor clrText) 
		{	AppendTextAsRtf(_text, fntFont, clrText, clrBack);
		}

		/// <summary>
		///		Añade el texto utilizando la fuente y el color especificado
		/// </summary>
		public void AppendTextAsRtf(string _text, Font fntFont, RtfColor clrText, RtfColor clrBack) 
		{	// Mueve el cursor al final del texto
				Select(TextLength, 0);
			// Inserta el texto
				InsertTextAsRtf(_text, fntFont, clrText, clrBack);
		}

		/// <summary>
		///		Inserta el texto utilizando la fuente y color actual
		/// </summary>
		public void InsertTextAsRtf(string _text) 
		{	InsertTextAsRtf(_text, Font);
		}


		/// <summary>
		///		Inserta el texto utilizando la fuente especificada
		/// </summary>
		public void InsertTextAsRtf(string _text, Font fntFont) 
		{	InsertTextAsRtf(_text, fntFont, clrText);
		}
		
		/// <summary>
		///		Inserta el texto utilizando la fuente y el color especificados
		/// </summary>
		public void InsertTextAsRtf(string _text, Font fntFont, RtfColor clrText) 
		{	InsertTextAsRtf(_text, fntFont, clrText, clrBack);
		}

		/// <summary>
		///		Inserta el texto utilizando la fuente y el color especificados
		/// </summary>
		public void InsertTextAsRtf(string _text, Font fntFont, RtfColor clrText, RtfColor clrBack) 
		{	StringBuilder sbRtf = new StringBuilder();

			// Añade la cabecera RTF
				sbRtf.Append(RTF_HEADER);
			// Crea la tabla de fuentes a partir de la fuente pasada y la añade a la cadena RTF
				sbRtf.Append(GetFontTable(fntFont));
			// Crea la tabla de color a partir de los colores pasados y la añade a la cadena RTF
				sbRtf.Append(GetColorTable(clrText, clrBack));
			// Crea el área del doucmento a partir del texto a añadir como RTF
				sbRtf.Append(GetDocumentArea(_text, fntFont));
			// Añade el texto a la cadena RTF
				SelectedRtf = sbRtf.ToString();
		}

		/// <summary>
		///		Crea el área de documento del RTF a insertar. El área de documento en este caso
		///		consiste del texto que se añade como RTF y el formato especificado en el objeto Font 
		///		que se ha pasado
		/// </summary>
		private string GetDocumentArea(string _text, Font fntFont) 
		{	StringBuilder _doc = new StringBuilder();
			
			// Añade el área del documento RTF
				_doc.Append(RTF_DOCUMENT_PRE);
			// Modifica el color de rfondo al tercer color de la tabla 
				_doc.Append(@"\highlight2");
			// Si la fuente está en negrita le añade la etiqueta correspondiente
				if (fntFont.Bold)
					_doc.Append(@"\b");
			// Si la fuente está en cursiva le añade la etiqueta correspondiente
				if (fntFont.Italic)
					_doc.Append(@"\i");
			// Si la fuente está tachada le añade la etiqueta correspondiente
				if (fntFont.Strikeout)
					_doc.Append(@"\strike");
			// Si la fuente está subrayada le añade la etiqueta correspondiente
				if (fntFont.Underline)
					_doc.Append(@"\ul");
			// Cambia la fuente a la primera de la tabla de fuentes
				_doc.Append(@"\f0");
			// Cambia el tampado de la fuente. En RTF el tamaño de fuente se mide en medios puntos, por tanto
			// el tamaño de fuente es dos veces el valor obtenido de Font.SizeInPoints
				_doc.Append(@"\fs");
				_doc.Append((int) Math.Round((2 * fntFont.SizeInPoints)));
			// Añade un espacio antes de comenzar el texto real (por claridad)
				_doc.Append(@" ");
			// Añade el texto real reemplazado lo saltos de línea por \par.
			// TODO --> El resto de caracteres especiales se debería tratar aquí
				_doc.Append(_text.Replace("\n", @"\par "));
			// Elimina el color de fondo
				_doc.Append(@"\highlight0");
			// Cierra la etiqueta de negrita
				if (fntFont.Bold)
					_doc.Append(@"\b0");
			// Cierra la etiqueta de cursiva
				if (fntFont.Italic)
					_doc.Append(@"\i0");
			// Cierra la etiqueta de tachado
				if (fntFont.Strikeout)
					_doc.Append(@"\strike0");
			// Cierra la etiqueta de subrayado
				if (fntFont.Underline)
					_doc.Append(@"\ulnone");
			// Vuelve a la fuente y tamaño predeterminado
				_doc.Append(@"\f0");
				_doc.Append(@"\fs20");
			// Cierra la cadena del área del documento
				_doc.Append(RTF_DOCUMENT_POST);
			// Devuelve la cadena
				return _doc.ToString();
		}

		/// <summary>
		///		Inserta una imagen. La imagen está en un Windows Format Metafile en formato hexadecimal
		/// </summary>
		public void InsertImage(Image _image) 
		{	StringBuilder sbRtf = new StringBuilder();

			// Añade la cabecera RTF
				sbRtf.Append(RTF_HEADER);
			// Crea la tabla de fuentes utilizando la fuente actual y la añade a la cadena RTF
				sbRtf.Append(GetFontTable(this.Font));
			// Crea la cadena de control de la imagen y la añade a la cadena RTF
				sbRtf.Append(GetImagePrefix(_image));
			// Crea el Windows Metafile y lo añade en formato hexadecimal
				sbRtf.Append(GetRtfImage(_image));
			// Cierra la cadena de control RTF de la imagen
				sbRtf.Append(RTF_IMAGE_POST);
			// Inserta la cadena en el cuadro de texto
				SelectedRtf = sbRtf.ToString();
		}

		/// <summary>
		/// Crea la cadena RTF de control que describe la imagen a insertar
		/// Define que es un archivo MM_ANISOTROPIC y las dimensiones actuales y objetivo de la imagen
		/// {\pict\wmetafile8\picw[A]\pich[B]\picwgoal[C]\pichgoal[D]
		/// 
		/// Donde ...
		/// 
		/// A	= ancho actual de la imagen en centésimas de milímetro (0.01mm)
		///		= Ancho de la imagen en pulgadas * Número de (0.01mm) por pulgada (25.4 mm)
		///		= (Ancho imagen en pixels / Resolución horizontal) * 2540
		///		= (Ancho imagen en pixels / Graphics.DpiX) * 2540
		/// 
		/// B	= alto actual de la imagen en centésimas de milímetro (0.01mm)
		///		= Alto de la imagen en pulgadas * Número de (0.01mm) por pulgada
		///		= (Alto imagen en pixels / Resolución vertical) * 2540
		///		= (Alto imagen en pixels / Graphics.DpiY) * 2540
		/// 
		/// C	= ancho final de la imagen en twips (1 Twip = 1/1440 pulgadas)
		///		= Ancho de la imagen en pulgadas * Número de twips por pulgada
		///		= (Ancho de la imagen en pixels / Resolución horizontal) * 1440
		///		= (Ancho de la imagen en pixels / Graphics.DpiX) * 1440
		/// 
		/// D	= alto final de la imagen en twips
		///		= Alto de la imagen en pulgadas * Número de twips por pulgada
		///		= (Alto de la imagen en pixels / Resolución vertical) * 1440
		///		= (Alto de la imagen en pixels / Graphics.DpiY) * 1440
		///	
		/// </summary>
		private string GetImagePrefix(Image _image) 
		{	StringBuilder sbRtf = new StringBuilder();
			float fltDpiX,fltDpiY;

				// Obtiene la resolución horizontal y vertical a la que se muestra el control
					using (Graphics grpCanvas = CreateGraphics()) 
						{	fltDpiX = grpCanvas.DpiX;
							fltDpiY = grpCanvas.DpiY;
						}
				// Calcula en ancho actual de la imagen en (0.01)mm
					int picw = (int)Math.Round((_image.Width / fltDpiX) * HMM_PER_INCH);
				// Calcula el alto actual de la imagen en (0.01)mm
					int pich = (int)Math.Round((_image.Height /fltDpiY) * HMM_PER_INCH);
				// Calcula el ancho destino de la imagen en twips
					int picwgoal = (int)Math.Round((_image.Width / fltDpiX) * TWIPS_PER_INCH);
				// Calcula el alto destino de la imagen en twips
					int pichgoal = (int)Math.Round((_image.Height /fltDpiY) * TWIPS_PER_INCH);
				// Añade los valores a la cadena de prefijo
					sbRtf.Append(@"{\pict\wmetafile8");
					sbRtf.Append(@"\picw");
					sbRtf.Append(picw);
					sbRtf.Append(@"\pich");
					sbRtf.Append(pich);
					sbRtf.Append(@"\picwgoal");
					sbRtf.Append(picwgoal);
					sbRtf.Append(@"\pichgoal");
					sbRtf.Append(pichgoal);
					sbRtf.Append(" ");
				// Devuelve la cadena
					return sbRtf.ToString();
		}

		/// <summary>
		///		Convierte la imagen en un Enhanced Metafile desde un Windows Metafile y devuelve los bits
		///		del Windows Metafile en hexadecimal
		/// </summary>
		private string GetRtfImage(Image _image) 
		{	StringBuilder sbRtf = null;
			MemoryStream stmStream = null; // Almacena el enhanced metafile
			Graphics grpGraphics = null; // Utilizado para crear el metafile y dibujar la imagen
			Metafile mfMetafile = null; // El enhanced metafile
			IntPtr hndDC; // Manejador al contexto del dispositivo utilizado para crear el metafile

				try 
					{	// Inicializa las variables
							sbRtf = new StringBuilder();
							stmStream = new MemoryStream();
						// Obtiene el contexto gráfico del RichTextBox
							using (grpGraphics = CreateGraphics()) 
								{	// Obtiene el contexto de dispositivo a partir del contexto gráfico
										hndDC = grpGraphics.GetHdc();
									// Crea un nuevo Enhanced Metafile desde el contexto de dispositivo
										mfMetafile = new Metafile(stmStream, hndDC);
									// Libera el contexto del dispositivo
										grpGraphics.ReleaseHdc(hndDC);
								}
						// Obtiene el contexto gráfico del Enhanced Metafile
							using (grpGraphics = Graphics.FromImage(mfMetafile)) 
								{	// Dibuja la imagen sobre el Enhanced Metafile
										grpGraphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height));
								}
						// Obtiene el manejador del Enhanced Metafile
							IntPtr _hEmf = mfMetafile.GetHenhmetafile();
						// Llama a EmfToWmfBits con un buffer nulo para obtener el tamaño necesario para almacenar
						// los bits del WMF
							uint _bufferSize = GdipEmfToWmfBits(_hEmf, 0, null, MM_ANISOTROPIC,	
																									EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
						// Crea un array para mantener los bits
							byte[] _buffer = new byte[_bufferSize];
						// Llama a EmfToWmfBits con un buffer correcto para copiar los en el buffer 
						// y devolver el número de bits del WMF
							uint _convertedSize = GdipEmfToWmfBits(_hEmf, _bufferSize, _buffer, MM_ANISOTROPIC, 
																										EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
						// Añade los bits a la cadena RTF en hexadecimal
							for (int i = 0; i < _buffer.Length; i++) 
								sbRtf.Append(String.Format("{0:X2}", _buffer[i]));
						// Devuelve la cadena con la imagen
							return sbRtf.ToString();
					}
				finally 
					{	if(mfMetafile != null)
							mfMetafile.Dispose();
						if(stmStream != null)
							stmStream.Close();
					}
		}

		/// <summary>
		///		Crea la cadena RTF que define la tabla de fuentes de RTF
		/// {\fonttbl{\f0\[FAMILY]\fcharset0 [FONT_NAME];}
		/// </summary>
		private string GetFontTable(Font fntFont) 
		{	StringBuilder sbFontTable = new StringBuilder();

			// Añade la cadena de control
				sbFontTable.Append(@"{\fonttbl{\f0");
				sbFontTable.Append(@"\");
			// Si la familia de la fuente corresponde a una familia RTF, añade el nombre de la familia RTF
			// si no, añade la constante RTF que define una fuente desconocida
				if (dicFontFamily.Contains(fntFont.FontFamily.Name))
					sbFontTable.Append(dicFontFamily[fntFont.FontFamily.Name]);
				else
					sbFontTable.Append(dicFontFamily[FF_UNKNOWN]);
			// \fcharset define el conjunto de carateres de una fuente. Se utiliza 0 para ANSI
				sbFontTable.Append(@"\fcharset0 ");
			// Añade el nombre de la fuente
				sbFontTable.Append(fntFont.Name);
			// Cierra la cadena de control
				sbFontTable.Append(@";}}");
			// Devuelve la cadena con la tabla
				return sbFontTable.ToString();	
		}

		/// <summary>
		///		Crea la cadena RTF que define la tabla de colores de RTF
		/// {\colortbl ;[TEXT_COLOR];[HIGHLIGHT_COLOR];}
		/// </summary>
		private string GetColorTable(RtfColor clrText, RtfColor clrBack) 
		{	StringBuilder sbColorTable = new StringBuilder();

			// Añade la cadena de la tabla de control
				sbColorTable.Append(@"{\colortbl ;");
			// Añade el color del texto
				sbColorTable.Append(dicColor[clrText]);
				sbColorTable.Append(@";");
			// Añade el color del fondo
				sbColorTable.Append(dicColor[clrBack]);
				sbColorTable.Append(@";}\n");
			// Devuelve la cadena con la tabla
				return sbColorTable.ToString();
		}
		
		public RtfColor TextColor
		{	get { return clrText; }
			set { clrText = value; }
		}

		public RtfColor HighlightColor
		{ get { return clrBack; }
			set { clrBack = value; }
		}
	}
}
