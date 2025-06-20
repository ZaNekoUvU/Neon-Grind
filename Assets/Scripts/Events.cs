using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static class GameEvents
    {
        public static Action OnFirstBossDefeated;
        public static Action<int> OnScoreChanged;
        public static Action OnSecondBossTriggered;
    }
}
