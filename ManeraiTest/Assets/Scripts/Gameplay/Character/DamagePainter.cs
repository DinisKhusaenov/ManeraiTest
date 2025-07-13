using System.Collections;
using UnityEngine;

namespace Gameplay.Character
{
    public class DamagePainter : MonoBehaviour
    {
        [Header("Mask Settings")]
        [SerializeField] private Vector2Int _maskSize     = new(1024, 1024);
        [SerializeField] private float       _bruiseDelay = 3f;

        [Header("Splats")]
        [SerializeField] private Texture2D   _redSplat;    // ваша капля покраснения
        [SerializeField] private Texture2D   _bruiseSplat;
        [Header("Shader")]
        [SerializeField] private Shader      _blitShader;

        private Material _blitMaterial;
        private RenderTexture _redMask;
        private RenderTexture _bruiseMask;
        private Material _headMaterial;

        [Header("Tints")] [SerializeField] private Color _redTint = new Color(1f, 0.2f, 0.2f, 1f);
        [SerializeField] private Color _bruiseTint = new Color(0.3f, 0f, 0.4f, 1f);

        static readonly int MainTexID = Shader.PropertyToID("_MainTex");
        static readonly int ColorID = Shader.PropertyToID("_SplatColor");
        static readonly int InfoID = Shader.PropertyToID("_SplatInfo");

        void Awake()
        {
            // 1) создаём материал
            _blitMaterial = new Material(_blitShader);

            // 2) создаём и обнуляем RT
            _redMask = CreateClearedRT();
            _bruiseMask = CreateClearedRT();

            // 3) «вкручиваем» RT в шейдер головы
            var rend = GetComponent<Renderer>();
            _headMaterial = rend.material; // инстанс
            _headMaterial.SetTexture("_RedMask", _redMask);
            _headMaterial.SetTexture("_BruiseMask", _bruiseMask);
        }

        // создаёт RT нужного размера и сразу заливает чёрным (alpha = 0)
        RenderTexture CreateClearedRT()
        {
            var rt = new RenderTexture(_maskSize.x, _maskSize.y, 0, RenderTextureFormat.R8);
            rt.Create();
            // clear
            var old = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = old;
            return rt;
        }

        /// <summary>
        /// Попадание — сразу краснота, потом синяк.
        /// </summary>
        public void PaintAt(Vector2 uv)
        {
            PaintRed(uv);
            StartCoroutine(DelayedBruise(uv));
        }

        void PaintRed(Vector2 uv)
        {
            BlitSplat(_redMask, _redSplat, _redTint, uv, 0.15f);
        }

        IEnumerator DelayedBruise(Vector2 uv)
        {
            yield return new WaitForSeconds(_bruiseDelay);
            BlitSplat(_bruiseMask, _bruiseSplat, _bruiseTint, uv, 0.2f);
        }

        // вот здесь передаём сплэт-текстуру прямо в MainTex,
        // + tint и параметры (uv.xy, scale = z)
        void BlitSplat(RenderTexture target, Texture2D splatTex, Color tint, Vector2 uv, float scale)
        {
            // Сохраняем старое содержимое
            var tmp = RenderTexture.GetTemporary(target.descriptor);
            Graphics.Blit(target, tmp);

            // Настраиваем шейдер
            _blitMaterial.SetTexture(MainTexID, splatTex);
            _blitMaterial.SetColor(ColorID, tint);
            _blitMaterial.SetVector(InfoID, new Vector4(uv.x, uv.y, scale, 0f));

            // Рисуем новый сплэт в маску
            Graphics.Blit(tmp, target, _blitMaterial);
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