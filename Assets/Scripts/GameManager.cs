using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System;

public class GameManager : MonoBehaviour
{

   
	public GameObject Explosion;

	Bubble[,] BubblesArray = new Bubble[BubbleColumns, BubbleRows];
	const int BubbleColumns = 8;
	const int BubbleRows = 13;
	public GameObject BubbleParameter;

	List<Bubble> SelectedBubbles = new List<Bubble>();
	private Material selectedBubbleColor;

	int score = 0;

	private bool AreBubblesSelected = false;
	private bool IsGameOver = false;
	private const int minBubblesToRemove = 2;

	// Use this for initialization
	void Start()
	{
		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		float aspect = Mathf.Round(camera.aspect * 100f) / 100f;

		//this is to be altered different Windows Phone 8.0 aspect ratios
		//there should be a better way of doing this
		if (aspect == 0.6f) //WXGA or WVGA
			camera.orthographicSize = 5;
		else if (aspect == 0.56f) //720p
		{
			camera.orthographicSize = 5.37f;
			camera.transform.position = new Vector3(camera.transform.position.x, 4.62f, camera.transform.position.z);
		}
		
		//this is to be used as the color that the selected Bubbles will have
		selectedBubbleColor = Resources.Load("Materials/whiteMaterial") as Material;
		AreBubblesSelected = false;
		IsGameOver = false;

		InitializeBubbles();
	}

	/// <summary>
	/// initializes the bubbles
	/// </summary>
	private void InitializeBubbles()
	{
		for (int column = 0; column < BubbleColumns; column++)
		{
			for (int row = 0; row < BubbleRows; row++)
			{
				MyMaterial material = MyMaterial.GetRandomMaterial(); //get a random color
				//create a new bubble
				var go = (GameObject)Instantiate(BubbleParameter,
					new Vector3((float)column * BubbleParameter.transform.localScale.x,
						(float)row * BubbleParameter.transform.localScale.y, 0f), Quaternion.identity);
				go.tag = material.ColorName;
				BubblesArray[column, row] = new Bubble(go, material);
				go.name = column.ToString() + "-" + row.ToString();

				var renderer = go.transform.renderer;
				renderer.material = material; //set the color


			}
		}
	}


	RaycastHit hit;
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel("StartScene");

		if (Input.GetButtonDown("Fire1"))
		{
			Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray1, out hit, Mathf.Infinity))
			{
				GameObject selectedBubbleGO = hit.transform.gameObject;


				int column = int.Parse(selectedBubbleGO.name.Split('-')[0]);
				int row = int.Parse(selectedBubbleGO.name.Split('-')[1]);

				Bubble selectedBubble = BubblesArray[column, row];

				if (!AreBubblesSelected)//user selects a Bubble
				{
					if (selectedBubble != null)
					{
						SelectedBubbles = new List<Bubble>();
						MarkBubbles(selectedBubble, column, row, selectedBubble.GameObject.tag);
						if (SelectedBubbles.Count < minBubblesToRemove) //not enough selected Bubbles
						{
							//reset the selected
							foreach (Bubble el in SelectedBubbles)
								el.GameObject.transform.renderer.material = el.OriginalBubbleMaterial;

							return;
						}
						AreBubblesSelected = true;
					}
				}
				else if (AreBubblesSelected) //Bubbles are already selected
				{
					if (SelectedBubbles.Contains(selectedBubble) && SelectedBubbles.Count >= minBubblesToRemove)//let's disappear them!
					{
						score += SelectedBubbles.Count;
						foreach (Bubble el in SelectedBubbles)
						{
							int column2 = int.Parse(el.GameObject.name.Split('-')[0]);
							int row2 = int.Parse(el.GameObject.name.Split('-')[1]);
							//create the explosion
							GameObject explosion = Instantiate(Explosion,
                                BubblesArray[column2, row2].GameObject.transform.position, BubblesArray[column2, row2].GameObject.transform.rotation) as GameObject;
							Destroy(explosion, 1f);
							if (SettingsManager.Sound) Camera.main.audio.Play();
							Destroy(BubblesArray[column2, row2].GameObject);
							BubblesArray[column2, row2] = null;
						}
						//let's deorganize the rest of the Bubbles
						ReallocateBubbles();

					}
					else
					{
						foreach (Bubble el in SelectedBubbles)
						{
							el.GameObject.renderer.material = el.OriginalBubbleMaterial;
						}
					}
					AreBubblesSelected = false;
				}
			}
			IsGameOver = CheckIsGameOver();
			if (IsGameOver)
				StartCoroutine(GotoGameOver());
		}
	}

	private IEnumerator GotoGameOver()
	{
		ScoreManager sm = new ScoreManager();
		sm.AddScore(new ScoreEntry() { ScoreInt = this.score, Date = DateTime.Now });
		yield return new WaitForSeconds(2f);
		Globals.GameScore = score;
		Application.LoadLevel("highScoresScene");
	}

	private void MarkBubbles(Bubble Bubble, int column, int row, string colorToCompare)
	{
		if (Bubble != null)
		{
			if (Bubble.GameObject.tag == colorToCompare)
			{
				if (SelectedBubbles.Contains(Bubble)) return; //we're not checking the same Bubble twice, this will incur a stack overflow

				Bubble.GameObject.transform.renderer.material = selectedBubbleColor;
				SelectedBubbles.Add(Bubble);

				//check bottom
				if (row > 0)
					MarkBubbles(BubblesArray[column, row - 1], column, row - 1, colorToCompare);
				if (column > 0) //check left
					MarkBubbles(BubblesArray[column - 1, row], column - 1, row, colorToCompare);
				if (column < BubbleColumns - 1) //check right
					MarkBubbles(BubblesArray[column + 1, row], column + 1, row, colorToCompare);
				if (row < BubbleRows - 1) //check top
					MarkBubbles(BubblesArray[column, row + 1], column, row + 1, colorToCompare);
			}
			else
				return;
		}
	}

	private bool CheckIsGameOver()
	{
		//if there are any Bubbles selected, there's no point in checking as it's definitely not game over
		if (AreBubblesSelected) return false;

		for (int column = 0; column <= BubbleColumns - 1; column++)
		{
			for (int row = BubbleRows - 1; row > 0; row--)
			{
				//we are comparing each Bubble with the ones located below and right from it
				if (BubblesArray[column, row] == null) continue;


				if (BubblesArray[column, row].GameObject.tag == BubblesArray[column, row - 1].GameObject.tag)
					return false;

				if (column < BubbleColumns - 1)
				{
					if (BubblesArray[column + 1, row] == null) continue;

					if (BubblesArray[column, row].GameObject.tag == BubblesArray[column + 1, row].GameObject.tag)
						return false;
				}
			}
		}

		return true;

	}



	void OnGUI()
	{
		Helpers.AutoResize(Globals.Width, Globals.Height);

		GUI.Label(new Rect(10, 0, 400, 50), "Score " + score.ToString());
	}



	private void ReallocateBubbles()
	{
		//first, let's clear the empty spaces in the rows
		for (int column = 0; column < BubbleColumns; column++)
			for (int row = BubbleRows - 1; row >= 0; row--)
			{
				//
				for (int l = 0; l <= row - 1; l++)
				{
					if (BubblesArray[column, l] == null && BubblesArray[column, l + 1] != null)
					{
						BubblesArray[column, l] = BubblesArray[column, l + 1];
						BubblesArray[column, l + 1] = null;
						BubblesArray[column, l].GameObject.name = column.ToString() + "-" + l.ToString();
					}
				}

			}

		//now, we'll check for empty columns
		for (int column = BubbleColumns - 1; column >= 0; column--)
		{
			for (int l = 1; l <= column; l++)
			{
				//we'll check the bottom element
				//if it's null, then the whole row is null
				if (BubblesArray[l, 0] == null && BubblesArray[l - 1, 0] != null)
				{
					//copy entire column...
					for (int k = 0; k < BubbleRows; k++)
					{
						if (BubblesArray[l - 1, k] == null) continue;
						BubblesArray[l, k] = BubblesArray[l - 1, k];
						BubblesArray[l - 1, k] = null;
						BubblesArray[l, k].GameObject.transform.position += new Vector3(BubbleParameter.transform.localScale.x, 0f, 0f);
						BubblesArray[l, k].GameObject.name = l.ToString() + "-" + k.ToString();
					}
				}
			}
		}
	}



}
