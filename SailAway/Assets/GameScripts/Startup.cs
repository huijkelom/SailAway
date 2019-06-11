using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    public float fieldSizeX;
    public float fieldSizeY;
    public float minBoatSize;

    public GameObject Boat;
    public GameObject PlayerBoat;
    public GameObject Wall;
    public GameObject GridTileHighlight;
    public GameObject Grid;

    public GameObject[] boats;
    public GameObject[] walls;
    public GameObject[] gridTileHighlights;
    //public Vector2[] takenSpawnPositions;

    private Vector2 spawnPos;
    private int minimumFieldSize = 5;
    private int amountOfHighlights = 0;

    public float xBoat3Pos = 0;
    public float yBoat3Pos = 0;

    public Vector2 boatStartPos;

    public int currentAmountOfBoats;
    public int amountOfOtherBoats;

    public Vector2 randomPosition;
    private GameObject playerCar;

    public Vector2 winPosition;
    public bool PreSetSolution;

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
        spawnPos = new Vector2(0, 0);
        winPosition = new Vector2(0, 0);
        walls = new GameObject[5];
        //FieldSizes will always be uneven
        if (fieldSizeX % 2 == 0)
        {
            fieldSizeX++;
        }
        if (fieldSizeY % 2 == 0)
        {
            fieldSizeY++;
        }
        Grid.transform.localScale = new Vector2(fieldSizeX, fieldSizeY);
        if (PreSetSolution)
        {
            CreatePreSetField();
        }
        else
        {
            CreateField();
        }
        amountOfHighlights = CalculateAmountOfHighlights();
        gridTileHighlights = new GameObject[amountOfHighlights];
        CreateGridTileHighlights();

        currentAmountOfBoats = 0;

        boats = new GameObject[amountOfOtherBoats];


        if (PreSetSolution)
        {
            CreatePreSetPuzzle();
        }
        else
        {
            SpawnPlayerBoat();
            CalculateWinPosition();
            CreatePuzzle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region PreSetSolutions
    private void CreatePreSetPuzzle()
    {
        //On the cards it says 6 by 6, but we use a 7 by 7 grid to not screw up the other calculations in the game
        //When making the solution just don't use one of the horizontal and one of the vertical lines that are also in the game
        //Make sure to slightly zoom out the camera to make the grid and text more visible, because the current position is made for a grid of x = 7 and y = 5
        //  while this grid will be 7 by 7
        winPosition = new Vector2(-3, -1);
        SpawnPreSetPlayerBoat();

        /*Put the rest of the boats here
        Remember to put them in the "boats" array one by one
        Set the currentDirection value of the BoatControl script of this boat, for instance to Direction.North
        Make sure to set the FrontPos and BackPos for the boats as well, which are part of the BoatControl script 
        (and possibly the MidPos as well for boats with size 3)
        You can simply set these by picking the coordinate for the correct grid tile
        
        After that you should set the currentAmountOfBoats value, not the amountOfOtherBoatsValue (that one should be done in the editor)
        
        In case of a boat that is 3 spaces big you should add a midPosition value to the BoatControl script
        

        The following part should be added to the end of placing a boat to fix the positioning
        Note that the current value that it is added to doens't have the correct name, but these steps need to be taken to fix the actual positioning of the boat
        Do make sure though that you don't edit the FrontPos, BackPos (and MidPos) values to do this. Those numbers shouldn't be altered at this section
        At the very end of placing the boat, use SetActive to make it usable and visible, 
        if you don't use this at the last step boats could be bumping into each other at the start, which screws up the entire puzzle
        if (boat.currentDirection == Direction.West || boat.currentDirection == Direction.East)
        {
            boatPosition.x -= 0.5f;
        }
        if (boat.currentDirection == Direction.North || boat.currentDirection == Direction.South)
        {
            boatPosition.y -= 0.5f;
        }
        boat.SetActive(true);
        */
    }

    /// <summary>
    /// Sets up the playing field with the standard for PreSet solutions
    /// </summary>
    private void CreatePreSetField()
    {
        fieldSizeX = 7;
        fieldSizeY = 7;

        //These are the up and down walls
        Vector2 wallPos = new Vector2(0, fieldSizeY / 2 + 0.5f);
        GameObject leftWall = Instantiate(Wall, wallPos, Quaternion.identity);
        //leftWall.transform.localScale = new Vector2(fieldSizeX+2, 1);
        leftWall.GetComponent<SpriteRenderer>().size = new Vector2(fieldSizeX + 2, 1);
        walls[0] = leftWall;

        wallPos *= -1;
        GameObject rightWall = Instantiate(Wall, wallPos, Quaternion.identity);
        //rightWall.transform.localScale = new Vector2(fieldSizeX+2, 1);
        rightWall.GetComponent<SpriteRenderer>().size = new Vector2(fieldSizeX + 2, 1);
        walls[1] = rightWall;


        //These are the left and right walls
        wallPos = new Vector2((fieldSizeX + 1) / 2, 0);
        GameObject upWall = Instantiate(Wall, wallPos, Quaternion.identity);
        //upWall.transform.localScale = new Vector2(1, fieldSizeY);
        upWall.GetComponent<SpriteRenderer>().size = new Vector2(1, fieldSizeY);
        walls[2] = upWall;

        wallPos *= -1;
        wallPos.y = (fieldSizeY - 1) / 2;
        GameObject downWall1 = Instantiate(Wall, wallPos, Quaternion.identity);
        //downWall1.transform.localScale = new Vector2(1, fieldSizeY/5*2);
        downWall1.GetComponent<SpriteRenderer>().size = new Vector2(1, fieldSizeY / 5 * 2);
        walls[3] = downWall1;

        wallPos.y *= -1;
        GameObject downWall2 = Instantiate(Wall, wallPos, Quaternion.identity);
        //downWall2.transform.localScale = new Vector2(1, fieldSizeY/5*2);
        downWall2.GetComponent<SpriteRenderer>().size = new Vector2(1, fieldSizeY / 5 * 2);
        walls[4] = downWall2;
    }

    /// <summary>
    /// Responsible for spawning the playerBoat, only currenly happens as player.direction = West specifically made for pre set solutions
    /// </summary>
    public void SpawnPreSetPlayerBoat()
    {
        Vector2 playerPos = new Vector2(2, -1);
        //Debug.Log(playerPos);
        playerCar = Instantiate(PlayerBoat, playerPos, Quaternion.identity);
        playerCar.transform.Rotate(0, 0, 90);
        PlayerBoatControl player = playerCar.GetComponent<PlayerBoatControl>();
        player.backPos = new Vector2(Mathf.Floor(fieldSizeX / 2), 0);
        player.GetComponent<PlayerBoatControl>().frontPos = player.backPos + CalculateDirectionModifier(player.currentDirection);
    }
    #endregion

    /// <summary>
    /// Sets up the playing field with the help of the public variables
    /// </summary>
    private void CreateField()
    {
        if(fieldSizeX < minimumFieldSize || fieldSizeY < minimumFieldSize)
        {
            return;
        }

        //These are the up and down walls
        Vector2 wallPos = new Vector2(0, fieldSizeY/2+0.5f);
        GameObject leftWall = Instantiate(Wall, wallPos, Quaternion.identity);
        //leftWall.transform.localScale = new Vector2(fieldSizeX+2, 1);
        leftWall.GetComponent<SpriteRenderer>().size = new Vector2(fieldSizeX + 2, 1);
        walls[0] = leftWall;

        wallPos *= -1;
        GameObject rightWall = Instantiate(Wall, wallPos, Quaternion.identity);
        //rightWall.transform.localScale = new Vector2(fieldSizeX+2, 1);
        rightWall.GetComponent<SpriteRenderer>().size = new Vector2(fieldSizeX + 2, 1);
        walls[1] = rightWall;


        //These are the left and right walls
        wallPos = new Vector2((fieldSizeX+1) / 2, 0);
        GameObject upWall = Instantiate(Wall, wallPos, Quaternion.identity);
        //upWall.transform.localScale = new Vector2(1, fieldSizeY);
        upWall.GetComponent<SpriteRenderer>().size = new Vector2(1, fieldSizeY);
        walls[2] = upWall;

        wallPos *= -1;
        wallPos.y = (fieldSizeY-1) / 2; 
        GameObject downWall1 = Instantiate(Wall, wallPos, Quaternion.identity);
        //downWall1.transform.localScale = new Vector2(1, fieldSizeY/5*2);
        downWall1.GetComponent<SpriteRenderer>().size = new Vector2(1, fieldSizeY/5*2);
        walls[3] = downWall1;

        wallPos.y *= -1;
        GameObject downWall2 = Instantiate(Wall, wallPos, Quaternion.identity);
        //downWall2.transform.localScale = new Vector2(1, fieldSizeY/5*2);
        downWall2.GetComponent<SpriteRenderer>().size = new Vector2(1, fieldSizeY/5*2);
        walls[4] = downWall2;
    }

    private void CalculateWinPosition()
    {
        int xPos = 0;
        int yPos = 0;
        switch (playerCar.GetComponent<PlayerBoatControl>().currentDirection)
        {
            //Should be west, but player always spawns at same position as of right now
            case Direction.East:
                xPos = Mathf.FloorToInt((fieldSizeX + 1) / 2 - 1)*-1;
                break;
        }
        winPosition = new Vector2(xPos, yPos);
    }

    /// <summary>
    /// Can count the amount of highlights that could maximally be used in the current scenario
    /// </summary>
    /// <returns>int of amount of highlights</returns>
    private int CalculateAmountOfHighlights()
    {
        float highestFieldSize = 0;
        if (fieldSizeX > fieldSizeY)
        {
            highestFieldSize = fieldSizeX;
        }
        else
        {
            highestFieldSize = fieldSizeY;
        }
        int amountOfHighlights = Mathf.FloorToInt(highestFieldSize - minBoatSize);
        return amountOfHighlights;
    }

    /// <summary>
    /// Uses the amount of max highlights, creates these and puts them in an array of highlights
    /// </summary>
    private void CreateGridTileHighlights()
    {
        for(int i = 0; i < amountOfHighlights; i++)
        {
            GameObject highlight = Instantiate(GridTileHighlight, new Vector3(fieldSizeX, fieldSizeY, 0), Quaternion.identity);
            highlight.SetActive(false);
            gridTileHighlights[i] = highlight;
        }
    }

    /// <summary>
    /// Responsible for spawning the playerBoat, only currenly happens as player.direction = West and playerBoat spawns on the highest x axis possible
    /// </summary>
    public void SpawnPlayerBoat()
    {
        Vector2 playerPos = new Vector2(Mathf.Floor(fieldSizeX / 2) -0.5f, 0);
        //Debug.Log(playerPos);
        playerCar = Instantiate(PlayerBoat, playerPos, Quaternion.identity);
        playerCar.transform.Rotate(0, 0, 90);
        PlayerBoatControl player = playerCar.GetComponent<PlayerBoatControl>();
        player.backPos = new Vector2(Mathf.Floor(fieldSizeX / 2), 0);
        player.GetComponent<PlayerBoatControl>().frontPos = player.backPos + CalculateDirectionModifier(player.currentDirection);
        //boats[0] = playerCar;
        /*playerCar.GetComponent<PlayerBoatControl>().frontPos = new Vector2(playerPos.x, 0);
        playerCar.GetComponent<PlayerBoatControl>().frontPos = new Vector2(playerPos.x - 1, 0);*/
    }

    /// <summary>
    /// Responsible for creating the boats and placing them in such a way that it creates a random but possible puzzle
    /// </summary>
    public void CreatePuzzle()
    {
        for (int i = 0; i < amountOfOtherBoats; i++)
        {
            //Decide where the boats placement will be
            //Every boat is facing West currently
            //So a Random.Range(1,4) for the facing direction
            //Player size is three and always has the middle row on the exit
            //The room that is left is fieldSize-playerSize on that lane
            //The other lanes have their full size left
            //A regular boat has a size of two, which means it interferes with the standard size of the game tiles
            //To make sure it gets on whole tiles we have to move the boat 0.5 in the way it's facing (facing up is y += 0.5)
            //exit row = 3 currently, which is y = 0 in this case and otherwise it would be x = 0 when facing up or down
            //if facing left the range to the left is (fieldSizeX/2-1)*-1 and the range to the right is 


            //Cars can't spawn on row y=0 if boat is facing East or West
            //Cars can't spawn between the x range of fieldSizeX/2-1 and the y range of (fieldSizeY-1) / 2
            GameObject boat = Instantiate(Boat, spawnPos, Quaternion.identity);
            currentAmountOfBoats++;
            boat.SetActive(false);

            bool positionTaken = true;
            randomPosition = new Vector2(0, 0);
            Vector2 randomSecondPosition = new Vector2(0, 0);
            Direction currentDirection;
            if(currentAmountOfBoats == 1)
            {
                int randomNorthSouth = Random.Range(0, 1);
                currentDirection = boat.GetComponent<BoatControl>().FaceFirstBoatDirection(randomNorthSouth);
            }
            else
            {
                currentDirection = boat.GetComponent<BoatControl>().FaceRandomDirection();
            }
            int loopAmount = 0;
            while (positionTaken && loopAmount < 5)
            {
                loopAmount++;
                positionTaken = false;
                
                if (currentAmountOfBoats == 1)
                {
                    randomPosition = CreateRandomFirstOtherBoatPosition(currentDirection);
                    /*Vector2 directionModifier = CalculateDirectionModifier(currentDirection);
                    Debug.Log("Boat " + currentAmountOfBoats + ", Position " + randomPosition + ", Position behind " + 
                    (randomPosition + directionModifier) + " currentDirection " + currentDirection);*/
                }
                else
                {
                    randomPosition = CreateRandomBoatPosition(currentDirection);
                }
                //for (int j = 0; j < (currentAmountOfBoats * 2 - 2); j++)
                for (int j = 0; j < (currentAmountOfBoats - 1); j++)
                {
                    Vector2 directionModifier = CalculateDirectionModifier(currentDirection);
                    //randomPosition = CalculateDirectionModifier(currentDirection, randomPosition);
                    //if (takenSpawnPositions[j] == randomPosition || takenSpawnPositions[j] == (randomPosition + directionModifier))
                    //if (takenSpawnPositions[j] == randomPosition || takenSpawnPositions[j] == randomPosition)
                    BoatControl checkCarControls = boats[j].GetComponent<BoatControl>();
                    if (checkCarControls.frontPos == randomPosition || checkCarControls.backPos == randomPosition ||
                        checkCarControls.frontPos == (randomPosition + directionModifier) || checkCarControls.backPos == (randomPosition + directionModifier))
                    {
                        positionTaken = true;
                        //Debug.Log("Boat " + currentAmountOfBoats + ", position taken");
                    }
                }
                //Check randomSecondPosition as well
                randomSecondPosition = randomPosition + CalculateDirectionModifier(currentDirection);
                if (!positionTaken && CheckIfColumnAlmostFull(randomSecondPosition, randomPosition, currentDirection))
                {
                    positionTaken = true;
                }
            }

            //takenSpawnPositions[currentAmountOfBoats-1] = randomPosition;
            //Vector2 randomSecondPosition = randomPosition + CalculateDirectionModifier(currentDirection);
            //takenSpawnPositions[currentAmountOfBoats] = randomSecondPosition;

            //Here the front and back positions are given to the boat for later use
            //This is done because the second position is the front for West and South, while the first position is the front for the others
            BoatControl carControl = boat.GetComponent<BoatControl>();
            if (currentDirection == Direction.West || currentDirection == Direction.South)
            {
                carControl.frontPos = randomSecondPosition;
                carControl.backPos = randomPosition;
            }
            else
            {
                carControl.frontPos = randomPosition;
                carControl.backPos = randomSecondPosition;
            }

            if (currentDirection == Direction.West || currentDirection == Direction.East)
            {
                randomPosition.x -= 0.5f;
            }
            if (currentDirection == Direction.North || currentDirection == Direction.South)
            {
                randomPosition.y -= 0.5f;
            }

            if (loopAmount >= 5)
            {
                boat.SetActive(false);
                i--;
                currentAmountOfBoats -= 1;
            }
            else
            {
                boat.transform.position = randomPosition;
                boat.SetActive(true);

                boats[i] = boat;
            }

            //Vector2[] takenPositions
        }
    }

    /// <summary>
    /// A helping function to randomize the position of a boat
    /// </summary>
    /// <param name="currentDirection">the determined direction of the boat</param>
    /// <returns>the random position for the boat</returns>
    private Vector2 CreateRandomBoatPosition(Direction currentDirection)
    {
        float yCar = 0;
        if (currentDirection == Direction.East || currentDirection == Direction.West)
        {
            //Make sure yCar is not 0 if direction is east or west
            while (yCar == 0)
            {
                yCar = Mathf.RoundToInt(Random.Range((fieldSizeY - 1) / 2 * -1, (fieldSizeY - 1) / 2));
            }
        }
        else
        {
            /*if(currentDirection == Direction.South)
            {
                
            }
            else
            {
                yCar = Mathf.RoundToInt(Random.Range((fieldSizeY - 1) / 2 * -1, (fieldSizeY - 1) / 2));
            }*/
            yCar = Mathf.RoundToInt(Random.Range((fieldSizeY - 1) / 2 * -1 + 1, (fieldSizeY - 1) / 2));
        }

        //If yCar = 0, yCar can't be on the place of the playerCar who is currenly always on the full right side which is 1 or more
        //In formula this is (fieldSizeX-1)/2 - 1, because this will skip the first two columns which are needed as space for the back of the player boat
        float xCar = 0;
        if (yCar == 0 || yCar == 1 && currentDirection == Direction.South || yCar == 1 && currentDirection == Direction.North)
        {
            if(currentDirection == Direction.West)
            {
                xCar = Mathf.RoundToInt(Random.Range(((fieldSizeX - 1) / 2 - 1) * -1 +1, 0));
            }
            else
            {
                xCar = Mathf.RoundToInt(Random.Range(((fieldSizeX - 1) / 2 - 1) * -1, 0));
            }
        }
        else
        {
            if(currentDirection == Direction.West)
            {
                xCar = Mathf.RoundToInt(Random.Range(((fieldSizeX - 1) / 2 - 1) * -1 + 1, (fieldSizeX - 1) / 2));
            }
            else
            {
                xCar = Mathf.RoundToInt(Random.Range(((fieldSizeX - 1) / 2 - 1) * -1, (fieldSizeX - 1) / 2));
            }
        }

        return new Vector2(xCar, yCar);
    }

    /// <summary>
    /// Function to check if the row is almost full, which could cause the puzzle to be unsolvable
    /// </summary>
    /// <param name="xPos">x position of the column that needs to be checked</param>
    /// <returns>boolean true if column has only one space or less left (two spaces left for Direction.South and Direction.North), otherwise returns false</returns>
    private bool CheckIfColumnAlmostFull(Vector2 boatFrontPos, Vector2 boatBackPos, Direction boatDirection)
    {
        /* Check if direction is east or west and in that case do an extra check just like after the if statement with the boatBackPos
         * Check if the frontPos is on the same x position as the boatFrontPos
         * Check if the backPos is on the same x position as the boat FrontPos
         * Add 1 to the amount of positions taken for every true outcome in those two
         * Compare the amount of positions taken to the available amount
         * If this number is 1 less than the available amount (or 2 less in case of boatDirections being North or South) return true
         * otherwise return false
         */
        int amountOfTakenColumnYPositionsFront = 0;
        int amountOfTakenColumnYPositionsBack = 0;
        
        for(int i = 0; i < currentAmountOfBoats-1; i++)
        {
            if(boats[i].GetComponent<BoatControl>().frontPos.x == boatFrontPos.x)
            {
                amountOfTakenColumnYPositionsFront++;
            }
            if(boats[i].GetComponent<BoatControl>().backPos.x == boatFrontPos.x)
            {
                amountOfTakenColumnYPositionsFront++;
            }
        }

        if (boatDirection == Direction.East || boatDirection == Direction.West)
        {
            for (int i = 0; i < currentAmountOfBoats-1; i++)
            {
                if (boats[i].GetComponent<BoatControl>().frontPos.x == boatBackPos.x)
                {
                    amountOfTakenColumnYPositionsBack++;
                }
                if (boats[i].GetComponent<BoatControl>().backPos.x == boatBackPos.x)
                {
                    amountOfTakenColumnYPositionsBack++;
                }
            }

            if(amountOfTakenColumnYPositionsBack >= fieldSizeY - 1)
            {
                return true;
            }
            else if (amountOfTakenColumnYPositionsFront >= fieldSizeY - 1)
            {
                return true;
            }
        }
        else
        {
            if (amountOfTakenColumnYPositionsFront >= fieldSizeY - 2)
            {
                return true;
            }
        }


        int amountOfTakenRowXPositionsFront = 0;
        int amountOfTakenRowXPositionsBack = 0;

        for (int i = 0; i < currentAmountOfBoats - 1; i++)
        {
            if (boats[i].GetComponent<BoatControl>().frontPos.y == boatFrontPos.y)
            {
                amountOfTakenRowXPositionsFront++;
            }
            if (boats[i].GetComponent<BoatControl>().backPos.y == boatFrontPos.y)
            {
                amountOfTakenRowXPositionsFront++;
            }
        }

        if (boatDirection == Direction.South || boatDirection == Direction.North)
        {
            for (int i = 0; i < currentAmountOfBoats - 1; i++)
            {
                if (boats[i].GetComponent<BoatControl>().frontPos.y == boatBackPos.y)
                {
                    amountOfTakenRowXPositionsBack++;
                }
                if (boats[i].GetComponent<BoatControl>().backPos.y == boatBackPos.y)
                {
                    amountOfTakenRowXPositionsBack++;
                }
            }

            if (amountOfTakenRowXPositionsBack >= fieldSizeX - 1)
            {
                return true;
            }
            else if (amountOfTakenRowXPositionsFront >= fieldSizeX - 1)
            {
                return true;
            }
        }
        else
        {
            if (amountOfTakenRowXPositionsFront >= fieldSizeX - 2)
            {
                return true;
            }
        }

        return false;
    }

    private Vector2 CreateRandomFirstOtherBoatPosition(Direction currentDirection)
    {
        //float yCar = Mathf.RoundToInt(Random.Range((fieldSizeY - 1) / 2 * -1 + 1, (fieldSizeY - 1) / 2));
        float yCar = Mathf.RoundToInt(Random.Range(0, 1));
        float xCar = 0;
        if (yCar == 0 || yCar == 1 && currentDirection == Direction.South || yCar == 1 && currentDirection == Direction.North)
        {
            xCar = Mathf.RoundToInt(Random.Range(((fieldSizeX - 1) / 2 - 1) * -1, 0));
        }
        return new Vector2(xCar, yCar);
    }

    /// <summary>
    /// Calculates the direction modifier, which can be used to correct the placement of boats depending on their direction
    /// </summary>
    /// <param name="currentDirection">determined direction of the boat</param>
    /// <returns>the directionModifier which is a number to correct the placement of boats corresponding with their direction</returns>
    private Vector2 CalculateDirectionModifier(Direction currentDirection/*, Vector2 position*/)
    {
        //Vector2 directionModifier = position;
        Vector2 directionModifier = new Vector2(0, 0);
        if (currentDirection == Direction.East)
        {
            directionModifier.x -= 1;
            directionModifier.y = 0;
        }
        else if (currentDirection == Direction.North)
        {
            //directionModifier.y += 1;
            directionModifier.y -= 1;
            directionModifier.x = 0;
        }
        else if (currentDirection == Direction.South)
        {
            directionModifier.y -= 1;
            directionModifier.x = 0;
        }
        else if (currentDirection == Direction.West)
        {
            directionModifier.x -= 1;
            directionModifier.y = 0;
        }
        return directionModifier;
    }

    /// <summary>
    /// Checks if a boat was hit or not
    /// </summary>
    /// <param name="foundGridPosition">the girdPosition that has been hit by the player</param>
    /// <returns>The script of the selected boat if a boat was selected, otherwise returns null</returns>
    public BoatControl FindHitBoat(Vector2 foundGridPosition)
    {
        BoatControl hitBoat = null;
        for(int i = 0; i < boats.Length; i++)
        {
            BoatControl checkCarControls = boats[i].GetComponent<BoatControl>();
            if (checkCarControls.frontPos == foundGridPosition || checkCarControls.backPos == foundGridPosition)
            {
                hitBoat = checkCarControls;
            }
        }
        return hitBoat;
    }

    /// <summary>
    /// Check if the player's position has been hit by a player
    /// </summary>
    /// <param name="foundGridPosition">the gridPosition that has been hit by the player</param>
    /// <returns>the GameObject of the boat that was hit, or null if no boat was hit</returns>
    public GameObject CheckPlayerPosition(Vector2 foundGridPosition)
    {

        GameObject hitBoat = null;
        
        Vector2[] positions = new Vector2[2];
        if (playerCar.GetComponent<PlayerBoatControl>().frontPos == foundGridPosition || playerCar.GetComponent<PlayerBoatControl>().backPos == foundGridPosition)
        {
            hitBoat = playerCar;
            positions[0] = hitBoat.GetComponent<PlayerBoatControl>().frontPos;
            positions[1] = hitBoat.GetComponent<PlayerBoatControl>().backPos;
        }
        return hitBoat;
    }

    /// <summary>
    /// Calculate the distance from frontPos and backPos of free spaces available
    /// </summary>
    /// <param name="boat">The BoatControl script of the selected boat</param>
    /// <returns>Vector2 with frontPos distance and backPos distance, values always have a positive value</returns>
    #region FindMoveTiles
    public Vector2 FindPossibleMoveTiles(BoatControl boat)
    {
        float distanceFreeSpacesFrontBoat = 0;
        float distanceFreeSpacesBackBoat = 0;
        switch(boat.currentDirection)
        {
            case Direction.North:
                if(boat.frontPos.y < Mathf.Floor(fieldSizeY / 2))
                {
                    GameObject shortestDistanceBoat = null;
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatControl>().frontPos.y > boat.frontPos.y && boats[i].GetComponent<BoatControl>().frontPos.x == boat.frontPos.x ||
                            boats[i].GetComponent<BoatControl>().backPos.y > boat.frontPos.y && boats[i].GetComponent<BoatControl>().backPos.x == boat.frontPos.x)
                        {
                            if (shortestDistanceBoat == null)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                            else
                            {
                                if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y > boats[i].GetComponent<BoatControl>().frontPos.y)
                                {
                                    shortestDistanceBoat = boats[i];
                                }
                            }

                        }
                    }

                    PlayerBoatControl player = playerCar.GetComponent<PlayerBoatControl>();
                    if (player.frontPos.y > boat.frontPos.y && player.frontPos.x == boat.frontPos.x ||
                            player.backPos.y > boat.frontPos.y && player.backPos.x == boat.frontPos.x)
                    {
                        if (shortestDistanceBoat == null)
                        {
                            shortestDistanceBoat = playerCar;
                        }
                        else
                        {
                            if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y > player.frontPos.y)
                            {
                                shortestDistanceBoat = playerCar;
                            }
                        }
                    }

                    float shortestDistance = 0;
                    if(shortestDistanceBoat != null)
                    {
                        //shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y - boat.frontPos.y;
                        if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y < shortestDistanceBoat.GetComponent<BoatControl>().backPos.y)
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y - boat.frontPos.y;
                        }
                        else
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.y - boat.frontPos.y;
                        }

                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                        shortestDistance--;
                    }
                    else
                    {
                        shortestDistance = Mathf.Floor(fieldSizeY / 2) - boat.frontPos.y;
                        if(shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                    }
                    //Debug.Log("shortest distance frontPos y = " + shortestDistance);
                    distanceFreeSpacesFrontBoat = shortestDistance;
                }
                else
                {
                    distanceFreeSpacesFrontBoat = 0;
                }
            
                if(boat.backPos.y > Mathf.Floor(fieldSizeY / 2) * -1)
                {
                    GameObject shortestDistanceBoat = null;
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatControl>().backPos.y < boat.backPos.y && boats[i].GetComponent<BoatControl>().backPos.x == boat.backPos.x ||
                            boats[i].GetComponent<BoatControl>().frontPos.y < boat.backPos.y && boats[i].GetComponent<BoatControl>().frontPos.x == boat.backPos.x)
                        {
                            if (shortestDistanceBoat == null)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                            else
                            {
                                if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y < boats[i].GetComponent<BoatControl>().frontPos.y)
                                {
                                    shortestDistanceBoat = boats[i];
                                }
                            }

                        }
                    }

                    PlayerBoatControl player = playerCar.GetComponent<PlayerBoatControl>();
                    if (player.backPos.y < boat.backPos.y && player.backPos.x == boat.backPos.x ||
                            player.frontPos.y < boat.backPos.y && player.frontPos.x == boat.backPos.x)
                    {
                        if (shortestDistanceBoat == null)
                        {
                            shortestDistanceBoat = playerCar;
                        }
                        else
                        {
                            if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y < player.frontPos.y)
                            {
                                shortestDistanceBoat = playerCar;
                            }
                        }
                    }

                    float shortestDistance = 0;
                    if (shortestDistanceBoat != null)
                    {
                        //shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.y - boat.backPos.y;
                        if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y > shortestDistanceBoat.GetComponent<BoatControl>().backPos.y)
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y - boat.backPos.y;
                        }
                        else
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.y - boat.backPos.y;
                        }

                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                        shortestDistance--;
                    }
                    else
                    {
                        shortestDistance = Mathf.Floor(fieldSizeY / 2)*-1 - boat.backPos.y;
                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                    }
                    //Debug.Log("shortest distance backPos y = " + shortestDistance);
                    distanceFreeSpacesBackBoat = shortestDistance;
                }
                else
                {
                    distanceFreeSpacesBackBoat = 0;
                }

                break;
            case Direction.South:
                if (boat.backPos.y < Mathf.Floor(fieldSizeY / 2))
                {
                    GameObject shortestDistanceBoat = null;
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatControl>().frontPos.y > boat.backPos.y && boats[i].GetComponent<BoatControl>().frontPos.x == boat.backPos.x ||
                            boats[i].GetComponent<BoatControl>().backPos.y > boat.backPos.y && boats[i].GetComponent<BoatControl>().backPos.x == boat.backPos.x)
                        {
                            if (shortestDistanceBoat == null)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                            else
                            {
                                if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y > boats[i].GetComponent<BoatControl>().frontPos.y)
                                {
                                    shortestDistanceBoat = boats[i];
                                }
                            }

                        }
                    }

                    PlayerBoatControl player = playerCar.GetComponent<PlayerBoatControl>();
                    if (player.frontPos.y > boat.backPos.y && player.frontPos.x == boat.backPos.x ||
                            player.backPos.y > boat.backPos.y && player.backPos.x == boat.backPos.x)
                    {
                        if (shortestDistanceBoat == null)
                        {
                            shortestDistanceBoat = playerCar;
                        }
                        else
                        {
                            if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y > player.frontPos.y)
                            {
                                shortestDistanceBoat = playerCar;
                            }
                        }
                    }

                    float shortestDistance = 0;
                    if (shortestDistanceBoat != null)
                    {
                        //shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y - boat.backPos.y;
                        if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y < shortestDistanceBoat.GetComponent<BoatControl>().backPos.y)
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y - boat.backPos.y;
                        }
                        else
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.y - boat.backPos.y;
                        }

                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                        shortestDistance--;
                    }
                    else
                    {
                        shortestDistance = Mathf.Floor(fieldSizeY / 2) - boat.backPos.y;
                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                    }
                    //Debug.Log("shortest distance backPos y = " + shortestDistance);
                    distanceFreeSpacesBackBoat = shortestDistance;
                }
                else
                {
                    distanceFreeSpacesBackBoat = 0;
                }

                if (boat.frontPos.y > Mathf.Floor(fieldSizeY / 2) * -1)
                {
                    GameObject shortestDistanceBoat = null;
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatControl>().backPos.y < boat.frontPos.y && boats[i].GetComponent<BoatControl>().backPos.x == boat.frontPos.x ||
                            boats[i].GetComponent<BoatControl>().frontPos.y < boat.frontPos.y && boats[i].GetComponent<BoatControl>().frontPos.x == boat.frontPos.x)
                        {
                            if (shortestDistanceBoat == null)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                            else
                            {
                                if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y < boats[i].GetComponent<BoatControl>().backPos.y)
                                {
                                    shortestDistanceBoat = boats[i];
                                }
                            }

                        }
                    }

                    PlayerBoatControl player = playerCar.GetComponent<PlayerBoatControl>();
                    if (player.backPos.y < boat.frontPos.y && player.backPos.x == boat.frontPos.x ||
                            player.frontPos.y < boat.frontPos.y && player.frontPos.x == boat.frontPos.x)
                    {
                        if (shortestDistanceBoat == null)
                        {
                            shortestDistanceBoat = playerCar;
                        }
                        else
                        {
                            if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y < player.backPos.y)
                            {
                                shortestDistanceBoat = playerCar;
                            }
                        }
                    }

                    float shortestDistance = 0;
                    if (shortestDistanceBoat != null)
                    {
                        //shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.y - boat.frontPos.y;
                        if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y > shortestDistanceBoat.GetComponent<BoatControl>().backPos.y)
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.y - boat.frontPos.y;
                        }
                        else
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.y - boat.frontPos.y;
                        }

                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                        shortestDistance--;
                    }
                    else
                    {
                        shortestDistance = Mathf.Floor(fieldSizeY / 2) * -1 - boat.frontPos.y;
                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                    }
                    //Debug.Log("shortest distance frontPos y = " + shortestDistance);
                    distanceFreeSpacesFrontBoat = shortestDistance;
                }
                else
                {
                    distanceFreeSpacesFrontBoat = 0;
                }

                break;
            case Direction.East:
                if (boat.frontPos.x < Mathf.Floor(fieldSizeX / 2))
                {
                    GameObject shortestDistanceBoat = null;
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatControl>().frontPos.x > boat.frontPos.x && boats[i].GetComponent<BoatControl>().frontPos.y == boat.frontPos.y ||
                            boats[i].GetComponent<BoatControl>().backPos.x > boat.frontPos.x && boats[i].GetComponent<BoatControl>().backPos.y == boat.frontPos.y)
                        {
                            if (shortestDistanceBoat == null)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                            else
                            {
                                if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x > boats[i].GetComponent<BoatControl>().frontPos.x)
                                {
                                    shortestDistanceBoat = boats[i];
                                }
                            }

                        }
                    }

                    float shortestDistance = 0;
                    if (shortestDistanceBoat != null)
                    {
                        if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x < shortestDistanceBoat.GetComponent<BoatControl>().backPos.x)
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x - boat.frontPos.x;
                        }
                        else
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.x - boat.frontPos.x;
                        }

                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                        shortestDistance--;
                    }
                    else
                    {
                        shortestDistance = Mathf.Floor(fieldSizeX / 2) - boat.frontPos.x;
                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                    }
                    //Debug.Log("shortest distance frontPos x = " + shortestDistance);
                    distanceFreeSpacesFrontBoat = shortestDistance;
                }
                else
                {
                    distanceFreeSpacesFrontBoat = 0;
                }

                if (boat.backPos.x > Mathf.Floor(fieldSizeX / 2) * -1)
                {
                    GameObject shortestDistanceBoat = null;
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatControl>().backPos.x < boat.backPos.x && boats[i].GetComponent<BoatControl>().backPos.y == boat.backPos.y ||
                            boats[i].GetComponent<BoatControl>().frontPos.x < boat.backPos.x && boats[i].GetComponent<BoatControl>().frontPos.y == boat.backPos.y)
                        {
                            if (shortestDistanceBoat == null)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                            else
                            {
                                if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x < boats[i].GetComponent<BoatControl>().frontPos.x)
                                {
                                    shortestDistanceBoat = boats[i];
                                }
                            }

                        }
                    }

                    float shortestDistance = 0;
                    if (shortestDistanceBoat != null)
                    {
                        if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x > shortestDistanceBoat.GetComponent<BoatControl>().backPos.x)
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x - boat.backPos.x;
                        }
                        else
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.x - boat.backPos.x;
                        }
                        
                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                        shortestDistance--;
                    }
                    else
                    {
                        shortestDistance = Mathf.Floor(fieldSizeX / 2) * -1 - boat.backPos.x;
                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                    }
                    //Debug.Log("shortest distance backPos x = " + shortestDistance);
                    distanceFreeSpacesBackBoat = shortestDistance;
                }
                else
                {
                    distanceFreeSpacesBackBoat = 0;
                }

                break;
            case Direction.West:
                if (boat.backPos.x < Mathf.Floor(fieldSizeX / 2))
                {
                    GameObject shortestDistanceBoat = null;
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatControl>().frontPos.x > boat.backPos.x && boats[i].GetComponent<BoatControl>().frontPos.y == boat.backPos.y ||
                            boats[i].GetComponent<BoatControl>().backPos.x > boat.backPos.x && boats[i].GetComponent<BoatControl>().backPos.y == boat.backPos.y)
                        {
                            if (shortestDistanceBoat == null)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                            else
                            {
                                if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x > boats[i].GetComponent<BoatControl>().backPos.x)
                                {
                                    shortestDistanceBoat = boats[i];
                                }
                            }

                        }
                    }

                    float shortestDistance = 0;
                    if (shortestDistanceBoat != null)
                    {
                        if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x < shortestDistanceBoat.GetComponent<BoatControl>().backPos.x)
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x - boat.backPos.x;
                        }
                        else
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.x - boat.backPos.x;
                        }

                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                        shortestDistance--;
                    }
                    else
                    {
                        shortestDistance = Mathf.Floor(fieldSizeX / 2) - boat.backPos.x;
                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                    }
                    //Debug.Log("shortest distance backPos x = " + shortestDistance);
                    distanceFreeSpacesBackBoat = shortestDistance;
                }
                else
                {
                    distanceFreeSpacesBackBoat = 0;
                }

                if (boat.frontPos.x > Mathf.Floor(fieldSizeX / 2) * -1)
                {
                    GameObject shortestDistanceBoat = null;
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatControl>().backPos.x < boat.frontPos.x && boats[i].GetComponent<BoatControl>().backPos.y == boat.frontPos.y ||
                            boats[i].GetComponent<BoatControl>().frontPos.x < boat.frontPos.x && boats[i].GetComponent<BoatControl>().frontPos.y == boat.frontPos.y)
                        {
                            if (shortestDistanceBoat == null)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                            else
                            {
                                if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x < boats[i].GetComponent<BoatControl>().frontPos.x)
                                {
                                    shortestDistanceBoat = boats[i];
                                }
                            }

                        }
                    }

                    float shortestDistance = 0;
                    if (shortestDistanceBoat != null)
                    {
                        if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x > shortestDistanceBoat.GetComponent<BoatControl>().backPos.x)
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x - boat.frontPos.x;
                        }
                        else
                        {
                            shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.x - boat.frontPos.x;
                        }

                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                        shortestDistance--;
                    }
                    else
                    {
                        shortestDistance = Mathf.Floor(fieldSizeX / 2) * -1 - boat.frontPos.x;
                        if (shortestDistance < 0)
                        {
                            shortestDistance *= -1;
                        }
                    }
                    //Debug.Log("shortest distance frontPos x = " + shortestDistance);
                    distanceFreeSpacesFrontBoat = shortestDistance;
                }
                else
                {
                    distanceFreeSpacesFrontBoat = 0;
                }

                break;
        }
        Vector2 FrontAndBackFreeDistances = new Vector2(distanceFreeSpacesFrontBoat, distanceFreeSpacesBackBoat);
        return FrontAndBackFreeDistances;
    }

    public Vector2 FindPossibleMoveTilesPlayer(PlayerBoatControl player)
    {
        float distanceFreeSpacesFrontBoat = 0;
        float distanceFreeSpacesBackBoat = 0;
        if(player.currentDirection == Direction.West)
        {
            if (player.backPos.x < Mathf.Floor(fieldSizeX / 2))
            {
                GameObject shortestDistanceBoat = null;
                for (int i = 0; i < boats.Length; i++)
                {
                    if (boats[i].GetComponent<BoatControl>().frontPos.x > player.backPos.x && boats[i].GetComponent<BoatControl>().frontPos.y == player.backPos.y ||
                        boats[i].GetComponent<BoatControl>().backPos.x > player.backPos.x && boats[i].GetComponent<BoatControl>().backPos.y == player.backPos.y)
                    {
                        if (shortestDistanceBoat == null)
                        {
                            shortestDistanceBoat = boats[i];
                        }
                        else
                        {
                            if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x > boats[i].GetComponent<BoatControl>().backPos.x)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                        }

                    }
                }

                float shortestDistance = 0;
                if (shortestDistanceBoat != null)
                {
                    if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x < shortestDistanceBoat.GetComponent<BoatControl>().backPos.x)
                    {
                        shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x - player.backPos.x;
                    }
                    else
                    {
                        shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.x - player.backPos.x;
                    }

                    if (shortestDistance < 0)
                    {
                        shortestDistance *= -1;
                    }
                    shortestDistance--;
                }
                else
                {
                    shortestDistance = Mathf.Floor(fieldSizeX / 2) - player.backPos.x;
                    if (shortestDistance < 0)
                    {
                        shortestDistance *= -1;
                    }
                }
                //Debug.Log("shortest distance backPos x = " + shortestDistance);
                distanceFreeSpacesBackBoat = shortestDistance;
            }
            else
            {
                distanceFreeSpacesBackBoat = 0;
            }

            if (player.frontPos.x > Mathf.Floor(fieldSizeX / 2) * -1)
            {
                GameObject shortestDistanceBoat = null;
                for (int i = 0; i < boats.Length; i++)
                {
                    if (boats[i].GetComponent<BoatControl>().backPos.x < player.frontPos.x && boats[i].GetComponent<BoatControl>().backPos.y == player.frontPos.y ||
                        boats[i].GetComponent<BoatControl>().frontPos.x < player.frontPos.x && boats[i].GetComponent<BoatControl>().frontPos.y == player.frontPos.y)
                    {
                        if (shortestDistanceBoat == null)
                        {
                            shortestDistanceBoat = boats[i];
                        }
                        else
                        {
                            if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x < boats[i].GetComponent<BoatControl>().frontPos.x)
                            {
                                shortestDistanceBoat = boats[i];
                            }
                        }

                    }
                }

                float shortestDistance = 0;
                if (shortestDistanceBoat != null)
                {
                    if (shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x > shortestDistanceBoat.GetComponent<BoatControl>().backPos.x)
                    {
                        shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().frontPos.x - player.frontPos.x;
                    }
                    else
                    {
                        shortestDistance = shortestDistanceBoat.GetComponent<BoatControl>().backPos.x - player.frontPos.x;
                    }

                    if (shortestDistance < 0)
                    {
                        shortestDistance *= -1;
                    }
                    shortestDistance--;
                }
                else
                {
                    shortestDistance = Mathf.Floor(fieldSizeX / 2) * -1 - player.frontPos.x;
                    if (shortestDistance < 0)
                    {
                        shortestDistance *= -1;
                    }
                }
                //Debug.Log("shortest distance frontPos x = " + shortestDistance);
                distanceFreeSpacesFrontBoat = shortestDistance;
            }
            else
            {
                distanceFreeSpacesFrontBoat = 0;
            }
        }
        Vector2 FrontAndBackFreeDistances = new Vector2(distanceFreeSpacesFrontBoat, distanceFreeSpacesBackBoat);
        return FrontAndBackFreeDistances;
    }

    #endregion

        /// <summary>
        /// Spawns the Free space indicators on the possible spaces to move to
        /// </summary>
        /// <param name="frontPosDistance">maxDistance of free space from frontPos</param>
        /// <param name="backPosDistance">maxDistance of free space from backPos</param>
        /// <param name="boat">BoatControl of selected boat</param>
        /// <returns>Amount of highlights that have been placed</returns>
    public int ShowFreeSpaces(float frontPosDistance, float backPosDistance, BoatControl boat)
    {
        float amountOfHighlightsNeeded = frontPosDistance + backPosDistance;
        int amountOfHighlightsPlaced = 0;
        if (amountOfHighlightsNeeded > 0)
        {
            bool frontPosDone = false;
            bool backPosDone = false;

            if (frontPosDistance <= 0)
            {
                frontPosDone = true;
            }
            if (backPosDistance <= 0)
            {
                backPosDone = true;
            }
            for (int i = 0; i < amountOfHighlightsNeeded; i++)
            {
                amountOfHighlightsPlaced++;
                switch (boat.currentDirection)
                {
                    case Direction.North:
                        if (!frontPosDone)
                        {
                            gridTileHighlights[i].transform.position = new Vector2(boat.frontPos.x, boat.frontPos.y + amountOfHighlightsPlaced);
                            if(frontPosDistance <= amountOfHighlightsPlaced)
                            {
                                frontPosDone = true;
                            }
                        }
                        else if(!backPosDone)
                        {
                            gridTileHighlights[i].transform.position = new Vector2(boat.backPos.x, boat.backPos.y - (amountOfHighlightsPlaced - frontPosDistance));
                        }
                        gridTileHighlights[i].SetActive(true);
                        break;
                    case Direction.South:
                        if (!frontPosDone)
                        {
                            gridTileHighlights[i].transform.position = new Vector2(boat.frontPos.x, boat.frontPos.y - amountOfHighlightsPlaced);
                            if (frontPosDistance <= amountOfHighlightsPlaced)
                            {
                                frontPosDone = true;
                            }
                        }
                        else if (!backPosDone)
                        {
                            gridTileHighlights[i].transform.position = new Vector2(boat.backPos.x, boat.backPos.y + (amountOfHighlightsPlaced - frontPosDistance));
                        }
                        gridTileHighlights[i].SetActive(true);
                        break;
                    case Direction.East:
                        if (!frontPosDone)
                        {
                            gridTileHighlights[i].transform.position = new Vector2(boat.frontPos.x + amountOfHighlightsPlaced, boat.frontPos.y);
                            if (frontPosDistance <= amountOfHighlightsPlaced)
                            {
                                frontPosDone = true;
                            }
                        }
                        else if (!backPosDone)
                        {
                            gridTileHighlights[i].transform.position = new Vector2(boat.backPos.x - (amountOfHighlightsPlaced - frontPosDistance), boat.backPos.y);
                        }
                        gridTileHighlights[i].SetActive(true);
                        break;
                    case Direction.West:
                        if (!frontPosDone)
                        {
                            gridTileHighlights[i].transform.position = new Vector2(boat.frontPos.x - amountOfHighlightsPlaced, boat.frontPos.y);
                            if (frontPosDistance <= amountOfHighlightsPlaced)
                            {
                                frontPosDone = true;
                            }
                        }
                        else if (!backPosDone)
                        {
                            gridTileHighlights[i].transform.position = new Vector2(boat.backPos.x + (amountOfHighlightsPlaced - frontPosDistance), boat.backPos.y);
                        }
                        gridTileHighlights[i].SetActive(true);
                        break;
                }
            }
        }
        return amountOfHighlightsPlaced;
    }

    /// <summary>
    /// Determines the spaces the player can move to from their current position and highlights them
    /// </summary>
    /// <param name="frontPosDistance">calculated maximum distance between the front of the playerBoat and a different boat or a wall</param>
    /// <param name="backPosDistance">calculated maximum distance between the front of the playerBoat and a different boat or a wall</param>
    /// <param name="player">the playerCarControl script of the player</param>
    /// <returns>the amount of highlights placed as int</returns>
    public int ShowFreeSpacesPlayer(float frontPosDistance, float backPosDistance, PlayerBoatControl player)
    {
        float amountOfHighlightsNeeded = frontPosDistance + backPosDistance;
        int amountOfHighlightsPlaced = 0;
        if (amountOfHighlightsNeeded > 0)
        {
            bool frontPosDone = false;
            bool backPosDone = false;

            if (frontPosDistance <= 0)
            {
                frontPosDone = true;
            }
            if (backPosDistance <= 0)
            {
                backPosDone = true;
            }
            for (int i = 0; i < amountOfHighlightsNeeded; i++)
            {
                amountOfHighlightsPlaced++;
                if (player.currentDirection == Direction.West)
                {
                    if (!frontPosDone)
                    {
                        gridTileHighlights[i].transform.position = new Vector2(player.frontPos.x - amountOfHighlightsPlaced, player.frontPos.y);
                        if (frontPosDistance <= amountOfHighlightsPlaced)
                        {
                            frontPosDone = true;
                        }
                    }
                    else if (!backPosDone)
                    {
                        gridTileHighlights[i].transform.position = new Vector2(player.backPos.x + (amountOfHighlightsPlaced - frontPosDistance), player.backPos.y);
                    }
                    gridTileHighlights[i].SetActive(true);
                }
            }
        }
        return amountOfHighlightsPlaced;
    }

    /// <summary>
    /// Determines if a highlight was hit by a player
    /// </summary>
    /// <param name="hitPosition">the hit gridPosition by the player</param>
    /// <param name="amountOfUsedHighlights">the amount of already active highlights</param>
    /// <returns></returns>
    public bool CheckHighlightHit(Vector2 hitPosition, int amountOfUsedHighlights)
    {
        if(amountOfUsedHighlights > amountOfHighlights)
        {
            amountOfUsedHighlights = amountOfHighlights;
        }

        bool highlightHit = false;
        for(int i = 0; i < amountOfUsedHighlights; i++)
        {
            if(gridTileHighlights[i].transform.position.x == hitPosition.x && gridTileHighlights[i].transform.position.y == hitPosition.y)
            {
                highlightHit = true;
            }
        }
        return highlightHit;
    }

    /// <summary>
    /// Hides all highlights that aren't hidden yet
    /// </summary>
    /// <param name="amountOfUsedHighlights">the amount of currently active highlights</param>
    public void HideAllHighlights(int amountOfUsedHighlights)
    {
        if(amountOfUsedHighlights > amountOfHighlights)
        {
            amountOfUsedHighlights = amountOfHighlights;
        }
        for(int i = 0; i < amountOfUsedHighlights; i++)
        {
            gridTileHighlights[i].transform.position = new Vector3(fieldSizeX, fieldSizeY, 0);
            gridTileHighlights[i].SetActive(false);
        }
    }

    /// <summary>
    /// Simple function to give access to the player object for other scripts
    /// </summary>
    /// <returns>GameObject of the player</returns>
    public GameObject GetPlayerCar()
    {
        return playerCar;
    }

    /// <summary>
    /// Disables all game elements for the end of the game
    /// </summary>
    public void SetGameInactive()
    {
        for(int i = 0; i < boats.Length; i++)
        {
            boats[i].SetActive(false);
        }

        for(int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(false);
        }

        playerCar.SetActive(false);
    }
}

