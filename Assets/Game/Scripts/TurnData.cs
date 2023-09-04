using System.Collections.Generic;
using System.Linq;

public class TurnData<T>
{
    private Dictionary<T, bool> dict;

    public TurnData()
    {
        dict = new Dictionary<T, bool>();
    }

    public void AddElement(T t)
    {
        dict[t] = true;
    }

    public bool GetElementStatus(T t)
    {
        if (dict.ContainsKey(t))
        {
            return dict[t];
        }

        ConsoleLogger.Log("GetElementStatus: no key found" + t);
        return false;
    }

    public void SetElementStatus(T t, bool status)
    {
        if (dict.ContainsKey(t))
        {
            dict[t] = status;
        }
        else
        {
            ConsoleLogger.Log("SetElementStatus: no key found" + t);
        }
    }

    public void Reset()
    {
        // FIXME remove/replace the ToList call
        foreach (var key in dict.Keys.ToList())
        {
            dict[key] = true;
        }
    }
}
