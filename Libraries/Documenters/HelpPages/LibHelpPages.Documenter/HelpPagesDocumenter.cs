using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibHelpPages.Documenter
{
	/// <summary>
	///		Clase para documentación de estructuras de documentación en archivos XML
	/// </summary>
	public class HelpPages : LibNSharpDoc.Models.Interfaces.IDocumenter
	{
		// Constantes privadas
		private const string StructType = "AdditionalPage";

		/// <summary>
		///		Interpreta las páginas adicionales de documentación
		/// </summary>
		public StructDocumentationModel Parse(StructParameterModelDictionary parameters)
		{
			string path = parameters.GetValue("Path");

				if (!path.IsEmpty() && System.IO.Directory.Exists(path))
					return ParsePath(null, path);
				else
					throw new NotImplementedException("No se encuentra el directorio");
		}

		/// <summary>
		///		Interpreta un directorio
		/// </summary>
		private StructDocumentationModel ParsePath(StructDocumentationModel parent, string path)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Global,
																			  System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path)),
																			  StructType, 0);

				// Genera las páginas
				foreach (string file in System.IO.Directory.GetFiles(path))
					if (file.EndsWith(".docp", StringComparison.CurrentCultureIgnoreCase))
						structDoc.Childs.Add(ParseFile(parent, file));
				// Genera las páginas de los directorios
				foreach (string strChild in System.IO.Directory.GetDirectories(path))
					structDoc.Childs.Add(ParsePath(parent, strChild));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Interpreta el contenido de una página
		/// </summary>
		private StructDocumentationModel ParseFile(StructDocumentationModel parent, string file)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Public,
																			  System.IO.Path.GetFileNameWithoutExtension(file),
																			  StructType, 0);

				// Carga la página
				try
				{
					MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(file);

						foreach (MLNode nodeML in fileML.Nodes)
							if (nodeML.Name == "Page")
								foreach (MLNode childML in nodeML.Nodes)
									if (childML.Name == "Title")
										structDoc.Name = childML.Value;
									else
										structDoc.Parameters.Add(CreateParameter(structDoc, childML.Name, childML));
				}
				catch (Exception exception)
				{
					structDoc.Parameters.Add("Content", $"Excepción al documentar {file}. Excepción: {exception.Message}");
				}
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Crea una estructura con el contenido de un nodo
		/// </summary>
		private StructParameterModel CreateParameter(StructDocumentationModel parent, string key, MLNode nodeML)
		{
			string value = new LibMarkupLanguage.Services.XML.XMLWriter().ConvertToString(nodeML.Nodes);

				// Quita los caracteres de CDATA
				value = value.ReplaceWithStringComparison("<![CDATA[", "", StringComparison.CurrentCultureIgnoreCase);
				value = value.ReplaceWithStringComparison("]]>", "", StringComparison.CurrentCultureIgnoreCase);
				// Devuelve el parámetro
				return new StructParameterModel(key, value, null);
		}
	}
}
