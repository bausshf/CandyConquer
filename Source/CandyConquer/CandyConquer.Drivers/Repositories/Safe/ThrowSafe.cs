// Project by Bauss
using System;

namespace CandyConquer.Drivers.Repositories.Safe
{
	/// <summary>
	/// Throw safe executions.
	/// </summary>
	public static class ThrowSafe
	{
		/// <summary>
		/// Executes an action safely without throwing exceptions.
		/// </summary>
		/// <param name="exec">The action to execute.</param>
		/// <returns>A tuple of a value indicating whether the action was executed without throwing and the caught exception, if any.</returns>
		public static Tuple<bool,Exception> Execute(Action exec)
		{
			try
			{
				exec();
				return new Tuple<bool,Exception>(true, null);
			}
			catch (Exception e)
			{
				return new Tuple<bool,Exception>(false, e);
			}
		}
		/// <summary>
		/// Executes an action safely without throwing exceptions.
		/// </summary>
		/// <param name="exec">The action to execute.</param>
		/// <param name="arg">The parameter argument for the action.</param>
		/// <returns>A tuple of a value indicating whether the action was executed without throwing and the caught exception, if any.</returns>
		public static Tuple<bool,Exception> Execute<T>(Action<T> exec, T arg)
		{
			try
			{
				exec(arg);
				return new Tuple<bool,Exception>(true, null);
			}
			catch (Exception e)
			{
				return new Tuple<bool,Exception>(false, e);
			}
		}
		
		/// <summary>
		/// Executes a function safely without throwing exceptions.
		/// </summary>
		/// <param name="exec">The function to execute.</param>
		/// <returns>A tuple of the functions return value, a value indicating whether the action was executed without throwing and the caught exception, if any.</returns>
		public static Tuple<bool,T,Exception> Execute<T>(Func<T> exec)
		{
			try
			{
				return new Tuple<bool,T,Exception>(true, exec(), null);
			}
			catch (Exception e)
			{
				return new Tuple<bool,T,Exception>(false, default(T), e);
			}
		}
		
		/// <summary>
		/// Executes a function safely without throwing exceptions.
		/// </summary>
		/// <param name="exec">The function to execute.</param>
		/// <param name="arg">The parameter argument for the function.</param>
		/// <returns>A tuple of the functions return value, a value indicating whether the action was executed without throwing and the caught exception, if any.</returns>
		public static Tuple<bool,TResult,Exception> Execute<T, TResult>(Func<T, TResult> exec, T arg)
		{
			try
			{
				return new Tuple<bool,TResult,Exception>(true, exec(arg), null);
			}
			catch (Exception e)
			{
				return new Tuple<bool,TResult,Exception>(false, default(TResult), e);
			}
		}
	}
}
