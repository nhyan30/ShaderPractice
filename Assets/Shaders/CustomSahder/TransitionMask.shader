Shader "UI/TransitionMask"
{
    Properties
    {
        _MainTex ("Image (to reveal)", 2D) = "white" {}
        _MaskTex ("Transition Video", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;
            float4 _MaskTex_ST;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uvMain : TEXCOORD0;
                float2 uvMask : TEXCOORD1;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvMain = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uvMask = TRANSFORM_TEX(v.texcoord, _MaskTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mainCol = tex2D(_MainTex, i.uvMain);
                fixed4 maskCol = tex2D(_MaskTex, i.uvMask);
    
                // Use mask alpha (if your video has transparency)
                float mask = maskCol.a; // or use brightness if alpha not present
                mainCol.a *= mask;
    
                return mainCol;
            }
            ENDCG
        }
    }
}
