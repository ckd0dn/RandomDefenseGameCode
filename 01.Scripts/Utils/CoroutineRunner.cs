using UnityEngine;
using System.Collections;
using CWLib;

public class CoroutineRunner : Singleton<CoroutineRunner>
{

    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}