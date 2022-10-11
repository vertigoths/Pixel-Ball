using UnityEngine;

namespace Voodoo.Utils
{
    public class VibrationsInitialization : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            Vibrations.ClearAllVibration();
        }
    }
}
