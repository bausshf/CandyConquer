// Project by Bauss
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace Candy
{
	/// <summary>
	/// Internal data model class.
	/// </summary>
	public abstract class InternalDataModel
	{
		/// <summary>
		/// The filler method associated with reading to the model.
		/// </summary>
		internal MethodInfo _fillReadMethod;
		
		/// <summary>
		/// The filler method associated with writing by the model.
		/// </summary>
		internal MethodInfo _fillWriteMethod;
		
		/// <summary>
		/// Gets or sets the cached id member of the model.
		/// </summary>
		internal PropertyInfo IdMember { get; set; }
		
		/// <summary>
		/// Gets or sets the store association of the model.
		/// </summary>
		internal InternalDataStore StoreAssociation { get; set; }
		
		/// <summary>
		/// Creates a new internal data model.
		/// </summary>
		internal InternalDataModel() { }
		
		/// <summary>
		/// Internally fills a data model with its associated data.
		/// </summary>
		/// <param name="internalModel">The internal model.</param>
		/// <param name="reader">The reader.</param>
		/// <param name="fillMethod">The fill method.</param>
		private static void InternalReadFill(InternalDataModel internalModel, DataReader reader, MethodInfo fillMethod)
		{
			if (fillMethod != null)
			{
				fillMethod.Invoke(null, new object[] { internalModel, reader });
			}
		}
		
		/// <summary>
		/// Internally fills a dictionary with the data to write from the model.
		/// </summary>
		/// <param name="internalModel">The internal model.</param>
		/// <param name="fillMethod">The fill method.</param>
		/// <returns>A dictionary with the members to write.</returns>
		private static Dictionary<string,object> InternalWriteFill(InternalDataModel internalModel, MethodInfo fillMethod, Func<KeyValuePair<string,object>, bool> predicate)
		{
			if (fillMethod != null)
			{
				return (Dictionary<string,object>)fillMethod.Invoke(null, new object[] { internalModel, predicate });
			}
			
			return new Dictionary<string, object>();
		}
		
		/// <summary>
		/// Fills a dictionary with the data to write from the model.
		/// </summary>
		/// <returns>A dictionary with the members to write.</returns>
		protected Dictionary<string,object> FillWrite(Func<KeyValuePair<string,object>,bool> predicate)
		{
			return InternalWriteFill(this, _fillWriteMethod, predicate);
		}
		
		/// <summary>
		/// Fills the model.
		/// </summary>
		/// <param name="reader">The reader to fill by.</param>
		public void Fill(DataReader reader)
		{
			InternalReadFill(this, reader, _fillReadMethod);
		}
		
		/// <summary>
		/// Fills the model.
		/// </summary>
		/// <param name="reader">The reader to fill by.</param>
		/// <param name="fillMethod">The associated fill method.</param>
		public void Fill(DataReader reader, out MethodInfo fillMethod)
		{
			fillMethod = _fillReadMethod;
			
			InvokeFill(reader, _fillReadMethod);
		}
		
		/// <summary>
		/// Fills the model.
		/// </summary>
		/// <param name="reader">The reader to fill by.</param>
		/// <param name="fillMethod">The associated fill method.</param>
		public void InvokeFill(DataReader reader, MethodInfo fillMethod)
		{
			InternalReadFill(this, reader, fillMethod);
		}
	}
	
	/// <summary>
	/// A data model.
	/// </summary>
	public abstract class DataModel<TModel,TBaseModel,TFiller> : InternalDataModel
		where TBaseModel : DataModel<TModel,TBaseModel,TFiller>
		where TFiller : InternalDataFiller
	{
		/// <summary>
		/// Gets or sets the cached read members of the model.
		/// </summary>
		internal IEnumerable<PropertyInfo> ReadMembers { get; set; }
		
		/// <summary>
		/// Gets or sets the cached write members of the model.
		/// </summary>
		internal IEnumerable<PropertyInfo> WriteMembers { get; set; }
		
		/// <summary>
		/// The entry point of the model.
		/// </summary>
		public string EntryPoint { get; private set; }
		
		/// <summary>
		/// The entry name of the model.
		/// </summary>
		[DataIgnore()]
		public string EntryName { get; protected set; }
		
		/// <summary>
		/// Boolean determining whether the model can write or not.
		/// </summary>
		[DataIgnore()]
		public bool CanWrite { get; set; }
		
		/// <summary>
		/// Creates a new data model.
		/// </summary>
		/// <param name="filler">The filler associated with the model.</param>
		protected DataModel(TFiller filler)
			: base()
		{
			_fillReadMethod = filler.GenerateFillMethodInternal<TModel,TBaseModel,TFiller>(DataIgnoreType.Read);
			_fillWriteMethod = filler.GenerateFillMethodInternal<TModel,TBaseModel,TFiller>(DataIgnoreType.Write);
			
			var entryAttribute =
				this.GetType().GetCustomAttributes(typeof(DataEntryAttribute), false)
				.FirstOrDefault() as DataEntryAttribute;
			
			if (entryAttribute != null)
			{
				EntryPoint = entryAttribute.EntryPoint;
				EntryName = entryAttribute.Name;
			}
			
			CanWrite = true;
		}
		
		/// <summary>
		/// A method to create this model.
		/// Typically used to add it to somekind of storage.
		/// </summary>
		/// <returns>True if the creation was a success.</returns>
		public abstract bool Create();
		
		/// <summary>
		/// A method to update this model.
		/// Typically used to update the storage of it.
		/// </summary>
		/// <returns>True if the update was a success.</returns>
		public abstract bool Update();
		
		/// <summary>
		/// A method to delete this model.
		/// Typically used to delete it from its storage.
		/// </summary>
		/// <returns>True if the deletion was a success.</returns>
		public abstract bool Delete();
		
		/// <summary>
		/// Updates the id property value.
		/// This usually updates the cache with its respective value.
		/// </summary>
		/// <param name="firstTimeOnly">If set to true it will only update if it doesn't exist.</param>
		public abstract void UpdateIdProperty(bool firstTimeOnly = false);
		
		/// <summary>
		/// Removes the association to the models store.
		/// Note: This should generally be called from Delete().
		/// </summary>
		public void RemoveStoreAssociation()
		{
			if (StoreAssociation != null && StoreAssociation.SyncStoreAssociation)
			{
				StoreAssociation.RemoveModelAssociation(this);
			}
		}
	}
}
