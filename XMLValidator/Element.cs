namespace XMLValidator
{
    public enum ElementType
    {
        Opening,
        Closing
    }

    public class Element
    {
        public ElementType Type { get; set; }
        public string Name { get; set; }

        public Element(ElementType type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public bool IsOpenedBy(Element element)
            => Type == ElementType.Closing &&
            element.Type == ElementType.Opening &&
            Name == element.Name;
    }
}
