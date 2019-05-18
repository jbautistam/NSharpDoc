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
	{ 
		// Constantes privadas
		private const string NoNameSpace = "NoNameSpace";

		/// <summary>
		///		Genera los grupos de espacios de nombres asociados a un programa
		/// </summary>
		internal NameSpaceGroupModelCollection Generate(ProgramModel program)
		{
			NameSpaceGroupModelCollection groups = GroupNameSpaces(program);

				// Añade las estructuras adicionales a los grupos
				AddStructsToGroups(groups, program);
				// Devuelve la colección de grupos
				return groups;
		}

		/// <summary>
		///		Agrupa los espacios de nombres de un programa
		/// </summary>
		private NameSpaceGroupModelCollection GroupNameSpaces(ProgramModel program)
		{
			NameSpaceGroupModelCollection groups = new NameSpaceGroupModelCollection();

				// Genera la documentación
				foreach (CompilationUnitModel compilationUnit in program.CompilationUnits)
				{ 
					// Añade los espacios de nombres
					AddNameSpacesToGroup(groups, compilationUnit);
					// Añade los elementos sin espacio de nombres
					AddStructsNoNameSpace(groups, compilationUnit);
				}
				// Devuelve los grupos
				return groups;
		}

		/// <summary>
		///		Procesa los elementos de documentación
		/// </summary>
		private void AddNameSpacesToGroup(NameSpaceGroupModelCollection groups, CompilationUnitModel compilationUnit)
		{
			foreach (NameSpaceModel nameSpace in compilationUnit.SearchNameSpaces())
				if (nameSpace != null)
					groups.Add(nameSpace);
		}

		/// <summary>
		///		Añade las estructuras que no tienen un espacio de nombres
		/// </summary>
		private void AddStructsNoNameSpace(NameSpaceGroupModelCollection groups, CompilationUnitModel compilationUnit)
		{
			LanguageStructModelCollection structsDoc = compilationUnit.SearchNoNameSpaces();

				if (structsDoc != null && structsDoc.Count > 0)
				{
					NameSpaceGroupModel group = GetGroupNoNameSpace(groups);

						// Añade las estructuras
						foreach (LanguageStructModel structDoc in structsDoc)
							group.NameSpace.Items.Add(structDoc);
				}
		}

		/// <summary>
		///		Añade las estructuras a los grupos
		/// </summary>
		private void AddStructsToGroups(NameSpaceGroupModelCollection groups, ProgramModel program)
		{
			foreach (CompilationUnitModel compilationUnit in program.CompilationUnits)
				foreach (LanguageStructModel structDoc in compilationUnit.Root.Items)
					if (structDoc is NameSpaceModel)
					{
						NameSpaceModel nameSpace = structDoc as NameSpaceModel;
						NameSpaceGroupModel group = groups.Search(nameSpace.Name);

							if (group != null)
								foreach (LanguageStructModel child in nameSpace.Items)
								{ 
									// Si no se ha añadido antes este espacio de nombres, se crea
									if (group.NameSpace == null)
										group.NameSpace = nameSpace;
									// Añade el elemento hijo
									AddStructToNameSpace(group.NameSpace, child);
								}
					}
					else
					{
						NameSpaceGroupModel group = GetGroupNoNameSpace(groups);

							if (group != null)
								AddStructToNameSpace(group.NameSpace, structDoc);
					}
		}

		/// <summary>
		///		Añade una estructura a un espacio de nombres teniendo en cuenta que si es una clase pueden ser clases parciales
		///	y si es un método puede estar sobreescrito
		/// </summary>
		private void AddStructToNameSpace(NameSpaceModel nameSpace, LanguageStructModel structDoc)
		{
			LanguageStructModel previous = nameSpace.Items.SearchByName(structDoc);

				// Si no existía se añade
				if (previous == null)
					nameSpace.Items.Add(structDoc);
		}

		/// <summary>
		///		Obtiene el grupo de espacios de nombres para las clases sin espacio de nombres
		/// </summary>
		private NameSpaceGroupModel GetGroupNoNameSpace(NameSpaceGroupModelCollection groups)
		{
			NameSpaceGroupModel group = groups.Search(NoNameSpace);

				// Si no se ha encontrado el espacio de nombres se añade
				if (group == null)
				{ 
					// Crea el espacio de nombres
					group = new NameSpaceGroupModel(null, NoNameSpace);
					// Añade el espacio de nombres a la colección
					groups.Add(group);
					// Crea el símbolo del espacio de nombres
					group.NameSpace = new NameSpaceModel(null);
					group.NameSpace.Name = NoNameSpace;
				}
				// Devuelve el grupo
				return group;
		}
	}
}
