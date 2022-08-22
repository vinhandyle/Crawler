using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : Singleton<CustomCursor>
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D interactCursor;
    [SerializeField] private Texture2D interactCursor2;

    public void SetCursor(int type)
    {
        Vector2 hotspot = new Vector2();

        switch (type)
        {
            case 0:
                Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
                break;

            case 1:
                Cursor.SetCursor(interactCursor, hotspot, CursorMode.Auto);
                break;

            case 2:
                Cursor.SetCursor(interactCursor2, hotspot, CursorMode.Auto);
                break;
        }
    }
}
