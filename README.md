# BakedEnv

Compatible with .NET 6 or higher.

![license](https://img.shields.io/github/license/zeplar-exe/BakedEnv)
![version](https://img.shields.io/nuget/v/BakedEnv)
![build](https://github.com/zeplar-exe/BakedEnv/actions/workflows/dotnet.yml/badge.svg)
![coverage](https://img.shields.io/codecov/c/github/zeplar-exe/BakedEnv)

[See the change log](...)

BakedEnv is a scripting language made in C#, for C#. Designed after Lua, it is built for extensibility between C# and
written scripts.

"The Lua of C#" as I like to call it. BakedEnv can sate all of your needs for an embedded scripting language in C#. From game
nodding to your own mini scripting language, it is applicable to a wide range of uses. BakedEnv is focused to be as extensible
as possible, allowing for the creation, implementation, and modification of core behavior. Additionally, the ease of access for
cross-language typing via functions and objects contribute to its applicability.

## Features

- User defined keywords
- User defined statements and control flow
- User defined objects and types
- User defined processor directives/statements
- C# interop-ability
- Extensible System.Object-BakedObject conversion
- Code injection into internal parse processes
- Direct access to the interpreter
- API/library injection
- Extensions to skip the boring environment setup
- Extensible variable/member access handling

[Try the live demo](...)

## Getting Started

### [Nuget](https://www.nuget.org/packages/BakedEnv/)

The library can be installed via [nuget](https://www.nuget.org/packages/BakedEnv/).

### Clone/Self-Build

An initial build is required to execute any source generators present. Errors will otherwise
appear in the solution until the next build.

## Samples

- [Functions - samples/function-caller.md](./samples/function-caller.md)

## Featured Uses

If you want to get your project added to this list, simply make a pull request.

*crickets chirp aggressively*

## License

This project is licensed under the **BSD 3-Clause License** - see the [LICENSE](LICENSE) file for more info. 
