using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml;

namespace XmlParserTestConsole
{


    public static class Extension
    {
        public static void ParseException(XPathNavigator nav, string message)
        {
            Console.WriteLine("Line " + ((IXmlLineInfo)nav).LineNumber +" | Row: " + ((IXmlLineInfo)nav).LinePosition  + " - " + message);
        }
    }

    public class MostOuterFrame
    {
        public OuterFrame OuterFrame { get; set; }

        public MostOuterFrame(XPathNavigator nav)
        {
            if (nav.HasChildren is false)
            {
                Extension.ParseException(nav, "No child Elements were found");
                return;
            }

            if (nav.MoveToChild("OuterFrame", string.Empty))
                OuterFrame = new OuterFrame(nav);
            else
                Extension.ParseException(nav, "No valid Outer Frame Element was found");


        }
    }

    public class OuterFrame
    {
        public string OuterFrameAttribute { get; set; }
        public List<MidFrame> MidFrames { get; set; } = new List<MidFrame>();

        public OuterFrame(XPathNavigator nav)
        {
            OuterFrameAttribute = nav.GetAttribute("OuterFrameAttribute", string.Empty);
            if (string.IsNullOrEmpty(OuterFrameAttribute))
                Extension.ParseException(nav, "No outer Frame Attribute was found");

            var childNodes = nav.SelectChildren("MidFrame", string.Empty);

            if (childNodes is null)
                Extension.ParseException(nav, "No MidFrame Elements were found");

            while (childNodes.MoveNext())
            {
                if (childNodes?.Current != null)
                    MidFrames.Add(new MidFrame(childNodes.Current));
            }
        }
    }

    public class MidFrame
    {
        public int MidFrameCounter { get; set; }
        public List<InnerElement> InnerElements { get; set; } = new List<InnerElement>();
        public List<InnerElementWithContent> InnerElementsWithContent { get; set; } = new List<InnerElementWithContent>();

        public MidFrame(XPathNavigator nav)
        {
            if (int.TryParse(nav.GetAttribute("MidFrameCounter", string.Empty), out var tempMidFrameCounter))
                MidFrameCounter = tempMidFrameCounter;

            var childNodes = nav.SelectChildren("InnerElement", string.Empty);

            if (childNodes is null)
                Extension.ParseException(nav, "No InnerElements Elements were found");

            while (childNodes.MoveNext())
            {
                if (childNodes?.Current != null)
                    InnerElements.Add(new InnerElement(childNodes.Current));
            }

            var childNodesWithContent = nav.SelectChildren("InnerElementWithContent", string.Empty);

            if (childNodesWithContent is null)
                Extension.ParseException(nav, "No InnerElementsWithContent Elements were found");

            while (childNodesWithContent.MoveNext())
            {
                if (childNodesWithContent?.Current != null)
                    InnerElementsWithContent.Add(new InnerElementWithContent(childNodesWithContent.Current));
            }
        }
    }

    public class InnerElement
    {
        public string Name { get; set; }

        public InnerElement(XPathNavigator nav)
        {
            Name = nav.GetAttribute("Name", string.Empty);
            if (string.IsNullOrEmpty(Name))
                Extension.ParseException(nav, "No Name Attribute");
        }
    }

    public class InnerElementWithContent
    {
        public string Content { get; set; }

        public InnerElementWithContent(XPathNavigator nav)
        {
            Content = nav.Value;
        }
    }
}
