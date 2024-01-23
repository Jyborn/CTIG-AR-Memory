namespace DefaultNamespace
{
    public static class ScoreManager
    {
        private static float _globalScore = 0f;
        private static bool _isGameWon = false;

        public static float GlobalScore
        {
            get { return _globalScore; }
            set { _globalScore = value; }
        }
        public static bool IsGameWon
        {
            get { return _isGameWon; }
            set { _isGameWon = value; }
        }
    }
}