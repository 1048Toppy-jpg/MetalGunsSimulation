//		グループ名/シェーダー名
Shader "Custom/Enemy"
{
	//ここで宣言したものが表示される
	Properties
	{
		//例…プロパティ変数名("インスペクター表示名",変数の型)="初期値"
		//※Unityのデフォルトで_(アンダースコア)と大文字から始める

		//本体の色
		_BaseColor("Base Color", Color) = (0,0,0,1)

		//輪郭の色
		_RimColor("Rim Color", Color) = (1,1,1,1)

		//RimColorの範囲
		_RimRange("Rim Range",Float) = 2.5

		//透過度			//↓最小値
		_Alpha("Alpha", Range(0.0, 1.0)) = 1.0
		// ↑最大値
	}


	//SubShader
	SubShader
	{
		//描画順番
		//([Background]→[Geometry]→[AlphaTest]→[Transparent]→[Overlay])
		Tags {"Queue" = "Transparent" }
		LOD 200

		//シェーダープログラムの開始位置
		CGPROGRAM

		//関数宣言
		//#pragma surface 関数名 ライティングモデルオプション(デフォルトでfullforwardshadows)
		//ライティングモデルオプションにalpha:fadeを指定することで、半透明で描くことが可
		#pragma surface surf Standard alpha:fade

		//シェーダーモデル
		#pragma target 3.0

		//Input構造体
		struct Input
		{
			float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
		};

		//変数宣言
		//数値の方はfloat,helf,fixedの三種類があり、
		//ワールド空間とテクスチャの座標はfloat,色はfixed,それ以外はhalf
		//型の語尾の数字は含まれる数値の数
		fixed4 _BaseColor;
		fixed4 _RimColor;
		float _RimRange;
		float _Alpha;

		//Surf関数
		//surfの中の「Input IN」はInput構造体から渡ってきた情報を読み込んでいる
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _BaseColor.rgb;
			o.Alpha = _Alpha;

			float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
													//↓高いほど輪郭範囲が小さくなる
			o.Emission = _RimColor.rgb * pow(rim, _RimRange);
		}

		//シェーダープログラムの終了位置
		ENDCG
		}
	//滑り止めシェーダー名
	FallBack "Diffuse"
}
