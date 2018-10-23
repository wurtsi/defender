using UnityEngine;
using System.Collections.Generic;

public class MobileInput : MonoBehaviour
{
    public PlayerController player;
    private List<int> currentTouches = new List<int>();

    public void Update()
    {
        if (Input.touchCount < 1)
        {
            player.NoInput();
            return;
        }

        Touch primaryTouch = new Touch();
        Touch secondaryTouch = new Touch();

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                currentTouches.Add(touch.fingerId);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (currentTouches.Count > 1 && touch.fingerId == currentTouches[1])
                    secondaryTouch = touch;

                currentTouches.Remove(touch.fingerId);
            }

            if (touch.fingerId == currentTouches[0])
                primaryTouch = touch;
            else if (touch.fingerId == currentTouches[1])
                secondaryTouch = touch;
        }

        Ray ray = Camera.main.ScreenPointToRay(primaryTouch.position);
        RaycastHit hit;



        if (primaryTouch.phase == TouchPhase.Moved || primaryTouch.phase == TouchPhase.Stationary)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.touchCount > 1)
                { 
                    if (secondaryTouch.phase == TouchPhase.Ended)
                        MultiTouchUp();
                    else
                        MultiTouchDown();
                }
                else
                    TouchDrag(hit);              
            }
        }
    }

    private void TouchDrag(RaycastHit hit)
    {
        player.Accelerate(hit.point);
    }

    private void MultiTouchDown()
    {
        player.NoInput();
        player.Shoot();
    }

    private void MultiTouchUp()
    {
        
    }
}

