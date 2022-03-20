using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // camera used for knowing if we are out of bounds
    [SerializeField] private Camera m_Camera;
    //neighbour gameobject
    [SerializeField] private GameObject m_Neighbour;

    // should we be placed after the neighbor?
    [SerializeField] private bool m_PlaceAfterNeighbour;

    // Speed used for scrolling the image
    [SerializeField] private float m_scrollingSpeed;

    [SerializeField] private float m_TextureHalfWidth;


    // Start is called before the first frame update
    void Start()
    {
        if(m_PlaceAfterNeighbour)
        {
            PlaceAfterNeighbour();
        }

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(renderer)
        {
            float pixelsPerUnit = renderer.sprite.pixelsPerUnit;
            float spriteWidth = renderer.sprite.rect.width;
            m_TextureHalfWidth = (spriteWidth / pixelsPerUnit) / 2.0f;
        }
    }

    float GetLeftSidePositionOfCamera()
    {
        float aspectRatio = (float) Screen.width / Screen.height;
        float screenHalfHeight = m_Camera.orthographicSize;
        float screenHalfWidth = screenHalfHeight * aspectRatio;
        return -screenHalfWidth;
    }

    void PlaceAfterNeighbour()
    {
        if(m_Neighbour)
        {
            SpriteRenderer neighbourRenderer = m_Neighbour.GetComponent<SpriteRenderer>(); 
            if (neighbourRenderer)
            {
                // get position of neighbour
                Vector3 neighbourPos = m_Neighbour.gameObject.transform.position;
                // get width
                float neighbourWidth = neighbourRenderer.sprite.rect.width;
                float pixelsPerUnit = neighbourRenderer.sprite.pixelsPerUnit;
                float epsilon = 0.05f;
                float amountToMove = neighbourWidth/ pixelsPerUnit;
                // set position to position + width of image
                gameObject.transform.position = new Vector3( neighbourPos.x + amountToMove - epsilon, neighbourPos.y, neighbourPos.z );
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get current position
        Vector3 currentPos = transform.position;
        // sub x axis by time * scrolling speed
        currentPos.x -= Time.deltaTime * m_scrollingSpeed;

        if(currentPos.x + m_TextureHalfWidth <= GetLeftSidePositionOfCamera())
        {
            PlaceAfterNeighbour();
        }
        else
        {
            // set as new position
            transform.position = currentPos;
        }
    }
}
