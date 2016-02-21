using System;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibNSharpDoc.Processor.Models.Documents
{
	/// <summary>
	///		Clase con los datos de un archivo de documentación
	/// </summary>
	public class DocumentFileModel
	{ 
		public DocumentFileModel(DocumentFileModel objParent, StructDocumentationModel objStruct, int intOrder)
		{ Parent = objParent;
			LanguageStruct = objStruct;
			if (objStruct != null)
				Name = objStruct.Name;
			Order = intOrder;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		internal string Debug(int intIndent)
		{ string strDebug = new string('\t', intIndent);

				// Añade los parámetros
					strDebug += Name + " (Order: " + Order + " Url: " + GetUrl("Documents") + ")";
				// Añade la documentación de los archivos
					return strDebug + Environment.NewLine + Childs.Debug(intIndent + 1);
		}

		/// <summary>
		///		Comprueba si es te documento se refiere a la estructura buscada
		/// </summary>
		private bool CheckContains(StructDocumentationModel objStruct)
		{ return StructType.EqualsIgnoreCase(objStruct.Type) && Name.EqualsIgnoreCase(objStruct.Name) && Order == objStruct.Order;
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda de este documento
		/// </summary>
		internal void TransformSearchLinks(Dictionary<String, DocumentFileModel> dctLinks, string strPathBase)
		{ MLBuilder.TransformSeachLinks(this, dctLinks, strPathBase);
		}

		/// <summary>
		///		Obtiene la Url del documento
		/// </summary>
		internal string GetUrl(string strPathBase)
		{ if (Parent == null)
				{ if (LanguageStruct.Type.EqualsIgnoreCase("Index"))
						return strPathBase;
					else
						return System.IO.Path.Combine(strPathBase, GetLastName(Name));
				}
			else
				return System.IO.Path.Combine(Parent.GetUrl(strPathBase), GetLastName(Name));
		}

		/// <summary>
		///		Obtiene el directorio local del documento
		/// </summary>
		internal string GetPathLocal()
		{ if (Parent == null)
				{ if (LanguageStruct.Type.EqualsIgnoreCase("Index"))
						return "";
					else
						return GetLastName(Name);
				}
			else
				return System.IO.Path.Combine(Parent.GetPathLocal(), GetLastName(Name));
		}

		/// <summary>
		///		Obtiene el último nombre de una cadena del tipo x.y.z
		/// </summary>
		private string GetLastName(string strName)
		{ // Si es un espacio de nombres recoge el nombre completo, si no, recoge el final de la cadena
				if (!StructType.EqualsIgnoreCase("NameSpace"))
					{	int intIndex = strName.IndexOf(".");
			
							// Corta a partir del punto
								while (intIndex > 0)
									{ strName = strName.Substring(intIndex + 1);
										intIndex = strName.IndexOf(".");
									}
							// Añade el orden si es necesario
								if (Order > 0)
									strName += "_" + Order.ToString();
					}
			// Devuelve el nombre de archivo
				return strName;
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
		{ get
				{ if (LanguageStruct == null)
						return 0;
					else
						return LanguageStruct.Order;
				}
			set
				{ if (LanguageStruct != null)
						LanguageStruct.Order = value;
				}
		}

		/// <summary>
		///		Tipo de estructura almacenada
		/// </summary>
		public string StructType 
		{ get
				{ if (LanguageStruct == null)
						return "NameSpace";
					else
						return LanguageStruct.Type;
				}
		}
	}
}
