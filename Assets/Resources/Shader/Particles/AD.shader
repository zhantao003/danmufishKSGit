Shader "Dream/Effect/ADD"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CullMode)]_Cull ("双面显示：off只显示正面   front显示双面显示  back显示背面",int) = 0
        [HDR]_BaseColor("主颜色",color) = (1,1,1,1)
        _MainTex("主贴图", 2D) = "white" {}
        _Opacity ("透明度", range(0, 1)) = 1
            _StencilComp("Stencil Comparison", Float) = 8
    _Stencil("Stencil ID", Float) = 0
    _StencilOp("Stencil Operation", Float) = 0
    _StencilWriteMask("Stencil Write Mask", Float) = 255
    _StencilReadMask("Stencil Read Mask", Float) = 255
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent"
        }
          Stencil
    {
        Ref[_Stencil]
        Comp[_StencilComp]
        Pass[_StencilOp]
        ReadMask[_StencilReadMask]
        WriteMask[_StencilWriteMask]
    }
        Pass
        {
            Blend One One,One One
            Cull [_Cull]
            ZWrite Off

            Name "Unlit"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 clor : COLOR;
            };

            struct Varyings
            {
                float4 clor : COLOR;
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            float4 _MainTex_ST;
            half _Opacity;
            CBUFFER_END
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.clor = v.clor;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 c;
                half4 baseMap = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                c = baseMap * _BaseColor * i.clor;
                half opacity = baseMap.a * _Opacity * i.clor.a;
                return half4(c.rgb * opacity, opacity);
            }
            ENDHLSL
        }
    }
}