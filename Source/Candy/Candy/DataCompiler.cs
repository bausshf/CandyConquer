// Project by Bauss
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Candy
{
	/// <summary>
	/// A compiler for data.
	/// </summary>
	internal static class DataCompiler
	{
		/// <summary>
		/// The code for the filler read method to compile in memory.
		/// </summary>
		private const string FILL_READ_CODE = @"using System;
using Candy;
@Usings

namespace @Namespace
{
	public class @Class
	{
		public static void @Method(InternalDataModel internalModel, @ReaderType reader)
		{
			if (internalModel == null)
			{
				return;
			}
			
			var model = (internalModel as @Type);
			
			@Members
		}
	}
}";
		
		/// <summary>
		/// Code for the filler write method to compile in memory.
		/// </summary>
		private const string FILL_WRITE_CODE = @"using System;
using System.Collections.Generic;
using Candy;
@Usings

namespace @Namespace
{
	public class @Class
	{
		public static Dictionary<string,object> @Method(InternalDataModel internalModel, Func<KeyValuePair<string,object>, bool> predicate)
		{
			var members = new Dictionary<string,object>();
			
			if (internalModel != null)
			{
				var model = (internalModel as @Type);
			
				@Members
			}
			
			return members;
		}
	}
}";
		
		/// <summary>
		/// The namespace of compiled code.
		/// </summary>
		internal const string Namespace = "DataFillEngineAssemblyBuilder";
		
		/// <summary>
		/// The class name of a reader.
		/// </summary>
		internal const string ReaderClassName = "DataAssemblyReader";
		
		/// <summary>
		/// The class name of a writer.
		/// </summary>
		internal const string WriterClassName = "DataAssemblyWriter";
		
		/// <summary>
		/// The method of a reader.
		/// </summary>
		internal const string ReadMethod = "ReadData";
		
		/// <summary>
		/// The method of a writer.
		/// </summary>
		internal const string WriteMethod = "WriteData";
		
		/// <summary>
		/// The reader code.
		/// </summary>
		internal static readonly string ReaderCode;
		
		/// <summary>
		/// The writer code.
		/// </summary>
		internal static readonly string WriterCode;
		
		/// <summary>
		/// Static constructor for DataCompiler.
		/// </summary>
		static DataCompiler()
		{
			ReaderCode = FILL_READ_CODE
				.Replace("@Namespace", Namespace)
				.Replace("@Class", ReaderClassName)
				.Replace("@Method", ReadMethod);
			WriterCode = FILL_WRITE_CODE
				.Replace("@Namespace", Namespace)
				.Replace("@Class", WriterClassName)
				.Replace("@Method", WriteMethod);
		}
		
		/// <summary>
		/// Compiles a piece of code into memory.
		/// </summary>
		/// <param name="code">The code to compile.</param>
		/// <returns>The results of the compilation.</returns>
		internal static CompilerResults Compile<TCodeProvider>(string code)
			where TCodeProvider : CodeDomProvider, new()
		{
			var compilerParameters = new CompilerParameters
			{
				GenerateInMemory = true
			};
			
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				compilerParameters.ReferencedAssemblies.Add(assembly.Location);
			}
			
			return new TCodeProvider().CompileAssemblyFromSource(compilerParameters, code);
		}
		
		/// <summary>
		/// Attempts to find a method inside a compiled assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <param name="_namespace">The namespace.</param>
		/// <param name="className">The class name.</param>
		/// <param name="methodName">The method name.</param>
		/// <returns>The method information.</returns>
		internal static MethodInfo FindMethod(Assembly assembly, string _namespace, string className, string methodName)
		{
			var parentClass = assembly.GetTypes().Where(type => type.IsClass &&
			                                            type.Namespace == _namespace &&
			                                            type.Name == className).FirstOrDefault();
			if (parentClass == null)
			{
				throw new DataCompilerException("Could not find the compiled class.");
			}
			
			var method = parentClass.GetMethods().Where(m => m.IsStatic && m.Name == methodName).FirstOrDefault();
			if (method == null)
			{
				throw new DataCompilerException("Could not find the compiled method.");
			}
			
			return method;
		}
	}
}
