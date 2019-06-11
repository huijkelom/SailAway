using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScript : Interactable
{
    private Startup startup;
    private bool boatSelected;
    private bool playerSelected;
    private int amountOfHighlightsActive;
    private BoatControl selectedBoat;
    private bool doneMoving;
    private int amountOfMoves;
    private bool gameEnding;

    public Text AmountOfMovesText;
    public Text VictoryText;
    public GameObject NextPuzzleButton;

    // Start is called before the first frame update
    void Start()
    {
        startup = FindObjectOfType<Startup>();
        boatSelected = false;
        playerSelected = false;
        amountOfHighlightsActive = 0;
        doneMoving = true;
        amountOfMoves = 0;
        gameEnding = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (gameEnding)
        {
            if (playerSelected && !doneMoving)
            {
                if (!startup.GetPlayerCar().GetComponent<PlayerBoatControl>().keepMoving)
                {
                    playerSelected = false;
                    doneMoving = true;

                    if (startup.GetPlayerCar().GetComponent<PlayerBoatControl>().frontPos == startup.winPosition ||
                        startup.GetPlayerCar().GetComponent<PlayerBoatControl>().backPos == startup.winPosition)
                    {
                        EndGame();
                    }
                }
            }
        }
    }

    /// <summary>
    /// When player hits on the grid this function determines what was hit and activates the function that should happen when this object is hit
    /// </summary>
    /// <param name="clickposition">position where the ball hit the screen</param>
    protected override void Click(Vector3 clickposition)
    {
        if (gameEnding)
        {
            return;
        }
        //Debug.Log("click");
        //SceneManager.LoadScene("SampleScene");
        //Put clickposition x = 1,0; y = 1,1; Boat is at x 1, y 1
        Vector2 position = new Vector2(Mathf.Round(clickposition.x), Mathf.Round(clickposition.y));
        //MoveBoat(position);
        if(boatSelected && !doneMoving)
        {
            if (!selectedBoat.GetComponent<BoatControl>().keepMoving)
            {
                boatSelected = false;
                selectedBoat = null;
                doneMoving = true;
            }
        }

        if (playerSelected && !doneMoving)
        {
            if (!startup.GetPlayerCar().GetComponent<PlayerBoatControl>().keepMoving)
            {
                playerSelected = false;
                doneMoving = true;

                /*if (startup.GetPlayerCar().GetComponent<PlayerBoatControl>().frontPos == startup.winPosition ||
                    startup.GetPlayerCar().GetComponent<PlayerBoatControl>().backPos == startup.winPosition)
                {
                    EndGame();
                }*/
            }
        }

        if (doneMoving)
        {
            BoatControl boat = startup.FindHitBoat(position);
            if (boat != null)
            {
                startup.HideAllHighlights(amountOfHighlightsActive);
                boatSelected = true;
                playerSelected = false;
                Vector2 FrontAndBackDistances = Vector2.zero;
                FrontAndBackDistances = startup.FindPossibleMoveTiles(boat);

                amountOfHighlightsActive = startup.ShowFreeSpaces(FrontAndBackDistances.x, FrontAndBackDistances.y, boat);
                selectedBoat = boat;
                //Show tiles available for boat
            }
            else
            {
                //Vector2[] playerPositions = startup.CheckPlayerPosition(position);
                //if(playerPositions[0] != playerPositions[1])
                GameObject player = startup.CheckPlayerPosition(position);
                if (player != null)
                {
                    //Debug.Log("PlayerBoat selected on: " + playerPositions[1] + " and backPos: " + playerPositions[2]);
                    startup.HideAllHighlights(amountOfHighlightsActive);
                    playerSelected = true;
                    boatSelected = false;
                    selectedBoat = null;
                    Vector2 FrontAndBackDistances = Vector2.zero;
                    FrontAndBackDistances = startup.FindPossibleMoveTilesPlayer(player.GetComponent<PlayerBoatControl>());
                    amountOfHighlightsActive = startup.ShowFreeSpacesPlayer(FrontAndBackDistances.x, FrontAndBackDistances.y, player.GetComponent<PlayerBoatControl>());
                }
                else
                {
                    if (boatSelected || playerSelected)
                    {
                        if (startup.CheckHighlightHit(position, amountOfHighlightsActive))
                        {
                            if (boatSelected)
                            {
                                selectedBoat.MoveBoat(position);
                            }
                            else if (playerSelected)
                            {
                                startup.GetPlayerCar().GetComponent<PlayerBoatControl>().MovePlayer(position);
                                if(position == startup.winPosition)
                                {
                                    gameEnding = true;
                                }
                            }

                        }
                        startup.HideAllHighlights(amountOfHighlightsActive);
                        //boatSelected = false;
                        //selectedBoat = null;
                        //playerSelected = false;
                        doneMoving = false;
                        amountOfMoves++;
                        AmountOfMovesText.text = "Gemaakte zetten: " + amountOfMoves;
                    }
                }
            }
        }

    }

    public void EndGame()
    {
        startup.SetGameInactive();
        AmountOfMovesText.gameObject.SetActive(false);
        VictoryText.gameObject.SetActive(true);
        NextPuzzleButton.SetActive(true);
    }
}
