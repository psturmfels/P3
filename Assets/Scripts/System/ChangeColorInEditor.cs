using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeColorInEditor : MonoBehaviour {
	public enum TileColor {
		Red,
		Green,
		Blue
	}

	private TileColor previousTileColor = TileColor.Red;
	public TileColor currentTileColor = TileColor.Red;
	public Sprite[] redSprites;
	public Sprite[] greenSprites;
	public Sprite[] blueSprites;

	private string[] redValues = new string[] { 
		"AbstractPlatformer_324",
		"AbstractPlatformer_285",
		"AbstractPlatformer_263",
		"AbstractPlatformer_114",
		"AbstractPlatformer_284",
		"AbstractPlatformer_134",
		"AbstractPlatformer_304",
		"AbstractPlatformer_123",
		"AbstractPlatformer_146",
		"AbstractPlatformer_4",
		"AbstractPlatformer_337",
		"AbstractPlatformer_255",
		"AbstractPlatformer_174",
		"AbstractPlatformer_210",
		"AbstractPlatformer_70",
		"AbstractPlatformer_240",
		"AbstractPlatformer_90",
		"AbstractPlatformer_190"
		};

	private string[] greenValues = new string[] { 
		"AbstractPlatformer_41", 
		"AbstractPlatformer_3",
		"AbstractPlatformer_40",
		"AbstractPlatformer_239",
		"AbstractPlatformer_68",
		"AbstractPlatformer_275",
		"AbstractPlatformer_22",
		"AbstractPlatformer_254",
		"AbstractPlatformer_274",
		"AbstractPlatformer_133",
		"AbstractPlatformer_112",
		"AbstractPlatformer_310",
		"AbstractPlatformer_89",
		"AbstractPlatformer_2",
		"AbstractPlatformer_199",
		"AbstractPlatformer_21",
		"AbstractPlatformer_225",
		"AbstractPlatformer_69"
	};

	private string[] blueValues = new string[] { 
		"AbstractPlatformer_162", 
		"AbstractPlatformer_115",
		"AbstractPlatformer_175",
		"AbstractPlatformer_25",
		"AbstractPlatformer_188",
		"AbstractPlatformer_44",
		"AbstractPlatformer_135",
		"AbstractPlatformer_43",
		"AbstractPlatformer_59",
		"AbstractPlatformer_264",
		"AbstractPlatformer_242",
		"AbstractPlatformer_91",
		"AbstractPlatformer_201",
		"AbstractPlatformer_124",
		"AbstractPlatformer_338",
		"AbstractPlatformer_313",
		"AbstractPlatformer_6",
		"AbstractPlatformer_176"
	};


	void Start () {
		
	}
	
	void Update () {
		if (previousTileColor != currentTileColor) {
			GameObject[] objects = GameObject.FindObjectsOfType (typeof(GameObject)) as GameObject[];

			foreach (GameObject obj in objects) {
				if (obj.GetComponent<SpriteRenderer> () != null) {
					SpriteRenderer sr = obj.GetComponent<SpriteRenderer> ();
					string objSpriteName = sr.sprite.name;
					switch (currentTileColor) {
					case TileColor.Blue:
						{
							int redIndex = FindIndexIn (ref redValues, ref objSpriteName);
							int greenIndex = FindIndexIn (ref greenValues, ref objSpriteName);
							if (redIndex != -1) {
								sr.sprite = blueSprites [redIndex];
							} else if (greenIndex != -1) {
								sr.sprite = blueSprites [greenIndex];
							}
						}
						break;

					case TileColor.Green:
						{
							int redIndex = FindIndexIn (ref redValues, ref objSpriteName);
							int blueIndex = FindIndexIn (ref blueValues, ref objSpriteName);
							if (redIndex != -1) {
								sr.sprite = greenSprites [redIndex];
							} else if (blueIndex != -1) {
								sr.sprite = greenSprites [blueIndex];
							}
						}
						break;

					case TileColor.Red:
						{
							int blueIndex = FindIndexIn (ref blueValues, ref objSpriteName);
							int greenIndex = FindIndexIn (ref greenValues, ref objSpriteName);
							if (blueIndex != -1) {
								sr.sprite = redSprites [blueIndex];
							} else if (greenIndex != -1) {
								sr.sprite = redSprites [greenIndex];
							}
						}
						break;

					}
					
				}
			}

			previousTileColor = currentTileColor;
		}
	}

	private int FindIndexIn(ref string[] nameArray, ref string searchedValue) {
		for (int index = 0; index < nameArray.Length; ++index) {
			if (nameArray [index] == searchedValue) {
				return index;
			}
		}
		return -1;
	}
}
