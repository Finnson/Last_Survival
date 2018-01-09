Shader "Dissolve"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Tex", 2D) = "white" {}
		_DissolveText("Dissolve Noise", 2D) = "white" {}
		_Tile("Noise Tile Size", Range(0, 1)) = 1
		_DissSize("Dissolve Size", Range(0, 1)) = 0.1
		_DissColor("Dissolve Color", Color) = (1,1,1,1)
		_AddColor("Adding Color", Color) = (1,1,1,1)
		_Amount("Amount", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Cull off

		CGPROGRAM
		#pragma target 3.0  
		#pragma surface surf BlinnPhong  

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _DissolveText;
		// 平铺值
		half _Tile;
		// 溶解程度
		half _Amount;
		half _DissSize;
		half4 _DissColor;
		half4 _AddColor;
								
		static half3 finalColor = float3(1,1,1);

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			// 设置主材质和颜色
			o.Albedo = tex.rgb * _Color.rgb;
			// 对裁剪材质进行采样，取R色值
			float ClipTex = tex2D(_DissolveText, IN.uv_MainTex / _Tile).r;

			float ClipAmount = ClipTex - _Amount;
			if (_Amount > 0)
			{
				if (ClipAmount < 0) clip(-0.1);
				else
				{
					// 针对没有被裁剪的点，【裁剪量】小于【裁剪大小】的做处理  
					if (ClipAmount < _DissSize)
					{
						if (_AddColor.x == 0)
							finalColor.x = _DissColor.x;
						else
							finalColor.x = ClipAmount / _DissSize;
						if (_AddColor.y == 0)
							finalColor.y = _DissColor.y;
						else
							finalColor.y = ClipAmount / _DissSize;
						if (_AddColor.z == 0)
							finalColor.z = _DissColor.z;
						else
							finalColor.z = ClipAmount / _DissSize;

						o.Albedo = o.Albedo * finalColor * 2;
					}
				}
			}
			o.Alpha = tex.a * _Color.a;
		}
		ENDCG
	}
}