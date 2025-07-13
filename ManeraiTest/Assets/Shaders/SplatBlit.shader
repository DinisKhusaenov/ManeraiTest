Shader "Custom/SplatBlit"
{
    Properties
    {
        _MainTex ("SplatTex", 2D) = "white" {}
        _RedMask ("OldMask", 2D) = "white" {}
        _SplatColor ("Color", Color) = (1,1,1,1)
        _SplatInfo ("uv.x, uv.y, scale, _unused", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Pass {
            Tags { "RenderType"="Opaque" }
            LOD 200
    
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
    
            sampler2D _RedMask;
            sampler2D _MainTex;
            float4 _SplatColor;
            float4 _SplatInfo;
    
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };
    
            v2f vert (appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.texcoord;
                return o;
            }
    
            fixed4 frag(v2f i) : SV_Target
            {
                float old = tex2D(_RedMask, i.uv).r;
    
                // вычисляем splat UV
                float2 delta = (i.uv - _SplatInfo.xy) / _SplatInfo.z + 0.5;
                float mask = tex2D(_MainTex, delta).a * _SplatColor.a;
    
                return fixed4(max(old, mask),0,0,0);
            }
            ENDCG
        }
    }
}
