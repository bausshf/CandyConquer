// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Items
{
	/// <summary>
	/// The item additions for + etc.
	/// </summary>
	public sealed class ItemAddition
	{
		/// <summary>
		/// Gets or sets the item Id.
		/// </summary>
		public uint ItemId { get; set; }
		
		/// <summary>
		/// Gets or sets the plus.
		/// </summary>
		public byte Plus { get; set; }
		
		/// <summary>
		/// Gets or sets the HP.
		/// </summary>
		public ushort HP { get; set; }
		
		/// <summary>
		/// Gets or sets the minimum attack.
		/// </summary>
		public uint MinAttack { get; set; }
		
		/// <summary>
		/// Gets or sets the maximum attack.
		/// </summary>
		public uint MaxAttack { get; set; }
		
		/// <summary>
		/// Gets or sets the defense.
		/// </summary>
		public ushort Defense { get; set; }
		
		/// <summary>
		/// Gets or sets the magic attack.
		/// </summary>
		public ushort MagicAttack { get; set; }
		
		/// <summary>
		/// Gets or sets the magic defense.
		/// </summary>
		public ushort MagicDefense { get; set; }
		
		/// <summary>
		/// Gets or sets the dexterity.
		/// </summary>
		public ushort Dexterity { get; set; }
		
		/// <summary>
		/// Gets or sets the dodge.
		/// </summary>
		public byte Dodge { get; set; }
	}
}
