using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
	[SerializeField] string file;

	//numbers to translate lat/long coords to origin
	//public const float TRANSLATEX = -73.98191574690729f; //leftmost x	[NEEDS TUNING] 
	//public const float TRANSLATEY = 40.76427025270916f; //bottommost y	[NEEDS TUNING]
	
	//center of great lawn(?) might be offset
	public const float TRANSLATEX = -73.96650723107567f; 	 
	public const float TRANSLATEY = 40.781470012217426f; 

	public const float SCALE = 20000;   //scale coords by this much [random arbitrary number for now lolol]

	//we can rotate the map if time idk

	[SerializeField] private Transform squirrel;

	//ui
	public int acornCt = 100;
	public Text acornTxt;

	void Awake()
	{

		List<Dictionary<string, object>> data = CSVReader.Read(file); //read map values

		Transform newsquirrel;

		for (var i = 0; i < data.Count; i+=20)	//currently this spawns every 20 squirrel since i tried spawning them in at once and that was a huge mistake
		{
			Vector3 pos = new Vector3( ( (float) data[i]["X"] - TRANSLATEX) * SCALE, ((float) data[i]["Y"] - TRANSLATEY) * SCALE, 0);	//get position of squirrel

			newsquirrel = Instantiate(squirrel, pos, Quaternion.identity);//draw a squirrel in each place

			newsquirrel.RotateAround(new Vector3(0, 0, 0), new Vector3(0,0,1), 37);	//this rotation is arbitrary and can be tuned later

			newsquirrel.rotation = Quaternion.identity;

			//shove variables into squirrel
			newsquirrel.gameObject.GetComponent<SquirrelAi>().color = (string)data[i]["Primary Fur Color"];
			newsquirrel.gameObject.GetComponent<SquirrelAi>().id = (string)data[i]["Unique Squirrel ID"];
			newsquirrel.gameObject.GetComponent<SquirrelAi>().age = (string)data[i]["Age"];

			print(newsquirrel.gameObject.GetComponent<SquirrelAi>().age);
			//newsquirrel.gameObject.GetComponent<SquirrelAi>().color = (string)data[i]["color"];
			//newsquirrel.gameObject.GetComponent<SquirrelAi>().color = (string)data[i]["color"];


			/*
			print("X " + data[i]["X"] + " " +
					"Y " + data[i]["Y"] + " " +
					"ID " + data[i]["Unique Squirrel ID"] + " " +
					"Hectare " + data[i]["Hectare"]);
			*/
		}





	}

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void DecreaseAcorns(){
		acornCt--;
		acornTxt.text = "x" + acornCt.ToString();
	}
}