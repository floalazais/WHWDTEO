Shader "WoolMaster"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Color", Color) = (1,1,1,1)
[HideInInspector] _RenderQueueType("Vector1 ", Float) = 1
[HideInInspector] _StencilRef("Vector1 ", Int) = 0
[HideInInspector] _StencilWriteMask("Vector1 ", Int) = 3
[HideInInspector] _StencilRefDepth("Vector1 ", Int) = 0
[HideInInspector] _StencilWriteMaskDepth("Vector1 ", Int) = 32
[HideInInspector] _StencilRefMV("Vector1 ", Int) = 128
[HideInInspector] _StencilWriteMaskMV("Vector1 ", Int) = 128
[HideInInspector] _StencilRefDistortionVec("Vector1 ", Int) = 64
[HideInInspector] _StencilWriteMaskDistortionVec("Vector1 ", Int) = 64
[HideInInspector] _StencilWriteMaskGBuffer("Vector1 ", Int) = 3
[HideInInspector] _StencilRefGBuffer("Vector1 ", Int) = 2
[HideInInspector] _ZTestGBuffer("Vector1 ", Int) = 4
[HideInInspector][ToggleUI] _RequireSplitLighting("Boolean", Float) = 0
[HideInInspector][ToggleUI] _ReceivesSSR("Boolean", Float) = 1
[HideInInspector] _SurfaceType("Vector1 ", Float) = 0
[HideInInspector] _BlendMode("Vector1 ", Float) = 0
[HideInInspector] _SrcBlend("Vector1 ", Float) = 1
[HideInInspector] _DstBlend("Vector1 ", Float) = 0
[HideInInspector] _AlphaSrcBlend("Vector1 ", Float) = 1
[HideInInspector] _AlphaDstBlend("Vector1 ", Float) = 0
[HideInInspector][ToggleUI] _ZWrite("Boolean", Float) = 0
[HideInInspector] _CullMode("Vector1 ", Float) = 2
[HideInInspector] _TransparentSortPriority("Vector1 ", Int) = 0
[HideInInspector] _CullModeForward("Vector1 ", Float) = 2
[HideInInspector][Enum(Front, 1, Back, 2)] _TransparentCullMode("Vector1", Float) = 2
[HideInInspector] _ZTestDepthEqualForOpaque("Vector1 ", Int) = 4
[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("Vector1", Float) = 4
[HideInInspector][ToggleUI] _TransparentBackfaceEnable("Boolean", Float) = 0
[HideInInspector][ToggleUI] _AlphaCutoffEnable("Boolean", Float) = 0
[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
[HideInInspector][ToggleUI] _UseShadowThreshold("Boolean", Float) = 0
[HideInInspector][ToggleUI] _DoubleSidedEnable("Boolean", Float) = 0
[HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Vector1", Float) = 2
[HideInInspector] _DoubleSidedConstants("Vector4", Vector) = (1,1,-1,0)

	}
		SubShader
	{
		Tags
		{
			"RenderPipeline" = "HDRenderPipeline"
			"RenderType" = "HDLitShader"
			"Queue" = "Geometry+0"
		}

		Stencil {
			Ref 1
			Comp always
			Pass replace
		}

		Pass
		{
		// based on HDLitPass.template
		Name "ShadowCaster"
		Tags { "LightMode" = "ShadowCaster" }

		//-------------------------------------------------------------------------------------
		// Render Modes (Blend, Cull, ZTest, Stencil, etc)
		//-------------------------------------------------------------------------------------

		Cull[_CullMode]


		ZWrite On

		ZClip[_ZClip]


		ColorMask 0

		//-------------------------------------------------------------------------------------
		// End Render Modes
		//-------------------------------------------------------------------------------------

		HLSLPROGRAM

		#pragma target 4.5
		#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
		//#pragma enable_d3d11_debug_symbols

		#pragma multi_compile_instancing
	#pragma instancing_options renderinglayer

		#pragma multi_compile _ LOD_FADE_CROSSFADE

		#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local _DOUBLESIDED_ON
		#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

		//-------------------------------------------------------------------------------------
		// Variant Definitions (active field translations to HDRP defines)
		//-------------------------------------------------------------------------------------
		// #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
		// #define _MATERIAL_FEATURE_TRANSMISSION 1
		// #define _MATERIAL_FEATURE_ANISOTROPY 1
		// #define _MATERIAL_FEATURE_IRIDESCENCE 1
		// #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
		// #define _ENABLE_FOG_ON_TRANSPARENT 1
		// #define _AMBIENT_OCCLUSION 1
		// #define _SPECULAR_OCCLUSION_FROM_AO 1
		// #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
		// #define _SPECULAR_OCCLUSION_CUSTOM 1
		#define _ENERGY_CONSERVING_SPECULAR 1
		// #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
		// #define _HAS_REFRACTION 1
		// #define _REFRACTION_PLANE 1
		// #define _REFRACTION_SPHERE 1
		// #define _DISABLE_DECALS 1
		// #define _DISABLE_SSR 1
		// #define _ADD_PRECOMPUTED_VELOCITY
		// #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
		// #define _DEPTHOFFSET_ON 1
		// #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

		//-------------------------------------------------------------------------------------
		// End Variant Definitions
		//-------------------------------------------------------------------------------------

		#pragma vertex Vert
		#pragma fragment Frag

		// If we use subsurface scattering, enable output split lighting (for forward pass)
		#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
		#define OUTPUT_SPLIT_LIGHTING
		#endif

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"

		// define FragInputs structure
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

		//-------------------------------------------------------------------------------------
		// Defines
		//-------------------------------------------------------------------------------------
				#define SHADERPASS SHADERPASS_SHADOWS
			// ACTIVE FIELDS:
			//   Material.Standard
			//   Specular.EnergyConserving
			//   SurfaceDescriptionInputs.TangentSpaceNormal
			//   SurfaceDescription.Alpha

		// this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
		// #define ATTRIBUTES_NEED_NORMAL
		// #define ATTRIBUTES_NEED_TANGENT
		// #define ATTRIBUTES_NEED_TEXCOORD0
		// #define ATTRIBUTES_NEED_TEXCOORD1
		// #define ATTRIBUTES_NEED_TEXCOORD2
		// #define ATTRIBUTES_NEED_TEXCOORD3
		// #define ATTRIBUTES_NEED_COLOR
		// #define VARYINGS_NEED_POSITION_WS
		// #define VARYINGS_NEED_TANGENT_TO_WORLD
		// #define VARYINGS_NEED_TEXCOORD0
		// #define VARYINGS_NEED_TEXCOORD1
		// #define VARYINGS_NEED_TEXCOORD2
		// #define VARYINGS_NEED_TEXCOORD3
		// #define VARYINGS_NEED_COLOR
		// #define VARYINGS_NEED_CULLFACE
		// #define HAVE_MESH_MODIFICATION

	// We need isFontFace when using double sided
	#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
		#define VARYINGS_NEED_CULLFACE
	#endif

		//-------------------------------------------------------------------------------------
		// End Defines
		//-------------------------------------------------------------------------------------

		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
	#ifdef DEBUG_DISPLAY
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
	#endif

		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

	#if (SHADERPASS == SHADERPASS_FORWARD)
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

		#define HAS_LIGHTLOOP

		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
	#else
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
	#endif

		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

		// Used by SceneSelectionPass
		int _ObjectId;
		int _PassValue;

		//-------------------------------------------------------------------------------------
		// Interpolator Packing And Struct Declarations
		//-------------------------------------------------------------------------------------
	// Generated Type: AttributesMesh
	struct AttributesMesh {
		float3 positionOS : POSITION;
		#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : INSTANCEID_SEMANTIC;
		#endif // UNITY_ANY_INSTANCING_ENABLED
	};

	// Generated Type: VaryingsMeshToPS
	struct VaryingsMeshToPS {
		float4 positionCS : SV_Position;
		#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
		#endif // UNITY_ANY_INSTANCING_ENABLED
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
	};
	struct PackedVaryingsMeshToPS {
		float4 positionCS : SV_Position; // unpacked
		#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
		#endif // UNITY_ANY_INSTANCING_ENABLED
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
		#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
	};
	PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
	{
		PackedVaryingsMeshToPS output;
		output.positionCS = input.positionCS;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif // UNITY_ANY_INSTANCING_ENABLED
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		return output;
	}
	VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
	{
		VaryingsMeshToPS output;
		output.positionCS = input.positionCS;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif // UNITY_ANY_INSTANCING_ENABLED
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		return output;
	}

	// Generated Type: VaryingsMeshToDS
	struct VaryingsMeshToDS {
		float3 positionRWS;
		float3 normalWS;
		#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
		#endif // UNITY_ANY_INSTANCING_ENABLED
	};
	struct PackedVaryingsMeshToDS {
		float3 interp00 : TEXCOORD0; // auto-packed
		float3 interp01 : TEXCOORD1; // auto-packed
		#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
		#endif // UNITY_ANY_INSTANCING_ENABLED
	};
	PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
	{
		PackedVaryingsMeshToDS output;
		output.interp00.xyz = input.positionRWS;
		output.interp01.xyz = input.normalWS;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif // UNITY_ANY_INSTANCING_ENABLED
		return output;
	}
	VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
	{
		VaryingsMeshToDS output;
		output.positionRWS = input.interp00.xyz;
		output.normalWS = input.interp01.xyz;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif // UNITY_ANY_INSTANCING_ENABLED
		return output;
	}

	//-------------------------------------------------------------------------------------
	// End Interpolator Packing And Struct Declarations
	//-------------------------------------------------------------------------------------

	//-------------------------------------------------------------------------------------
	// Graph generated code
	//-------------------------------------------------------------------------------------
			// Shared Graph Properties (uniform inputs)
			CBUFFER_START(UnityPerMaterial)
			float4 _EmissionColor;
			float _RenderQueueType;
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			CBUFFER_END


				// Pixel Graph Inputs
					struct SurfaceDescriptionInputs {
						float3 TangentSpaceNormal; // optional
					};
			// Pixel Graph Outputs
				struct SurfaceDescription
				{
					float Alpha;
				};

				// Shared Graph Node Functions
				// Pixel Graph Evaluation
					SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
					{
						SurfaceDescription surface = (SurfaceDescription)0;
						surface.Alpha = 1;
						return surface;
					}

					//-------------------------------------------------------------------------------------
					// End graph generated code
					//-------------------------------------------------------------------------------------

				// $include("VertexAnimation.template.hlsl")


				//-------------------------------------------------------------------------------------
				// TEMPLATE INCLUDE : SharedCode.template.hlsl
				//-------------------------------------------------------------------------------------
					FragInputs BuildFragInputs(VaryingsMeshToPS input)
					{
						FragInputs output;
						ZERO_INITIALIZE(FragInputs, output);

						// Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
						// TODO: this is a really poor workaround, but the variable is used in a bunch of places
						// to compute normals which are then passed on elsewhere to compute other values...
						output.tangentToWorld = k_identity3x3;
						output.positionSS = input.positionCS;       // input.positionCS is SV_Position

						// output.positionRWS = input.positionRWS;
						// output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
						// output.texCoord0 = input.texCoord0;
						// output.texCoord1 = input.texCoord1;
						// output.texCoord2 = input.texCoord2;
						// output.texCoord3 = input.texCoord3;
						// output.color = input.color;
						#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
						output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
						#elif SHADER_STAGE_FRAGMENT
						// output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
						#endif // SHADER_STAGE_FRAGMENT

						return output;
					}

					SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
					{
						SurfaceDescriptionInputs output;
						ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

						// output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
						// output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
						// output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
						output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
						// output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
						// output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
						// output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
						// output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
						// output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
						// output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
						// output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
						// output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
						// output.WorldSpaceViewDirection =     normalize(viewWS);
						// output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
						// output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
						// float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
						// output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
						// output.WorldSpacePosition =          GetAbsolutePositionWS(input.positionRWS);
						// output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
						// output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
						// output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
						// output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
						// output.uv0 =                         input.texCoord0;
						// output.uv1 =                         input.texCoord1;
						// output.uv2 =                         input.texCoord2;
						// output.uv3 =                         input.texCoord3;
						// output.VertexColor =                 input.color;
						// output.FaceSign =                    input.isFrontFace;
						// output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value

						return output;
					}

					// existing HDRP code uses the combined function to go directly from packed to frag inputs
					FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
					{
						UNITY_SETUP_INSTANCE_ID(input);
						VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
						return BuildFragInputs(unpacked);
					}

					//-------------------------------------------------------------------------------------
					// END TEMPLATE INCLUDE : SharedCode.template.hlsl
					//-------------------------------------------------------------------------------------


						void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
						{
							// setup defaults -- these are used if the graph doesn't output a value
							ZERO_INITIALIZE(SurfaceData, surfaceData);

							// specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
							// however specularOcclusion can come from the graph, so need to be init here so it can be override.
							surfaceData.specularOcclusion = 1.0;

							// copy across graph values, if defined
							// surfaceData.baseColor =                 surfaceDescription.Albedo;
							// surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
							// surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
							// surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
							// surfaceData.metallic =                  surfaceDescription.Metallic;
							// surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
							// surfaceData.thickness =                 surfaceDescription.Thickness;
							// surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
							// surfaceData.specularColor =             surfaceDescription.Specular;
							// surfaceData.coatMask =                  surfaceDescription.CoatMask;
							// surfaceData.anisotropy =                surfaceDescription.Anisotropy;
							// surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
							// surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;

					#ifdef _HAS_REFRACTION
							if (_EnableSSRefraction)
							{
								// surfaceData.ior =                       surfaceDescription.RefractionIndex;
								// surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
								// surfaceData.atDistance =                surfaceDescription.RefractionDistance;

								surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
								surfaceDescription.Alpha = 1.0;
							}
							else
							{
								surfaceData.ior = 1.0;
								surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
								surfaceData.atDistance = 1.0;
								surfaceData.transmittanceMask = 0.0;
								surfaceDescription.Alpha = 1.0;
							}
					#else
							surfaceData.ior = 1.0;
							surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
							surfaceData.atDistance = 1.0;
							surfaceData.transmittanceMask = 0.0;
					#endif

							// These static material feature allow compile time optimization
							surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
					#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
					#endif
					#ifdef _MATERIAL_FEATURE_TRANSMISSION
							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
					#endif
					#ifdef _MATERIAL_FEATURE_ANISOTROPY
							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
					#endif
							// surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;

					#ifdef _MATERIAL_FEATURE_IRIDESCENCE
							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
					#endif
					#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
					#endif

					#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
							// Require to have setup baseColor
							// Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
							surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
					#endif

					#ifdef _DOUBLESIDED_ON
						float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
					#else
						float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
					#endif

						// tangent-space normal
						float3 normalTS = float3(0.0f, 0.0f, 1.0f);
						// normalTS = surfaceDescription.Normal;

						// compute world space normal
						GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

						surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

						surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
						// surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);

				#if HAVE_DECALS
						if (_EnableDecals)
						{
							// Both uses and modifies 'surfaceData.normalWS'.
							DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
							ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
						}
				#endif

						bentNormalWS = surfaceData.normalWS;
						// GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);

						surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);


						// By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
						// If user provide bent normal then we process a better term
				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
						// Just use the value passed through via the slot (not active otherwise)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
						// If we have bent normal and ambient occlusion, process a specular occlusion
						surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
						surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
						surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
				#endif

				#ifdef DEBUG_DISPLAY
						if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
						{
							// TODO: need to update mip info
							surfaceData.metallic = 0;
						}

						// We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
						// as it can modify attribute use for static lighting
						ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
					}

					void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
					{
				#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
						uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
						LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif

						ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

						SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
						SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);

						// Perform alpha test very early to save performance (a killed pixel will not sample textures)
						// TODO: split graph evaluation to grab just alpha dependencies first? tricky..
						// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
						// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
						// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
						// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);

						// ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);

						float3 bentNormalWS;
						BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

						// Builtin Data
						// For back lighting we use the oposite vertex normal 
						InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

						// override sampleBakedGI:
						// builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
						// builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;

						// builtinData.emissiveColor = surfaceDescription.Emission;

						// builtinData.depthOffset = surfaceDescription.DepthOffset;

				#if (SHADERPASS == SHADERPASS_DISTORTION)
						builtinData.distortion = surfaceDescription.Distortion;
						builtinData.distortionBlur = surfaceDescription.DistortionBlur;
				#else
						builtinData.distortion = float2(0.0, 0.0);
						builtinData.distortionBlur = 0.0;
				#endif

						PostInitBuiltinData(V, posInput, surfaceData, builtinData);
					}

					//-------------------------------------------------------------------------------------
					// Pass Includes
					//-------------------------------------------------------------------------------------
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
					//-------------------------------------------------------------------------------------
					// End Pass Includes
					//-------------------------------------------------------------------------------------

					ENDHLSL
				}

				Pass
				{
						// based on HDLitPass.template
						Name "META"
						Tags { "LightMode" = "META" }

						//-------------------------------------------------------------------------------------
						// Render Modes (Blend, Cull, ZTest, Stencil, etc)
						//-------------------------------------------------------------------------------------

						Cull Off






						//-------------------------------------------------------------------------------------
						// End Render Modes
						//-------------------------------------------------------------------------------------

						HLSLPROGRAM

						#pragma target 4.5
						#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
						//#pragma enable_d3d11_debug_symbols

						#pragma multi_compile_instancing
					#pragma instancing_options renderinglayer

						#pragma multi_compile _ LOD_FADE_CROSSFADE

						#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
						#pragma shader_feature_local _DOUBLESIDED_ON
						#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

						//-------------------------------------------------------------------------------------
						// Variant Definitions (active field translations to HDRP defines)
						//-------------------------------------------------------------------------------------
						// #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
						// #define _MATERIAL_FEATURE_TRANSMISSION 1
						// #define _MATERIAL_FEATURE_ANISOTROPY 1
						// #define _MATERIAL_FEATURE_IRIDESCENCE 1
						// #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
						// #define _ENABLE_FOG_ON_TRANSPARENT 1
						// #define _AMBIENT_OCCLUSION 1
						// #define _SPECULAR_OCCLUSION_FROM_AO 1
						// #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
						// #define _SPECULAR_OCCLUSION_CUSTOM 1
						#define _ENERGY_CONSERVING_SPECULAR 1
						// #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
						// #define _HAS_REFRACTION 1
						// #define _REFRACTION_PLANE 1
						// #define _REFRACTION_SPHERE 1
						// #define _DISABLE_DECALS 1
						// #define _DISABLE_SSR 1
						// #define _ADD_PRECOMPUTED_VELOCITY
						// #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
						// #define _DEPTHOFFSET_ON 1
						// #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

						//-------------------------------------------------------------------------------------
						// End Variant Definitions
						//-------------------------------------------------------------------------------------

						#pragma vertex Vert
						#pragma fragment Frag

						// If we use subsurface scattering, enable output split lighting (for forward pass)
						#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
						#define OUTPUT_SPLIT_LIGHTING
						#endif

						#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

						#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"

						// define FragInputs structure
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

						//-------------------------------------------------------------------------------------
						// Defines
						//-------------------------------------------------------------------------------------
								#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
							// ACTIVE FIELDS:
							//   Material.Standard
							//   Specular.EnergyConserving
							//   SurfaceDescriptionInputs.TangentSpaceNormal
							//   SurfaceDescription.Albedo
							//   SurfaceDescription.Normal
							//   SurfaceDescription.BentNormal
							//   SurfaceDescription.CoatMask
							//   SurfaceDescription.Metallic
							//   SurfaceDescription.Emission
							//   SurfaceDescription.Smoothness
							//   SurfaceDescription.Occlusion
							//   SurfaceDescription.Alpha
							//   AttributesMesh.normalOS
							//   AttributesMesh.tangentOS
							//   AttributesMesh.uv0
							//   AttributesMesh.uv1
							//   AttributesMesh.color
							//   AttributesMesh.uv2

						// this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
						#define ATTRIBUTES_NEED_NORMAL
						#define ATTRIBUTES_NEED_TANGENT
						#define ATTRIBUTES_NEED_TEXCOORD0
						#define ATTRIBUTES_NEED_TEXCOORD1
						#define ATTRIBUTES_NEED_TEXCOORD2
						// #define ATTRIBUTES_NEED_TEXCOORD3
						#define ATTRIBUTES_NEED_COLOR
						// #define VARYINGS_NEED_POSITION_WS
						// #define VARYINGS_NEED_TANGENT_TO_WORLD
						// #define VARYINGS_NEED_TEXCOORD0
						// #define VARYINGS_NEED_TEXCOORD1
						// #define VARYINGS_NEED_TEXCOORD2
						// #define VARYINGS_NEED_TEXCOORD3
						// #define VARYINGS_NEED_COLOR
						// #define VARYINGS_NEED_CULLFACE
						// #define HAVE_MESH_MODIFICATION

					// We need isFontFace when using double sided
					#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
						#define VARYINGS_NEED_CULLFACE
					#endif

						//-------------------------------------------------------------------------------------
						// End Defines
						//-------------------------------------------------------------------------------------

						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
					#ifdef DEBUG_DISPLAY
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
					#endif

						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

					#if (SHADERPASS == SHADERPASS_FORWARD)
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

						#define HAS_LIGHTLOOP

						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
					#else
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
					#endif

						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

						// Used by SceneSelectionPass
						int _ObjectId;
						int _PassValue;

						//-------------------------------------------------------------------------------------
						// Interpolator Packing And Struct Declarations
						//-------------------------------------------------------------------------------------
					// Generated Type: AttributesMesh
					struct AttributesMesh {
						float3 positionOS : POSITION;
						float3 normalOS : NORMAL; // optional
						float4 tangentOS : TANGENT; // optional
						float4 uv0 : TEXCOORD0; // optional
						float4 uv1 : TEXCOORD1; // optional
						float4 uv2 : TEXCOORD2; // optional
						float4 color : COLOR; // optional
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : INSTANCEID_SEMANTIC;
						#endif // UNITY_ANY_INSTANCING_ENABLED
					};

					// Generated Type: VaryingsMeshToPS
					struct VaryingsMeshToPS {
						float4 positionCS : SV_Position;
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : CUSTOM_INSTANCE_ID;
						#endif // UNITY_ANY_INSTANCING_ENABLED
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
						#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					};
					struct PackedVaryingsMeshToPS {
						float4 positionCS : SV_Position; // unpacked
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
						#endif // UNITY_ANY_INSTANCING_ENABLED
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
						#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					};
					PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
					{
						PackedVaryingsMeshToPS output;
						output.positionCS = input.positionCS;
						#if UNITY_ANY_INSTANCING_ENABLED
						output.instanceID = input.instanceID;
						#endif // UNITY_ANY_INSTANCING_ENABLED
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						output.cullFace = input.cullFace;
						#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						return output;
					}
					VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
					{
						VaryingsMeshToPS output;
						output.positionCS = input.positionCS;
						#if UNITY_ANY_INSTANCING_ENABLED
						output.instanceID = input.instanceID;
						#endif // UNITY_ANY_INSTANCING_ENABLED
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						output.cullFace = input.cullFace;
						#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						return output;
					}

					// Generated Type: VaryingsMeshToDS
					struct VaryingsMeshToDS {
						float3 positionRWS;
						float3 normalWS;
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : CUSTOM_INSTANCE_ID;
						#endif // UNITY_ANY_INSTANCING_ENABLED
					};
					struct PackedVaryingsMeshToDS {
						float3 interp00 : TEXCOORD0; // auto-packed
						float3 interp01 : TEXCOORD1; // auto-packed
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
						#endif // UNITY_ANY_INSTANCING_ENABLED
					};
					PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
					{
						PackedVaryingsMeshToDS output;
						output.interp00.xyz = input.positionRWS;
						output.interp01.xyz = input.normalWS;
						#if UNITY_ANY_INSTANCING_ENABLED
						output.instanceID = input.instanceID;
						#endif // UNITY_ANY_INSTANCING_ENABLED
						return output;
					}
					VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
					{
						VaryingsMeshToDS output;
						output.positionRWS = input.interp00.xyz;
						output.normalWS = input.interp01.xyz;
						#if UNITY_ANY_INSTANCING_ENABLED
						output.instanceID = input.instanceID;
						#endif // UNITY_ANY_INSTANCING_ENABLED
						return output;
					}

					//-------------------------------------------------------------------------------------
					// End Interpolator Packing And Struct Declarations
					//-------------------------------------------------------------------------------------

					//-------------------------------------------------------------------------------------
					// Graph generated code
					//-------------------------------------------------------------------------------------
							// Shared Graph Properties (uniform inputs)
							CBUFFER_START(UnityPerMaterial)
							float4 _EmissionColor;
							float _RenderQueueType;
							float _StencilRef;
							float _StencilWriteMask;
							float _StencilRefDepth;
							float _StencilWriteMaskDepth;
							float _StencilRefMV;
							float _StencilWriteMaskMV;
							float _StencilRefDistortionVec;
							float _StencilWriteMaskDistortionVec;
							float _StencilWriteMaskGBuffer;
							float _StencilRefGBuffer;
							float _ZTestGBuffer;
							float _RequireSplitLighting;
							float _ReceivesSSR;
							float _SurfaceType;
							float _BlendMode;
							float _SrcBlend;
							float _DstBlend;
							float _AlphaSrcBlend;
							float _AlphaDstBlend;
							float _ZWrite;
							float _CullMode;
							float _TransparentSortPriority;
							float _CullModeForward;
							float _TransparentCullMode;
							float _ZTestDepthEqualForOpaque;
							float _ZTestTransparent;
							float _TransparentBackfaceEnable;
							float _AlphaCutoffEnable;
							float _AlphaCutoff;
							float _UseShadowThreshold;
							float _DoubleSidedEnable;
							float _DoubleSidedNormalMode;
							float4 _DoubleSidedConstants;
							CBUFFER_END


								// Pixel Graph Inputs
									struct SurfaceDescriptionInputs {
										float3 TangentSpaceNormal; // optional
									};
							// Pixel Graph Outputs
								struct SurfaceDescription
								{
									float3 Albedo;
									float3 Normal;
									float3 BentNormal;
									float CoatMask;
									float Metallic;
									float3 Emission;
									float Smoothness;
									float Occlusion;
									float Alpha;
								};

								// Shared Graph Node Functions
								// Pixel Graph Evaluation
									SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
									{
										SurfaceDescription surface = (SurfaceDescription)0;
										surface.Albedo = IsGammaSpace() ? float3(0.7353569, 0.7353569, 0.7353569) : SRGBToLinear(float3(0.7353569, 0.7353569, 0.7353569));
										surface.Normal = IN.TangentSpaceNormal;
										surface.BentNormal = IN.TangentSpaceNormal;
										surface.CoatMask = 0;
										surface.Metallic = 0;
										surface.Emission = float3(0, 0, 0);
										surface.Smoothness = 0.5;
										surface.Occlusion = 1;
										surface.Alpha = 1;
										return surface;
									}

									//-------------------------------------------------------------------------------------
									// End graph generated code
									//-------------------------------------------------------------------------------------

								// $include("VertexAnimation.template.hlsl")


								//-------------------------------------------------------------------------------------
								// TEMPLATE INCLUDE : SharedCode.template.hlsl
								//-------------------------------------------------------------------------------------
									FragInputs BuildFragInputs(VaryingsMeshToPS input)
									{
										FragInputs output;
										ZERO_INITIALIZE(FragInputs, output);

										// Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
										// TODO: this is a really poor workaround, but the variable is used in a bunch of places
										// to compute normals which are then passed on elsewhere to compute other values...
										output.tangentToWorld = k_identity3x3;
										output.positionSS = input.positionCS;       // input.positionCS is SV_Position

										// output.positionRWS = input.positionRWS;
										// output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
										// output.texCoord0 = input.texCoord0;
										// output.texCoord1 = input.texCoord1;
										// output.texCoord2 = input.texCoord2;
										// output.texCoord3 = input.texCoord3;
										// output.color = input.color;
										#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
										output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
										#elif SHADER_STAGE_FRAGMENT
										// output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
										#endif // SHADER_STAGE_FRAGMENT

										return output;
									}

									SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
									{
										SurfaceDescriptionInputs output;
										ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

										// output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
										// output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
										// output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
										output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
										// output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
										// output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
										// output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
										// output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
										// output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
										// output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
										// output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
										// output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
										// output.WorldSpaceViewDirection =     normalize(viewWS);
										// output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
										// output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
										// float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
										// output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
										// output.WorldSpacePosition =          GetAbsolutePositionWS(input.positionRWS);
										// output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
										// output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
										// output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
										// output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
										// output.uv0 =                         input.texCoord0;
										// output.uv1 =                         input.texCoord1;
										// output.uv2 =                         input.texCoord2;
										// output.uv3 =                         input.texCoord3;
										// output.VertexColor =                 input.color;
										// output.FaceSign =                    input.isFrontFace;
										// output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value

										return output;
									}

									// existing HDRP code uses the combined function to go directly from packed to frag inputs
									FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
									{
										UNITY_SETUP_INSTANCE_ID(input);
										VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
										return BuildFragInputs(unpacked);
									}

									//-------------------------------------------------------------------------------------
									// END TEMPLATE INCLUDE : SharedCode.template.hlsl
									//-------------------------------------------------------------------------------------


										void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
										{
											// setup defaults -- these are used if the graph doesn't output a value
											ZERO_INITIALIZE(SurfaceData, surfaceData);

											// specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
											// however specularOcclusion can come from the graph, so need to be init here so it can be override.
											surfaceData.specularOcclusion = 1.0;

											// copy across graph values, if defined
											surfaceData.baseColor = surfaceDescription.Albedo;
											surfaceData.perceptualSmoothness = surfaceDescription.Smoothness;
											surfaceData.ambientOcclusion = surfaceDescription.Occlusion;
											// surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
											surfaceData.metallic = surfaceDescription.Metallic;
											// surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
											// surfaceData.thickness =                 surfaceDescription.Thickness;
											// surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
											// surfaceData.specularColor =             surfaceDescription.Specular;
											surfaceData.coatMask = surfaceDescription.CoatMask;
											// surfaceData.anisotropy =                surfaceDescription.Anisotropy;
											// surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
											// surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;

									#ifdef _HAS_REFRACTION
											if (_EnableSSRefraction)
											{
												// surfaceData.ior =                       surfaceDescription.RefractionIndex;
												// surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
												// surfaceData.atDistance =                surfaceDescription.RefractionDistance;

												surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
												surfaceDescription.Alpha = 1.0;
											}
											else
											{
												surfaceData.ior = 1.0;
												surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
												surfaceData.atDistance = 1.0;
												surfaceData.transmittanceMask = 0.0;
												surfaceDescription.Alpha = 1.0;
											}
									#else
											surfaceData.ior = 1.0;
											surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
											surfaceData.atDistance = 1.0;
											surfaceData.transmittanceMask = 0.0;
									#endif

											// These static material feature allow compile time optimization
											surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
									#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
									#endif
									#ifdef _MATERIAL_FEATURE_TRANSMISSION
											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
									#endif
									#ifdef _MATERIAL_FEATURE_ANISOTROPY
											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
									#endif
											// surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;

									#ifdef _MATERIAL_FEATURE_IRIDESCENCE
											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
									#endif
									#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
									#endif

									#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
											// Require to have setup baseColor
											// Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
											surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
									#endif

									#ifdef _DOUBLESIDED_ON
										float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
									#else
										float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
									#endif

										// tangent-space normal
										float3 normalTS = float3(0.0f, 0.0f, 1.0f);
										normalTS = surfaceDescription.Normal;

										// compute world space normal
										GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

										surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

										surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
										// surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);

								#if HAVE_DECALS
										if (_EnableDecals)
										{
											// Both uses and modifies 'surfaceData.normalWS'.
											DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
											ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
										}
								#endif

										bentNormalWS = surfaceData.normalWS;
										// GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);

										surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);


										// By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
										// If user provide bent normal then we process a better term
								#if defined(_SPECULAR_OCCLUSION_CUSTOM)
										// Just use the value passed through via the slot (not active otherwise)
								#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
										// If we have bent normal and ambient occlusion, process a specular occlusion
										surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
								#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
										surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
								#endif

								#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
										surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
								#endif

								#ifdef DEBUG_DISPLAY
										if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
										{
											// TODO: need to update mip info
											surfaceData.metallic = 0;
										}

										// We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
										// as it can modify attribute use for static lighting
										ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
								#endif
									}

									void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
									{
								#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
										uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
										LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
								#endif

								#ifdef _DOUBLESIDED_ON
									float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
								#else
									float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
								#endif

										ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

										SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
										SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);

										// Perform alpha test very early to save performance (a killed pixel will not sample textures)
										// TODO: split graph evaluation to grab just alpha dependencies first? tricky..
										// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
										// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
										// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
										// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);

										// ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);

										float3 bentNormalWS;
										BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

										// Builtin Data
										// For back lighting we use the oposite vertex normal 
										InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

										// override sampleBakedGI:
										// builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
										// builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;

										builtinData.emissiveColor = surfaceDescription.Emission;

										// builtinData.depthOffset = surfaceDescription.DepthOffset;

								#if (SHADERPASS == SHADERPASS_DISTORTION)
										builtinData.distortion = surfaceDescription.Distortion;
										builtinData.distortionBlur = surfaceDescription.DistortionBlur;
								#else
										builtinData.distortion = float2(0.0, 0.0);
										builtinData.distortionBlur = 0.0;
								#endif

										PostInitBuiltinData(V, posInput, surfaceData, builtinData);
									}

									//-------------------------------------------------------------------------------------
									// Pass Includes
									//-------------------------------------------------------------------------------------
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassLightTransport.hlsl"
									//-------------------------------------------------------------------------------------
									// End Pass Includes
									//-------------------------------------------------------------------------------------

									ENDHLSL
								}

								Pass
								{
										// based on HDLitPass.template
										Name "SceneSelectionPass"
										Tags { "LightMode" = "SceneSelectionPass" }

										//-------------------------------------------------------------------------------------
										// Render Modes (Blend, Cull, ZTest, Stencil, etc)
										//-------------------------------------------------------------------------------------






										ColorMask 0

										//-------------------------------------------------------------------------------------
										// End Render Modes
										//-------------------------------------------------------------------------------------

										HLSLPROGRAM

										#pragma target 4.5
										#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
										//#pragma enable_d3d11_debug_symbols

										#pragma multi_compile_instancing
									#pragma instancing_options renderinglayer

										#pragma multi_compile _ LOD_FADE_CROSSFADE

										#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
										#pragma shader_feature_local _DOUBLESIDED_ON
										#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

										//-------------------------------------------------------------------------------------
										// Variant Definitions (active field translations to HDRP defines)
										//-------------------------------------------------------------------------------------
										// #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
										// #define _MATERIAL_FEATURE_TRANSMISSION 1
										// #define _MATERIAL_FEATURE_ANISOTROPY 1
										// #define _MATERIAL_FEATURE_IRIDESCENCE 1
										// #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
										// #define _ENABLE_FOG_ON_TRANSPARENT 1
										// #define _AMBIENT_OCCLUSION 1
										// #define _SPECULAR_OCCLUSION_FROM_AO 1
										// #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
										// #define _SPECULAR_OCCLUSION_CUSTOM 1
										#define _ENERGY_CONSERVING_SPECULAR 1
										// #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
										// #define _HAS_REFRACTION 1
										// #define _REFRACTION_PLANE 1
										// #define _REFRACTION_SPHERE 1
										// #define _DISABLE_DECALS 1
										// #define _DISABLE_SSR 1
										// #define _ADD_PRECOMPUTED_VELOCITY
										// #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
										// #define _DEPTHOFFSET_ON 1
										// #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

										//-------------------------------------------------------------------------------------
										// End Variant Definitions
										//-------------------------------------------------------------------------------------

										#pragma vertex Vert
										#pragma fragment Frag

										// If we use subsurface scattering, enable output split lighting (for forward pass)
										#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
										#define OUTPUT_SPLIT_LIGHTING
										#endif

										#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

										#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"

										// define FragInputs structure
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

										//-------------------------------------------------------------------------------------
										// Defines
										//-------------------------------------------------------------------------------------
												#define SHADERPASS SHADERPASS_DEPTH_ONLY
											#define SCENESELECTIONPASS
											#pragma editor_sync_compilation
											// ACTIVE FIELDS:
											//   Material.Standard
											//   Specular.EnergyConserving
											//   SurfaceDescriptionInputs.TangentSpaceNormal
											//   SurfaceDescription.Alpha

										// this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
										// #define ATTRIBUTES_NEED_NORMAL
										// #define ATTRIBUTES_NEED_TANGENT
										// #define ATTRIBUTES_NEED_TEXCOORD0
										// #define ATTRIBUTES_NEED_TEXCOORD1
										// #define ATTRIBUTES_NEED_TEXCOORD2
										// #define ATTRIBUTES_NEED_TEXCOORD3
										// #define ATTRIBUTES_NEED_COLOR
										// #define VARYINGS_NEED_POSITION_WS
										// #define VARYINGS_NEED_TANGENT_TO_WORLD
										// #define VARYINGS_NEED_TEXCOORD0
										// #define VARYINGS_NEED_TEXCOORD1
										// #define VARYINGS_NEED_TEXCOORD2
										// #define VARYINGS_NEED_TEXCOORD3
										// #define VARYINGS_NEED_COLOR
										// #define VARYINGS_NEED_CULLFACE
										// #define HAVE_MESH_MODIFICATION

									// We need isFontFace when using double sided
									#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
										#define VARYINGS_NEED_CULLFACE
									#endif

										//-------------------------------------------------------------------------------------
										// End Defines
										//-------------------------------------------------------------------------------------

										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
									#ifdef DEBUG_DISPLAY
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
									#endif

										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

									#if (SHADERPASS == SHADERPASS_FORWARD)
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

										#define HAS_LIGHTLOOP

										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
									#else
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
									#endif

										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

										// Used by SceneSelectionPass
										int _ObjectId;
										int _PassValue;

										//-------------------------------------------------------------------------------------
										// Interpolator Packing And Struct Declarations
										//-------------------------------------------------------------------------------------
									// Generated Type: AttributesMesh
									struct AttributesMesh {
										float3 positionOS : POSITION;
										#if UNITY_ANY_INSTANCING_ENABLED
										uint instanceID : INSTANCEID_SEMANTIC;
										#endif // UNITY_ANY_INSTANCING_ENABLED
									};

									// Generated Type: VaryingsMeshToPS
									struct VaryingsMeshToPS {
										float4 positionCS : SV_Position;
										#if UNITY_ANY_INSTANCING_ENABLED
										uint instanceID : CUSTOM_INSTANCE_ID;
										#endif // UNITY_ANY_INSTANCING_ENABLED
										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
										#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
									};
									struct PackedVaryingsMeshToPS {
										float4 positionCS : SV_Position; // unpacked
										#if UNITY_ANY_INSTANCING_ENABLED
										uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
										#endif // UNITY_ANY_INSTANCING_ENABLED
										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
										#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
									};
									PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
									{
										PackedVaryingsMeshToPS output;
										output.positionCS = input.positionCS;
										#if UNITY_ANY_INSTANCING_ENABLED
										output.instanceID = input.instanceID;
										#endif // UNITY_ANY_INSTANCING_ENABLED
										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										output.cullFace = input.cullFace;
										#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										return output;
									}
									VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
									{
										VaryingsMeshToPS output;
										output.positionCS = input.positionCS;
										#if UNITY_ANY_INSTANCING_ENABLED
										output.instanceID = input.instanceID;
										#endif // UNITY_ANY_INSTANCING_ENABLED
										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										output.cullFace = input.cullFace;
										#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										return output;
									}

									// Generated Type: VaryingsMeshToDS
									struct VaryingsMeshToDS {
										float3 positionRWS;
										float3 normalWS;
										#if UNITY_ANY_INSTANCING_ENABLED
										uint instanceID : CUSTOM_INSTANCE_ID;
										#endif // UNITY_ANY_INSTANCING_ENABLED
									};
									struct PackedVaryingsMeshToDS {
										float3 interp00 : TEXCOORD0; // auto-packed
										float3 interp01 : TEXCOORD1; // auto-packed
										#if UNITY_ANY_INSTANCING_ENABLED
										uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
										#endif // UNITY_ANY_INSTANCING_ENABLED
									};
									PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
									{
										PackedVaryingsMeshToDS output;
										output.interp00.xyz = input.positionRWS;
										output.interp01.xyz = input.normalWS;
										#if UNITY_ANY_INSTANCING_ENABLED
										output.instanceID = input.instanceID;
										#endif // UNITY_ANY_INSTANCING_ENABLED
										return output;
									}
									VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
									{
										VaryingsMeshToDS output;
										output.positionRWS = input.interp00.xyz;
										output.normalWS = input.interp01.xyz;
										#if UNITY_ANY_INSTANCING_ENABLED
										output.instanceID = input.instanceID;
										#endif // UNITY_ANY_INSTANCING_ENABLED
										return output;
									}

									//-------------------------------------------------------------------------------------
									// End Interpolator Packing And Struct Declarations
									//-------------------------------------------------------------------------------------

									//-------------------------------------------------------------------------------------
									// Graph generated code
									//-------------------------------------------------------------------------------------
											// Shared Graph Properties (uniform inputs)
											CBUFFER_START(UnityPerMaterial)
											float4 _EmissionColor;
											float _RenderQueueType;
											float _StencilRef;
											float _StencilWriteMask;
											float _StencilRefDepth;
											float _StencilWriteMaskDepth;
											float _StencilRefMV;
											float _StencilWriteMaskMV;
											float _StencilRefDistortionVec;
											float _StencilWriteMaskDistortionVec;
											float _StencilWriteMaskGBuffer;
											float _StencilRefGBuffer;
											float _ZTestGBuffer;
											float _RequireSplitLighting;
											float _ReceivesSSR;
											float _SurfaceType;
											float _BlendMode;
											float _SrcBlend;
											float _DstBlend;
											float _AlphaSrcBlend;
											float _AlphaDstBlend;
											float _ZWrite;
											float _CullMode;
											float _TransparentSortPriority;
											float _CullModeForward;
											float _TransparentCullMode;
											float _ZTestDepthEqualForOpaque;
											float _ZTestTransparent;
											float _TransparentBackfaceEnable;
											float _AlphaCutoffEnable;
											float _AlphaCutoff;
											float _UseShadowThreshold;
											float _DoubleSidedEnable;
											float _DoubleSidedNormalMode;
											float4 _DoubleSidedConstants;
											CBUFFER_END


												// Pixel Graph Inputs
													struct SurfaceDescriptionInputs {
														float3 TangentSpaceNormal; // optional
													};
											// Pixel Graph Outputs
												struct SurfaceDescription
												{
													float Alpha;
												};

												// Shared Graph Node Functions
												// Pixel Graph Evaluation
													SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
													{
														SurfaceDescription surface = (SurfaceDescription)0;
														surface.Alpha = 1;
														return surface;
													}

													//-------------------------------------------------------------------------------------
													// End graph generated code
													//-------------------------------------------------------------------------------------

												// $include("VertexAnimation.template.hlsl")


												//-------------------------------------------------------------------------------------
												// TEMPLATE INCLUDE : SharedCode.template.hlsl
												//-------------------------------------------------------------------------------------
													FragInputs BuildFragInputs(VaryingsMeshToPS input)
													{
														FragInputs output;
														ZERO_INITIALIZE(FragInputs, output);

														// Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
														// TODO: this is a really poor workaround, but the variable is used in a bunch of places
														// to compute normals which are then passed on elsewhere to compute other values...
														output.tangentToWorld = k_identity3x3;
														output.positionSS = input.positionCS;       // input.positionCS is SV_Position

														// output.positionRWS = input.positionRWS;
														// output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
														// output.texCoord0 = input.texCoord0;
														// output.texCoord1 = input.texCoord1;
														// output.texCoord2 = input.texCoord2;
														// output.texCoord3 = input.texCoord3;
														// output.color = input.color;
														#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
														output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
														#elif SHADER_STAGE_FRAGMENT
														// output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
														#endif // SHADER_STAGE_FRAGMENT

														return output;
													}

													SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
													{
														SurfaceDescriptionInputs output;
														ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

														// output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
														// output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
														// output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
														output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
														// output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
														// output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
														// output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
														// output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
														// output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
														// output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
														// output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
														// output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
														// output.WorldSpaceViewDirection =     normalize(viewWS);
														// output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
														// output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
														// float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
														// output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
														// output.WorldSpacePosition =          GetAbsolutePositionWS(input.positionRWS);
														// output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
														// output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
														// output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
														// output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
														// output.uv0 =                         input.texCoord0;
														// output.uv1 =                         input.texCoord1;
														// output.uv2 =                         input.texCoord2;
														// output.uv3 =                         input.texCoord3;
														// output.VertexColor =                 input.color;
														// output.FaceSign =                    input.isFrontFace;
														// output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value

														return output;
													}

													// existing HDRP code uses the combined function to go directly from packed to frag inputs
													FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
													{
														UNITY_SETUP_INSTANCE_ID(input);
														VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
														return BuildFragInputs(unpacked);
													}

													//-------------------------------------------------------------------------------------
													// END TEMPLATE INCLUDE : SharedCode.template.hlsl
													//-------------------------------------------------------------------------------------


														void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
														{
															// setup defaults -- these are used if the graph doesn't output a value
															ZERO_INITIALIZE(SurfaceData, surfaceData);

															// specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
															// however specularOcclusion can come from the graph, so need to be init here so it can be override.
															surfaceData.specularOcclusion = 1.0;

															// copy across graph values, if defined
															// surfaceData.baseColor =                 surfaceDescription.Albedo;
															// surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
															// surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
															// surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
															// surfaceData.metallic =                  surfaceDescription.Metallic;
															// surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
															// surfaceData.thickness =                 surfaceDescription.Thickness;
															// surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
															// surfaceData.specularColor =             surfaceDescription.Specular;
															// surfaceData.coatMask =                  surfaceDescription.CoatMask;
															// surfaceData.anisotropy =                surfaceDescription.Anisotropy;
															// surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
															// surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;

													#ifdef _HAS_REFRACTION
															if (_EnableSSRefraction)
															{
																// surfaceData.ior =                       surfaceDescription.RefractionIndex;
																// surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
																// surfaceData.atDistance =                surfaceDescription.RefractionDistance;

																surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
																surfaceDescription.Alpha = 1.0;
															}
															else
															{
																surfaceData.ior = 1.0;
																surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																surfaceData.atDistance = 1.0;
																surfaceData.transmittanceMask = 0.0;
																surfaceDescription.Alpha = 1.0;
															}
													#else
															surfaceData.ior = 1.0;
															surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
															surfaceData.atDistance = 1.0;
															surfaceData.transmittanceMask = 0.0;
													#endif

															// These static material feature allow compile time optimization
															surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
													#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
													#endif
													#ifdef _MATERIAL_FEATURE_TRANSMISSION
															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
													#endif
													#ifdef _MATERIAL_FEATURE_ANISOTROPY
															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
													#endif
															// surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;

													#ifdef _MATERIAL_FEATURE_IRIDESCENCE
															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
													#endif
													#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
													#endif

													#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
															// Require to have setup baseColor
															// Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
															surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
													#endif

													#ifdef _DOUBLESIDED_ON
														float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
													#else
														float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
													#endif

														// tangent-space normal
														float3 normalTS = float3(0.0f, 0.0f, 1.0f);
														// normalTS = surfaceDescription.Normal;

														// compute world space normal
														GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

														surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

														surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
														// surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);

												#if HAVE_DECALS
														if (_EnableDecals)
														{
															// Both uses and modifies 'surfaceData.normalWS'.
															DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
															ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
														}
												#endif

														bentNormalWS = surfaceData.normalWS;
														// GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);

														surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);


														// By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
														// If user provide bent normal then we process a better term
												#if defined(_SPECULAR_OCCLUSION_CUSTOM)
														// Just use the value passed through via the slot (not active otherwise)
												#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
														// If we have bent normal and ambient occlusion, process a specular occlusion
														surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
												#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
														surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
												#endif

												#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
														surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
												#endif

												#ifdef DEBUG_DISPLAY
														if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
														{
															// TODO: need to update mip info
															surfaceData.metallic = 0;
														}

														// We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
														// as it can modify attribute use for static lighting
														ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
												#endif
													}

													void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
													{
												#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
														uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
														LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
												#endif

												#ifdef _DOUBLESIDED_ON
													float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
												#else
													float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
												#endif

														ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

														SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
														SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);

														// Perform alpha test very early to save performance (a killed pixel will not sample textures)
														// TODO: split graph evaluation to grab just alpha dependencies first? tricky..
														// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
														// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
														// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
														// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);

														// ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);

														float3 bentNormalWS;
														BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

														// Builtin Data
														// For back lighting we use the oposite vertex normal 
														InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

														// override sampleBakedGI:
														// builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
														// builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;

														// builtinData.emissiveColor = surfaceDescription.Emission;

														// builtinData.depthOffset = surfaceDescription.DepthOffset;

												#if (SHADERPASS == SHADERPASS_DISTORTION)
														builtinData.distortion = surfaceDescription.Distortion;
														builtinData.distortionBlur = surfaceDescription.DistortionBlur;
												#else
														builtinData.distortion = float2(0.0, 0.0);
														builtinData.distortionBlur = 0.0;
												#endif

														PostInitBuiltinData(V, posInput, surfaceData, builtinData);
													}

													//-------------------------------------------------------------------------------------
													// Pass Includes
													//-------------------------------------------------------------------------------------
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
													//-------------------------------------------------------------------------------------
													// End Pass Includes
													//-------------------------------------------------------------------------------------

													ENDHLSL
												}

												Pass
												{
														// based on HDLitPass.template
														Name "DepthOnly"
														Tags { "LightMode" = "DepthOnly" }

														//-------------------------------------------------------------------------------------
														// Render Modes (Blend, Cull, ZTest, Stencil, etc)
														//-------------------------------------------------------------------------------------

														Cull[_CullMode]


														ZWrite On


														// Stencil setup
													Stencil
													{
													   WriteMask[_StencilWriteMaskDepth]
													   Ref[_StencilRefDepth]
													   Comp Always
													   Pass Replace
													}


														//-------------------------------------------------------------------------------------
														// End Render Modes
														//-------------------------------------------------------------------------------------

														HLSLPROGRAM

														#pragma target 4.5
														#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
														//#pragma enable_d3d11_debug_symbols

														#pragma multi_compile_instancing
													#pragma instancing_options renderinglayer

														#pragma multi_compile _ LOD_FADE_CROSSFADE

														#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
														#pragma shader_feature_local _DOUBLESIDED_ON
														#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

														//-------------------------------------------------------------------------------------
														// Variant Definitions (active field translations to HDRP defines)
														//-------------------------------------------------------------------------------------
														// #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
														// #define _MATERIAL_FEATURE_TRANSMISSION 1
														// #define _MATERIAL_FEATURE_ANISOTROPY 1
														// #define _MATERIAL_FEATURE_IRIDESCENCE 1
														// #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
														// #define _ENABLE_FOG_ON_TRANSPARENT 1
														// #define _AMBIENT_OCCLUSION 1
														// #define _SPECULAR_OCCLUSION_FROM_AO 1
														// #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
														// #define _SPECULAR_OCCLUSION_CUSTOM 1
														#define _ENERGY_CONSERVING_SPECULAR 1
														// #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
														// #define _HAS_REFRACTION 1
														// #define _REFRACTION_PLANE 1
														// #define _REFRACTION_SPHERE 1
														// #define _DISABLE_DECALS 1
														// #define _DISABLE_SSR 1
														// #define _ADD_PRECOMPUTED_VELOCITY
														// #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
														// #define _DEPTHOFFSET_ON 1
														// #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

														//-------------------------------------------------------------------------------------
														// End Variant Definitions
														//-------------------------------------------------------------------------------------

														#pragma vertex Vert
														#pragma fragment Frag

														// If we use subsurface scattering, enable output split lighting (for forward pass)
														#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
														#define OUTPUT_SPLIT_LIGHTING
														#endif

														#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

														#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"

														// define FragInputs structure
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

														//-------------------------------------------------------------------------------------
														// Defines
														//-------------------------------------------------------------------------------------
																#define SHADERPASS SHADERPASS_DEPTH_ONLY
															#pragma multi_compile _ WRITE_NORMAL_BUFFER
															#pragma multi_compile _ WRITE_MSAA_DEPTH
															// ACTIVE FIELDS:
															//   Material.Standard
															//   Specular.EnergyConserving
															//   SurfaceDescriptionInputs.TangentSpaceNormal
															//   SurfaceDescription.Normal
															//   SurfaceDescription.Smoothness
															//   SurfaceDescription.Alpha
															//   AttributesMesh.normalOS
															//   AttributesMesh.tangentOS
															//   AttributesMesh.uv0
															//   AttributesMesh.uv1
															//   AttributesMesh.color
															//   AttributesMesh.uv2
															//   AttributesMesh.uv3
															//   FragInputs.tangentToWorld
															//   FragInputs.positionRWS
															//   FragInputs.texCoord0
															//   FragInputs.texCoord1
															//   FragInputs.texCoord2
															//   FragInputs.texCoord3
															//   FragInputs.color
															//   VaryingsMeshToPS.tangentWS
															//   VaryingsMeshToPS.normalWS
															//   VaryingsMeshToPS.positionRWS
															//   VaryingsMeshToPS.texCoord0
															//   VaryingsMeshToPS.texCoord1
															//   VaryingsMeshToPS.texCoord2
															//   VaryingsMeshToPS.texCoord3
															//   VaryingsMeshToPS.color
															//   AttributesMesh.positionOS

														// this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
														#define ATTRIBUTES_NEED_NORMAL
														#define ATTRIBUTES_NEED_TANGENT
														#define ATTRIBUTES_NEED_TEXCOORD0
														#define ATTRIBUTES_NEED_TEXCOORD1
														#define ATTRIBUTES_NEED_TEXCOORD2
														#define ATTRIBUTES_NEED_TEXCOORD3
														#define ATTRIBUTES_NEED_COLOR
														#define VARYINGS_NEED_POSITION_WS
														#define VARYINGS_NEED_TANGENT_TO_WORLD
														#define VARYINGS_NEED_TEXCOORD0
														#define VARYINGS_NEED_TEXCOORD1
														#define VARYINGS_NEED_TEXCOORD2
														#define VARYINGS_NEED_TEXCOORD3
														#define VARYINGS_NEED_COLOR
														// #define VARYINGS_NEED_CULLFACE
														// #define HAVE_MESH_MODIFICATION

													// We need isFontFace when using double sided
													#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
														#define VARYINGS_NEED_CULLFACE
													#endif

														//-------------------------------------------------------------------------------------
														// End Defines
														//-------------------------------------------------------------------------------------

														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
													#ifdef DEBUG_DISPLAY
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
													#endif

														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

													#if (SHADERPASS == SHADERPASS_FORWARD)
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

														#define HAS_LIGHTLOOP

														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
													#else
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
													#endif

														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

														// Used by SceneSelectionPass
														int _ObjectId;
														int _PassValue;

														//-------------------------------------------------------------------------------------
														// Interpolator Packing And Struct Declarations
														//-------------------------------------------------------------------------------------
													// Generated Type: AttributesMesh
													struct AttributesMesh {
														float3 positionOS : POSITION;
														float3 normalOS : NORMAL; // optional
														float4 tangentOS : TANGENT; // optional
														float4 uv0 : TEXCOORD0; // optional
														float4 uv1 : TEXCOORD1; // optional
														float4 uv2 : TEXCOORD2; // optional
														float4 uv3 : TEXCOORD3; // optional
														float4 color : COLOR; // optional
														#if UNITY_ANY_INSTANCING_ENABLED
														uint instanceID : INSTANCEID_SEMANTIC;
														#endif // UNITY_ANY_INSTANCING_ENABLED
													};

													// Generated Type: VaryingsMeshToPS
													struct VaryingsMeshToPS {
														float4 positionCS : SV_Position;
														float3 positionRWS; // optional
														float3 normalWS; // optional
														float4 tangentWS; // optional
														float4 texCoord0; // optional
														float4 texCoord1; // optional
														float4 texCoord2; // optional
														float4 texCoord3; // optional
														float4 color; // optional
														#if UNITY_ANY_INSTANCING_ENABLED
														uint instanceID : CUSTOM_INSTANCE_ID;
														#endif // UNITY_ANY_INSTANCING_ENABLED
														#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
														FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
														#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
													};
													struct PackedVaryingsMeshToPS {
														float3 interp00 : TEXCOORD0; // auto-packed
														float3 interp01 : TEXCOORD1; // auto-packed
														float4 interp02 : TEXCOORD2; // auto-packed
														float4 interp03 : TEXCOORD3; // auto-packed
														float4 interp04 : TEXCOORD4; // auto-packed
														float4 interp05 : TEXCOORD5; // auto-packed
														float4 interp06 : TEXCOORD6; // auto-packed
														float4 interp07 : TEXCOORD7; // auto-packed
														float4 positionCS : SV_Position; // unpacked
														#if UNITY_ANY_INSTANCING_ENABLED
														uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
														#endif // UNITY_ANY_INSTANCING_ENABLED
														#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
														FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
														#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
													};
													PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
													{
														PackedVaryingsMeshToPS output;
														output.positionCS = input.positionCS;
														output.interp00.xyz = input.positionRWS;
														output.interp01.xyz = input.normalWS;
														output.interp02.xyzw = input.tangentWS;
														output.interp03.xyzw = input.texCoord0;
														output.interp04.xyzw = input.texCoord1;
														output.interp05.xyzw = input.texCoord2;
														output.interp06.xyzw = input.texCoord3;
														output.interp07.xyzw = input.color;
														#if UNITY_ANY_INSTANCING_ENABLED
														output.instanceID = input.instanceID;
														#endif // UNITY_ANY_INSTANCING_ENABLED
														#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
														output.cullFace = input.cullFace;
														#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
														return output;
													}
													VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
													{
														VaryingsMeshToPS output;
														output.positionCS = input.positionCS;
														output.positionRWS = input.interp00.xyz;
														output.normalWS = input.interp01.xyz;
														output.tangentWS = input.interp02.xyzw;
														output.texCoord0 = input.interp03.xyzw;
														output.texCoord1 = input.interp04.xyzw;
														output.texCoord2 = input.interp05.xyzw;
														output.texCoord3 = input.interp06.xyzw;
														output.color = input.interp07.xyzw;
														#if UNITY_ANY_INSTANCING_ENABLED
														output.instanceID = input.instanceID;
														#endif // UNITY_ANY_INSTANCING_ENABLED
														#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
														output.cullFace = input.cullFace;
														#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
														return output;
													}

													// Generated Type: VaryingsMeshToDS
													struct VaryingsMeshToDS {
														float3 positionRWS;
														float3 normalWS;
														#if UNITY_ANY_INSTANCING_ENABLED
														uint instanceID : CUSTOM_INSTANCE_ID;
														#endif // UNITY_ANY_INSTANCING_ENABLED
													};
													struct PackedVaryingsMeshToDS {
														float3 interp00 : TEXCOORD0; // auto-packed
														float3 interp01 : TEXCOORD1; // auto-packed
														#if UNITY_ANY_INSTANCING_ENABLED
														uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
														#endif // UNITY_ANY_INSTANCING_ENABLED
													};
													PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
													{
														PackedVaryingsMeshToDS output;
														output.interp00.xyz = input.positionRWS;
														output.interp01.xyz = input.normalWS;
														#if UNITY_ANY_INSTANCING_ENABLED
														output.instanceID = input.instanceID;
														#endif // UNITY_ANY_INSTANCING_ENABLED
														return output;
													}
													VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
													{
														VaryingsMeshToDS output;
														output.positionRWS = input.interp00.xyz;
														output.normalWS = input.interp01.xyz;
														#if UNITY_ANY_INSTANCING_ENABLED
														output.instanceID = input.instanceID;
														#endif // UNITY_ANY_INSTANCING_ENABLED
														return output;
													}

													//-------------------------------------------------------------------------------------
													// End Interpolator Packing And Struct Declarations
													//-------------------------------------------------------------------------------------

													//-------------------------------------------------------------------------------------
													// Graph generated code
													//-------------------------------------------------------------------------------------
															// Shared Graph Properties (uniform inputs)
															CBUFFER_START(UnityPerMaterial)
															float4 _EmissionColor;
															float _RenderQueueType;
															float _StencilRef;
															float _StencilWriteMask;
															float _StencilRefDepth;
															float _StencilWriteMaskDepth;
															float _StencilRefMV;
															float _StencilWriteMaskMV;
															float _StencilRefDistortionVec;
															float _StencilWriteMaskDistortionVec;
															float _StencilWriteMaskGBuffer;
															float _StencilRefGBuffer;
															float _ZTestGBuffer;
															float _RequireSplitLighting;
															float _ReceivesSSR;
															float _SurfaceType;
															float _BlendMode;
															float _SrcBlend;
															float _DstBlend;
															float _AlphaSrcBlend;
															float _AlphaDstBlend;
															float _ZWrite;
															float _CullMode;
															float _TransparentSortPriority;
															float _CullModeForward;
															float _TransparentCullMode;
															float _ZTestDepthEqualForOpaque;
															float _ZTestTransparent;
															float _TransparentBackfaceEnable;
															float _AlphaCutoffEnable;
															float _AlphaCutoff;
															float _UseShadowThreshold;
															float _DoubleSidedEnable;
															float _DoubleSidedNormalMode;
															float4 _DoubleSidedConstants;
															CBUFFER_END


																// Pixel Graph Inputs
																	struct SurfaceDescriptionInputs {
																		float3 TangentSpaceNormal; // optional
																	};
															// Pixel Graph Outputs
																struct SurfaceDescription
																{
																	float3 Normal;
																	float Smoothness;
																	float Alpha;
																};

																// Shared Graph Node Functions
																// Pixel Graph Evaluation
																	SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
																	{
																		SurfaceDescription surface = (SurfaceDescription)0;
																		surface.Normal = IN.TangentSpaceNormal;
																		surface.Smoothness = 0.5;
																		surface.Alpha = 1;
																		return surface;
																	}

																	//-------------------------------------------------------------------------------------
																	// End graph generated code
																	//-------------------------------------------------------------------------------------

																// $include("VertexAnimation.template.hlsl")


																//-------------------------------------------------------------------------------------
																// TEMPLATE INCLUDE : SharedCode.template.hlsl
																//-------------------------------------------------------------------------------------
																	FragInputs BuildFragInputs(VaryingsMeshToPS input)
																	{
																		FragInputs output;
																		ZERO_INITIALIZE(FragInputs, output);

																		// Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
																		// TODO: this is a really poor workaround, but the variable is used in a bunch of places
																		// to compute normals which are then passed on elsewhere to compute other values...
																		output.tangentToWorld = k_identity3x3;
																		output.positionSS = input.positionCS;       // input.positionCS is SV_Position

																		output.positionRWS = input.positionRWS;
																		output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
																		output.texCoord0 = input.texCoord0;
																		output.texCoord1 = input.texCoord1;
																		output.texCoord2 = input.texCoord2;
																		output.texCoord3 = input.texCoord3;
																		output.color = input.color;
																		#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
																		output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
																		#elif SHADER_STAGE_FRAGMENT
																		// output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
																		#endif // SHADER_STAGE_FRAGMENT

																		return output;
																	}

																	SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
																	{
																		SurfaceDescriptionInputs output;
																		ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

																		// output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
																		// output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
																		// output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
																		output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
																		// output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
																		// output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
																		// output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
																		// output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
																		// output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
																		// output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
																		// output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
																		// output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
																		// output.WorldSpaceViewDirection =     normalize(viewWS);
																		// output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
																		// output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
																		// float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
																		// output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
																		// output.WorldSpacePosition =          GetAbsolutePositionWS(input.positionRWS);
																		// output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
																		// output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
																		// output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
																		// output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
																		// output.uv0 =                         input.texCoord0;
																		// output.uv1 =                         input.texCoord1;
																		// output.uv2 =                         input.texCoord2;
																		// output.uv3 =                         input.texCoord3;
																		// output.VertexColor =                 input.color;
																		// output.FaceSign =                    input.isFrontFace;
																		// output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value

																		return output;
																	}

																	// existing HDRP code uses the combined function to go directly from packed to frag inputs
																	FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
																	{
																		UNITY_SETUP_INSTANCE_ID(input);
																		VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
																		return BuildFragInputs(unpacked);
																	}

																	//-------------------------------------------------------------------------------------
																	// END TEMPLATE INCLUDE : SharedCode.template.hlsl
																	//-------------------------------------------------------------------------------------


																		void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
																		{
																			// setup defaults -- these are used if the graph doesn't output a value
																			ZERO_INITIALIZE(SurfaceData, surfaceData);

																			// specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
																			// however specularOcclusion can come from the graph, so need to be init here so it can be override.
																			surfaceData.specularOcclusion = 1.0;

																			// copy across graph values, if defined
																			// surfaceData.baseColor =                 surfaceDescription.Albedo;
																			surfaceData.perceptualSmoothness = surfaceDescription.Smoothness;
																			// surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
																			// surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
																			// surfaceData.metallic =                  surfaceDescription.Metallic;
																			// surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
																			// surfaceData.thickness =                 surfaceDescription.Thickness;
																			// surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
																			// surfaceData.specularColor =             surfaceDescription.Specular;
																			// surfaceData.coatMask =                  surfaceDescription.CoatMask;
																			// surfaceData.anisotropy =                surfaceDescription.Anisotropy;
																			// surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
																			// surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;

																	#ifdef _HAS_REFRACTION
																			if (_EnableSSRefraction)
																			{
																				// surfaceData.ior =                       surfaceDescription.RefractionIndex;
																				// surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
																				// surfaceData.atDistance =                surfaceDescription.RefractionDistance;

																				surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
																				surfaceDescription.Alpha = 1.0;
																			}
																			else
																			{
																				surfaceData.ior = 1.0;
																				surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																				surfaceData.atDistance = 1.0;
																				surfaceData.transmittanceMask = 0.0;
																				surfaceDescription.Alpha = 1.0;
																			}
																	#else
																			surfaceData.ior = 1.0;
																			surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																			surfaceData.atDistance = 1.0;
																			surfaceData.transmittanceMask = 0.0;
																	#endif

																			// These static material feature allow compile time optimization
																			surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
																	#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
																			surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
																	#endif
																	#ifdef _MATERIAL_FEATURE_TRANSMISSION
																			surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
																	#endif
																	#ifdef _MATERIAL_FEATURE_ANISOTROPY
																			surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
																	#endif
																			// surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;

																	#ifdef _MATERIAL_FEATURE_IRIDESCENCE
																			surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
																	#endif
																	#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
																			surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
																	#endif

																	#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
																			// Require to have setup baseColor
																			// Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
																			surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
																	#endif

																	#ifdef _DOUBLESIDED_ON
																		float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
																	#else
																		float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
																	#endif

																		// tangent-space normal
																		float3 normalTS = float3(0.0f, 0.0f, 1.0f);
																		normalTS = surfaceDescription.Normal;

																		// compute world space normal
																		GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

																		surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

																		surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
																		// surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);

																#if HAVE_DECALS
																		if (_EnableDecals)
																		{
																			// Both uses and modifies 'surfaceData.normalWS'.
																			DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
																			ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
																		}
																#endif

																		bentNormalWS = surfaceData.normalWS;
																		// GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);

																		surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);


																		// By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
																		// If user provide bent normal then we process a better term
																#if defined(_SPECULAR_OCCLUSION_CUSTOM)
																		// Just use the value passed through via the slot (not active otherwise)
																#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
																		// If we have bent normal and ambient occlusion, process a specular occlusion
																		surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
																#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
																		surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
																#endif

																#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
																		surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
																#endif

																#ifdef DEBUG_DISPLAY
																		if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
																		{
																			// TODO: need to update mip info
																			surfaceData.metallic = 0;
																		}

																		// We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
																		// as it can modify attribute use for static lighting
																		ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
																#endif
																	}

																	void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
																	{
																#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
																		uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
																		LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
																#endif

																#ifdef _DOUBLESIDED_ON
																	float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
																#else
																	float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
																#endif

																		ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

																		SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
																		SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);

																		// Perform alpha test very early to save performance (a killed pixel will not sample textures)
																		// TODO: split graph evaluation to grab just alpha dependencies first? tricky..
																		// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
																		// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
																		// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
																		// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);

																		// ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);

																		float3 bentNormalWS;
																		BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

																		// Builtin Data
																		// For back lighting we use the oposite vertex normal 
																		InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

																		// override sampleBakedGI:
																		// builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
																		// builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;

																		// builtinData.emissiveColor = surfaceDescription.Emission;

																		// builtinData.depthOffset = surfaceDescription.DepthOffset;

																#if (SHADERPASS == SHADERPASS_DISTORTION)
																		builtinData.distortion = surfaceDescription.Distortion;
																		builtinData.distortionBlur = surfaceDescription.DistortionBlur;
																#else
																		builtinData.distortion = float2(0.0, 0.0);
																		builtinData.distortionBlur = 0.0;
																#endif

																		PostInitBuiltinData(V, posInput, surfaceData, builtinData);
																	}

																	//-------------------------------------------------------------------------------------
																	// Pass Includes
																	//-------------------------------------------------------------------------------------
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
																	//-------------------------------------------------------------------------------------
																	// End Pass Includes
																	//-------------------------------------------------------------------------------------

																	ENDHLSL
																}

																Pass
																{
																		// based on HDLitPass.template
																		Name "GBuffer"
																		Tags { "LightMode" = "GBuffer" }

																		//-------------------------------------------------------------------------------------
																		// Render Modes (Blend, Cull, ZTest, Stencil, etc)
																		//-------------------------------------------------------------------------------------

																		Cull[_CullMode]

																		ZTest[_ZTestGBuffer]



																		// Stencil setup
																	Stencil
																	{
																	   WriteMask[_StencilWriteMaskGBuffer]
																	   Ref[_StencilRefGBuffer]
																	   Comp Always
																	   Pass Replace
																	}


																		//-------------------------------------------------------------------------------------
																		// End Render Modes
																		//-------------------------------------------------------------------------------------

																		HLSLPROGRAM

																		#pragma target 4.5
																		#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
																		//#pragma enable_d3d11_debug_symbols

																		#pragma multi_compile_instancing
																	#pragma instancing_options renderinglayer

																		#pragma multi_compile _ LOD_FADE_CROSSFADE

																		#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
																		#pragma shader_feature_local _DOUBLESIDED_ON
																		#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

																		//-------------------------------------------------------------------------------------
																		// Variant Definitions (active field translations to HDRP defines)
																		//-------------------------------------------------------------------------------------
																		// #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
																		// #define _MATERIAL_FEATURE_TRANSMISSION 1
																		// #define _MATERIAL_FEATURE_ANISOTROPY 1
																		// #define _MATERIAL_FEATURE_IRIDESCENCE 1
																		// #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
																		// #define _ENABLE_FOG_ON_TRANSPARENT 1
																		// #define _AMBIENT_OCCLUSION 1
																		// #define _SPECULAR_OCCLUSION_FROM_AO 1
																		// #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
																		// #define _SPECULAR_OCCLUSION_CUSTOM 1
																		#define _ENERGY_CONSERVING_SPECULAR 1
																		// #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
																		// #define _HAS_REFRACTION 1
																		// #define _REFRACTION_PLANE 1
																		// #define _REFRACTION_SPHERE 1
																		// #define _DISABLE_DECALS 1
																		// #define _DISABLE_SSR 1
																		// #define _ADD_PRECOMPUTED_VELOCITY
																		// #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
																		// #define _DEPTHOFFSET_ON 1
																		// #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

																		//-------------------------------------------------------------------------------------
																		// End Variant Definitions
																		//-------------------------------------------------------------------------------------

																		#pragma vertex Vert
																		#pragma fragment Frag

																		// If we use subsurface scattering, enable output split lighting (for forward pass)
																		#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
																		#define OUTPUT_SPLIT_LIGHTING
																		#endif

																		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

																		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"

																		// define FragInputs structure
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

																		//-------------------------------------------------------------------------------------
																		// Defines
																		//-------------------------------------------------------------------------------------
																				#define SHADERPASS SHADERPASS_GBUFFER
																			#pragma multi_compile _ DEBUG_DISPLAY
																			#pragma multi_compile _ LIGHTMAP_ON
																			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
																			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
																			#pragma multi_compile _ SHADOWS_SHADOWMASK
																			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
																			#pragma multi_compile _ LIGHT_LAYERS
																			// ACTIVE FIELDS:
																			//   Material.Standard
																			//   Specular.EnergyConserving
																			//   SurfaceDescriptionInputs.TangentSpaceNormal
																			//   SurfaceDescription.Albedo
																			//   SurfaceDescription.Normal
																			//   SurfaceDescription.BentNormal
																			//   SurfaceDescription.CoatMask
																			//   SurfaceDescription.Metallic
																			//   SurfaceDescription.Emission
																			//   SurfaceDescription.Smoothness
																			//   SurfaceDescription.Occlusion
																			//   SurfaceDescription.Alpha
																			//   FragInputs.tangentToWorld
																			//   FragInputs.positionRWS
																			//   FragInputs.texCoord1
																			//   FragInputs.texCoord2
																			//   VaryingsMeshToPS.tangentWS
																			//   VaryingsMeshToPS.normalWS
																			//   VaryingsMeshToPS.positionRWS
																			//   VaryingsMeshToPS.texCoord1
																			//   VaryingsMeshToPS.texCoord2
																			//   AttributesMesh.tangentOS
																			//   AttributesMesh.normalOS
																			//   AttributesMesh.positionOS
																			//   AttributesMesh.uv1
																			//   AttributesMesh.uv2

																		// this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
																		#define ATTRIBUTES_NEED_NORMAL
																		#define ATTRIBUTES_NEED_TANGENT
																		// #define ATTRIBUTES_NEED_TEXCOORD0
																		#define ATTRIBUTES_NEED_TEXCOORD1
																		#define ATTRIBUTES_NEED_TEXCOORD2
																		// #define ATTRIBUTES_NEED_TEXCOORD3
																		// #define ATTRIBUTES_NEED_COLOR
																		#define VARYINGS_NEED_POSITION_WS
																		#define VARYINGS_NEED_TANGENT_TO_WORLD
																		// #define VARYINGS_NEED_TEXCOORD0
																		#define VARYINGS_NEED_TEXCOORD1
																		#define VARYINGS_NEED_TEXCOORD2
																		// #define VARYINGS_NEED_TEXCOORD3
																		// #define VARYINGS_NEED_COLOR
																		// #define VARYINGS_NEED_CULLFACE
																		// #define HAVE_MESH_MODIFICATION

																	// We need isFontFace when using double sided
																	#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
																		#define VARYINGS_NEED_CULLFACE
																	#endif

																		//-------------------------------------------------------------------------------------
																		// End Defines
																		//-------------------------------------------------------------------------------------

																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
																	#ifdef DEBUG_DISPLAY
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
																	#endif

																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

																	#if (SHADERPASS == SHADERPASS_FORWARD)
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

																		#define HAS_LIGHTLOOP

																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
																	#else
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
																	#endif

																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
																		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

																		// Used by SceneSelectionPass
																		int _ObjectId;
																		int _PassValue;

																		//-------------------------------------------------------------------------------------
																		// Interpolator Packing And Struct Declarations
																		//-------------------------------------------------------------------------------------
																	// Generated Type: AttributesMesh
																	struct AttributesMesh {
																		float3 positionOS : POSITION;
																		float3 normalOS : NORMAL; // optional
																		float4 tangentOS : TANGENT; // optional
																		float4 uv1 : TEXCOORD1; // optional
																		float4 uv2 : TEXCOORD2; // optional
																		#if UNITY_ANY_INSTANCING_ENABLED
																		uint instanceID : INSTANCEID_SEMANTIC;
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																	};

																	// Generated Type: VaryingsMeshToPS
																	struct VaryingsMeshToPS {
																		float4 positionCS : SV_Position;
																		float3 positionRWS; // optional
																		float3 normalWS; // optional
																		float4 tangentWS; // optional
																		float4 texCoord1; // optional
																		float4 texCoord2; // optional
																		#if UNITY_ANY_INSTANCING_ENABLED
																		uint instanceID : CUSTOM_INSTANCE_ID;
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
																		#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																	};
																	struct PackedVaryingsMeshToPS {
																		float3 interp00 : TEXCOORD0; // auto-packed
																		float3 interp01 : TEXCOORD1; // auto-packed
																		float4 interp02 : TEXCOORD2; // auto-packed
																		float4 interp03 : TEXCOORD3; // auto-packed
																		float4 interp04 : TEXCOORD4; // auto-packed
																		float4 positionCS : SV_Position; // unpacked
																		#if UNITY_ANY_INSTANCING_ENABLED
																		uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
																		#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																	};
																	PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
																	{
																		PackedVaryingsMeshToPS output;
																		output.positionCS = input.positionCS;
																		output.interp00.xyz = input.positionRWS;
																		output.interp01.xyz = input.normalWS;
																		output.interp02.xyzw = input.tangentWS;
																		output.interp03.xyzw = input.texCoord1;
																		output.interp04.xyzw = input.texCoord2;
																		#if UNITY_ANY_INSTANCING_ENABLED
																		output.instanceID = input.instanceID;
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																		output.cullFace = input.cullFace;
																		#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																		return output;
																	}
																	VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
																	{
																		VaryingsMeshToPS output;
																		output.positionCS = input.positionCS;
																		output.positionRWS = input.interp00.xyz;
																		output.normalWS = input.interp01.xyz;
																		output.tangentWS = input.interp02.xyzw;
																		output.texCoord1 = input.interp03.xyzw;
																		output.texCoord2 = input.interp04.xyzw;
																		#if UNITY_ANY_INSTANCING_ENABLED
																		output.instanceID = input.instanceID;
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																		output.cullFace = input.cullFace;
																		#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																		return output;
																	}

																	// Generated Type: VaryingsMeshToDS
																	struct VaryingsMeshToDS {
																		float3 positionRWS;
																		float3 normalWS;
																		#if UNITY_ANY_INSTANCING_ENABLED
																		uint instanceID : CUSTOM_INSTANCE_ID;
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																	};
																	struct PackedVaryingsMeshToDS {
																		float3 interp00 : TEXCOORD0; // auto-packed
																		float3 interp01 : TEXCOORD1; // auto-packed
																		#if UNITY_ANY_INSTANCING_ENABLED
																		uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																	};
																	PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
																	{
																		PackedVaryingsMeshToDS output;
																		output.interp00.xyz = input.positionRWS;
																		output.interp01.xyz = input.normalWS;
																		#if UNITY_ANY_INSTANCING_ENABLED
																		output.instanceID = input.instanceID;
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																		return output;
																	}
																	VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
																	{
																		VaryingsMeshToDS output;
																		output.positionRWS = input.interp00.xyz;
																		output.normalWS = input.interp01.xyz;
																		#if UNITY_ANY_INSTANCING_ENABLED
																		output.instanceID = input.instanceID;
																		#endif // UNITY_ANY_INSTANCING_ENABLED
																		return output;
																	}

																	//-------------------------------------------------------------------------------------
																	// End Interpolator Packing And Struct Declarations
																	//-------------------------------------------------------------------------------------

																	//-------------------------------------------------------------------------------------
																	// Graph generated code
																	//-------------------------------------------------------------------------------------
																			// Shared Graph Properties (uniform inputs)
																			CBUFFER_START(UnityPerMaterial)
																			float4 _EmissionColor;
																			float _RenderQueueType;
																			float _StencilRef;
																			float _StencilWriteMask;
																			float _StencilRefDepth;
																			float _StencilWriteMaskDepth;
																			float _StencilRefMV;
																			float _StencilWriteMaskMV;
																			float _StencilRefDistortionVec;
																			float _StencilWriteMaskDistortionVec;
																			float _StencilWriteMaskGBuffer;
																			float _StencilRefGBuffer;
																			float _ZTestGBuffer;
																			float _RequireSplitLighting;
																			float _ReceivesSSR;
																			float _SurfaceType;
																			float _BlendMode;
																			float _SrcBlend;
																			float _DstBlend;
																			float _AlphaSrcBlend;
																			float _AlphaDstBlend;
																			float _ZWrite;
																			float _CullMode;
																			float _TransparentSortPriority;
																			float _CullModeForward;
																			float _TransparentCullMode;
																			float _ZTestDepthEqualForOpaque;
																			float _ZTestTransparent;
																			float _TransparentBackfaceEnable;
																			float _AlphaCutoffEnable;
																			float _AlphaCutoff;
																			float _UseShadowThreshold;
																			float _DoubleSidedEnable;
																			float _DoubleSidedNormalMode;
																			float4 _DoubleSidedConstants;
																			CBUFFER_END


																				// Pixel Graph Inputs
																					struct SurfaceDescriptionInputs {
																						float3 TangentSpaceNormal; // optional
																					};
																			// Pixel Graph Outputs
																				struct SurfaceDescription
																				{
																					float3 Albedo;
																					float3 Normal;
																					float3 BentNormal;
																					float CoatMask;
																					float Metallic;
																					float3 Emission;
																					float Smoothness;
																					float Occlusion;
																					float Alpha;
																				};

																				// Shared Graph Node Functions
																				// Pixel Graph Evaluation
																					SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
																					{
																						SurfaceDescription surface = (SurfaceDescription)0;
																						surface.Albedo = IsGammaSpace() ? float3(0.7353569, 0.7353569, 0.7353569) : SRGBToLinear(float3(0.7353569, 0.7353569, 0.7353569));
																						surface.Normal = IN.TangentSpaceNormal;
																						surface.BentNormal = IN.TangentSpaceNormal;
																						surface.CoatMask = 0;
																						surface.Metallic = 0;
																						surface.Emission = float3(0, 0, 0);
																						surface.Smoothness = 0.5;
																						surface.Occlusion = 1;
																						surface.Alpha = 1;
																						return surface;
																					}

																					//-------------------------------------------------------------------------------------
																					// End graph generated code
																					//-------------------------------------------------------------------------------------

																				// $include("VertexAnimation.template.hlsl")


																				//-------------------------------------------------------------------------------------
																				// TEMPLATE INCLUDE : SharedCode.template.hlsl
																				//-------------------------------------------------------------------------------------
																					FragInputs BuildFragInputs(VaryingsMeshToPS input)
																					{
																						FragInputs output;
																						ZERO_INITIALIZE(FragInputs, output);

																						// Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
																						// TODO: this is a really poor workaround, but the variable is used in a bunch of places
																						// to compute normals which are then passed on elsewhere to compute other values...
																						output.tangentToWorld = k_identity3x3;
																						output.positionSS = input.positionCS;       // input.positionCS is SV_Position

																						output.positionRWS = input.positionRWS;
																						output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
																						// output.texCoord0 = input.texCoord0;
																						output.texCoord1 = input.texCoord1;
																						output.texCoord2 = input.texCoord2;
																						// output.texCoord3 = input.texCoord3;
																						// output.color = input.color;
																						#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
																						output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
																						#elif SHADER_STAGE_FRAGMENT
																						// output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
																						#endif // SHADER_STAGE_FRAGMENT

																						return output;
																					}

																					SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
																					{
																						SurfaceDescriptionInputs output;
																						ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

																						// output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
																						// output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
																						// output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
																						output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
																						// output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
																						// output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
																						// output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
																						// output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
																						// output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
																						// output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
																						// output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
																						// output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
																						// output.WorldSpaceViewDirection =     normalize(viewWS);
																						// output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
																						// output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
																						// float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
																						// output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
																						// output.WorldSpacePosition =          GetAbsolutePositionWS(input.positionRWS);
																						// output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
																						// output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
																						// output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
																						// output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
																						// output.uv0 =                         input.texCoord0;
																						// output.uv1 =                         input.texCoord1;
																						// output.uv2 =                         input.texCoord2;
																						// output.uv3 =                         input.texCoord3;
																						// output.VertexColor =                 input.color;
																						// output.FaceSign =                    input.isFrontFace;
																						// output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value

																						return output;
																					}

																					// existing HDRP code uses the combined function to go directly from packed to frag inputs
																					FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
																					{
																						UNITY_SETUP_INSTANCE_ID(input);
																						VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
																						return BuildFragInputs(unpacked);
																					}

																					//-------------------------------------------------------------------------------------
																					// END TEMPLATE INCLUDE : SharedCode.template.hlsl
																					//-------------------------------------------------------------------------------------


																						void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
																						{
																							// setup defaults -- these are used if the graph doesn't output a value
																							ZERO_INITIALIZE(SurfaceData, surfaceData);

																							// specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
																							// however specularOcclusion can come from the graph, so need to be init here so it can be override.
																							surfaceData.specularOcclusion = 1.0;

																							// copy across graph values, if defined
																							surfaceData.baseColor = surfaceDescription.Albedo;
																							surfaceData.perceptualSmoothness = surfaceDescription.Smoothness;
																							surfaceData.ambientOcclusion = surfaceDescription.Occlusion;
																							// surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
																							surfaceData.metallic = surfaceDescription.Metallic;
																							// surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
																							// surfaceData.thickness =                 surfaceDescription.Thickness;
																							// surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
																							// surfaceData.specularColor =             surfaceDescription.Specular;
																							surfaceData.coatMask = surfaceDescription.CoatMask;
																							// surfaceData.anisotropy =                surfaceDescription.Anisotropy;
																							// surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
																							// surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;

																					#ifdef _HAS_REFRACTION
																							if (_EnableSSRefraction)
																							{
																								// surfaceData.ior =                       surfaceDescription.RefractionIndex;
																								// surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
																								// surfaceData.atDistance =                surfaceDescription.RefractionDistance;

																								surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
																								surfaceDescription.Alpha = 1.0;
																							}
																							else
																							{
																								surfaceData.ior = 1.0;
																								surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																								surfaceData.atDistance = 1.0;
																								surfaceData.transmittanceMask = 0.0;
																								surfaceDescription.Alpha = 1.0;
																							}
																					#else
																							surfaceData.ior = 1.0;
																							surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																							surfaceData.atDistance = 1.0;
																							surfaceData.transmittanceMask = 0.0;
																					#endif

																							// These static material feature allow compile time optimization
																							surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
																					#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
																							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
																					#endif
																					#ifdef _MATERIAL_FEATURE_TRANSMISSION
																							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
																					#endif
																					#ifdef _MATERIAL_FEATURE_ANISOTROPY
																							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
																					#endif
																							// surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;

																					#ifdef _MATERIAL_FEATURE_IRIDESCENCE
																							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
																					#endif
																					#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
																							surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
																					#endif

																					#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
																							// Require to have setup baseColor
																							// Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
																							surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
																					#endif

																					#ifdef _DOUBLESIDED_ON
																						float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
																					#else
																						float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
																					#endif

																						// tangent-space normal
																						float3 normalTS = float3(0.0f, 0.0f, 1.0f);
																						normalTS = surfaceDescription.Normal;

																						// compute world space normal
																						GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

																						surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

																						surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
																						// surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);

																				#if HAVE_DECALS
																						if (_EnableDecals)
																						{
																							// Both uses and modifies 'surfaceData.normalWS'.
																							DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
																							ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
																						}
																				#endif

																						bentNormalWS = surfaceData.normalWS;
																						// GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);

																						surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);


																						// By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
																						// If user provide bent normal then we process a better term
																				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
																						// Just use the value passed through via the slot (not active otherwise)
																				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
																						// If we have bent normal and ambient occlusion, process a specular occlusion
																						surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
																				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
																						surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
																				#endif

																				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
																						surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
																				#endif

																				#ifdef DEBUG_DISPLAY
																						if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
																						{
																							// TODO: need to update mip info
																							surfaceData.metallic = 0;
																						}

																						// We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
																						// as it can modify attribute use for static lighting
																						ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
																				#endif
																					}

																					void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
																					{
																				#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
																						uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
																						LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
																				#endif

																				#ifdef _DOUBLESIDED_ON
																					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
																				#else
																					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
																				#endif

																						ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

																						SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
																						SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);

																						// Perform alpha test very early to save performance (a killed pixel will not sample textures)
																						// TODO: split graph evaluation to grab just alpha dependencies first? tricky..
																						// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
																						// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
																						// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
																						// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);

																						// ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);

																						float3 bentNormalWS;
																						BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

																						// Builtin Data
																						// For back lighting we use the oposite vertex normal 
																						InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

																						// override sampleBakedGI:
																						// builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
																						// builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;

																						builtinData.emissiveColor = surfaceDescription.Emission;

																						// builtinData.depthOffset = surfaceDescription.DepthOffset;

																				#if (SHADERPASS == SHADERPASS_DISTORTION)
																						builtinData.distortion = surfaceDescription.Distortion;
																						builtinData.distortionBlur = surfaceDescription.DistortionBlur;
																				#else
																						builtinData.distortion = float2(0.0, 0.0);
																						builtinData.distortionBlur = 0.0;
																				#endif

																						PostInitBuiltinData(V, posInput, surfaceData, builtinData);
																					}

																					//-------------------------------------------------------------------------------------
																					// Pass Includes
																					//-------------------------------------------------------------------------------------
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassGBuffer.hlsl"
																					//-------------------------------------------------------------------------------------
																					// End Pass Includes
																					//-------------------------------------------------------------------------------------

																					ENDHLSL
																				}

																				Pass
																				{
																						// based on HDLitPass.template
																						Name "MotionVectors"
																						Tags { "LightMode" = "MotionVectors" }

																						//-------------------------------------------------------------------------------------
																						// Render Modes (Blend, Cull, ZTest, Stencil, etc)
																						//-------------------------------------------------------------------------------------

																						Cull[_CullMode]


																						ZWrite On


																						// Stencil setup
																					Stencil
																					{
																					   WriteMask[_StencilWriteMaskMV]
																					   Ref[_StencilRefMV]
																					   Comp Always
																					   Pass Replace
																					}


																						//-------------------------------------------------------------------------------------
																						// End Render Modes
																						//-------------------------------------------------------------------------------------

																						HLSLPROGRAM

																						#pragma target 4.5
																						#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
																						//#pragma enable_d3d11_debug_symbols

																						#pragma multi_compile_instancing
																					#pragma instancing_options renderinglayer

																						#pragma multi_compile _ LOD_FADE_CROSSFADE

																						#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
																						#pragma shader_feature_local _DOUBLESIDED_ON
																						#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

																						//-------------------------------------------------------------------------------------
																						// Variant Definitions (active field translations to HDRP defines)
																						//-------------------------------------------------------------------------------------
																						// #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
																						// #define _MATERIAL_FEATURE_TRANSMISSION 1
																						// #define _MATERIAL_FEATURE_ANISOTROPY 1
																						// #define _MATERIAL_FEATURE_IRIDESCENCE 1
																						// #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
																						// #define _ENABLE_FOG_ON_TRANSPARENT 1
																						// #define _AMBIENT_OCCLUSION 1
																						// #define _SPECULAR_OCCLUSION_FROM_AO 1
																						// #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
																						// #define _SPECULAR_OCCLUSION_CUSTOM 1
																						#define _ENERGY_CONSERVING_SPECULAR 1
																						// #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
																						// #define _HAS_REFRACTION 1
																						// #define _REFRACTION_PLANE 1
																						// #define _REFRACTION_SPHERE 1
																						// #define _DISABLE_DECALS 1
																						// #define _DISABLE_SSR 1
																						// #define _ADD_PRECOMPUTED_VELOCITY
																						// #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
																						// #define _DEPTHOFFSET_ON 1
																						// #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

																						//-------------------------------------------------------------------------------------
																						// End Variant Definitions
																						//-------------------------------------------------------------------------------------

																						#pragma vertex Vert
																						#pragma fragment Frag

																						// If we use subsurface scattering, enable output split lighting (for forward pass)
																						#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
																						#define OUTPUT_SPLIT_LIGHTING
																						#endif

																						#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

																						#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"

																						// define FragInputs structure
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

																						//-------------------------------------------------------------------------------------
																						// Defines
																						//-------------------------------------------------------------------------------------
																								#define SHADERPASS SHADERPASS_MOTION_VECTORS
																							#pragma multi_compile _ WRITE_NORMAL_BUFFER
																							#pragma multi_compile _ WRITE_MSAA_DEPTH
																							// ACTIVE FIELDS:
																							//   Material.Standard
																							//   Specular.EnergyConserving
																							//   SurfaceDescriptionInputs.TangentSpaceNormal
																							//   SurfaceDescription.Normal
																							//   SurfaceDescription.Smoothness
																							//   SurfaceDescription.Alpha
																							//   AttributesMesh.normalOS
																							//   AttributesMesh.tangentOS
																							//   AttributesMesh.uv0
																							//   AttributesMesh.uv1
																							//   AttributesMesh.color
																							//   AttributesMesh.uv2
																							//   AttributesMesh.uv3
																							//   FragInputs.tangentToWorld
																							//   FragInputs.positionRWS
																							//   FragInputs.texCoord0
																							//   FragInputs.texCoord1
																							//   FragInputs.texCoord2
																							//   FragInputs.texCoord3
																							//   FragInputs.color
																							//   VaryingsMeshToPS.tangentWS
																							//   VaryingsMeshToPS.normalWS
																							//   VaryingsMeshToPS.positionRWS
																							//   VaryingsMeshToPS.texCoord0
																							//   VaryingsMeshToPS.texCoord1
																							//   VaryingsMeshToPS.texCoord2
																							//   VaryingsMeshToPS.texCoord3
																							//   VaryingsMeshToPS.color
																							//   AttributesMesh.positionOS

																						// this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
																						#define ATTRIBUTES_NEED_NORMAL
																						#define ATTRIBUTES_NEED_TANGENT
																						#define ATTRIBUTES_NEED_TEXCOORD0
																						#define ATTRIBUTES_NEED_TEXCOORD1
																						#define ATTRIBUTES_NEED_TEXCOORD2
																						#define ATTRIBUTES_NEED_TEXCOORD3
																						#define ATTRIBUTES_NEED_COLOR
																						#define VARYINGS_NEED_POSITION_WS
																						#define VARYINGS_NEED_TANGENT_TO_WORLD
																						#define VARYINGS_NEED_TEXCOORD0
																						#define VARYINGS_NEED_TEXCOORD1
																						#define VARYINGS_NEED_TEXCOORD2
																						#define VARYINGS_NEED_TEXCOORD3
																						#define VARYINGS_NEED_COLOR
																						// #define VARYINGS_NEED_CULLFACE
																						// #define HAVE_MESH_MODIFICATION

																					// We need isFontFace when using double sided
																					#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
																						#define VARYINGS_NEED_CULLFACE
																					#endif

																						//-------------------------------------------------------------------------------------
																						// End Defines
																						//-------------------------------------------------------------------------------------

																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
																					#ifdef DEBUG_DISPLAY
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
																					#endif

																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

																					#if (SHADERPASS == SHADERPASS_FORWARD)
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

																						#define HAS_LIGHTLOOP

																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
																					#else
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
																					#endif

																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
																						#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

																						// Used by SceneSelectionPass
																						int _ObjectId;
																						int _PassValue;

																						//-------------------------------------------------------------------------------------
																						// Interpolator Packing And Struct Declarations
																						//-------------------------------------------------------------------------------------
																					// Generated Type: AttributesMesh
																					struct AttributesMesh {
																						float3 positionOS : POSITION;
																						float3 normalOS : NORMAL; // optional
																						float4 tangentOS : TANGENT; // optional
																						float4 uv0 : TEXCOORD0; // optional
																						float4 uv1 : TEXCOORD1; // optional
																						float4 uv2 : TEXCOORD2; // optional
																						float4 uv3 : TEXCOORD3; // optional
																						float4 color : COLOR; // optional
																						#if UNITY_ANY_INSTANCING_ENABLED
																						uint instanceID : INSTANCEID_SEMANTIC;
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																					};

																					// Generated Type: VaryingsMeshToPS
																					struct VaryingsMeshToPS {
																						float4 positionCS : SV_Position;
																						float3 positionRWS; // optional
																						float3 normalWS; // optional
																						float4 tangentWS; // optional
																						float4 texCoord0; // optional
																						float4 texCoord1; // optional
																						float4 texCoord2; // optional
																						float4 texCoord3; // optional
																						float4 color; // optional
																						#if UNITY_ANY_INSTANCING_ENABLED
																						uint instanceID : CUSTOM_INSTANCE_ID;
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																						FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
																						#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																					};
																					struct PackedVaryingsMeshToPS {
																						float3 interp00 : TEXCOORD0; // auto-packed
																						float3 interp01 : TEXCOORD1; // auto-packed
																						float4 interp02 : TEXCOORD2; // auto-packed
																						float4 interp03 : TEXCOORD3; // auto-packed
																						float4 interp04 : TEXCOORD4; // auto-packed
																						float4 interp05 : TEXCOORD5; // auto-packed
																						float4 interp06 : TEXCOORD6; // auto-packed
																						float4 interp07 : TEXCOORD7; // auto-packed
																						float4 positionCS : SV_Position; // unpacked
																						#if UNITY_ANY_INSTANCING_ENABLED
																						uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																						FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
																						#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																					};
																					PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
																					{
																						PackedVaryingsMeshToPS output;
																						output.positionCS = input.positionCS;
																						output.interp00.xyz = input.positionRWS;
																						output.interp01.xyz = input.normalWS;
																						output.interp02.xyzw = input.tangentWS;
																						output.interp03.xyzw = input.texCoord0;
																						output.interp04.xyzw = input.texCoord1;
																						output.interp05.xyzw = input.texCoord2;
																						output.interp06.xyzw = input.texCoord3;
																						output.interp07.xyzw = input.color;
																						#if UNITY_ANY_INSTANCING_ENABLED
																						output.instanceID = input.instanceID;
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																						output.cullFace = input.cullFace;
																						#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																						return output;
																					}
																					VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
																					{
																						VaryingsMeshToPS output;
																						output.positionCS = input.positionCS;
																						output.positionRWS = input.interp00.xyz;
																						output.normalWS = input.interp01.xyz;
																						output.tangentWS = input.interp02.xyzw;
																						output.texCoord0 = input.interp03.xyzw;
																						output.texCoord1 = input.interp04.xyzw;
																						output.texCoord2 = input.interp05.xyzw;
																						output.texCoord3 = input.interp06.xyzw;
																						output.color = input.interp07.xyzw;
																						#if UNITY_ANY_INSTANCING_ENABLED
																						output.instanceID = input.instanceID;
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																						output.cullFace = input.cullFace;
																						#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																						return output;
																					}

																					// Generated Type: VaryingsMeshToDS
																					struct VaryingsMeshToDS {
																						float3 positionRWS;
																						float3 normalWS;
																						#if UNITY_ANY_INSTANCING_ENABLED
																						uint instanceID : CUSTOM_INSTANCE_ID;
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																					};
																					struct PackedVaryingsMeshToDS {
																						float3 interp00 : TEXCOORD0; // auto-packed
																						float3 interp01 : TEXCOORD1; // auto-packed
																						#if UNITY_ANY_INSTANCING_ENABLED
																						uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																					};
																					PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
																					{
																						PackedVaryingsMeshToDS output;
																						output.interp00.xyz = input.positionRWS;
																						output.interp01.xyz = input.normalWS;
																						#if UNITY_ANY_INSTANCING_ENABLED
																						output.instanceID = input.instanceID;
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																						return output;
																					}
																					VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
																					{
																						VaryingsMeshToDS output;
																						output.positionRWS = input.interp00.xyz;
																						output.normalWS = input.interp01.xyz;
																						#if UNITY_ANY_INSTANCING_ENABLED
																						output.instanceID = input.instanceID;
																						#endif // UNITY_ANY_INSTANCING_ENABLED
																						return output;
																					}

																					//-------------------------------------------------------------------------------------
																					// End Interpolator Packing And Struct Declarations
																					//-------------------------------------------------------------------------------------

																					//-------------------------------------------------------------------------------------
																					// Graph generated code
																					//-------------------------------------------------------------------------------------
																							// Shared Graph Properties (uniform inputs)
																							CBUFFER_START(UnityPerMaterial)
																							float4 _EmissionColor;
																							float _RenderQueueType;
																							float _StencilRef;
																							float _StencilWriteMask;
																							float _StencilRefDepth;
																							float _StencilWriteMaskDepth;
																							float _StencilRefMV;
																							float _StencilWriteMaskMV;
																							float _StencilRefDistortionVec;
																							float _StencilWriteMaskDistortionVec;
																							float _StencilWriteMaskGBuffer;
																							float _StencilRefGBuffer;
																							float _ZTestGBuffer;
																							float _RequireSplitLighting;
																							float _ReceivesSSR;
																							float _SurfaceType;
																							float _BlendMode;
																							float _SrcBlend;
																							float _DstBlend;
																							float _AlphaSrcBlend;
																							float _AlphaDstBlend;
																							float _ZWrite;
																							float _CullMode;
																							float _TransparentSortPriority;
																							float _CullModeForward;
																							float _TransparentCullMode;
																							float _ZTestDepthEqualForOpaque;
																							float _ZTestTransparent;
																							float _TransparentBackfaceEnable;
																							float _AlphaCutoffEnable;
																							float _AlphaCutoff;
																							float _UseShadowThreshold;
																							float _DoubleSidedEnable;
																							float _DoubleSidedNormalMode;
																							float4 _DoubleSidedConstants;
																							CBUFFER_END


																								// Pixel Graph Inputs
																									struct SurfaceDescriptionInputs {
																										float3 TangentSpaceNormal; // optional
																									};
																							// Pixel Graph Outputs
																								struct SurfaceDescription
																								{
																									float3 Normal;
																									float Smoothness;
																									float Alpha;
																								};

																								// Shared Graph Node Functions
																								// Pixel Graph Evaluation
																									SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
																									{
																										SurfaceDescription surface = (SurfaceDescription)0;
																										surface.Normal = IN.TangentSpaceNormal;
																										surface.Smoothness = 0.5;
																										surface.Alpha = 1;
																										return surface;
																									}

																									//-------------------------------------------------------------------------------------
																									// End graph generated code
																									//-------------------------------------------------------------------------------------

																								// $include("VertexAnimation.template.hlsl")


																								//-------------------------------------------------------------------------------------
																								// TEMPLATE INCLUDE : SharedCode.template.hlsl
																								//-------------------------------------------------------------------------------------
																									FragInputs BuildFragInputs(VaryingsMeshToPS input)
																									{
																										FragInputs output;
																										ZERO_INITIALIZE(FragInputs, output);

																										// Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
																										// TODO: this is a really poor workaround, but the variable is used in a bunch of places
																										// to compute normals which are then passed on elsewhere to compute other values...
																										output.tangentToWorld = k_identity3x3;
																										output.positionSS = input.positionCS;       // input.positionCS is SV_Position

																										output.positionRWS = input.positionRWS;
																										output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
																										output.texCoord0 = input.texCoord0;
																										output.texCoord1 = input.texCoord1;
																										output.texCoord2 = input.texCoord2;
																										output.texCoord3 = input.texCoord3;
																										output.color = input.color;
																										#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
																										output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
																										#elif SHADER_STAGE_FRAGMENT
																										// output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
																										#endif // SHADER_STAGE_FRAGMENT

																										return output;
																									}

																									SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
																									{
																										SurfaceDescriptionInputs output;
																										ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

																										// output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
																										// output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
																										// output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
																										output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
																										// output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
																										// output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
																										// output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
																										// output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
																										// output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
																										// output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
																										// output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
																										// output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
																										// output.WorldSpaceViewDirection =     normalize(viewWS);
																										// output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
																										// output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
																										// float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
																										// output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
																										// output.WorldSpacePosition =          GetAbsolutePositionWS(input.positionRWS);
																										// output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
																										// output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
																										// output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
																										// output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
																										// output.uv0 =                         input.texCoord0;
																										// output.uv1 =                         input.texCoord1;
																										// output.uv2 =                         input.texCoord2;
																										// output.uv3 =                         input.texCoord3;
																										// output.VertexColor =                 input.color;
																										// output.FaceSign =                    input.isFrontFace;
																										// output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value

																										return output;
																									}

																									// existing HDRP code uses the combined function to go directly from packed to frag inputs
																									FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
																									{
																										UNITY_SETUP_INSTANCE_ID(input);
																										VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
																										return BuildFragInputs(unpacked);
																									}

																									//-------------------------------------------------------------------------------------
																									// END TEMPLATE INCLUDE : SharedCode.template.hlsl
																									//-------------------------------------------------------------------------------------


																										void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
																										{
																											// setup defaults -- these are used if the graph doesn't output a value
																											ZERO_INITIALIZE(SurfaceData, surfaceData);

																											// specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
																											// however specularOcclusion can come from the graph, so need to be init here so it can be override.
																											surfaceData.specularOcclusion = 1.0;

																											// copy across graph values, if defined
																											// surfaceData.baseColor =                 surfaceDescription.Albedo;
																											surfaceData.perceptualSmoothness = surfaceDescription.Smoothness;
																											// surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
																											// surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
																											// surfaceData.metallic =                  surfaceDescription.Metallic;
																											// surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
																											// surfaceData.thickness =                 surfaceDescription.Thickness;
																											// surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
																											// surfaceData.specularColor =             surfaceDescription.Specular;
																											// surfaceData.coatMask =                  surfaceDescription.CoatMask;
																											// surfaceData.anisotropy =                surfaceDescription.Anisotropy;
																											// surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
																											// surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;

																									#ifdef _HAS_REFRACTION
																											if (_EnableSSRefraction)
																											{
																												// surfaceData.ior =                       surfaceDescription.RefractionIndex;
																												// surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
																												// surfaceData.atDistance =                surfaceDescription.RefractionDistance;

																												surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
																												surfaceDescription.Alpha = 1.0;
																											}
																											else
																											{
																												surfaceData.ior = 1.0;
																												surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																												surfaceData.atDistance = 1.0;
																												surfaceData.transmittanceMask = 0.0;
																												surfaceDescription.Alpha = 1.0;
																											}
																									#else
																											surfaceData.ior = 1.0;
																											surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																											surfaceData.atDistance = 1.0;
																											surfaceData.transmittanceMask = 0.0;
																									#endif

																											// These static material feature allow compile time optimization
																											surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
																									#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
																											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
																									#endif
																									#ifdef _MATERIAL_FEATURE_TRANSMISSION
																											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
																									#endif
																									#ifdef _MATERIAL_FEATURE_ANISOTROPY
																											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
																									#endif
																											// surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;

																									#ifdef _MATERIAL_FEATURE_IRIDESCENCE
																											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
																									#endif
																									#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
																											surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
																									#endif

																									#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
																											// Require to have setup baseColor
																											// Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
																											surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
																									#endif

																									#ifdef _DOUBLESIDED_ON
																										float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
																									#else
																										float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
																									#endif

																										// tangent-space normal
																										float3 normalTS = float3(0.0f, 0.0f, 1.0f);
																										normalTS = surfaceDescription.Normal;

																										// compute world space normal
																										GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

																										surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

																										surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
																										// surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);

																								#if HAVE_DECALS
																										if (_EnableDecals)
																										{
																											// Both uses and modifies 'surfaceData.normalWS'.
																											DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
																											ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
																										}
																								#endif

																										bentNormalWS = surfaceData.normalWS;
																										// GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);

																										surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);


																										// By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
																										// If user provide bent normal then we process a better term
																								#if defined(_SPECULAR_OCCLUSION_CUSTOM)
																										// Just use the value passed through via the slot (not active otherwise)
																								#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
																										// If we have bent normal and ambient occlusion, process a specular occlusion
																										surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
																								#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
																										surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
																								#endif

																								#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
																										surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
																								#endif

																								#ifdef DEBUG_DISPLAY
																										if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
																										{
																											// TODO: need to update mip info
																											surfaceData.metallic = 0;
																										}

																										// We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
																										// as it can modify attribute use for static lighting
																										ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
																								#endif
																									}

																									void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
																									{
																								#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
																										uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
																										LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
																								#endif

																								#ifdef _DOUBLESIDED_ON
																									float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
																								#else
																									float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
																								#endif

																										ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

																										SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
																										SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);

																										// Perform alpha test very early to save performance (a killed pixel will not sample textures)
																										// TODO: split graph evaluation to grab just alpha dependencies first? tricky..
																										// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
																										// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
																										// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
																										// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);

																										// ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);

																										float3 bentNormalWS;
																										BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

																										// Builtin Data
																										// For back lighting we use the oposite vertex normal 
																										InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

																										// override sampleBakedGI:
																										// builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
																										// builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;

																										// builtinData.emissiveColor = surfaceDescription.Emission;

																										// builtinData.depthOffset = surfaceDescription.DepthOffset;

																								#if (SHADERPASS == SHADERPASS_DISTORTION)
																										builtinData.distortion = surfaceDescription.Distortion;
																										builtinData.distortionBlur = surfaceDescription.DistortionBlur;
																								#else
																										builtinData.distortion = float2(0.0, 0.0);
																										builtinData.distortionBlur = 0.0;
																								#endif

																										PostInitBuiltinData(V, posInput, surfaceData, builtinData);
																									}

																									//-------------------------------------------------------------------------------------
																									// Pass Includes
																									//-------------------------------------------------------------------------------------
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassMotionVectors.hlsl"
																									//-------------------------------------------------------------------------------------
																									// End Pass Includes
																									//-------------------------------------------------------------------------------------

																									ENDHLSL
																								}

																								Pass
																								{
																										// based on HDLitPass.template
																										Name "Forward"
																										Tags { "LightMode" = "Forward" }

																										//-------------------------------------------------------------------------------------
																										// Render Modes (Blend, Cull, ZTest, Stencil, etc)
																										//-------------------------------------------------------------------------------------
																										Blend[_SrcBlend][_DstBlend],[_AlphaSrcBlend][_AlphaDstBlend]

																										Cull[_CullModeForward]

																										ZTest[_ZTestDepthEqualForOpaque]

																										ZWrite[_ZWrite]


																										// Stencil setup
																									Stencil
																									{
																									   WriteMask[_StencilWriteMask]
																									   Ref[_StencilRef]
																									   Comp Always
																									   Pass Replace
																									}

																										ColorMask[_ColorMaskTransparentVel] 1

																										//-------------------------------------------------------------------------------------
																										// End Render Modes
																										//-------------------------------------------------------------------------------------

																										HLSLPROGRAM

																										#pragma target 4.5
																										#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
																										//#pragma enable_d3d11_debug_symbols

																										#pragma multi_compile_instancing
																									#pragma instancing_options renderinglayer

																										#pragma multi_compile _ LOD_FADE_CROSSFADE

																										#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
																										#pragma shader_feature_local _DOUBLESIDED_ON
																										#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

																										//-------------------------------------------------------------------------------------
																										// Variant Definitions (active field translations to HDRP defines)
																										//-------------------------------------------------------------------------------------
																										// #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
																										// #define _MATERIAL_FEATURE_TRANSMISSION 1
																										// #define _MATERIAL_FEATURE_ANISOTROPY 1
																										// #define _MATERIAL_FEATURE_IRIDESCENCE 1
																										// #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
																										// #define _ENABLE_FOG_ON_TRANSPARENT 1
																										// #define _AMBIENT_OCCLUSION 1
																										// #define _SPECULAR_OCCLUSION_FROM_AO 1
																										// #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
																										// #define _SPECULAR_OCCLUSION_CUSTOM 1
																										#define _ENERGY_CONSERVING_SPECULAR 1
																										// #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
																										// #define _HAS_REFRACTION 1
																										// #define _REFRACTION_PLANE 1
																										// #define _REFRACTION_SPHERE 1
																										// #define _DISABLE_DECALS 1
																										// #define _DISABLE_SSR 1
																										// #define _ADD_PRECOMPUTED_VELOCITY
																										// #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
																										// #define _DEPTHOFFSET_ON 1
																										// #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

																										//-------------------------------------------------------------------------------------
																										// End Variant Definitions
																										//-------------------------------------------------------------------------------------

																										#pragma vertex Vert
																										#pragma fragment Frag

																										// If we use subsurface scattering, enable output split lighting (for forward pass)
																										#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
																										#define OUTPUT_SPLIT_LIGHTING
																										#endif

																										#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

																										#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"

																										// define FragInputs structure
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

																										//-------------------------------------------------------------------------------------
																										// Defines
																										//-------------------------------------------------------------------------------------
																												#define SHADERPASS SHADERPASS_FORWARD
																											#pragma multi_compile _ DEBUG_DISPLAY
																											#pragma multi_compile _ LIGHTMAP_ON
																											#pragma multi_compile _ DIRLIGHTMAP_COMBINED
																											#pragma multi_compile _ DYNAMICLIGHTMAP_ON
																											#pragma multi_compile _ SHADOWS_SHADOWMASK
																											#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
																											#pragma multi_compile USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
																											#pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH SHADOW_VERY_HIGH
																											// ACTIVE FIELDS:
																											//   Material.Standard
																											//   Specular.EnergyConserving
																											//   SurfaceDescriptionInputs.TangentSpaceNormal
																											//   SurfaceDescription.Albedo
																											//   SurfaceDescription.Normal
																											//   SurfaceDescription.BentNormal
																											//   SurfaceDescription.CoatMask
																											//   SurfaceDescription.Metallic
																											//   SurfaceDescription.Emission
																											//   SurfaceDescription.Smoothness
																											//   SurfaceDescription.Occlusion
																											//   SurfaceDescription.Alpha
																											//   FragInputs.tangentToWorld
																											//   FragInputs.positionRWS
																											//   FragInputs.texCoord1
																											//   FragInputs.texCoord2
																											//   VaryingsMeshToPS.tangentWS
																											//   VaryingsMeshToPS.normalWS
																											//   VaryingsMeshToPS.positionRWS
																											//   VaryingsMeshToPS.texCoord1
																											//   VaryingsMeshToPS.texCoord2
																											//   AttributesMesh.tangentOS
																											//   AttributesMesh.normalOS
																											//   AttributesMesh.positionOS
																											//   AttributesMesh.uv1
																											//   AttributesMesh.uv2

																										// this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
																										#define ATTRIBUTES_NEED_NORMAL
																										#define ATTRIBUTES_NEED_TANGENT
																										// #define ATTRIBUTES_NEED_TEXCOORD0
																										#define ATTRIBUTES_NEED_TEXCOORD1
																										#define ATTRIBUTES_NEED_TEXCOORD2
																										// #define ATTRIBUTES_NEED_TEXCOORD3
																										// #define ATTRIBUTES_NEED_COLOR
																										#define VARYINGS_NEED_POSITION_WS
																										#define VARYINGS_NEED_TANGENT_TO_WORLD
																										// #define VARYINGS_NEED_TEXCOORD0
																										#define VARYINGS_NEED_TEXCOORD1
																										#define VARYINGS_NEED_TEXCOORD2
																										// #define VARYINGS_NEED_TEXCOORD3
																										// #define VARYINGS_NEED_COLOR
																										// #define VARYINGS_NEED_CULLFACE
																										// #define HAVE_MESH_MODIFICATION

																									// We need isFontFace when using double sided
																									#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
																										#define VARYINGS_NEED_CULLFACE
																									#endif

																										//-------------------------------------------------------------------------------------
																										// End Defines
																										//-------------------------------------------------------------------------------------

																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
																									#ifdef DEBUG_DISPLAY
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
																									#endif

																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

																									#if (SHADERPASS == SHADERPASS_FORWARD)
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

																										#define HAS_LIGHTLOOP

																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
																									#else
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
																									#endif

																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
																										#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

																										// Used by SceneSelectionPass
																										int _ObjectId;
																										int _PassValue;

																										//-------------------------------------------------------------------------------------
																										// Interpolator Packing And Struct Declarations
																										//-------------------------------------------------------------------------------------
																									// Generated Type: AttributesMesh
																									struct AttributesMesh {
																										float3 positionOS : POSITION;
																										float3 normalOS : NORMAL; // optional
																										float4 tangentOS : TANGENT; // optional
																										float4 uv1 : TEXCOORD1; // optional
																										float4 uv2 : TEXCOORD2; // optional
																										#if UNITY_ANY_INSTANCING_ENABLED
																										uint instanceID : INSTANCEID_SEMANTIC;
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																									};

																									// Generated Type: VaryingsMeshToPS
																									struct VaryingsMeshToPS {
																										float4 positionCS : SV_Position;
																										float3 positionRWS; // optional
																										float3 normalWS; // optional
																										float4 tangentWS; // optional
																										float4 texCoord1; // optional
																										float4 texCoord2; // optional
																										#if UNITY_ANY_INSTANCING_ENABLED
																										uint instanceID : CUSTOM_INSTANCE_ID;
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																										FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
																										#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																									};
																									struct PackedVaryingsMeshToPS {
																										float3 interp00 : TEXCOORD0; // auto-packed
																										float3 interp01 : TEXCOORD1; // auto-packed
																										float4 interp02 : TEXCOORD2; // auto-packed
																										float4 interp03 : TEXCOORD3; // auto-packed
																										float4 interp04 : TEXCOORD4; // auto-packed
																										float4 positionCS : SV_Position; // unpacked
																										#if UNITY_ANY_INSTANCING_ENABLED
																										uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																										FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
																										#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																									};
																									PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
																									{
																										PackedVaryingsMeshToPS output;
																										output.positionCS = input.positionCS;
																										output.interp00.xyz = input.positionRWS;
																										output.interp01.xyz = input.normalWS;
																										output.interp02.xyzw = input.tangentWS;
																										output.interp03.xyzw = input.texCoord1;
																										output.interp04.xyzw = input.texCoord2;
																										#if UNITY_ANY_INSTANCING_ENABLED
																										output.instanceID = input.instanceID;
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																										output.cullFace = input.cullFace;
																										#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																										return output;
																									}
																									VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
																									{
																										VaryingsMeshToPS output;
																										output.positionCS = input.positionCS;
																										output.positionRWS = input.interp00.xyz;
																										output.normalWS = input.interp01.xyz;
																										output.tangentWS = input.interp02.xyzw;
																										output.texCoord1 = input.interp03.xyzw;
																										output.texCoord2 = input.interp04.xyzw;
																										#if UNITY_ANY_INSTANCING_ENABLED
																										output.instanceID = input.instanceID;
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																										output.cullFace = input.cullFace;
																										#endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
																										return output;
																									}

																									// Generated Type: VaryingsMeshToDS
																									struct VaryingsMeshToDS {
																										float3 positionRWS;
																										float3 normalWS;
																										#if UNITY_ANY_INSTANCING_ENABLED
																										uint instanceID : CUSTOM_INSTANCE_ID;
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																									};
																									struct PackedVaryingsMeshToDS {
																										float3 interp00 : TEXCOORD0; // auto-packed
																										float3 interp01 : TEXCOORD1; // auto-packed
																										#if UNITY_ANY_INSTANCING_ENABLED
																										uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																									};
																									PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
																									{
																										PackedVaryingsMeshToDS output;
																										output.interp00.xyz = input.positionRWS;
																										output.interp01.xyz = input.normalWS;
																										#if UNITY_ANY_INSTANCING_ENABLED
																										output.instanceID = input.instanceID;
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																										return output;
																									}
																									VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
																									{
																										VaryingsMeshToDS output;
																										output.positionRWS = input.interp00.xyz;
																										output.normalWS = input.interp01.xyz;
																										#if UNITY_ANY_INSTANCING_ENABLED
																										output.instanceID = input.instanceID;
																										#endif // UNITY_ANY_INSTANCING_ENABLED
																										return output;
																									}

																									//-------------------------------------------------------------------------------------
																									// End Interpolator Packing And Struct Declarations
																									//-------------------------------------------------------------------------------------

																									//-------------------------------------------------------------------------------------
																									// Graph generated code
																									//-------------------------------------------------------------------------------------
																											// Shared Graph Properties (uniform inputs)
																											CBUFFER_START(UnityPerMaterial)
																											float4 _EmissionColor;
																											float _RenderQueueType;
																											float _StencilRef;
																											float _StencilWriteMask;
																											float _StencilRefDepth;
																											float _StencilWriteMaskDepth;
																											float _StencilRefMV;
																											float _StencilWriteMaskMV;
																											float _StencilRefDistortionVec;
																											float _StencilWriteMaskDistortionVec;
																											float _StencilWriteMaskGBuffer;
																											float _StencilRefGBuffer;
																											float _ZTestGBuffer;
																											float _RequireSplitLighting;
																											float _ReceivesSSR;
																											float _SurfaceType;
																											float _BlendMode;
																											float _SrcBlend;
																											float _DstBlend;
																											float _AlphaSrcBlend;
																											float _AlphaDstBlend;
																											float _ZWrite;
																											float _CullMode;
																											float _TransparentSortPriority;
																											float _CullModeForward;
																											float _TransparentCullMode;
																											float _ZTestDepthEqualForOpaque;
																											float _ZTestTransparent;
																											float _TransparentBackfaceEnable;
																											float _AlphaCutoffEnable;
																											float _AlphaCutoff;
																											float _UseShadowThreshold;
																											float _DoubleSidedEnable;
																											float _DoubleSidedNormalMode;
																											float4 _DoubleSidedConstants;
																											CBUFFER_END


																												// Pixel Graph Inputs
																													struct SurfaceDescriptionInputs {
																														float3 TangentSpaceNormal; // optional
																													};
																											// Pixel Graph Outputs
																												struct SurfaceDescription
																												{
																													float3 Albedo;
																													float3 Normal;
																													float3 BentNormal;
																													float CoatMask;
																													float Metallic;
																													float3 Emission;
																													float Smoothness;
																													float Occlusion;
																													float Alpha;
																												};

																												// Shared Graph Node Functions
																												// Pixel Graph Evaluation
																													SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
																													{
																														SurfaceDescription surface = (SurfaceDescription)0;
																														surface.Albedo = IsGammaSpace() ? float3(0.7353569, 0.7353569, 0.7353569) : SRGBToLinear(float3(0.7353569, 0.7353569, 0.7353569));
																														surface.Normal = IN.TangentSpaceNormal;
																														surface.BentNormal = IN.TangentSpaceNormal;
																														surface.CoatMask = 0;
																														surface.Metallic = 0;
																														surface.Emission = float3(0, 0, 0);
																														surface.Smoothness = 0.5;
																														surface.Occlusion = 1;
																														surface.Alpha = 1;
																														return surface;
																													}

																													//-------------------------------------------------------------------------------------
																													// End graph generated code
																													//-------------------------------------------------------------------------------------

																												// $include("VertexAnimation.template.hlsl")


																												//-------------------------------------------------------------------------------------
																												// TEMPLATE INCLUDE : SharedCode.template.hlsl
																												//-------------------------------------------------------------------------------------
																													FragInputs BuildFragInputs(VaryingsMeshToPS input)
																													{
																														FragInputs output;
																														ZERO_INITIALIZE(FragInputs, output);

																														// Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
																														// TODO: this is a really poor workaround, but the variable is used in a bunch of places
																														// to compute normals which are then passed on elsewhere to compute other values...
																														output.tangentToWorld = k_identity3x3;
																														output.positionSS = input.positionCS;       // input.positionCS is SV_Position

																														output.positionRWS = input.positionRWS;
																														output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
																														// output.texCoord0 = input.texCoord0;
																														output.texCoord1 = input.texCoord1;
																														output.texCoord2 = input.texCoord2;
																														// output.texCoord3 = input.texCoord3;
																														// output.color = input.color;
																														#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
																														output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
																														#elif SHADER_STAGE_FRAGMENT
																														// output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
																														#endif // SHADER_STAGE_FRAGMENT

																														return output;
																													}

																													SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
																													{
																														SurfaceDescriptionInputs output;
																														ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

																														// output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
																														// output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
																														// output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
																														output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
																														// output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
																														// output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
																														// output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
																														// output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
																														// output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
																														// output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
																														// output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
																														// output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
																														// output.WorldSpaceViewDirection =     normalize(viewWS);
																														// output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
																														// output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
																														// float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
																														// output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
																														// output.WorldSpacePosition =          GetAbsolutePositionWS(input.positionRWS);
																														// output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
																														// output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
																														// output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
																														// output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
																														// output.uv0 =                         input.texCoord0;
																														// output.uv1 =                         input.texCoord1;
																														// output.uv2 =                         input.texCoord2;
																														// output.uv3 =                         input.texCoord3;
																														// output.VertexColor =                 input.color;
																														// output.FaceSign =                    input.isFrontFace;
																														// output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value

																														return output;
																													}

																													// existing HDRP code uses the combined function to go directly from packed to frag inputs
																													FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
																													{
																														UNITY_SETUP_INSTANCE_ID(input);
																														VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
																														return BuildFragInputs(unpacked);
																													}

																													//-------------------------------------------------------------------------------------
																													// END TEMPLATE INCLUDE : SharedCode.template.hlsl
																													//-------------------------------------------------------------------------------------


																														void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
																														{
																															// setup defaults -- these are used if the graph doesn't output a value
																															ZERO_INITIALIZE(SurfaceData, surfaceData);

																															// specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
																															// however specularOcclusion can come from the graph, so need to be init here so it can be override.
																															surfaceData.specularOcclusion = 1.0;

																															// copy across graph values, if defined
																															surfaceData.baseColor = surfaceDescription.Albedo;
																															surfaceData.perceptualSmoothness = surfaceDescription.Smoothness;
																															surfaceData.ambientOcclusion = surfaceDescription.Occlusion;
																															// surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
																															surfaceData.metallic = surfaceDescription.Metallic;
																															// surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
																															// surfaceData.thickness =                 surfaceDescription.Thickness;
																															// surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
																															// surfaceData.specularColor =             surfaceDescription.Specular;
																															surfaceData.coatMask = surfaceDescription.CoatMask;
																															// surfaceData.anisotropy =                surfaceDescription.Anisotropy;
																															// surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
																															// surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;

																													#ifdef _HAS_REFRACTION
																															if (_EnableSSRefraction)
																															{
																																// surfaceData.ior =                       surfaceDescription.RefractionIndex;
																																// surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
																																// surfaceData.atDistance =                surfaceDescription.RefractionDistance;

																																surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
																																surfaceDescription.Alpha = 1.0;
																															}
																															else
																															{
																																surfaceData.ior = 1.0;
																																surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																																surfaceData.atDistance = 1.0;
																																surfaceData.transmittanceMask = 0.0;
																																surfaceDescription.Alpha = 1.0;
																															}
																													#else
																															surfaceData.ior = 1.0;
																															surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
																															surfaceData.atDistance = 1.0;
																															surfaceData.transmittanceMask = 0.0;
																													#endif

																															// These static material feature allow compile time optimization
																															surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
																													#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
																															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
																													#endif
																													#ifdef _MATERIAL_FEATURE_TRANSMISSION
																															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
																													#endif
																													#ifdef _MATERIAL_FEATURE_ANISOTROPY
																															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
																													#endif
																															// surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;

																													#ifdef _MATERIAL_FEATURE_IRIDESCENCE
																															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
																													#endif
																													#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
																															surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
																													#endif

																													#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
																															// Require to have setup baseColor
																															// Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
																															surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
																													#endif

																													#ifdef _DOUBLESIDED_ON
																														float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
																													#else
																														float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
																													#endif

																														// tangent-space normal
																														float3 normalTS = float3(0.0f, 0.0f, 1.0f);
																														normalTS = surfaceDescription.Normal;

																														// compute world space normal
																														GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

																														surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

																														surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
																														// surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);

																												#if HAVE_DECALS
																														if (_EnableDecals)
																														{
																															// Both uses and modifies 'surfaceData.normalWS'.
																															DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
																															ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
																														}
																												#endif

																														bentNormalWS = surfaceData.normalWS;
																														// GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);

																														surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);


																														// By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
																														// If user provide bent normal then we process a better term
																												#if defined(_SPECULAR_OCCLUSION_CUSTOM)
																														// Just use the value passed through via the slot (not active otherwise)
																												#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
																														// If we have bent normal and ambient occlusion, process a specular occlusion
																														surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
																												#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
																														surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
																												#endif

																												#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
																														surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
																												#endif

																												#ifdef DEBUG_DISPLAY
																														if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
																														{
																															// TODO: need to update mip info
																															surfaceData.metallic = 0;
																														}

																														// We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
																														// as it can modify attribute use for static lighting
																														ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
																												#endif
																													}

																													void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
																													{
																												#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
																														uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
																														LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
																												#endif

																												#ifdef _DOUBLESIDED_ON
																													float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
																												#else
																													float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
																												#endif

																														ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

																														SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
																														SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);

																														// Perform alpha test very early to save performance (a killed pixel will not sample textures)
																														// TODO: split graph evaluation to grab just alpha dependencies first? tricky..
																														// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
																														// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
																														// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
																														// DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);

																														// ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);

																														float3 bentNormalWS;
																														BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);

																														// Builtin Data
																														// For back lighting we use the oposite vertex normal 
																														InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

																														// override sampleBakedGI:
																														// builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
																														// builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;

																														builtinData.emissiveColor = surfaceDescription.Emission;

																														// builtinData.depthOffset = surfaceDescription.DepthOffset;

																												#if (SHADERPASS == SHADERPASS_DISTORTION)
																														builtinData.distortion = surfaceDescription.Distortion;
																														builtinData.distortionBlur = surfaceDescription.DistortionBlur;
																												#else
																														builtinData.distortion = float2(0.0, 0.0);
																														builtinData.distortionBlur = 0.0;
																												#endif

																														PostInitBuiltinData(V, posInput, surfaceData, builtinData);
																													}

																													//-------------------------------------------------------------------------------------
																													// Pass Includes
																													//-------------------------------------------------------------------------------------
																														#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassForward.hlsl"
																													//-------------------------------------------------------------------------------------
																													// End Pass Includes
																													//-------------------------------------------------------------------------------------

																													ENDHLSL
																												}

	}
		CustomEditor "UnityEditor.Experimental.Rendering.HDPipeline.HDLitGUI"
																														FallBack "Hidden/InternalErrorShader"
}