// Project by Bauss
using System;
using System.Collections.Generic;

namespace Candy
{
	/// <summary>
	/// A data proxy.
	/// A proxy used to request a list of model data.
	/// </summary>
	public abstract class DataProxy<TModel>
		where TModel : InternalDataModel
	{
		/// <summary>
		/// The parameters specified for the proxy.
		/// </summary>
		public DataProxyParameterCollection Parameters { get; private set; }
		
		/// <summary>
		/// Creates a new data proxy.
		/// </summary>
		public DataProxy()
		{
			Parameters = new DataProxyParameterCollection();
		}
		
		/// <summary>
		/// Requests data on the proxy by the specified parameters.
		/// </summary>
		/// <param name="success">Set to true if the request was a success.</param>
		/// <returns>A list of model data.</returns>
		public abstract List<TModel> RequestData(out bool success);
	}
}
