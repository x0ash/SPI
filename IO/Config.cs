namespace IO
{
    [Serializable]
    public struct Config
    {
        public string key { get; set; }

        public Config()
        {
            key = "";
        }
    }
}
