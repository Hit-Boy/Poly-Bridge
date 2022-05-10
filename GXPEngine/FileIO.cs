using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.IO;

public class FileIO
{
    public string directory = Directory.GetCurrentDirectory() + "\\";
    public FileIO()
    {

    }

    public List<string> ReadLevel(string path)
    {
        List<string> lines = new List<string>();
        return lines = File.ReadAllLines(path).ToList(); // Returns a list where every list item is a different line of the file
    }

    public void CopyContent(string origin, string destination)
    {
        origin = directory + origin + ".txt";
        destination = directory + destination + ".txt";

        List<string> currentLines = ReadLevel(origin);
        List<string> existingLines = ReadLevel(destination);

        existingLines.AddRange(currentLines);

        File.WriteAllLines(destination, existingLines);
    }

    public void AddToFile(string destination, string text)
    {
        destination = directory + destination + ".txt";
        List<string> currentLines = ReadLevel(destination);
        currentLines.Add(text);

        File.WriteAllLines(destination, currentLines);
    }
}