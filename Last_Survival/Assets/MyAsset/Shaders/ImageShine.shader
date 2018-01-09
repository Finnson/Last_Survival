Shader "ImageShine"
{
	Properties
	{
		_image("image", 2D) = "white" {}
		_percent("_percent", Range(-3.5, 3.5)) = 0
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			sampler2D _image;
			float _percent;

			struct v2f
			{
				float4 pos:SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			fixed4 frag(v2f i) : COLOR0
			{
				fixed4 k = tex2D(_image, i.uv);
				float2 uv = (i.uv + fixed2(_percent, _percent)) * 2;

				fixed2x2 rotMat = fixed2x2(0.866,0.5,-0.5,0.866);
				uv = mul(rotMat, uv);
				
				fixed v = saturate(lerp(fixed(1), fixed(0), abs(uv.y)));
				k += fixed4(v,v,v,v);
				
				return k;
			}
			ENDCG
		}
	}
	FallBack Off
}