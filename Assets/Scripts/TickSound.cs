using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickSound : MonoBehaviour
{
    [SerializeField] private AudioSource _tickAudioSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Corner")
            _tickAudioSource.Play();
    }
}
