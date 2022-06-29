using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquationViewController : MonoBehaviour
{
    public Equation equation;
    public List<TokenViewController> leftTokenViews;
    public GameObject tokenPrefab;

    public void BuildTokenViews()
    {
        DestroyTokenViews();
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

    protected void PositionTokenViews()
    {
        foreach (TokenViewController t in leftTokenViews)
        {
            Vector3 position = new(leftTokenViews.IndexOf(t) * 3, 0, 0);
            t.gameObject.transform.position = position;
        }
    }

    protected void DestroyTokenViews()
    {
        foreach(TokenViewController t in leftTokenViews)
        {
            Destroy(t.gameObject);
        }
        leftTokenViews.Clear();
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
        PositionTokenViews();
    }
}