Shader "URP/NeonUnlitGlow"
{
    Properties
    {
        [MainTexture] _MainTex ("Main Tex (RGBA)", 2D) = "white" {}
        [HDR] _TintColor ("Base Tint", Color) = (1,1,1,1)
        [HDR] _EmissionColor ("Emission Color", Color) = (0.0, 1.0, 1.0, 1.0)
        _EmissionIntensity ("Emission Intensity", Range(0, 20)) = 5.0

        // Glow(외곽) 세팅
        [HDR] _GlowColor ("Glow Color", Color) = (0.0, 1.0, 1.0, 1.0)
        _GlowWidth ("Glow Width (px)", Range(0, 10)) = 4.0
        _GlowSoftness ("Glow Softness", Range(0.1, 5.0)) = 1.5

        _AlphaCut ("Alpha Cut (optional)", Range(0.0, 1.0)) = 0.001
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "FORWARD"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_TexelSize; // x=1/w, y=1/h
                float4 _TintColor;
                float4 _EmissionColor;
                float  _EmissionIntensity;

                float4 _GlowColor;
                float  _GlowWidth;
                float  _GlowSoftness;

                float  _AlphaCut;
            CBUFFER_END

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;   // SpriteRenderer tint 대응
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            // 주변 알파를 샘플링해 글로우 마스크 생성
            float SampleAlpha(float2 uv)
            {
                float4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                return c.a;
            }

            float fragGlowMask(float2 uv)
            {
                // 픽셀 크기 기준 오프셋
                float2 px = _MainTex_TexelSize.xy * _GlowWidth;
                // 8방향 간단 커널
                float a0 = SampleAlpha(uv);
                float a1 = SampleAlpha(uv + float2( px.x,  0));
                float a2 = SampleAlpha(uv + float2(-px.x,  0));
                float a3 = SampleAlpha(uv + float2( 0,  px.y));
                float a4 = SampleAlpha(uv + float2( 0, -px.y));
                float a5 = SampleAlpha(uv + float2( px.x,  px.y));
                float a6 = SampleAlpha(uv + float2(-px.x,  px.y));
                float a7 = SampleAlpha(uv + float2( px.x, -px.y));
                float a8 = SampleAlpha(uv + float2(-px.x, -px.y));

                float maxN = max(a1, max(a2, max(a3, max(a4, max(a5, max(a6, max(a7, a8)))))));
                // 가장자리 영역만 남기기 (주체 알파 제외) + 소프트
                float glow = saturate((maxN - a0) * _GlowSoftness);
                return glow;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                // 본체
                float4 baseCol = tex * _TintColor * i.color;
                // 글로우 마스크
                float glowMask = fragGlowMask(i.uv);

                // 발광
                float3 emissive = (_EmissionColor.rgb * _EmissionIntensity) * tex.a;
                emissive += _GlowColor.rgb * glowMask * _EmissionIntensity;

                // 최종 색 (언릿)
                float4 outCol = float4(baseCol.rgb + emissive, tex.a);
                // 필요시 알파 컷
                if (outCol.a <= _AlphaCut) discard;
                return outCol;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
