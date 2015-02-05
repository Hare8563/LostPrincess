using UnityEngine;
using System.Collections;
using CSV;

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
		

		private CsvReader lvData;
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
		public Status(){
			lvData = new CsvReader ("Assets/LvTable.csv");
			LEV = 1;
			EXP = 0;
			HP = lvData.getParamValue(1, CsvParam.HP);
			MP = lvData.getParamValue(1, CsvParam.MP);
			BOW_POW = lvData.getParamValue (1, CsvParam.BOW_ATK);
			Sword_Power=lvData.getParamValue(1,CsvParam.SWORD_ATK);
			Magic_Power = lvData.getParamValue(1,CsvParam.MAGIC_ATK);
			NAME = "NONAME";
		}

		public Status(int Lv, string LvTablePath){
			lvData = new CsvReader (LvTablePath);
			LEV = Lv;
			EXP = 0;
			HP = lvData.getParamValue(LEV-1, CsvParam.HP);
			MP = lvData.getParamValue(LEV-1, CsvParam.MP);
			BOW_POW = lvData.getParamValue (LEV-1, CsvParam.BOW_ATK);
			Sword_Power=lvData.getParamValue(LEV-1,CsvParam.SWORD_ATK);
			Magic_Power = lvData.getParamValue(LEV-1,CsvParam.MAGIC_ATK);
			NAME = "NONAME";
		}

        /// <summary>
		/// パラメータ指定コンストラクタ
        /// </summary>
        /// <param name="Lev">レベル</param>
        /// <param name="Exp">取得経験値</param>
		/// <param name="Hp">HP</param>
		/// <param name="LvTablePath">LevelTableのファイルパス</param>
		public Status(int Lev, int Exp,int Hp, string LvTablePath){
			lvData = new CsvReader (LvTablePath);
            LEV = Lev;
			EXP = Exp;
			HP = Hp < lvData.getParamValue(Lev, CsvParam.HP) ? Hp : lvData.getParamValue(Lev, CsvParam.HP);//Hpが最大値以下の場合はHpを入れる
			MP = lvData.getParamValue(Lev, CsvParam.MP);
			BOW_POW = lvData.getParamValue (Lev, CsvParam.BOW_ATK);
			Sword_Power = lvData.getParamValue(Lev, CsvParam.SWORD_ATK);
			Magic_Power = lvData.getParamValue(Lev, CsvParam.MAGIC_ATK);
			NAME = "NONAME";
        }

		/// <summary>
		/// ステータス+名前指定コンストラクタ
		/// </summary>
		/// <param name="Lev">レベル</param>
		/// <param name="Exp">取得経験値</param>
		/// <param name="Hp">ヒットポイント</param>
		/// <param name="Mp">マジックポイント</param>
		/// <param name="Name">キャラクター名</param>
		/// <param name="LvTablePath">LevelTableのファイルパス</param>
		public Status(int Lev, int Exp, int Hp, int Mp, string Name, string LvTablePath){
			lvData = new CsvReader (LvTablePath);
			LEV = Lev;
			EXP = Exp;
			HP = Hp < lvData.getParamValue(Lev, CsvParam.HP) ? Hp : lvData.getParamValue(Lev, CsvParam.HP);//Hpが最大値以下の場合はHpを入れる
			MP = Mp < lvData.getParamValue(Lev, CsvParam.MP) ? Mp : lvData.getParamValue(Lev, CsvParam.MP);
			BOW_POW = lvData.getParamValue (Lev, CsvParam.BOW_ATK);
			Sword_Power = lvData.getParamValue(Lev, CsvParam.SWORD_ATK);
			Magic_Power = lvData.getParamValue(Lev, CsvParam.MAGIC_ATK);
			NAME = Name;
		}
		
        /// <summary>
        /// レベルアップ
        /// </summary>
		public void LevUp(){
            this.LEV++;
            this.HP = lvData.getParamValue(this.LEV, CsvParam.HP);
            this.MP = lvData.getParamValue(this.LEV, CsvParam.MP);
            this.EXP = 0;
			this.Sword_Power = lvData.getParamValue(this.LEV, CsvParam.SWORD_ATK);
			this.Magic_Power = lvData.getParamValue(this.LEV, CsvParam.MAGIC_ATK);
			this.BOW_POW = lvData.getParamValue(this.LEV, CsvParam.BOW_ATK);
            this.ExpLimit = 5 * LEV + 5;
        }
	}
}
