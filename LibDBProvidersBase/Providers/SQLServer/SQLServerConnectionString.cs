using System;

namespace Bau.Libraries.LibDBProvidersBase.Providers.SQLServer
{
	/// <summary>
	///		Cadena de conexión de SQL Server
	/// </summary>
	public class SQLServerConnectionString : IConnectionString
	{ // Enumerados públicos
			public enum ConnectionType
				{ Normal,
					File
				}
		// Variables privadas
			private string strConnectionString = null;

		public SQLServerConnectionString() 
		{ TimeOut = 30;
		}

		public SQLServerConnectionString(string strConnectionString)
		{ ConnectionString = strConnectionString;
		}

		public SQLServerConnectionString(string strServer, string strUser, string strPassword, string strDataBase) : this()
		{ Type = ConnectionType.Normal;
			Server = strServer;
			User = strUser;
			Password = strPassword;
			DataBase = strDataBase;
		}

		public SQLServerConnectionString(string strServer, string strDataBaseFile)
		{ Type = ConnectionType.File;
			Server = strServer;
			DataBaseFile = strDataBaseFile;
		}

		/// <summary>
		///		Tipo de conexión
		/// </summary>
		public ConnectionType Type { get; set; }

		/// <summary>
		///		Servidor
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		///		Indica si se debe utilizar seguridad integrada
		/// </summary>
		public bool UseIntegratedSecurity { get; set; }

		/// <summary>
		///		Usuario
		/// </summary>
		public string User { get; set; }

		/// <summary>
		///		Contraseña
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		///		Base de datos
		/// </summary>
		public string DataBase { get; set; }

		/// <summary>
		///		Archivo de base de datos
		/// </summary>
		public string DataBaseFile { get; set; }

		/// <summary>
		///		Tiempo de espera para la conexión
		/// </summary>
		public int TimeOut { get; set; }

		/// <summary>
		///		Cadena de conexión
		/// </summary>
		public string ConnectionString 
		{ get
				{ if (!string.IsNullOrEmpty(strConnectionString))
						return strConnectionString;
					else
						switch (Type)
							{ case ConnectionType.File:
									return string.Format("Data Source={0};AttachDbFilename=\"{1}\";Connect Timeout={2};Integrated Security={3};",
																			 Server, DataBaseFile, TimeOut, UseIntegratedSecurity);
								case ConnectionType.Normal:
									return string.Format("Server={0};Uid={1};Pwd={2};DataBase={3};Integrated Security={4};", Server, User, Password, DataBase, UseIntegratedSecurity);
								default:
									throw new LibDBProvidersBase.DBExceptions.DBException("Tipo de conexión desconocida");
							}
				}
			set { strConnectionString = value; }
		}
	}
}