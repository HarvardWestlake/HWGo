using UnityEngine;

public class PokeballCatch : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the Pokeball hits a teacher
        if (collision.gameObject.tag == "Teacher")
        {
            // Stop the Pokeball's movement
            GetComponent<Rigidbody>().isKinematic = true;

            // Optional: Parent the Pokeball to the teacher to make them move together
            transform.SetParent(collision.transform);

            // Wait for 3 seconds and then remove both the Pokeball and the teacher
            Destroy(collision.gameObject, 3f);
            Destroy(gameObject, 3f);
        }
    }
}