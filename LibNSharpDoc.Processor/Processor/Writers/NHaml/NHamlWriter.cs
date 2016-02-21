using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.NHaml
{
	/// <summary>
	///		Generador de un archivo NHaml para documentación
	/// </summary>
	public class NHamlWriter : IDocumentWriter
	{ // Variables privadas
			private NHamlConversor objConversor = new NHamlConversor();

		/// <summary>
		///		Graba la documentación Nhaml en un archivo
		/// </summary>
		public void Save(DocumentFileModel objDocument, MLIntermedialBuilder objMLBuilder, 
										 string strFileNameTemplate, string strPath)
		{ Save(GetMLFile(objDocument, ConvertMLNode(objMLBuilder)),
										 System.IO.Path.Combine(strPath, objDocument.GetPathLocal()));
		}

		/// <summary>
		///		Convierte una estructura XML en NHaml
		/// </summary>
		private string ConvertMLNode(MLIntermedialBuilder objMLBuilder)
		{ return objConversor.Convert(objMLBuilder);
		}

		/// <summary>
		///		Graba un archivo XML
		/// </summary>
		private void Save(MLFile objMLFile, string strPath)
		{	string strFileName = System.IO.Path.Combine(strPath, "Document.dcx");

				// Crea el directorio
					LibHelper.Files.HelperFiles.MakePath(strPath);
				// Graba el archivo de documentación
					new LibMarkupLanguage.Services.XML.XMLWriter().Save(objMLFile, strFileName);
		}

		/// <summary>
		///		Obtiene el archivo XML asociado a un documento
		/// </summary>
		private MLFile GetMLFile(DocumentFileModel objDocument, string strNHaml)
		{ string strDescription = objDocument.Name;

				// Obtiene la descripción
					if (objDocument.LanguageStruct != null)
						strDescription = objDocument.LanguageStruct.Name;
				// Obtiene el archivo
					return GetMLFile(objDocument.Name, strDescription, strNHaml);
		}

		/// <summary>
		///		Obtiene el archivo XML asociado a un documento
		/// </summary>
		private MLFile GetMLFile(string strTitle, string strDescription, string strNhaml)
		{ MLFile objMLFile = new MLFile();
			MLNode objMLRoot = objMLFile.Nodes.Add("Document");

				// Añade el contenido
					objMLRoot.Nodes.Add("Title", strTitle);
					objMLRoot.Nodes.Add("Description", strDescription);
					objMLRoot.Nodes.Add("Content", strNhaml);
					objMLRoot.Nodes.Add("ShowAtRss", false);
					objMLRoot.Nodes.Add("Type", 2);
					objMLRoot.Nodes.Add("Scope", 0);
					objMLRoot.Nodes.Add("DateCreate", DateTime.Now);
				// Devuelve el archivo
					return objMLFile;
		}
	}
}
