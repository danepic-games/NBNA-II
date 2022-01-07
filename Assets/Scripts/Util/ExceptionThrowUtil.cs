using UnityEngine;

namespace Util {
    public class ExceptionThrowUtil {

        public static void MissingComponent() {
            throw new MissingComponentException();
        }

        public static void MissingGameObject() {
            throw new MissingReferenceException();
        }

    }
}