using System;
using System.IO; 
using System.Collections.Generic;

namespace HW1
{
    class Program
    {
        private static List<Coordinate> ListofData = new List<Coordinate>();
        
        struct Coordinate
        {
            int x;
            int y;
            public Coordinate(int inputX,int inputY)
            {
                x=inputX;
                y=inputY;
            }
            public Coordinate(string inputX,string inputY)
            {
                x=Int32.Parse(inputX);
                y=Int32.Parse(inputY);
            }
            public override string ToString()
            {
                return "["+x+","+y+"]";
            }
        }

    
        static void Main(string[] args)
        {
            ReadFile();
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
                ListofData.Add(new Coordinate(input[0],input[1]));
            }            
            foreach(Coordinate c in ListofData)
            {
                index++;
                Console.Write(index+": ");
                Console.WriteLine(c);
            }
              
            // to close the stream 
            sr.Close();  
         
        }

        //E : set of examples, each of the form <X1, X2, X3, …, Y>
        //eta : learning rate.
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
}
