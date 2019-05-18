using System;
using System.Windows.Forms;

using Bau.Libraries.LibCommonHelper.Extensors;

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
		internal static void Save()
		{
			Properties.Settings.Default.Save();
		}

		/// <summary>
		///		Obtiene un directorio concatenado con el directorio de inicio de la aplicación
		/// </summary>
		private static string GetApplicationPath(string path, string defaultValue)
		{
			if (!path.IsEmpty())
				return path;
			else
				return System.IO.Path.Combine(Application.StartupPath, defaultValue);
		}

		/// <summary>
		///		Tipo de salida
		/// </summary>
		internal static int OutputType
		{
			get { return Properties.Settings.Default.OutputType; }
			set { Properties.Settings.Default.OutputType = value; }
		}

		/// <summary>
		///		Directorio de plantillas
		/// </summary>
		internal static string TemplatesPath
		{
			get { return GetApplicationPath(Properties.Settings.Default.TemplatesPath, "Data\\Templates\\Main"); }
			set { Properties.Settings.Default.TemplatesPath = value; }
		}

		/// <summary>
		///		Directorio de salida
		/// </summary>
		internal static string OutputPath
		{
			get { return GetApplicationPath(Properties.Settings.Default.OutputPath, "Data\\Output"); }
			set { Properties.Settings.Default.OutputPath = value; }
		}

		/// <summary>
		///		Nombre del archivo de proyecto
		/// </summary>
		internal static string LastProject
		{
			get { return Properties.Settings.Default.LastProject; }
			set { Properties.Settings.Default.LastProject = value; }
		}
	}
}
