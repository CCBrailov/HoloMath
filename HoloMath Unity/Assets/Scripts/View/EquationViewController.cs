using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquationViewController : MonoBehaviour
{
    public Equation equation;
    public List<TokenViewController> leftTokenViews;
    public GameObject tokenPrefab;

    protected void BuildTokenViews()
    {
        leftTokenViews = new List<TokenViewController>();
        foreach (Token t in equation.leftSide)
        {
            Vector3 position = new(equation.leftSide.IndexOf(t) * 3, 0, 0);
            GameObject tokenView = Instantiate(tokenPrefab, position, Quaternion.identity, this.transform);
            TokenViewController controller = (TokenViewController)tokenView.GetComponent("TokenViewController");
            controller.token = t;
            controller.equationView = this;
            leftTokenViews.Add(controller);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        equation = new Equation();
        leftTokenViews = new List<TokenViewController>();
        BuildTokenViews();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
