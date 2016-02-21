using System;
using Microsoft.Win32;

namespace Bau.Libraries.LibHelper.API
{
	/// <summary>
	///		Clase con funciones �tiles de acceso al registro
	/// </summary>
	public class RegistryApi
	{ // Constantes privadas 
			private const string cnstStrWindowsRun = @"Software\Microsoft\Windows\CurrentVersion\Run";
		// Enumerados p�blicos
			/// <summary>
			///		Ra�z de una clave de registro
			/// </summary>
			public enum RegistryRoot
				{
					/// <summary>Almacena la informaci�n sobre las preferencias de usuario</summary>
					CurrentUser,
					/// <summary>Almacena la informaci�n de la m�quina local</summary>
					LocalMachine,
					/// <summary>Almacena informaci�n sobre tipos y clases y sus propiedades</summary>
					ClassesRoot,
					/// <summary>Almacena informaci�n sobre la configuraci�n predeterminada de usuario</summary>
					Users,
					/// <summary>Almacnea informaci�n para componentes de software</summary>
					PerformanceData,
					/// <summary>Almacena informaci�n sobre hardware no espec�fico del usuario</summary>
					CurrentConfig
				}

    /// <summary>
    ///		Asocia una extensi�n con un ejecutable y una acci�n
    /// </summary>
    public void LinkExtension(string strExtension, string strExecutableFileName, string strProgramId)
    {	LinkExtension(strExtension, strExecutableFileName, strProgramId, "open", "");
    }

    /// <summary>
    ///		Asocia una extensi�n con un ejecutable y una acci�n
    /// </summary>
    public void LinkExtension(string strExtension, string strExecutableFileName, string strProgramId,
																		 string strCommand)
    {	LinkExtension(strExtension, strExecutableFileName, strProgramId, strCommand, "");
    }

    /// <summary>
    ///		Asocia una extensi�n con un ejecutable y una acci�n
    /// </summary>
    public void LinkExtension(string strExtension, string strExecutableFileName, string strProgramId, 
																		 string strCommand, string strDescription)
    { string strLinkedProgramID;
      RegistryKey objRegistryKey = null;
      RegistryKey objRegistryKeyShell = null;

				// El comando predeterminado es open
					if (string.IsNullOrEmpty(strCommand))
						strCommand = "open";
				// Obtiene la descripci�n
					if (string.IsNullOrEmpty(strDescription))
						strDescription = strExtension + " Descripci�n de " + strProgramId;
				// Normaliza la extensi�n
					strExtension = NormalizeExtension(strExtension);
				// Obtiene el ID del programa a partir de la extensi�n
				  strLinkedProgramID = GetProgIdFromExtension(strExtension);
				// Si no hay nada asociado, se crean las claves, si hay algo asociado se modifican
				  if (string.IsNullOrEmpty(strLinkedProgramID) || strLinkedProgramID.Length == 0)
				    {	// Crear la clave con la extensi�n
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
				      // Si es un comando que se a�ade, no existir�
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
    ///		Permite quitar un comando asociado con una extensi�n
    /// </summary>
    public void DeleteLinkCommandExtension(string strExtension, string strCommand)
    { if (!string.IsNullOrEmpty(strCommand) && !strCommand.Equals("open", StringComparison.CurrentCultureIgnoreCase))
				{ string strProgramID;
				
						// Normaliza la extensi�n
							strExtension = NormalizeExtension(strExtension);
						// Obtiene el ID de programa de la extensi�n
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
    /// Quita la extensi�n indicada de los tipos de archivos registrados
    /// </summary>
    public void DeleteLinkProgramExtension(string strExtension)
    { string strProgramID;
    
				// Obtiene la extensi�n
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
    ///		Comprueba si la extensi�n indicada est� registrada.
    /// </summary>
    public bool ExistsLink(string strExtension)
    { string strProgramID = GetProgIdFromExtension(NormalizeExtension(strExtension));

        return !string.IsNullOrEmpty(strProgramID) && strProgramID.Length > 0;
    }

		/// <summary>
		///		A�ade un punto al inicio de una extensi�n
		/// </summary>
		private string NormalizeExtension(string strExtension)
		{ if (!strExtension.StartsWith("."))
				return "." + strExtension;
			else
				return strExtension;
		}
		
    /// <summary>
    ///		M�todo para obtener el ID de programa de una extensi�n
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
    /// A�ade al registro de Windows la aplicaci�n usando el t�tulo indicado
    /// </summary>
    public void AddToWindowsStart(string strTitle, string strExeFileName)
    {	using (RegistryKey objRegistryKey = Registry.LocalMachine.OpenSubKey(cnstStrWindowsRun, true))
				{	if(objRegistryKey != null)
						objRegistryKey.SetValue(strTitle, "\"" + strExeFileName + "\"");
        }
    }

    /// <summary>
    ///		Quita del registro de Windows la aplicaci�n relacionada con el t�tulo indicado
    /// </summary>
    public void RemoveFromWindowsStart(string strTitle)
    {	using (RegistryKey objRegistryKey = Registry.LocalMachine.OpenSubKey(cnstStrWindowsRun, true))
				{	if(objRegistryKey != null)
						objRegistryKey.DeleteValue(strTitle, false);
        }
    }

    /// <summary>
    ///		Comprueba si una aplicaci�n con el t�tulo indicado est� en el inicio de Windows
    /// </summary>
    public bool IsAtWindowsStart(string strTitle, out string strFileName)
    {	// Inicializa el valor de salida
				strFileName = null;
			// Obtiene el valor del registro
          using (RegistryKey objRegistryKey = Registry.LocalMachine.OpenSubKey(cnstStrWindowsRun, true))
						{	if(objRegistryKey != null)
								strFileName = objRegistryKey.GetValue(strTitle).ToString();
						}
			// Devuelve el valor que indica si est� en el inicio de Windows
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
						throw new NotImplementedException("No se encuentra la ra�z de la clave " + intRoot.ToString());
		}

		/// <summary>
		///		Obtiene la clave ra�z
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
