using System;

namespace NSharpDocCompiler
{
	/// <summary>
	///		Compilador de documentación de NSharpDoc
	/// </summary>
	public class Program
	{
		public static void Main(string[] args)
		{ 
			// Línea de presentación
			Console.WriteLine("NSharpDoc Compiler");
			// Comprueba los argumentos
			if (args.Length == 0)
				Console.Error.WriteLine("Error: Escriba el nombre del archivo de proyecto a compilar (NSharpDocCompiler.exe <nombre de archivo>)");
			else
			{
				string fileName = args[0];

					if (string.IsNullOrEmpty(fileName))
						Console.Error.WriteLine("Error: Escriba el nombre del archivo de proyecto a compilar (NSharpDocCompiler.exe <nombre de archivo>)");
					else if (!System.IO.File.Exists(fileName))
						Console.Error.WriteLine($"Error: No se encuentra el archivo {fileName}");
					else
					{
						Bau.Libraries.LibNSharpDoc.Processor.NSharpDocManager manager = new Bau.Libraries.LibNSharpDoc.Processor.NSharpDocManager();

							try
							{ 
								// Genera el proyecto
								manager.Generate(fileName);
								// Muestra los errores
								if (manager.Errors.Count == 0)
									Console.WriteLine("Documentación generada correctamente");
								else
									foreach (string strError in manager.Errors)
										Console.Error.WriteLine(strError);
							}
							catch (Exception exception)
							{
								Console.Error.WriteLine($"Error en la generación de la documentación. {exception.Message}");
							}
				}
			}
			// Espera una tecla
			#if DEBUG
				Console.WriteLine("Pulse una tecla");
				Console.ReadLine();
			#endif
		}
	}
}
