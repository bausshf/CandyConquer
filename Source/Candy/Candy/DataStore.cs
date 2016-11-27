// Project by Bauss
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Candy
{
	/// <summary>
	/// Internal data store.
	/// </summary>
	public abstract class InternalDataStore
	{
		/// <summary>
		/// A property determining whether the store association should be synced with its models.
		/// </summary>
		public bool SyncStoreAssociation { get; set; }
		
		/// <summary>
		/// Creates a new internal data store.
		/// </summary>
		protected InternalDataStore() { }
		
		/// <summary>
		/// A method for removing the model association.
		/// </summary>
		/// <param name="model">The model.</param>
		internal abstract void RemoveModelAssociation(InternalDataModel model);
	}
	
	/// <summary>
	/// A data store.
	/// A storage of model data.
	/// Stores are meant to be a gateway collection of data where the store is filled by a proxy.
	/// The proxy could ex. be an ajax proxy that requests data from a remote server,
	/// in which we then store the data in the store.
	/// It could also be that the store proxies to different end points depending on some
	/// criterias ex. maybe there's different "driver types" which determines the endpoint.
	/// Ex. one end point may need an ajax request to respond with data,
	/// another end point may need raw sockets to respond with data and
	/// a last is a direct database end point.
	/// A store with a proxy could eliminate possible 3 different calls all places where the
	/// data of the store has to be used, into one simple call to the store.
	/// The store and the proxy will then take care of where to gather the data.
	/// Stores should only be used where convenient.
	/// </summary>
	public abstract class DataStore<TModel> : InternalDataStore, IEnumerator, IEnumerable
		where TModel : InternalDataModel
	{
		/// <summary>
		/// The list of model data.
		/// </summary>
		private List<TModel> _data;
		/// <summary>
		/// The current position (foreach).
		/// </summary>
		private int _position;
		/// <summary>
		/// Gets or sets the proxy of the store.
		/// </summary>
		public DataProxy<TModel> Proxy { get; set; }
		/// <summary>
		/// Gets or sets a boolean determining whether the data should be cached or not.
		/// </summary>
		public bool Cache { get; set; }
		
		/// <summary>
		/// Creates a new data store.
		/// </summary>
		/// <param name="cache">A boolean determing whether the data should be cached or not. Default is true.</param>
		public DataStore(bool cache = true)
			: base()
		{
			_data = new List<TModel>();
			_position = -1;
			SyncStoreAssociation = true;
			
			Cache = cache;
		}
		
		/// <summary>
		/// Clears the store.
		/// </summary>
		public void Clear()
		{
			if (SyncStoreAssociation)
			{
				foreach (var model in _data)
				{
					model.StoreAssociation = null;
				}
			}
			
			_data = new List<TModel>();
		}
		
		/// <summary>
		/// Loads the store.
		/// </summary>
		/// <param name="removeRecords">Set to true if all records currently in the store should be removed.</param>
		public void Load(bool removeRecords = true)
		{
			if (removeRecords || !Cache)
			{
				Clear();
			}
			
			if (Proxy != null)
			{
				bool success;
				var data = Proxy.RequestData(out success);
				if (success && data != null && data.Count > 0)
				{
					if (removeRecords)
					{
						foreach (var model in data)
						{
							model.StoreAssociation = this;
							_data.Add(model);
						}
					}
					else
					{
						if (Cache)
						{
							var newModels = data.Where(m =>
							                           {
							                           	if (m.IdMember == null)
							                           	{
							                           		m.IdMember = DataHelper.GetIdProperty<TModel>(m);
							                           	}
							                           	
							                           	var idValue = m.IdMember.GetValue(m, null);
							                           	return this[idValue] == default(TModel);
							                           }).ToList();
							foreach (var model in newModels)
							{
								if (SyncStoreAssociation)
								{
									model.StoreAssociation = this;
								}
								
								_data.Add(model);
							}
							
							if (newModels.Count > 0)
							{
								Order();
							}
						}
						else
						{
							foreach (var model in data)
							{
								var cachedModel = _data
									.FirstOrDefault(cm =>
									                {
									                	if (cm.IdMember == null)
									                	{
									                		cm.IdMember = DataHelper.GetIdProperty<TModel>(cm);
									                	}
									                	
									                	var cachedIdValue = cm.IdMember.GetValue(cm, null);
									                	
									                	if (model.IdMember == null)
									                	{
									                		model.IdMember = DataHelper.GetIdProperty<TModel>(model);
									                	}
									                	
									                	var idValue = model.IdMember.GetValue(model, null);
									                	
									                	return Object.Equals(cachedIdValue, idValue);
									                });
								if (cachedModel != null)
								{
									RemoveModelAssociation(cachedModel);
								}
								
								if (SyncStoreAssociation)
								{
									model.StoreAssociation = this;
								}
								
								_data.Add(model);
							}
							
							Order();
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Gets a model based on an id value.
		/// Returns null if the model wasn't found.
		/// </summary>
		public TModel this[object idValue]
		{
			get { return _data.Where(model => Object.Equals(model.IdMember.GetValue(model, null), idValue)).FirstOrDefault(); }
		}
		
		/// <summary>
		/// Orders the store data by their id.
		/// </summary>
		private void Order()
		{
			_data = _data.OrderBy(model => model.IdMember.GetValue(model, null)).ToList();
		}
		
		/// <summary>
		/// Removes a model association.
		/// </summary>
		/// <param name="model">The model.</param>
		internal override void RemoveModelAssociation(InternalDataModel model)
		{
			if (SyncStoreAssociation)
			{
				model.StoreAssociation = null;
			}
			_data.Remove((TModel)model);
		}
		
		/// <summary>
		/// Gets the enumerator. (foreach)
		/// </summary>
		/// <returns>IEnumerator.</returns>
		public IEnumerator GetEnumerator()
		{
			return (IEnumerator)this;
		}
		
		/// <summary>
		/// Moves next. (foreach)
		/// </summary>
		/// <returns></returns>
		public bool MoveNext()
		{
			_position++;
			if (_position < _data.Count)
			{
				return true;
			}
			else
			{
				Reset();
				return false;
			}
		}
		
		/// <summary>
		/// Resets the position. (foreach)
		/// </summary>
		public void Reset()
		{
			_position = -1;
		}
		
		/// <summary>
		/// Gets the current object. (foreach)
		/// </summary>
		public object Current
		{
			get { return _data[_position]; }
		}
	}
}