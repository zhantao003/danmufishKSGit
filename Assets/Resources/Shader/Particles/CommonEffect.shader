Shader "Dream/Effect/CommonEffect"
{
    Properties
    {
        [Header(Render)]
        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("Src Blend",Float) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("Dst Blend",Float) = 0
        [Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull",int) = 0
        [Enum(Off,0,On,1)]_ZWrite("ZWrite",int) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)]_ZTest("ZTest", int) = 4
        [Header(Stencil)]
        [ForceInt] 
        _StencilRef("StencilRef",float) = 0 
        [Enum(UnityEngine.Rendering.CompareFunction)] 
        _StencilComp("StencilComp",int) =0 
        [Enum(UnityEngine.Rendering.StencilOp)] 
        _StencilOp("StencilOp",int)=0 
        [ForceInt] 
        _StencilReadMask("ReadMask",int)=255 
        [ForceInt] 
        _StencilWriteMask("WriteMask",int)=255 
        [Header(Base)]
        _MainTex ("Base Texture", 2D) = "white" {}
        _Add("是ADD",float) = 0
        _AddTex ("Add Texture", 2D) = "white" {}
        _AddTexUVSpeed("AddTex Speed(XY) Intensity(Z) Rotate(W)", vector) = (0, 0, 1, 0)
        [HDR]_BaseColor ("_BaseColor",color) = (1,1,1,1)
        _Clip("Clip",Range(0.0, 3)) = 0.5
        _Alpha("Alpha",Range(0.0, 1)) = 1
        _MainAlphaPow("MainAlphaPow",Range(0.0, 1)) =0
        _BaseOperate("Speed(XY) Intensity(Z) Rotate(W)", vector) = (0, 0, 1, 0)

        [Header(Mask)]
        [Toggle]_MaskEnabled("Mask Enabled",int) = 0
        _MaskTex("MaskTex",2D) = "white"{}
        _MaskUVSpeed("MaskUVSpeed(XY)", vector) = (0, 0,1, 1)

        [Header(Dissolve)]
        [Toggle]_DissolveEnabled("Dissolve Enabled",int) = 0
        _DissolveTex("DissolveTex", 2D) = "white"{}
        _Dissolve("Dissolve(Mask Tex.R)", Range(0.0001, 1)) = 0.3

        [Header(Distort)]
        [Toggle]_DistortEnabled("Distort Enabled",int) = 0
        _DistortTex("DistortTex",2D) = "white"{}
        _Distort("Distort",Range(0,3)) = 0
        _DistortUVSpeed("DistortUVSpeed(XY)",vector) = (0, 0, 0, 0)

        [Header(VertexMove)]
        [Toggle]_VertexMoveEnabled("Distort Enabled",int) = 0
        _VertexMoveTex("VertexMoveTex",2D) = "white"{}
        _VertexMoveUVSpeed("VertexMoveUVSpeed(XY)",vector) = (0, 0, 0, 0)

        [Header(Rim)]
        [Toggle]_RimEnabled("Rim Enabled",int) = 0
        [HDR]_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", Range(0.000001, 3.0)) = 0.1
        _RimIntensity("Rim Intensity", Range(0.00001, 1.0)) = 1
        _RimAlpha("Rim Alpha", Range(0.00001, 1.0)) = 1

    

    }

    SubShader
    {
        Tags {  "RenderType"="Transparent" "Queue"="Transparent""RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Blend [_SrcBlend] [_DstBlend]//,[_SrcBlend] [_DstBlend]
            Cull [_Cull]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            
            Stencil
        { 
            Ref [_StencilRef] 
            Comp[_StencilComp] 
            Pass[_StencilOp] 
            ReadMask[_StencilReadMask] 
            WriteMask[_StencilWriteMask] 
        }
            Name "Unlit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _ADDTEX_ON
            #pragma multi_compile _ _CLIP_ON
            #pragma multi_compile _ _MASKENABLED_ON
            #pragma multi_compile _ _DISSOLVEENABLED_ON
            #pragma multi_compile _ _DISTORTENABLED_ON
            #pragma multi_compile _ _VERTEXMOVEENABLED_ON   //shader_feature
            #pragma multi_compile _ _RIMENABLED_ON
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                float3 normal   : NORMAL;
                float4 color    : COLOR ;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 uv           : TEXCOORD0;
                float4 vertex       : SV_POSITION;
                float4 uv1          : TEXCOORD1;
                float3 worldNormal  : TEXCOORD2;
                float3 worldViewDir : TEXCOORD3;
                float4 uv2          : TEXCOORD4;
                float4 color        : COLOR ;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            float4 _MainTex_ST, _BaseOperate;
            float _Alpha;
            float4 _MaskTex_ST;
            float4 _MaskUVSpeed;
            sampler2D _DissolveTex;
            float4 _DissolveTex_ST;
            float4 _VertexMoveTex_ST,_VertexMoveUVSpeed;
            float _Dissolve;
            half _Clip;
            sampler2D _DistortTex;
            float4 _AddTex_ST,_AddTexUVSpeed;
            float4 _DistortTex_ST, _DistortUVSpeed, _RimColor;
            float _Distort, _RimPower, _RimIntensity, _BreatheIntensity;
            float _RimAlpha;
            half _MainAlphaPow;
            half _Add;
            CBUFFER_END
            
            sampler2D _AddTex;
            sampler2D _MainTex;
            sampler2D _MaskTex;
            sampler2D _VertexMoveTex;
            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.color = v.color;
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex) + float2(_BaseOperate.xy) * _Time.y;
              
                uv = uv - float2(0.5, 0.5);
                uv = float2(uv.x * cos(_BaseOperate.w) - uv.y * sin(_BaseOperate.w), 
                            uv.x * sin(_BaseOperate.w) + uv.y * cos(_BaseOperate.w));
                
                o.uv.xy = uv + float2(0.5, 0.5);
                #if _MASKENABLED_ON
                    o.uv.zw = TRANSFORM_TEX(v.uv,_MaskTex) + float2(_MaskUVSpeed.xy) * _Time.y;
                #endif
                # if _DISSOLVEENABLED_ON
                    o.uv1.xy = TRANSFORM_TEX(v.uv, _DissolveTex);
                #endif
                
                #if _DISTORTENABLED_ON
                    o.uv1.zw = TRANSFORM_TEX(v.uv,_DistortTex) + float2(_DistortUVSpeed.xy) * _Time.y;
                #endif
                
                #if _ADDTEX_ON
                 o.uv2.xy = TRANSFORM_TEX(v.uv,_AddTex) + float2(_AddTexUVSpeed.xy) * _Time.y;
                #endif

                #if _VERTEXMOVEENABLED_ON
                float2 vertexUV = TRANSFORM_TEX(v.uv,_VertexMoveTex) + float2(_VertexMoveUVSpeed.xy) * _Time.y;
                half VertexMoveTex1 = tex2Dlod(_VertexMoveTex,float4( vertexUV, 0, 0.0)).r;
                v.vertex.xyz =v.vertex.xyz + v.normal*_VertexMoveUVSpeed.z*VertexMoveTex1;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                #else
                 o.vertex = TransformObjectToHClip(v.vertex.xyz);
                #endif

                
                

                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.worldViewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                // UNITY_TRANSFER_INSTANCE_ID(i, v);
                float2 distort = i.uv.xy;
                #if _DISTORTENABLED_ON
                    half4 distortTex = tex2D(_DistortTex,i.uv1.zw);
                    distort = lerp(i.uv.xy, distortTex.rr, _Distort);
                #endif

                
                half4 col = tex2D(_MainTex, distort) *_BaseColor*i.color;

                #if _ADDTEX_ON
                half4 addcol = tex2D(_AddTex, i.uv2.xy) * _AddTexUVSpeed.z;
                col = lerp(col,addcol,_AddTexUVSpeed.w);
                #endif
                
                #if _MASKENABLED_ON
                    half maskTex = tex2D(_MaskTex,i.uv.zw).r;
                    col *= maskTex;
                #endif
                
                #if _DISSOLVEENABLED_ON
                    half4 dissolveTex = tex2D(_DissolveTex,i.uv1.xy);
                    _Dissolve =saturate( _Dissolve*(1- i.color.a));
                    clip(dissolveTex.r - _Dissolve);
                #endif

                half alpha = col.a*_Alpha*i.color.a;

                #if _RIMENABLED_ON
                    half rimx =1- max(0, dot(i.worldViewDir, i.worldNormal));
                    half rim =  pow(rimx, 1 /_RimPower);
				    half3 rimColor = _RimColor * rim * _RimIntensity;
                    col.rgb += rimColor;
                    alpha =lerp(alpha,(1-rim)*alpha,_RimAlpha) ;
                #endif

                #if _CLIP_ON
                clip(col.a-_Clip*(1-i.color.a) );
                #endif
                col.rgb = lerp(col.rgb, col.rgb*alpha,_Add);//col.rgb * max(alpha,_Add);//
                return half4(col.rgb,max( _MainAlphaPow,alpha));
            }
            ENDHLSL
        }
    }
  CustomEditor "CommonEffectShaderEditor"
}
