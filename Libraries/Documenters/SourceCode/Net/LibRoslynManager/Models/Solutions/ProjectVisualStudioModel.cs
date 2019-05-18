using System;
using System.IO;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibRoslynManager.Models.Solutions
{
	/// <summary>
	///		Clase con los datos de un proyecto
	/// </summary>
	public class ProjectVisualStudioModel
	{
		public ProjectVisualStudioModel(SolutionVisualStudioModel objSolution, string strName, string strFileName)
		{ // Asigna las propiedades
				Name = strName;
				FileNameRelative = strFileName;
			// Asigna el directorio origen
				if (objSolution.FileName.EqualsIgnoreCase(strFileName))
					FullFileNameSource = strFileName;
				else
					FullFileNameSource = CombinePaths(Path.GetDirectoryName(objSolution.FileName), FileNameRelative);
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string Debug()
		{ return Name + Environment.NewLine;
		}

		/// <summary>
		///		Combina dos directorios
		/// </summary>
		private string CombinePaths(string strPathParent, string strFileName)
		{ // Quita los directorios finales mientras el nombre de archivo destino comience por ../
				while (strFileName.StartsWith("../") || strFileName.StartsWith("..\\"))
					{ strPathParent = Path.GetDirectoryName(strPathParent);
						strFileName = strFileName.Substring(3);
					}
			// Combina los directorios
				return Path.Combine(strPathParent, strFileName);
		}

		/// <summary>
		///		Carga los archivos asociados al proyecto
		/// </summary>
		internal void LoadFiles()
		{ if (File.Exists(FullFileNameSource))
				{ MLFile objMLFile = new LibMarkupLanguage.Services.XML.XMLParser().Load(FullFileNameSource);

						// Carga los datos
							foreach (MLNode objMLRoot in objMLFile.Nodes)
								if (objMLRoot.Name == "Project")
									foreach (MLNode objMLNode in objMLRoot.Nodes)
										if (objMLNode.Name == "ItemGroup")
											foreach (MLNode objMLChild in objMLNode.Nodes)
												if (objMLChild.Name == "Compile")
													{ string strFile = objMLChild.Attributes["Include"].Value;

															if (!strFile.IsEmpty())
																{ FileVisualStudioModel objFile = new FileVisualStudioModel();

																		// Asigna el archivo
																			objFile.FileName = strFile;
																			objFile.FullFileName = Path.Combine(Path.GetDirectoryName(FullFileNameSource), strFile);
																		// Añade el archivo a la colección
																			Files.Add(objFile);
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
		{ get { return Path.GetDirectoryName(FullFileNameSource); }
		}

		/// <summary>
		///		Nombre del archivo de proyecto completo
		/// </summary>
		public string FullFileNameSource { get; }

		/// <summary>
		///		Archivos asociados al proyecto
		/// </summary>
		public FileVisualStudioModelCollection Files { get; } = new FileVisualStudioModelCollection();

		///// <summary>
		/////		Estructuras definidas en el proyecto
		///// </summary>
		//public ProjectStructModelCollection Structs { get; } = new ProjectStructModelCollection();
	}
}
