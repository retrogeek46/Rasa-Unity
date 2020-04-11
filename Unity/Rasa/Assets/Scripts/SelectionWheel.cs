using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionWheel : MonoBehaviour {

    [Header("SelectionWheel GameObjects & References")]
    public GameObject       selectionWheel;         // reference for selection wheel gameobject
    public GameObject[]     Pokemons;               // array of Pokemon's
    public GameObject[]     Slots;                  // array of slots
    public GameObject[]     gameObjectsToHide;      // array of gameObjects which are to be hidden
    public NetworkManager   networkManager;         // reference to network manager
    
    [Header("SelectionWheel Parameters")]
    public float            sensitivity;            // how smooth the rotation animation is
    
    [HideInInspector]
    public bool             wheelRotated = true;    // bool to check if wheel has been rotated
    public static int       currentSlot;            // current slot for selection wheel
    
    private string[]        pokemonNames = new string[5] {
        "Charizard",
        "Rayquaza",
        "Geodude",
        "Lapras",
        "Pikachu"
    };
    private Quaternion[]    validPositions = new Quaternion[5] {
            Quaternion.Euler(90, 180, 0),
            Quaternion.Euler(90, 252, 0),
            Quaternion.Euler(90, 324, 0),
            Quaternion.Euler(90, 36, 0),
            Quaternion.Euler(90, 108, 0)
    };
    private bool            isRotating = false;     // bool to check if selection wheel is rotating
    
    // Start is called before the first frame update
    void Start () {
        // align Pokemon sprites with slots on selection wheel
        // and init wheel variables
        UpdatePokemonTransforms ();
        wheelRotated = true;
        currentSlot = 0;

        // restart bot when app is started so that previous 
        // conversations are overwritten
        networkManager.SendMessageToRasa("/restart");
    }

    // Update is called once per frame
    void Update () {
        if (wheelRotated) {
            UpdatePokemonTransforms();
        }
    }

    /// <summary>
    /// This method updates the Pokemon transforms so that they are lined up with 
    /// the selection wheel.
    /// </summary>
    public void UpdatePokemonTransforms () {
        for (int i = 0; i < 5; i++) {
            Pokemons[i].transform.position = Slots[i].transform.position;
        }
        if (!isRotating) {
            wheelRotated = false;
        }   
    }

    /// <summary>
    /// This method rotates the selection wheel
    /// </summary>
    /// <param name="direction">the direction in which the wheel should turn. 0 for left, 1 for right</param>
    public void RotateWheel (int direction) {
        // rotate the wheel if not already rotating
        // TODO: make it animate
        if (!isRotating) {
            if (direction == 0) {
                if (currentSlot == 0) {
                    currentSlot = 5;
                }
                Quaternion rotation = validPositions[--currentSlot];
                StartCoroutine(RotateSelectionWheel(rotation));
                //selectionWheel.transform.rotation = rotation;
            } else {
                if (currentSlot == 4) {
                    currentSlot = -1;
                }
                Quaternion rotation = validPositions[++currentSlot];
                StartCoroutine(RotateSelectionWheel(rotation));
                //selectionWheel.transform.rotation = rotation;
            }
        }

        // set rotate wheel bool to true
        wheelRotated = true;
    }
    
    /// <summary>
    /// This method sends greet message to bot based on selected pokemon
    /// </summary>
    public void SelectPokemon () {
        // when this button is pressed bot UI comes up and user can talk to it

        // hide buttons, show pokemon in spotlight, shrink selection wheel
        foreach (GameObject gameObjectToHide in gameObjectsToHide) {
            gameObjectToHide.SetActive(false); 
        }
        Pokemons[currentSlot].SetActive(true);

        // show bot UI and send message to bot
        BotUI.botUIActive = true;
        networkManager.SendMessageToRasa("/greet{\"selected_pokemon\":\"" + pokemonNames[currentSlot] + "\"}");
    }

    /// <summary>
    /// This coroutine rotates the selection wheel to current slot value
    /// </summary>
    /// <param name="targetRotation">the quaternion for correct rotation</param>
    /// <returns></returns>
    private IEnumerator RotateSelectionWheel (Quaternion targetRotation) {
        isRotating = true;
        //Debug.Log("target rotation is : " + targetRotation.eulerAngles);
        while (selectionWheel.transform.rotation != targetRotation) {
            selectionWheel.transform.rotation = Quaternion.RotateTowards(selectionWheel.transform.rotation, targetRotation, sensitivity);
            yield return new WaitForSeconds(0.01f);
        }
        isRotating = false;
    }
}
