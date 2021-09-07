using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TakamineProduction
{
	/// <summary>入力インターフェースを統合し、自由に切り替え可能な入力クラス。内部クラスのIFsからご利用ください</summary>
	public class DXInterFace
	{
		/// <summary>インターフェースの種類</summary>
		public DXIFType IFType { get; private set; }
		/// <summary>インターフェースの入力のID</summary>
		public int IFID { get; private set; }
		/// <summary>入力の押下フレーム数。押し続けると増え続け、離すと0に戻る。intの最大値まで計測する</summary>
		public int Frame { get; private set; }
		/// <summary>入力の押下フレーム数が1であることを表す</summary>
		public bool IsPressed { get; private set; }
		/// <summary>入力の押下フレーム数が1以上であることを表す</summary>
		public bool IsPressing { get; private set; }
		/// <summary>入力の押下フレーム数が1以上から0に戻った最初のフレームであることを表す。</summary>
		public bool WasPressed { get; private set; }


		/// <summary>コンストラクタ</summary>
		/// <param name="inputType">インターフェースの種類</param>
		/// <param name="inputID">インターフェースの入力のID</param>
		public DXInterFace(DXIFType inputType, int inputID)
		{
			Reset(inputType, inputID);
		}

		/// <summary>インターフェースを再設定する</summary>
		/// <param name="inputType">インターフェースの種類</param>
		/// <param name="inputID">インターフェースの入力のID</param>
		public void Reset(DXIFType inputType, int inputID)
		{
			IFType = inputType;
			IFID = inputID;
			IsPressed = IsPressing = WasPressed = false;
			Frame = 0;
		}


		/// <summary>DXInterFaceをまとめて管理するDictionaryクラス</summary>
		public class IFs : Dictionary<string, DXInterFace>
		{
			/// <summary>前回のUpdateから比較したホイールの回転量を表す（奥：正数　手前：負数）</summary>
			public int WheelVol { get; private set; }
			/// <summary>Update時点でのマウスのX座標</summary>
			public int MX { get; private set; }
			/// <summary>Update時点でのマウスのY座標</summary>
			public int MY { get; private set; }


			/// <summary>全ての入力を更新する。毎フレーム実行してください</summary>
			public void Update()
			{
				WheelVol = DX.GetMouseWheelRotVol();

				DX.GetMousePoint(out int x, out int y);
				MX = x;
				MY = y;

				int mouseIn = DX.GetMouseInput();
				byte[] keyIn = new byte[256];
				DX.GetHitKeyStateAll(keyIn);

				foreach (var i in Values)
				{
					switch (i.IFType)
					{
						case DXIFType.KeyBoard:
							InputUpdate(i, keyIn[i.IFID] == 1);
							break;
						case DXIFType.Mouse:
							InputUpdate(i, (mouseIn & i.IFID) != 0);
							break;
					}
				}

				//**********************************************************************************

				void InputUpdate(DXInterFace i,bool pressed)
				{
					if (pressed)
					{
						if (i.Frame < int.MaxValue)
							i.Frame++;
					}
					else
					{
						i.WasPressed = (i.Frame > 0);
						i.Frame = 0;
					}

					i.IsPressed = (i.Frame == 1);
					i.IsPressing = (i.Frame >= 1);
				}
			}
		}

		public enum DXIFType
		{
			Mouse,
			KeyBoard
		}
	}
}
