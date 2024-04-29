namespace IO
{
    [Serializable]
    internal struct Config
    {
        public string key { get; set; }

        public Config()
        {
            key = "";
        }
    }
}
