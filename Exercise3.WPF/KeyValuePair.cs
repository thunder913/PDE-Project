namespace Exercise3.WPF
{
    public class KeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"Key {Key} - Value {Value}";
        }
    }
}
