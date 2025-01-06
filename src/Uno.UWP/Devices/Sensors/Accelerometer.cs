﻿#if __IOS__ || __ANDROID__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using Uno.Helpers;
using Windows.Foundation;

namespace Windows.Devices.Sensors
{
	/// <summary>
	/// This sensor returns G-force values with respect to the x, y, and z axes.
	/// </summary>
	public partial class Accelerometer
	{
		private readonly static object _syncLock = new();

		private static Accelerometer _instance;
		private static bool _initializationAttempted;

		private readonly StartStopEventWrapper<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>> _readingChangedWrapper;
		private readonly StartStopEventWrapper<TypedEventHandler<Accelerometer, AccelerometerShakenEventArgs>> _shakenWrapper;

		/// <summary>
		/// Gets or sets the transformation that needs to be applied to sensor data. Transformations to be applied are tied to the display orientation with which to align the sensor data.
		/// </summary>
		/// <remarks>
		/// This is not currently implemented, and acts as if <see cref="ReadingTransform" /> was set to <see cref="Graphics.Display.DisplayOrientations.Portrait" />.
		/// </remarks>
		[Uno.NotImplemented]
		public Graphics.Display.DisplayOrientations ReadingTransform { get; set; } = Graphics.Display.DisplayOrientations.Portrait;

		/// <summary>
		/// Hides the public parameterless constructor
		/// </summary>
		private Accelerometer()
		{
			_readingChangedWrapper = new StartStopEventWrapper<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>>(
				() => StartReadingChanged(),
				() => StopReadingChanged(),
				_syncLock);
			_shakenWrapper = new StartStopEventWrapper<TypedEventHandler<Accelerometer, AccelerometerShakenEventArgs>>(
				() => StartShaken(),
				() => StopShaken(),
				_syncLock);
		}

		/// <summary>
		/// Returns the default accelerometer.
		/// </summary>
		/// <returns>The default accelerometer or null if no integrated accelerometers are found.</returns>
		public static Accelerometer GetDefault()
		{
			if (_initializationAttempted)
			{
				return _instance;
			}
			lock (_syncLock)
			{
				if (!_initializationAttempted)
				{
					_instance = TryCreateInstance();
					_initializationAttempted = true;
				}
				return _instance;
			}
		}

		/// <summary>
		/// Occurs each time the accelerometer reports a new sensor reading.
		/// </summary>
		public event TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> ReadingChanged
		{
			add => _readingChangedWrapper.AddHandler(value);
			remove => _readingChangedWrapper.RemoveHandler(value);
		}

		/// <summary>
		/// Occurs when the accelerometer detects that the device has been shaken.
		/// </summary>
		public event TypedEventHandler<Accelerometer, AccelerometerShakenEventArgs> Shaken
		{
			add => _shakenWrapper.AddHandler(value);
			remove => _shakenWrapper.RemoveHandler(value);
		}

		private void OnReadingChanged(AccelerometerReading reading)
		{
			_readingChangedWrapper.Event?.Invoke(this, new AccelerometerReadingChangedEventArgs(reading));
		}

		internal void OnShaken(DateTimeOffset timestamp)
		{
			_shakenWrapper.Event?.Invoke(this, new AccelerometerShakenEventArgs(timestamp));
		}
	}
}
#endif
