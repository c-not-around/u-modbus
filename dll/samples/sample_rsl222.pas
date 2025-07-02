{$reference ..\bin\x86_x64\UModbus.Client.dll}


uses
  System,
  System.IO,
  System.Threading,
  System.Text,
  UModbus.Client;


const
  LOG_FILE   = 'rsl222-mb.log';
  MB_REQ_GAP = 500;


procedure LogAppend(message: string);
begin
  Console.WriteLine(message);
  &File.AppendAllText(LOG_FILE, DateTime.Now.ToString('yyyy-MM-dd HH:mm:ss.fff') + #9 + message + #13#10);
end;

begin
  var client          := new ModbusRtuClient('COM1:4800:8N1');
  client.SlaveAddress := 1;
  client.Timeout      := 1000;
  
  if client.Connect() then
    begin
      var msg  := 'req: f17; resp: ';
      var desc := client.UserRequest($11, new byte[0]);
      if desc.Status = RequestStatus.Valid then
        begin
          var len  := desc.Data[0];
          var text := Encoding.ASCII.GetString(desc.Data, 1, len);
          LogAppend(msg+'"'+text+'"');
        end
      else
        LogAppend(msg+'error - '+desc.Status.ToString());
      
      Thread.Sleep(MB_REQ_GAP);
      
      for var a := -1 to 1 do
        begin
          var adr  := $C072 + a;
          msg      := String.Format('req: f04,0x{0:X4},2; resp: ', adr);
          var resp := client.ReadInputRegisters(adr, 2);
          if resp.Status = RequestStatus.Valid then
            begin
              msg += 'data[';
              for var i := 0 to resp.Raw.Length-1 do
                msg += ' 0x' + resp.Raw[i].ToString('X2');
              msg += ' ]';
              LogAppend(msg);
            end
          else
            LogAppend(msg+'error - '+resp.Status.ToString());
            
          Thread.Sleep(MB_REQ_GAP);
          
          adr  := $C074 + a;
          msg  := String.Format('req: f04,0x{0:X4},4; resp: ', adr);
          resp := client.ReadInputRegisters(adr, 4);
          if resp.Status = RequestStatus.Valid then
            begin
              msg += 'data[';
              for var i := 0 to resp.Raw.Length-1 do
                msg += ' 0x' + resp.Raw[i].ToString('X2');
              msg += ' ]';
              LogAppend(msg);
            end
          else
            LogAppend(msg+'error - '+resp.Status.ToString());
            
          Thread.Sleep(MB_REQ_GAP);
        end;
      
      client.Disconnect();
    end
  else
    LogAppend(
      String.Format(
        'connect to {0:s}:{1:D}:8{2:c}{3:D} fail.', 
        client.Port, 
        client.Baudrate, 
        client.Parity.ToString()[1], 
        integer(client.StopBits)
      )
    );
  
  LogAppend('DONE!');
end.