Shader "ACME/Outline"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Size("Size",Range(0,0.1)) = 0.05
		_Color("color",Color) = (1,1,1,1)
		_OutLineColor("OutLineColor",Color) = (1,1,1,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha
				cull front
				Zwrite off
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal:NORMAL;
				float4 color:COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;

				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _OutLineColor;
			float _Size;

			v2f vert(appdata v)
			{
				v2f o;
				//float4 Vpos = mul(UNITY_MATRIX_MV,v.vertex);
				//float3 Vnormal = mul(UNITY_MATRIX_IT_MV,v.color.xyz - 1);
				//Vnormal.z = -0.05;
				//o.vertex = mul(UNITY_MATRIX_P,Vpos + float4(Vnormal,0)*_Size / 10);
				o.vertex = UnityObjectToClipPos(v.vertex*(1 + _Size));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _OutLineColor;
			}
			ENDCG
		}


		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Zwrite off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal:NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _Color;


				v2f vert(appdata v)
				{
					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return _Color * col;
			}
			ENDCG
		}
		}
}
