using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Plant : MonoBehaviour
{
    [SerializeField] float foodValue = 10f;
    [SerializeField] string eaterTag = "FishBoid";
    [SerializeField] float secondHandRadius = 6f;
    [SerializeField] Animator eatenAnim;
    bool _isEaten;

    CircleCollider2D _col;

    public UnityEvent OnEaten = new();

    void Start() {
        _col = GetComponent<CircleCollider2D>();
        OnEaten.AddListener(() => {
            StartCoroutine(FeedOthersAround());
            GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f, 0f);
            eatenAnim.SetTrigger("EffectTrigger");
        });
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out AgentHunger otherHunger)) {
            otherHunger.Feed(foodValue);
            if (!_isEaten) {
                _isEaten = true;
                OnEaten.Invoke();
            }
        }
    }

    IEnumerator FeedOthersAround() {
        _col.radius = secondHandRadius;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
