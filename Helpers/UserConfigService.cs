using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class UserConfigService
{
    private readonly string _filePath = "userconfig.json";
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public Dictionary<string, object> LoadConfig()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var res = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, _settings);
            return res ?? new Dictionary<string, object>();
        }
        return new Dictionary<string, object>();
    }

    public void SaveConfig(Dictionary<string, object> config)
    {
        var json = JsonConvert.SerializeObject(config, _settings);
        File.WriteAllText(_filePath, json);
    }
}