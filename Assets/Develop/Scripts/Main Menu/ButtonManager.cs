using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject menuMain, menuStart, menuLoad, menuOptions, menuCredits; // prefabs
    public Canvas canvas;
    GameObject menuElement;
    List<GameObject> activeMenuObjects;
    private void Awake()
    {
        activeMenuObjects = new List<GameObject>(); // create menu object list
        GameObject main = Instantiate<GameObject>(menuMain, canvas.transform); // instantiate main menu
        activeMenuObjects.Add(main); // add it to the active menu objects list 
        AssignEventListeners(main); // assign event listeners to the prefab's buttons
    }

    void ClearMenuElements() // Clear all menu elements that are saved in the activeMenuObjects - list
    {
        foreach(GameObject g in activeMenuObjects) // for each object in the list 
            Destroy(g); // destroy it 
        activeMenuObjects.Clear(); // clear the list
    }
   

    public void ButtonClicked(string type) 
    {
        if(type == "startGame" || type == "backToStart") 
        {
            ClearMenuElements();
            GameObject menuElement = Instantiate<GameObject>(menuStart, canvas.transform);
            activeMenuObjects.Add(menuElement);
            AssignEventListeners(menuElement);
        } else if (type == "options") // if the button clicked was "options" 
        {
            ClearMenuElements(); // Clear all menu elements
            GameObject menuElement = Instantiate<GameObject>(menuOptions, canvas.transform); // Instantiate new element
            activeMenuObjects.Add(menuElement); // Add it to the list of active menu elements
            AssignEventListeners(menuElement); // Assign event listeners to the prefab's buttons
        } else if (type == "credits")
        {
            ClearMenuElements();
            GameObject menuElement = Instantiate<GameObject>(menuCredits, canvas.transform);
            activeMenuObjects.Add(menuElement);
            AssignEventListeners(menuElement);
        } else if (type == "exit")
        {
            Application.Quit();
        } else if (type == "backToMain")
        {
            ClearMenuElements();
            GameObject menuElement = Instantiate<GameObject>(menuMain, canvas.transform);
            activeMenuObjects.Add(menuElement);
            AssignEventListeners(menuElement);
        } else if (type == "newGame") // If the user clicks "new game"
        {
            ClearMenuElements(); // Remove all menu elements
            // TODO: Start the game
            StartCoroutine(LoadScene(2));
            
        } else if (type == "loadGame")
        {
            ClearMenuElements();
            GameObject menuElement = Instantiate<GameObject>(menuLoad, canvas.transform);
            activeMenuObjects.Add(menuElement);
            AssignEventListeners(menuElement);
            // TODO: Call load - function that gets savefiles and displays buttons in UI
            // TODO: Start the game from saved position when user clicks load-button.
        }
    }
    IEnumerator LoadScene(int sceneIdx)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIdx,LoadSceneMode.Single);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    } 
    void AssignEventListeners(GameObject prefab)
    {
        foreach (Button b in prefab.GetComponentsInChildren<Button>()) // for each button element in the prefab created
        {
            switch (b.name) // check the button's name and assign event listener
            {
                case "StartGame":
                    b.onClick.AddListener(delegate { ButtonClicked("startGame"); });
                    break;
                case "Options":
                    b.onClick.AddListener(delegate { ButtonClicked("options"); });
                    break;
                case "Credits":
                    b.onClick.AddListener(delegate { ButtonClicked("credits"); });
                    break;
                case "Exit":
                    b.onClick.AddListener(delegate { ButtonClicked("exit"); });
                    break;
                case "NewGame":
                    b.onClick.AddListener(delegate { ButtonClicked("newGame"); });
                    break;
                case "LoadGame":
                    b.onClick.AddListener(delegate { ButtonClicked("loadGame"); });
                    break;
                case "BackMain":
                    b.onClick.AddListener(delegate { ButtonClicked("backToMain"); });
                    break;
                case "BackStart":
                    b.onClick.AddListener(delegate { ButtonClicked("backToStart"); });
                    break;
            }
        }
    }
}
