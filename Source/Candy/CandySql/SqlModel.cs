// Project by Bauss
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Candy
{
	/// <summary>
	/// A sql data model.
	/// </summary>
	public abstract class SqlModel<TModel> : DataModel<TModel, SqlModel<TModel>,SqlFiller>
		where TModel : InternalDataModel
	{
		/// <summary>
		/// The parameter cache.
		/// </summary>
		private Dictionary<string, object> _pars;
		/// <summary>
		/// The id property cache.
		/// </summary>
		private PropertyInfo _idProperty;
		/// <summary>
		/// Id parameter dictionary cache.
		/// </summary>
		private Dictionary<string,object> _idPars;
		
		/// <summary>
		/// Boolean determining whether deletions of this model should be logged.
		/// It's done by setting a flag on the column for deletion.
		/// </summary>
		/// <remarks>Requires the table to implement a "Deleted" column with the type "bit".</remarks>
		[DataIgnore()]
		public bool DeleteLog { get; protected set; }
		
		/// <summary>
		/// Creates a new sql model.
		/// </summary>
		protected SqlModel()
			: base(SqlFiller.Filler)
		{
			if (string.IsNullOrWhiteSpace(EntryName) || string.IsNullOrWhiteSpace(EntryPoint))
			{
				throw new TypeLoadException("Failed to load the model's entry attribute. Make sure it has a DataEntryAttribute declared.");
			}
			
			_pars = new Dictionary<string, object>();
			
			_idProperty = DataHelper.GetIdProperty<SqlModel<TModel>>(this);
			if (_idProperty == null)
			{
				throw new TypeLoadException("There must be an id property associated with the model.");
			}
			
			_idPars = new Dictionary<string, object>();
		}
		
		/// <summary>
		/// Creeates a dictionary of the parameters to use.
		/// </summary>
		/// <param name="ignoreCache">IF set to true it will ignore the parameter cache.</param>
		/// <returns>The dictionary of the parameters.</returns>
		private Dictionary<string, object> CreateParameters(bool ignoreCache = false)
		{
			return FillWrite(member =>
			                 {
			                 	if (!_pars.ContainsKey(member.Key))
			                 	{
			                 		_pars.Add(member.Key, member.Value);
			                 	}
			                 	else if (Object.Equals(_pars[member.Key], member.Value) && !ignoreCache)
			                 	{
			                 		return false;
			                 	}
			                 	
			                 	_pars[member.Key] = member.Value;
			                 	return true;
			                 });
		}
		
		/// <summary>
		/// Creates a new row for this record.
		/// </summary>
		public override bool Create()
		{
			if (!CanWrite)
			{
				return false;
			}
			
			if (_idPars.ContainsKey("Deleted"))
			{
				_idPars.Remove("Deleted");
			}
			
			var pars = CreateParameters(true);
			
			_idProperty.SetValue(this, Sql.ExecuteScalar(EntryPoint, string.Format("INSERT INTO {0} {1}", EntryName, Sql.GetIns(pars)), pars), null);
			UpdateIdProperty();
			
			return true;
		}
		
		/// <summary>
		/// Creates a new row of the record but in another entry.
		/// This is useful for tables that shares same table structure, but different data.
		/// </summary>
		/// <param name="entryName">The new entry name.</param>
		/// <param name="overrideEntryName">If set to true then the old entry name is not backed up.</param>
		/// <returns>True if the creation was a success.</returns>
		public bool Create(string entryName, bool overrideEntryName = true)
		{
			if (overrideEntryName)
			{
				EntryName = entryName;
				return Create();
			}
			else
			{
				var oldEntryName = EntryName;
				EntryName = entryName;
				var result = Create();
				EntryName = oldEntryName;
				
				return result;
			}
		}
		
		/// <summary>
		/// Updates the associated row of this record.
		/// </summary>
		public override bool Update()
		{
			if (!CanWrite)
			{
				return false;
			}
			
			var pars = CreateParameters();
			if (pars.Count == 0)
			{
				return false;
			}
			
			if (_idPars.ContainsKey("Deleted"))
			{
				_idPars.Remove("Deleted");
				pars.Add("Deleted", false);
			}
			
			var updateSet = Sql.GetSet(pars);
			UpdateIdProperty(true);
			
			if (pars.ContainsKey(_idProperty.Name))
			{
				pars[_idProperty.Name] = _idPars[_idProperty.Name];
			}
			else
			{
				pars.Add(_idProperty.Name, _idPars[_idProperty.Name]);
			}
			
			return Sql.ExecuteNonQuery(EntryPoint, string.Format("UPDATE {0} SET {1} WHERE Id = @Id", EntryName, updateSet), pars) > 0;
		}
		
		/// <summary>
		/// Updates the row of the record but in another entry.
		/// This is useful for tables that shares same table structure, but different data.
		/// </summary>
		/// <param name="entryName">The new entry name.</param>
		/// <param name="overrideEntryName">If set to true then the old entry name is not backed up.</param>
		/// <returns>True if the update was a success.</returns>
		public bool Update(string entryName, bool overrideEntryName = true)
		{
			if (overrideEntryName)
			{
				EntryName = entryName;
				return Update();
			}
			else
			{
				var oldEntryName = EntryName;
				EntryName = entryName;
				var result = Update();
				EntryName = oldEntryName;
				
				return result;
			}
		}
		
		/// <summary>
		/// Deletes this record.
		/// </summary>
		public override bool Delete()
		{
			UpdateIdProperty(true);
			
			bool success;
			if (DeleteLog)
			{
				if (_idPars.ContainsKey("Deleted"))
				{
					_idPars["Deleted"] = true;
				}
				else
				{
					_idPars.Add("Deleted", true);
				}
				
				success = Sql.ExecuteNonQuery(EntryPoint, string.Format("UPDATE {0} SET [Deleted] = @Deleted WHERE Id = @Id", EntryName), _idPars) > 0;
			}
			else
			{
				success = Sql.ExecuteNonQuery(EntryPoint, string.Format("DELETE FROM {0} WHERE Id = @Id", EntryName), _idPars) > 0;
			}
			
			if (success)
			{
				RemoveStoreAssociation();
			}
			
			return success;
		}
		
		/// <summary>
		/// Deletes this record from another entry.
		/// This is useful for tables that shares same table structure, but different data.
		/// </summary>
		/// <param name="entryName">The new entry name.</param>
		/// <param name="overrideEntryName">If set to true then the old entry name is not backed up.</param>
		/// <returns>True if the deletion was a success.</returns>
		public bool Delete(string entryName, bool overrideEntryName = true)
		{
			if (overrideEntryName)
			{
				EntryName = entryName;
				return Delete();
			}
			else
			{
				var oldEntryName = EntryName;
				EntryName = entryName;
				var result = Delete();
				EntryName = oldEntryName;
				
				return result;
			}
		}
		
		/// <summary>
		/// Updates the id property's cached value.
		/// </summary>
		/// <param name="firstTimeOnly">If set to true it will only update if it doesn't exist.</param>
		public override void UpdateIdProperty(bool firstTimeOnly = false)
		{
			if (firstTimeOnly)
			{
				var value = _idProperty.GetValue(this, null);
				
				if (!_idPars.ContainsKey(_idProperty.Name))
				{
					_idPars.Add(_idProperty.Name, value);
				}
			}
			else
			{
				var value = _idProperty.GetValue(this, null);
				
				if (!_idPars.ContainsKey(_idProperty.Name))
				{
					_idPars.Add(_idProperty.Name, value);
				}
				else
				{
					_idPars[_idProperty.Name] = value;
				}
			}
		}
	}
}
