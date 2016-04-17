Shader "Sprites/Overlay"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			float4 Overlay(float4 a, float4 b)
			{
				float4 r = float4(0, 0, 0, 1);
				if (a.r > 0.5) { r.r = 1 - (1 - 2 * (a.r - 0.5))*(1 - b.r); }
				else { r.r = (2 * a.r)*b.r; }

				if (a.g > 0.5) { r.g = 1 - (1 - 2 * (a.g - 0.5))*(1 - b.g); }
				else { r.g = (2 * a.g)*b.g; }

				if (a.b > 0.5) { r.b = 1 - (1 - 2 * (a.b - 0.5))*(1 - b.b); }
				else { r.b = (2 * a.b)*b.b; }

				return r;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 t = SampleSpriteTexture(IN.texcoord);
				fixed4 o = Overlay(t, IN.color);
				
				o.a = t.a;
				o.rgb *= t.a*IN.color.a;
				o.a = t.a*IN.color.a;

				return o;
			}
		ENDCG
		}
	}
}
