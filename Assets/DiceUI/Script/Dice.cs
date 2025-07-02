using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dice : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject dice; //dice object
    Animator anim; //animator object
    public TMP_Text result; // number is shown in canvas => dice => textresult
    public int randomNumber; //number that is chosen by a program randomly
    private string randomNumber2; // the same number is converted to the text form
    
    void Start()
    {
      
      result = gameObject.GetComponent<TMP_Text>();
      dice = gameObject.GetComponent<GameObject>();
      anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {        
      if(Input.GetMouseButtonDown(0)) //if mouse button is clicked does action below
      {
          randomNumber = Random.Range(1, 21);   //range of numbers on the dice is 1-20
          randomNumber2 = randomNumber.ToString(); // number is converted to the text form
          result.text = randomNumber2; // number is shown in canvas => dice => textresult     
      }
    }
}
