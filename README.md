# UModbus

UModbus - .NET library for working with devices via the Modbus protocol, as well as a Modbus client tool based on it.

## UModbus.Client library

Capabilities:
* Modbus full implementation. All standard functions of the Modbus protocol are implemented, as well as the ability to execute user requests (functions designed by the device manufacturer).
* Support for all transport level options for Modbus: RTU, ASCII, TCP, UDP.
* Keeping a log of data exchange with a remote device.

## UModbus client tool

Capabilities:
* Supported functions:
   * `FC01` - `READ_COIL_STATUS`        - reading one/several coils
   * `FC02` - `READ_DISCRETE_INPUTS`    - reading one/several inputs
   * `FC03` - `READ_HOLDING_REGISTERS`  - reading one/multiple holding registers
   * `FC04` - `READ_INPUT_REGISTERS`    - reading one/multiple input registers
   * `FC05` - `WRITE_SINGLE_COIL`       - writing value of one value coil
   * `FC16` - `WRITE_MULTIPLE_REGISTER` - writing values to multiple holding registers
   * `FCxx` - Device manufacturer-defined function with a code from the Modbus protocol user function range.
* Support for all major data types:
   * `Int16`
   * `Uint16`
   * `Int32`
   * `Uint32`
   * `Int64`
   * `Uint64`
   * `Float32` (float)
   * `Float64` (double)
* Selection of byte order for variables longer than a 16-bit word
* Save data-map to `*.json` file
* Load data-map from `*.json` file
* Logging
* Cycle read
* Search for a slave device on a common bus

![screnn](https://github.com/c-not-around/u-modbus/assets/173079314/38e7d232-2193-437c-8977-8b41932a3a47)
