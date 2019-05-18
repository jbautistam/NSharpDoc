using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base
{
	/// <summary>
	///		Colección de <see cref="LanguageStructModel"/>
	/// </summary>
	public class LanguageStructModelCollection : List<LanguageStructModel>
	{
		/// <summary>
		///		Añade un elemento a la colección
		/// </summary>
		public LanguageStructModel Add(LanguageStructModel.StructType typeId, LanguageStructModel parent)
		{
			return AddItem(new LanguageStructModel(typeId, parent));
		}

		/// <summary>
		///		Crea un espacio de nombres
		/// </summary>
		public Structs.NameSpaceModel CreateSpaceModel(LanguageStructModel parent)
		{
			return AddItem(new Structs.NameSpaceModel(parent));
		}

		/// <summary>
		///		Crea una clase
		/// </summary>
		public Structs.ClassModel CreateClass(LanguageStructModel parent)
		{
			return AddItem(new Structs.ClassModel(parent));
		}

		/// <summary>
		///		Crea una estructura
		/// </summary>
		public Structs.StructModel CreateStruct(LanguageStructModel parent)
		{
			return AddItem(new Structs.StructModel(parent));
		}

		/// <summary>
		///		Crea un interface
		/// </summary>
		public Structs.InterfaceModel CreateInterface(LanguageStructModel parent)
		{
			return AddItem(new Structs.InterfaceModel(parent));
		}

		/// <summary>
		///		Crea un método
		/// </summary>
		public Methods.MethodModel CreateMethod(LanguageStructModel parent)
		{
			return AddItem(new Methods.MethodModel(parent));
		}

		/// <summary>
		///		Crea un enumerado
		/// </summary>
		public Simple.EnumModel CreateEnum(LanguageStructModel parent)
		{
			return AddItem(new Simple.EnumModel(parent));
		}

		/// <summary>
		///		Crea un miembro de un enumerado
		/// </summary>
		public Simple.EnumMemberModel CreateEnumMember(LanguageStructModel parent)
		{
			return AddItem(new Simple.EnumMemberModel(parent));
		}

		/// <summary>
		///		Crea una propiedad
		/// </summary>
		public Methods.PropertyModel CreateProperty(LanguageStructModel parent)
		{
			return AddItem(new Methods.PropertyModel(parent));
		}

		/// <summary>
		///		Crea un constructor
		/// </summary>
		public Methods.ConstructorModel CreateConstructor(LanguageStructModel parent)
		{
			return AddItem(new Methods.ConstructorModel(parent));
		}

		/// <summary>
		///		Añade y devuelve un elemento
		/// </summary>
		private TypeData AddItem<TypeData>(TypeData item) where TypeData : LanguageStructModel
		{ 
			// Añade el elemento
			Add(item);
			// Devuelve el elemento
			return item;
		}

		/// <summary>
		///		Comprueba si existe una estructura por su nombre y tipo
		/// </summary>
		public bool ExistsByName(LanguageStructModel structDoc)
		{
			return SearchByName(structDoc) != null;
		}

		/// <summary>
		///		Obtiene un elemento por su nombre
		/// </summary>
		public LanguageStructModel SearchByName(LanguageStructModel structDoc)
		{
			return this.FirstOrDefault(item => item.Type == structDoc.Type && item.Name.EqualsIgnoreCase(structDoc.Name));
		}

		/// <summary>
		///		Ordena las estructuras por nombre
		/// </summary>
		public void SortByName()
		{
			Sort((first, second) => first.Name.CompareTo(second.Name));
		}
	}
}
