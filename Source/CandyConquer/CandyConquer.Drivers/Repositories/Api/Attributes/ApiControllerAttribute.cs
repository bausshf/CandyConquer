// Project by Bauss
using System;

namespace CandyConquer.Drivers.Repositories.Api.Attributes
{
	/// <summary>
	/// An api controller attribute.
	/// All controller classes should have this attribute declared.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ApiControllerAttribute : Attribute
	{
	}
}
