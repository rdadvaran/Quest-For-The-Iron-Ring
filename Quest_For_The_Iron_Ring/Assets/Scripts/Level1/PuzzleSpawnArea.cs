using UnityEngine;

public class PuzzleSpawnArea : MonoBehaviour
{
    public float width = 1f;
    public float height = 1f;

    public Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(-width / 2f, width / 2f);
        float randomY = Random.Range(-height / 2f, height / 2f);

        return new Vector2(transform.position.x + randomX, transform.position.y + randomY);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0f));
    }
}
