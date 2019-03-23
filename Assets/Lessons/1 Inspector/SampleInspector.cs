using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SampleEnum
{
    enum1,
    enum2,
    enum3
}

[System.Serializable]   //сериализация. Позволяет производить бинарную сериализацию. Все сериализуемые объекты отображаются в инспекторе
public class SampleClass
{
    public float field1;
    public Vector3 field2;
}

public class SampleInspector : MonoBehaviour {

    #region Стандарт

    [Header("Стандартный вариант")] //атрибут для заголовка
    public bool bool1;
    public string string1;
    public int int1;
    public float float1;
    public Vector3 Vector3_1;
    public GameObject gameObject1;
    public LayerMask layerMask1;
    public SampleEnum sampleEnum1;
    public SampleClass sampleClass1;

    public float[] array1;
    public List<GameObject> list1;
    public Dictionary<int, string> dictionary;
    public float[,] array2;

    private bool bool2;

    #endregion

    #region С атрибутами
   
    [Header("Вариант с атрибутами")]
    [Space(20)] //атрибут для вертикального отступа в указанное количество пикселей

    [Tooltip("Подсказка")] public bool bool3; // атрибут для выведения подсказки при наведении
    [Range(0, 10)] public float float3; //указание границ значения
    [SerializeField] private List<SampleClass> list3; //сериализуемое поле. позволяет отображать даже приватные поля (полезно для дебага)
    #endregion

    //метод для отрисовки своих Gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(0.3f, 0.3f, 0.3f));
    }

    //отрисовка своих Gizmo, только когда выделен объект, на котором повешан скрипт
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach(var c in list1)
        {
            Gizmos.DrawLine(transform.position, c.transform.position);
            Gizmos.DrawSphere(c.transform.position, 0.5f);
        }
    }
}
