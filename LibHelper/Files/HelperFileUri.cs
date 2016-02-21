using System;
using System.IO;

using Bau.Libraries.LibHelper.Extensors;
namespace Bau.Libraries.LibHelper.Files 
{
	/// <summary>
	///		Clase para tratamiento de nombres de archivo
	/// </summary>
	public static class HelperFileUri 
	{
		/// <summary>
		///		Obtiene el primer directorio
		/// </summary>
		public static string GetFirstPath(string strFileName)
		{ string [] arrStrPath = Split(strFileName);

				// Obtiene el primer directorio
					if (arrStrPath.Length > 0)
						return arrStrPath[0];
					else
						return null;
		}

		/// <summary>
		///		Obtiene la Url relativa
		/// </summary>
		public static string GetUrlRelative(string strUrlSource, string strUrlTarget)
		{ string [] arrStrURLPage = Split(strUrlSource);
			string [] arrStrURLTarget = Split(strUrlTarget);
			string strURL = "";
			int intIndex = 0, intIndexTarget;
			
				// Quita los directorios iniciales que sean iguales
					while (intIndex < arrStrURLTarget.Length - 1 &&
								 arrStrURLTarget[intIndex].Equals(arrStrURLPage[intIndex], StringComparison.CurrentCultureIgnoreCase))
						intIndex++;
				// Añade todos los .. que sean necesarios
					intIndexTarget = intIndex;
					while (intIndexTarget < arrStrURLPage.Length - 1)
						{ // Añade el salto
								strURL += "../";
							// Incrementa el índice
								intIndexTarget++;
						}
				// Añade los archivos finales
					while (intIndex < arrStrURLTarget.Length)
						{ // Añade el directorio
								strURL += arrStrURLTarget[intIndex];
							// Añade el separador
								if (intIndex < arrStrURLTarget.Length - 1)
									strURL += "/";
							// Incrementa el índice
								intIndex++;
						}
				// Devuelve la URL
					return strURL;
		}

		/// <summary>
		///		Parte una cadena por \ o por /
		/// </summary>
		private static string [] Split(string strUrl)
		{ if (strUrl.IsEmpty())
				return new string [] { "" };
			else if (strUrl.IndexOf('/') >= 0)
				return strUrl.Split('/');
			else
				return strUrl.Split('\\');
		}
	}
}
