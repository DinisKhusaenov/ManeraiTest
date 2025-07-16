using System.Collections;
using UnityEngine;

namespace Gameplay.Character
{
    public class DamagePainter : MonoBehaviour
    {
        [Header("Mask Settings")]
        [SerializeField]
        private Vector2Int _maskSize = new(1024, 1024);

        [SerializeField] private float _bruiseDelay = 3f;
        [SerializeField] private float _scale = 0.05f;

        [Header("Splats")] 
        [SerializeField] private Texture2D _redSplat;
        [SerializeField] private Texture2D _bruiseSplat;
        [Header("Shader")] 
        [SerializeField] private Shader _blitShader;
        [SerializeField] private Material _headMaterial;

        private Material _blitMaterial;
        private RenderTexture _redMask;
        private RenderTexture _bruiseMask;

        [Header("Tints")] [SerializeField] private Color _redTint = new Color(1f, 0.2f, 0.2f, 1f);
        [SerializeField] private Color _bruiseTint = new Color(0.3f, 0f, 0.4f, 1f);

        static readonly int MaskTexID  = Shader.PropertyToID("_MaskTex");
        static readonly int SplatTexID = Shader.PropertyToID("_SplatTex");
        static readonly int ColorID    = Shader.PropertyToID("_Color");
        static readonly int InfoID     = Shader.PropertyToID("_Info");

        void Awake()
        {
            // 1) создаём материал
            _blitMaterial = new Material(_blitShader);

            // 2) создаём и обнуляем RT
            _redMask = CreateClearedRT();
            _bruiseMask = CreateClearedRT();

            _headMaterial.SetTexture("_RedMask", _redMask);
            _headMaterial.SetTexture("_BruiseMask", _bruiseMask);
        }

        // создаёт RT нужного размера и сразу заливает чёрным (alpha = 0)
        RenderTexture CreateClearedRT()
        {
            var rt = new RenderTexture(_maskSize.x, _maskSize.y, 0, RenderTextureFormat.ARGB32);
            rt.wrapMode = TextureWrapMode.Clamp;
            rt.filterMode = FilterMode.Bilinear;
            rt.Create();
            // clear
            var old = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = old;
            return rt;
        }

        public void PaintAt(Vector2 uv)
        {
            PaintRed(uv);
            StartCoroutine(DelayedBruise(uv));
        }

        void PaintRed(Vector2 uv)
        {
            BlitSplat(_redMask, _redSplat, _redTint, uv, _scale, 0f);
        }

        IEnumerator DelayedBruise(Vector2 uv)
        {
            yield return new WaitForSeconds(_bruiseDelay);
            BlitSplat(_bruiseMask, _redSplat, _bruiseTint, uv, _scale, 1f);
        }

        void BlitSplat(
            RenderTexture mask,
            Texture2D     splatTex,
            Color         tint,
            Vector2       uv,
            float         scale,
            float         mode     // ← наш новый параметр
        )
        {
            var tmp = RenderTexture.GetTemporary(mask.descriptor);
            Graphics.Blit(mask, tmp);

            _blitMaterial.SetTexture("_MaskTex", tmp);
            _blitMaterial.SetTexture("_SplatTex", splatTex);
            _blitMaterial.SetColor("_Color", tint);
            // последний компонент _Info.w = mode
            _blitMaterial.SetVector(InfoID, new Vector4(uv.x, uv.y, scale, mode));

            Graphics.Blit(null, mask, _blitMaterial);
            RenderTexture.ReleaseTemporary(tmp);
        }

        void OnDestroy()
        {
            Destroy(_blitMaterial);
            _redMask.Release();
            _bruiseMask.Release();
        }
    }
}