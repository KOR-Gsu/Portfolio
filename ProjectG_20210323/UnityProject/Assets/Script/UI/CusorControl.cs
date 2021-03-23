using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorControl : MonoBehaviour
{
	private LayerMask layerMask;
    private Define.CursorType cursorTpye = Define.CursorType.None;
    private Texture2D defaultImage;
    private Texture2D attackImage;

    void Start()
    {
		layerMask = (int)Define.Layer.Enemy | (int)Define.Layer.Ground;
		defaultImage = Managers.Resource.Load<Texture2D>("Texture/Cursor/Default");
        attackImage = Managers.Resource.Load<Texture2D>("Texture/Cursor/Attack");
	}

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, layerMask))
		{
			if (hit.collider.gameObject.layer == (int)Define.Layer.Enemy)
			{
				if (cursorTpye != Define.CursorType.Attack)
				{
					Cursor.SetCursor(attackImage, new Vector2(attackImage.width / 3, 0), CursorMode.Auto);
					cursorTpye = Define.CursorType.Attack;
				}
			}
			else
			{
				if (cursorTpye != Define.CursorType.Default)
				{
					Cursor.SetCursor(defaultImage, new Vector2(defaultImage.width / 5, 0), CursorMode.Auto);
					cursorTpye = Define.CursorType.Default;
				}
			}
		}
	}
}
