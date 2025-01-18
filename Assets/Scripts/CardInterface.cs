using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CardInterface
{
    public string GetName();

    public Sprite GetSprite();

    public GameObject GetGameObject();

    public CardInterface FindReference(int index);

    public int GetReferenceListLength();

}
