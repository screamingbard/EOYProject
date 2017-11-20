using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------
//This code makes the backgroudn birds spawn and fly across the screen.
//The more points that have been reached the more birds will be flying across the 
//sky. 
//
//This script should be placed on the parent object that is the 
//parent of all of the "respawn points". 
//----------------------------------------------------------------------------------
public class BirdbackgroundSummon : MonoBehaviour {

    //----------------------------------------------------------------------------------
    //Allows for a player tag to be defined by one script rather than 
    //the many scrips from the plaeyr detect script.
    //----------------------------------------------------------------------------------
    public string playerTag;

    //------------------------------------------------------------------------------------------------------
    //This Checks which Spawn point is currently Selected, 
    //this variable should only increment depending on which spawn point was selected last.
    //------------------------------------------------------------------------------------------------------
    uint CurrentSelectedSpawnCount = 0;

    //------------------------------------------------------------------------------------------------------
    //This object is used for modifying the height depending on the size of the bird object
    //------------------------------------------------------------------------------------------------------
    public float objheight = 1;

    //------------------------------------------------------------------------------------------------------
    //This variable simply holds the bird prefab that will be instanciated and will continue to cycle.
    //------------------------------------------------------------------------------------------------------
    public GameObject birdPrefab;

    //------------------------------------------------------------------------------------------------------
    //This holds the current selected bird.
    //------------------------------------------------------------------------------------------------------
    private List<birdus> birdsStore = new List<birdus>();

    //------------------------------------------------------------------------
    //This offset variable makes the next bird spawn just below the last bird.
    //------------------------------------------------------------------------
    public float Offset;

    //----------------------------------------------------------------------------------
    //Privatly stores a variable that should not be modified by the user of the code.
    //----------------------------------------------------------------------------------
    private float StoreOffset;

    //------------------------------------------------------------------------------------------------------
    //This is the starting point that the bird will spawn at then teleport back to after reaching the end.
    //------------------------------------------------------------------------------------------------------
    public GameObject Start;

    //------------------------------------------------------------------------------------------------------
    //This is the minimum speed of the roaming birds.
    //------------------------------------------------------------------------------------------------------
    [Range(0, float.MaxValue)]
    public float minSpeed = 15;

    //------------------------------------------------------------------------------------------------------
    //This is the maximum speed of the roaming birds.
    //------------------------------------------------------------------------------------------------------
    [Range(0, float.MaxValue)]
    public float maxSpeed = 16;

    //------------------------------------------------------------------------------------------------------
    //This end point limits how far the bird is able to fly,
    //When reached the bird will teleport back to the start point so that 
    //recreation of the bird will not be required.
    //------------------------------------------------------------------------------------------------------
    public GameObject End;

    void Awake()
    {
        //This small calculation makes sure that the initial start value is not forgotten
        Offset = Start.transform.position.y;
        StoreOffset = Offset;
        
        //This Creates a bird at the beginning
        AddBird();
    }
    
    //------------------------------------------------------------------------------------------------------
    //This is the update function which allows the birds to move using deltaTime.
    //------------------------------------------------------------------------------------------------------
    void Update()
    {
        //Makes the birds keep flying and resetting when required
        CycleBird();
    }

    //------------------------------------------------------------------------------------------------------
    //This method makes the bird actually move,
    //This cycles through all of the objects stored in the birdStore list and makes sure that they move
    //and reset when they get too far.
    //------------------------------------------------------------------------------------------------------
    void CycleBird()
    {
            //The loop that cycles through all of the variables in birdStore
            foreach (birdus go in birdsStore)
            {
            //makes the bird move based on a speed and deltaTime
                go.Chicken.transform.position += new Vector3(go.speed * Time.deltaTime, 0, 0);

                //This if statement makes sure that if the bird is moving towards the right it will keep looping in order to move to the right.
                if (go.Chicken.transform.position.x > End.transform.position.x)
                    go.Chicken.transform.position = new Vector3(Start.transform.position.x, go.Chicken.transform.position.y, go.Chicken.transform.position.z);

                //This if statement makes sure that if the bird is moving towards the left it will keep looping in order to move to the left.
                if (go.Chicken.transform.position.x < Start.transform.position.x)
                    go.Chicken.transform.position = new Vector3(End.transform.position.x, go.Chicken.transform.position.y, go.Chicken.transform.position.z);
            }
    }

    //------------------------------------------------------------------------------------------------------
    //This method creates a bird every time it is called also adding it to a list.
    //This is the key function for this script.
    //------------------------------------------------------------------------------------------------------
    public void AddBird()
    {
        //This if statement checks if the scene has created any bird objects, and if there was none it creates one
        if (birdsStore.Count > 0)
        {
            //creates a temp gameobject to store the bird before putting it into the list
            birdus temp;

            //offsets the birds location by a number to make sure the birds dont spawn on eachother
            Offset = StoreOffset - (CurrentSelectedSpawnCount * objheight);

            //puts the instantiated bird into the variable and changes its variables for usage
            temp.Chicken = Instantiate(birdPrefab, Start.transform.position - new Vector3(0, Start.transform.position.y - Offset, 0), Quaternion.identity);

            //Adds the temporary object to the birdStore list
            temp.speed = RandomSpeedGen();

            //adds the bird object to a list of bird gameobjects
            birdsStore.Add(temp);
            
            //increments the last selected point
            CurrentSelectedSpawnCount++;
        }
        
        //Must be placed below in order to make sure that the above if statement does not activate after a variable was changed
        if (birdsStore.Count <= 0)
        {
            //Temporary variable that stores which 
            birdus temp;

            //temp variable that stores the instantiated object
            temp.Chicken = Instantiate(birdPrefab, Start.transform.position, Quaternion.identity);

            //Sets the speed of the next created birds
            temp.speed = RandomSpeedGen();

            //Sets the speed of the first created bird
            birdsStore.Add(temp);

            //increments the last selected point
            CurrentSelectedSpawnCount++;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------
    //This method controls the speed of the roaming birds by randomly generating a number between the maximum and minimum.
    //Safety measures are put in.
    //--------------------------------------------------------------------------------------------------------------------
    float RandomSpeedGen()
    {
        //To make sure that the speed variable will not be overly broken
        //This changes the minimum speed to be the maximum if the minimum variable is larger than the maximum
        if (minSpeed > maxSpeed)
        {
            //temporarily stores the minimum speed for switching
            float temp = minSpeed;
        
            //Switches the min and max to the desired values
            minSpeed = maxSpeed;
            maxSpeed = temp;
        }
        
        //Randomly generates a number based on a range
        float speed = Random.Range(minSpeed, maxSpeed);
        
        //returns the speed
        return speed;
    }
}

//------------------------------------------------------
//holds the gameobject and speed of the created birds.
//------------------------------------------------------
struct birdus
{
    public GameObject Chicken;
    public float speed;
}
