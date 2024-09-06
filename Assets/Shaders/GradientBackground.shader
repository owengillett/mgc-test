Shader "Custom/GradientBackground"
 {
     Properties
     {
         _Color1 ("Color 1", Color) = (1.0, 1.0, 1.0, 1.0)
         _Color2 ("Color 2", Color) = (0.75, 0.75, 0.75, 1.0)
     }
     SubShader
     {
         Tags { "RenderType"="Opaque" "Queue"="Background" }
         LOD 100
 
         ZWrite Off
         Cull Off
 
         Pass
         {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
 
             #include "UnityCG.cginc"
 
             fixed4 _Color1;
             fixed4 _Color2;
             fixed4 _Color3;
             fixed4 _Color4;
 
             half _Pos1;
             half _Pos2;
 
             struct appdata
             {
                 float4 vertex : POSITION;
                 float2 uv : TEXCOORD0;
             };
 
             struct v2f
             {
                 float4 pos : SV_POSITION;
                 float4 uv : TEXCOORD0;
             };
 
             v2f vert (appdata v)
             {
                 v2f o;
                 o.pos = UnityObjectToClipPos (v.vertex);
                 o.uv = ComputeScreenPos (o.pos);
                 return o;
             }
 
             //This is a slightly cheaper version of smoothstep for linear gradients
             float linstep (float a, float b, float x)
             {
                 return saturate ((x - a) / (b - a));
             }
 
             fixed4 frag (v2f i) : SV_Target
             {
                 //screen-space UVs
                 float2 uv = i.uv.xy / i.uv.w;
                  return lerp (_Color1, _Color2, uv.y);
             }
             ENDCG
         }
     }
 }