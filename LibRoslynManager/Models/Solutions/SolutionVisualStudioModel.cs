using System;
using System.IO;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibHelper.Files;

namespace Bau.Libraries.LibRoslynManager.Models.Solutions
{
	/// <summary>
	///		Clase con los datos de una solución
	/// </summary>
	public class SolutionVisualStudioModel
	{
		public SolutionVisualStudioModel(string strFileName)
		{ FileName = strFileName;
		}

		/// <summary>
		///		Carga los datos de una solución
		/// </summary>
		public void Load()
		{ // Si es un proyecto, carga el proyecto, si no, carga la solución
				if (FileName.EndsWith(".csproj", StringComparison.CurrentCultureIgnoreCase))
					Projects.Add(new ProjectVisualStudioModel(this, Path.GetFileNameWithoutExtension(FileName), FileName));
				else
					{ List<string> objColLines = LoadLines(); 

							// Recorre el texto de la solución y carga los proyectos
								foreach (string strLine in objColLines)
									ParseLine(strLine.TrimIgnoreNull());
					}
				// Recorre los proyectos de la solución y carga los archivos
					foreach (ProjectVisualStudioModel objProject in Projects)
						objProject.LoadFiles();
		}

		/// <summary>
		///		Carga las líneas de un archivo
		/// </summary>
		private List<string> LoadLines()
		{ List<string> objColLines = new List<string>();
			string strContent = HelperFiles.LoadTextFile(FileName);

				// Carga el archivo y lo separa en líneas
					if (!strContent.IsEmpty())
						objColLines = strContent.SplitByString("\n");
				// Devuelve las líneas del archivo
					return objColLines;
		}

		/// <summary>
		///		Interpreta una línea
		/// </summary>
		/// <remarks>
		///		Las líneas son de tipo:
		///		Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LibCosmosManager.ExportImport", "CosmosManager\Libraries\LibCosmosManager.ExportImport\LibCosmosManager.ExportImport.csproj", "{47175CC1-6DB1-453A-A37B-D77C4497BE19}"
		/// </remarks>
		private void ParseLine(string strLine)
		{ if (!strLine.IsEmpty() && strLine.StartsWithIgnoreNull("Project"))
				{ string [] arrStrParts = strLine.Split('=');

						if (arrStrParts.Length == 2)
							{ string [] arrStrProject = arrStrParts[1].Split(',');

									if (arrStrProject.Length >= 2 && !arrStrProject[0].IsEmpty() && !arrStrProject[1].IsEmpty())
										Projects.Add(this,
																 arrStrProject[0].Replace("\"", "").TrimIgnoreNull(),
																 arrStrProject[1].Replace("\"", "").TrimIgnoreNull());
							}
				}
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string Debug()
		{ string strDebug = FileName + Environment.NewLine;

				// Añade los proyectos y los archivos
					foreach (ProjectVisualStudioModel objProject in Projects)
						{ // Añade los proyectos
								strDebug += "\t" + objProject.FullFileNameSource + Environment.NewLine;
							// Añade los archivos
								foreach (FileVisualStudioModel objFile in objProject.Files)
									strDebug += "\t\t" + objFile.FullFileName + Environment.NewLine;
						}
				// Devuelve la cadena
					return strDebug;
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
