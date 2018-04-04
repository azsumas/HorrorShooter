// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "gt_material"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_UVTiling("UV Tiling", Int) = 1
		_BaseColor("Base Color", 2D) = "white" {}
		_Metallic("Metallic", 2D) = "white" {}
		_Roughness("Roughness", 2D) = "white" {}
		_RoughnessMultiplier("Roughness Multiplier", Range( 0 , 1)) = 1
		[Toggle]_InvertRoughness("Invert Roughness", Int) = 0
		_AmbientOcclusion("Ambient Occlusion", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		[Toggle]_FlipNormalDirection("Flip Normal Direction", Int) = 0
		_Height("Height", 2D) = "white" {}
		_POMScale("POM Scale", Range( -1 , 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#pragma shader_feature _FLIPNORMALDIRECTION_ON
		#pragma shader_feature _INVERTROUGHNESS_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 texcoord_0;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Normal;
		uniform int _UVTiling;
		uniform sampler2D _Height;
		uniform float _POMScale;
		uniform float4 _Height_ST;
		uniform sampler2D _BaseColor;
		uniform sampler2D _Metallic;
		uniform float _RoughnessMultiplier;
		uniform sampler2D _Roughness;
		uniform sampler2D _AmbientOcclusion;


		inline float2 POM( sampler2D heightMap, float2 uvs, float2 dx, float2 dy, float3 normalWorld, float3 viewWorld, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv )
		{
			float3 result = 0;
			int stepIndex = 0;
			int numSteps = ( int )lerp( maxSamples, minSamples, dot( normalWorld, viewWorld ) );
			float layerHeight = 1.0 / numSteps;
			float2 plane = parallax * ( viewDirTan.xy / viewDirTan.z );
			uvs += refPlane * plane;
			float2 deltaTex = -plane * layerHeight;
			float2 prevTexOffset = 0;
			float prevRayZ = 1.0f;
			float prevHeight = 0.0f;
			float2 currTexOffset = deltaTex;
			float currRayZ = 1.0f - layerHeight;
			float currHeight = 0.0f;
			float intersection = 0;
			float2 finalTexOffset = 0;
			while ( stepIndex < numSteps + 1 )
			{
				currHeight = tex2Dgrad( heightMap, uvs + currTexOffset, dx, dy ).r;
				if ( currHeight > currRayZ )
				{
					stepIndex = numSteps + 1;
				}
				else
				{
					stepIndex++;
					prevTexOffset = currTexOffset;
					prevRayZ = currRayZ;
					prevHeight = currHeight;
					currTexOffset += deltaTex;
					currRayZ -= layerHeight;
				}
			}
			int sectionSteps = 5;
			int sectionIndex = 0;
			float newZ = 0;
			float newHeight = 0;
			while ( sectionIndex < sectionSteps )
			{
				intersection = ( prevHeight - prevRayZ ) / ( prevHeight - currHeight + currRayZ - prevRayZ );
				finalTexOffset = prevTexOffset + intersection * deltaTex;
				newZ = prevRayZ - intersection * layerHeight;
				newHeight = tex2Dgrad( heightMap, uvs + finalTexOffset, dx, dy ).r;
				if ( newHeight > newZ )
				{
					currTexOffset = finalTexOffset;
					currHeight = newHeight;
					currRayZ = newZ;
					deltaTex = intersection * deltaTex;
					layerHeight = intersection * layerHeight;
				}
				else
				{
					prevTexOffset = finalTexOffset;
					prevHeight = newHeight;
					prevRayZ = newZ;
					deltaTex = ( 1 - intersection ) * deltaTex;
					layerHeight = ( 1 - intersection ) * layerHeight;
				}
				sectionIndex++;
			}
			return uvs + finalTexOffset;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 temp_cast_0 = _UVTiling;
			o.texcoord_0.xy = v.texcoord.xy * temp_cast_0 + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float2 OffsetPOM55 = POM( _Height, i.texcoord_0, ddx(i.texcoord_0), ddx(i.texcoord_0), ase_worldNormal, worldViewDir, worldViewDir, 8, 16, ( _POMScale / 10.0 ), 0, _Height_ST.xy, float2(0,0) );
			float2 myVarName0 = OffsetPOM55;
			float2 temp_output_59_0 = ddx( i.texcoord_0 );
			float2 temp_output_60_0 = ddy( i.texcoord_0 );
			float3 tex2DNode52 = UnpackNormal( tex2D( _Normal, myVarName0, temp_output_59_0, temp_output_60_0 ) );
			#ifdef _FLIPNORMALDIRECTION_ON
			float3 staticSwitch67 = ( tex2DNode52 * float3(1,-1,1) );
			#else
			float3 staticSwitch67 = tex2DNode52;
			#endif
			o.Normal = staticSwitch67;
			o.Albedo = tex2D( _BaseColor, myVarName0, temp_output_59_0, temp_output_60_0 ).xyz;
			o.Metallic = tex2D( _Metallic, myVarName0, float2( 0,0 ), float2( 0,0 ) ).x;
			float4 temp_output_53_0 = ( _RoughnessMultiplier * tex2D( _Roughness, myVarName0, temp_output_59_0, temp_output_60_0 ) );
			#ifdef _INVERTROUGHNESS_ON
			float4 staticSwitch68 = ( 1.0 - temp_output_53_0 );
			#else
			float4 staticSwitch68 = temp_output_53_0;
			#endif
			o.Smoothness = staticSwitch68.x;
			o.Occlusion = tex2D( _AmbientOcclusion, myVarName0, temp_output_59_0, temp_output_60_0 ).x;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=11001
3018;352;1342;824;2710.696;59.9045;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;57;-2379.444,352.9988;Float;False;Property;_POMScale;POM Scale;10;0;0;-1;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;66;-2236.893,420.8993;Float;False;Constant;_Float0;Float 0;15;0;10;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.IntNode;7;-2184.8,-16.20081;Float;False;Property;_UVTiling;UV Tiling;0;0;1;0;1;INT
Node;AmplifyShaderEditor.TexturePropertyNode;64;-2271.588,555.598;Float;True;Property;_Height;Height;9;0;None;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1943.192,-39.40097;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;56;-2309.044,170.8987;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;65;-2067.891,359.799;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;55;-1868.337,291.7998;Float;False;0;8;16;5;0.02;0;False;1,1;False;0,0;6;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.DdxOpNode;59;-1537.427,-45.09937;Float;False;1;0;FLOAT2;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;-1559.432,206.0993;Float;False;myVarName0;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.DdyOpNode;60;-1543.127,40.40038;Float;False;1;0;FLOAT2;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;54;-720.0359,357.1988;Float;False;Property;_RoughnessMultiplier;Roughness Multiplier;4;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;4;-1020.594,291.9976;Float;True;Property;_Roughness;Roughness;3;0;None;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;49;-1043.827,-128.2012;Float;False;Constant;_Vector0;Vector 0;11;0;1,-1,1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-417.6367,350.7985;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;52;-1223.633,-536.902;Float;True;Property;_Normal;Normal;7;0;None;True;0;True;bump;Auto;True;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-980.1271,-247.8012;Float;False;2;2;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.OneMinusNode;28;-416.6129,245.1956;Float;False;1;0;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;1;-1222.699,-746.9019;Float;True;Property;_BaseColor;Base Color;1;0;None;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;5;-1031.996,537.3986;Float;True;Property;_AmbientOcclusion;Ambient Occlusion;6;0;None;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-1015.296,83.99842;Float;True;Property;_Metallic;Metallic;2;0;None;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StaticSwitch;68;-256.9893,337.5959;Float;False;Property;_InvertRoughness;Invert Roughness;5;0;0;False;2;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.StaticSwitch;67;-707.8906,-366.7005;Float;False;Property;_FlipNormalDirection;Flip Normal Direction;8;0;0;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;53,-2;Float;False;True;6;Float;ASEMaterialInspector;0;Standard;gt_material;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;2;2;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;7;0
WireConnection;65;0;57;0
WireConnection;65;1;66;0
WireConnection;55;0;6;0
WireConnection;55;1;64;0
WireConnection;55;2;65;0
WireConnection;55;3;56;0
WireConnection;59;0;6;0
WireConnection;58;0;55;0
WireConnection;60;0;6;0
WireConnection;4;1;58;0
WireConnection;4;3;59;0
WireConnection;4;4;60;0
WireConnection;53;0;54;0
WireConnection;53;1;4;0
WireConnection;52;1;58;0
WireConnection;52;3;59;0
WireConnection;52;4;60;0
WireConnection;50;0;52;0
WireConnection;50;1;49;0
WireConnection;28;0;53;0
WireConnection;1;1;58;0
WireConnection;1;3;59;0
WireConnection;1;4;60;0
WireConnection;5;1;58;0
WireConnection;5;3;59;0
WireConnection;5;4;60;0
WireConnection;3;1;58;0
WireConnection;68;0;28;0
WireConnection;68;1;53;0
WireConnection;67;0;50;0
WireConnection;67;1;52;0
WireConnection;0;0;1;0
WireConnection;0;1;67;0
WireConnection;0;3;3;0
WireConnection;0;4;68;0
WireConnection;0;5;5;0
ASEEND*/
//CHKSM=4777F3BA8D0D0EAD4F55CF8EC23DB2326F1B6F31