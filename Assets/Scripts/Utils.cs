using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceyPokeProto {
    public static class Utils {
        public static System.Random rng = new System.Random();

        public static T getRandomElement<T>(this IEnumerable<T> list) {
            return list.OrderBy(i => rng.Next()).First();
        }

        public static List<T> getManyRandomElements<T>(this IEnumerable<T> list, int number) {
            return list.OrderBy(i => rng.Next()).Take(number).ToList();
        }

        public static IEnumerable<TSource>
            Also<TSource>(this IEnumerable<TSource> source, Action<TSource> selector) {
            source.ToList().ForEach(i => selector(i));
            return source;
        }

        public static List<T> Shuffled<T>(this List<T> list) {
            return list.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public static T AddAndGetComponent<T>(this GameObject parent) where T : Component {
            parent.AddComponent<T>();
            return parent.GetComponent<T>();
        }

        public static void assignSpriteFromPath(this GameObject gameObj, string path) {
            gameObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
        }
    }
}