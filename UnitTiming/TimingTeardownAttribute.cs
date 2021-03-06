﻿using System;

namespace UnitTiming
{
	/// <summary>
	/// Marks the method used to finish up a single performance run. This is called
	/// for every run, regardless of the number.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class TimingTeardownAttribute
		: Attribute
	{
	}
}