using System;
using System.Collections.Generic;
using System.Linq;

namespace XMLValidator
{
    public class Validator
    {
        public bool DetermineSxml(string xml)
        {
            var parser = new XmlParser();

            try
            {
                var elements = parser.ParseXml(xml).ToList();

                if(!elements.Any() || elements.Count() % 2 != 0)
                {
                    return false;
                }
                return AreElemetsBalanced(elements);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool AreElemetsBalanced(IEnumerable<Element> elements)
        {
            var stack = new Stack<Element>();

            foreach (var element in elements)
            {

                if (!stack.TryPeek(out var stackElement) || element.Type == ElementType.Opening)
                {
                    stack.Push(element);
                    continue;
                }

                if (!element.IsOpenedBy(stackElement))
                {
                    return false;
                }

                stack.Pop();
            }

            return stack.Count() == 0;
        }
    }
}
