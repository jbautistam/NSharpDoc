using System;
using System.Windows.Forms;

namespace Bau.Applications.NSharpDoc
{
	/// <summary>
	///		Clase de configuración de la aplicación
	/// </summary>
	public static class Configuration
	{
		/// <summary>
		///		Graba los parámetros de configuración
		/// </summary>
		internal static void Save(string strLastProject)
		{ // Ajusta los datos en memoria
				LastProject = strLastProject;
			// Ajusta los datos de las propiedades
				Properties.Settings.Default.LastProject = strLastProject;
			// Graba la configuración
				Properties.Settings.Default.Save();
		}

		/// <summary>
		///		Nombre del archivo de proyecto
		/// </summary>
		internal static string LastProject 
		{ get { return Properties.Settings.Default.LastProject; }
			set { Properties.Settings.Default.LastProject = value; }
		}
	}
}
