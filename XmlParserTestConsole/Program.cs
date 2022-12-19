using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace XmlParserTestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string filePath = @".\Data\SampleData.xml";

            XPathDocument docNav = new XPathDocument(filePath);
            XPathNavigator nav = docNav.CreateNavigator();

            nav.MoveToRoot();

            nav.MoveToChild(XPathNodeType.Element);

            MostOuterFrame test = new MostOuterFrame(nav);

            Console.ReadLine();
        }
    }
}
