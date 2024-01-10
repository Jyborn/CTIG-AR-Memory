namespace DefaultNamespace
{
    public static class ScoreManager
    {
        private static float _globalScore = 0f;

        public static float GlobalScore
        {
            get { return _globalScore; }
            set { _globalScore = value; }
        }
    }
}