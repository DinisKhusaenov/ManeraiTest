using UnityEngine;

namespace Gameplay.Damage
{
    [CreateAssetMenu(menuName = "Configs/DamageConfig", fileName = "DamageConfig")]
    public class DamageConfig : ScriptableObject
    {
        [field: SerializeField] public Texture2D[] DamageMarks { get; private set; }
        [field: SerializeField] public Color RedTint { get; private set; }
        [field: SerializeField] public Color BruiseTint { get; private set; }
        [field: SerializeField] public Shader BlitShader { get; private set; }
        [field: SerializeField] public Vector2Int MarkSize { get; private set; } = new(1024, 1024);
        [field: SerializeField] public float BruiseDelay { get; private set; }
        
        public readonly int MaskTexID = Shader.PropertyToID("_MaskTex");
        public readonly int SplatTexID = Shader.PropertyToID("_SplatTex");
        public readonly int ColorID = Shader.PropertyToID("_Color");
        public readonly int InfoID = Shader.PropertyToID("_Info");

        public Texture2D GetRandomMark()
        {
            return DamageMarks[Random.Range(0, DamageMarks.Length - 1)];
        }
    }
}