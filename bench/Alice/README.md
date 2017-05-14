# Alice's Adventures in Wonderland

This folder contains a simple test executable which benchmarks the performance of .NET regex against IronRure. Each benchmark measures how long, in `Stopwatch` ticks, it takes to find all matches of a given pattern in the entire Project Gutenberg copy of *Alice's Adventures in Wonderland* by *Lewis Carroll*.

An example set of test results, run on a 4-core i7 MacBook Pro:

```
legged (\b(\w+)-legged)
   rure::legged:     251899,    267830,    268564,   3220008 (mean:    1002075, median:     268197, r: 1280541.38), 19.0x slower
   byts::legged:      11104,     11890,     14934,    190538 (mean:      57116, median:      13412, r: 77044.22), Winner
   .net::legged:   39318866,  39325767,  42431311,  45336936 (mean:   41603220, median:   40878539, r: 2501571.27), 3046.9x slower
alice (Alice)
    rure::alice:     300305,    301274,    304523,    464942 (mean:     342761, median:     302898, r: 70558.53), 3.9x slower
    byts::alice:      60960,     61403,     62414,     63947 (mean:      62181, median:      61908, r: 1147.73), Winner
    .net::alice:     359063,    362245,    375480,    398446 (mean:     373808, median:     368862, r: 15499.55), 5.0x slower
alice_rooted (^Alice)
rure::alice_rooted:     233941,    239032,    239173,    281125 (mean:     248317, median:     239102, r: 19058.19), 1270.8x slower
byts::alice_rooted:        157,       174,       202,       369 (mean:        225, median:        188, r: 84.39), Winner
.net::alice_rooted:        131,       140,       873,     19221 (mean:       5091, median:        506, r: 8163.37), 1.7x slower
alice_rooted2 (Alice$)
rure::alice_rooted2:     199131,    202915,    263349,    690806 (mean:     339050, median:     233132, r: 204678.40), 1287.0x slower
byts::alice_rooted2:        160,       174,       189,       343 (mean:        216, median:        181, r: 73.75), Winner
.net::alice_rooted2:     269685,    269844,    270500,    275714 (mean:     271435, median:     270172, r: 2488.87), 1491.7x slower
numbers (\d+)
  rure::numbers:     519176,    520194,    520600,    612661 (mean:     543157, median:     520397, r: 40131.07), 0.6x slower
  byts::numbers:     328425,    329236,    329447,    329828 (mean:     329234, median:     329341, r: 513.00), Winner
  .net::numbers:    2997613,   3000663,   3004897,   3041233 (mean:    3011101, median:    3002780, r: 17587.67), 8.1x slower
abwords (a[^x]{20}b)
  rure::abwords:     626019,    652697,    661764,  10040087 (mean:    2995141, median:     657230, r: 4067422.26), 0.6x slower
  byts::abwords:     409849,    410525,    415127,    416420 (mean:     412980, median:     412826, r: 2840.48), Winner
  .net::abwords:    2095180,   2767433,   3340633,   3473769 (mean:    2919253, median:    3054033, r: 544792.33), 6.4x slower
email (\w+@\w+.\w+)
    rure::email:     515369,    570251,    652824,   1656428 (mean:     848718, median:     611537, r: 468891.07), 1.0x slower
    byts::email:     312546,    312772,    312994,    316517 (mean:     313707, median:     312883, r: 1629.92), Winner
    .net::email:   40924231,  41436962,  42180829,  43927518 (mean:   42117385, median:   41808895, r: 1136573.64), 132.6x slower
quotes ("[^"]+")
   rure::quotes:     726681,    759935,    762919,   1117193 (mean:     841682, median:     761427, r: 159701.07), 0.6x slower
   byts::quotes:     465612,    467048,    467328,    520313 (mean:     480075, median:     467188, r: 23240.39), Winner
   .net::quotes:    1182894,   1190590,   1248877,   1332002 (mean:    1238590, median:    1219733, r: 59660.85), 1.6x slower
quotes2 ("[^"]{0,30}[?!.]")
  rure::quotes2:     375686,    381705,    382229,    928415 (mean:     517008, median:     381967, r: 237539.42), 1.1x slower
  byts::quotes2:     181847,    182861,    184870,    185383 (mean:     183740, median:     183865, r: 1443.30), Winner
  .net::quotes2:    2490927,   2496617,   2650443,   2664148 (mean:    2575533, median:    2573530, r: 81929.90), 13.0x slower
quote_said ("[^"]+"\s+said)
rure::quote_said:     374875,    374923,    380056,    491168 (mean:     405255, median:     377489, r: 49646.27), 1.1x slower
byts::quote_said:     177926,    180075,    183324,    188163 (mean:     182372, median:     181699, r: 3856.33), Winner
.net::quote_said:    4259407,   4693886,   4754750,   6074973 (mean:    4945754, median:    4724318, r: 679361.42), 25.0x slower
section ((\*\s+){4}\*)
  rure::section:     193689,    193885,    194449,    260299 (mean:     210580, median:     194167, r: 28706.35), 45.7x slower
  byts::section:       4100,      4146,      4168,      5948 (mean:       4590, median:       4157, r: 784.14), Winner
  .net::section:     502045,    502087,    502469,    513879 (mean:     505120, median:     502278, r: 5059.71), 119.8x slower
repeated_negation ([a-q][^u-z]{13}x)
rure::repeated_negation:   23725849,  23891204,  24054032, 121792077 (mean:   48365790, median:   23972618, r: 42392845.07), 0.2x slower
byts::repeated_negation:   23320316,  23591309,  23776237,  24456051 (mean:   23785978, median:   23683773, r: 419472.83), 0.2x slower
.net::repeated_negation:   18308238,  18666819,  21197396,  21269235 (mean:   19860422, median:   19932107, r: 1378968.53), Winner
ing_suffix ([a-zA-Z]+ing)
rure::ing_suffix:     478548,    480771,    492646,    589678 (mean:     510410, median:     486708, r: 46077.74), 1.0x slower
byts::ing_suffix:     240242,    241462,    242899,    251552 (mean:     244038, median:     242180, r: 4438.55), Winner
.net::ing_suffix:   24765539,  25341318,  26766151,  27353227 (mean:   26056558, median:   26053734, r: 1044410.24), 106.6x slower
name_alt (Alice|Adventure)
 rure::name_alt:     309743,    317897,    338508,    358129 (mean:     331069, median:     328202, r: 18814.10), 4.6x slower
 byts::name_alt:      58637,     58975,     59036,     61009 (mean:      59414, median:      59005, r: 933.19), Winner
 .net::name_alt:     618271,    619061,    619085,    622007 (mean:     619606, median:     619073, r: 1424.39), 9.5x slower
name_alt2 (Alice|Hatter|Cheshire|Dinah)
rure::name_alt2:     613287,    705929,    734173,   1023932 (mean:     769330, median:     720051, r: 153645.12), 1.0x slower
byts::name_alt2:     363928,    364627,    366371,    368209 (mean:     365783, median:     365499, r: 1658.96), Winner
.net::name_alt2:    2375911,   2378334,   2502382,   2607713 (mean:    2466085, median:    2440358, r: 96446.29), 5.7x slower
name_alt3 (.{0,3}(Alice|Hatter|Cheshire|Dinah))
rure::name_alt3:     572425,    587157,    602476,   1163149 (mean:     731301, median:     594816, r: 249553.43), 0.3x slower
byts::name_alt3:     433873,    457621,    460644,    502539 (mean:     463669, median:     459132, r: 24720.47), Winner
.net::name_alt3:   94887995,  97705048,  99224139, 100344663 (mean:   98040461, median:   98464593, r: 2047009.05), 213.5x slower
nomatch_uncommon (zqj)
rure::nomatch_uncommon:     189929,    190717,    190889,    240186 (mean:     202930, median:     190803, r: 21512.66), 32.1x slower
byts::nomatch_uncommon:       5363,      5633,      5895,      9623 (mean:       6628, median:       5764, r: 1739.08), Winner
.net::nomatch_uncommon:     276949,    276986,    277067,    278794 (mean:     277449, median:     277026, r: 777.71), 47.1x slower
nomatch_common (aei)
rure::nomatch_common:     265292,    267307,    290375,    315407 (mean:     284595, median:     278841, r: 20336.35), 2.5x slower
byts::nomatch_common:      80277,     80464,     80820,     82927 (mean:      81122, median:      80642, r: 1060.21), Winner
.net::nomatch_common:     328487,    328686,    329521,    381676 (mean:     342092, median:     329103, r: 22856.84), 3.1x slower
common ((?i)the)
   rure::common:     794989,    872846,    897260,   1287945 (mean:     963260, median:     885053, r: 191223.80), 0.5x slower
   byts::common:     598904,    600095,    600805,    609538 (mean:     602335, median:     600450, r: 4213.47), Winner
   .net::common:    1516400,   1549349,   1571868,   1691908 (mean:    1582381, median:    1560608, r: 66240.65), 1.6x slower
short_lines (^.{16,20}$)
rure::short_lines:     203152,    203469,    203883,    282480 (mean:     223246, median:     203676, r: 34199.75), 807.2x slower
byts::short_lines:        233,       245,       260,       571 (mean:        327, median:        252, r: 141.05), Winner
.net::short_lines:        291,       325,       550,     11850 (mean:       3254, median:        437, r: 4963.90), 0.7x slower
dotplus (.+)
  rure::dotplus:    1020685,   1024777,   1042517,   1092363 (mean:    1045085, median:    1033647, r: 28502.54), 0.2x slower
  byts::dotplus:     831937,    833627,    834811,    834894 (mean:     833817, median:     834219, r: 1195.67), Winner
  .net::dotplus:    1433560,   1472972,   1616928,   1737061 (mean:    1565130, median:    1544950, r: 120464.79), 0.9x slower
dotplus_nl ((?s).+)
rure::dotplus_nl:     482049,    482227,    497715,    514100 (mean:     494022, median:     489971, r: 13221.56), 0.8x slower
byts::dotplus_nl:     277599,    277733,    278067,    280278 (mean:     278419, median:     277900, r: 1086.60), Winner
.net::dotplus_nl:    1561056,   1642134,   1644580,   1645318 (mean:    1623272, median:    1643357, r: 35939.75), 4.9x slower
alice_hattter (Alice.{0,25}Hatter|Hatter.{0,25}Alice)
rure::alice_hattter:     269886,    273955,   1320481,   2234540 (mean:    1024715, median:     797218, r: 819231.57), 8.9x slower
byts::alice_hattter:      79760,     80411,     81211,     84605 (mean:      81496, median:      80811, r: 1866.68), Winner
.net::alice_hattter:    2462189,   2944215,   3160557,   3213413 (mean:    2945093, median:    3052386, r: 296486.68), 36.8x slower
name_suffixes (([A-Za-z]lice|[A-Za-z]heshire)[^a-zA-Z])
rure::name_suffixes:     660516,    680393,    726359,    830451 (mean:     724429, median:     703376, r: 65704.70), 0.6x slower
byts::name_suffixes:     401378,    436962,    437661,    442324 (mean:     429581, median:     437311, r: 16413.10), Winner
.net::name_suffixes:   24839809,  24893901,  26018144,  31502366 (mean:   26813555, median:   25456022, r: 2747652.27), 57.2x slower
dotstar (.*)
  rure::dotstar:    1455614,   1456097,   1460706,   1591548 (mean:    1490991, median:    1458401, r: 58090.48), 0.1x slower
  byts::dotstar:    1269216,   1270864,   1271539,   1273704 (mean:    1271330, median:    1271201, r: 1609.79), Winner
  .net::dotstar:    2040246,   2053420,   2229392,   2240420 (mean:    2140869, median:    2141406, r: 94232.48), 0.7x slower
  ```