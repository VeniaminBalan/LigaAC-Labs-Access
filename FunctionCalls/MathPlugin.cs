using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;

namespace FunctionCalls;


public class MathPlugin
{
    [KernelFunction, Description("Add two numbers.")]
    public double Add(double a, double b)
    {
        return a + b;
    }
    [KernelFunction, Description("Subtract two numbers.")]
    public double Subtract(double a, double b)
    {
        return a - b;
    }
    [KernelFunction, Description("Multiply two numbers.")]
    public double Multiply(double a, double b)
    {
        return a * b;
    }
    [KernelFunction, Description("Divide two numbers.")]
    public double Divide(double a, double b)
    {
        if (b == 0) throw new DivideByZeroException("Cannot divide by zero.");
        return a / b;
    }
}
