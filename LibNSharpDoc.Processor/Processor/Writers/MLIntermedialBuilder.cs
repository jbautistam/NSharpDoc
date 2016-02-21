using System;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers
{
	/// <summary>
	///		Clase interna de generación en nodos ML
	/// </summary>
	public class MLIntermedialBuilder
	{ // Constantes privadas
			private const string cnstStrTagRoot = "Page";
			private const string cnstStrTagSpan = "Part";
			private const string cnstStrTagBold = "IsBold";
			private const string cnstStrTagItalic = "IsItalic";
			private const string cnstStrTagLink = "Link";
			private const string cnstStrTagSearchLink = "SearchLink";
			private const string cnstStrTagHref = "Ref";

		/// <summary>
		///		Limpia el constructor
		/// </summary>
		public void Clear()
		{ Root = new MLNode(cnstStrTagRoot);
		}

		/// <summary>
		///		Comprueba si un nodo está vacío
		/// </summary>
		public bool CheckIsEmpty(MLNode objMLNode)
		{ return objMLNode.Nodes.Count == 0 && objMLNode.Value.IsEmpty();
		}

		/// <summary>
		///		Comprueba si un nodo es complejo
		/// </summary>
		public bool CheckIsComplex(MLNode objMLNode)
		{ return objMLNode.Nodes.Count > 0;
		}

		/// <summary>
		///		Comprueba si es un nodo de span
		/// </summary>
		public bool CheckIsSpanNode(MLNode objMLNode)
		{ return objMLNode.Name == MLIntermedialBuilder.cnstStrTagSpan;
		}

		/// <summary>
		///		Comprueba si es un nodo de span
		/// </summary>
		public bool CheckIsLinkNode(MLNode objMLNode)
		{ return objMLNode.Name == MLIntermedialBuilder.cnstStrTagLink;
		}

		/// <summary>
		///		Comprueba si un nodo de span está definido como negrita
		/// </summary>
		public bool CheckIsBold(MLNode objMLNode)
		{ return objMLNode.Attributes[cnstStrTagBold].Value.GetBool();
		}

		/// <summary>
		///		Comprueba si un atributo se corresponde con la negrita
		/// </summary>
		public bool CheckIsBold(MLAttribute objMLAttribute)
		{	return objMLAttribute.Name == cnstStrTagBold;
		}

		/// <summary>
		///		Comprueba si un nodo de span está definido como cursiva
		/// </summary>
		public bool CheckIsItalic(MLNode objMLNode)
		{ return objMLNode.Attributes[cnstStrTagItalic].Value.GetBool();
		}

		/// <summary>
		///		Comprueba si un atributo se corresponde con la cursiva
		/// </summary>
		public bool CheckIsItalic(MLAttribute objMLAttribute)
		{ return objMLAttribute.Name == cnstStrTagItalic;
		}

		/// <summary>
		///		Comprueba si un atributo es una referencia
		/// </summary>
		public bool CheckIsHref(MLAttribute objMLAttribute)
		{ return objMLAttribute.Name == cnstStrTagHref;
		}

		/// <summary>
		///		Obtiene un span
		/// </summary>
		public MLNode GetSpan(string strText, bool blnBold = false, bool blnItalic = false)
		{ MLNode objSpan = new MLNode(cnstStrTagSpan, strText);

				// Añade los atributos
					objSpan.Attributes.Add(cnstStrTagBold, blnBold);
					objSpan.Attributes.Add(cnstStrTagItalic, blnItalic);
				// Devuelve el nodo
					return objSpan;
		}

		/// <summary>
		///		Obtiene un vínculo
		/// </summary>
		public MLNode GetLink(string strTitle, string strUrl)
		{	MLNode objMLNode = new MLNode(cnstStrTagLink, strTitle);

				// Añade la referencia
					objMLNode.Attributes.Add(cnstStrTagHref, strUrl);
				// Devuelve el nodo
					return objMLNode;
		}

		/// <summary>
		///		Obtiene un vínculo para un elemento que se va a sustituir por otro en el postproceso
		/// </summary>
		/// <returns>
		///		Cuando se añade información como un nombre de tipo, nos interesa poder generar un
		///	hipervínculo pero aún no sabemos en qué documento se ha creado la información, por eso
		///	creamos un nodo de "vínculo a postprocesar" para modificarlo una vez generados todos los
		///	documentos
		/// </returns>
		public MLNode GetSearchLink(string strTitle, string strUrlSearch)
		{ MLNode objMLNode = new MLNode(cnstStrTagSearchLink, strTitle);

				// Añade la referencia
					objMLNode.Attributes.Add(cnstStrTagHref, strUrlSearch);
				// Devuelve el nodo
					return objMLNode;
		}

		/// <summary>
		///		Obtiene el valor del atributo href de un hipervínculo
		/// </summary>
		public string GetHref(MLNode objMLNode)
		{ return objMLNode.Attributes[cnstStrTagHref].Value;
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda
		/// </summary>
		internal void TransformSeachLinks(DocumentFileModel objDocument, Dictionary<string, DocumentFileModel> dctLinks, string strPathBase)
		{ foreach (MLNode objMLNode in Root.Nodes)
				TransformSeachLinks(objDocument, dctLinks, objMLNode, strPathBase);
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda
		/// </summary>
		private void TransformSeachLinks(DocumentFileModel objDocument, Dictionary<string, DocumentFileModel> dctLinks, MLNode objMLNode, string strPathBase)
		{ if (objMLNode.Name == cnstStrTagSearchLink)
				{ string strTagLink = objMLNode.Attributes[cnstStrTagHref].Value;
					DocumentFileModel objDocumentTarget;
						
						// Obtiene la referencia
							if (dctLinks.TryGetValue(strTagLink, out objDocumentTarget))
								{	objMLNode.Name = cnstStrTagLink;
									objMLNode.Attributes[cnstStrTagHref].Value = objDocumentTarget.GetUrl(strPathBase);
								}
							else
								objMLNode.Name = cnstStrTagSpan;
				}
			else
				foreach (MLNode objMLChild in objMLNode.Nodes)
					TransformSeachLinks(objDocument, dctLinks, objMLChild, strPathBase);
		}

		/// <summary>
		///		Nodo raíz
		/// </summary>
		public MLNode Root { get; private set; } = new MLNode(cnstStrTagRoot);
	}
}
