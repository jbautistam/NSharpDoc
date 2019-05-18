using System;
using System.IO;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;

namespace Bau.Libraries.LibNetParser.Models.Solutions
{
	/// <summary>
	///		Clase con los datos de una solución
	/// </summary>
	public class SolutionVisualStudioModel
	{
		public SolutionVisualStudioModel(string fileName)
		{
			FileName = fileName;
		}

		/// <summary>
		///		Carga los datos de una solución
		/// </summary>
		public void Load()
		{ 
			// Si es un proyecto, carga el proyecto, si no, carga la solución
			if (FileName.EndsWith(".csproj", StringComparison.CurrentCultureIgnoreCase))
				Projects.Add(new ProjectVisualStudioModel(this, Path.GetFileNameWithoutExtension(FileName), FileName));
			else
				foreach (string line in LoadLines())
					ParseLine(line.TrimIgnoreNull());
			// Recorre los proyectos de la solución y carga los archivos
			foreach (ProjectVisualStudioModel project in Projects)
				project.LoadFiles();
		}

		/// <summary>
		///		Carga las líneas de un archivo
		/// </summary>
		private List<string> LoadLines()
		{
			List<string> lines = new List<string>();
			string content = HelperFiles.LoadTextFile(FileName);

				// Carga el archivo y lo separa en líneas
				if (!content.IsEmpty())
					lines = content.SplitByString("\n");
				// Devuelve las líneas del archivo
				return lines;
		}

		/// <summary>
		///		Interpreta una línea
		/// </summary>
		/// <remarks>
		///		Las líneas son de tipo:
		///		Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LibCosmosManager.ExportImport", "CosmosManager\Libraries\LibCosmosManager.ExportImport\LibCosmosManager.ExportImport.csproj", "{47175CC1-6DB1-453A-A37B-D77C4497BE19}"
		/// </remarks>
		private void ParseLine(string line)
		{
			if (!line.IsEmpty() && line.StartsWithIgnoreNull("Project"))
			{
				string[] parts = line.Split('=');

					if (parts.Length == 2)
					{
						string[] projectParts = parts[1].Split(',');

						if (projectParts.Length >= 2 && !projectParts[0].IsEmpty() && !projectParts[1].IsEmpty())
							Projects.Add(this,
										 projectParts[0].Replace("\"", "").TrimIgnoreNull(),
										 projectParts[1].Replace("\"", "").TrimIgnoreNull());
					}
			}
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string Debug()
		{
			string debug = FileName + Environment.NewLine;

				// Añade los proyectos y los archivos
				foreach (ProjectVisualStudioModel project in Projects)
				{ 
					// Añade los proyectos
					debug += "\t" + project.FullFileNameSource + Environment.NewLine;
					// Añade los archivos
					foreach (FileVisualStudioModel file in project.Files)
						debug += "\t\t" + file.FullFileName + Environment.NewLine;
				}
				// Devuelve la cadena
				return debug;
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		///		Proyectos
		/// </summary>
		public ProjectVisualStudioModelCollection Projects { get; private set; } = new ProjectVisualStudioModelCollection();
	}
}
