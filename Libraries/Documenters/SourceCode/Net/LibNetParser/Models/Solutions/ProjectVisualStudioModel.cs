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
				// Carga los archivos de proyecto (para Net Framework)
				Files.AddRange(LoadFilesProject(FullFileNameSource));
				// Si no se ha cargado ningún archivo de proyecto, puede que sea un archivo de proyecto de Net Standard
				// en este caso, se recogen todos los archivos de los directorios
				if (Files.Count == 0)
					Files.AddRange(LoadFilesPath(Path.GetDirectoryName(FullFileNameSource), GetFilesExtension(FullFileNameSource)));
			}
		}

		/// <summary>
		///		Carga los archivos de proyecto (en Net Framework, los archivos de proyecto están bajo el nodo ItemGroup)
		/// </summary>
		private FileVisualStudioModelCollection LoadFilesProject(string projectFileName)
		{
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(projectFileName);
			FileVisualStudioModelCollection files = new FileVisualStudioModelCollection();

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
													files.Add(file);
											}
									}
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Carga los archivos de un directorio y sus subdirectorios correspondientes a la máscara
		/// </summary>
		private FileVisualStudioModelCollection LoadFilesPath(string path, string extension)
		{
			System.Collections.Generic.List<string> sourceFiles = LibCommonHelper.Files.HelperFiles.ListRecursive(path);
			FileVisualStudioModelCollection files = new FileVisualStudioModelCollection();

				// Recorre los archivos de los directorios creando los archivos a interpretar
				foreach (string sourceFile in sourceFiles)
					if (!string.IsNullOrEmpty(sourceFile) && sourceFile.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase))
					{
						FileVisualStudioModel file = new FileVisualStudioModel();

							// Asigna el archivo
							file.FileName = Path.GetFileName(sourceFile);
							file.FullFileName = Path.Combine(path, sourceFile);
							// Añade el archivo a la colección
							files.Add(file);
					}
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Obtiene la extensión de los archivos dependiendo de la extensión del proyecto
		/// </summary>
		private string GetFilesExtension(string projectFileName)
		{
			if (projectFileName.EndsWith(".csproj", StringComparison.CurrentCultureIgnoreCase))
				return ".cs";
			else 
				return ".vb";
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
