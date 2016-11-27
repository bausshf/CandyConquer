// Project by Bauss
using System;
using CandyConquer.Security.Api;

namespace CandyConquer.Drivers.Repositories.Api.Attributes
{
	/// <summary>
	/// An api call attribute.
	/// Declares a method as an api call.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ApiCallAttribute : Attribute
	{
		/// <summary>
		/// The security of the call.
		/// </summary>
		public CallSecurity CallSecurity { get; set; }
		/// <summary>
		/// The identity of the call.
		/// </summary>
		public ushort Identity { get; set; }
		
		
		/// <summary>
		/// The sub identity of the call.
		/// </summary>
		private uint _subIdentity = uint.MaxValue;
		
		/// <summary>
		/// Gets or sets the sub identity of the call.
		/// </summary>
		public uint SubIdentity
		{
			get { return _subIdentity; }
			set
			{
				_subIdentity = value;
			}
		}
		
		/// <summary>
		/// Boolean determining whether the call is returning the packet type or not.
		/// </summary>
		public bool TypeReturner { get; set; }
	}
}
