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

		static private int MouseIn { get; set; }
		static private byte[] KeyIn { get; set; } = new byte[256];


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



		static private void InputUpdate()
		{
			MouseIn = DX.GetMouseInput();
			DX.GetHitKeyStateAll(KeyIn);
		}

		private void Update()
		{
			switch (IFType)
			{
				case DXIFType.KeyBoard:
					IFUpdate(KeyIn[IFID] == 1);
					break;
				case DXIFType.Mouse:
					IFUpdate((MouseIn & IFID) != 0);
					break;
			}

			void IFUpdate(bool pressed)
			{
				if (pressed)
				{
					if (Frame < int.MaxValue)
						Frame++;
				}
				else
				{
					WasPressed = (Frame > 0);
					Frame = 0;
				}

				IsPressed = (Frame == 1);
				IsPressing = (Frame >= 1);
			}
		}

		/// <summary>DXInterFaceをまとめて管理するDictionaryクラス</summary>
		public class DXController : Dictionary<string, DXInterFace>
		{
			/// <summary>前回のUpdateから比較したホイールの回転量を表す（奥：正数　手前：負数）</summary>
			public int Wheel { get; private set; }
			/// <summary>Update時点でのマウスのX座標</summary>
			public int MX { get; private set; }
			/// <summary>Update時点でのマウスのY座標</summary>
			public int MY { get; private set; }


			/// <summary>インターフェースを追加する</summary>
			/// <param name="key">追加する要素のキー</param>
			/// <param name="inputType">インターフェースの種類</param>
			/// <param name="inputID">インターフェースの入力のID</param>
			public void Add(string key, DXIFType inputType, int inputID)
			{
				Add(key, new DXInterFace(inputType, inputID));
			}

			/// <summary>全ての入力を更新する。毎フレーム実行してください</summary>
			public void Update()
			{
				Wheel = DX.GetMouseWheelRotVol();

				DX.GetMousePoint(out int x, out int y);
				MX = x;
				MY = y;

				InputUpdate();

				foreach (var i in Values)
					i.Update();
			}
		}

		/// <summary>インターフェースの種類</summary>
		public enum DXIFType
		{
			/// <summary>マウス</summary>
			Mouse,
			/// <summary>キーボード</summary>
			KeyBoard
		}
	}
}
