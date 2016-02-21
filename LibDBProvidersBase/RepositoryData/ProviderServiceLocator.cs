using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibDBProvidersBase.RepositoryData
{
	/// <summary>
	///		Controlador de repositorios
	/// </summary>
	public class ProviderServiceLocator 
	{	// Variables privadas
			private static ProviderServiceLocator objInstance = null;

		public ProviderServiceLocator()
		{ Repositories = new Dictionary<string, IProviderRepository>();
		}

		/// <summary>
		///		Obtiene una instancia del controlador
		/// </summary>
		public static ProviderServiceLocator GetInstance()
		{	// Crea una instancia global si no existía
				if (objInstance == null)
					objInstance = new ProviderServiceLocator();
			// Devuelve la instancia
				return objInstance;
		}

		/// <summary>
		///		Registra un proveedor de un repositorio para un tipo
		/// </summary>
		public void Register(string strKey, IProviderRepository objRepository)
		{ if (!Repositories.ContainsKey(strKey))
				Repositories.Add(strKey, objRepository);
		}

		/// <summary>
		///		Obtiene un nuevo proveedor de repositorio para la clave definida
		/// </summary>
		public IProviderRepository GetInstance(string strKey)
		{ if (Repositories.ContainsKey(strKey))
				return Repositories[strKey].GetInstance();
			else
				return null;
		}

		/// <summary>
		///		Diccionario con los repositorios
		/// </summary>
		private Dictionary<string, IProviderRepository> Repositories { get; set; }
	}
}
