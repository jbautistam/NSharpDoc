using System;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;

namespace Bau.Libraries.LibRoslynManager.Models.Solutions
{
	/// <summary>
	///		Colección de <see cref="ProjectVisualStudioModel"/>
	/// </summary>
	public class ProjectVisualStudioModelCollection : List<ProjectVisualStudioModel>
	{
		/// <summary>
		///		Añade un proyecto a la colección
		/// </summary>
		public void Add(SolutionVisualStudioModel objSolution, string strName, string strFileName)
		{ Add(new ProjectVisualStudioModel(objSolution, strName, strFileName));
		}

		/// <summary>
		///		Busca un proyecto por su nombre de archivo
		/// </summary>
		internal ProjectVisualStudioModel SearchByProjectName(string strName)
		{ // Busca el proyecto
				foreach (ProjectVisualStudioModel objProject in this)
					if (System.IO.Path.GetFileName(objProject.FullFileNameSource).EqualsIgnoreCase(strName))
						return objProject;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
				return null;
		}
	}
}
