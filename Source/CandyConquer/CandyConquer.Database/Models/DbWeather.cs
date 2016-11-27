// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Weathers' table.
	/// </summary>
	[DataEntry(Name = "Weathers", EntryPoint = ConnectionStrings.World)]
	public sealed class DbWeather : SqlModel<DbWeather>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MapId { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string WeatherType { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string WeatherIntensity { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string WeatherAppearance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int WeatherChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string WeatherTime { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string WeatherSeason { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public bool WeatherThunder { get; set; }
	}
}
