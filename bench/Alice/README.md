# Alice's Adventures in Wonderland

This folder contains a simple test executable which benchmark the performance of .NET regex against IronRure.

``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.471 (1803/April2018Update/Redstone4)
Intel Core i7-7600U CPU 2.80GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|               Method |  Job | Runtime |              Pattern |         Mean |        Error |       StdDev |       Median | Ratio | RatioSD | Rank | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|--------------------- |----- |-------- |--------------------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|-----:|------------:|------------:|------------:|--------------------:|
**DotnetRegex** |  **Clr** |     **Clr** | **([A-Z(...)zA-Z] [39]** |  **27,939.7 us** |   **543.415 us** |   **861.914 us** |  **27,679.6 us** |  **1.00** |    **0.00** |    **2** |           **-** |           **-** |           **-** |            **112167 B** |
| DotnetRegexNoCompile |  Clr |     Clr | ([A-Z(...)zA-Z] [39] |  32,640.6 us |   571.470 us |   506.594 us |  32,486.5 us |  1.16 |    0.04 |    3 |           - |           - |           - |            111957 B |
|            RustRegex |  Clr |     Clr | ([A-Z(...)zA-Z] [39] |     644.0 us |    11.217 us |     9.366 us |     645.6 us |  0.02 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            194296 B |
|     RustRegexUnicode |  Clr |     Clr | ([A-Z(...)zA-Z] [39] |     645.9 us |     9.097 us |     7.596 us |     648.0 us |  0.02 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            194296 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          DotnetRegex | Core |    Core | ([A-Z(...)zA-Z] [39] |  25,951.2 us |   610.997 us | 1,302.085 us |  25,525.4 us |  1.00 |    0.00 |    2 |     31.2500 |           - |           - |            111904 B |
| DotnetRegexNoCompile | Core |    Core | ([A-Z(...)zA-Z] [39] |  30,481.0 us |   535.121 us |   474.370 us |  30,479.8 us |  1.12 |    0.08 |    3 |     31.2500 |           - |           - |            111904 B |
|            RustRegex | Core |    Core | ([A-Z(...)zA-Z] [39] |     519.2 us |     9.290 us |     8.690 us |     522.7 us |  0.02 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            194264 B |
|     RustRegexUnicode | Core |    Core | ([A-Z(...)zA-Z] [39] |     525.3 us |    10.229 us |     9.568 us |     525.9 us |  0.02 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            194264 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          **DotnetRegex** |  **Clr** |     **Clr** | **.{0,3(...)inah) [35]** | **113,598.7 us** |   **837.441 us** |   **699.301 us** | **113,597.7 us** | **1.000** |    **0.00** |    **2** |           **-** |           **-** |           **-** |            **131072 B** |
| DotnetRegexNoCompile |  Clr |     Clr | .{0,3(...)inah) [35] | 132,910.6 us | 2,572.743 us | 2,280.670 us | 133,658.2 us | 1.169 |    0.02 |    3 |           - |           - |           - |            131072 B |
|            RustRegex |  Clr |     Clr | .{0,3(...)inah) [35] |     648.2 us |    12.087 us |    11.307 us |     648.1 us | 0.006 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            197480 B |
|     RustRegexUnicode |  Clr |     Clr | .{0,3(...)inah) [35] |     651.9 us |    12.882 us |    12.652 us |     654.9 us | 0.006 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            197480 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          DotnetRegex | Core |    Core | .{0,3(...)inah) [35] |  99,323.6 us | 1,010.225 us |   895.538 us |  99,175.4 us | 1.000 |    0.00 |    2 |           - |           - |           - |            129760 B |
| DotnetRegexNoCompile | Core |    Core | .{0,3(...)inah) [35] | 143,089.4 us | 2,854.448 us | 4,527.457 us | 142,794.1 us | 1.445 |    0.06 |    3 |           - |           - |           - |            129760 B |
|            RustRegex | Core |    Core | .{0,3(...)inah) [35] |     536.2 us |    10.624 us |    14.182 us |     534.5 us | 0.005 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            197144 B |
|     RustRegexUnicode | Core |    Core | .{0,3(...)inah) [35] |     536.9 us |     7.922 us |     7.411 us |     540.3 us | 0.005 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            197144 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          **DotnetRegex** |  **Clr** |     **Clr** | **Alice(...)Alice [37]** |   **9,242.9 us** |   **188.440 us** |   **436.738 us** |   **9,057.5 us** |  **1.00** |    **0.00** |    **3** |           **-** |           **-** |           **-** |               **640 B** |
| DotnetRegexNoCompile |  Clr |     Clr | Alice(...)Alice [37] |   7,870.1 us |   135.770 us |   194.717 us |   7,874.1 us |  0.85 |    0.05 |    2 |           - |           - |           - |               640 B |
|            RustRegex |  Clr |     Clr | Alice(...)Alice [37] |     376.8 us |     2.883 us |     2.408 us |     376.6 us |  0.04 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            177912 B |
|     RustRegexUnicode |  Clr |     Clr | Alice(...)Alice [37] |     382.4 us |     4.798 us |     4.254 us |     381.5 us |  0.04 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            177912 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          DotnetRegex | Core |    Core | Alice(...)Alice [37] |   8,993.5 us |    54.987 us |    45.917 us |   9,002.9 us |  1.00 |    0.00 |    3 |           - |           - |           - |               608 B |
| DotnetRegexNoCompile | Core |    Core | Alice(...)Alice [37] |   7,815.1 us |   141.966 us |   132.795 us |   7,820.0 us |  0.87 |    0.02 |    2 |           - |           - |           - |               608 B |
|            RustRegex | Core |    Core | Alice(...)Alice [37] |     264.4 us |     9.852 us |     9.216 us |     261.8 us |  0.03 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            177664 B |
|     RustRegexUnicode | Core |    Core | Alice(...)Alice [37] |     263.2 us |     2.407 us |     2.010 us |     263.2 us |  0.03 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            177664 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          **DotnetRegex** |  **Clr** |     **Clr** |      **Alice|Adventure** |   **5,423.9 us** |    **52.777 us** |    **46.785 us** |   **5,413.9 us** |  **1.00** |    **0.00** |    **3** |     **39.0625** |           **-** |           **-** |             **96494 B** |
| DotnetRegexNoCompile |  Clr |     Clr |      Alice|Adventure |   3,465.2 us |    66.869 us |    71.549 us |   3,476.1 us |  0.64 |    0.01 |    2 |     42.9688 |           - |           - |             96489 B |
|            RustRegex |  Clr |     Clr |      Alice|Adventure |     374.1 us |     3.289 us |     2.916 us |     373.9 us |  0.07 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            194752 B |
|     RustRegexUnicode |  Clr |     Clr |      Alice|Adventure |     376.2 us |     3.066 us |     2.394 us |     376.2 us |  0.07 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            194752 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          DotnetRegex | Core |    Core |      Alice|Adventure |   5,043.2 us |   104.551 us |    92.682 us |   5,020.3 us |  1.00 |    0.00 |    3 |     31.2500 |           - |           - |             96472 B |
| DotnetRegexNoCompile | Core |    Core |      Alice|Adventure |   3,634.1 us |    47.997 us |    40.080 us |   3,640.9 us |  0.72 |    0.01 |    2 |     42.9688 |           - |           - |             96472 B |
|            RustRegex | Core |    Core |      Alice|Adventure |     272.1 us |     7.291 us |    21.152 us |     261.4 us |  0.06 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            194504 B |
|     RustRegexUnicode | Core |    Core |      Alice|Adventure |     260.8 us |     2.299 us |     2.151 us |     260.7 us |  0.05 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            194504 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          **DotnetRegex** |  **Clr** |     **Clr** |         **[a-zA-Z]+ing** |  **39,568.6 us** |   **221.802 us** |   **185.215 us** |  **39,536.4 us** |  **1.00** |    **0.00** |    **2** |     **76.9231** |           **-** |           **-** |            **258410 B** |
| DotnetRegexNoCompile |  Clr |     Clr |         [a-zA-Z]+ing |  39,526.8 us |   483.777 us |   403.975 us |  39,561.9 us |  1.00 |    0.01 |    2 |     76.9231 |           - |           - |            258410 B |
|            RustRegex |  Clr |     Clr |         [a-zA-Z]+ing |     743.6 us |     9.442 us |     8.370 us |     744.8 us |  0.02 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            221144 B |
|     RustRegexUnicode |  Clr |     Clr |         [a-zA-Z]+ing |     749.0 us |     3.459 us |     3.067 us |     749.6 us |  0.02 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            221144 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          DotnetRegex | Core |    Core |         [a-zA-Z]+ing |  36,429.0 us |   318.464 us |   265.932 us |  36,440.8 us |  1.00 |    0.00 |    2 |     71.4286 |           - |           - |            257960 B |
| DotnetRegexNoCompile | Core |    Core |         [a-zA-Z]+ing |  40,295.8 us | 4,055.937 us | 5,945.140 us |  38,018.9 us |  1.19 |    0.22 |    3 |     71.4286 |           - |           - |            257960 B |
|            RustRegex | Core |    Core |         [a-zA-Z]+ing |     625.1 us |     7.850 us |     6.958 us |     625.6 us |  0.02 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            220824 B |
|     RustRegexUnicode | Core |    Core |         [a-zA-Z]+ing |     625.4 us |    10.622 us |     9.416 us |     628.7 us |  0.02 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            220824 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          **DotnetRegex** |  **Clr** |     **Clr** |       **\b(\w+)-legged** |  **42,229.6 us** |   **769.165 us** |   **681.844 us** |  **41,982.1 us** |  **1.00** |    **0.00** |    **3** |           **-** |           **-** |           **-** |              **1365 B** |
| DotnetRegexNoCompile |  Clr |     Clr |       \b(\w+)-legged |  47,638.8 us |   894.243 us |   836.476 us |  47,380.3 us |  1.13 |    0.03 |    4 |           - |           - |           - |               745 B |
|            RustRegex |  Clr |     Clr |       \b(\w+)-legged |     567.3 us |     6.071 us |     5.381 us |     566.8 us |  0.01 |    0.00 |    1 |     54.6875 |     54.6875 |     54.6875 |            177912 B |
|     RustRegexUnicode |  Clr |     Clr |       \b(\w+)-legged |   7,733.4 us |   118.493 us |   110.839 us |   7,711.3 us |  0.18 |    0.00 |    2 |     54.6875 |     54.6875 |     54.6875 |            177968 B |
|                      |      |         |                      |              |              |              |              |       |         |      |             |             |             |                     |
|          DotnetRegex | Core |    Core |       \b(\w+)-legged |  39,175.7 us |   641.499 us |   568.672 us |  39,052.9 us |  1.00 |    0.00 |    3 |           - |           - |           - |               688 B |
| DotnetRegexNoCompile | Core |    Core |       \b(\w+)-legged |  45,478.8 us |   840.672 us |   786.365 us |  45,561.8 us |  1.16 |    0.02 |    4 |           - |           - |           - |               688 B |
|            RustRegex | Core |    Core |       \b(\w+)-legged |     454.3 us |     6.174 us |     5.473 us |     451.7 us |  0.01 |    0.00 |    1 |     55.1758 |     55.1758 |     55.1758 |            177664 B |
|     RustRegexUnicode | Core |    Core |       \b(\w+)-legged |   7,506.6 us |    32.266 us |    28.603 us |   7,504.4 us |  0.19 |    0.00 |    2 |     54.6875 |     54.6875 |     54.6875 |            177664 B |
