{$reference ..\bin\x86_x64\UModbus.Client.dll}


uses
  System,
  UModbus.Client;


begin
  var client          := new ModbusUdpClient('192.168.1.100:502');
  client.SlaveAddress := 0;
  client.Timeout      := 1000;
  
  if client.Connect() then
    begin
      var DevInfo := client.ReadDeviceInfoBasic();
      if DevInfo.Status = RequestStatus.Valid then
        Console.WriteLine(DevInfo.ToString())
      else
        Console.WriteLine('error: '+DevInfo.Status.ToString());
      
      var BoardTemp := client.ReadHoldingRegisters($0013, 1);
      if BoardTemp.Status = RequestStatus.Valid then
        Console.WriteLine(String.Format('T = {0:g}*C', BoardTemp.Data[0]))
      else
        Console.WriteLine('error: '+BoardTemp.Status.ToString());
      
      client.Disconnect();
    end
  else
    Console.WriteLine(String.Format('connect to {0:s}:{1:D} fail.', client.IpAddress, client.Port));
end.