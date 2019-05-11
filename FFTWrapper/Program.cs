using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FFTWSharp;

namespace FFTWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] input = { 1.2,3.4,5.6,7.8};
            double[] output ;

            //for (int i = 0; i < 16000; i++)
            //{
            //    input[i] = 0.002249;
            //}
            output = FFT(input);
        }

        static double[] FFT(double[] data,bool real = true)
        {
            if(real)
            {
                data = ToComplex(data);
            }
            int n = data.Length;
            //allocate an unmanaged memory block for input and output data..both are of same size we can use single block for both
            IntPtr ptr = fftw.malloc(n * sizeof(double));

            //pass the managed input data to unmanaged block
            Marshal.Copy(data, 0, ptr, n);

            //plan the fft and execute it
            //n/2 because fftw numbers are saved as pairs of real and imaginary
            IntPtr plan = fftw.dft_1d(n / 2, ptr, ptr, fftw_direction.Forward, fftw_flags.Estimate);
            fftw.execute(plan);

            //create an array to store the output values
            var fft = new double[n];

            //pass the unmanaged output data to managed array
            Marshal.Copy(ptr, fft, 0, n);

            //do some cleaning
            fftw.destroy_plan(plan);
            fftw.free(ptr);
            fftw.cleanup();

            return fft;
        }
        static double[] ToComplex(double[] real)
        {
            var n = real.Length;
            double[] complex = new double[2 * n];
            for (int i = 0; i < n; i++)
            {
                complex[2 * i] = real[i];
            }
            return complex;
        }
    }
}
