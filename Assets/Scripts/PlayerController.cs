using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Inventory")]
    public Inventory inventory;
    public int maxCropCapacityPerType = -1; // -1 = unlimited

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = new Inventory(maxCropCapacityPerType);
    }

    private void FixedUpdate()
    {
        // Read input
        float horizontal = Input.GetAxis("Horizontal"); // Q/D
        float vertical = Input.GetAxis("Vertical");     // Z/S

        // Move in local space
        Vector3 move = transform.forward * vertical + transform.right * horizontal;

        // Apply velocity
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Cast ray downward
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
            {
                FieldManager field = hit.collider.GetComponent<FieldManager>();
                if (field != null)
                {
                    // If field is WATERED, the player collects the crop
                    if (field.GetCurrentState() == FieldManager.FieldState.WATERED)
                    {
                        inventory.AddCrop(field.crop, 1);
                    }

                    field.Action();
                }

                // Check for shop
                ShopTileManager shop = hit.collider.GetComponent<ShopTileManager>();
                if (shop != null)
                {
                    int moneyEarned = shop.SellInventory(inventory);

                    // Add coins to CoinManager
                    CoinManager.instance.AddCoins(moneyEarned);
                }

            }
        }
    }
}
