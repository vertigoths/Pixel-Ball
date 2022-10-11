using TMPro;
using UnityEngine;

namespace Voodoo.Utils
{
	public class DemoManager : MonoBehaviour 
	{
		/// Demo debugText
		public TextMeshProUGUI debugTextBox;
		
		protected string platformString;

		/// <summary>
		/// Initialize for iOS
		/// </summary>
		protected virtual void Awake()
		{
			Vibrations.iOSInitializeHaptics ();
		}
		
		protected virtual void Start()
		{
			DisplayInformation ();
		}
		
		protected virtual void DisplayInformation()
		{
			if (Vibrations.Android ())
			{
				platformString = "API version " + Vibrations.AndroidSDKVersion().ToString();
			} 
			else if (Vibrations.iOS ())
			{
				platformString = "iOS " + Vibrations.iOSSDKVersion(); 
			} 
			else
			{
				platformString = Application.platform + ", not supported by Vibrations for now.";
			}

			debugTextBox.text = "Platform : " + platformString +" "+ Vibrations.HapticsSupported().ToString();
		}

		/// <summary>
		/// Release iOS haptics
		/// </summary>
		protected virtual void OnDisable()
		{
			Vibrations.iOSReleaseHaptics ();
		}


		public virtual void TriggerDefault()
		{
			#if UNITY_IOS || UNITY_ANDROID
				Handheld.Vibrate ();	
			#endif
		}


		public virtual void TriggerVibrate()
		{
			Vibrations.Vibrate ();
		}


		public virtual void TriggerSelection()
		{
			Vibrations.Haptic (HapticTypes.Selection);
		}


		public virtual void TriggerSuccess()
		{
			Vibrations.Haptic (HapticTypes.Success);
		}


		public virtual void TriggerWarning()
		{
			Vibrations.Haptic (HapticTypes.Warning);
		}


		public virtual void TriggerFailure()
		{
			Vibrations.Haptic (HapticTypes.Failure);
		}


		public virtual void TriggerLightImpact()
		{
			Vibrations.Haptic (HapticTypes.LightImpact);
		}


		public virtual void TriggerMediumImpact()
		{
			Vibrations.Haptic (HapticTypes.MediumImpact);
		}
		
		public virtual void TriggerHeavyImpact()
		{
			Vibrations.Haptic (HapticTypes.HeavyImpact);
		}
	}
}