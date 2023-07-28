using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using Producer_Consumer_Problem.Models;
using Producer_Consumer_Problem.Views.Shared;

namespace Producer_Consumer_Problem.Services
{
    public class Producer
    {
        private readonly SharedBuffer _buffer;
        private readonly Random _random = new Random();

        public Producer(SharedBuffer buffer)
        {
            _buffer = buffer;
        }

        public void Produce()
        {
            for (int i = 1; i <= 10; i++)
            {
                ITStudent student = GenerateStudent();
                string fileName = $"student{i}.xml";

                // Convert student information to XML and save to file
                string xmlData = ConvertStudentToXml(student);
                File.WriteAllText(fileName, xmlData);

                // Add integer (file name) to the buffer
                _buffer.AddToBuffer(i);

                Thread.Sleep(_random.Next(500, 1500)); // Simulate random production time
            }

            // Signal that the producer has finished producing
            _buffer.SetProducerFinished();
        }

        private ITStudent GenerateStudent()
        {
            // Implement your logic to generate random student information
            // For simplicity, I'm providing a sample student data here
            var student = new ITStudent
            {
                StudentName = "Lucky Tsabedze",
                StudentID = _random.Next(10000000, 99999999),
                Programme = "Computer Science",
                Courses = new List<Course>
                {
                    new Course { CourseName = "Discrete Mathematics", Mark = _random.Next(0, 101) },
                    new Course { CourseName = "Intergrative Programming Technologies", Mark = _random.Next(0, 101) },
                    // Add more courses as needed
                }
            };

            return student;
        }

        private string ConvertStudentToXml(ITStudent student)
        {
            // Implement your logic to convert the ITStudent object to XML format
            // For simplicity, I'm using XML serialization here
            XmlSerializer serializer = new XmlSerializer(typeof(ITStudent));
            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, student);
            return writer.ToString();
        }
    }
}
