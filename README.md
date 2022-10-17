# PinGenerator

Creates a C++ header file and documentation for an Arduino project.

## C++ Header file

The header file is setup to create 3 main namespaces; `digtl`- Digital pins, `anlg` - analog pins, and `Ser` - serial communication pins. a nested namespace is used to group the different pins further.

### Example:

> project-pinout.h
> ```cpp
> namespace digtl {
>   namespace leds {
>     const int LEDA_PIN = 42;
>     const int LEDB_PIN = 24;
>     const int LEDC_PIN = 72;
>   };
> };
> ```

## Documentation

A markdown file is also generated with table formatting.

### Example:

> Project Pinout.md
> ## Digital
> | Name | Pin |
> |---|---|
> | LEDA | 42 |
> | LEDB | 24 |
> | LEDC | 72 |
