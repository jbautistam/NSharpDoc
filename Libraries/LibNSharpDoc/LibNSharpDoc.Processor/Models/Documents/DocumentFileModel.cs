using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibNSharpDoc.Processor.Models.Documents
{
	/// <summary>
	///		Clase con los datos de un archivo de documentación
	/// </summary>
	public class DocumentFileModel
	{
		public DocumentFileModel(DocumentFileModel parent, StructDocumentationModel structDoc, int order)
		{
			Parent = parent;
			LanguageStruct = structDoc;
			if (structDoc != null)
				Name = structDoc.Name;
			Order = order;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		internal string Debug(int indent)
		{
			string debug = new string('\t', indent);

				// Añade los parámetros
				debug += Name + " (Order: " + Order + " Url: " + GetUrl("Documents") + ")";
				// Añade la documentación de los archivos
				return debug + Environment.NewLine + Childs.Debug(indent + 1);
		}

		/// <summary>
		///		Comprueba si este documento se refiere a la estructura buscada
		/// </summary>
		private bool CheckContains(StructDocumentationModel structDoc)
		{
			return StructType.EqualsIgnoreCase(structDoc.Type) && Name.EqualsIgnoreCase(structDoc.Name) && Order == structDoc.Order;
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda de este documento
		/// </summary>
		internal void TransformSearchLinks(Dictionary<string, DocumentFileModel> links, string pathBase)
		{
			MLBuilder.TransformSeachLinks(this, links, pathBase);
		}

		/// <summary>
		///		Obtiene la Url del documento
		/// </summary>
		internal string GetUrl(string pathBase)
		{
			if (Parent == null)
			{
				if (LanguageStruct.Type.EqualsIgnoreCase("Index"))
					return pathBase;
				else
					return System.IO.Path.Combine(pathBase, LibCommonHelper.Files.HelperFiles.Normalize(GetLastName(Name)));
			}
			else
				return System.IO.Path.Combine(Parent.GetUrl(pathBase), LibCommonHelper.Files.HelperFiles.Normalize(GetLastName(Name)));
		}

		/// <summary>
		///		Obtiene el directorio local del documento
		/// </summary>
		internal string GetPathLocal()
		{
			if (Parent == null)
			{
				if (LanguageStruct.Type.EqualsIgnoreCase("Index"))
					return "";
				else
					return LibCommonHelper.Files.HelperFiles.Normalize(GetLastName(Name));
			}
			else
				return System.IO.Path.Combine(Parent.GetPathLocal(), LibCommonHelper.Files.HelperFiles.Normalize(GetLastName(Name)));
		}

		/// <summary>
		///		Obtiene el último nombre de una cadena del tipo x.y.z
		/// </summary>
		private string GetLastName(string name)
		{ 
			// Si es un espacio de nombres recoge el nombre completo, si no, recoge el final de la cadena
			if (!StructType.EqualsIgnoreCase("NameSpace"))
			{
				int index = name.IndexOf(".");

					// Corta a partir del punto
					while (index > 0)
					{
						name = name.Substring(index + 1);
						index = name.IndexOf(".");
					}
					// Añade el orden si es necesario
					if (Order > 0)
						name += "_" + Order.ToString();
			}
			// Devuelve el nombre de archivo
			return name;
		}

		/// <summary>
		///		Archivo padre
		/// </summary>
		public DocumentFileModel Parent { get; private set; }

		/// <summary>
		///		Elementos hijo
		/// </summary>
		public DocumentFileModelCollection Childs { get; } = new DocumentFileModelCollection();

		/// <summary>
		///		Estructura principal documentada
		/// </summary>
		public StructDocumentationModel LanguageStruct { get; set; }

		/// <summary>
		///		Estructuras añadidas al documento
		/// </summary>
		public StructDocumentationModelCollection StructsReferenced { get; } = new StructDocumentationModelCollection();

		/// <summary>
		///		Generador de archivos intermedios de documentación
		/// </summary>
		public Processor.Writers.MLIntermedialBuilder MLBuilder { get; } = new Processor.Writers.MLIntermedialBuilder();

		/// <summary>
		///		Nombre del elemento descrito
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Orden del elemento (para las sobrecargas)
		/// </summary>
		public int Order
		{
			get
			{
				if (LanguageStruct == null)
					return 0;
				else
					return LanguageStruct.Order;
			}
			set
			{
				if (LanguageStruct != null)
					LanguageStruct.Order = value;
			}
		}

		/// <summary>
		///		Tipo de estructura almacenada
		/// </summary>
		public string StructType
		{
			get
			{
				if (LanguageStruct == null)
					return "NameSpace";
				else
					return LanguageStruct.Type;
			}
		}
	}
}