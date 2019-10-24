using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HW1
{
    class Program
    {
        private static List<Coordinate> ListofData = new List<Coordinate>();
         private static List<float> bestWeights = new List<float>();
        private static float learningRate;

        public static float bestSSE,bestETa;
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
            float positiveInf = 1 / 0.0f;;
            bestSSE=positiveInf;

            ReadFile();
            for(int i =0;i<100;i++)
            {   
                //int ran=(int)(GenerateRandomNumber())%100;
                LinearLearner(2);
                if(i%10 == 0)
                {
                    Console.WriteLine("Iteration: "+ i);
                }
            }
            
            Console.WriteLine("--------------");
            
            foreach (Coordinate c in ListofData)
            {
                float yCap=0.0f;
                foreach(float w in bestWeights)
                {
                    yCap+=w*c.x;
                }  
                float delta = (float)(c.y) - yCap;
                string str= c.y +" + " + yCap + " = "  + delta;
                Console.WriteLine(str);

            }
            Console.WriteLine("--------------");
            Console.WriteLine("bestweights");
            int index=0;
            foreach(float w in bestWeights)
            {
                
                Console.WriteLine("weight"+index+": "+ w);
                index++;
            }
            Console.WriteLine("bestsumofSquaresError: "+bestSSE);
            Console.WriteLine("bestLearningRate: "+bestETa);
            /* 
            for(int i=0;i<50;i++)
            {
                float test = GenerateRandomFloat(0.00f,1.00f,5);
                Console.WriteLine(i + ": "+ test);
            }*/

        }

        private static void  ReadFile()
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
            /* 
            int index = 0;
            foreach (Coordinate c in ListofData)
            {
                index++;
                Console.Write(index + ": ");
                Console.WriteLine(c);
            }*/

            // to close the stream 
            sr.Close();

        }

        public static void LinearLearner(int numWeights)
        {
            //E : set of examples, each of the form <X1, X2, X3, …, Y>
            //at main function
            //eta : learning rate.
            //learningRate = 0.0000001f;
            learningRate = GenerateRandomFloat(0.0000000f,0.000001f, 12);
            //Console.WriteLine("ETA: "+learningRate);
            //initialize w0,…,wn randomly
            List<float> weights = new List<float>();

            for(int i=0;i<numWeights;i++)
            {
                weights.Add(GenerateRandomFloat(0.00f, 100.00f, 5));
            }
            
            float ycap;   

            for (int i = 0; i < 10000; i++)
            {

                //REPEAT
                foreach (Coordinate c in ListofData)
                {
                    //Compute yCap ( from w[0], w[1] and E[k] )
                    //Ycap := wi * Xi(e)
                    ycap=0.0f;
                    foreach(float w in weights)
                    {
                        ycap+=w*c.x;
                    }  
                    //delta :=  Ex[k].y - yCap
                    float delta = (float)(c.y) - ycap;

                    //        delta := Y(e) - Ycap
                    //        update := eta * delta
                    for(int j = 0;j<weights.Count;j++)
                    {
                        //wi := wi + update * Xi(e)
                        //w[i] := w[i]  +  eta * delta * E[k].x[i] 
                        //float sumofSquaresError=(float)Math.Pow(delta,2);
                        weights[j] = weights[j] + learningRate *delta * c.x;
                    }

                }
                

            }
            //sum of squares
            float sumofSquaresError=0.0f;
            foreach (Coordinate c in ListofData)
            {
                ycap=0.0f;
                foreach(float w in weights)
                {
                    ycap+=w*c.x;
                }  
                float delta = (float)(c.y) - ycap;
                sumofSquaresError+=(float)Math.Pow(delta,2);
            }

            //Console.WriteLine("w0: "+weights[0]);
            //Console.WriteLine("w1: "+weights[1]);
            //Console.WriteLine("sumofSquaresError: "+sumofSquaresError);
            if(sumofSquaresError < bestSSE)
            {
                bestWeights=weights;
                bestSSE = sumofSquaresError;
                bestETa=learningRate;
            }
            //algo
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
