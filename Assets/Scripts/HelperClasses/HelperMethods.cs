using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    public static bool GetComponentsAtBoxLocation<T>(out List<T> listComponentsAtBoxPosition, Vector2 point, Vector2 size, float angle) {

        bool found = false;
        List<T> componentList = new List<T>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(point, size, angle);

        for (int i = 0; i < collider2DArray.Length; i++) {

            T tComponent = collider2DArray[i].gameObject.GetComponentInParent<T>();
            if (tComponent!= null) {
                
                found = true;
                componentList.Add(tComponent);
            }
            else {

                tComponent = collider2DArray[i].gameObject.GetComponentInChildren<T>();
                if (tComponent!= null) {
                    
                    found = true;
                    componentList.Add(tComponent);
                }
            }
        }

        listComponentsAtBoxPosition = componentList;
        
        return found;
    }
}
