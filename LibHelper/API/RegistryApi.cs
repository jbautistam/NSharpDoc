using System;
using Microsoft.Win32;

namespace Bau.Libraries.LibHelper.API
{
	/// <summary>
	///		Clase con funciones útiles de acceso al registro
	/// </summary>
	public class RegistryApi
	{ // Constantes privadas 
			private const string cnstStrWindowsRun = @"Software\Microsoft\Windows\CurrentVersion\Run";
		// Enumerados públicos
			/// <summary>
			///		Raíz de una clave de registro
			/// </summary>
			public enum RegistryRoot
				{
					/// <summary>Almacena la información sobre las preferencias de usuario</summary>
					CurrentUser,
					/// <summary>Almacena la información de la máquina local</summary>
					LocalMachine,
					/// <summary>Almacena información sobre tipos y clases y sus propiedades</summary>
					ClassesRoot,
					/// <summary>Almacena información sobre la configuración predeterminada de usuario</summary>
					Users,
					/// <summary>Almacnea información para componentes de software</summary>
					PerformanceData,
					/// <summary>Almacena información sobre hardware no específico del usuario</summary>
					CurrentConfig
				}

    /// <summary>
    ///		Asocia una extensión con un ejecutable y una acción
    /// </summary>
    public void LinkExtension(string strExtension, string strExecutableFileName, string strProgramId)
    {	LinkExtension(strExtension, strExecutableFileName, strProgramId, "open", "");
    }

    /// <summary>
    ///		Asocia una extensión con un ejecutable y una acción
    /// </summary>
    public void LinkExtension(string strExtension, string strExecutableFileName, string strProgramId,
																		 string strCommand)
    {	LinkExtension(strExtension, strExecutableFileName, strProgramId, strCommand, "");
    }

    /// <summary>
    ///		Asocia una extensión con un ejecutable y una acción
    /// </summary>
    public void LinkExtension(string strExtension, string strExecutableFileName, string strProgramId, 
																		 string strCommand, string strDescription)
    { string strLinkedProgramID;
      RegistryKey objRegistryKey = null;
      RegistryKey objRegistryKeyShell = null;

				// El comando predeterminado es open
					if (string.IsNullOrEmpty(strCommand))
						strCommand = "open";
				// Obtiene la descripción
					if (string.IsNullOrEmpty(strDescription))
						strDescription = strExtension + " Descripción de " + strProgramId;
				// Normaliza la extensión
					strExtension = NormalizeExtension(strExtension);
				// Obtiene el ID del programa a partir de la extensión
				  strLinkedProgramID = GetProgIdFromExtension(strExtension);
				// Si no hay nada asociado, se crean las claves, si hay algo asociado se modifican
				  if (string.IsNullOrEmpty(strLinkedProgramID) || strLinkedProgramID.Length == 0)
				    {	// Crear la clave con la extensión
				        objRegistryKey = Registry.ClassesRoot.CreateSubKey(strExtension);
				        objRegistryKey.SetValue("", strProgramId);
				      // Crea la calve con el programa
				        objRegistryKey = Registry.ClassesRoot.CreateSubKey(strProgramId);
				        objRegistryKey.SetValue("", strDescription);
				      // Crea la clave con el comando
				        objRegistryKeyShell = objRegistryKey.CreateSubKey("shell\\" + strCommand + "\\command");
				    }
				  else
				    {	// Abrimos la clave clave indicando que vamos a escribir para que nos permita crear nuevas subclaves.
				        objRegistryKey = Registry.ClassesRoot.OpenSubKey(strLinkedProgramID, true);
				        objRegistryKeyShell = objRegistryKey.OpenSubKey("shell\\" + strCommand + "\\command", true);
				      // Si es un comando que se añade, no existirá
				        if (objRegistryKeyShell == null)
				          objRegistryKeyShell = objRegistryKey.CreateSubKey(strProgramId);
				    }
				  // Si tenemos la clave de registro del Shell
				    if (objRegistryKeyShell != null)
				      {	objRegistryKeyShell.SetValue("", "\"" + strExecutableFileName + "\" \"%1\"");
				        objRegistryKeyShell.Close();
				      }
    }

    /// <summary>
    ///		Permite quitar un comando asociado con una extensión
    /// </summary>
    public void DeleteLinkCommandExtension(string strExtension, string strCommand)
    { if (!string.IsNullOrEmpty(strCommand) && !strCommand.Equals("open", StringComparison.CurrentCultureIgnoreCase))
				{ string strProgramID;
				
						// Normaliza la extensión
							strExtension = NormalizeExtension(strExtension);
						// Obtiene el ID de programa de la extensión
							strProgramID = GetProgIdFromExtension(strExtension);
						// Elimina la clave del registro
            if (!string.IsNullOrEmpty(strProgramID) && strProgramID.Length > 0)
							using(RegistryKey objRegistryKey = Registry.ClassesRoot.OpenSubKey(strProgramID, true))
                {	if (objRegistryKey != null)
                    objRegistryKey.DeleteSubKeyTree("shell\\" + strCommand);
                }
        }
    }

    /// <summary>
    /// Quita la extensión indicada de los tipos de archivos registrados
    /// </summary>
    public void DeleteLinkProgramExtension(string strExtension)
    { string strProgramID;
    
				// Obtiene la extensión
					strExtension = NormalizeExtension(strExtension);
				// Obtiene el ID del programa
					strProgramID = GetProgIdFromExtension(strExtension);
				// Elimina las claves
					if (!string.IsNullOrEmpty(strProgramID) && strProgramID.Length > 0)
            {	// Elimina la subclave
                Registry.ClassesRoot.DeleteSubKeyTree(strExtension);
							// Elimina la clave
                Registry.ClassesRoot.DeleteSubKeyTree(strProgramID);
            }
    }

    /// <summary>
    ///		Comprueba si la extensión indicada está registrada.
    /// </summary>
    public bool ExistsLink(string strExtension)
    { string strProgramID = GetProgIdFromExtension(NormalizeExtension(strExtension));

        return !string.IsNullOrEmpty(strProgramID) && strProgramID.Length > 0;
    }

		/// <summary>
		///		Añade un punto al inicio de una extensión
		/// </summary>
		private string NormalizeExtension(string strExtension)
		{ if (!strExtension.StartsWith("."))
				return "." + strExtension;
			else
				return strExtension;
		}
		
    /// <summary>
    ///		Método para obtener el ID de programa de una extensión
    /// </summary>
    public string GetProgIdFromExtension(string strExtension)
    { string strProgramID = "";

				// Obtiene el ID del prgorama
          using (RegistryKey objRegistryKey = Registry.ClassesRoot.OpenSubKey(strExtension))
						{	if (objRegistryKey != null && objRegistryKey.GetValue("") != null)
								{	// Obtiene el ID
										strProgramID = objRegistryKey.GetValue("").ToString();
									// Cierra la clave
										objRegistryKey.Close();
								}
						}
				// Devuelve el ID del programa
					return strProgramID;
    }

    /// <summary>
    /// Añade al registro de Windows la aplicación usando el título indicado
    /// </summary>
    public void AddToWindowsStart(string strTitle, string strExeFileName)
    {	using (RegistryKey objRegistryKey = Registry.LocalMachine.OpenSubKey(cnstStrWindowsRun, true))
				{	if(objRegistryKey != null)
						objRegistryKey.SetValue(strTitle, "\"" + strExeFileName + "\"");
        }
    }

    /// <summary>
    ///		Quita del registro de Windows la aplicación relacionada con el título indicado
    /// </summary>
    public void RemoveFromWindowsStart(string strTitle)
    {	using (RegistryKey objRegistryKey = Registry.LocalMachine.OpenSubKey(cnstStrWindowsRun, true))
				{	if(objRegistryKey != null)
						objRegistryKey.DeleteValue(strTitle, false);
        }
    }

    /// <summary>
    ///		Comprueba si una aplicación con el título indicado está en el inicio de Windows
    /// </summary>
    public bool IsAtWindowsStart(string strTitle, out string strFileName)
    {	// Inicializa el valor de salida
				strFileName = null;
			// Obtiene el valor del registro
          using (RegistryKey objRegistryKey = Registry.LocalMachine.OpenSubKey(cnstStrWindowsRun, true))
						{	if(objRegistryKey != null)
								strFileName = objRegistryKey.GetValue(strTitle).ToString();
						}
			// Devuelve el valor que indica si está en el inicio de Windows
				return !string.IsNullOrEmpty(strFileName);
    }

		/// <summary>
		///		Asigna un valor a un elemento
		/// </summary>
		public void SetValue(RegistryRoot intRoot, string strKeyName, string strValueName, object objValue, RegistryValueKind intIDType)
		{	RegistryKey objRegistryRoot = GetRegistryRoot(intRoot);

				// Crea la clave
					if (objRegistryRoot != null)
						{ RegistryKey objKey = objRegistryRoot.CreateSubKey(strKeyName);

								// Asigna el valor
									if (objKey != null)
										Registry.SetValue(objKey.Name, strValueName, objValue, intIDType);
									else
										throw new NotSupportedException("No se ha podido crea la subclave: " + strKeyName);
						}
					else
						throw new NotImplementedException("No se encuentra la raíz de la clave " + intRoot.ToString());
		}

		/// <summary>
		///		Obtiene la clave raíz
		/// </summary>
		private RegistryKey GetRegistryRoot(RegistryRoot intRoot)
		{ switch (intRoot)
				{	case RegistryRoot.ClassesRoot:
						return Registry.ClassesRoot;
					case RegistryRoot.CurrentConfig:
						return Registry.CurrentConfig;
					case RegistryRoot.CurrentUser:
						return Registry.CurrentUser;
					case RegistryRoot.LocalMachine:
						return Registry.LocalMachine;
					case RegistryRoot.PerformanceData:
						return Registry.PerformanceData;
					case RegistryRoot.Users:
					default:
						return null;
				}
		}
	}
}
