using System;
using System.IO;

namespace Bau.Libraries.LibMarkupLanguage.Tools
{
	/// <summary>
	///		Clase de ayuda para el tratamiento de archivos
	/// </summary>
	internal static class FileHelper
	{
		/// <summary>
		/// 	Carga un archivo de texto en una cadena
		/// </summary>
		internal static string LoadTextFile(string strFileName)
		{ string strData, strContent = "";

				// Carga el archivo
					using (StreamReader stmFile = new StreamReader(strFileName, System.Text.Encoding.GetEncoding("UTF-8")))
						{ // Lee los datos
								while ((strData = stmFile.ReadLine()) != null)
									{ // Le añade un salto de línea si es necesario
											if (strContent != "")
												strContent += "\n";
										// Añade la línea leída
											strContent += strData;
									}
							// Cierra el stream
								stmFile.Close();
						}
				// Devuelve el contenido
					return strContent;
		}

		/// <summary>
		/// 	Graba una cadena en un archivo de texto
		/// </summary>
		internal static void SaveTextFile(string strFileName, string strText)
		{	using (StreamWriter stmFile = new StreamWriter(strFileName, false, System.Text.Encoding.GetEncoding("UTF-8")))
				{ // Escribe la cadena
						stmFile.WriteLine(strText);
					// Cierra el stream
						stmFile.Close();
				}
		}
	}
}
