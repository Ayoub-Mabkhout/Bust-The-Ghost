using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private int height = 8, width = 20;
    private double xGhost, yGhost;
    public static bool bust = false;
    GameObject[,] tiles;
    double[,] probabilities;


    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid(){

        GameObject referenceTile = (GameObject) GameObject.Find("RefTile");
        Vector3 tileSize = referenceTile.transform.localScale;
        tiles = new GameObject[height,width];
        probabilities = new double[height,width];


        for(int row = 0; row < height; row++){
            for(int col = 0; col < width; col++){
                GameObject tile = (GameObject) Instantiate(referenceTile,transform);
                float x = col * tileSize.x;
                float y = row * tileSize.y;

                tile.transform.position = new Vector2(x,y);

                tiles[row,col] = tile;
                Tile s = tile.GetComponent<Tile>();
                s.n_x = col;
                s.n_y = row;
                // Debug.Log(tileObject.GetSprite().transform.position);

            }
        }
        
        float gridHeight = height * tileSize.y;
        float gridWidth = width * tileSize.x;
        // transform.position = new Vector2((-gridWidth + 2*tileSize.x)/2 , (-gridHeight + 2*tileSize.y)/2);
        transform.position = new Vector2((-gridWidth+tileSize.x)/2,(-gridHeight)/2 + 1);

        // Set Ghost Location
        // xGhost = tiles[0,0].transform.position.x;
        // yGhost = tiles[0,0].transform.position.y;
        PlaceGhost();

        // Set initial probabilities
        double probability = (double) 1/(height*width);
        
        for(int row = 0; row < height; row++){
            for(int col = 0; col < width; col++){
                GameObject tile = tiles[row,col];
                probabilities[row,col] = (double) probability;
                double probabilityText = probability;
                // Debug.Log(probabilities[row,col]);
                tile.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text =  probabilityText.ToString("P");
            }
        }
    }

	private void PlaceGhost(){
	
		int xGhostTile = UnityEngine.Random.Range(0, 20);
		int yGhostTile = UnityEngine.Random.Range(0, 8);
        GameObject tile = tiles[yGhostTile,xGhostTile];
        xGhost = tile.transform.position.x;
        yGhost = tile.transform.position.y;
		Debug.Log("Ghost Position: (" +xGhostTile+ "," +yGhostTile+")");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            // Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            GameObject tile = GetClickedTile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(!(tile is null))
            {

                if(bust)
                    BustTheGhost(tile);
                
                else{
                    if(!(tile.GetComponent<Tile>().clicked)){
                        SelectNewTile(tile);
                        UpdateGrid(tile);
                        tile.GetComponent<Tile>().clicked = true; // to avoid being able to clic on a tile more than once
                    }
                }
            }
        }
    }

    

    private void SelectNewTile(GameObject tile){

        float Xcoordinate = tile.transform.position.x;
        float Ycoordinate = tile.transform.position.y;

        float distanceX = (float) Math.Abs(Xcoordinate - xGhost);
        float distanceY = (float) Math.Abs(Ycoordinate - yGhost);

        float distanceFromGhost = distanceX + distanceY;

        SpriteRenderer spr = tile.GetComponent<SpriteRenderer>();

        double sensedDistance = Proba.nextGaussian(distanceFromGhost);
        //Debug.Log(distanceFromGhost.ToString("F") + " vs " + sensedDistance.ToString("F"));

        spr.color = getColorFromDistance((int) Math.Round(sensedDistance,0));
        TextMesh  tileText = tile.transform.GetChild(0).gameObject.GetComponent<TextMesh>();

    }


    public void UpdateGrid(GameObject selectedTile){

        double sum = 0;
        double distance1=-999;
        double distance2=-999;
        Color tileColor = selectedTile.GetComponent<SpriteRenderer>().color;
        Color orange = new Color(1, 0.843f, 0, 1);



        for(int row = 0; row < height; row++){
            for(int col =0; col<width; col++){
                
                GameObject tile = tiles[row,col];

                String probabilityText = tile.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text;
                double priorProbability = probabilities[row,col]; 
                
                // get distance between current tile and selected tile
                double distanceX = Math.Abs(tile.transform.position.x - selectedTile.transform.position.x);
                double distanceY = Math.Abs(tile.transform.position.y - selectedTile.transform.position.y);
                double distanceFromSelectedTile = distanceX + distanceY;

                // infer a range of distances between the current tile and the ghost
                // from the selected tile's color and from the distance calculated above
                if(tileColor == Color.green){
                    if(distanceFromSelectedTile < 4.51f){
                        distance1 = distanceFromSelectedTile - 6.59f; //very high
                        distance2 = distanceFromSelectedTile - 4.51f;
                    }
                }
                if(tileColor == Color.yellow){
                    distance1 = distanceFromSelectedTile - 4.49f;
                    distance2 = distanceFromSelectedTile - 2.51f;
                }
                if(tileColor == orange){
                    distance1 = distanceFromSelectedTile - 2.49f;
                    distance2 = distanceFromSelectedTile - 0.51f;
                }
                if(tileColor == Color.red){
                    distance1 = distanceFromSelectedTile - 0.49f;
                    distance2 = distanceFromSelectedTile - 0f;
                }

                // Error Checking
                if(!(distance1 == -999 || distance2 == -999)){

                    // Compute the probability of Ghost in current cell given the new approximated
                    // range of distances between the current cell and the ghost cell
                    double p_Col_Dist = 2 * Proba.MonteCarloProbability(distance1, distance2, 0);
                    // Debug.Log("P_Col_Dist for tile ("+row+","+col+") is " + p_Col_Dist+"+\n
                    //For parameters "+distance1+" and " + distance2);
                    

                    // updating the probability table (before normalization)
                    probabilities[row,col] *= p_Col_Dist; // prior probability * probability of color 
                                                          //given (approximate) distance from ghost
                }
                // keep count of the sum of probabilities in order to normalize in the next step
                
                sum += probabilities[row,col];
                // Debug.Log("sum is "+sum);
            }
        }

        // Normalization
            for(int row = 0; row < height; row++){
                for(int col = 0; col < width; col++){
                    double sum2 = 0;
                    probabilities[row,col] /= sum;
                    foreach(double prob in probabilities)
                        sum2 += prob;
                    // Debug.Log("sum is " + sum + " and sum2 is" + sum2);
                    // updating displayed probability text
                    GameObject tile = tiles[row,col];
                    double probabilityText = probabilities[row,col];
                    tile.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = probabilityText.ToString("P");
                }
            }        
    }
    

    public void BustTheGhost(GameObject tile){
            
        double Xcoordinate = tile.transform.position.x;
        double Ycoordinate = tile.transform.position.y;

        GameObject canvas = GameObject.Find("Canvas");
        GameObject popUp = canvas.transform.GetChild(1).gameObject;
        popUp.SetActive(true);
        GameObject box = popUp.transform.GetChild(0).gameObject;
        Text popUpText = box.transform.GetChild(0).gameObject.GetComponent<Text>();
        if(Xcoordinate == xGhost && Ycoordinate == yGhost)
        {
            print("WON! You busted the Ghost.");
            popUpText.text = "WON! You busted the Ghost.";
        }
        else
        {
            print("LOST! This is not the ghost cell.");
            popUpText.text = "LOST! This is not the ghost cell.";
        }

        bust = false;
        }


    public GameObject GetClickedTile(Vector3 mousePos) { 
        foreach(GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            if (t.isClicked(mousePos))
                return tile;
        }
        return null;
    }

    public Color getColorFromDistance(int distance) {

        switch (distance)
        {
            case 0:
                return Color.red;
            case 1:
            case 2:
                return new Color(1, 0.843f, 0, 1);
            case 3:
            case 4:
                return Color.yellow;
            default:
                return Color.green;

        }

    }
}
