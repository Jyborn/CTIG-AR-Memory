using System.Collections.Generic;

namespace GameManagers
{
    public interface GameManager
    {
        List<FlippableCard> MemoryCards { get; set; }

        void GoToScoreScreen();
    }
}