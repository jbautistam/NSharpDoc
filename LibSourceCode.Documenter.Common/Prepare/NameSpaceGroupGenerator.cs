using System;
using System.Collections.Generic;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs;
using Bau.Libraries.LibSourceCode.Models.Groups;

namespace Bau.Libraries.LibSourceCode.Documenter.Common.Prepare
{
	/// <summary>
	///		Generador de un grupo de espacios de nombres
	/// </summary>
	internal class NameSpaceGroupGenerator
	{ // Constantes privadas
			private const string cnstStrNoNameSpace = "NoNameSpace";

		/// <summary>
		///		Genera los grupos de espacios de nombres asociados a un programa
		/// </summary>
		internal NameSpaceGroupModelCollection Generate(ProgramModel objProgram)
		{ NameSpaceGroupModelCollection objColGroups = GroupNameSpaces(objProgram);

				// Añade las estructuras adicionales a los grupos
					AddStructsToGroups(objColGroups, objProgram);
				// Devuelve la colección de grupos
					return objColGroups;
		}

		/// <summary>
		///		Agrupa los espacios de nombres de un programa
		/// </summary>
		private NameSpaceGroupModelCollection GroupNameSpaces(ProgramModel objProgram)
		{ NameSpaceGroupModelCollection objColGroups = new NameSpaceGroupModelCollection();

				// Genera la documentación
					foreach (CompilationUnitModel objCompilationUnit in objProgram.CompilationUnits)
						{ // Añade los espacios de nombres
								AddNameSpacesToGroup(objColGroups, objCompilationUnit);
							// Añade los elementos sin espacio de nombres
								AddStructsNoNameSpace(objColGroups, objCompilationUnit);
						}
				// Devuelve los grupos
					return objColGroups;
		}

		/// <summary>
		///		Procesa los elementos de documentación
		/// </summary>
		private void AddNameSpacesToGroup(NameSpaceGroupModelCollection objColGroups, CompilationUnitModel objCompilationUnit)
		{ List<NameSpaceModel> objColNameSpaces = objCompilationUnit.SearchNameSpaces();

				// Añade los espacios de nombres al documento
					foreach (NameSpaceModel objNameSpace in objColNameSpaces)
						if (objNameSpace != null)
							objColGroups.Add(objNameSpace);
		}

		/// <summary>
		///		Añade las estructuras que no tienen un espacio de nombres
		/// </summary>
		private void AddStructsNoNameSpace(NameSpaceGroupModelCollection objColGroups, CompilationUnitModel objCompilationUnit)
		{ LanguageStructModelCollection objColStructs = objCompilationUnit.SearchNoNameSpaces();

				if (objColStructs != null && objColStructs.Count > 0)
					{	NameSpaceGroupModel objGroup = GetGroupNoNameSpace(objColGroups); 

							// Añade las estructuras
								foreach (LanguageStructModel objStruct in objColStructs)
									objGroup.NameSpace.Items.Add(objStruct);
					}
		}

		/// <summary>
		///		Añade las estructuras a los grupos
		/// </summary>
		private void AddStructsToGroups(NameSpaceGroupModelCollection objColGroups, ProgramModel objProgram)
		{ foreach (CompilationUnitModel objCompilationUnit in objProgram.CompilationUnits)
				foreach (LanguageStructModel objStruct in objCompilationUnit.Root.Items)
					if (objStruct is NameSpaceModel)
						{ NameSpaceModel objNameSpace = objStruct as NameSpaceModel;
							NameSpaceGroupModel objGroup = objColGroups.Search(objNameSpace.Name);

								if (objGroup != null)
									foreach (LanguageStructModel objChild in objNameSpace.Items)
										{ // Si no se ha añadido antes este espacio de nombres, se crea
												if (objGroup.NameSpace == null)
													objGroup.NameSpace = objNameSpace;
											// Añade el elemento hijo
												AddStructToNameSpace(objGroup.NameSpace, objChild);
										}
						}
					else
						{ NameSpaceGroupModel objGroup = GetGroupNoNameSpace(objColGroups);

								if (objGroup != null)
									AddStructToNameSpace(objGroup.NameSpace, objStruct);
						}
		}

		/// <summary>
		///		Añade una estructura a un espacio de nombres teniendo en cuenta que si es una clase pueden ser clases parciales
		///	y si es un método puede estar sobreescrito
		/// </summary>
		private void AddStructToNameSpace(NameSpaceModel objNameSpace, LanguageStructModel objStruct)
		{ LanguageStructModel objPrevious = objNameSpace.Items.SearchByName(objStruct);

				// Si no existía se añade
					if (objPrevious == null)
						objNameSpace.Items.Add(objStruct);
		}

		/// <summary>
		///		Obtiene el grupo de espacios de nombres para las clases sin espacio de nombres
		/// </summary>
		private NameSpaceGroupModel GetGroupNoNameSpace(NameSpaceGroupModelCollection objColGroups)
		{ NameSpaceGroupModel objGroup = objColGroups.Search(cnstStrNoNameSpace);

				// Si no se ha encontrado el espacio de nombres se añade
					if (objGroup == null)
						{ // Crea el espacio de nombres
								objGroup = new NameSpaceGroupModel(null, cnstStrNoNameSpace);
							// Añade el espacio de nombres a la colección
								objColGroups.Add(objGroup);
							// Crea el símbolo del espacio de nombres
								objGroup.NameSpace = new NameSpaceModel(null);
								objGroup.NameSpace.Name = cnstStrNoNameSpace;
						}
				// Devuelve el grupo
					return objGroup;
		}
	}
}
