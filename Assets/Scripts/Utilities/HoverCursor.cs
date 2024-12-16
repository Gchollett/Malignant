using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverCursor : MonoBehaviour
{
    public Texture2D cursor;
    private void OnMouseEnter() {
        Cursor.SetCursor(cursor,Vector2.zero,CursorMode.Auto);
    }
    private void OnMouseExit() {
        Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
    }

    private void OnDestroy() {
        Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);   
    }

    private void OnDisable() {
        Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);   
    }
}
