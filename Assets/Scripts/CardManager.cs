using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }  // Singleton instance

    public GameObject cardPrefab;  // The card prefab to instantiate
    public Sprite[] cardFrontImages;  // Array of possible front images for the card
    public Sprite cardBackImage;  // Back image for the card
    private GameObject[] dealtCards;  // Array to hold dealt cards
    private GameObject[] middleCards;  // Array to hold the five middle cards
    private List<GameObject> centerCards;  // List of cards moved to the center
    private GameObject discardPileCard;  // Holds the most recently discarded card
    private bool gameEnded = false;

    private float verticalSpacing = 2.0f;  // Vertical spacing for advancing cards to the center

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dealtCards = new GameObject[16];  // Initialize the array for 16 cards
        middleCards = new GameObject[5];  // Array to hold 5 middle cards
        centerCards = new List<GameObject>();  // List for center cards
        DealCards();  // Call the method to deal the cards
    }

    // Deal the cards into the stack and middle
    void DealCards()
    {
        // Stack of face-down cards in memory (not displayed)
        for (int i = 0; i < 16; i++)
        {
            dealtCards[i] = cardPrefab;  // Store the prefab in the stack (no instantiation here yet)
        }

        // Deal 5 cards face down to the middle (playable cards)
        for (int i = 0; i < 5; i++)
        {
            float xPos = -3.5f + (i * 2.5f);  // Spread out the middle cards horizontally
            GameObject middleCard = Instantiate(cardPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            middleCards[i] = middleCard;

            Card cardScript = middleCard.GetComponent<Card>();
            if (cardScript != null)
            {
                cardScript.frontImage = cardFrontImages[i % cardFrontImages.Length];  // Assign a front image
                cardScript.backImage = cardBackImage;  // Assign the back image
            }
        }
    }

    // Notify that a card has been clicked and flipped
    public void CardFlipped(Card card)
    {
        // if (gameEnded) return;

        // Move the flipped card to the center (stack vertically above the original position)
        Vector3 newPosition = new Vector3(card.transform.position.x, card.transform.position.y + verticalSpacing, 0f);  // Fixed vertical movement
        card.MoveToCenter(newPosition);
        centerCards.Add(card.gameObject);  // Track the card moved to the center
        Debug.Log("Card moved to the center.");
    }

    // Move the most recently added card to the discard pile
    public void MoveToDiscardPile(Card card)
    {
        if (centerCards.Count > 0)
        {
            // Get the last card in the center (most recently moved)
            GameObject cardToDiscard = centerCards[centerCards.Count - 1];
            cardToDiscard.GetComponent<Card>().MoveToDiscardPile();  // Move it to the discard pile
            centerCards.RemoveAt(centerCards.Count - 1);  // Remove it from the center list

            // If there's already a card in the discard pile, hide it
            if (discardPileCard != null)
            {
                discardPileCard.SetActive(false);  // Hide the previous discarded card
            }

            // Update the discard pile to show the most recently discarded card
            discardPileCard = cardToDiscard;  // Track the discarded card
            discardPileCard.SetActive(true);  // Make the card visible
            Debug.Log("Card discarded.");
        }

        // End the game after all cards have been discarded
        // Make sure this is only triggered after all cards are discarded
        if (centerCards.Count == 0 && !gameEnded)
        {
            EndGame();  // End the game if all cards are discarded
        }
    }

    // End the game
    public void EndGame()
    {
        gameEnded = true;
        Debug.Log("Game Over!");
        // Add any additional end-game logic here (e.g., stopping the game, showing UI)
    }
}
