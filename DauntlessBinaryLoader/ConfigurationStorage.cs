using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DauntlessBinaryLoader
{
    internal class ConfigurationStorage
    {
        // Token: 0x1700002C RID: 44
        // (get) Token: 0x060000F8 RID: 248 RVA: 0x00006D81 File Offset: 0x00004F81
        // (set) Token: 0x060000F9 RID: 249 RVA: 0x00006D89 File Offset: 0x00004F89
        [JsonIgnore]
        public string KeyPath { get; private set; } = string.Empty;

        // Token: 0x060000FA RID: 250 RVA: 0x00006D92 File Offset: 0x00004F92
        static ConfigurationStorage()
        {
            ConfigurationStorage.ReadConfigFile();
        }

        private ConfigurationStorage() { }

        // Token: 0x060000FC RID: 252 RVA: 0x00006DFE File Offset: 0x00004FFE
        public bool ShouldSerializeValues()
        {
            return this.Values.Count > 0;
        }

        // Token: 0x060000FD RID: 253 RVA: 0x00006E0E File Offset: 0x0000500E
        public bool ShouldSerializeSubKeys()
        {
            return this.SubKeys.Count > 0;
        }

        // Token: 0x060000FE RID: 254 RVA: 0x00006E20 File Offset: 0x00005020
        public static ConfigurationStorage Get(string path = null)
        {
            ConfigurationStorage configurationStorage = ConfigurationStorage.mRoot;
            if (string.IsNullOrEmpty(path))
            {
                return configurationStorage;
            }
            ConfigurationStorage configurationStorage2 = configurationStorage;
            string[] array = path.Split(new char[]
            {
                '\\'
            });
            ConfigurationStorage obj = configurationStorage;
            lock (obj)
            {
                foreach (string text in array)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        configurationStorage2 = configurationStorage2.CreateSingleSubKey(text);
                    }
                }
            }
            return configurationStorage2;
        }

        // Token: 0x060000FF RID: 255 RVA: 0x00006EAC File Offset: 0x000050AC
        public ConfigurationStorage GetSubKey(string path)
        {
            return ConfigurationStorage.Get(Path.Combine(this.KeyPath, path));
        }

        // Token: 0x06000100 RID: 256 RVA: 0x00006EC0 File Offset: 0x000050C0
        private ConfigurationStorage CreateSingleSubKey(string name)
        {
            ConfigurationStorage obj = ConfigurationStorage.mRoot;
            ConfigurationStorage result;
            lock (obj)
            {
                ConfigurationStorage configurationStorage = null;
                if (!this.SubKeys.TryGetValue(name, out configurationStorage))
                {
                    configurationStorage = new ConfigurationStorage();
                    this.SubKeys[name] = configurationStorage;
                }
                configurationStorage.KeyPath = Path.Combine(this.KeyPath, name);
                result = configurationStorage;
            }
            return result;
        }

        // Token: 0x1700002D RID: 45
        public string this[string key]
        {
            get
            {
                ConfigurationStorage obj = ConfigurationStorage.mRoot;
                string result;
                lock (obj)
                {
                    string text = null;
                    this.Values.TryGetValue(key, out text);
                    Console.WriteLine("Configuration::Get: '{0}' returned '{1}'", new object[]
                    {
                        Path.Combine(this.KeyPath, key),
                        text
                    });
                    result = text;
                }
                return result;
            }
            set
            {
                ConfigurationStorage obj = ConfigurationStorage.mRoot;
                lock (obj)
                {
                    bool flag2 = false;
                    string text = null;
                    this.Values.TryGetValue(key, out text);
                    if (value == null)
                    {
                        if (text == null)
                        {
                            return;
                        }
                        this.Values.Remove(key);
                        flag2 = true;
                        Console.WriteLine("Configuration::Set: Deleted '{0}'", new object[]
                        {
                            Path.Combine(this.KeyPath, key)
                        });
                    }
                    else if (text == null || !string.Equals(text, value, StringComparison.Ordinal))
                    {
                        this.Values[key] = value;
                        flag2 = true;
                        Console.WriteLine("Configuration::Set: '{0}' to '{1}'", new object[]
                        {
                            Path.Combine(this.KeyPath, key),
                            value
                        });
                    }
                    if (flag2)
                    {
                        ConfigurationStorage.UpdateConfigFile();
                    }
                }
            }
        }

        // Token: 0x06000103 RID: 259 RVA: 0x00007088 File Offset: 0x00005288
        public IEnumerable<ConfigurationStorage> GetSubKeys()
        {
            ConfigurationStorage obj = ConfigurationStorage.mRoot;
            IEnumerable<ConfigurationStorage> result;
            lock (obj)
            {
                List<ConfigurationStorage> list = new List<ConfigurationStorage>(this.SubKeys.Count);
                foreach (KeyValuePair<string, ConfigurationStorage> keyValuePair in this.SubKeys)
                {
                    keyValuePair.Value.KeyPath = Path.Combine(this.KeyPath, keyValuePair.Key);
                    list.Add(keyValuePair.Value);
                }
                result = list;
            }
            return result;
        }

        // Token: 0x06000104 RID: 260 RVA: 0x00007140 File Offset: 0x00005340
        public IEnumerable<string> GetSubKeyNames()
        {
            ConfigurationStorage obj = ConfigurationStorage.mRoot;
            IEnumerable<string> result;
            lock (obj)
            {
                result = this.SubKeys.Keys.ToArray<string>();
            }
            return result;
        }

        // Token: 0x06000105 RID: 261 RVA: 0x0000718C File Offset: 0x0000538C
        public void DeleteKey(string name)
        {
            ConfigurationStorage obj = ConfigurationStorage.mRoot;
            lock (obj)
            {
                if (this.SubKeys.Remove(name))
                {
                    ConfigurationStorage.UpdateConfigFile();
                }
            }
        }

        // Token: 0x06000106 RID: 262 RVA: 0x000071D8 File Offset: 0x000053D8
        private static ConfigurationStorage ReadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
                JsonSerializer jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
                Console.WriteLine("Reading configuration from: '{0}'", new object[]
                {
                    filePath
                });
                return JsonConvert.DeserializeObject<ConfigurationStorage>(File.ReadAllText(filePath));
            }
            return null;
        }

        // Token: 0x06000107 RID: 263 RVA: 0x0000721C File Offset: 0x0000541C
        private static void ReadConfigFile()
        {
            Func<ConfigurationStorage>[] array = new Func<ConfigurationStorage>[3];
            array[0] = (() => ConfigurationStorage.ReadFile(ConfigurationStorage.mConfigFile));
            array[1] = (() => ConfigurationStorage.ReadFile(ConfigurationStorage.mConfigFileBak));
            array[2] = delegate
            {
                Console.WriteLine("Using default configuration.");
                return new ConfigurationStorage();
            };
            Func<ConfigurationStorage>[] array2 = array;
            foreach (Func<ConfigurationStorage> func in array2)
            {
                try
                {
                    ConfigurationStorage configurationStorage = func();
                    if (configurationStorage != null)
                    {
                        ConfigurationStorage.mRoot = configurationStorage;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was an issue reading the configuration, trying a fallback...");
                    throw ex;
                }
            }
        }

        // Token: 0x06000108 RID: 264 RVA: 0x000072EC File Offset: 0x000054EC
        private static void UpdateConfigFile()
        {
            ConfigurationStorage obj = ConfigurationStorage.mRoot;
            lock (obj)
            {
                try
                {
                    if (File.Exists(ConfigurationStorage.mConfigFileBak))
                    {
                        File.Delete(ConfigurationStorage.mConfigFileBak);
                    }
                    if (File.Exists(ConfigurationStorage.mConfigFile))
                    {
                        File.Move(ConfigurationStorage.mConfigFile, ConfigurationStorage.mConfigFileBak);
                    }
                    string contents = JsonConvert.SerializeObject(ConfigurationStorage.mRoot, Formatting.Indented);
                    File.WriteAllText(ConfigurationStorage.mConfigFile, contents);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to update the configuration file with the changes.");
                    throw ex;
                }
            }
        }

        // Token: 0x04000059 RID: 89
        private static ConfigurationStorage mRoot = new ConfigurationStorage();

        // Token: 0x0400005A RID: 90
        private static readonly string mConfigFile = @"C:\Program Files\Phoenix Labs\Dauntless\Data\configuration.json";

        // Token: 0x0400005B RID: 91
        private static readonly string mConfigFileBak = ConfigurationStorage.mConfigFile + ".bak";

        // Token: 0x0400005C RID: 92
        public readonly SortedDictionary<string, string> Values = new SortedDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        // Token: 0x0400005D RID: 93
        public readonly SortedDictionary<string, ConfigurationStorage> SubKeys = new SortedDictionary<string, ConfigurationStorage>(StringComparer.InvariantCultureIgnoreCase);
    }
}
