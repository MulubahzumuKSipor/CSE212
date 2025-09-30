using System.Text.Json;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // TODO Problem 1 - ADD YOUR CODE HERE
        var set = new HashSet<string>(words);
        var result = new List<string>();

        foreach (var w in set)
        {
            if (string.IsNullOrEmpty(w) || w.Length != 2) continue;

            if (w[0] == w[1]) continue; // Skip words with identical letters
            
            var rev = new string(new[] { w[1], w[0] });

            if (set.Contains(rev) && String.CompareOrdinal(w, rev) < 0)
            {
                result.Add($"{w} & {rev}");
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
     public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var line in File.ReadLines(filename))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Split by comma. If your CSV uses quotes with commas inside, a more robust parser is needed.
            var fields = line.Split(',');

            // Ensure there are enough columns
            if (fields.Length < 4)
                continue;

            var degree = fields[3].Trim().Trim('"'); // remove whitespace and quotes

            if (string.IsNullOrEmpty(degree))
                continue;

            // Increment count in dictionary
            if (degrees.ContainsKey(degree))
                degrees[degree]++;
            else
                degrees[degree] = 1;
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
   public static bool IsAnagram(string a, string b)
    {
        if (a == null || b == null) return false;

        // Normalize: remove spaces and lower-case
        var s1 = a.Replace(" ", "", StringComparison.Ordinal).ToLowerInvariant();
        var s2 = b.Replace(" ", "", StringComparison.Ordinal).ToLowerInvariant();

        if (s1.Length != s2.Length) return false;

        var counts = new Dictionary<char,int>();

        foreach (char c in s1)
        {
            counts[c] = counts.GetValueOrDefault(c) + 1;
        }

        foreach (char c in s2)
        {
            if (!counts.TryGetValue(c, out int cnt) || cnt == 0)
                return false;
            counts[c] = cnt - 1;
        }

        // all counts should be zero
        foreach (var kv in counts)
            if (kv.Value != 0) return false;

        return true;
    }

    // <summary>
    // I’ll provide a clear Maze class shape and implementations for the four moves. I assume the maze is represented as a Dictionary<(int x, int y), Cell> where Cell tells which directions are allowed.
    // </summary>

    public class Cell
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }
        public bool Down { get; set; }
    }

    public class Maze
    {
        private readonly Dictionary<(int x,int y), Cell> _grid;
        private (int x, int y) _position;

        public Maze(Dictionary<(int,int), Cell> grid, (int x,int y) start)
        {
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            _position = start;
        }

        public (int x,int y) Position => _position;

        public string GetStatus() => $"Current location (x={_position.x}, y={_position.y})";

        public bool MoveLeft()
        {
            var next = (_position.x - 1, _position.y);
            if (!_grid.ContainsKey(next) || _grid[_position].Left)
                throw new InvalidOperationException("Can't go that way!");
            _position = next;
            return true;
        }

        public bool MoveRight()
        {
            var next = (_position.x + 1, _position.y);
            if (!_grid.ContainsKey(next) || _grid[_position].Right)
                throw new InvalidOperationException("Can't go that way!");
            _position = next;
            return true;
        }

        public bool MoveUp()
        {
            var next = (_position.x, _position.y - 1);
            if (!_grid.ContainsKey(next) || _grid[_position].Up)
                throw new InvalidOperationException("Can't go that way!");
            _position = next;
            return true;
        }

        public bool MoveDown()
        {
            var next = (_position.x, _position.y + 1);
            if (!_grid.ContainsKey(next) || _grid[_position].Down)
                throw new InvalidOperationException("Can't go that way!");
            _position = next;
            return true;
        }
    }


    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        // TODO Problem 5:
        // 1. Add code in FeatureCollection.cs to describe the JSON using classes and properties 
        // on those classes so that the call to Deserialize above works properly.
        // 2. Add code below to create a string out each place a earthquake has happened today and its magitude.
        // 3. Return an array of these string descriptions.
        if (featureCollection?.Features == null)
            return Array.Empty<string>();

        var results = new List<string>();

        foreach (var feature in featureCollection.Features)
        {
            var place = feature.Properties?.Place ?? "Unknown location";
            var mag = feature.Properties?.Mag?.ToString("0.##") ?? "N/A";
            results.Add($"{place} - Mag {mag}");
        }

        return results.ToArray();
    }

    // Local classes for JSON deserialization
        class FeatureCollection
        {
            [JsonPropertyName("features")]
            public List<Feature> Features { get; set; } = new List<Feature>();
        }

        class Feature
        {
            [JsonPropertyName("properties")]
            public Properties Properties { get; set; } = new Properties();
        }

        class Properties
        {
            [JsonPropertyName("place")]
            public string? Place { get; set; }

            [JsonPropertyName("mag")]
            public double? Mag { get; set; }
        }
}
