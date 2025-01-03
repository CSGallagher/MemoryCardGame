using UnityEngine;

public class Card : MonoBehaviour
{
    public Sprite frontImage;   // Front image of the card
    public Sprite backImage;    // Back image for the card
    private SpriteRenderer spriteRenderer;
    private bool isFlipped = false;  // Track if the card is flipped

    private Vector3 originalPosition;  // Store the original position for moving

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = backImage;  // Set the initial sprite to the back image
        originalPosition = transform.position;  // Save the card's initial position
    }

    void OnMouseDown()
    {
        if (!isFlipped)
        {
            FlipCard();  // Flip the card when clicked
            CardManager.Instance.CardFlipped(this);  // Notify the manager about the card flip
        }
        else
        {
            CardManager.Instance.MoveToDiscardPile(this);  // Move to the discard pile if the card is flipped
        }
    }

    // Flip the card to reveal the front image
    void FlipCard()
    {
        isFlipped = true;
        spriteRenderer.sprite = frontImage;  // Change sprite to the front image
        Debug.Log("Card flipped.");
    }

    // Move the card above the other cards vertically (center position)
    public void MoveToCenter(Vector3 targetPosition)
    {
        transform.position = targetPosition;  // Move the card to the target position (only vertical movement)
        Debug.Log("Card moved to the center.");
    }

    // Move the card to the discard pile
    public void MoveToDiscardPile()
    {
        transform.position = new Vector3(10f, 0f, 0f);  // Discard pile position (right side)
        spriteRenderer.sprite = frontImage;  // Ensure it's face-up
        Debug.Log("Card discarded.");
    }

    // Reset the card to its original position (when it's placed in the stack)
    public void ResetCard()
    {
        transform.position = originalPosition;  // Reset position
        isFlipped = false;  // Flip the card back to the back image
        spriteRenderer.sprite = backImage;  // Set the back image again
    }
}
