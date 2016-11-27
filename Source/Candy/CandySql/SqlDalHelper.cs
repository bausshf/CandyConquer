// Project by Bauss
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Candy
{
	/// <summary>
	/// A helper class for Dal repositories.
	/// </summary>
	public static class SqlDalHelper
	{
		/// <summary>
		/// Gets all rows based on a specific model and sql query.
		/// </summary>
		/// <param name="connString">The connection string.</param>
		/// <param name="sql">The sql query.</param>
		/// <param name="pars">The parameters.</param>
		/// <returns>A list of all the rows.</returns>
		public static List<TModel> GetAll<TModel>(string connString, string sql, Dictionary<string,object> pars)
			where TModel : InternalDataModel, new()
		{
			var models = new List<TModel>();
			
			using (var reader = Sql.ExecuteReader(connString, sql, pars))
			{
				MethodInfo fillMethod = null;
				while (reader.Read())
				{
					var model = new TModel();
					
					if (fillMethod == null)
					{
						model.Fill(reader, out fillMethod);
					}
					else
					{
						model.InvokeFill(reader, fillMethod);
					}
					
					models.Add(model);
				}
			}
			
			return models;
		}
		
		/// <summary>
		/// Gets a specific row based on a model and sql query.
		/// </summary>
		/// <param name="connString">The connection string.</param>
		/// <param name="sql">The sql query.</param>
		/// <param name="pars">The parameters.</param>
		/// <returns>The row if found, default(TModel) otherwise.</returns>
		public static TModel Get<TModel>(string connString, string sql, Dictionary<string,object> pars)
			where TModel : InternalDataModel, new()
		{
			using (var reader = Sql.ExecuteReader(connString, sql, pars))
			{
				if (reader.Read())
				{
					var model = new TModel();
					model.Fill(reader);
					return model;
				}
				else
				{
					return default(TModel);
				}
			}
		}
		
		/// <summary>
		/// Creates models from a collection of models.
		/// </summary>
		/// <param name="models">The models to create.</param>
		/// <returns>An integer describing the amount of models created.</returns>
		public static int Create<TModel>(this IEnumerable<TModel> models)
			where TModel : DataModel<TModel,SqlModel<TModel>,SqlFiller>
		{
			int count = 0;
			foreach (var model in models)
			{
				if (model.Create())
				{
					count++;
				}
			}
			
			return count;
		}
		
		/// <summary>
		/// Deletes models from a collection of models.
		/// </summary>
		/// <param name="models">The models to create.</param>
		/// <returns>An integer describing the amount of models deleted.</returns>
		public static int Delete<TModel>(this IEnumerable<TModel> models)
			where TModel : DataModel<TModel,SqlModel<TModel>,SqlFiller>
		{
			int count = 0;
			foreach (var model in models)
			{
				if (model.Delete())
				{
					count++;
				}
			}
			
			return count;
		}
		
		/// <summary>
		/// Updates models from a collection of models.
		/// </summary>
		/// <param name="models">The models to update.</param>
		/// <returns>An integer describing the amount of models updated.</returns>
		public static int Update<TModel>(this IEnumerable<TModel> models)
			where TModel : DataModel<TModel,SqlModel<TModel>,SqlFiller>
		{
			int count = 0;
			foreach (var model in models)
			{
				if (model.Update())
				{
					count++;
				}
			}
			
			return count;
		}
	}
}
