using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public static class Utils {
    public static T getRandomElement<T>(this IEnumerable<T> list) {
        var rnd = new System.Random();
        return list.OrderBy(i => rnd.Next()).First();
    }

    public static List<T> getManyRandomElements<T>(this IEnumerable<T> list, int number) {
        var rnd = new System.Random();
        return list.OrderBy(i => rnd.Next()).Take(number).ToList();
    }

    public static void OverrideListener(this UnityEvent onClick, UnityAction call) {
        onClick.RemoveAllListeners();
        onClick.AddListener(call);
    }
}
