Shader "Outline"
{
	Properties
	{
		_MainColor("Main Color", Color) = (0.5,0.5,0.5,1)
		_TextureDiffuse("Texture Diffuse", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (0.5,0.5,0.5,1)
		_RimPower("Rim Power", Range(0.0, 36)) = 0.1
		_RimIntensity("Rim Intensity", Range(0.0, 100)) = 3
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Pass
		{
			Name "ForwardBase"

			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			#pragma target 3.0

			uniform float4 _LightColor0;
			uniform float4 _MainColor;
			uniform sampler2D _TextureDiffuse;
			uniform float4 _TextureDiffuse_ST;
			uniform float4 _RimColor;
			uniform float _RimPower;
			uniform float _RimIntensity;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 pos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				float4 posWorld : TEXCOORD1;
				LIGHTING_COORDS(3,4)
			};

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o;
				o.texcoord = v.texcoord;
				o.normal = mul(float4(v.normal,0), unity_WorldToObject).xyz;
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				return o;
			}

			fixed4 frag(VertexOutput i) : COLOR
			{
				float3 ViewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				float3 NormalDirection = normalize(i.normal);
				float3 LightDirection = normalize(_WorldSpaceLightPos0.xyz);

				//衰减值
				float Attenuation = LIGHT_ATTENUATION(i);
				//衰减后颜色值
				float3 AttenColor = Attenuation * _LightColor0.xyz;

				//计算漫反射
				float NdotL = dot(NormalDirection, LightDirection);
				float3 Diffuse = max(0.0, NdotL) * AttenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;

				//计算边缘强度
				half Rim = 1.0 - max(0, dot(i.normal, ViewDirection));
				//计算出边缘自发光强度
				float3 Emissive = _RimColor.rgb * pow(Rim,_RimPower) *_RimIntensity;

				//最终颜色 = （漫反射系数 * 纹理颜色 * rgb颜色）+自发光颜色
				float3 finalColor = Diffuse * (tex2D(_TextureDiffuse,TRANSFORM_TEX(i.texcoord.rg, _TextureDiffuse)).rgb*_MainColor.rgb) + Emissive;

				return fixed4(finalColor,1);
			}
			ENDCG
		}
	}

	FallBack "Diffuse"
}