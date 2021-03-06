﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UnitTiming.Status;

namespace UnitTiming
{
	/// <summary>
	/// The primary container for running a performance test. This handles the
	/// management of the high-level components and includes the status
	/// listeners and context items.
	/// </summary>
	public class Runner
	{
		#region Status Listeners

		private readonly CompositeStatusListener statusListener =
			new CompositeStatusListener();

		/// <summary>
		/// Gets or sets the status listener.
		/// </summary>
		/// <value>The status listener.</value>
		public CompositeStatusListener StatusListener
		{
			get { return statusListener; }
		}

		#endregion Status Listeners

		#region Assembly Loading

		private readonly List<TypeRunner> assemblyRunners = new List<TypeRunner>();

		/// <summary>
		/// Loads the assembly from the given file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public void LoadAssembly(string filename)
		{
			// Load the file and go through the various types inside
			// the assembly.
			var file = new FileInfo(filename);
			Assembly assembly = Assembly.LoadFile(file.FullName);

			foreach (Type type in assembly.GetTypes())
			{
				// See if the type implements the [TimingFixture].
				object[] attributes =
					type.GetCustomAttributes(typeof(TimingFixtureAttribute), false);

				if (attributes.Length == 0)
				{
					continue;
				}

				// Load this type into memory, pulling out the various tests.
				assemblyRunners.Add(new TypeRunner(this, type));
			}
		}

		#endregion Assembly Loading

		#region Running

		/// <summary>
		/// Runs the various assembly tests.
		/// </summary>
		public void Run()
		{
			foreach (TypeRunner assemblyRunner in assemblyRunners)
			{
				assemblyRunner.Run();
			}
		}

		#endregion
	}
}