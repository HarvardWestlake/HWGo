using UnityEngine;
using System.Collections;

public class PokeballCatch : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the Pokeball hits a teacher
        if (collision.gameObject.tag == "Teacher")
        {
            // Stop the Pokeball's movement
            GetComponent<Rigidbody>().isKinematic = true;

            // Parent the Pokeball to the teacher
            transform.SetParent(collision.transform);

            // Start the catching coroutine
            StartCoroutine(CatchSequence(collision.gameObject));
        }
    }

    IEnumerator CatchSequence(GameObject teacher)
    {
        // Store the original rotation
        Quaternion originalRotation = transform.rotation;
        float rotationAmount = 30.0f; // Degree of rotation for the shake
        float shakeDuration = 0.1f;
        float delayBetweenShakes = 1.0f;

        for (int i = 0; i < 3; i++) // Shake three times
        {
            // Rotate to one side
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles + new Vector3(0, rotationAmount, 0));
            yield return new WaitForSeconds(shakeDuration);

            // Rotate back to original
            transform.rotation = originalRotation;
            yield return new WaitForSeconds(shakeDuration);

            // Rotate to the other side
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles - new Vector3(0, rotationAmount, 0));
            yield return new WaitForSeconds(shakeDuration);

            // Reset to original rotation
            transform.rotation = originalRotation;
            yield return new WaitForSeconds(delayBetweenShakes); // Wait before the next shake
        }

        // Wait for a moment after the last shake
        yield return new WaitForSeconds(1f);

        // Destroy the teacher and Pokeball
        Destroy(teacher);
        Destroy(gameObject);
    }
}
