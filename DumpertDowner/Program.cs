using System.Diagnostics;
using System.Text.RegularExpressions;

var url = Environment.GetCommandLineArgs()[1];
Console.WriteLine($"download:{url}");
var id = new Regex("\\/([0-9a-z]*)_").Match(url).Groups[1].Captures[0];
Console.WriteLine($"id:{id}");


var client = new HttpClient();
client.DefaultRequestHeaders.Add("user-agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
client.DefaultRequestHeaders.Add("origin", "https://www.dumpert.nl");
client.DefaultRequestHeaders.Add("authority", "api-live.dumpert.nl");
client.DefaultRequestHeaders.Add("path", $"/mobile_api/json/related/{id}/");
var content = await client.GetStringAsync(url);
var m3u8links = new Regex("https\\:\\/\\/media.dumpert.nl\\/dmp\\/media\\/video\\/[a-z0-9]*\\/[a-z0-9]*\\/[a-z0-9]*\\/[a-z0-9\\-]*\\/[a-z0-9]*\\/index.m3u8").Match(content);
var m3u8 = m3u8links.Captures.First().Value;
Console.WriteLine($"m3u8:{m3u8}");
Process.Start("ffmpeg.exe", new []{"-y","-i", m3u8, "-c", "copy", "-bsf:a", "aac_adtstoasc", "out.mp4" });
