using System.Threading.Tasks;

namespace Cryptex.Services.DiffieHellman
{
    public interface IDemoDHCryptography
    {
        int G { get; set; }
        int P { get; set; }
        int PrivateA { get; set; }
        int PrivateB { get; set; }
        int PublicA { get; }
        int PublicB { get; }
        int KeyA { get; set; }
        int KeyB { get; set; }
        Task<int> CalculatePublicA();
        Task<int> CalculatePublicB();
        Task<int> CalculateKeyA();
        Task<int> CalculateKeyB();
    }
}