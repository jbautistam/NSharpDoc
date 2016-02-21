using System;

namespace Bau.Libraries.LibNSharpDoc.Processor.Models.Templates
{
	/// <summary>
	///		Modelo para los datos de una plantilla
	/// </summary>
	internal class TemplateModel
	{
		/// <summary>
		///		Tipo de plantilla
		/// </summary>
		internal enum TemplateType
			{ Index,
				Page
			}

		internal TemplateModel(TemplateType intIDType, string strName, string strNameStructType, 
													 string strPath, string strRelativeFileName, string strRootTemplate)
		{ IDType = intIDType;
			Name = strName;
			NameStructType = strNameStructType;
			Path = strPath;
			RelativeFileName = strRelativeFileName;
			RootTemplate = strRootTemplate;
		}

		/// <summary>
		///		Tipo de plantilla
		/// </summary>
		internal TemplateType IDType { get; }

		/// <summary>
		///		Nombre de la plantilla
		/// </summary>
		internal string Name { get; }

		/// <summary>
		///		Tipo de estructura que se trata en la plantilla
		/// </summary>
		internal string NameStructType { get; }

		/// <summary>
		///		Directorio base del archivo
		/// </summary>
		internal string Path { get; }

		/// <summary>
		///		Nombre de archivo relativo a la raíz
		/// </summary>
		public string RelativeFileName { get; }

		/// <summary>
		///		Nombre completo del archivo
		/// </summary>
		public string FullFileName
		{ get { return System.IO.Path.Combine(Path, RelativeFileName); }
		}

		/// <summary>
		///		Nombre de la plantilla raíz
		/// </summary>
		public string RootTemplate { get; }

		/// <summary>
		///		Nombre completo de la plantilla raíz
		/// </summary>
		public string FullFileNameRootTemplate
		{ get { return System.IO.Path.Combine(Path, RootTemplate); }
		}
	}
}
