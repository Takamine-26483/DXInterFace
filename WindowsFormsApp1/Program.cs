using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using TakamineProduction;


namespace WindowsFormsApp1
{
	static class Program
	{
		
		const string BTN_NEXT = "next";
		const string BTN_KETTEI = "kettei";
		const string BTN_UP = "up";
		const string BTN_DOWN = "down";
		const string BTN_LEFT = "left";
		const string BTN_RIGHT = "right";

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			DX.SetGraphMode(800, 400, 32);
			DX.ChangeWindowMode(1);
			DX.DxLib_Init();
			DX.SetDrawScreen(DX.DX_SCREEN_BACK);



			//★　DXInterFaceを管理するクラス(Dictionaryとほぼ同じもの)のインスタンスを作成
			var input = new DXInterFace.DXController();



			//★　インターフェースを登録する。キー(string)と入力情報(インターフェースの種類とそのID)
			input.Add(BTN_NEXT, DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_ESCAPE);
			input.Add(BTN_KETTEI, DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_Z);
			input.Add(BTN_UP, new DXInterFace(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_UP));
			input.Add(BTN_DOWN, new DXInterFace(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_DOWN));
			input.Add(BTN_LEFT, new DXInterFace(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_LEFT));
			input.Add(BTN_RIGHT, new DXInterFace(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_RIGHT));

			//★　テスト１
			Loop(input, "Test1:決定ボタン＝Zキー　移動＝矢印キー　次のテスト＝エスケープキー");



			//★　既存インターフェースを再設定する。
			input[BTN_NEXT].Reset(DXInterFace.DXIFType.Mouse, DX.MOUSE_INPUT_LEFT);
			input[BTN_KETTEI].Reset(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_TAB);
			input[BTN_UP].Reset(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_W);
			input[BTN_DOWN].Reset(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_S);
			input[BTN_LEFT].Reset(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_A);
			input[BTN_RIGHT].Reset(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_D);

			//★　テスト２
			Loop(input, "Test2:決定ボタン＝TABキー　移動＝WASDキー　次のテスト＝左クリック");



			//★　既存インターフェースを再設定する。
			input[BTN_NEXT].Reset(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_1);
			input[BTN_KETTEI].Reset(DXInterFace.DXIFType.Mouse, DX.MOUSE_INPUT_MIDDLE);
			input[BTN_LEFT].Reset(DXInterFace.DXIFType.Mouse, DX.MOUSE_INPUT_LEFT);
			input[BTN_RIGHT].Reset(DXInterFace.DXIFType.Mouse, DX.MOUSE_INPUT_RIGHT);

			//★　テスト３
			Loop(input, "Test3:決定ボタン＝マウスホイール　移動＝左右クリック＋SDキー　終了＝1");



			DX.DxLib_End();
		}


		static void Loop(DXInterFace.DXController input, string mes)
		{
			int x = 0, y = 40;

			do
			{
				DX.ClearDrawScreen();
				input.Update();

				if (input[BTN_UP].IsPressing) --y;
				if (input[BTN_DOWN].IsPressing) ++y;
				if (input[BTN_LEFT].IsPressing) --x;
				if (input[BTN_RIGHT].IsPressing) ++x;

				DX.DrawString(0, 0, mes, DX.GetColor(255, 170, 170));
				DX.DrawString(x, y, "[BTN_KETTEI].Frame(押下フレーム数) = " + input[BTN_KETTEI].Frame, DX.GetColor(255, 170, 170));
				DX.DrawString(x, y + 20, "[BTN_KETTEI].IsPressed(押下フレーム数が1) = " + input[BTN_KETTEI].IsPressed, DX.GetColor(255, 170, 170));
				DX.DrawString(x, y + 40, "[BTN_KETTEI].IsPressing(押下フレーム数が1以上) = " + input[BTN_KETTEI].IsPressing, DX.GetColor(255, 170, 170));
				DX.DrawString(x, y + 60, "[BTN_KETTEI].WasPressed(押下フレーム数が0に戻った最初のフレーム) = " + input[BTN_KETTEI].WasPressed, DX.GetColor(255, 170, 170));

				DX.DrawString(input.MX, input.MY, input.MX + "," + input.MY, DX.GetColor(255, 170, 170));


				DX.ScreenFlip();
			}
			while (DX.ProcessMessage() != -1 && !input[BTN_NEXT].IsPressed);
		}
	}
}
