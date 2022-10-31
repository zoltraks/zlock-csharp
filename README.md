ZLock Command Utility
=====================

Execute process with exclusive file lock.

Program written for .NET.

Download compiled binaries from [→ here](download/Release.7z).

Help
----

```
zlock --help
```

```
USAGE

    zlock [options] <lock> <command> [<arguments>...]

OPTIONS

    -h
    --help
                             display this help

    -V
    --version
                             display version

    -v
    --verbose
                             increase verbosity

    -n
    --nb
    --nonblock
                             fail rather than wait

    -w <secs>
    --wait <secs>
    --timeout <secs>
                             wait for a limited amount of time

    -E <number>
    --conflict-exit-code <number>
    --lock-exit-code <number>
                             exit code after conflict on lock file

    -L <number>
    --limit-exit-code <number>
                             exit code after execution limit reached

    -t <secs>
    --time-limit <secs>
                             limit execution time

    -k
    --keep
                             don't delete lock file

    -q
    --quiet
                             don't print anything

AUTHORS

    Filip Golewski

```

Usage
-----

Eternaly wait to acquire lock ``one.lock`` and execute command ``ping -t 0.0.0.0``.

```
zlock one.lock ping -t 0.0.0.0
```

Run command with limited time of execution set to 10 seconds.

```
zlock one.lock -t 10 ping -t 0.0.0.0
```

Wait for 5 seconds to acquire lock ``one.lock`` or fail.

Be verbose.

```
zlock -v -w 5 one.lock ping -t 0.0.0.0
```

Compilation
-----------

In case you need to compile without support for legacy .NET 2.0 change configuration in csproj file by removing ``net20``.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>

    <TargetFrameworks>net40;netcoreapp3.1;net20</TargetFrameworks>
```

```xml
    <TargetFrameworks>net40;netcoreapp3.1</TargetFrameworks>
```

For recent Linux distribution use netcoreapp6.0.

```xml
    <TargetFrameworks>netcoreapp6.0</TargetFrameworks>
```
