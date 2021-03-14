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

	System.Random random = new System.Random();

	void Awake()
	{
		List<Dictionary<string, object>> data = CSVReader.Read(file); //read map values

		Transform newsquirrel;

		for (var i = 0; i < data.Count; i+=20)	//currently this just spawns every 20th squirrel since i tried spawning them in at once and that was a huge mistake
		{

			Vector3 pos = new Vector3( ( (float) data[i]["X"] - TRANSLATEX) * SCALE, ((float) data[i]["Y"] - TRANSLATEY) * SCALE, 0);	//get position of squirrel and transform

			newsquirrel = Instantiate(squirrel, pos, Quaternion.identity);//draw a squirrel in each place

			newsquirrel.RotateAround(new Vector3(0, 0, 0), new Vector3(0,0,1), 37);	//this rotation is arbitrary and can be tuned later
			newsquirrel.rotation = Quaternion.identity;

			//shove variables into squirrel
			newsquirrel.gameObject.GetComponent<SquirrelAi>().color = (string)data[i]["Primary Fur Color"];
			newsquirrel.gameObject.GetComponent<SquirrelAi>().id = (string)data[i]["Unique Squirrel ID"];
			newsquirrel.gameObject.GetComponent<SquirrelAi>().age = (string)data[i]["Age"];

			//nightmare of data structures
			Dictionary<string, object> temp = new Dictionary<string, object>();
			temp.Add("Kuks", data[i]["Kuks"]);
			temp.Add("Quaas", data[i]["Quaas"]);
			temp.Add("Moans", data[i]["Moans"]);
			newsquirrel.gameObject.GetComponent<SquirrelAi>().noise = pickRandomTrue(temp);

			temp = new Dictionary<string, object>();
			//Running,Chasing,Climbing,Eating,Foraging
			temp.Add("Running", data[i]["Running"]);
			temp.Add("Chasing", data[i]["Chasing"]);
			temp.Add("Climbing", data[i]["Climbing"]);
			temp.Add("Eating", data[i]["Eating"]);
			temp.Add("Foraging", data[i]["Foraging"]);
			newsquirrel.gameObject.GetComponent<SquirrelAi>().defaultBehavior = pickRandomTrue(temp);

			temp = new Dictionary<string, object>();
			//Runs from, indifferent, approaches
			temp.Add("Runs from", data[i]["Runs from"]);
			temp.Add("Approaches", data[i]["Approaches"]);
			temp.Add("Indifferent", data[i]["Indifferent"]);
			newsquirrel.gameObject.GetComponent<SquirrelAi>().playerBehavior = pickRandomTrue(temp);

			temp = new Dictionary<string, object>();
			//probably extra lol [tail twitches/flags]
			temp.Add("Tail flags", data[i]["Tail flags"]);
			temp.Add("Tail twitches", data[i]["Tail twitches"]);
			newsquirrel.gameObject.GetComponent<SquirrelAi>().behaviors = pickRandomTrue(temp);

			/*
			print("X " + data[i]["X"] + " " +
					"Y " + data[i]["Y"] + " " +
					"ID " + data[i]["Unique Squirrel ID"] + " " +
					"Hectare " + data[i]["Hectare"]);
			*/
		}

	}


	//pick random from the dataset
	string pickRandomTrue(Dictionary<string, object> bools) {
		List<string> truevals = new List<string>();

		//print(bools["Kuks"]);

		foreach (KeyValuePair<string, object> kvp in bools) 
		{
			if ((string)kvp.Value == "true") {
				truevals.Add(kvp.Key);
			}
		}

		if (truevals.Count < 1) return "";
		else {
			string randval = (string)truevals[random.Next(0, truevals.Count)];
			Debug.Log(randval);
			return randval;
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