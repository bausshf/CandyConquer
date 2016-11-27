// Project by Bauss
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Candy
{
	/// <summary>
	/// A sql data filler based on Candy.DataFiller.
	/// DO NOT CALL THIS, USED INTERNALLY ONLY, BUT HAS TO BE DECLARED PUBLIC DUE TO GENERIC INHERITANCE.
	/// </summary>
	public class SqlFiller : DataFiller<SqlDataReader,SqlFiller>
	{
		/// <summary>
		/// The filler.
		/// </summary>
		private static SqlFiller _filler;
		
		/// <summary>
		/// Gets the sql filler.
		/// </summary>
		public static SqlFiller Filler
		{
			get
			{
				if (_filler == null)
				{
					_filler = new SqlFiller();
				}
				return _filler;
			}
		}
		
		/// <summary>
		/// Creates a new sql filler.
		/// </summary>
		private SqlFiller()
			: base()
		{
		}
		
		/// <summary>
		/// Creates a read format based on a collection of properties.
		/// </summary>
		/// <param name="members">The members to create the read format based on.</param>
		/// <returns>A list of data read info used for the formatting.</returns>
		public override List<DataReadInfo> CreateReadFormat(IEnumerable<PropertyInfo> members)
		{
			if (members == null)
			{
				return new List<DataReadInfo>();
			}
			
			var readInfos = new List<DataReadInfo>();
			
			foreach (var member in members)
			{
				if (member.SetMethod != null)
				{
					var type = member.PropertyType;
					var typeName = type.Name;
					var readName = member.Name;
					var readAttribute = member.GetCustomAttribute<DataReadFormatAttribute>();
					var memberAttribute = member.GetCustomAttribute<DataMemberInfoAttribute>();
					
					if (memberAttribute != null)
					{
						readName = memberAttribute.Name;
					}
					
					if (readAttribute != null)
					{
						readInfos.Add(new DataReadCustomInfo
						              {
						              	Member = member,
						              	ReadName = readName,
						              	TypeName = typeName,
						              	ReadFormat = readAttribute.ReadFormat,
						              	AssociatedNamespaces = readAttribute.AssociatedNamespaces
						              });
					}
					else
					{
						var typeConversion = typeName;
						
						if (typeName.Contains("Nullable"))
						{
							typeConversion = string.Concat(type.GetProperty("Value").PropertyType + "?");
						}
						
						readInfos.Add(new DataReadInfo
						              {
						              	Member = member,
						              	ReadName = readName,
						              	TypeName = typeConversion
						              });
					}
				}
			}
			
			return readInfos;
		}
	}
}
