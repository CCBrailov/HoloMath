using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using TMPro;

public class EventListener : MonoBehaviour
{
    public RightHand2 rightHand;
    public LeftHand2 leftHand;
    public ExpressionController exCon;

    public TextMeshPro debugText;
    protected string debugString = "debug";

    protected bool gripping;
    public UnityEvent squeeze;
    public UnityEvent release;

    protected TokenController leftSelect = null;
    protected TokenController rightSelect = null;
    protected TokenController inHand = null;
    protected GameObject inHandObject = null;

    protected bool holding = false;
    
    [SerializeField]
    protected bool dragging = false;
    [SerializeField]
    protected float dragDist = 0;

    protected bool stretching = false;
    protected float stretchDist = 0;
    protected float stretchStart = 0;

    protected List<int> lawfulPlacements;
    protected List<TokenController> lawfulSwaps;

    public AudioClip pop1;
    public AudioClip pop2;

    void Awake()
    {
        lawfulPlacements = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetListeners();
    }

    // Update is called once per frame
    void Update()
    {
        if(!rightHand.GripDown && !leftHand.GripDown)
        {
            ResetListeners();
        }

        ColorTokens();

        if (dragging)
        {
            dragDist = Vector3.Distance(rightSelect.gameObject.transform.position, rightHand.gameObject.transform.position);
            debugString = "Dragging " + dragDist;
            if (dragDist > 0.15f)
            {
                EndDrag();
                PickUp(rightSelect);
            }
        }

        if (stretching)
        {
            stretchDist = Vector3.Distance(leftHand.gameObject.transform.position, rightHand.gameObject.transform.position);
            debugString = "Stretching " + stretchDist/stretchStart;
            bool operandsAreNeighbors = (Mathf.Abs(exCon.tokenControllers.IndexOf(rightSelect) - exCon.tokenControllers.IndexOf(leftSelect))) == 2;
            if (stretchDist / stretchStart > 3
                && rightSelect == leftSelect)
            {
                rightSelect.Expand();
                rightHand.GetComponent<AudioSource>().clip = pop2;
                rightHand.GetComponent<AudioSource>().Play();
                EndStretch();
            }
            else if (stretchDist / stretchStart < 0.5 
                && operandsAreNeighbors)
            {
                TokenController op = exCon.tokenControllers[exCon.tokenControllers.IndexOf(rightSelect) - 1];
                op.Simplify();
            }
        }

        debugText.SetText(debugString);
    }

    void ColorTokens()
    {
        List<TokenController> controllers = exCon.tokenControllers;
        foreach (TokenController tc in controllers)
        {
            tc.textMesh.color = new(255, 255, 255);
            tc.textMesh.fontSize = 1;
        }

        if(rightHand.hoveredElements.Count > 0)
        {
            rightHand.hoveredElements[0].textMesh.color = new(0, 0, 255);
        }

        if(rightSelect != null)
        {
            rightSelect.textMesh.fontSize = 1.5f;
        }

        if(leftSelect != null)
        {
            leftSelect.textMesh.color = new(255, 0, 0);
        }

        foreach(TokenController tc in exCon.tokenControllers)
        {
            if (lawfulPlacements.Count > 0 
                && !lawfulPlacements.Contains(controllers.IndexOf(tc)))
            {
                tc.textMesh.alpha = 0.3f;
            }
        }
    }

    void PickUp(TokenController tc)
    {
        rightHand.GetComponent<AudioSource>().clip = pop2;
        rightHand.GetComponent<AudioSource>().Play();

        rightSelect.Hide();
        exCon.AddParentheses();
        lawfulPlacements = exCon.expression.LawfulPlacements(rightSelect.token);
        
        inHandObject = Instantiate(exCon.tokenPrefab, rightHand.gameObject.transform.position, rightSelect.gameObject.transform.rotation, rightHand.gameObject.transform);
        inHand = inHandObject.GetComponent<TokenController>();
        inHand.token = rightSelect.token;
        inHand.textMesh.color = new(255, 255, 255);
        inHandObject.transform.localScale = new(20, 20, 20);
        inHandObject.GetComponent<BoxCollider>().enabled = false;
        holding = true;
    }

    void Drop()
    {
        lawfulSwaps = new();
        foreach(int i in lawfulPlacements)
        {
            lawfulSwaps.Add(exCon.tokenControllers[i]);
        }

        if(inHandObject != null)
        {
            Destroy(inHandObject);
            rightSelect.Show();
        }

        if (rightHand.hovering
            && rightSelect.token is Operand
            && rightHand.hoveredElements[0].token is Operand
            && lawfulSwaps.Contains(rightHand.hoveredElements[0]))
        {
            exCon.SwapTokenControllers(rightSelect, rightHand.hoveredElements[0]);
        }

        lawfulPlacements.Clear();
        exCon.RemoveParentheses();

        rightSelect = null;
        inHandObject = null;
        inHand = null;
        holding = false;
        EndDrag();
    }

    void StartDrag()
    {
        rightHand.GetComponent<AudioSource>().clip = pop1;
        rightHand.GetComponent<AudioSource>().Play();
        leftHand.squeeze.AddListener(LeftSqueeze);
        dragging = true;
    }

    void EndDrag()
    {
        debugString = "End Drag";
        leftHand.squeeze.RemoveListener(LeftSqueeze);
        dragging = false;
        dragDist = 0;
    }

    void StartStretch()
    {
        leftHand.release.AddListener(EndStretch);
        rightHand.release.AddListener(EndStretch);
        EndDrag();
        stretching = true;
        if (leftHand.hovering)
        {
            leftSelect = leftHand.hoveredElements[0];
            stretchStart = Vector3.Distance(leftHand.gameObject.transform.position, rightHand.gameObject.transform.position);
            stretchDist = stretchStart;
        }
        else
        {
            EndStretch();
        }
    }

    void EndStretch()
    {
        debugString = "End Stretch";
        stretching = false;
        stretchDist = 0;
        ResetListeners();
    }

    #region Default Listeners
    void ResetListeners()
    {
        rightHand.squeeze.RemoveAllListeners();
        rightHand.release.RemoveAllListeners();
        rightHand.trigger.RemoveAllListeners();
        rightHand.triggerUp.RemoveAllListeners();

        leftHand.squeeze.RemoveAllListeners();
        leftHand.release.RemoveAllListeners();

        rightHand.squeeze.AddListener(RightSqueeze);
        rightHand.release.AddListener(RightRelease);
        rightHand.trigger.AddListener(RightTrigger);
    }
    void RightTrigger()
    {
        Debug.Log("Trigger!");
        if (rightHand.hovering)
        {
            rightHand.hoveredElements[0].Simplify();
            rightHand.hoveredElements.Clear();
        }
    }
    void LeftSqueeze()
    {
        Debug.Log("[L] Squeeze!");
        EndDrag();
        StartStretch();
    }
    void LeftRelease()
    {
        Debug.Log("[L] Release!");
        EndStretch();
    }
    void RightSqueeze()
    {
        Debug.Log("[R] Squeeze!");
        if (rightHand.hovering && rightHand.hoveredElements[0].token is Operand)
        {
            rightSelect = rightHand.hoveredElements[0];
            StartDrag();
        }
    }
    void RightRelease()
    {
        Debug.Log("[R] Release!");
        dragDist = 0;
        Drop();
    }
    #endregion
}
