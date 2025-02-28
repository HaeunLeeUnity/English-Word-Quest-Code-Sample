using System.Collections.Generic;
using UnityEngine;

namespace LHEPackage.Helper
{
    public static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds waitForSec;
            if (!waitForSeconds.TryGetValue(seconds, out waitForSec))
                waitForSeconds.Add(seconds, waitForSec = new WaitForSeconds(seconds));
            return waitForSec;
        }
    }
}