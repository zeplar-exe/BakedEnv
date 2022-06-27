# BakedEnv

![build](https://github.com/zeplar-exe/BakedEnv/actions/workflows/dotnet.yml/badge.svg)

BakedEnv is a scripting language made in C#, for C#. Designed after Lua, BakedEnv is built for extensibility between C# and
written scripts.

## Getting Started

### Dependencies

* .NET 6

### Installation

#### API

The C# library can be installed via [nuget](https://www.nuget.org/packages/BakedEnv/) or inclusion of the desired version's DLL in your project. 
Learn more about the latter [here](https://stackoverflow.com/questions/7685718/).

#### CLI

Every version of the project's CLI can be found under [releases](https://github.com/zeplar-exe/BakedEnv/releases/).
After downloading, the exe must be placed in the system PATH. Learn more [here](https://stackoverflow.com/questions/4822400/).

### Usage

#### API

The highest level classes for interaction with the language are `BakedEnvironment` and `BakedInterpreter`. 
The latter providing a more direct approach.

Samples coming soon, I swear.

#### CLI

Assuming the exe is accessible from your command line, commands can be used as below:

```shell
> ben execute -r
```

`ben execute` is the most commonly used command here. It can execute BakedEnv scripts on the fly
from the command line or an external file.

```shell
> ben interactive
```

While devoid of function at the moment, `ben interactive` will be useful for interactive shell scripting
and enhanced debugging.

Run `ben [...] --help` for extensive command information.

## License

This project is licensed under the **BSD 3-Clause License** - see the [LICENSE](LICENSE) file for details