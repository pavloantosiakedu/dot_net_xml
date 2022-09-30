using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Приклад використання потокових класів для обробки Xml
            string xmlFile = @"C:\Users\antosp\source\repos\ConsoleApp6\ConsoleApp6\StudentList.xml";
            // Приклад потокового формування/запису xml-документа
            using (XmlTextWriter writer = new XmlTextWriter(xmlFile, null))
            {
                writer.WriteStartDocument(); // <?xml version="1.0" ?>
                writer.WriteComment("Created @ " + DateTime.Now.ToString()); // <!-- Created @ 2022-09-06 9:18  -->
                writer.WriteStartElement("studentList"); // <studentList>
                // перший студент
                writer.WriteStartElement("student"); // <student>
                writer.WriteAttributeString("course", "2"); // <student course="2>
                writer.WriteElementString("fullname", "Ivanov Ivan"); // <fullname>Ivanov Ivan</fullname>
                //writer.WriteAttributeString("course", "2"); // Will throw an exception
                writer.WriteElementString("rating", "90"); // <rating>90</rating>
                writer.WriteEndElement(); // </student>
                // другий студент
                writer.WriteStartElement("student"); // <student>
                writer.WriteAttributeString("course", "1"); // <student course="1>
                writer.WriteElementString("fullname", "Petrenko Petro"); // <fullname>Petrenko Petro</fullname>
                writer.WriteElementString("rating", "75"); // <rating>75</rating>
                writer.WriteEndElement(); // </student>

                writer.WriteEndElement(); // </studentList>
                writer.WriteEndDocument(); // маркер кінця документу
            }

            // Приклад потокового зчитування xml-документа
            var studentList = new List<dynamic>();
            using (XmlTextReader reader = new XmlTextReader(xmlFile))
            {
                while (reader.Read()) // поки не кінець xml-документа
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "student")
                    {
                        var course = reader.GetAttribute("course");
                        reader.ReadStartElement("student");
                        var fullname = reader.ReadElementString("fullname");
                        var strRating = reader.ReadElementString("rating");

                        // додаємо зчитані дані студента до списку
                        studentList.Add(new
                        {
                            fullname,
                            course,
                            rating = Convert.ToDouble(strRating)
                        });
                    }
                }
            }

            // далі обробляємо список засобами .net
            foreach(var student in studentList)
            {
                Console.WriteLine("{0} {1} {2}", student.fullname, student.course, student.rating);
            }
            #endregion

            #region Приклад використання класів для обробки Xml в памяті
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);
            //doc.Save(xmlFile);

            XmlNodeList booklist = doc.GetElementsByTagName("book");
            var s = "г.ш";
            // якщо бібліотека не порожня :)
            if (booklist.Count != 0)
            {
                // перебираємо кожне джерело
                foreach (XmlElement b in booklist)
                    // дивимось чи відповідає автор джерела заданому користувачем
                    if (b.GetAttribute("author").Trim().ToLower().StartsWith(s.Trim().ToLower()))
                    {
                        // якщо це автор, який задав користувач, то виводимо назву джерела
                        Console.WriteLine(b.InnerText);
                    }

            }
            #endregion

            Console.ReadKey();
        }
    }
}
