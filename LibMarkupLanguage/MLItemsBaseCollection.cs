using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Colección de <see cref="MLItemBase"/>
	/// </summary>
	public class MLItemsBaseCollection<TypeData> : List<TypeData> where TypeData : MLItemBase, new()
	{
		/// <summary>
		///		Añade una colección de elementos
		/// </summary>
		public void Add(MLItemsBaseCollection<TypeData> objColData)
		{ foreach (TypeData objData in objColData)
				Add(objData);
		}
		
		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strName)
		{ return Add(null, strName, null, true);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strName, string strValue)
		{ return Add(null, strName, strValue, true);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strName, bool blnValue)
		{ return Add(null, strName, blnValue);
		}
		
		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strName, double? dblValue)
		{ return Add(null, strName, dblValue);
		}
								
		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strName, DateTime dtmValue)
		{ return Add(null, strName, dtmValue);
		}
										
		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strPrefix, string strName, bool blnValue)
		{ return Add(strPrefix, strName, MLItemBase.Format(blnValue), false);
		}
		
		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strPrefix, string strName, double? dblValue)
		{ return Add(strPrefix, strName, MLItemBase.Format(dblValue), false);
		}
								
		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strPrefix, string strName, DateTime dtmValue)
		{ return Add(strPrefix, strName, MLItemBase.Format(dtmValue), false);
		}
								
		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strPrefix, string strName, string strValue, bool blnIsCData)
		{ TypeData objData = new TypeData();
		
				// Asigna los valores
					objData.Prefix = strPrefix;
					objData.Name = strName;
					objData.Value = strValue;
					objData.IsCData = blnIsCData;
				// Añade el nodo
					Add(objData);
				// Devuelve el objeto
					return objData;
		}
		
		/// <summary>
		///		Busca un elemento en la colección
		/// </summary>
		public TypeData Search(string strName)
		{ // Busca el elemento en la colección
				foreach (TypeData objData in this)
					if (objData.Name.Equals(strName))
						return objData;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
				return null;
		}

		/// <summary>
		///		Obtiene un valor lógico de un elemento
		/// </summary>
		public bool GetValue(string strName, bool blnDefault)
		{ TypeData objNode = Search(strName);
		
				if (objNode != null)
					return objNode.GetValue(blnDefault);
				else
					return blnDefault;
		}		

		/// <summary>
		///		Obtiene un valor entero de un elemento
		/// </summary>
		public int GetValue(string strName, int intDefault)
		{ TypeData objNode = Search(strName);
		
				if (objNode != null)
					return objNode.GetValue(intDefault);
				else
					return intDefault;
		}		

		/// <summary>
		///		Obtiene un valor doble de un elemento
		/// </summary>
		public double GetValue(string strName, double dblDefault)
		{ TypeData objNode = Search(strName);
		
				if (objNode != null)
					return objNode.GetValue(dblDefault);
				else
					return dblDefault;
		}		

		/// <summary>
		///		Obtiene la fecha de un elemento
		/// </summary>
		public DateTime GetValue(string strName, DateTime dtmDefault)
		{ TypeData objNode = Search(strName);
		
				if (objNode != null)
					return objNode.GetValue(dtmDefault);
				else
					return dtmDefault;
		}		

		/// <summary>
		///		Obtiene el valor de un elemento
		/// </summary>
		public string GetValue(string strName)
		{ TypeData objNode = Search(strName);
		
				if (objNode != null)
					return objNode.Value;
				else
					return null;
		}
		
		/// <summary>
		///		Indizador por el nombre del elemento
		/// </summary>
		public TypeData this[string strName]
		{ get
				{ TypeData objData = Search(strName);
				
						if (objData == null)
							return new TypeData();
						else
							return objData;
				}
			set
				{ TypeData objData = Search(strName);
				
						if (objData != null)
							objData = value;
				}
		}
	}
}
