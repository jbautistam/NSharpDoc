using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibHelper.Extensors;

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
		public LanguageStructModel Add(LanguageStructModel.StructType intIDType, LanguageStructModel objParent)
		{ return AddItem(new LanguageStructModel(intIDType, objParent));
		}

		/// <summary>
		///		Crea un espacio de nombres
		/// </summary>
		public Structs.NameSpaceModel CreateSpaceModel(LanguageStructModel objParent)
		{ return AddItem(new Structs.NameSpaceModel(objParent));
		}

		/// <summary>
		///		Crea una clase
		/// </summary>
		public Structs.ClassModel CreateClass(LanguageStructModel objParent)
		{ return AddItem(new Structs.ClassModel(objParent));
		}

		/// <summary>
		///		Crea una estructura
		/// </summary>
		public Structs.StructModel CreateStruct(LanguageStructModel objParent)
		{ return AddItem(new Structs.StructModel(objParent));
		}

		/// <summary>
		///		Crea un interface
		/// </summary>
		public Structs.InterfaceModel CreateInterface(LanguageStructModel objParent)
		{ return AddItem(new Structs.InterfaceModel(objParent));
		}

		/// <summary>
		///		Crea un método
		/// </summary>
		public Methods.MethodModel CreateMethod(LanguageStructModel objParent)
		{ return AddItem(new Methods.MethodModel(objParent));
		}

		/// <summary>
		///		Crea un enumerado
		/// </summary>
		public Simple.EnumModel CreateEnum(LanguageStructModel objParent)
		{ return AddItem(new Simple.EnumModel(objParent));
		}

		/// <summary>
		///		Crea un miembro de un enumerado
		/// </summary>
		public Simple.EnumMemberModel CreateEnumMember(LanguageStructModel objParent)
		{ return AddItem(new Simple.EnumMemberModel(objParent));
		}

		/// <summary>
		///		Crea una propiedad
		/// </summary>
		public Methods.PropertyModel CreateProperty(LanguageStructModel objParent)
		{ return AddItem(new Methods.PropertyModel(objParent));
		}

		/// <summary>
		///		Crea un constructor
		/// </summary>
		public Methods.ConstructorModel CreateConstructor(LanguageStructModel objParent)
		{ return AddItem(new Methods.ConstructorModel(objParent));
		}

		/// <summary>
		///		Añade y devuelve un elemento
		/// </summary>
		private TypeData AddItem<TypeData>(TypeData objItem) where TypeData : LanguageStructModel
		{ // Añade el elemento
				Add(objItem);
			// Devuelve el elemento
				return objItem;
		}

		/// <summary>
		///		Comprueba si existe una estructura por su nombre y tipo
		/// </summary>
		public bool ExistsByName(LanguageStructModel objStruct)
		{ return SearchByName(objStruct) != null;
		}

		/// <summary>
		///		Obtiene un elemento por su nombre
		/// </summary>
		public LanguageStructModel SearchByName(LanguageStructModel objStruct)
		{ return this.FirstOrDefault(objItem => objItem.IDType == objStruct.IDType && 
																						objItem.Name.EqualsIgnoreCase(objStruct.Name));
		}

		/// <summary>
		///		Ordena las estructuras por nombre
		/// </summary>
		public void SortByName()
		{ Sort((objFirst, objSecond) => objFirst.Name.CompareTo(objSecond.Name));
		}
	}
}
