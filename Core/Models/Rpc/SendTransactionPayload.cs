namespace Core.Models.Rpc;

[Serializable]
public struct SendTransactionPayload
{
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public ulong Amount { get; set; }
}