// Project by Bauss
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Candy
{
	/// <summary>
	/// A helper class for handling data.
	/// </summary>
	public static class DataHelper
	{
		/// <summary>
		/// Gets the id property based on a model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>The id property.</returns>
		public static PropertyInfo GetIdProperty<TModel>(TModel model)
			where TModel : InternalDataModel
		{
			if (model.IdMember != null)
			{
				return model.IdMember;
			}
			
			model.IdMember = model.GetType().GetProperties().Where(property =>
			                                                       {
			                                                       	var specialAttribute = property.GetCustomAttribute<DataSpecialTypeAttribute>();
			                                                       	return (specialAttribute != null && specialAttribute.DataType == SpecialDataType.Id);
			                                                       }).FirstOrDefault();
			return model.IdMember;
		}
		/// <summary>
		/// Gets all properties by a specific ignore type.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="handleType">The type to handle. (Not ignore.)</param>
		/// <returns>An IEnumerable of PropertyInfo which is the properties.</returns>
		public static IEnumerable<PropertyInfo> GetPropertiesByIgnore<TModel,TBaseModel,TFiller>(TBaseModel model, DataIgnoreType handleType)
			where TBaseModel : DataModel<TModel,TBaseModel,TFiller>
			where TFiller : InternalDataFiller
		{
			if (model != null)
			{
				switch (handleType)
				{
					case DataIgnoreType.Read:
						if (model.ReadMembers != null)
						{
							return model.ReadMembers;
						}
						break;
						
					case DataIgnoreType.Write:
						if (model.WriteMembers != null)
						{
							return model.WriteMembers;
						}
						break;
						
					default:
						throw new ArgumentException("handleType");
				}
			}
			
			var properties = typeof(TModel).GetProperties().Where(property =>
			                                                      {
			                                                      	var ignoreAttribute = property.GetCustomAttribute<DataIgnoreAttribute>();
			                                                      	return ignoreAttribute == null || (ignoreAttribute.IgnoreType != handleType && ignoreAttribute.IgnoreType != DataIgnoreType.Both);
			                                                      }).ToList();
			
			if (model != null)
			{
				switch (handleType)
				{
					case DataIgnoreType.Read:
						model.ReadMembers = properties;
						break;
						
					case DataIgnoreType.Write:
						model.WriteMembers = properties;
						break;
						
					default:
						throw new ArgumentException("handleType");
				}
			}
			
			return properties;
		}
		
		/// <summary>
		/// Formats a DataReadInfo member into a compilable string.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <param name="fillCompileType">The filler compile type.</param>
		/// <param name="namespaces">A collection of namespaces where custom namespaces are added.</param>
		/// <returns>A string that's compilable.</returns>
		internal static string FormatMember(DataReadInfo member, DataIgnoreType fillCompileType, ICollection<string> namespaces)
		{
			// Reader ...///
			const string READ_FORMAT = "reader.Get<{0}>(\"{1}\")";
			const string SET_CUSTOM_FORMAT = "model.{0} = {1};";
			const string SET_FORMAT = "model.{0} = ({1}){2};";
			
			// Writer ...
			const string WRITE_FORMAT = "if (predicate.Invoke(new KeyValuePair<string,object>(\"{0}\", model.{1}))) {{ members.Add(\"{0}\", model.{1}); }}";
			const string WRITE_TIMESTAMP_FORMAT = "model.{0} = DateTime.UtcNow; if (predicate.Invoke(new KeyValuePair<string,object>(\"{0}\", model.{1}))) {{ members.Add(\"{0}\", model.{1}); }}";
			const string WRITE_STRING_FORMAT = "if (predicate.Invoke(new KeyValuePair<string,object>(\"{0}\", model.{1}.ToString()))) {{ members.Add(\"{0}\", model.{1}.ToString()); }}";
			
			if (fillCompileType == DataIgnoreType.Read)
			{
				var custom = (member as DataReadCustomInfo);
				
				if (custom != null)
				{
					if (custom.AssociatedNamespaces != null)
					{
						namespaces.AddRange(custom.AssociatedNamespaces.Select(ns => string.Format("using {0};", ns)));
					}
					
					var value = string.Format(READ_FORMAT, typeof(string).Name, member.ReadName);
					
					return string.Format(SET_CUSTOM_FORMAT, member.Member.Name, custom.ReadFormat.Replace("@value", value));
				}
				else
				{
					var value = string.Format(READ_FORMAT, member.TypeName, member.ReadName);
					
					return string.Format(SET_FORMAT, member.Member.Name, member.TypeName, value);
				}
			}
			else // (implicit) if (fillCompileType == DataIgnoreType.Write)
			{
				var custom = (member as DataReadCustomInfo);
				
				if (custom != null)
				{
					if (custom.AssociatedNamespaces != null)
					{
						namespaces.AddRange(custom.AssociatedNamespaces.Select(ns => string.Format("using {0};", ns)));
					}
				}
				
				var specialAttribute = member.Member.GetCustomAttribute<DataSpecialTypeAttribute>();
				if (specialAttribute != null)
				{
					// implement more ...
					// use switch ...
					switch (specialAttribute.DataType)
					{
						case SpecialDataType.Timestamp:
							{
								return string.Format(WRITE_TIMESTAMP_FORMAT, member.ReadName, member.Member.Name);
							}
						case SpecialDataType.AsString:
							{
								return string.Format(WRITE_STRING_FORMAT, member.ReadName, member.Member.Name);
							}
					}
				}
				
				return string.Format(WRITE_FORMAT, member.ReadName, member.Member.Name);
			}
		}
		
		/// <summary>
		/// Gets a compiler code based on a filler compile type.
		/// </summary>
		/// <param name="fillCompileType">The filler compile type.</param>
		/// <param name="filler">The filler.</param>
		/// <param name="fillType">The filler type.</param>
		/// <param name="formattedMembers">The formatted members.</param>
		/// <param name="namespaces">A collection of namespaces to format into the code.</param>
		/// <returns>A compilable string.</returns>
		internal static string GetCompilerCode<TFiller>(DataIgnoreType fillCompileType, TFiller filler, Type fillType, IEnumerable<string> formattedMembers, IEnumerable<string> namespaces)
			where TFiller : InternalDataFiller
		{
			var code = fillCompileType == DataIgnoreType.Read ?
				DataCompiler.ReaderCode.Replace("@ReaderType", filler.ReaderName) :
				DataCompiler.WriterCode;
			
			return code.Replace("@Type", fillType.FullName.Replace("+", "."))
				.Replace("@Members", string.Join("\r\n", formattedMembers))
				.Replace("@Usings", string.Join("\r\n", namespaces));
		}
	}
}
