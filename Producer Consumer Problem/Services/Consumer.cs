using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using Producer_Consumer_Problem.Models;
using Producer_Consumer_Problem.Views.Shared;

namespace Producer_Consumer_Problem.Services
{
    public class Consumer
    {
        private readonly SharedBuffer _buffer;
        private readonly SemaphoreSlim _bufferLock;
        private bool _producerFinished = false;

        public Consumer(SharedBuffer buffer)
        {
            _buffer = buffer;
            _bufferLock = buffer.GetSemaphore(); // Get the semaphore from the shared buffer
        }

        public void Consume()
        {
            while (true)
            {
                int fileName;
                bool isBufferEmpty = false;

                // Try to acquire the lock to access the buffer
                if (_bufferLock.Wait(100)) // Wait for 100ms to acquire the lock
                {
                    try
                    {
                        if (_buffer.IsEmpty)
                        {
                            // Buffer is empty, check if the producer has finished producing
                            if (_producerFinished)
                            {
                                isBufferEmpty = true;
                                break; // Exit the loop if the buffer is empty and producer is done
                            }
                        }
                        else
                        {
                            fileName = _buffer.RemoveFromBuffer();
                            string xmlData = File.ReadAllText($"student{fileName}.xml");

                            ITStudent student = ConvertXmlToStudent(xmlData);
                            double averageMark = student.CalculateAverageMark();
                            bool isPass = student.IsPass();

                            // Print the student's information, marks, average, and pass/fail status
                            Console.WriteLine($"Student Name: {student.StudentName}");
                            Console.WriteLine($"Student ID: {student.StudentID}");
                            Console.WriteLine($"Programme: {student.Programme}");
                            Console.WriteLine("Courses and Marks:");
                            foreach (var course in student.Courses)
                            {
                                Console.WriteLine($"{course.CourseName}: {course.Mark}");
                            }
                            Console.WriteLine($"Average Mark: {averageMark}");
                            Console.WriteLine($"Pass/Fail: {(isPass ? "Pass" : "Fail")}");

                            // Clear the XML file
                            File.Delete($"student{fileName}.xml");
                        }
                    }
                    finally
                    {
                        _bufferLock.Release();
                    }
                }
                else
                {
                    // Failed to acquire the lock, buffer is still in use by the producer
                    // Add some delay to give the producer time to produce more items
                    Thread.Sleep(100);
                }

                if (isBufferEmpty)
                {
                    // Add some delay to give the producer time to produce more items
                    Thread.Sleep(100);
                }
            }
        }

        private ITStudent ConvertXmlToStudent(string xmlData)
        {
            // Implement your logic to parse XML data and convert it back to ITStudent object
            // For simplicity, I'm using XML deserialization here
            XmlSerializer serializer = new XmlSerializer(typeof(ITStudent));
            using StringReader reader = new StringReader(xmlData);
            return (ITStudent)serializer.Deserialize(reader);
        }

        public void SetProducerFinished()
        {
            _producerFinished = true;
        }
    }
}
