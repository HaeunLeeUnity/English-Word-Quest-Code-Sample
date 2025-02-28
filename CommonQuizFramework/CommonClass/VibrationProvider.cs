namespace CommonQuizFramework.CommonClass
{
    // Vibration 라이브러리를 사용하여 진동 기능 제공하고
    // 활성화 / 비활성화 기능을 추가 구현

    public static class VibrationProvider
    {
        private static bool _useVibration = false;

        public static bool UseVibration
        {
            set => _useVibration = value;
        }

        public static void Initialization()
        {
            Vibration.Init();
        }

        public static void Vibrate()
        {
            if (_useVibration) Vibration.Vibrate();
        }

        public static void VibratePop()
        {
            if (_useVibration) Vibration.VibratePop();
        }

        public static void VibrateNope()
        {
            if (_useVibration) Vibration.VibrateNope();
        }

        public static void VibratePeek()
        {
            if (_useVibration) Vibration.VibratePeek();
        }
    }
}