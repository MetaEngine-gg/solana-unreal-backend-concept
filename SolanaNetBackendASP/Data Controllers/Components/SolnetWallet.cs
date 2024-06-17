using Solnet.Wallet;
using Solnet.Wallet.Bip39;

namespace SolanaNetBackendASP.Data_Controllers.Components;

public static class SolnetWallet
{
    public static Wallet CreateWallet()
    {
        var wordList = WordList.English;
        var mnemonic = GenerateMnemonic(wordList, WordCount.TwentyFour);
        var wallet = new Wallet(mnemonic, wordList);

        return wallet;
    }

    public static string GetPrivateKey(Wallet wallet)
    {
        return wallet.Account.PrivateKey;
    }
    
    private static string GenerateMnemonic(WordList wordList, WordCount wordCount)
    {
        var mnemonic = new Mnemonic(wordList, wordCount);
        return mnemonic.ToString();
    }
}