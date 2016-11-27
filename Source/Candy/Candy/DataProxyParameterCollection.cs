// Project by Bauss
using System;
using System.Collections.Generic;

namespace Candy
{
	/// <summary>
	/// A proxy parameter collection.
	/// </summary>
	public sealed class DataProxyParameterCollection
	{
		/// <summary>
		/// The parameters.
		/// </summary>
		private Dictionary<string,object> _params;
		
		/// <summary>
		/// Creates a new data proxy parameter collection.
		/// </summary>
		internal DataProxyParameterCollection()
		{
			_params = new Dictionary<string, object>();
		}
		
		/// <summary>
		/// Adds or updates a parameter.
		/// </summary>
		/// <param name="paramName">The parameter name.</param>
		/// <param name="value">The value.</param>
		public void AddOrUpdate(string paramName, object value)
		{
			if (_params.ContainsKey(paramName))
			{
				_params[paramName] = value;
			}
			else
			{
				_params.Add(paramName, value);
			}
		}
		
		/// <summary>
		/// Attempts to get a value from the parameter collection.
		/// </summary>
		public object this[string paramName]
		{
			get
			{
				object value;
				if (!_params.TryGetValue(paramName, out value))
				{
					value = null;
				}
				
				return value;
			}
		}
	}
}
