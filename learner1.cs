using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace HW1
{
    class Program
    {
        private static List<Coordinate> ListofData = new List<Coordinate>();
        private static List<Coordinate> ListofValidData = new List<Coordinate>();
        private static float learningRate;
        public static int bestIteration;

        public static float bestSSE, bestETa, bestw0, bestw1;
        public static int numIterations = 5000;
        struct Coordinate
        {
            public int x;
            public int y;
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
            float positiveInf = 1 / 0.0f; ;
            bestSSE = positiveInf;
            ReadFile();
            for (int i = 1; i <= 10000; i++)
            {
                LinearLearner(i);
                if (i % 100 == 0)
                {
                    Console.WriteLine("Iteration: " + i);
                }
            }

            Console.WriteLine("--------------");

            foreach (Coordinate c in ListofValidData)
            {
                float ycap = 0.0f;
                ycap += bestw0 * 1;
                ycap += bestw1 * c.x;
                float delta = (float)(c.y) - ycap;
                string str = c.y + " + " + ycap + " = " + delta;
                Console.WriteLine(str);
            }

            Console.WriteLine("--------------");
            Console.WriteLine("bestweights");

            Console.WriteLine("w0: " + bestw0);
            Console.WriteLine("w1: " + bestw1);

            Console.WriteLine("bestsumofSquaresError: " + bestSSE);
            Console.WriteLine("bestLearningRate: " + bestETa);
            Console.WriteLine("bestIteration: " + bestIteration);

            //output
            StreamWriter file = new System.IO.StreamWriter("learner1output.txt");
            file.WriteLine("CS-5001: HW#1");
            file.WriteLine("Programmer: Alain Markus P. Santos-Tankia\n");
            file.WriteLine("Using learning rate eta = "+bestETa);
            file.WriteLine("Using "+numIterations+" iterations.\n");
            file.WriteLine("OUPUT");
            file.WriteLine("w0: " + bestw0);
            file.WriteLine("w1: " + bestw1+"\n");
            file.WriteLine("Validation");
            file.WriteLine("Sum-of-Squares Error = "+bestSSE);
            file.Close();

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



            while (sr.EndOfStream == false)
            {
                input = sr.ReadLine().Split('	');
                ListofData.Add(new Coordinate(input[0], input[1]));
            }
            // to close the stream 
            sr.Close();

            //
            sr = new StreamReader("chocovalid.txt");


            // This is use to specify from where  
            // to start reading input stream 
            sr.BaseStream.Seek(0, SeekOrigin.Begin);


            while (sr.EndOfStream == false)
            {
                input = sr.ReadLine().Split('	');
                ListofValidData.Add(new Coordinate(input[0], input[1]));
            }

            // to close the stream 
            sr.Close();


        }

        public static void LinearLearner(int iteration)
        {
            //E : set of examples, each of the form <X1, X2, X3, …, Y>
            //at main function
            //eta : learning rate.
            learningRate = GenerateRandomFloat(0.0000000f, 0.00002f, 12);

            float w0 = GenerateRandomFloat(0.00f, 100.00f, 5);
            float w1 = GenerateRandomFloat(0.00f, 100.00f, 5);

            float ycap;

            for (int i = 0; i < numIterations; i++)
            {

                //REPEAT
                foreach (Coordinate c in ListofData)
                {
                    //Compute yCap ( from w[0], w[1] and E[k] )
                    //Ycap := wi * Xi(e)
                    ycap = 0.0f;

                    ycap += w0 * 1;
                    ycap += w1 * c.x;

                    //delta :=  Ex[k].y - yCap
                    float delta = (float)(c.y) - ycap;

                    //        delta := Y(e) - Ycap
                    //        update := eta * delta
                    w0 = w0 + learningRate * delta * 1;
                    w1 = w1 + learningRate * delta * c.x;

                }


            }
            //sum of squares

            //use validation
            float sumofSquaresError = 0.0f;
            foreach (Coordinate c in ListofValidData)
            {
                ycap = 0.0f;
                ycap += w0 * 1;
                ycap += w1 * c.x;

                float delta = (float)(c.y) - ycap;
                sumofSquaresError += (float)Math.Pow(delta, 2);
            }
            
            //use validation
            if (sumofSquaresError < bestSSE)
            {
                bestIteration = iteration;
                bestw0 = w0;
                bestw1 = w1;
                bestSSE = sumofSquaresError;
                bestETa = learningRate;
            }

        }

        //assumes floats have the same descimal places.
        static float GenerateRandomFloat(float min, float max, int upto)
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
            double powOf = Math.Pow(10f, count);
            int maxInteger = (int)(max * powOf);
            //Console.WriteLine("maxInteger: "+maxInteger);


            int randomInteger = Math.Abs(unchecked((int)GenerateRandomNumber()));


            int moddedRandom = randomInteger % maxInteger;
            float randomFloat = moddedRandom / ((float)powOf);

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
