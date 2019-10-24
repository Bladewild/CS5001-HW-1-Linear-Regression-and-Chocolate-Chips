using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HW1
{
    class Program
    {
        private static List<Coordinate> ListofData = new List<Coordinate>();
        private static float learningRate;

        struct Coordinate
        {
            int x;
            int y;
            public Coordinate(int inputX, int inputY)
            {
                x = inputX;
                y = inputY;
            }
            public Coordinate(string inputX, string inputY)
            {
                x = Int32.Parse(inputX);
                y = Int32.Parse(inputY);
            }
            public override string ToString()
            {
                return "[" + x + "," + y + "]";
            }
        }


        static void Main(string[] args)
        {
            //LinearLearner();
            for(int i=0;i<50;i++)
            {
                float test = GenerateRandomFloat(0.00f,1.00f,5);
                Console.WriteLine(i + ": "+ test);
            }

        }

        private static void ReadFile()
        {
            // Takinga a new input stream i.e.  
            // geeksforgeeks.txt and opens it 
            StreamReader sr = new StreamReader("chocodata.txt");


            // This is use to specify from where  
            // to start reading input stream 
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            string[] input;


            int index = 0;
            while (sr.EndOfStream == false)
            {
                input = sr.ReadLine().Split('	');
                ListofData.Add(new Coordinate(input[0], input[1]));
            }
            foreach (Coordinate c in ListofData)
            {
                index++;
                Console.Write(index + ": ");
                Console.WriteLine(c);
            }

            // to close the stream 
            sr.Close();

        }

        public static void LinearLearner()
        {
            //E : set of examples, each of the form <X1, X2, X3, …, Y>
            ReadFile();
            //eta : learning rate.
            learningRate= 0.5f;
            //initialize w0,…,wn randomly
            //REPEAT
            //    FOR EACH example e in E D
            //        Ycap := wi * Xi(e)
            //        delta := Y(e) - Ycap
            //        update := eta * delta
            //        FOR EACH wi DO
            //            wi := wi + update * Xi(e)
            //UNTIL some stop criteria is true
            //RETURN w0,…,wn

        }

        //assumes floats have the same descimal places.
        static float GenerateRandomFloat(float min,float max,int upto)
        {
            //convert max and min to decimal
            //find the decimal places, use that make max to be an int
            // mod that int to randomInteger, get result and convert back to float
            // add min to float

            decimal maxDecimal = (decimal)max;
            //Console.WriteLine("maxInteger: "+maxDecimal);

            //experiment later, for now set
            //int count = BitConverter.GetBytes(decimal.GetBits(maxDecimal)[3])[2];
            int count = upto;

            //Console.WriteLine("count: "+count);
            double powOf=Math.Pow(10f,count);
            int maxInteger= (int)(max*powOf); 
            //Console.WriteLine("maxInteger: "+maxInteger);

            
            int randomInteger = Math.Abs(unchecked((int)GenerateRandomNumber()));
            
            
            int moddedRandom= randomInteger % maxInteger;
            float randomFloat= moddedRandom/((float)powOf);

            float result = (randomFloat) + min;
            return result;
        }
        static uint GenerateRandomNumber()
        {
            //Sourced from https://stackify.com/csharp-random-numbers/ 
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            //convert 4 bytes to an integer
            var byteArray = new byte[4];
            provider.GetBytes(byteArray);
            uint randomInteger = BitConverter.ToUInt32(byteArray, 0);
            return randomInteger;
        }
        
    }
}
