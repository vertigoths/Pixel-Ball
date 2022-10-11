using UnityEngine;
using System;
using System.Runtime.InteropServices;
#if UNITY_IOS
	using UnityEngine.iOS;
#endif

namespace Voodoo.Utils
{
	public enum HapticTypes { Selection, Success, Warning, Failure, LightImpact, MediumImpact, HeavyImpact, None }

	public static class Vibrations 
	{
		// INTERFACE ---------------------------------------------------------------------------------------------------------

		public static bool canVibrate = true;
		
		public static long LightDuration = 20;
		public static long MediumDuration = 40;
		public static long HeavyDuration = 80;
		public static int LightAmplitude = 40;
		public static int MediumAmplitude = 120;
		public static int HeavyAmplitude = 255;
		private static int sdkVersion = -1;
        private static long[] _lightimpactPattern = { 0, LightDuration };
        private static int[] _lightimpactPatternAmplitude = { 0, LightAmplitude };
        private static long[] _mediumimpactPattern = { 0, MediumDuration };
        private static int[] _mediumimpactPatternAmplitude = { 0, MediumAmplitude };
        private static long[] _HeavyimpactPattern = { 0, HeavyDuration };
        private static int[] _HeavyimpactPatternAmplitude = { 0, HeavyAmplitude };
        private static long[] _successPattern = { 0, LightDuration, LightDuration, HeavyDuration};
		private static int[] _successPatternAmplitude = { 0, LightAmplitude, 0, HeavyAmplitude};
		private static long[] _warningPattern = { 0, HeavyDuration, LightDuration, MediumDuration};
		private static int[] _warningPatternAmplitude = { 0, HeavyAmplitude, 0, MediumAmplitude};
		private static long[] _failurePattern = { 0, MediumDuration, LightDuration, MediumDuration, LightDuration, HeavyDuration, LightDuration, LightDuration};
		private static int[] _failurePatternAmplitude = { 0, MediumAmplitude, 0, MediumAmplitude, 0, HeavyAmplitude, 0, LightAmplitude};

		public static bool Android()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
				return true;
			#else
				return false;
			#endif
		}

		public static bool iOS()
		{
			#if UNITY_IOS && !UNITY_EDITOR
				return true;
			#else
				return false;
			#endif
		}
		
		/// <summary>
		/// Launch a simple Vibration
		/// </summary>
		public static void Vibrate()
        {
            if (Android ())
			{
				AndroidVibrate (MediumDuration);
			} 
			else if (iOS ())
			{
				iOSTriggerHaptics (HapticTypes.MediumImpact);
			} 
		}


		/// <summary>
		/// Launch a specific vibration
		/// </summary>
		/// <param name="type"></param>
		/// <param name="defaultToRegularVibrate"></param>
		public static void Haptic(HapticTypes type, bool defaultToRegularVibrate = false)
		{
            if (defaultToRegularVibrate)
            {
                #if UNITY_IOS || UNITY_ANDROID
                    Handheld.Vibrate();
                #endif
                return;
            }

			if (Android ())
			{
				switch (type)
				{
                    case HapticTypes.None:
                        // do nothing
                        break;
					case HapticTypes.Selection:
						AndroidVibrate (LightDuration, LightAmplitude);
						break;

					case HapticTypes.Success:
						AndroidVibrate(_successPattern, _successPatternAmplitude, -1);
						break;

					case HapticTypes.Warning:
						AndroidVibrate(_warningPattern, _warningPatternAmplitude, -1);
						break;

					case HapticTypes.Failure:
						AndroidVibrate(_failurePattern, _failurePatternAmplitude, -1);
						break;

					case HapticTypes.LightImpact:
						AndroidVibrate (_lightimpactPattern, _lightimpactPatternAmplitude, -1);
						break;

					case HapticTypes.MediumImpact:
                        AndroidVibrate (_mediumimpactPattern, _mediumimpactPatternAmplitude, -1);
						break;

					case HapticTypes.HeavyImpact:
						AndroidVibrate (_HeavyimpactPattern, _HeavyimpactPatternAmplitude, -1);
						break;
				}
			} 
			else if (iOS ())
			{
				iOSTriggerHaptics (type);
			} 
		}

        // INTERFACE END ---------------------------------------------------------------------------------------------------------

		// ALL

		public static void ClearAllVibration()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidCancelVibrations();
			#elif UNITY_IOS && !UNITY_EDITOR
			iOSReleaseHaptics();
			#endif
		}

        // Android ---------------------------------------------------------------------------------------------------------


#if UNITY_ANDROID && !UNITY_EDITOR
			private static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			private static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			private static AndroidJavaObject AndroidVibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
			private static AndroidJavaClass VibrationEffectClass;
			private static AndroidJavaObject VibrationEffect;
			private static int DefaultAmplitude;
            private static IntPtr AndroidVibrateMethodRawClass = AndroidJNIHelper.GetMethodID(AndroidVibrator.GetRawClass(), "vibrate", "(J)V", false);
            private static jvalue[] AndroidVibrateMethodRawClassParameters = new jvalue[1];
#else
        private static AndroidJavaClass UnityPlayer;
			private static AndroidJavaObject CurrentActivity;
			private static AndroidJavaObject AndroidVibrator = null;
			private static AndroidJavaClass VibrationEffectClass = null;
			private static AndroidJavaObject VibrationEffect;
			private static int DefaultAmplitude;
            private static IntPtr AndroidVibrateMethodRawClass = IntPtr.Zero;
            private static jvalue[] AndroidVibrateMethodRawClassParameters = null;
#endif

        /// <summary>
        /// Requests a default vibration on Android, for the specified duration, in milliseconds
        /// </summary>
        /// <param name="milliseconds">Milliseconds.</param>
        public static void AndroidVibrate(long milliseconds)
		{
			if(!canVibrate){ return; }
			
			if (!Android ()) { return; }
            AndroidVibrateMethodRawClassParameters[0].j = milliseconds;
            AndroidJNI.CallVoidMethod(AndroidVibrator.GetRawObject(), AndroidVibrateMethodRawClass, AndroidVibrateMethodRawClassParameters);
        }

        /// <summary>
        /// Requests a vibration of the specified amplitude and duration. If amplitude is not supported by the device's SDK, a default vibration will be requested
        /// </summary>
        /// <param name="milliseconds">Milliseconds.</param>
        /// <param name="amplitude">Amplitude.</param>
        public static void AndroidVibrate(long milliseconds, int amplitude)
		{
			if(!canVibrate){ return; }
			
			if (!Android ()) { return; }
			// amplitude is only supported after API26
			if ((AndroidSDKVersion() < 26)) 
			{ 
				AndroidVibrate (milliseconds); 
			}
			else
			{
				VibrationEffectClassInitialization ();
				VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject> ("createOneShot", new object[] { milliseconds,	amplitude });
                AndroidVibrator.Call ("vibrate", VibrationEffect);
			}
		}

		// Requests a vibration on Android for the specified pattern and optional repeat
		// Straight out of the Android documentation :
		// Pass in an array of ints that are the durations for which to turn on or off the vibrator in milliseconds. 
		// The first value indicates the number of milliseconds to wait before turning the vibrator on. 
		// The next value indicates the number of milliseconds for which to keep the vibrator on before turning it off. 
		// Subsequent values alternate between durations in milliseconds to turn the vibrator off or to turn the vibrator on.
		// repeat:  the index into pattern at which to repeat, or -1 if you don't want to repeat.
		public static void AndroidVibrate(long[] pattern, int repeat)
		{
			if(!canVibrate){ return; }
			
			if (!Android ()) { return; }
			if ((AndroidSDKVersion () < 26))
			{ 
				AndroidVibrator.Call ("vibrate", pattern, repeat);
			}
			else
			{
				VibrationEffectClassInitialization ();
				VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject> ("createWaveform", new object[] { pattern,	repeat });
                AndroidVibrator.Call ("vibrate", VibrationEffect);
            }
		}

		/// <summary>
		/// Requests a vibration on Android for the specified pattern, amplitude and optional repeat
		/// </summary>
		/// <param name="pattern">Pattern.</param>
		/// <param name="amplitudes">Amplitudes.</param>
		/// <param name="repeat">Repeat.</param>
		public static void AndroidVibrate(long[] pattern, int[] amplitudes, int repeat )
		{
			if(!canVibrate){ return; }
			
			if (!Android ()) { return; }
			if ((AndroidSDKVersion () < 26))
			{ 
				AndroidVibrator.Call ("vibrate", pattern, repeat);
			}
			else
			{
				VibrationEffectClassInitialization ();
				VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject> ("createWaveform", new object[] { pattern,	amplitudes, repeat });
				AndroidVibrator.Call ("vibrate", VibrationEffect);
			}
		}

		/// <summary>
		/// Stops all Android vibrations that may be active
		/// </summary>
		public static void AndroidCancelVibrations()
		{
			if (!Android ()) { return; }
			AndroidVibrator.Call("cancel");
		}

		/// <summary>
		/// Initializes the VibrationEffectClass if needed.
		/// </summary>
		private static void VibrationEffectClassInitialization ()
		{
			if (VibrationEffectClass == null)
            {
                VibrationEffectClass = new AndroidJavaClass ("android.os.VibrationEffect");
            }	
		}

		/// <summary>
		/// Returns the current Android SDK version as an int
		/// </summary>
		/// <returns>The SDK version.</returns>
		public static int AndroidSDKVersion() 
		{
			if (sdkVersion == -1)
			{
				int apiLevel = int.Parse (SystemInfo.operatingSystem.Substring(SystemInfo.operatingSystem.IndexOf("-") + 1, 3));
				sdkVersion = apiLevel;
				return apiLevel;	
			}
			else
			{
				return sdkVersion;
			}
		}
			
		// Android End ---------------------------------------------------------------------------------------------------------

		// iOS ----------------------------------------------------------------------------------------------------------------


		#if UNITY_IOS && !UNITY_EDITOR
			[DllImport ("__Internal")]
			private static extern void InstantiateFeedbackGenerators();
			[DllImport ("__Internal")]
			private static extern void ReleaseFeedbackGenerators();
			[DllImport ("__Internal")]
			private static extern void SelectionHaptic();
			[DllImport ("__Internal")]
			private static extern void SuccessHaptic();
			[DllImport ("__Internal")]
			private static extern void WarningHaptic();
			[DllImport ("__Internal")]
			private static extern void FailureHaptic();
			[DllImport ("__Internal")]
			private static extern void LightImpactHaptic();
			[DllImport ("__Internal")]
			private static extern void MediumImpactHaptic();
			[DllImport ("__Internal")]
			private static extern void HeavyImpactHaptic();
		#else
			private static void InstantiateFeedbackGenerators() {}
			private static void ReleaseFeedbackGenerators() {}
			private static void SelectionHaptic() {}
			private static void SuccessHaptic() {}
			private static void WarningHaptic() {}
			private static void FailureHaptic() {}
			private static void LightImpactHaptic() {}
			private static void MediumImpactHaptic() {}
			private static void HeavyImpactHaptic() {}
		#endif
		private static bool iOSHapticsInitialized = false;


		public static void iOSInitializeHaptics()
		{

			if (!iOS ()) { return; }
			InstantiateFeedbackGenerators ();
			iOSHapticsInitialized = true;
		}


		public static void iOSReleaseHaptics ()
		{
			if (!iOS ()) { return; }
			ReleaseFeedbackGenerators ();
		}


		public static bool HapticsSupported()
		{
			bool hapticsSupported = false;
#if UNITY_IOS
			DeviceGeneration generation = Device.generation;
			if ((generation == DeviceGeneration.iPhone3G)
			|| (generation == DeviceGeneration.iPhone3GS)
			|| (generation == DeviceGeneration.iPodTouch1Gen)
			|| (generation == DeviceGeneration.iPodTouch2Gen)
			|| (generation == DeviceGeneration.iPodTouch3Gen)
			|| (generation == DeviceGeneration.iPodTouch4Gen)
			|| (generation == DeviceGeneration.iPhone4)
			|| (generation == DeviceGeneration.iPhone4S)
			|| (generation == DeviceGeneration.iPhone5)
			|| (generation == DeviceGeneration.iPhone5C)
			|| (generation == DeviceGeneration.iPhone5S)
			|| (generation == DeviceGeneration.iPhone6)
			|| (generation == DeviceGeneration.iPhone6Plus)
			|| (generation == DeviceGeneration.iPhone6S)
            || (generation == DeviceGeneration.iPhoneSE1Gen)
			|| (generation == DeviceGeneration.iPhone6SPlus))
			{
			hapticsSupported = false;
			}
			else
			{
			hapticsSupported = true;
			}
#endif

            return hapticsSupported;
		}
	

		public static void iOSTriggerHaptics(HapticTypes type)
		{
			if(!canVibrate){ return; }
			
			if (!iOS ()) { return; }

			if (!iOSHapticsInitialized)
			{
				iOSInitializeHaptics ();
			}

			if (HapticsSupported())
			{
				switch (type)
				{
					case HapticTypes.Selection:
						SelectionHaptic ();
						break;

					case HapticTypes.Success:
						SuccessHaptic ();
						break;

					case HapticTypes.Warning:
						WarningHaptic ();
						break;

					case HapticTypes.Failure:
						FailureHaptic ();
						break;

					case HapticTypes.LightImpact:
						LightImpactHaptic ();
						break;

					case HapticTypes.MediumImpact:
						MediumImpactHaptic ();
						break;

					case HapticTypes.HeavyImpact:
						HeavyImpactHaptic ();
						break;
				}
			}
		}
		
		public static string iOSSDKVersion() 
		{
			#if UNITY_IOS && !UNITY_EDITOR
				return Device.systemVersion;
			#else
				return null;
			#endif
		}

		// iOS End ----------------------------------------------------------------------------------------------------------------
	}
}