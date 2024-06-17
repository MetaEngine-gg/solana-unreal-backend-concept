using Solnet.Wallet;
using Solnet.Wallet.Bip39;

namespace SolanaNetBackendASP.Data_Controllers.Components;

public static class SolnetWallet
{
    public static Wallet CreateWallet()
    {
        // create new mnemonic
        // To initialize a wallet and have access to the same keys generated in sollet (the default)
        var wordList = WordList.English;
        
        var mnemonic = GenerateMnemonic(wordList, WordCount.TwentyFour);
        var wallet = new Wallet(mnemonic, wordList);

        return wallet;
    }
    
    
    private static string GenerateMnemonic(WordList wordList, WordCount wordCount)
    {
        var mnemonic = new Mnemonic(wordList, wordCount);
        return mnemonic.ToString();
    }
}