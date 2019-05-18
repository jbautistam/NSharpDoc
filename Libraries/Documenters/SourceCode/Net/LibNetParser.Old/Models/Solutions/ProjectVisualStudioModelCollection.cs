using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibNetParser.Models.Solutions
{
	/// <summary>
	///		Colección de <see cref="ProjectVisualStudioModel"/>
	/// </summary>
	public class ProjectVisualStudioModelCollection : List<ProjectVisualStudioModel>
	{
		/// <summary>
		///		Añade un proyecto a la colección
		/// </summary>
		public void Add(SolutionVisualStudioModel solution, string name, string fileName)
		{
			Add(new ProjectVisualStudioModel(solution, name, fileName));
		}

		/// <summary>
		///		Busca un proyecto por su nombre de archivo
		/// </summary>
		internal ProjectVisualStudioModel SearchByProjectName(string name)
		{ 
			// Busca el proyecto
			foreach (ProjectVisualStudioModel project in this)
				if (System.IO.Path.GetFileName(project.FullFileNameSource).EqualsIgnoreCase(name))
					return project;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}
	}
}
