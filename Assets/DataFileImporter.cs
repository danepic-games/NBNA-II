#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "dat")]
public class DataFileImporter : ScriptedImporter {
    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx) {
        var text = Decrypt(ctx.assetPath);
        var ta = new TextAsset(text);
        ctx.AddObjectToAsset("main obj", ta);
        ctx.SetMainObject(ta);
    }

    static IEnumerator<char> EncryptionKey = "odBearBecauseHeIsVeryGoodSiuHungIsAGo".GetEnumerator();

    static string Decrypt(string fileName)
    {
        if (fileName == null || !File.Exists(fileName)) return "";
        // First 123 bytes of lf2 .dat files are useless
        var bytes = File.ReadAllBytes(fileName).Skip(123);
        return DecryptByteSequence(bytes);
    }

    static string DecryptByteSequence(IEnumerable<byte> byteStream)
    {
        EncryptionKey.Reset();
        return new string(byteStream.Select(DecryptByte).ToArray());
    }

    static Func<byte, char> DecryptByte = b => (char)(b - NextEncryptionByte());

    static byte NextEncryptionByte()
    {
        if (!EncryptionKey.MoveNext())
        {
            EncryptionKey.Reset();
            EncryptionKey.MoveNext();
        }
        return (byte)EncryptionKey.Current;
    }
}
#endif
