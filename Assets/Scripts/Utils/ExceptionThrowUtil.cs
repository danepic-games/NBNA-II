using System.IO;
using UnityEngine;

namespace Util {
    public class ExceptionThrowUtil {

        public static void MissingComponent() {
            throw new MissingComponentException();
        }

        public static void MissingGameObject() {
            throw new MissingReferenceException();
        }

        public static void MissingDataFile() {
            throw new FileNotFoundException("Data files not found!");
        }

    }
}