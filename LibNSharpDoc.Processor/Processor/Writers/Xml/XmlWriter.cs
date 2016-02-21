using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.Xml
{
	/// <summary>
	///		Generador de un archivo Xml para documentación
	/// </summary>
	public class XmlWriter : IDocumentWriter
	{ 
		/// <summary>
		///		Graba la documentación Nhaml en un archivo
		/// </summary>
		public void Save(DocumentFileModel objDocument, MLIntermedialBuilder objMLBuilder, 
										 string strFileNameTemplate, string strPath)
		{ MLFile objMLFile = GetMLFile(objDocument.Name, "", objMLBuilder.Root);

				// Graba el archivo
					Save(objMLFile, strPath, LibHelper.Files.HelperFiles.Normalize(objDocument.Name, false) + ".xml");
		}

		/// <summary>
		///		Graba un archivo XML
		/// </summary>
		private void Save(MLFile objMLFile, string strPath, string strFileName)
		{	// Crea el directorio
				LibHelper.Files.HelperFiles.MakePath(strPath);
			// Graba el archivo de documentación
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(objMLFile, System.IO.Path.Combine(strPath, strFileName));
		}

		/// <summary>
		///		Obtiene el archivo XML asociado a un documento
		/// </summary>
		private MLFile GetMLFile(string strTitle, string strDescription, MLNode objMLNode)
		{ MLFile objMLFile = new MLFile();
			MLNode objMLRoot = objMLFile.Nodes.Add("Document");

				// Añade el contenido
					objMLRoot.Nodes.Add("Title", strTitle);
					objMLRoot.Nodes.Add("Description", strDescription);
					objMLRoot.Nodes.Add("Content").Nodes.AddRange(objMLNode.Nodes);
					objMLRoot.Nodes.Add("DateCreate", DateTime.Now);
				// Devuelve el archivo
					return objMLFile;
		}
	}
}
