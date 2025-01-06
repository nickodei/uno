#if __IOS__ || __ANDROID__ || __WASM__
using Uno.Extensions;
using Uno.Foundation.Logging;
using Uno.Helpers;
using Windows.Foundation;

namespace Windows.Devices.Sensors
{
	/// <summary>
	/// Represents a gyrometer sensor that provides angular velocity values with respect to the x, y, and z axes.
	/// </summary>
	public partial class Gyrometer
	{
		private readonly static object _syncLock = new object();

		private static Gyrometer _instance;
		private static bool _initializationAttempted;

		private readonly StartStopEventWrapper<TypedEventHandler<Gyrometer, GyrometerReadingChangedEventArgs>> _readingChangedWrapper;

		/// <summary>
		/// Hides the public parameterless constructor
		/// </summary>
		private Gyrometer()
		{
			_readingChangedWrapper = new StartStopEventWrapper<TypedEventHandler<Gyrometer, GyrometerReadingChangedEventArgs>>(
				() => StartReading(),
				() => StopReading(),
				_syncLock);
		}

		/// <summary>
		/// Returns the default gyrometer.
		/// </summary>
		/// <returns>Null if no integrated gyrometers are found.</returns>
		public static Gyrometer GetDefault()
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
		/// Occurs each time the gyrometer reports the current sensor reading.
		/// </summary>
		public event TypedEventHandler<Gyrometer, GyrometerReadingChangedEventArgs> ReadingChanged
		{
			add => _readingChangedWrapper.AddHandler(value);
			remove => _readingChangedWrapper.RemoveHandler(value);
		}

		private void OnReadingChanged(GyrometerReading reading)
		{
			if (this.Log().IsEnabled(Uno.Foundation.Logging.LogLevel.Debug))
			{
				this.Log().DebugFormat($"Gyrometer reading received " +
					$"X:{reading.AngularVelocityX}, Y:{reading.AngularVelocityY}, Z:{reading.AngularVelocityZ}");
			}
			_readingChangedWrapper.Event?.Invoke(this, new GyrometerReadingChangedEventArgs(reading));
		}
	}
}
#endif
