/*
 Simon Lager
 2014-08-19
 00:44
 */
using UnityEngine;
using System.Collections;
using System.IO;
[ExecuteInEditMode]
public class SubtitleReader : MonoBehaviour {

    //file name of textfile
    public string fileName;
    // array of timers
    public float[] textTimer;
     //temporary text to display
    private string tempText;
    //store all the subtitle
    private string[] text;
    //checks if you need to display subtitle
    private bool checker;
    //counts thru the arrays
    private int counter;
    StreamReader subtitle = null;

    public void Awake() {
     //have same array as timer
    text = new string[textTimer.Length];
    counter = 0;
    }
    public void Start() {
        //checks so user types text file
        if (fileName == null)
        {
            Debug.LogError("Please type the name of the text file");
        }

        //reads the file and store it in a array
        subtitle = new StreamReader("Assets/" + fileName);
        for (int i = 0; i < text.Length; i++)
        {
            text[i] = subtitle.ReadLine();
        }
    }
    public void Update()
    {
        CheckTimer();
    }

    // counts down in game time secunds
    void CheckTimer()
    {
      if(textTimer[counter] > 0){
          textTimer[counter] -= Time.deltaTime;
      }
    //if the timer hits 0
      if(textTimer[counter]  <= 0){
          textTimer[counter] = 0;
          DisplaySubtitle();
      }
    }
    //displays the subtitle
    void DisplaySubtitle()
    {
        checker = true;
        tempText = text[counter];
        //checks how manny rows the array has left
        if (counter < textTimer.Length -1 )
        {
            // adds to the counter
            counter++;
            //returns to countdown
            return;
        }
        else {
            //what to do if its done
            // add code to move to next cutscen ore next level

            Debug.Log("Done");
        }
    }
    public void OnGUI()
    {
        if (checker)
        {
            //displays the text box
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height / 15), tempText);
        }
    }

}

