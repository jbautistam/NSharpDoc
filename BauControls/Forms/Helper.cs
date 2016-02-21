using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Bau.Controls.Forms
{
	/// <summary>
	///		Clase de uso general para el manejo de mensajes
	/// </summary>
	public class Helper
	{ // Delegados 
			private delegate void ShowMessageCallback(Form frmWindow, string strMessage); // Permite llamadas asíncronas para mostrar un mensaje
			private delegate void ShowErrorCallback(Form frmWindow, string strMessage, Exception objException); // Permite llamadas asíncronas para mostrar un mensaje
			
		/// <summary>
		///		Muestra un cuadro de mensaje
		/// </summary>
		public static void ShowMessage(Form frmWindow, string strMessage)
		{ // InvokeRequired compara el ID del hilo llamante con el ID del hilo creador, si son diferentes devuelve True
				if (frmWindow != null && frmWindow.InvokeRequired)
					{	ShowMessageCallback fncCallback = new ShowMessageCallback(ShowMessage);
					
							frmWindow.Invoke(fncCallback, new object[] { frmWindow, strMessage });
					}
				else
					MessageBox.Show(frmWindow, strMessage, Application.ProductName);
		}

		/// <summary>
		/// 	Muestra un mensaje de error
		/// </summary>
		public static void ShowError(Form frmWindow, string strMessage, Exception objException)
		{ if (objException == null)
				ShowMessage(frmWindow, strMessage);
			else
				ShowMessage(frmWindow, strMessage + "\n" + objException.Message);
		}
		
		/// <summary>
		///		Muestra un cuadro de mensaje con la respuesta Sí / No
		/// </summary>
		public static bool ShowQuestion(Form frmWindow, string strMessage)
		{ return MessageBox.Show(frmWindow, strMessage, Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes;
		}
		
		/// <summary>
		///		Muestra un cuadro de mensaje con la respuesta Sí / No / Cancelar
		/// </summary>
		public static DialogResult ShowQuestionCancel(Form frmWindow, string strMessage)
		{ return MessageBox.Show(frmWindow, strMessage, Application.ProductName, MessageBoxButtons.YesNoCancel);
		}

		/// <summary>
		///		Muestra un cuadro de mensaje para introducir un texto
		/// </summary>
		public static string ShowInputMessage(string strTitle, string strMessage, string strInputValue)
		{ frmMessage frmNewMessage = new frmMessage();
		
				// Pasa los valores al formulario
					frmNewMessage.Text = strTitle;
					frmNewMessage.Message = strMessage;
					frmNewMessage.InputText = strInputValue;
				// Muestra el formulario
					frmNewMessage.ShowDialog();
				// Devuelve el valor
					if (frmNewMessage.DialogResult == DialogResult.OK)
						return frmNewMessage.InputText;
					else
						return null;
		}
		
		/// <summary>
		/// 	Obtiene el nombre de un fichero a partir de un cuadro de diálogo de apertura de archivos
		/// </summary>		
		public static string GetFileNameOpen(string strFilter)
		{	return GetFileNameOpen(null, strFilter, false);
		}

		/// <summary>
		/// 	Obtiene el nombre de un fichero a partir de un cuadro de diálogo de apertura de archivos
		/// </summary>		
		public static string GetFileNameOpen(string strFilter, bool blnMultiple)
		{	return GetFileNameOpen(null, strFilter, blnMultiple);
		}

		/// <summary>
		/// 	Obtiene el nombre de un fichero a partir de un cuadro de diálogo de apertura de archivos
		/// </summary>		
		public static string GetFileNameOpen(string strPath, string strFilter)
		{	return GetFileNameOpen(strPath, strFilter, false);
		}

		/// <summary>
		/// 	Obtiene el nombre de un fichero a partir de un cuadro de diálogo de apertura de archivos
		/// </summary>		
		public static string GetFileNameOpen(string strPath, string strFilter, bool blnMultiple)
		{	OpenFileDialog dlgFile = new OpenFileDialog();
			string strFileName = "";

				// Asigna las propiedades al cuadro de diálogo
					dlgFile.CheckFileExists = true;
					dlgFile.CheckPathExists = true;
					dlgFile.Filter = strFilter;
					dlgFile.Multiselect = blnMultiple;
				// Asigna el directorio inicial
					if (!string.IsNullOrEmpty(strPath))
						dlgFile.InitialDirectory = strPath;
				// Muestra el cuadro de diálogo y obtiene el nombre de archivo
					if (dlgFile.ShowDialog() == DialogResult.OK)
						{ if (blnMultiple)	
								foreach (string strFile in dlgFile.FileNames)
									{ // Añade un separador si es necesario
											if (!string.IsNullOrEmpty(strFileName))
												strFileName += ";";
										// Añade el nombre de archivo
											strFileName += strFile;
									}
							else
								strFileName = dlgFile.FileName;
						}
				// Si ha llegado hasta aquí es porque no se ha seleccionado ningún archivo
					return strFileName;
		}

		/// <summary>
		/// 	Obtiene el nombre de un archivo a grabar utilizando un cuadro de diálogo de grabación de archivos
		/// </summary>
		public static string GetFileNameSave(string strFilter)
		{ return GetFileNameSave(null, null, strFilter);
		}

		/// <summary>
		/// 	Obtiene el nombre de un archivo a grabar utilizando un cuadro de diálogo de grabación de archivos
		/// </summary>
		public static string GetFileNameSave(string strPath, string strFilter)
		{ return GetFileNameSave(strPath, null, strFilter);
		}

		/// <summary>
		/// 	Obtiene el nombre de un archivo a grabar utilizando un cuadro de diálogo de grabación de archivos
		/// </summary>
		public static string GetFileNameSave(string strPath, string strFileName, string strFilter)
		{ SaveFileDialog dlgFile = new SaveFileDialog();

				// Cambia el nombre del archivo si en el directorio nos llega un nombre de archivo
					if (File.Exists(strPath) && string.IsNullOrEmpty(strFileName))
						{ strFileName = Path.GetFileName(strPath);
							strPath = Path.GetDirectoryName(strPath);
						}
				// Asigna las propiedades al cuadro de diálogo
					dlgFile.CheckFileExists = false;
					dlgFile.CheckPathExists = true;
					dlgFile.Filter = strFilter;
				// Asigna el directorio inicial
					if (!string.IsNullOrEmpty(strPath))
						dlgFile.InitialDirectory = strPath;
				// Asigna el nombre de archivo inicial
					dlgFile.FileName = strFileName;
				// Muestra el cuadro de diálogo y obtiene el nombre de archivo
					if (dlgFile.ShowDialog() == DialogResult.OK)
						return dlgFile.FileName;
				// Si ha llegado hasta aquí es porque no se ha seleccionado ningún archivo
					return "";
		}
		
		/// <summary>
		///		Obtiene el nombre de un directorio
		/// </summary>
		public static string GetPathName(string strPath)
		{ FolderBrowserDialog dlgPath = new FolderBrowserDialog();
		
				// Inicializa las propiedades
					dlgPath.SelectedPath = strPath;
				// Muestra el cuadro de diálogo y obtiene el nombre del directorio
					if (dlgPath.ShowDialog() == DialogResult.OK)
						return dlgPath.SelectedPath;
				// Si ha llegado hasta aquí es porque no se ha seleccionado ningún directorio
					return "";
		}
		
		/// <summary>
		/// 	Abre un cuadro de diálogo para solicitar un color
		/// </summary>
		public static Color GetColor()
		{ Color clrNewColor = Color.Transparent;
			ColorDialog dlgColor = new ColorDialog();
			
			// Muestra el cuadro de diálogo y obtiene el resultado
				if (dlgColor.ShowDialog() == DialogResult.OK)
					clrNewColor = dlgColor.Color;
			// Devuelve el color seleccionado
				return clrNewColor;			
		}
	}
}
