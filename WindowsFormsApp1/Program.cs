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
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			
			DX.ChangeWindowMode(1);
			DX.DxLib_Init();
			DX.SetDrawScreen(DX.DX_SCREEN_BACK);

			var input = new DXInterFace.IFs();
			input.Add("up", new DXInterFace(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_UP));
			input.Add("down", new DXInterFace(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_DOWN));
			input.Add("left", new DXInterFace(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_LEFT));
			input.Add("right", new DXInterFace(DXInterFace.DXIFType.KeyBoard, DX.KEY_INPUT_RIGHT));
			input.Add("click", new DXInterFace(DXInterFace.DXIFType.Mouse, DX.MOUSE_INPUT_LEFT));

			int x = 0, y = 0;


			while (DX.ProcessMessage() != -1)
			{
				DX.ClearDrawScreen();
				input.Update();

				if (input["up"].IsPressing) --y; 
				if (input["down"].IsPressing) ++y; 
				if (input["left"].IsPressing) --x; 
				if (input["right"].IsPressing) ++x;

				if (input["up"].WasPressed) x += 20;
				if (input["click"].IsPressed) y += 100;

				DX.DrawString(x, y, input["up"].Frame.ToString(), DX.GetColor(255, 0, 0));

				DX.ScreenFlip();
			}

			DX.DxLib_End();
		}
	}
}
