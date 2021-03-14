using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	static char[] TRIM_CHARS = { '\"' };

	public static List<Dictionary<string, object>> Read(string file)
	{
<<<<<<< HEAD
		
		var list = new List<Dictionary<string, object>>();
		TextAsset data = Resources.Load(file) as TextAsset;

		var lines = Regex.Split(data.text, LINE_SPLIT_RE);

		if (lines.Length <= 1) return list;

		var header = Regex.Split(lines[0], SPLIT_RE);
		for (var i = 1; i < lines.Length; i++)
		{

			var values = Regex.Split(lines[i], SPLIT_RE);
			if (values.Length == 0 || values[0] == "") continue;

			var entry = new Dictionary<string, object>();
			for (var j = 0; j < header.Length && j < values.Length; j++)
			{
=======
		var list = new List<Dictionary<string, object>>();
		TextAsset data = Resources.Load (file) as TextAsset;

		var lines = Regex.Split (data.text, LINE_SPLIT_RE);

		if(lines.Length <= 1) return list;

		var header = Regex.Split(lines[0], SPLIT_RE);
		for(var i=1; i < lines.Length; i++) {

			var values = Regex.Split(lines[i], SPLIT_RE);
			if(values.Length == 0 ||values[0] == "") continue;

			var entry = new Dictionary<string, object>();
			for(var j=0; j < header.Length && j < values.Length; j++ ) {
>>>>>>> 1c2f9f1081b0483fc32e637ca0b3f0102cb4f505
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				object finalvalue = value;
				int n;
				float f;
<<<<<<< HEAD
				if (int.TryParse(value, out n))
				{
					finalvalue = n;
				}
				else if (float.TryParse(value, out f))
				{
=======
				if(int.TryParse(value, out n)) {
					finalvalue = n;
				} else if (float.TryParse(value, out f)) {
>>>>>>> 1c2f9f1081b0483fc32e637ca0b3f0102cb4f505
					finalvalue = f;
				}
				entry[header[j]] = finalvalue;
			}
<<<<<<< HEAD
			list.Add(entry);
		}
		return list;
	}
}
=======
			list.Add (entry);
		}
		return list;
	}
}
>>>>>>> 1c2f9f1081b0483fc32e637ca0b3f0102cb4f505
