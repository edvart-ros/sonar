
Shader "Unlit/SonarReplace"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap ("Texture", 2D) = "white" {}
        _SonarFOV ("float", float) = 60.0
        _SonarRange ("float", float) = 20.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT; // Add tangent vector to appdata
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normalWS : TEXCOORD2;
                float3 posWS : TEXCOORD3;
                float3 tangentWS : TEXCOORD4; // Tangent in world space
                float3 bitangentWS : TEXCOORD5; // Bitangent in world space
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _SonarFOV;
            float _SonarRange;

            sampler2D _NormalMap;
            float4 _NormalMap_ST;
	        static const float Rad2Deg = 180.0 / UNITY_PI;
	        static const float Deg2Rad = 3.14 / 180.0;


            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.posWS = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                o.normalWS = UnityObjectToWorldNormal(v.normal);
                o.tangentWS = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.bitangentWS = cross(o.normalWS, o.tangentWS) * v.tangent.w;
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float3 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 viewDirWS = normalize(i.posWS - _WorldSpaceCameraPos);
                float3 viewDirVS = -normalize(UnityWorldSpaceViewDir(i.posWS));

                float3 normalSample = tex2D(_NormalMap, i.uv).xyz * 2.0 - 1.0;
                float3 normalWS = normalize(
                    normalSample.x * i.tangentWS +
                    normalSample.y * i.bitangentWS +
                    normalSample.z * i.normalWS);

                float intensity = -dot(viewDirWS, normalWS);
                float d = length(i.posWS - _WorldSpaceCameraPos);
                
                float azimuthDeg = atan2(viewDirVS.x, viewDirVS.z)*Rad2Deg;
                float azimuthNormed = azimuthDeg/(_SonarFOV/2.0);
                return float3(2, 0, 0);
                return float3(clamp(d/_SonarRange, 0, 1), intensity, azimuthNormed);
            }
            ENDCG
        }
    }
}
