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

	public GameObject squirrel_cinammon;
	public GameObject squirrel_black;
	public GameObject squirrel_gray;

	//acorns
	public int acornCt = 100;
	public Text acornTxt;

	//gallery
	public List<Dictionary<string,string>> squirrelSet = new List<Dictionary<string,string>>(); 
	public GameObject infoScreen;
	public List<string> savedSquirrels = new List<string>();

	System.Random random = new System.Random();

	public AudioSource bgm;

	void Awake()
	{
		List<Dictionary<string, object>> data = CSVReader.Read(file); //read map values

		GameObject newsquirrel;;

		for (var i = Random.Range(0,30); i < data.Count; i+=20)	//currently this just spawns every 20th squirrel since i tried spawning them in at once and that was a huge mistake
		{

			Vector3 pos = new Vector3( ( (float) data[i]["X"] - TRANSLATEX) * SCALE, ((float) data[i]["Y"] - TRANSLATEY) * SCALE, 0);	//get position of squirrel and transform

			string squirrelColor = (string)data[i]["Primary Fur Color"];
			GameObject sq = null;
			if(squirrelColor == "Gray"){
				sq = squirrel_gray;
			}else if(squirrelColor == "Black"){
				sq = squirrel_black;
			}else{
				sq = squirrel_cinammon;
			}


			newsquirrel = Instantiate(sq, pos, Quaternion.identity);//draw a squirrel in each place

			newsquirrel.transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0,0,1), 37);	//this rotation is arbitrary and can be tuned later
			newsquirrel.transform.rotation = Quaternion.identity;

			//shove variables into squirrel
			SquirrelAi sqAI = newsquirrel.GetComponent<SquirrelAi>();
			sqAI.color = squirrelColor;
			sqAI.id = (string)data[i]["Unique Squirrel ID"];
			sqAI.age = (string)data[i]["Age"];
			sqAI.SetAgeSpeed();

			//nightmare of data structures
			Dictionary<string, object> temp = new Dictionary<string, object>();
			temp.Add("Kuks", data[i]["Kuks"]);
			temp.Add("Quaas", data[i]["Quaas"]);
			temp.Add("Moans", data[i]["Moans"]);
			sqAI.noise = pickRandomTrue(temp).ToLower();

			temp = new Dictionary<string, object>();
			//Running,Chasing,Climbing,Eating,Foraging
			temp.Add("Running", data[i]["Running"]);
			temp.Add("Chasing", data[i]["Chasing"]);
			temp.Add("Climbing", data[i]["Climbing"]);
			temp.Add("Eating", data[i]["Eating"]);
			temp.Add("Foraging", data[i]["Foraging"]);
			sqAI.defaultBehavior = pickRandomTrue(temp).ToLower();

			temp = new Dictionary<string, object>();
			//Runs from, indifferent, approaches
			temp.Add("Runs from", data[i]["Runs from"]);
			temp.Add("Approaches", data[i]["Approaches"]);
			temp.Add("Indifferent", data[i]["Indifferent"]);
			sqAI.playerBehavior = pickRandomTrue(temp).ToLower();

			temp = new Dictionary<string, object>();
			//probably extra lol [tail twitches/flags]
			temp.Add("Tail flags", data[i]["Tail flags"]);
			temp.Add("Tail twitches", data[i]["Tail twitches"]);
			sqAI.behaviors = pickRandomTrue(temp).ToLower();

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
			//Debug.Log(randval);
			return randval;
		} 
	}

	// Use this for initialization
	void Start()
	{
		infoScreen.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown("m")){
			bgm.volume = (bgm.volume == 0.05f ? 0f : 0.05f);
		}
	}

	public void DecreaseAcorns(){
		acornCt--;
		acornTxt.text = "x" + acornCt.ToString();
	}

	//adds squirrel to the dictionary set
	public void AddSquirrel(string id, string color, string activity, string noise, string humans, string funfact, Sprite sprite){
		//already saved
		if(savedSquirrels.Contains(id)){
			return;
		}

		savedSquirrels.Add(id);

		Dictionary<string,string> sq = new Dictionary<string,string>();
		sq.Add("id", id);
		sq.Add("color", color);
		sq.Add("fav_activity", activity);
		sq.Add("noise", noise);
		sq.Add("humans", humans);
		sq.Add("fun_fact", funfact);

		squirrelSet.Add(sq);
		Debug.Log(squirrelSet.Count);

		if(squirrelSet.Count < 10){
			GameObject.Find("Pic" + squirrelSet.Count).GetComponent<Image>().sprite = sprite;
		}
	}

	//show the entry 
	public void ShowGalleryEntry(int entry){
		if(entry > squirrelSet.Count){
			return;
		}

		//populate entry
		Dictionary<string,string> sq = squirrelSet[entry-1];

		infoScreen.transform.Find("ID").GetComponent<Text>().text = sq["id"];
		infoScreen.transform.Find("Pic").GetComponent<Image>().sprite = GameObject.Find("Pic" + entry).GetComponent<Image>().sprite;
		infoScreen.transform.Find("Activity").GetComponent<Text>().text = "Fave Activity: " + sq["fav_activity"];
		infoScreen.transform.Find("Noise").GetComponent<Text>().text = "Noise: " + sq["noise"];
		infoScreen.transform.Find("Humans").GetComponent<Text>().text = "Humans: " + sq["humans"];
		infoScreen.transform.Find("Fun Fact").GetComponent<Text>().text = "Fun Fact: " + sq["fun_fact"];

		infoScreen.SetActive(true);
	}

	//hide the gallery
	public void HideGalleryEntry(){
		infoScreen.SetActive(false);
	}

}