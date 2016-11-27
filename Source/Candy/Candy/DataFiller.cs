// Project by Bauss
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Candy
{
	/// <summary>
	/// Internal data filler.
	/// </summary>
	public abstract class InternalDataFiller
	{
		/// <summary>
		/// Creates a new internal data filler.
		/// </summary>
		internal InternalDataFiller() { }
		
		/// <summary>
		/// Gets the reader name.
		/// </summary>
		public string ReaderName { get; internal set; }
		
		/// <summary>
		/// Creates a read format internally.
		/// </summary>
		/// <param name="members">The members to create the read format from.</param>
		/// <returns>A list of the formatting data.</returns>
		internal abstract List<DataReadInfo> CreateReadFormatInternal(IEnumerable<PropertyInfo> members);
		
		/// <summary>
		/// Generates a fill method internally.
		/// </summary>
		/// <returns>The generated fill method.</returns>
		internal MethodInfo GenerateFillMethodInternal<TModel,TBaseModel,TFiller>(DataIgnoreType fillCompileType)
			where TBaseModel : DataModel<TModel,TBaseModel,TFiller>
			where TFiller : InternalDataFiller
		{
			return DataFillGenerator.GenerateFillMethod<TModel,TBaseModel,TFiller>(fillCompileType);
		}
	}
	
	/// <summary>
	/// A data filler.
	/// </summary>
	public abstract class DataFiller<TReader,TFiller> : InternalDataFiller
		where TReader : DataReader
		where TFiller : InternalDataFiller
	{
		/// <summary>
		/// Creates a new data filler.
		/// </summary>
		public DataFiller()
		{
			if (!DataFillGenerator.ContainsFiller<TFiller>())
			{
				DataFillGenerator.TryAddFiller(typeof(TFiller).FullName, this);
			}
			
			ReaderName = typeof(TReader).FullName;
		}
		
		/// <summary>
		/// Creates a read format internally.
		/// </summary>
		/// <param name="members">The members to create the read format from.</param>
		/// <returns>A list of the formatting data.</returns>
		internal override List<DataReadInfo> CreateReadFormatInternal(IEnumerable<PropertyInfo> members)
		{
			return CreateReadFormat(members);
		}
		
		/// <summary>
		/// Creates a read format.
		/// </summary>
		/// <param name="members">The members to create the read format from.</param>
		/// <returns>A list of the formatting data.</returns>
		public abstract List<DataReadInfo> CreateReadFormat(IEnumerable<PropertyInfo> members);
	}
}
