【ItemDropスクリプトの導入】
①「Item.zip」を解凍
②中にある「Prefabs」フォルダをUnityのAssetsに入れる
③「Health.cs」に下のコードを追加
④Item Dropにドロップさせたいオブジェクトを追加

【『Health.cs』の追加】
・グローバル変数(48行目あたりに)

	//	★追加
	public GameObject itemDrop;
	//	★

・Die関数（"最後尾"に記入）

	//	★追加（一番下に書かないと以降の処理が動作しない）
	Instantiate(itemDrop, transform.position, Quaternion.identity);
	//	★
