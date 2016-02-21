using System;

using Bau.Libraries.LibHelper.Extensors;
using System.Collections.Generic;

namespace Bau.Libraries.LibNSharpDoc.Processor.Models.Projects
{
	/// <summary>
	///		Lista de <see cref="ProviderModel"/>
	/// </summary>
	public class ProviderModelCollection : List<ProviderModel>
	{
		/// <summary>
		///		Elimina un elemento por su clave
		/// </summary>
		public void RemoveByID(string strKey)
		{ for (int intIndex = Count - 1; intIndex >= 0; intIndex--)
				if (this[intIndex].ID == strKey)
					RemoveAt(intIndex);
		}
		/// <summary>
		///		Indice de un elemento
		/// </summary>
		public int IndexOf(string strKey)
		{ // Busca el índice del elemento
				for (int intIndex = 0; intIndex < Count; intIndex++)
					if (this[intIndex].ID.EqualsIgnoreCase(strKey))
						return intIndex;
			// Si ha llegado hasta aquí es porque no ha encontrado ningún elemento
				return -1;
		}

		/// <summary>
		///		Elimina un elemento
		/// </summary>
		public void Remove(string strKey)
		{ int intIndex = IndexOf(strKey);

				if (intIndex != -1)
					RemoveAt(intIndex);
		}

		/// <summary>
		///		Obtiene un elemento
		/// </summary>
		public ProviderModel this[string strKey]
		{ get
				{ int intIndex = IndexOf(strKey);

						if (intIndex < 0)
							return null;
						else
							return this[intIndex];
				}
			set
				{ int intIndex = IndexOf(strKey);

						if (intIndex >= 0)
							this[intIndex] = value;
				}
		}
	}
}
