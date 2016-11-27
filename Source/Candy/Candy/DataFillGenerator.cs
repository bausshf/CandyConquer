// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Candy
{
	/// <summary>
	/// A fill generator.
	/// </summary>
	internal static class DataFillGenerator
	{
		/// <summary>
		/// Collection of fillers.
		/// </summary>
		private static ConcurrentDictionary<string, InternalDataFiller> _fillers;
		/// <summary>
		/// Collection of filler read methods.
		/// </summary>
		private static ConcurrentDictionary<string, MethodInfo> _fillerReadMethods;
		
		/// <summary>
		/// Collection of filler write methods.
		/// </summary>
		private static ConcurrentDictionary<string, MethodInfo> _fillerWriteMethods;
		
		
		
		/// <summary>
		/// Static constructor for the fill generator.
		/// </summary>
		static DataFillGenerator()
		{
			_fillers = new ConcurrentDictionary<string, InternalDataFiller>();
			_fillerReadMethods = new ConcurrentDictionary<string, MethodInfo>();
			_fillerWriteMethods = new ConcurrentDictionary<string, MethodInfo>();
		}
		
		/// <summary>
		/// Tries to add a filler.
		/// </summary>
		/// <param name="fillName">The filler name.</param>
		/// <param name="filler">The filler.</param>
		/// <returns>True if the filler was added.</returns>
		internal static bool TryAddFiller(string fillName, InternalDataFiller filler)
		{
			return _fillers.TryAdd(fillName, filler);
		}
		
		/// <summary>
		/// Checks if the filler collection contains a specific filler, based on the filler type.
		/// </summary>
		/// <returns>True if the filler exists within the collection.</returns>
		internal static bool ContainsFiller<T>()
			where T : InternalDataFiller
		{
			return _fillers.ContainsKey(typeof(T).FullName);
		}
		
		/// <summary>
		/// Attempts to get a filler by a filler type.
		/// </summary>
		/// <param name="filler">The filler if found.</param>
		/// <returns>True if found, false otherwise.</returns>
		private static bool TryGetFiller<T>(out T filler)
			where T : InternalDataFiller
		{
			InternalDataFiller internalFiller;
			if (_fillers.TryGetValue(typeof(T).FullName, out internalFiller))
			{
				filler = (T)internalFiller;
				return true;
			}
			else
			{
				filler = default(T);
				return false;
			}
		}
		
		/// <summary>
		/// Generates a fill method based on a model and a filler.
		/// </summary>
		/// <returns>A fill method.</returns>
		internal static MethodInfo GenerateFillMethod<TModel,TBaseModel,TFiller>(DataIgnoreType fillCompileType)
			where TBaseModel : DataModel<TModel,TBaseModel,TFiller>
			where TFiller : InternalDataFiller
		{
			var fillType = typeof(TModel);
			
			MethodInfo fillerMethod;
			if (fillCompileType == DataIgnoreType.Read)
			{
				if (_fillerReadMethods.TryGetValue(fillType.FullName, out fillerMethod))
				{
					return fillerMethod;
				}
			}
			else // (implicit) if (fillCompileType == DataIgnoreType.Write)
			{
				if (_fillerWriteMethods.TryGetValue(fillType.FullName, out fillerMethod))
				{
					return fillerMethod;
				}
			}
			
			TFiller filler;
			if (!TryGetFiller<TFiller>(out filler))
			{
				throw new MemberAccessException("Cannot access the filler context. Make sure the filler has an instance.");
			}

			var members = filler.CreateReadFormatInternal(DataHelper.GetPropertiesByIgnore<TModel,TBaseModel,TFiller>(null, fillCompileType));
			var namespaces = new List<string>();
			var formattedMembers = members.Select(member => DataHelper.FormatMember(member, fillCompileType, namespaces));
			namespaces = namespaces.Distinct().ToList();

			var code = DataHelper.GetCompilerCode(fillCompileType, filler, fillType, formattedMembers, namespaces);
			return CompileFillMethod(code, fillType, fillCompileType);
		}
		
		/// <summary>
		/// Compiles the fill method based on generated code and a fill type.
		/// </summary>
		/// <param name="code">The generated code to compile.</param>
		/// <param name="fillType">The associated fill type.</param>
		/// <returns>The compiled fill method.</returns>
		private static MethodInfo CompileFillMethod(string code, Type fillType, DataIgnoreType fillCompilerType)
		{
			var compilerResults = DataCompiler.Compile<CSharpCodeProvider>(code);
			if (compilerResults.Errors.HasErrors)
			{
				Console.WriteLine(code);
				var errors = new List<string>();
				foreach (var error in compilerResults.Errors)
				{
					errors.Add(error.ToString());
				}
				throw new DataCompilerException("Failed to compile ...", errors);
			}
			else
			{
				var method = DataCompiler.FindMethod(compilerResults.CompiledAssembly,
				                                     DataCompiler.Namespace,
				                                     fillCompilerType == DataIgnoreType.Read ? DataCompiler.ReaderClassName : DataCompiler.WriterClassName,
				                                     fillCompilerType == DataIgnoreType.Read ? DataCompiler.ReadMethod : DataCompiler.WriteMethod);
				
				if (method != null)
				{
					if (fillCompilerType == DataIgnoreType.Read)
					{
						_fillerReadMethods.TryAdd(fillType.FullName, method);
					}
					else // (implicit) if (fillCompilerType == DataIgnoreType.Write)
					{
						_fillerWriteMethods.TryAdd(fillType.FullName, method);
					}
				}
				
				return method;
			}
		}
	}
}
