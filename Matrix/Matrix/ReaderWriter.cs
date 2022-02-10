using System;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Collections.Generic;

namespace Matrix
{
    public class ReaderWriter<T>
{
    private readonly char _delimiter;


    public ReaderWriter(char delimiter = ';')
    {
        _delimiter = delimiter;
    }

    public T[][] ReadFile(string filepath)
    {
        var res = new ResizeArray<T[]>();
        using (var sr = File.OpenText(filepath))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                var line = s.Split(_delimiter).Select((s1 => (T)Convert.ChangeType(s1, typeof(T))));
                    var arline = line.ToArray();
                res.Add(arline);
            }
        }

        return res.Value();
    }

    public void WriteFile(T[][] mx, string filepath)
    {
        var enumerable = mx
            .AsEnumerable()
            .Select(
                line => line
                    .AsEnumerable()
                    .Select(num => $"{num}")
                    .Aggregate((accum, current) => accum + _delimiter + current)
                );
        File.WriteAllLines(filepath, enumerable);
    }
}
}