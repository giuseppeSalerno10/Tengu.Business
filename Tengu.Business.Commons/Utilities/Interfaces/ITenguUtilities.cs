namespace Tengu.Business.Commons
{
    public interface ITenguUtilities
    {
        int DamerauLevenshteinDistance(string firstText, string secondText);
        int Minimum(int a, int b);
        int Minimum(int a, int b, int c);
    }
}