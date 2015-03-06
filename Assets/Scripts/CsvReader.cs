using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CSV{

	public enum CsvParam{
				HP=1, MP=2, SWORD_ATK=3, MAGIC_ATK=4, BOW_ATK=5
	};

	public class CsvReader {
		private int[,] csvData;

		public CsvReader(string filePath){
            //Debug.Log(filePath);
            var csvFile = Resources.Load(filePath) as TextAsset;
           
//            using (System.IO.StreamReader sr = new System.IO.StreamReader(filePath, System.Text.Encoding.Default))
//			{
					string str = "";
					List<string> arrText = new List<string>();

                    System.IO.StringReader reader = new System.IO.StringReader(csvFile.text);
                    while (reader.Peek() > -1)
					{
                            str = reader.ReadLine();
							if (str != null)
							{
									arrText.Add(str);
							}

					}
					int line_count = arrText.Count;
					string temp = (string)arrText[0];
					string[] temp2 = temp.Split(',');
					int col_count = temp2.Length;
					csvData = new int[line_count, col_count];
					int a = 0, b = 0;
					foreach (string sOut in arrText)
					{
							string[] temp_line = sOut.Split(',');
							foreach (string value in temp_line)
							{
									if (value != "")
									{
											csvData[a, b] = int.Parse(value);
											b++;
									}
							}
							b = 0;
							a++;
					}

//					sr.Close();
//			}
		}

		public int getParamValue(int level, CsvParam param){
						int index = (int)param;
						return csvData[level, index];
		}
	}
}
