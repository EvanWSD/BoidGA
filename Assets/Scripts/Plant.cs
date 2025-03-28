using System;
using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] float foodValue = 10f;
    [SerializeField] string eaterTag = "FishBoid";
    [SerializeField] float secondHandRadius = 6f;
    bool _isEaten;


    CircleCollider2D _col;

    void Start() {
        _col = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out AgentHunger otherHunger)) {
            otherHunger.Feed(foodValue);
            if (!_isEaten) {
                _isEaten = true;
                StartCoroutine(FeedOthersAround());
            }
        }
    }

    IEnumerator FeedOthersAround() {
        _col.radius = secondHandRadius;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
