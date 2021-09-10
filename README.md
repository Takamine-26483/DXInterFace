# DXInterFace
（DXライブラリ専用）入力インターフェースを統合し、自由に切り替え可能な入力クラス。内部クラスのDXControllerからご利用ください。

## サンプル
WindowsFormsApp1にあります。
下記要約。
```
//★　DXInterFaceを管理するクラス(Dictionaryとほぼ同じもの)のインスタンスを作成
var input = new DXInterFace.DXController();

//★　インターフェースを登録する。キー(string)と入力情報(インターフェースの種類とそのID)
input.Add("Button_A", DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_Z);       //Button_Aという名前にキーボードのZキーを割り当て
input.Add("Button_B", DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_X);       //Button_Bという名前にキーボードのXキーを割り当て
input.Add("Button_Fire", DXInterFace.DXIFType.Mouse, DX.MOUSE_INPUT_LEFT);  //Button_Fireという名前にマウスの左クリックを割り当て

//★　入力状態を更新する。（フレームごとに毎回実行する）
input.Update();

//★　下記で入力状態を取得する。
//input["Button_A"].Frame       //Button_Aの押下フレーム数。押下されると加算され、離されると0に戻る
//input["Button_A"].IsPressed   //Button_Aの押下フレーム数 == 1
//input["Button_A"].IsPressing  //Button_Aの押下フレーム数 >= 1
//input["Button_A"].WasPressed  //Button_Aの押下フレーム数が0に戻った最初のフレームである

//★　既存インターフェースを再設定する。
input["Button_A"].Reset(DXInterFace.DXIFType.Mouse, DX.MOUSE_INPUT_MIDDLE);   //Button_Aの入力をZキー　→　マウスホイールボタン　へ変更
```
