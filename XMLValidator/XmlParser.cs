using System;
using System.Collections.Generic;
using System.Text;

namespace XMLValidator
{
    public class XmlParser
    {
        private enum State
        {
            Empty,
            OpeningBracket,
            Slash,
            Text,
            ClosingBracket,
            InnerText
        }

        private enum Command
        {
            ToSlash,
            ToText,
            ToClosingBracket,
            ToOpeningBracket
        }

        private readonly Dictionary<(State, Command), State> transitions = new Dictionary<(State, Command), State>() {
            { (State.Empty, Command.ToOpeningBracket), State.OpeningBracket },
            { (State.OpeningBracket, Command.ToText), State.Text},
            { (State.OpeningBracket, Command.ToSlash), State.Slash},
            { (State.Slash, Command.ToText), State.Text},
            { (State.Text, Command.ToText), State.Text},
            { (State.Text, Command.ToClosingBracket), State.ClosingBracket},
            { (State.ClosingBracket, Command.ToOpeningBracket), State.OpeningBracket},
            { (State.ClosingBracket, Command.ToText), State.InnerText},
            { (State.InnerText, Command.ToText), State.InnerText},
            { (State.InnerText, Command.ToOpeningBracket), State.OpeningBracket},

        };

        public IEnumerable<Element> ParseXml(string xml)
        {
            var state = State.Empty;
            var nodeName = new StringBuilder();
            var isClosingTag = false;

            foreach (var item in xml)
            {
                var command = GetCommand(item);
                state = NextState(state, command);

                switch (state)
                {
                    case State.OpeningBracket:
                        isClosingTag = false;
                        nodeName.Clear();
                        break;
                    case State.Slash:
                        isClosingTag = true;
                        break;
                    case State.Text:
                        nodeName.Append(item);
                        break;
                    case State.ClosingBracket:
                        yield return new Element(isClosingTag ? ElementType.Closing : ElementType.Opening,
                            nodeName.ToString());
                        break;
                    default:
                        break;
                }
            }
        }

        private Command GetCommand(char charecter)
        {
            return charecter switch
            {
                '<' => Command.ToOpeningBracket,
                '/' => Command.ToSlash,
                '>' => Command.ToClosingBracket,
                _ => Command.ToText
            };
        }

        private State NextState(State state, Command command)
        {
            var transition = (state, command);

            if(!transitions.TryGetValue(transition, out var nextState))
            {
                throw new FormatException("Unexpected charecter in XML");
            }
            return nextState;
        }
    }
}
