using MessageSendingService;

Console.WriteLine("Start sending messages");

////روش ساده
//Console.WriteLine("Execute simple way");
//SimpleSending simpleSending = new SimpleSending();
//await simpleSending.SendAsync();

//روش منصفانه
Console.WriteLine("Execute fairly way");
FairlySending fairlySending = new FairlySending();
await fairlySending.SendAsync();

Console.ReadKey();