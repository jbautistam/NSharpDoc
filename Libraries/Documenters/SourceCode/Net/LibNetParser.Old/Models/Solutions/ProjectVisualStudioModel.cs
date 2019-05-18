using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibNetParser.Models.Solutions
{
	/// <summary>
	///		Clase con los datos de un proyecto
	/// </summary>
	public class ProjectVisualStudioModel
	{
		public ProjectVisualStudioModel(SolutionVisualStudioModel solution, string name, string fileName)
		{ 
			// Asigna las propiedades
			Name = name;
			FileNameRelative = fileName;
			// Asigna el directorio origen
			if (solution.FileName.EqualsIgnoreCase(fileName))
				FullFileNameSource = fileName;
			else
				FullFileNameSource = CombinePaths(Path.GetDirectoryName(solution.FileName), FileNameRelative);
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string Debug()
		{
			return Name + Environment.NewLine;
		}

		/// <summary>
		///		Combina dos directorios
		/// </summary>
		private string CombinePaths(string pathParent, string fileName)
		{ 
			// Quita los directorios finales mientras el nombre de archivo destino comience por ../
			while (fileName.StartsWith("../") || fileName.StartsWith("..\\"))
			{
				pathParent = Path.GetDirectoryName(pathParent);
				fileName = fileName.Substring(3);
			}
			// Combina los directorios
			return Path.Combine(pathParent, fileName);
		}

		/// <summary>
		///		Carga los archivos asociados al proyecto
		/// </summary>
		internal void LoadFiles()
		{
			if (File.Exists(FullFileNameSource))
			{
				MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(FullFileNameSource);

					// Carga los datos
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == "Project")
							foreach (MLNode nodeML in rootML.Nodes)
								if (nodeML.Name == "ItemGroup")
									foreach (MLNode childML in nodeML.Nodes)
										if (childML.Name == "Compile")
										{
											string fileName = childML.Attributes["Include"].Value;

												if (!fileName.IsEmpty())
												{
													FileVisualStudioModel file = new FileVisualStudioModel();

														// Asigna el archivo
														file.FileName = fileName;
														file.FullFileName = Path.Combine(Path.GetDirectoryName(FullFileNameSource), fileName);
														// Añade el archivo a la colección
														Files.Add(file);
												}
										}
			}
		}

		/// <summary>
		///		Nombre del proyecto
		/// </summary>
		public string Name { get; }

		/// <summary>
		///		Nombre de archivo de proyecto relativo
		/// </summary>
		public string FileNameRelative { get; }

		/// <summary>
		///		Directorio origen
		/// </summary>
		public string PathSource
		{
			get { return Path.GetDirectoryName(FullFileNameSource); }
		}

		/// <summary>
		///		Nombre del archivo de proyecto completo
		/// </summary>
		public string FullFileNameSource { get; }

		/// <summary>
		///		Archivos asociados al proyecto
		/// </summary>
		public FileVisualStudioModelCollection Files { get; } = new FileVisualStudioModelCollection();
	}
}
