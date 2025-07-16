Shader "Custom/SplatBlit"
{
    Properties
    {
        _MaskTex   ("Old Mask",      2D)     = "black" {}   // чёрная по умолчанию
        _SplatTex  ("Splat Texture", 2D)     = "white" {}
        _Color     ("Tint Color",    Color)  = (1,0,0,1)    // красный
        _Info      ("UV.x,y, Scale", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Blend Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MaskTex;
            sampler2D _SplatTex;
            float4   _Color;
            float4   _Info;    // xy = centerUV, z = scale, w unused

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert(appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 prev = tex2D(_MaskTex, i.uv);
                if (_Info.z <= 0) return prev;

                float2 d = (i.uv - _Info.xy)/_Info.z + 0.5;
                if (d.x<0||d.x>1||d.y<0||d.y>1) return prev;

                // форма кисти
                float a = tex2D(_SplatTex, d).a * _Color.a;

                if (_Info.w < 0.5)
                {
                    // red mode: копим в R
                    prev.r = max(prev.r, a * _Color.r);
                }
                else
                {
                    // bruise mode: копим форму в Alpha
                    prev.a = max(prev.a, a);
                }
                return prev;
            }
            ENDCG
        }
    }
}
