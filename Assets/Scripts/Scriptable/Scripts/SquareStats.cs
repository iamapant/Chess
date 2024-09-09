using Rendering;
using UnityEngine;

namespace Scriptable.Scripts {
    [CreateAssetMenu]
    public class SquareStats : ScriptableObject {
        public IRenderAdapter RenderAdapter;
        public int GoldPerTurn;
        public float GoldModifier = 1f;
        public float FaithPerTurn;
        public float FaithMultiplier = 1f;
        public float ExpPerTurn;
        public float MaxExp;
        public float Stability;
        public float MaxStability;
        public float MinStability;
        public float StabilityPerTurn;
        
    }
}