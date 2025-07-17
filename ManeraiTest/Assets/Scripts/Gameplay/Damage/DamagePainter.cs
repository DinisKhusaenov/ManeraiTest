using System.Collections;
using Gameplay.StaticData;
using UnityEngine;
using Zenject;

namespace Gameplay.Damage
{
    public class DamagePainter : MonoBehaviour
    {
        private readonly int RedMaskId = Shader.PropertyToID("_RedMask");
        private readonly int BruiseMaskId = Shader.PropertyToID("_BruiseMask");
        
        [SerializeField] private float _markScale = 0.05f;
        [SerializeField] private Material _headMaterial;

        private Material _blitMaterial;
        private RenderTexture _redMask;
        private RenderTexture _bruiseMask;
        private DamageConfig _damageConfig;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _damageConfig = staticDataService.DamageConfig;
            _blitMaterial = new Material(_damageConfig.BlitShader);

            _redMask = CreateClearedRenderTexture();
            _bruiseMask = CreateClearedRenderTexture();

            _headMaterial.SetTexture(RedMaskId, _redMask);
            _headMaterial.SetTexture(BruiseMaskId, _bruiseMask);
        }

        private RenderTexture CreateClearedRenderTexture()
        {
            var renderTexture = new RenderTexture(_damageConfig.MarkSize.x, _damageConfig.MarkSize.y, 0, RenderTextureFormat.ARGB32)
                {
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear
                };
            renderTexture.Create();
            var old = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = old;
            return renderTexture;
        }

        public void PaintAt(Vector2 uv)
        {
            Texture2D bruiseTexture = _damageConfig.GetRandomMark();
            PaintRed(uv, bruiseTexture);
            StartCoroutine(TransitionToBruise(uv, bruiseTexture));
        }

        private void PaintRed(Vector2 uv, Texture2D bruise)
        {
            BlitSplat(_redMask, bruise, _damageConfig.RedTint, uv, _markScale, 0f);
        }

        private IEnumerator TransitionToBruise(Vector2 uv, Texture2D bruise)
        {
            float elapsed = 0f;
            while (elapsed < _damageConfig.BruiseDelay)
            {
                float t = Mathf.Clamp01(elapsed / _damageConfig.BruiseDelay);
                BlitSplat(_bruiseMask, bruise, new Color(1, 1, 1, t), uv, _markScale, 1f);

                elapsed += Time.deltaTime;
                yield return null;
            }
            BlitSplat(_bruiseMask, bruise, Color.white, uv, _markScale, 1f);
        }

        void BlitSplat(
            RenderTexture mask,
            Texture2D splatTex,
            Color tint,
            Vector2 uv,
            float scale,
            float mode
        )
        {
            var tmp = RenderTexture.GetTemporary(mask.descriptor);
            Graphics.Blit(mask, tmp);

            _blitMaterial.SetTexture(_damageConfig.MaskTexID, tmp);
            _blitMaterial.SetTexture(_damageConfig.SplatTexID, splatTex);
            _blitMaterial.SetColor(_damageConfig.ColorID, tint);
            _blitMaterial.SetVector(_damageConfig.InfoID, new Vector4(uv.x, uv.y, scale, mode));

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