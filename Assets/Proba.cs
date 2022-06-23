using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proba
{
    static double stdDev = 1.0F;


    // generates a random value following the normal probability distribution given a mean and a standard deviation
    public static double nextGaussian(float distance)
    {
        double sensedDistance;
        
        double mean = (double)distance;
        System.Random rand = new System.Random();

        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();

        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

        sensedDistance =
                mean + stdDev * randStdNormal;  //random normal(mean,stdDev^2)

        if (sensedDistance < 0) sensedDistance = 0;
        return sensedDistance;
    }


    // calculates the value of the normal distribution function for a value
    // given a mean and a standard deviation
    public static double NormalFunction(double value, double mean){

        // normal distribution function f(x)
        // f(x) = (e^(-(value-mean)^2/(2*stdDev^2))/(stdDev*sqrt(2*pi))
        double a = -Math.Pow((value-mean),2f)/(2f*Math.Pow(stdDev,2f));
        double b = stdDev * Math.Sqrt(2*Mathf.PI);
        double f = (Math.Exp(a)/b);
        return f;
    }

    // computes an approximation of a probability for a color (range of distances) from the Normal Function
    //  by computing the integral of that function between the two values that delimit the range of distances
    // It does so using the Monte Carlo method of computation that relies on repeated random sampling of the function
    public static double MonteCarloProbability(double start, double end, double mean){
        int n_iterations = 100;
        double sum = 0;
        for(int i = 0; i < n_iterations ; i++){
            sum+= NormalFunction(RandomDouble(start,end),mean);
        }
        return  (sum/n_iterations)*Math.Abs(start-end);
    }

    // generates a random float number between a and b
    public static double RandomDouble(double a, double b){
        System.Random rand = new System.Random();
        return (double) (Math.Abs(a-b)*rand.NextDouble()+Math.Min(a,b));

    }

}
