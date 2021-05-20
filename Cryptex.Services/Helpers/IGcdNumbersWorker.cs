namespace Cryptex.Services.Helpers
{
    public interface IGcdNumbersWorker
    {
        long GcdExtended(long a, long b, ref long x, ref long y);
        long Gcd(long a, long b);
    }
}