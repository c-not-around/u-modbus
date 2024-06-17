{$reference UModbus.dll}


uses
  System,
  System.Text,
  UModbus;


begin
  var client          := new ModbusRtuClient('COM1:4800:8N1');
  client.SlaveAddress := 1;
  client.Timeout      := 1000;
  
  if client.Connect() then
    begin
      var desc := client.UserRequest($11, new byte[0]);
      if desc.Status = RequestStatus.Valid then
        begin
          var len  := desc.Data[0];
          var text := Encoding.ASCII.GetString(desc.Data, 1, len);
          Console.WriteLine(text);
        end
      else
        Console.WriteLine('error: '+desc.Status.ToString());
      
      client.Disconnect();
    end
  else
    Console.WriteLine(
      String.Format(
        'connect to {0:s}:{1:D}:8{2:c}{3:D} fail.', 
        client.Port, 
        client.Baudrate, 
        client.Parity.ToString()[1], 
        integer(client.StopBits)
      )
    );
end.