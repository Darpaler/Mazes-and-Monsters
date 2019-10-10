using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    // The target node
    public bool isRunning;

    // Start is called before the first frame update
    public virtual void Start() { }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isRunning)
        {
            LookAndAnimate();
            Move();
        }
    }

    public virtual IEnumerator RunRace() { yield return null; }

    public virtual IEnumerator CalculatePath() { yield return null; }
    protected virtual IEnumerator OnAddObstacle() { yield return null; }
    protected virtual void LookAndAnimate() { }
    protected virtual void Move() { }
}
