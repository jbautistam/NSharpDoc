using System;
using System.Collections.Generic;
using System.Linq;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Colección de <see cref="MLNameSpace"/>
	/// </summary>
	public class MLNameSpacesCollection : List<MLNameSpace>
	{
		/// <summary>
		///		Añade un espacio de nombres
		/// </summary>
		public void Add(string strPrefix, string strNameSpace)
		{ if (!Exists(strPrefix))
				Add(new MLNameSpace(strPrefix, strNameSpace));
		}

		/// <summary>
		///		Comprueba si existe un prefijo
		/// </summary>
		private bool Exists(string strPrefix)
		{ // Recorre la colección
				foreach (MLNameSpace objNameSpace in this)
					if (objNameSpace.Prefix == strPrefix)
						return true;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
				return false;
		}

		/// <summary>
		///		Busca un espacio de nombre por su prefijo
		/// </summary>
		public MLNameSpace Search(string strPrefix)
		{ return this.FirstOrDefault(objMLNameSpace => objMLNameSpace.Prefix.Equals(strPrefix));
		}

		/// <summary>
		///		Obtiene el espacio de nombres que coincide con un prefijo
		/// </summary>
		public MLNameSpace this[string strPrefix]
		{ get
				{ MLNameSpace objNameSpace = Search(strPrefix);

						if (objNameSpace == null)
							return new MLNameSpace(strPrefix, "");
						else
							return objNameSpace;
				}
			set
				{ MLNameSpace objNameSpace = Search(strPrefix);

						if (objNameSpace == null)
							Add(strPrefix, value.NameSpace);
						else
							objNameSpace.NameSpace = value.NameSpace;
				}
		}
	}
}
