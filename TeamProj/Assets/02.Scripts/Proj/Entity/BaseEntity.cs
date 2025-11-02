using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    private static int m_id = 0;

    private int id;
    public int Id
    {
        get => id;
        set
        {
            id = value;
            m_id++;
        }
    }
	private string entityName;
	public virtual void Setup(string name)
    {
        Id = m_id;
        entityName = name;
    }
    public abstract void updated();

    public void PrintText(string text)
    {
        Debug.Log($"{entityName} : {text}");
    }


}
