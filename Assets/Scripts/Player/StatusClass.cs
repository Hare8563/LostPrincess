using UnityEngine;
using System.Collections;

namespace StatusClass{
	public class Status{
		private int alpha = 2;
		public int ExpLimit = 10;
		public int LEV;//Level
		public int  EXP;//Experience value
		public int HP;//HO
		public int MP;
		public int BOW_POW;
		public int Sword_Power;
		public int Magic_Power;
		public string NAME;
		
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
		public Status(){
			LEV = 1;
			EXP = 0;
			HP = 10;
			MP = 10;
			BOW_POW = 3;
			Sword_Power=10;
			Magic_Power = 8;
			NAME = "NONAME";
		}

        /// <summary>
        /// ステータス指定コンストラクタ
        /// </summary>
        /// <param name="Lev">レベル</param>
        /// <param name="Exp">取得経験値</param>
        /// <param name="Hp">ヒットポイント</param>
        /// <param name="Mp">マジックポイント</param>
		public Status(int Lev, int Exp, int Hp, int Mp){
            LEV = Lev;
            EXP = Exp;
            HP = Hp;
            MP = Mp;
            BOW_POW = (int)(1.0f * Mathf.Log10((float)Lev)) + alpha;
            Sword_Power = (int)(2.0f * Mathf.Log10((float)Lev)) + alpha;
            Magic_Power = (int)(2.0f * Mathf.Log10((float)Lev)) + alpha;
			NAME = "NONAME";
        }

		/// <summary>
		/// ステータス+名前指定コンストラクタ
		/// </summary>
		/// <param name="Lev">レベル</param>
		/// <param name="Exp">取得経験値</param>
		/// <param name="Hp">ヒットポイント</param>
		/// <param name="Mp">マジックポイント</param>
		public Status(int Lev, int Exp, int Hp, int Mp, string Name){
			LEV = Lev;
			EXP = Exp;
			HP = Hp;
			MP = Mp;
			BOW_POW = (int)(1.0f * Mathf.Log10((float)Lev)) + alpha;
			Sword_Power = (int)(2.0f * Mathf.Log10((float)Lev)) + alpha;
			Magic_Power = (int)(2.0f * Mathf.Log10((float)Lev)) + alpha;
			NAME = Name;
		}
		
        /// <summary>
        /// レベルアップ
        /// </summary>
		public void LevUp(){
            System.Random rand = new System.Random();
            alpha = rand.Next(0, 5);
            this.LEV++;
            this.EXP = 0;
            this.Sword_Power = (int)(2.0f * Mathf.Log10((float)this.LEV)) + alpha;
            this.Magic_Power = (int)(2.0f * Mathf.Log10((float)this.LEV)) + alpha;
            this.BOW_POW = (int)(1.0f * Mathf.Log10((float)this.LEV)) + alpha;
            this.ExpLimit = 5 * LEV + 5;
        }
	}
}
