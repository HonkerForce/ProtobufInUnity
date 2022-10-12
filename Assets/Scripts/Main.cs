using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using Test;
using Test2;
using System.IO;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        person per = new person
        {
            Name = "ddy"
        };

        per.Num.Add(1);
        per.Num.Add(2);

        byte[] bytes;
        using (MemoryStream stream = new MemoryStream())
        {
            per.WriteTo(stream);

            bytes = stream.ToArray();
        }

        string print = "";
        foreach (var b in bytes)
        {
            print += (int)b + " ";
        }
        Debug.Log(print);

        person parsePer = person.Parser.ParseFrom(bytes);
        print = parsePer.ToString();
        Debug.Log(print);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
