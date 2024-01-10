using System.Collections.Generic;

namespace GameManagers
{
    public interface GameManager
    {
        static float Score { get; set; }
        List<FlippableCard> MemoryCards { get; set; }

        void GoToScoreScreen();
    }
}