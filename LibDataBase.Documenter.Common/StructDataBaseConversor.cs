using System;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibDBSchema;
using Bau.Libraries.LibDBSchema.DataSchema;

namespace Bau.Libraries.LibDataBase.Documenter.Common
{
	/// <summary>
	///		Conversor a una serie de estructuras la información de esquema de una base de datos
	/// </summary>
	public class StructDataBaseConversor
	{
		/// <summary>
		///		Convierte un esquema en una estructura de documentación
		/// </summary>
		public StructDocumentationModel Convert(Schema objSchema)
		{ StructDocumentationModel objStruct = new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global,
																																				objSchema.DataBase, "DataBase", 0);

				// Añade los parámetros a la estructura
					objStruct.Parameters.Add("DataBase", objSchema.DataBase);
					objStruct.Parameters.Add("Server", objSchema.Server);
				// Convierte el esquema
					objStruct.Childs.AddRange(ConvertTables(objStruct, objSchema));
					objStruct.Childs.AddRange(ConvertViews(objStruct, objSchema.Views));
					objStruct.Childs.AddRange(ConvertProcedures(objStruct, objSchema.Routines));
				// Devuelve las estructuras convertidas
					return objStruct;
		}

		/// <summary>
		///		Convierte las tablas
		/// </summary>
		private StructDocumentationModelCollection ConvertTables(StructDocumentationModel objParent, Schema objSchema)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Crea la estructura de las tablas
					foreach (SchemaTable objTable in objSchema.Tables)
						{ StructDocumentationModel objStruct = CreateStruct(objParent, objTable, "Table");

								// Añade los parámetros de la tabla
									objStruct.Parameters.Add("Summary", objTable.Description);
									objStruct.Parameters.Add("Type", objTable.Type.ToString());
									objStruct.Parameters.Add("DateCreate", Format(objTable.DateCreate));
									objStruct.Parameters.Add("DateUpdate", Format(objTable.DateUpdate));
								// Añade las columnas, restricciones, triggers...
									objStruct.Childs.AddRange(ConvertColumns(objStruct, objTable.Columns, objTable));
									objStruct.Childs.AddRange(ConvertConstrainst(objStruct, objTable.Constraints));
									objStruct.Childs.AddRange(ConvertTriggers(objParent, objSchema.Triggers.SearchByTable(objTable.Name)));
								// Añade la estructura a la colección
									objColStructs.Add(objStruct);
						}
				// Devuelve la colección de estructuras
					return objColStructs;
		}

		/// <summary>
		///		Convierte las vistas
		/// </summary>
		private StructDocumentationModelCollection ConvertViews(StructDocumentationModel objParent, SchemaViewsCollection objColViews)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Crea la estructura de las vistas
					foreach (SchemaView objView in objColViews)
						{ StructDocumentationModel objStruct = CreateStruct(objParent, objView, "View");

								// Añade los parámetros de la tabla
									objStruct.Parameters.Add("Prototype", objView.Definition);
									objStruct.Parameters.Add("CheckOption", objView.CheckOption);
									objStruct.Parameters.Add("IsUpdatable", objView.IsUpdatable);
								// Añade las columnas
									objStruct.Childs.AddRange(ConvertColumns(objStruct, objView.Columns, null));
								// Añade la estructura a la colección
									objColStructs.Add(objStruct);
						}
				// Devuelve la colección de estructuras
					return objColStructs;
		}

		/// <summary>
		///		Convierta las restricciones
		/// </summary>
		private StructDocumentationModelCollection ConvertConstrainst(StructDocumentationModel objParent, SchemaConstraintsCollection objColConstraints)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Convierte las restricciones
					foreach (SchemaConstraint objConstraint in objColConstraints)
						{ StructDocumentationModel objStruct = CreateStruct(objParent, objConstraint, "Constraint");

								// Añade los parámetros
									objStruct.Parameters.Add("Table", objConstraint.Table);
									objStruct.Parameters.Add("Column", objConstraint.Column);
									objStruct.Parameters.Add("Type", objConstraint.Type.ToString());
									objStruct.Parameters.Add("Position", objConstraint.Position);
								// Añade la estructura a la colección
									objColStructs.Add(objStruct);
						}
				// Devuelve la documentación
					return objColStructs;
		}

		/// <summary>
		///		Convierte los procedimientos
		/// </summary>
		private StructDocumentationModelCollection ConvertProcedures(StructDocumentationModel objParent, SchemaRoutinesCollection objColRoutines)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Crea la estructura de las rutinas
					foreach (SchemaRoutine objRoutine in objColRoutines)
						{ StructDocumentationModel objStruct = CreateStruct(objParent, objRoutine, objRoutine.Type.ToString());

								// Añade los parámetros de la rutina
									objStruct.Parameters.Add("Summary", "");
									objStruct.Parameters.Add("Prototype", objRoutine.Definition);
								// Añade la estructura a la colección
									objColStructs.Add(objStruct);
						}
				// Devuelve la colección de estructuras
					return objColStructs;
		}

		/// <summary>
		///		Convierte los desencadenadores
		/// </summary>
		private StructDocumentationModelCollection ConvertTriggers(StructDocumentationModel objParent, SchemaTriggersCollection objColTriggers)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Añade los desencadenadores
					foreach (SchemaTrigger objTrigger in objColTriggers)
						{ StructDocumentationModel objStruct = CreateStruct(objParent, objTrigger, "Trigger");

								// Añade los parámetros de la tabla
									objStruct.Parameters.Add("Table", objTrigger.Table);
									objStruct.Parameters.Add("UserName", objTrigger.UserName);
									objStruct.Parameters.Add("Category", objTrigger.Category);
									objStruct.Parameters.Add("IsExecuted", objTrigger.IsExecuted);
									objStruct.Parameters.Add("IsExecutionAnsiNullsOn", objTrigger.IsExecutionAnsiNullsOn);
									objStruct.Parameters.Add("IsExecutionQuotedIdentOn", objTrigger.IsExecutionQuotedIdentOn);
									objStruct.Parameters.Add("IsAnsiNullsOn", objTrigger.IsAnsiNullsOn);
									objStruct.Parameters.Add("IsQuotedIdentOn", objTrigger.IsQuotedIdentOn);
									objStruct.Parameters.Add("IsExecutionAfterTrigger", objTrigger.IsExecutionAfterTrigger);
									objStruct.Parameters.Add("IsExecutionDeleteTrigger", objTrigger.IsExecutionDeleteTrigger);
									objStruct.Parameters.Add("IsExecutionFirstDeleteTrigger", objTrigger.IsExecutionFirstDeleteTrigger);
									objStruct.Parameters.Add("IsExecutionFirstInsertTrigger", objTrigger.IsExecutionFirstInsertTrigger);
									objStruct.Parameters.Add("IsExecutionFirstUpdateTrigger", objTrigger.IsExecutionFirstUpdateTrigger);
									objStruct.Parameters.Add("IsExecutionInsertTrigger", objTrigger.IsExecutionInsertTrigger);
									objStruct.Parameters.Add("IsExecutionInsteadOfTrigger", objTrigger.IsExecutionInsteadOfTrigger);
									objStruct.Parameters.Add("IsExecutionLastDeleteTrigger", objTrigger.IsExecutionLastDeleteTrigger);
									objStruct.Parameters.Add("IsExecutionLastInsertTrigger", objTrigger.IsExecutionLastInsertTrigger);
									objStruct.Parameters.Add("IsExecutionLastUpdateTrigger", objTrigger.IsExecutionLastUpdateTrigger);
									objStruct.Parameters.Add("IsExecutionTriggerDisabled", objTrigger.IsExecutionTriggerDisabled);
									objStruct.Parameters.Add("IsExecutionUpdateTrigger", objTrigger.IsExecutionUpdateTrigger);
									objStruct.Parameters.Add("DateCreate", objTrigger.DateCreate);
									objStruct.Parameters.Add("DateReference", objTrigger.DateReference);
									objStruct.Parameters.Add("Prototype", objTrigger.Content);
								// Añade la estructura
									objColStructs.Add(objStruct);
						}
				// Devuelve la colección de estructuras
					return objColStructs;
		}

		/// <summary>
		///		Convierta las columnas
		/// </summary>
		private StructDocumentationModelCollection ConvertColumns(StructDocumentationModel objParent, SchemaColumnsCollection objColColumns, SchemaTable objTable)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Ordena las columnas
					objColColumns.SortByOrdinalPosition(); 
				// Añade la información de las columnas
					foreach (SchemaColumn objColumn in objColColumns)
						{ StructDocumentationModel objStruct = CreateStruct(objParent, objColumn, "Column");

								// Añade los parámetros de la columna
									objStruct.Parameters.Add("Summary", objColumn.Description);
									objStruct.Parameters.Add("Default", objColumn.Default);
									objStruct.Parameters.Add("IsNullable", objColumn.IsNullable);
									objStruct.Parameters.Add("DataType", objColumn.DataType);
									objStruct.Parameters.Add("CharacterMaximumLength", objColumn.CharacterMaximumLength);
									objStruct.Parameters.Add("NumericPrecision", objColumn.NumericPrecision);
									objStruct.Parameters.Add("NumericPrecisionRadix", objColumn.NumericPrecisionRadix);
									objStruct.Parameters.Add("NumericScale", objColumn.NumericScale);
									objStruct.Parameters.Add("DateTimePrecision", objColumn.DateTimePrecision);
									objStruct.Parameters.Add("CharacterSetName", objColumn.CharacterSetName);
									objStruct.Parameters.Add("CollationCatalog", objColumn.CollationCatalog);
									objStruct.Parameters.Add("CollationSchema", objColumn.CollationSchema);
									objStruct.Parameters.Add("CollationName", objColumn.CollationName);
									objStruct.Parameters.Add("IsIdentity", objColumn.IsIdentity);
								// Obtiene el valor que indica si es una tabla foránea
									if (objTable != null)
										objStruct.Parameters.Add("IsForeignKey", CheckIsForeignKey(objTable, objColumn));
								// Obtiene el tipo
									objStruct.Parameters.Add("Type", GetDataType(objColumn));
								// Añade la estructura a la colección
									objColStructs.Add(objStruct);
						}
				// Devuelve la colección
					return objColStructs;
		}

		/// <summary>
		///		Comprueba si una columna es una clave foránea
		/// </summary>
		private bool CheckIsForeignKey(SchemaTable objTable, SchemaColumn objColumn)
		{ // Recorre las restricciones comprobando si es una clave foránea
				foreach (SchemaConstraint objConstraint in objTable.Constraints)
					if (objConstraint.Type == SchemaConstraint.ConstraintType.ForeignKey && 
							objColumn.Name.EqualsIgnoreCase(objConstraint.Column))
						return true;
			// Si ha llegado hasta aquí es porque no es una clave foránea
				return false;
		}

		/// <summary>
		///		Obtiene el tipo de datos de una columna
		/// </summary>
		private string GetDataType(SchemaColumn objColumn)
		{ string strType = objColumn.DataType;

				// Añade los datos
					if (objColumn.CharacterMaximumLength > 0)
						strType += $"({objColumn.CharacterMaximumLength})";
				// Devuelve el tipo
					return strType;
    }

		/// <summary>
		///		Crea una estructura de documentación
		/// </summary>
		private StructDocumentationModel CreateStruct(StructDocumentationModel objParent, SchemaItem objItem, string strType)
		{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent,
																																				StructDocumentationModel.ScopeType.Global, 
																																				objItem.Name, strType, 0);

				// Añade los parámetros básicos
					objStruct.Parameters.Add("Schema", objItem.Schema);
					objStruct.Parameters.Add("Catalog", objItem.Catalog);
					objStruct.Parameters.Add("FullName", objItem.FullName);
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Formatea una fecha
		/// </summary>
		private string Format(DateTime? dtmValue)
		{ if (dtmValue == null)
				return "-";
			else
				return string.Format("{0:dd-MM-yyyy hh:mm}", dtmValue);
		}
	}
}
